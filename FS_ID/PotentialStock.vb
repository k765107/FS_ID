Imports System.IO
Imports System.Text

Module PotentialStock

    '========================================================
    ' 潛力股資料
    '
    ' FS_ID.DAT 的實際結構：
    '
    ' 01 02 tb 01 [檔名長度] [檔名]
    '
    ' 01 02 cb 01 06 STRING 01 02 ID 01 00
    '
    ' 01 [長度] [股票代碼]
    ' 01 [長度] [股票代碼]
    ' 01 [長度] [股票代碼]
    '
    ' 每一組 FS_STK 都是一個潛力股分類。
    '
    ' 目前只處理：
    ' FS_ID.DAT → 潛力股分類 → 股票代碼
    '
    ' 不讀 XNAME6.STK
    ' 不修改 FS_ID.DAT
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


        '====================================================
        ' FS_STK 群組名稱
        '====================================================
        Dim displayNames() As String = {"4/30連30在100下", "11/17,100-500,20", "1/22週出現黃色", "5/7,100-500", "5/12突破綠市", "5/12突破綠櫃", "多重潛力股", "AA突破", "AA突破櫃", "站上週100M"}


        Dim searchPos As Integer = 0


        '====================================================
        ' 找所有：
        '
        ' 01 02 tb 01
        '
        ' 每一個就是 FS_STK 群組
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
            ' 取得檔名長度
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


            Dim internalFileName As String =
                Encoding.ASCII.GetString(
                    data,
                    fileNameStart,
                    fileNameLength)


            '================================================
            ' 找下一個 FS_STK
            '
            ' 這就是目前群組的結束位置
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

                groupEnd = nextGroup

            Else

                groupEnd = data.Length

            End If


            '================================================
            ' 只處理 FS_STK
            '================================================
            If internalFileName.StartsWith(
                "FS_STK",
                StringComparison.OrdinalIgnoreCase) Then


                Dim ids As List(Of String) =
                    ReadStockIDs(
                        data,
                        fileNameStart + fileNameLength,
                        groupEnd)


                '================================================
                ' 取得 FS_STK 編號
                '================================================
                Dim groupNumber As Integer =
                    GetGroupNumber(
                        internalFileName)


                Dim groupName As String


                If groupNumber >= 0 AndAlso
                   groupNumber < displayNames.Length Then

                    groupName =
                        displayNames(groupNumber)

                Else

                    groupName =
                        internalFileName

                End If


                '================================================
                ' 建立群組
                '================================================
                result.Add(
                    New PotentialGroup With {.Name = groupName, .StockIDs = ids})


            End If


            '================================================
            ' 繼續找下一組
            '================================================
            If nextGroup >= 0 Then

                searchPos =
                    nextGroup

            Else

                Exit While

            End If


        End While


        Return result

    End Function


    '========================================================
    ' 讀取某一個 FS_STK 裡面的股票代碼
    '
    ' 實際格式：
    '
    ' 01 06 30 30 36 33 32 52
    ' ↑  ↑
    ' │  └── 長度 = 6
    ' └───── 欄位標記
    '
    ' 不再猜股票代碼格式。
    ' 直接按照 Length 讀取。
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
        ' 找到：
        '
        ' 01 00
        '
        ' 這裡只是 ID 區塊的標記
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
        ' 真正第一筆股票從 01 06 開始
        '====================================================
        Dim pos As Integer =
        dataStart + 2


        '====================================================
        ' 逐筆讀取
        '====================================================
        While pos + 2 <= endPos


            '------------------------------------------------
            ' 每筆第一個 Byte 必須是 01
            '------------------------------------------------
            If data(pos) <> &H1 Then

                Exit While

            End If


            '------------------------------------------------
            ' 第二個 Byte = 股票代碼長度
            '------------------------------------------------
            Dim codeLength As Integer =
            data(pos + 1)


            '------------------------------------------------
            ' 長度不合理就停止
            '------------------------------------------------
            If codeLength <= 0 OrElse
           codeLength > 32 Then

                Exit While

            End If


            '------------------------------------------------
            ' 股票代碼開始
            '------------------------------------------------
            Dim codeStart As Integer =
            pos + 2


            Dim codeEnd As Integer =
            codeStart + codeLength


            If codeEnd > endPos Then

                Exit While

            End If


            '------------------------------------------------
            ' 讀取股票代碼
            '------------------------------------------------
            Dim stockID As String =
            Encoding.ASCII.GetString(
                data,
                codeStart,
                codeLength)


            stockID =
            stockID.Trim()


            '================================================
            ' 檢查股票代碼
            '
            ' 例如：
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
            ' 檢查每一個字元
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
            ' 必須至少有一個數字
            '------------------------------------------------
            If Not hasNumber Then

                valid = False

            End If


            '================================================
            ' 加入股票代碼
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
    ' FS_STK → 編號
    '
    ' FS_STK.DAT    = 0
    ' FS_STK01.dat  = 1
    ' ...
    ' FS_STK09.dat  = 9
    '========================================================
    Private Function GetGroupNumber(
        fileName As String) As Integer


        Dim name As String =
            Path.GetFileNameWithoutExtension(
                fileName)


        If name.Equals(
            "FS_STK",
            StringComparison.OrdinalIgnoreCase) Then

            Return 0

        End If


        If name.StartsWith(
            "FS_STK",
            StringComparison.OrdinalIgnoreCase) Then


            Dim suffix As String =
                name.Substring(6)


            Dim number As Integer


            If Integer.TryParse(
                suffix,
                number) Then

                Return number

            End If

        End If


        Return -1

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


        If startIndex < 0 Then
            startIndex = 0
        End If


        If search.Length >
           source.Length Then

            Return -1

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

End Module