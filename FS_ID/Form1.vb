Imports System.IO


Public Class Form1

    '========================================================
    ' 潛力股資料
    '========================================================
    Private potentialGroups As New List(Of PotentialGroup)
    Private fsIDFile As String
    Private xnameFile As String

    Private backupTime As TimeSpan
    Private lastBackupDate As DateTime = DateTime.MinValue
    Private iniFile As String = Path.Combine(Application.StartupPath, "FSSETUP.INI")

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim folder As String =
       Application.StartupPath


        fsIDFile =
            Path.Combine(
                folder,
                "SYSTEM\01\FS_ID.DAT")


        xnameFile =
            Path.Combine(
                folder,
                "XNAME6.STK")


        '====================================================
        ' 檢查 FS_ID.DAT
        '====================================================
        If Not File.Exists(fsIDFile) Then

            MessageBox.Show(
                "找不到 FS_ID.DAT：" &
                vbCrLf & vbCrLf &
                fsIDFile,
                "錯誤",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

            Return

        End If


        '====================================================
        ' 檢查 XNAME6.STK
        '====================================================
        If Not File.Exists(xnameFile) Then

            MessageBox.Show(
                "找不到 XNAME6.STK：" &
                vbCrLf & vbCrLf &
                xnameFile,
                "錯誤",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

            Return

        End If


        '====================================================
        ' 開始讀取 FS_ID.DAT
        '====================================================
        Try

            potentialGroups =
                LoadPotentialGroups(fsIDFile)


            '================================================
            ' 清空 ListBox1
            '================================================
            ListBox1.Items.Clear()


            '================================================
            ' 將潛力股分類放入 ListBox1
            '================================================
            For Each group As PotentialGroup _
                In potentialGroups

                ListBox1.Items.Add(group)

            Next


            '================================================
            ' 如果有資料，自動選第一組
            '================================================
            If ListBox1.Items.Count > 0 Then

                ListBox1.SelectedIndex = 0

            End If


        Catch ex As Exception

            MessageBox.Show(
                "讀取 FS_ID.DAT 發生錯誤：" &
                vbCrLf & vbCrLf &
                ex.Message,
                "錯誤",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try











        Dim iniFile As String =
        Path.Combine(Application.StartupPath, "FSSETUP.INI")

        '如果 FSSETUP.INI 不存在，建立預設時間 23:00
        If Not File.Exists(iniFile) Then

            File.WriteAllLines(iniFile, {
            "[Backup]",
            "Time=23:59"
        })

        End If

        '讀取 FSSETUP.INI
        Dim lines() As String = File.ReadAllLines(iniFile)

        For Each line As String In lines

            If line.StartsWith("Time=") Then

                Dim timeText As String = line.Substring(5)

                Dim savedTime As TimeSpan

                If TimeSpan.TryParse(timeText, savedTime) Then

                    '設定 DateTimePicker
                    dtBackupTime.Value =
                    Date.Today.Add(savedTime)

                    '設定程式目前使用的時間
                    backupTime = savedTime

                End If

                Exit For

            End If

        Next

        '載入備份清單
        LoadBackupList()
        backupTime = dtBackupTime.Value.TimeOfDay
    End Sub

    Private Sub timerBackup_Tick(sender As Object, e As EventArgs) Handles timerBackup.Tick

        Dim now As DateTime = DateTime.Now

        '目前時間是否已經到達設定的備份時間
        If now.TimeOfDay >= backupTime Then

            '今天還沒有備份
            If lastBackupDate.Date <> now.Date Then

                BackupFile()

                '記錄今天已經備份
                lastBackupDate = now.Date

            End If

        End If

    End Sub

    Private Sub LoadBackupList()

        '========================================
        ' 備份資料夾
        '========================================
        Dim backupFolder As String = "D:\FS_BAK"

        '先清除 ListBox 原本的內容
        lstBackup.Items.Clear()

        '========================================
        ' 確認備份資料夾是否存在
        '========================================
        If Not Directory.Exists(backupFolder) Then

            lstBackup.Items.Add("目前沒有備份資料")

            Exit Sub

        End If

        '========================================
        ' 找出所有 FS_ID.DAT 備份檔
        '========================================
        Dim files() As String =
        Directory.GetFiles(backupFolder, "*FS_ID.DAT")

        '========================================
        ' 排序
        '
        ' 檔名本身就是：
        '
        ' 20260908FS_ID.DAT
        ' 20260907FS_ID.DAT
        ' 20260906FS_ID.DAT
        '
        ' 因為日期是 yyyyMMdd，
        ' 所以按照檔名字串由大到小排序，
        ' 就會變成最新日期在最前面。
        '========================================
        Array.Sort(files)
        Array.Reverse(files)

        '========================================
        ' 一個一個處理
        '========================================
        For Each file As String In files

            '取得檔案名稱
            Dim fileName As String =
            Path.GetFileName(file)

            '========================================
            ' 取出前 8 碼日期
            '========================================
            If fileName.Length >= 17 Then

                Dim dateText As String =
                fileName.Substring(0, 8)

                Dim backupDate As DateTime

                '========================================
                ' 確認前 8 碼是不是日期
                '========================================
                If DateTime.TryParseExact(
                dateText,
                "yyyyMMdd",
                Nothing,
                Globalization.DateTimeStyles.None,
                backupDate) Then

                    '========================================
                    ' 顯示：
                    '
                    ' 2026年9月8日
                    '========================================
                    Dim displayDate As String =
                    backupDate.ToString("yyyy年M月d日")

                    lstBackup.Items.Add(displayDate)

                End If

            End If

        Next

    End Sub






    Private Sub lstBackup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstBackup.SelectedIndexChanged
        btnRestore.Enabled = (lstBackup.SelectedIndex >= 0)
    End Sub

    Private Sub btnSaveTime_Click(sender As Object, e As EventArgs) Handles btnSaveTime.Click

        Dim iniFile As String =
        Path.Combine(Application.StartupPath, "FSSETUP.INI")

        Dim backupTimeText As String =
        dtBackupTime.Value.ToString("HH:mm")

        Try

            '寫入 FSSETUP.INI
            File.WriteAllLines(iniFile, {
            "[Backup]",
            "Time=" & backupTimeText
        })

            '同步更新目前程式使用的備份時間
            backupTime = dtBackupTime.Value.TimeOfDay

            MessageBox.Show(
            "每天自動備份時間已設定為：" &
            backupTimeText,
            "設定完成",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

        Catch ex As Exception

            MessageBox.Show(
            "設定儲存失敗！" & vbCrLf & vbCrLf &
            ex.Message,
            "錯誤",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)

        End Try

    End Sub




    Private Sub BackupFile()

        Try

            '========================================
            ' 程式所在資料夾
            ' 例如：
            ' D:\XGVS
            '========================================
            Dim programFolder As String = Application.StartupPath

            '========================================
            ' 原始檔案
            '========================================
            Dim sourceFile As String =
                Path.Combine(programFolder, "SYSTEM\01\FS_ID.DAT")

            '========================================
            ' 備份資料夾
            ' 程式所在資料夾的上一層
            '
            ' D:\XGVS
            '     ↓
            ' D:\
            '     ↓
            ' D:\FS_BAK
            '========================================
            Dim parentFolder As DirectoryInfo =
                Directory.GetParent(programFolder)

            Dim backupFolder As String =
                Path.Combine(parentFolder.FullName, "FS_BAK")

            '========================================
            ' 確認原始檔案存在
            '========================================
            If Not File.Exists(sourceFile) Then

                MessageBox.Show(
                    "找不到原始檔案：" & vbCrLf &
                    sourceFile,
                    "備份失敗",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error)

                Exit Sub

            End If

            '========================================
            ' 如果備份資料夾不存在，就建立
            '========================================
            If Not Directory.Exists(backupFolder) Then
                Directory.CreateDirectory(backupFolder)
            End If

            '========================================
            ' 取得今天日期
            '
            ' 例如：
            ' 2026/09/08
            '
            ' 變成：
            ' 20260908
            '========================================
            Dim today As String =
                DateTime.Now.ToString("yyyyMMdd")

            '========================================
            ' 建立備份檔名
            '
            ' 20260908FS_ID.DAT
            '========================================
            Dim backupFile As String =
                Path.Combine(
                    backupFolder,
                    today & "FS_ID.DAT")

            '========================================
            ' 執行備份
            '
            ' True = 如果今天已經有備份，就覆蓋
            '========================================
            File.Copy(
                sourceFile,
                backupFile,
                True)

            '========================================
            ' 顯示成功訊息
            '========================================
            lstBackup.Items.Insert(
                0,
                DateTime.Now.ToString("yyyy年M月d日"))

            MessageBox.Show(
                "備份完成！" & vbCrLf & vbCrLf &
                backupFile,
                "備份成功",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

        Catch ex As Exception

            MessageBox.Show(
                "備份失敗！" & vbCrLf & vbCrLf &
                ex.Message,
                "錯誤",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btnBackupNow_Click(sender As Object, e As EventArgs) Handles btnBackupNow.Click
        BackupFile()
    End Sub

    Private Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click

        If lstBackup.SelectedIndex < 0 Then
            MessageBox.Show(
            "請先選擇要還原的日期。",
            "提示",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning)
            Exit Sub
        End If

        Try

            '========================================
            ' 取得選取的日期
            ' 例如：
            ' 2026年9月4日
            '========================================
            Dim selectedText As String =
            lstBackup.SelectedItem.ToString()

            Dim restoreDate As DateTime

            If Not DateTime.TryParseExact(
            selectedText,
            "yyyy年M月d日",
            Nothing,
            Globalization.DateTimeStyles.None,
            restoreDate) Then

                MessageBox.Show(
                "無法辨識選取的備份日期。",
                "還原失敗",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

                Exit Sub

            End If

            '========================================
            ' 程式所在資料夾
            '========================================
            Dim programFolder As String =
            Application.StartupPath

            '========================================
            ' 原始檔案
            ' D:\XGVS\SYSTEM\01\FS_ID.DAT
            '========================================
            Dim sourceFile As String =
            Path.Combine(
                programFolder,
                "SYSTEM\01\FS_ID.DAT")

            '========================================
            ' 備份資料夾
            ' D:\FS_BAK
            '========================================
            Dim parentFolder As DirectoryInfo =
            Directory.GetParent(programFolder)

            Dim backupFolder As String =
            Path.Combine(
                parentFolder.FullName,
                "FS_BAK")

            '========================================
            ' 找到選取的備份檔
            ' 例如：
            ' D:\FS_BAK\20260904FS_ID.DAT
            '========================================
            Dim backupFile As String =
            Path.Combine(
                backupFolder,
                restoreDate.ToString("yyyyMMdd") &
                "FS_ID.DAT")

            '========================================
            ' 確認備份檔存在
            '========================================
            If Not File.Exists(backupFile) Then

                MessageBox.Show(
                "找不到備份檔案：" & vbCrLf & vbCrLf &
                backupFile,
                "還原失敗",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

                Exit Sub

            End If

            '========================================
            ' 還原前再次確認
            '========================================
            Dim result As DialogResult =
            MessageBox.Show(
                "確定要還原以下備份嗎？" & vbCrLf & vbCrLf &
                restoreDate.ToString("yyyy年M月d日") &
                vbCrLf & vbCrLf &
                "還原後會覆蓋目前的 FS_ID.DAT。",
                "確認還原",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning)

            If result <> DialogResult.Yes Then
                Exit Sub
            End If

            '========================================
            ' 執行還原
            '========================================
            File.Copy(
            backupFile,
            sourceFile,
            True)

            MessageBox.Show(
            "還原完成！" & vbCrLf & vbCrLf &
            "備份日期：" &
            restoreDate.ToString("yyyy年M月d日") &
            vbCrLf & vbCrLf &
            "已還原至：" & vbCrLf &
            sourceFile,
            "還原成功",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

        Catch ex As Exception

            MessageBox.Show(
            "還原失敗！" & vbCrLf & vbCrLf &
            ex.Message,
            "錯誤",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(
    sender As Object,
    e As EventArgs
) Handles ListBox1.SelectedIndexChanged


        '====================================================
        ' 清空右邊股票代號
        '====================================================
        ListBox2.Items.Clear()


        If ListBox1.SelectedIndex < 0 Then

            txtGroupName.Text = ""

            Return

        End If


        '====================================================
        ' 取得目前選取的潛力股群組
        '====================================================
        Dim group As PotentialGroup =
        TryCast(
            ListBox1.SelectedItem,
            PotentialGroup)


        If group Is Nothing Then

            txtGroupName.Text = ""

            Return

        End If


        '====================================================
        ' TextBox 顯示目前群組名稱
        '====================================================
        txtGroupName.Text =
        group.Name


        '====================================================
        ' ListBox2 目前只顯示股票代號
        '====================================================
        For Each stockID As String In group.StockIDs

            Dim stockName As String =
        FindStockNameTest(
            xnameFile,
            stockID)

            '====================================================
            ' XNAME6.STK 找不到股票名稱
            '
            ' 代表這筆可能是舊股票 / 無效代號
            ' 不顯示在 ListBox2
            '
            ' 例如：
            '
            ' 2867 → 找不到股名 → 不顯示
            '====================================================
            If String.IsNullOrWhiteSpace(stockName) Then
                Continue For
            End If


            Dim stockInfo As New StockInfo With {
        .ID = stockID,
        .Name = stockName
    }

            ListBox2.Items.Add(stockInfo)

        Next


    End Sub

    Private Sub btnSaveGroupName_Click(sender As Object, e As EventArgs) Handles btnSaveGroupName.Click

        ' 沒有選取群組
        If ListBox1.SelectedIndex < 0 Then
            MessageBox.Show("請先選擇一個群組！")
            Return
        End If

        ' 取得目前輸入的名稱
        Dim newName As String = txtGroupName.Text.Trim()

        ' 如果名稱是空白
        ' 自動改成「多重潛力股」
        If String.IsNullOrWhiteSpace(newName) Then
            newName = "多重潛力股"
        End If

        ' 寫回 FS_ID.DAT
        If UpdatePotentialGroupName(
        fsIDFile,
        ListBox1.SelectedIndex,
        newName) Then

            ' 更新目前記憶中的群組名稱
            Dim group As PotentialGroup =
            DirectCast(ListBox1.Items(ListBox1.SelectedIndex), PotentialGroup)

            group.Name = newName

            ' TextBox 同步顯示
            txtGroupName.Text = newName

            ' ListBox1 同步刷新顯示
            ListBox1.Items(ListBox1.SelectedIndex) = group

            MessageBox.Show(
            "名稱修改完成！" & vbCrLf &
            "名稱：" & newName,
            "完成",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

        Else
            MessageBox.Show(
            "名稱修改失敗！",
            "錯誤",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)
        End If

    End Sub

    '========================================================
    ' 刪除目前選取的單一股票
    '========================================================
    Private Sub btnDeleteStock_Click(
    sender As Object,
    e As EventArgs
) Handles btnDeleteStock.Click


        '====================================================
        ' 必須先選擇群組
        '====================================================
        If ListBox1.SelectedIndex < 0 Then

            MessageBox.Show(
            "請先選擇一個群組！",
            "提示",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning)

            Return

        End If


        '====================================================
        ' 必須先選擇股票
        '====================================================
        If ListBox2.SelectedIndex < 0 Then

            MessageBox.Show(
            "請先選擇要刪除的股票！",
            "提示",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning)

            Return

        End If


        '====================================================
        ' 取得群組 Index
        '====================================================
        Dim groupIndex As Integer =
        ListBox1.SelectedIndex


        '====================================================
        ' 取得股票 Index
        '====================================================
        Dim stockIndex As Integer =
        ListBox2.SelectedIndex


        '====================================================
        ' 確認資料存在
        '====================================================
        If groupIndex >= potentialGroups.Count Then
            Return
        End If


        Dim group As PotentialGroup =
        potentialGroups(groupIndex)


        If stockIndex >= group.StockIDs.Count Then
            Return
        End If


        '====================================================
        ' 取得股票代號
        '====================================================
        Dim stockID As String =
        group.StockIDs(stockIndex)


        '====================================================
        ' 確認刪除
        '====================================================
        Dim result As DialogResult =
        MessageBox.Show(
            "確定要刪除這一檔股票嗎？" &
            vbCrLf & vbCrLf &
            "群組：" &
            ListBox1.Items(groupIndex).ToString() &
            vbCrLf &
            "股票代號：" &
            stockID &
            vbCrLf & vbCrLf &
            "⚠ 這個動作會直接修改 FS_ID.DAT。",
            "確認刪除個股",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning)


        If result <> DialogResult.Yes Then
            Return
        End If


        '====================================================
        ' 執行刪除
        '====================================================
        Try

            Dim success As Boolean =
            DeletePotentialStock(
                fsIDFile,
                groupIndex,
                stockIndex)


            If Not success Then

                MessageBox.Show(
                "刪除股票失敗！" &
                vbCrLf & vbCrLf &
                "FS_ID.DAT 沒有被更新。",
                "錯誤",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

                Return

            End If


            '================================================
            ' 更新記憶體
            '================================================
            group.StockIDs.RemoveAt(stockIndex)


            '================================================
            ' 更新 ListBox2
            '================================================
            ListBox2.Items.RemoveAt(stockIndex)


            '================================================
            ' 如果刪除後還有股票
            ' 就選擇適當的一筆
            '================================================
            If ListBox2.Items.Count > 0 Then

                If stockIndex >= ListBox2.Items.Count Then

                    ListBox2.SelectedIndex =
                    ListBox2.Items.Count - 1

                Else

                    ListBox2.SelectedIndex =
                    stockIndex

                End If

            End If


            '================================================
            ' 完成
            '================================================
            MessageBox.Show(
            "刪除完成！" &
            vbCrLf & vbCrLf &
            "群組：" &
            ListBox1.Items(groupIndex).ToString() &
            vbCrLf &
            "股票：" &
            stockID,
            "完成",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)


        Catch ex As Exception

            MessageBox.Show(
            "刪除股票時發生錯誤！" &
            vbCrLf & vbCrLf &
            ex.Message,
            "錯誤",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)

        End Try

    End Sub

    '========================================================
    ' 清除目前群組的全部股票
    '========================================================
    Private Sub btnClearGroupStocks_Click(
    sender As Object,
    e As EventArgs) Handles btnClearGroupStocks.Click

        '----------------------------------------------------
        ' 沒有選擇群組
        '----------------------------------------------------
        If ListBox1.SelectedIndex < 0 Then

            MessageBox.Show(
            "請先選擇要清除的群組！",
            "提示",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning)

            Return

        End If


        '----------------------------------------------------
        ' 取得群組 Index
        '----------------------------------------------------
        Dim groupIndex As Integer =
        ListBox1.SelectedIndex


        '----------------------------------------------------
        ' 確認 Index 有效
        '----------------------------------------------------
        If groupIndex >= potentialGroups.Count Then

            MessageBox.Show(
            "找不到指定的群組資料！",
            "錯誤",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)

            Return

        End If


        '----------------------------------------------------
        ' 取得目前群組
        '----------------------------------------------------
        Dim group As PotentialGroup =
        potentialGroups(groupIndex)


        '----------------------------------------------------
        ' 取得群組名稱
        '
        ' ListBox1 目前放的是名稱文字
        '----------------------------------------------------
        Dim groupName As String =
        ListBox1.Items(groupIndex).ToString()


        '----------------------------------------------------
        ' 如果本來就沒有股票
        '----------------------------------------------------
        If group.StockIDs.Count = 0 Then

            MessageBox.Show(
            "「" & groupName & "」目前沒有股票。",
            "提示",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

            Return

        End If


        '----------------------------------------------------
        ' 確認
        '----------------------------------------------------
        Dim result As DialogResult =
        MessageBox.Show(
            "確定要清除「" &
            groupName &
            "」裡的全部股票嗎？" &
            vbCrLf &
            vbCrLf &
            "股票數量：" &
            group.StockIDs.Count &
            " 支" &
            vbCrLf &
            vbCrLf &
            "群組名稱會保留。" &
            vbCrLf &
            "只會清除這一組的股票。",
            "清除群組股票",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)


        If result <> DialogResult.Yes Then
            Return
        End If


        '----------------------------------------------------
        ' 執行清除
        '----------------------------------------------------
        If DeletePotentialGroupStocks(
        fsIDFile,
        groupIndex) Then


            '------------------------------------------------
            ' 更新記憶體資料
            '------------------------------------------------
            group.StockIDs.Clear()


            '------------------------------------------------
            ' 清空右側 ListBox2
            '------------------------------------------------
            ListBox2.Items.Clear()


            '------------------------------------------------
            ' 左側 ListBox1 不動
            ' 群組名稱保留
            '------------------------------------------------
            ListBox1.SelectedIndex =
            groupIndex


            '------------------------------------------------
            ' TextBox 群組名稱保留
            '------------------------------------------------
            txtGroupName.Text =
            groupName


            '------------------------------------------------
            ' 完成訊息
            '------------------------------------------------
            MessageBox.Show(
            "清除完成！" &
            vbCrLf &
            vbCrLf &
            "群組：" &
            groupName &
            vbCrLf &
            "原本股票：" &
            group.StockIDs.Count &
            vbCrLf &
            vbCrLf &
            "全部股票已清除。" &
            vbCrLf &
            "群組名稱保留不變。",
            "完成",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)


        Else

            MessageBox.Show(
            "清除失敗！" &
            vbCrLf &
            vbCrLf &
            "FS_ID.DAT 沒有被修改。",
            "錯誤",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)

        End If

    End Sub


    Private Sub txtTestStockID_TextChanged(
    sender As Object,
    e As EventArgs
) Handles txtTestStockID.TextChanged

        Dim stockID As String =
            txtTestStockID.Text.Trim()

        If stockID = "" Then

            lblTestStockName.Text =
                "尚未輸入"

            Return

        End If


        Dim stockName As String =
            FindStockNameTest(
                xnameFile,
                stockID)


        If stockName = "" Then

            lblTestStockName.Text =
                "找不到股名"

        Else

            lblTestStockName.Text =
                stockName

        End If

    End Sub

End Class