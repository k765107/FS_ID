Imports System.IO
Imports System.Text

Module PotentialStock

    '========================================================
    ' 潛力股資料處理
    '
    ' 目前功能：
    '
    ' 1. 讀取 SYSTEM\01\FS_ID.DAT
    ' 2. 讀取 10 組潛力股
    ' 3. 讀取每組股票代號
    ' 4. 讀取 FS_ID.DAT 裡真正的分類名稱
    ' 5. 修改分類名稱
    '
    ' 目前暫時不處理：
    '
    ' XNAME6.STK
    ' 股票代號 → 股票名稱
    '
    ' 後面再另外研究。
    '========================================================


    '========================================================
    ' 潛力股群組
    '========================================================
    Public Class PotentialGroup

        Public Property Name As String

        Public Property StockIDs As List(Of String)

        Public Overrides Function ToString() As String

            Return Name

        End Function

    End Class


    '========================================================
    ' 讀取 FS_ID.DAT
    '
    ' 取得：
    '
    ' ListBox1
    '     ↓
    ' 潛力股分類
    '
    ' ListBox2
    '     ↓
    ' 股票代號
    '========================================================
    Public Function LoadPotentialGroups(
        fileName As String) As List(Of PotentialGroup)


        Dim result As New List(Of PotentialGroup)


        If Not File.Exists(fileName) Then

            Throw New FileNotFoundException(
                "找不到 FS_ID.DAT：" &
                vbCrLf &
                fileName)

        End If


        '====================================================
        ' 讀取 Binary
        '====================================================
        Dim data() As Byte =
            File.ReadAllBytes(fileName)


        Dim searchPos As Integer = 0


        '====================================================
        ' 找所有 FS_STK
        '
        ' 01 02 tb 01
        '
        ' 每一個代表一個潛力股群組
        '====================================================
        While True


            Dim groupStart As Integer =
                FindBytes(
                    data,
                    New Byte() {
                        &H1,
                        &H2,
                        &H74,
                        &H62,
                        &H1
                    },
                    searchPos)


            If groupStart < 0 Then

                Exit While

            End If


            '================================================
            ' 取得內部檔名長度
            '================================================
            If groupStart + 5 >= data.Length Then

                Exit While

            End If


            Dim fileNameLength As Integer =
                data(groupStart + 5)


            Dim fileNameStart As Integer =
                groupStart + 6


            If fileNameStart + fileNameLength >
               data.Length Then

                Exit While

            End If


            '================================================
            ' 取得 FS_STK 名稱
            '
            ' 例如：
            '
            ' FS_STK.DAT
            ' FS_STK01.dat
            ' FS_STK02.dat
            '================================================
            Dim internalFileName As String =
                Encoding.ASCII.GetString(
                    data,
                    fileNameStart,
                    fileNameLength)


            '================================================
            ' 找下一個 FS_STK
            '
            ' 下一個 FS_STK 就是目前群組的結束
            '================================================
            Dim nextGroup As Integer =
                FindBytes(
                    data,
                    New Byte() {
                        &H1,
                        &H2,
                        &H74,
                        &H62,
                        &H1
                    },
                    fileNameStart + fileNameLength)


            Dim groupEnd As Integer


            If nextGroup >= 0 Then

                groupEnd =
                    nextGroup

            Else

                groupEnd =
                    data.Length

            End If


            '================================================
            ' 只處理 FS_STK
            '================================================
            If internalFileName.StartsWith(
                "FS_STK",
                StringComparison.OrdinalIgnoreCase) Then


                '================================================
                ' 讀取股票代號
                '================================================
                Dim stockIDs As List(Of String) =
                    ReadStockIDs(
                        data,
                        fileNameStart + fileNameLength,
                        groupEnd)


                '================================================
                ' 建立群組
                '
                ' 名稱先暫時使用內部名稱
                ' 後面再從 FS_ID.DAT 讀真正名稱
                '================================================
                result.Add(
                    New PotentialGroup With {.Name = internalFileName, .StockIDs = stockIDs})


            End If


            '================================================
            ' 繼續下一組
            '================================================
            If nextGroup >= 0 Then

                searchPos =
                    nextGroup

            Else

                Exit While

            End If


        End While


        '====================================================
        ' 讀取 FS_ID.DAT 裡真正的潛力股名稱
        '====================================================
        Dim realNames As List(Of String) =
            LoadPotentialGroupNames(fileName)


        '====================================================
        ' 將真正名稱套入群組
        '
        ' 這裡非常重要：
        '
        ' 不再使用寫死的 displayNames()
        '====================================================
        Dim count As Integer =
            Math.Min(
                result.Count,
                realNames.Count)


        For i As Integer = 0 To count - 1

            result(i).Name =
                realNames(i)

        Next


        Return result

    End Function


    '========================================================
    ' 讀取某一個 FS_STK 裡面的股票代號
    '
    ' 實際格式：
    '
    ' 01 00
    '
    ' 01 06 00632R
    ' 01 06 00664R
    ' 01 06 00665L
    ' 01 06 00673R
    '
    ' 每筆：
    '
    ' 01
    ' 長度
    ' 股票代號
    '========================================================
    Private Function ReadStockIDs(
        data() As Byte,
        startPos As Integer,
        endPos As Integer) As List(Of String)


        Dim result As New List(Of String)


        '====================================================
        ' 找 ID 欄位
        '
        ' 01 02 ID
        '====================================================
        Dim idPos As Integer =
            FindBytes(
                data,
                New Byte() {
                    &H1,
                    &H2,
                    &H49,
                    &H44
                },
                startPos)


        If idPos < 0 OrElse
           idPos >= endPos Then

            Return result

        End If


        '====================================================
        ' 找：
        '
        ' 01 00
        '
        ' ID 區域開始標記
        '====================================================
        Dim dataStart As Integer =
            FindBytes(
                data,
                New Byte() {
                    &H1,
                    &H0
                },
                idPos + 4)


        If dataStart < 0 OrElse
           dataStart >= endPos Then

            Return result

        End If


        '====================================================
        ' 真正第一筆股票：
        '
        ' 01 06 00632R
        '
        ' 所以跳過前面的：
        '
        ' 01 00
        '====================================================
        Dim pos As Integer =
            dataStart + 2


        '====================================================
        ' 逐筆讀取股票代號
        '====================================================
        While pos + 2 <= endPos


            '================================================
            ' 每筆第一個 Byte 必須是 01
            '================================================
            If data(pos) <> &H1 Then

                Exit While

            End If


            '================================================
            ' 第二個 Byte = 股票代號長度
            '================================================
            Dim codeLength As Integer =
                data(pos + 1)


            '================================================
            ' 合理長度
            '================================================
            If codeLength <= 0 OrElse
               codeLength > 32 Then

                Exit While

            End If


            '================================================
            ' 股票代號開始
            '================================================
            Dim codeStart As Integer =
                pos + 2


            Dim codeEnd As Integer =
                codeStart + codeLength


            If codeEnd > endPos Then

                Exit While

            End If


            '================================================
            ' 讀取股票代號
            '================================================
            Dim stockID As String =
                Encoding.ASCII.GetString(
                    data,
                    codeStart,
                    codeLength)


            stockID =
                stockID.Trim()


            '================================================
            ' 檢查股票代號
            '
            ' 可以：
            '
            ' 1102
            ' 0050
            ' 00632R
            ' 00665L
            ' IX0198
            '
            ' 不接受：
            '
            ' ce
            ' te
            '================================================
            Dim valid As Boolean = True


            '------------------------------------------------
            ' 長度至少 4
            '------------------------------------------------
            If stockID.Length < 4 Then

                valid = False

            End If


            '------------------------------------------------
            ' 長度最多 32
            '------------------------------------------------
            If stockID.Length > 32 Then

                valid = False

            End If


            '------------------------------------------------
            ' 至少要有一個數字
            '------------------------------------------------
            Dim hasNumber As Boolean = False


            '------------------------------------------------
            ' 檢查每個字元
            '------------------------------------------------
            For Each c As Char In stockID


                '數字
                If c >= "0"c AndAlso
                   c <= "9"c Then

                    hasNumber = True


                    '大寫英文
                ElseIf c >= "A"c AndAlso
                       c <= "Z"c Then

                    '允許


                Else

                    valid = False

                End If


            Next


            '------------------------------------------------
            ' 至少要有一個數字
            '------------------------------------------------
            If Not hasNumber Then

                valid = False

            End If


            '================================================
            ' 加入股票代號
            '================================================
            If valid Then


                If Not result.Contains(stockID) Then

                    result.Add(stockID)

                End If


            End If


            '================================================
            ' 下一筆
            '================================================
            pos =
                codeEnd


        End While


        Return result

    End Function


    '========================================================
    ' 從 FS_ID.DAT 讀取真正的潛力股名稱
    '
    ' 名稱資料格式：
    '
    ' 01 0A [Big5名稱]
    '
    ' 例如：
    '
    ' 多重潛力股
    '
    ' 01 0A
    ' A6 68 AD AB BC E7 A4 4F AA D1
    '
    '
    ' 修改後：
    '
    ' 一二三四五六七八
    '
    ' 01 10
    ' A4 40 A4 47 A4 54 A5 7C
    ' A4 AD A4 BB A4 43 A4 4B
    '========================================================
    Public Function LoadPotentialGroupNames(
        fileName As String) As List(Of String)


        Dim result As New List(Of String)


        If Not File.Exists(fileName) Then

            Return result

        End If


        Dim data() As Byte =
            File.ReadAllBytes(fileName)


        '====================================================
        ' 找名稱區域
        '
        ' 01 02 tb
        '====================================================
        Dim tbPos As Integer =
            FindBytes(
                data,
                New Byte() {
                    &H1,
                    &H2,
                    &H74,
                    &H62
                },
                0)


        If tbPos < 0 Then

            Return result

        End If


        '====================================================
        ' 找：
        '
        ' 01 01 01 0A
        '====================================================
        Dim nameArea As Integer =
            FindBytes(
                data,
                New Byte() {
                    &H1,
                    &H1,
                    &H1,
                    &HA
                },
                tbPos)


        If nameArea < 0 Then

            Return result

        End If


        '====================================================
        ' 找名稱列表開始：
        '
        ' 01 00
        '====================================================
        Dim listStart As Integer =
            FindBytes(
                data,
                New Byte() {
                    &H1,
                    &H0
                },
                nameArea + 4)


        If listStart < 0 Then

            Return result

        End If


        '====================================================
        ' 第一組名稱
        '====================================================
        Dim pos As Integer =
            listStart + 2


        Dim encoding As Encoding =
            Encoding.GetEncoding(950)


        '====================================================
        ' 最多 10 組
        '====================================================
        For i As Integer = 0 To 9


            If pos + 2 > data.Length Then

                Exit For

            End If


            '------------------------------------------------
            ' 名稱資料必須以 01 開始
            '------------------------------------------------
            If data(pos) <> &H1 Then

                Exit For

            End If


            '------------------------------------------------
            ' 第二個 Byte = 名稱 Byte 長度
            '------------------------------------------------
            Dim nameLength As Integer =
                data(pos + 1)


            If nameLength <= 0 OrElse
               nameLength > 255 Then

                Exit For

            End If


            '------------------------------------------------
            ' 名稱開始
            '------------------------------------------------
            Dim nameStart As Integer =
                pos + 2


            Dim nameEnd As Integer =
                nameStart + nameLength


            If nameEnd > data.Length Then

                Exit For

            End If


            '------------------------------------------------
            ' Big5 / CP950 解碼
            '------------------------------------------------
            Dim name As String =
                encoding.GetString(
                    data,
                    nameStart,
                    nameLength)


            name =
                name.Trim()


            result.Add(name)


            '------------------------------------------------
            ' 下一組
            '------------------------------------------------
            pos =
                nameEnd


        Next


        Return result

    End Function


    '========================================================
    ' 修改 FS_ID.DAT 中的潛力股名稱
    '
    ' groupIndex：
    '
    ' 0 = 第1組
    ' 1 = 第2組
    ' 2 = 第3組
    ' ...
    ' 6 = 第7組
    ' ...
    ' 9 = 第10組
    '
    ' 格式：
    '
    ' 01
    ' 長度
    ' Big5名稱
    '========================================================
    Public Function UpdatePotentialGroupName(
        fileName As String,
        groupIndex As Integer,
        newName As String) As Boolean


        If Not File.Exists(fileName) Then

            Throw New FileNotFoundException(
                "找不到 FS_ID.DAT：" &
                vbCrLf &
                fileName)

        End If


        If groupIndex < 0 OrElse
           groupIndex > 9 Then

            Throw New ArgumentOutOfRangeException(
                NameOf(groupIndex))

        End If


        If String.IsNullOrWhiteSpace(newName) Then

            Throw New Exception(
                "名稱不能是空白。")

        End If


        newName =
            newName.Trim()


        '====================================================
        ' Big5 / CP950
        '====================================================
        Dim encoding As Encoding =
            Encoding.GetEncoding(950)


        Dim nameBytes() As Byte =
            encoding.GetBytes(newName)


        '====================================================
        ' 名稱長度使用 1 Byte
        '====================================================
        If nameBytes.Length > 255 Then

            Throw New Exception(
                "名稱太長，Big5 編碼後超過 255 Bytes。")

        End If


        '====================================================
        ' 讀取原始檔
        '====================================================
        Dim data() As Byte =
            File.ReadAllBytes(fileName)


        '====================================================
        ' 找名稱區域
        '
        ' 01 02 tb
        '====================================================
        Dim tbPos As Integer =
            FindBytes(
                data,
                New Byte() {
                    &H1,
                    &H2,
                    &H74,
                    &H62
                },
                0)


        If tbPos < 0 Then

            Throw New Exception(
                "找不到 FS_ID.DAT 的名稱區域。")

        End If


        '====================================================
        ' 找名稱區域標記
        '
        ' 01 01 01 0A
        '====================================================
        Dim nameArea As Integer =
            FindBytes(
                data,
                New Byte() {
                    &H1,
                    &H1,
                    &H1,
                    &HA
                },
                tbPos)


        If nameArea < 0 Then

            Throw New Exception(
                "找不到潛力股名稱區域。")

        End If


        '====================================================
        ' 找名稱列表開始
        '
        ' 01 00
        '====================================================
        Dim listStart As Integer =
            FindBytes(
                data,
                New Byte() {
                    &H1,
                    &H0
                },
                nameArea + 4)


        If listStart < 0 Then

            Throw New Exception(
                "找不到潛力股名稱列表。")

        End If


        '====================================================
        ' 第一組名稱
        '====================================================
        Dim pos As Integer =
            listStart + 2


        '====================================================
        ' 找第 N 組名稱
        '====================================================
        For i As Integer = 0 To groupIndex - 1


            If pos + 2 > data.Length Then

                Throw New Exception(
                    "無法找到第 " &
                    (i + 1).ToString() &
                    " 組名稱。")

            End If


            If data(pos) <> &H1 Then

                Throw New Exception(
                    "名稱資料格式錯誤。")

            End If


            Dim oldLength As Integer =
                data(pos + 1)


            Dim nextPos As Integer =
                pos + 2 + oldLength


            If nextPos > data.Length Then

                Throw New Exception(
                    "名稱資料超出檔案範圍。")

            End If


            pos =
                nextPos


        Next


        '====================================================
        ' 確認目前名稱資料
        '====================================================
        If pos + 2 > data.Length Then

            Throw New Exception(
                "名稱資料位置超出檔案範圍。")

        End If


        If data(pos) <> &H1 Then

            Throw New Exception(
                "名稱資料格式錯誤。")

        End If


        '====================================================
        ' 舊名稱長度
        '====================================================
        Dim oldLength2 As Integer =
            data(pos + 1)


        Dim oldNameStart As Integer =
            pos + 2


        Dim oldNameEnd As Integer =
            oldNameStart + oldLength2


        If oldNameEnd > data.Length Then

            Throw New Exception(
                "原名稱資料超出檔案範圍。")

        End If


        '====================================================
        ' 建立新名稱資料
        '
        ' 01
        ' 新長度
        ' 名稱
        '====================================================
        Dim newRecord As New List(Of Byte)


        newRecord.Add(&H1)


        newRecord.Add(
            CByte(nameBytes.Length))


        For Each b As Byte In nameBytes

            newRecord.Add(b)

        Next


        '====================================================
        ' 建立新的完整檔案
        '====================================================
        Dim newData As New List(Of Byte)


        '----------------------------------------------------
        ' 舊名稱之前的資料
        '----------------------------------------------------
        For i As Integer = 0 To pos - 1

            newData.Add(
                data(i))

        Next


        '----------------------------------------------------
        ' 新名稱
        '----------------------------------------------------
        For Each b As Byte In newRecord

            newData.Add(b)

        Next


        '----------------------------------------------------
        ' 舊名稱之後的資料
        '----------------------------------------------------
        For i As Integer =
            oldNameEnd To data.Length - 1

            newData.Add(
                data(i))

        Next


        '====================================================
        ' 暫存檔
        '====================================================
        Dim tempFile As String =
            fileName & ".tmp"


        '====================================================
        ' 寫入暫存檔
        '====================================================
        File.WriteAllBytes(
            tempFile,
            newData.ToArray())


        '====================================================
        ' 替換原始檔
        '====================================================
        File.Delete(fileName)


        File.Move(
            tempFile,
            fileName)


        Return True

    End Function


    '========================================================
    ' 搜尋 Byte 陣列
    '========================================================
    Private Function FindBytes(
        source() As Byte,
        search() As Byte,
        startIndex As Integer) As Integer


        If source Is Nothing Then

            Return -1

        End If


        If search Is Nothing Then

            Return -1

        End If


        If search.Length = 0 Then

            Return -1

        End If


        If search.Length > source.Length Then

            Return -1

        End If


        If startIndex < 0 Then

            startIndex = 0

        End If


        For i As Integer =
            startIndex To source.Length - search.Length


            Dim found As Boolean =
                True


            For j As Integer =
                0 To search.Length - 1


                If source(i + j) <>
                   search(j) Then

                    found = False

                    Exit For

                End If


            Next


            If found Then

                Return i

            End If


        Next


        Return -1

    End Function



    '========================================================
    ' 刪除指定群組中的「單一股票」
    '
    ' groupIndex = 群組 Index
    ' stockIndex = ListBox2 / StockIDs 的 Index
    '
    ' ★ 只刪除一檔股票
    ' ★ 群組名稱不變
    ' ★ 其他股票不變
    ' ★ 其他群組不變
    ' ★ 股票數量 - 1
    '========================================================
    Public Function DeletePotentialStock(
        fileName As String,
        groupIndex As Integer,
        stockIndex As Integer) As Boolean

        Try

            If Not File.Exists(fileName) Then
                Return False
            End If

            If groupIndex < 0 OrElse groupIndex > 9 Then
                Return False
            End If

            If stockIndex < 0 Then
                Return False
            End If


            '====================================================
            ' 讀取 FS_ID.DAT
            '====================================================
            Dim data() As Byte =
                File.ReadAllBytes(fileName)


            '====================================================
            ' 找出 10 個 FS_STK 群組
            '
            ' 01 02 74 62 01
            '====================================================
            Dim headers As New List(Of Integer)

            Dim searchPos As Integer = 0

            Dim groupMarker() As Byte = {
                &H1,
                &H2,
                &H74,
                &H62,
                &H1
            }


            Do

                Dim pos As Integer =
                    FindBytes(
                        data,
                        groupMarker,
                        searchPos)

                If pos < 0 Then
                    Exit Do
                End If

                headers.Add(pos)

                searchPos =
                    pos + groupMarker.Length

            Loop


            '====================================================
            ' 必須至少有 10 組
            '====================================================
            If headers.Count < 10 Then
                Return False
            End If


            '====================================================
            ' 取得指定群組
            '====================================================
            Dim groupStart As Integer =
                headers(groupIndex)


            '====================================================
            ' 取得群組結束位置
            '====================================================
            Dim groupEnd As Integer

            If groupIndex < 9 Then

                groupEnd =
                    headers(groupIndex + 1)

            Else

                groupEnd =
                    data.Length

            End If


            '====================================================
            ' 找股票數量
            '
            ' 01 01 01 [數量]
            '====================================================
            Dim countMarker() As Byte = {
                &H1,
                &H1,
                &H1
            }


            Dim countPos As Integer =
                FindBytesRange(
                    data,
                    countMarker,
                    groupStart,
                    groupEnd)


            If countPos < 0 Then
                Return False
            End If


            Dim oldCount As Integer =
                data(countPos + 3)


            '====================================================
            ' 確認股票 Index
            '====================================================
            If stockIndex >= oldCount Then
                Return False
            End If


            If oldCount <= 0 Then
                Return False
            End If


            '====================================================
            ' 找股票資料開始
            '
            ' 01 00
            '====================================================
            Dim stockStartMarker() As Byte = {
                &H1,
                &H0
            }


            Dim stockStart As Integer =
                FindBytesRange(
                    data,
                    stockStartMarker,
                    countPos,
                    groupEnd)


            If stockStart < 0 Then
                Return False
            End If


            stockStart += 2


            '====================================================
            ' 找股票資料結束
            '
            ' 01 02 63 65
            '====================================================
            Dim stockEndMarker() As Byte = {
                &H1,
                &H2,
                &H63,
                &H65
            }


            Dim stockEnd As Integer =
                FindBytesRange(
                    data,
                    stockEndMarker,
                    stockStart,
                    groupEnd)


            If stockEnd < 0 Then
                Return False
            End If


            '====================================================
            ' 找第 stockIndex 檔股票
            '
            ' 每筆格式：
            '
            ' 01
            ' 長度
            ' 股票代號
            '
            ' 例如：
            '
            ' 01 06 00632R
            '====================================================
            Dim currentPos As Integer =
                stockStart


            Dim targetStart As Integer = -1
            Dim targetEnd As Integer = -1


            For i As Integer = 0 To oldCount - 1

                If currentPos + 2 > stockEnd Then
                    Return False
                End If


                If data(currentPos) <> &H1 Then
                    Return False
                End If


                Dim stockLength As Integer =
                    data(currentPos + 1)


                Dim recordEnd As Integer =
                    currentPos + 2 + stockLength


                If recordEnd > stockEnd Then
                    Return False
                End If


                '--------------------------------------------
                ' 找到指定股票
                '--------------------------------------------
                If i = stockIndex Then

                    targetStart =
                        currentPos

                    targetEnd =
                        recordEnd

                    Exit For

                End If


                currentPos =
                    recordEnd

            Next


            '====================================================
            ' 沒找到
            '====================================================
            If targetStart < 0 OrElse
               targetEnd <= targetStart Then

                Return False

            End If


            '====================================================
            ' 股票數量 - 1
            '====================================================
            data(countPos + 3) =
                CByte(oldCount - 1)


            '====================================================
            ' 建立新的檔案
            '
            ' 只移除 targetStart ～ targetEnd
            '====================================================
            Dim newData As New List(Of Byte)


            '----------------------------------------------------
            ' 被刪除股票之前
            '----------------------------------------------------
            For i As Integer = 0 To targetStart - 1

                newData.Add(
                    data(i))

            Next


            '----------------------------------------------------
            ' 被刪除股票之後
            '----------------------------------------------------
            For i As Integer =
                targetEnd To data.Length - 1

                newData.Add(
                    data(i))

            Next


            '====================================================
            ' 暫存檔
            '====================================================
            Dim tempFile As String =
                fileName & ".tmp"


            File.WriteAllBytes(
                tempFile,
                newData.ToArray())


            '====================================================
            ' 替換原始檔
            '====================================================
            File.Delete(fileName)

            File.Move(
                tempFile,
                fileName)


            Return True


        Catch

            Return False

        End Try

    End Function


    '========================================================
    ' FindBytesRange
    '
    ' 在指定範圍內搜尋 Byte 陣列
    '
    ' startIndex = 開始位置
    ' endPos     = 結束位置
    '
    ' 找到 → 回傳位置
    ' 找不到 → 回傳 -1
    '========================================================
    Private Function FindBytesRange(
        source() As Byte,
        search() As Byte,
        startIndex As Integer,
        endPos As Integer) As Integer

        If source Is Nothing Then
            Return -1
        End If

        If search Is Nothing Then
            Return -1
        End If

        If search.Length = 0 Then
            Return -1
        End If

        If search.Length > source.Length Then
            Return -1
        End If

        If startIndex < 0 Then
            startIndex = 0
        End If

        If endPos > source.Length Then
            endPos = source.Length
        End If

        If endPos < startIndex Then
            Return -1
        End If

        If endPos - startIndex < search.Length Then
            Return -1
        End If


        For i As Integer =
            startIndex To endPos - search.Length

            Dim found As Boolean = True

            For j As Integer =
                0 To search.Length - 1

                If source(i + j) <> search(j) Then

                    found = False
                    Exit For

                End If

            Next


            If found Then
                Return i
            End If

        Next


        Return -1

    End Function

    '========================================================
    ' 清除指定群組的全部股票
    '
    ' ★ 重要：
    ' 不直接修改整個股票資料區
    '
    ' 而是重複使用：
    '
    ' DeletePotentialStock()
    '
    ' 已經驗證成功的「刪除單一個股」功能。
    '
    ' 這樣可以確保：
    '
    ' ★ 每次只刪一支
    ' ★ 群組名稱不動
    ' ★ 其他群組不動
    ' ★ 後面的名稱資料不動
    '========================================================
    Public Function DeletePotentialGroupStocks(
    fileName As String,
    groupIndex As Integer) As Boolean

        Try

            '----------------------------------------------------
            ' 基本檢查
            '----------------------------------------------------
            If Not File.Exists(fileName) Then
                Return False
            End If

            If groupIndex < 0 OrElse
           groupIndex > 9 Then

                Return False

            End If


            '----------------------------------------------------
            ' 先讀取目前群組
            '----------------------------------------------------
            Dim groups As List(Of PotentialGroup) =
            LoadPotentialGroups(fileName)


            If groups Is Nothing Then
                Return False
            End If


            If groupIndex >= groups.Count Then
                Return False
            End If


            Dim group As PotentialGroup =
            groups(groupIndex)


            '----------------------------------------------------
            ' 沒有股票
            '----------------------------------------------------
            If group.StockIDs Is Nothing OrElse
           group.StockIDs.Count = 0 Then

                Return True

            End If


            '----------------------------------------------------
            ' 記錄股票數量
            '----------------------------------------------------
            Dim stockCount As Integer =
            group.StockIDs.Count


            '----------------------------------------------------
            ' ★ 從最後一支開始刪
            '
            ' 為什麼從最後開始？
            '
            ' 因為刪除前面的股票後，
            ' 後面的 Index 會往前移。
            '
            ' 從最後往前刪，
            ' Index 不會影響前面的股票。
            '----------------------------------------------------
            For i As Integer =
            stockCount - 1 To 0 Step -1


                '------------------------------------------------
                ' 使用已經成功測試過的
                ' 「刪除單一個股」
                '------------------------------------------------
                If Not DeletePotentialStock(
                fileName,
                groupIndex,
                i) Then

                    Return False

                End If


            Next


            '----------------------------------------------------
            ' 全部成功
            '----------------------------------------------------
            Return True


        Catch ex As Exception

            Return False

        End Try

    End Function

End Module