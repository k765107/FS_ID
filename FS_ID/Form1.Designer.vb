<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請勿使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lblSource = New System.Windows.Forms.Label()
        Me.txtSource = New System.Windows.Forms.TextBox()
        Me.lblBackup = New System.Windows.Forms.Label()
        Me.txtBackup = New System.Windows.Forms.TextBox()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.dtBackupTime = New System.Windows.Forms.DateTimePicker()
        Me.btnSaveTime = New System.Windows.Forms.Button()
        Me.btnBackupNow = New System.Windows.Forms.Button()
        Me.lblRestore = New System.Windows.Forms.Label()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.lblRestoreFile = New System.Windows.Forms.Label()
        Me.txtRestoreFile = New System.Windows.Forms.TextBox()
        Me.btnFindRestore = New System.Windows.Forms.Button()
        Me.btnRestore = New System.Windows.Forms.Button()
        Me.lstLog = New System.Windows.Forms.ListBox()
        Me.timerBackup = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.Location = New System.Drawing.Point(61, 39)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.Size = New System.Drawing.Size(65, 12)
        Me.lblSource.TabIndex = 6
        Me.lblSource.Text = "原始檔案："
        '
        'txtSource
        '
        Me.txtSource.Location = New System.Drawing.Point(132, 36)
        Me.txtSource.Name = "txtSource"
        Me.txtSource.ReadOnly = True
        Me.txtSource.Size = New System.Drawing.Size(183, 22)
        Me.txtSource.TabIndex = 7
        Me.txtSource.Text = "D:\XGVS\SYSTEM\01\FS_ID.DAT"
        '
        'lblBackup
        '
        Me.lblBackup.AutoSize = True
        Me.lblBackup.Location = New System.Drawing.Point(61, 78)
        Me.lblBackup.Name = "lblBackup"
        Me.lblBackup.Size = New System.Drawing.Size(65, 12)
        Me.lblBackup.TabIndex = 8
        Me.lblBackup.Text = "備份位置："
        '
        'txtBackup
        '
        Me.txtBackup.Location = New System.Drawing.Point(132, 78)
        Me.txtBackup.Name = "txtBackup"
        Me.txtBackup.Size = New System.Drawing.Size(183, 22)
        Me.txtBackup.TabIndex = 9
        Me.txtBackup.Text = "D:\FS_BAK"
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.Location = New System.Drawing.Point(417, 67)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(89, 12)
        Me.lblTime.TabIndex = 10
        Me.lblTime.Text = "每天備份時間："
        '
        'dtBackupTime
        '
        Me.dtBackupTime.CustomFormat = "HH:mm"
        Me.dtBackupTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtBackupTime.Location = New System.Drawing.Point(512, 63)
        Me.dtBackupTime.Name = "dtBackupTime"
        Me.dtBackupTime.ShowUpDown = True
        Me.dtBackupTime.Size = New System.Drawing.Size(72, 22)
        Me.dtBackupTime.TabIndex = 11
        '
        'btnSaveTime
        '
        Me.btnSaveTime.Location = New System.Drawing.Point(604, 63)
        Me.btnSaveTime.Name = "btnSaveTime"
        Me.btnSaveTime.Size = New System.Drawing.Size(75, 23)
        Me.btnSaveTime.TabIndex = 12
        Me.btnSaveTime.Text = "儲存時間"
        Me.btnSaveTime.UseVisualStyleBackColor = True
        '
        'btnBackupNow
        '
        Me.btnBackupNow.Location = New System.Drawing.Point(523, 109)
        Me.btnBackupNow.Name = "btnBackupNow"
        Me.btnBackupNow.Size = New System.Drawing.Size(75, 23)
        Me.btnBackupNow.TabIndex = 13
        Me.btnBackupNow.Text = "立即備份"
        Me.btnBackupNow.UseVisualStyleBackColor = True
        '
        'lblRestore
        '
        Me.lblRestore.AutoSize = True
        Me.lblRestore.Location = New System.Drawing.Point(130, 194)
        Me.lblRestore.Name = "lblRestore"
        Me.lblRestore.Size = New System.Drawing.Size(77, 12)
        Me.lblRestore.TabIndex = 14
        Me.lblRestore.Text = "選擇還原日期"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Location = New System.Drawing.Point(132, 209)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(200, 22)
        Me.DateTimePicker1.TabIndex = 15
        '
        'lblRestoreFile
        '
        Me.lblRestoreFile.AutoSize = True
        Me.lblRestoreFile.Location = New System.Drawing.Point(130, 246)
        Me.lblRestoreFile.Name = "lblRestoreFile"
        Me.lblRestoreFile.Size = New System.Drawing.Size(77, 12)
        Me.lblRestoreFile.TabIndex = 16
        Me.lblRestoreFile.Text = "找到的備份："
        '
        'txtRestoreFile
        '
        Me.txtRestoreFile.Location = New System.Drawing.Point(213, 243)
        Me.txtRestoreFile.Name = "txtRestoreFile"
        Me.txtRestoreFile.ReadOnly = True
        Me.txtRestoreFile.Size = New System.Drawing.Size(100, 22)
        Me.txtRestoreFile.TabIndex = 17
        '
        'btnFindRestore
        '
        Me.btnFindRestore.Location = New System.Drawing.Point(349, 211)
        Me.btnFindRestore.Name = "btnFindRestore"
        Me.btnFindRestore.Size = New System.Drawing.Size(75, 23)
        Me.btnFindRestore.TabIndex = 18
        Me.btnFindRestore.Text = "尋找備份"
        Me.btnFindRestore.UseVisualStyleBackColor = True
        '
        'btnRestore
        '
        Me.btnRestore.Enabled = False
        Me.btnRestore.Location = New System.Drawing.Point(462, 211)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(149, 23)
        Me.btnRestore.TabIndex = 19
        Me.btnRestore.Text = "還原選定日期（覆蓋）"
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'lstLog
        '
        Me.lstLog.FormattingEnabled = True
        Me.lstLog.ItemHeight = 12
        Me.lstLog.Location = New System.Drawing.Point(358, 246)
        Me.lstLog.Name = "lstLog"
        Me.lstLog.Size = New System.Drawing.Size(120, 88)
        Me.lstLog.TabIndex = 20
        '
        'timerBackup
        '
        Me.timerBackup.Enabled = True
        Me.timerBackup.Interval = 10000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.lstLog)
        Me.Controls.Add(Me.btnRestore)
        Me.Controls.Add(Me.btnFindRestore)
        Me.Controls.Add(Me.txtRestoreFile)
        Me.Controls.Add(Me.lblRestoreFile)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Controls.Add(Me.lblRestore)
        Me.Controls.Add(Me.btnBackupNow)
        Me.Controls.Add(Me.btnSaveTime)
        Me.Controls.Add(Me.dtBackupTime)
        Me.Controls.Add(Me.lblTime)
        Me.Controls.Add(Me.txtBackup)
        Me.Controls.Add(Me.lblBackup)
        Me.Controls.Add(Me.txtSource)
        Me.Controls.Add(Me.lblSource)
        Me.Name = "Form1"
        Me.Text = "f"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSource As Label
    Friend WithEvents txtSource As TextBox
    Friend WithEvents lblBackup As Label
    Friend WithEvents txtBackup As TextBox
    Friend WithEvents lblTime As Label
    Friend WithEvents dtBackupTime As DateTimePicker
    Friend WithEvents btnSaveTime As Button
    Friend WithEvents btnBackupNow As Button
    Friend WithEvents lblRestore As Label
    Friend WithEvents DateTimePicker1 As DateTimePicker
    Friend WithEvents lblRestoreFile As Label
    Friend WithEvents txtRestoreFile As TextBox
    Friend WithEvents btnFindRestore As Button
    Friend WithEvents btnRestore As Button
    Friend WithEvents lstLog As ListBox
    Friend WithEvents timerBackup As Timer


End Class
