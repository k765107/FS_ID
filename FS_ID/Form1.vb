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
        ' 清空右邊
        '====================================================
        ListBox2.Items.Clear()


        If ListBox1.SelectedIndex < 0 Then

            Return

        End If


        '====================================================
        ' 取得目前選取的群組
        '====================================================
        Dim group As PotentialGroup =
        TryCast(
            ListBox1.SelectedItem,
            PotentialGroup)


        If group Is Nothing Then

            Return

        End If


        '====================================================
        ' 目前只顯示股票代碼
        '====================================================
        For Each stockID As String In group.StockIDs

            ListBox2.Items.Add(stockID)

        Next


    End Sub
End Class