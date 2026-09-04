Imports System.IO


Public Class Form1
    Private Sub btnSaveTime_Click(sender As Object, e As EventArgs) Handles btnSaveTime.Click

    End Sub

    Private Sub btnBackupNow_Click(sender As Object, e As EventArgs) Handles btnBackupNow.Click
        Try
            Dim programFolder As String = Application.StartupPath
            '========================================
            ' 原始檔案
            '========================================
            Dim sourceFile As String = Path.Combine(programFolder, "SYSTEM\01\FS_ID.DAT")

            Dim parentFolder As DirectoryInfo = Directory.GetParent(programFolder)
            '========================================
            ' 備份資料夾
            '========================================
            Dim backupFolder As String = Path.Combine(parentFolder.FullName, "FS_BAK")

            '========================================
            ' 先確認原始檔案存在
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
            ' 如果 D:\FS_BAK 不存在，就建立它
            '========================================
            If Not Directory.Exists(backupFolder) Then
                Directory.CreateDirectory(backupFolder)
            End If

            '========================================
            ' 取得今天日期
            ' 格式：yyyyMMdd
            '
            ' 例如：
            ' 2026/09/04
            ' 變成：
            ' 20260904
            '========================================
            Dim today As String = DateTime.Now.ToString("yyyyMMdd")

            '========================================
            ' 組合備份檔案名稱
            '========================================
            Dim backupFile As String =
                Path.Combine(backupFolder, today & "FS_ID.DAT")

            '========================================
            ' 執行備份
            '
            ' True = 如果同一天已經存在，就覆蓋
            '========================================
            File.Copy(sourceFile, backupFile, True)

            '========================================
            ' 顯示成功訊息
            '========================================
            MessageBox.Show(
                "備份完成！" & vbCrLf & vbCrLf &
                "備份檔案：" & vbCrLf &
                backupFile,
                "備份成功",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

            '========================================
            ' 在 ListBox 留下紀錄
            '========================================
            lstLog.Items.Add(
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") &
                "  備份成功：" &
                backupFile)

        Catch ex As Exception

            '========================================
            ' 發生錯誤
            '========================================
            MessageBox.Show(
                "備份失敗！" & vbCrLf & vbCrLf &
                ex.Message,
                "錯誤",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)

            lstLog.Items.Add(
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") &
                "  備份失敗：" &
                ex.Message)

        End Try
    End Sub
End Class