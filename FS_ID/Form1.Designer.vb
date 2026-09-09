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
        Me.btnRestore = New System.Windows.Forms.Button()
        Me.lstBackup = New System.Windows.Forms.ListBox()
        Me.timerBackup = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.ListBox2 = New System.Windows.Forms.ListBox()
        Me.txtGroupName = New System.Windows.Forms.TextBox()
        Me.btnSaveGroupName = New System.Windows.Forms.Button()
        Me.btnDeleteStock = New System.Windows.Forms.Button()
        Me.btnClearGroupStocks = New System.Windows.Forms.Button()
        Me.txtTestStockID = New System.Windows.Forms.TextBox()
        Me.lblTestStockName = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblSource.Location = New System.Drawing.Point(10, 27)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.Size = New System.Drawing.Size(87, 16)
        Me.lblSource.TabIndex = 6
        Me.lblSource.Text = "原始檔案："
        '
        'txtSource
        '
        Me.txtSource.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSource.Enabled = False
        Me.txtSource.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txtSource.Location = New System.Drawing.Point(94, 24)
        Me.txtSource.Name = "txtSource"
        Me.txtSource.ReadOnly = True
        Me.txtSource.Size = New System.Drawing.Size(183, 20)
        Me.txtSource.TabIndex = 7
        Me.txtSource.Text = "\SYSTEM\01\FS_ID.DAT"
        '
        'lblBackup
        '
        Me.lblBackup.AutoSize = True
        Me.lblBackup.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblBackup.Location = New System.Drawing.Point(10, 55)
        Me.lblBackup.Name = "lblBackup"
        Me.lblBackup.Size = New System.Drawing.Size(87, 16)
        Me.lblBackup.TabIndex = 8
        Me.lblBackup.Text = "備份位置："
        '
        'txtBackup
        '
        Me.txtBackup.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtBackup.Enabled = False
        Me.txtBackup.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txtBackup.Location = New System.Drawing.Point(94, 55)
        Me.txtBackup.Name = "txtBackup"
        Me.txtBackup.ReadOnly = True
        Me.txtBackup.Size = New System.Drawing.Size(86, 20)
        Me.txtBackup.TabIndex = 9
        Me.txtBackup.Text = "\FS_BAK"
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblTime.Location = New System.Drawing.Point(10, 84)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(119, 16)
        Me.lblTime.TabIndex = 10
        Me.lblTime.Text = "每天備份時間："
        '
        'dtBackupTime
        '
        Me.dtBackupTime.CustomFormat = "HH:mm"
        Me.dtBackupTime.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.dtBackupTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtBackupTime.Location = New System.Drawing.Point(123, 77)
        Me.dtBackupTime.Name = "dtBackupTime"
        Me.dtBackupTime.ShowUpDown = True
        Me.dtBackupTime.Size = New System.Drawing.Size(72, 27)
        Me.dtBackupTime.TabIndex = 11
        '
        'btnSaveTime
        '
        Me.btnSaveTime.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnSaveTime.Location = New System.Drawing.Point(201, 75)
        Me.btnSaveTime.Name = "btnSaveTime"
        Me.btnSaveTime.Size = New System.Drawing.Size(86, 35)
        Me.btnSaveTime.TabIndex = 12
        Me.btnSaveTime.Text = "儲存時間"
        Me.btnSaveTime.UseVisualStyleBackColor = True
        '
        'btnBackupNow
        '
        Me.btnBackupNow.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnBackupNow.Location = New System.Drawing.Point(141, 146)
        Me.btnBackupNow.Name = "btnBackupNow"
        Me.btnBackupNow.Size = New System.Drawing.Size(86, 35)
        Me.btnBackupNow.TabIndex = 13
        Me.btnBackupNow.Text = "立即備份"
        Me.btnBackupNow.UseVisualStyleBackColor = True
        '
        'lblRestore
        '
        Me.lblRestore.AutoSize = True
        Me.lblRestore.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblRestore.Location = New System.Drawing.Point(12, 113)
        Me.lblRestore.Name = "lblRestore"
        Me.lblRestore.Size = New System.Drawing.Size(103, 16)
        Me.lblRestore.TabIndex = 14
        Me.lblRestore.Text = "選擇還原日期"
        '
        'btnRestore
        '
        Me.btnRestore.Enabled = False
        Me.btnRestore.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnRestore.Location = New System.Drawing.Point(141, 187)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(86, 35)
        Me.btnRestore.TabIndex = 19
        Me.btnRestore.Text = "還原備份"
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'lstBackup
        '
        Me.lstBackup.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lstBackup.FormattingEnabled = True
        Me.lstBackup.ItemHeight = 16
        Me.lstBackup.Location = New System.Drawing.Point(15, 146)
        Me.lstBackup.Name = "lstBackup"
        Me.lstBackup.Size = New System.Drawing.Size(120, 260)
        Me.lstBackup.TabIndex = 20
        '
        'timerBackup
        '
        Me.timerBackup.Enabled = True
        Me.timerBackup.Interval = 10000
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnRestore)
        Me.GroupBox1.Controls.Add(Me.lstBackup)
        Me.GroupBox1.Controls.Add(Me.btnBackupNow)
        Me.GroupBox1.Controls.Add(Me.lblRestore)
        Me.GroupBox1.Controls.Add(Me.btnSaveTime)
        Me.GroupBox1.Controls.Add(Me.dtBackupTime)
        Me.GroupBox1.Controls.Add(Me.lblSource)
        Me.GroupBox1.Controls.Add(Me.lblTime)
        Me.GroupBox1.Controls.Add(Me.txtSource)
        Me.GroupBox1.Controls.Add(Me.txtBackup)
        Me.GroupBox1.Controls.Add(Me.lblBackup)
        Me.GroupBox1.Font = New System.Drawing.Font("新細明體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(17, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(299, 426)
        Me.GroupBox1.TabIndex = 21
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "潛力股備份"
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 12
        Me.ListBox1.Location = New System.Drawing.Point(370, 39)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(120, 184)
        Me.ListBox1.TabIndex = 22
        '
        'ListBox2
        '
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.ItemHeight = 12
        Me.ListBox2.Location = New System.Drawing.Point(510, 39)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(154, 376)
        Me.ListBox2.TabIndex = 23
        '
        'txtGroupName
        '
        Me.txtGroupName.Location = New System.Drawing.Point(370, 238)
        Me.txtGroupName.Name = "txtGroupName"
        Me.txtGroupName.Size = New System.Drawing.Size(100, 22)
        Me.txtGroupName.TabIndex = 24
        '
        'btnSaveGroupName
        '
        Me.btnSaveGroupName.Location = New System.Drawing.Point(370, 266)
        Me.btnSaveGroupName.Name = "btnSaveGroupName"
        Me.btnSaveGroupName.Size = New System.Drawing.Size(75, 23)
        Me.btnSaveGroupName.TabIndex = 25
        Me.btnSaveGroupName.Text = "修改群組名稱"
        Me.btnSaveGroupName.UseVisualStyleBackColor = True
        '
        'btnDeleteStock
        '
        Me.btnDeleteStock.Location = New System.Drawing.Point(672, 39)
        Me.btnDeleteStock.Name = "btnDeleteStock"
        Me.btnDeleteStock.Size = New System.Drawing.Size(75, 23)
        Me.btnDeleteStock.TabIndex = 26
        Me.btnDeleteStock.Text = "刪除個股"
        Me.btnDeleteStock.UseVisualStyleBackColor = True
        '
        'btnClearGroupStocks
        '
        Me.btnClearGroupStocks.Location = New System.Drawing.Point(370, 308)
        Me.btnClearGroupStocks.Name = "btnClearGroupStocks"
        Me.btnClearGroupStocks.Size = New System.Drawing.Size(91, 23)
        Me.btnClearGroupStocks.TabIndex = 27
        Me.btnClearGroupStocks.Text = "清除群組股票"
        Me.btnClearGroupStocks.UseVisualStyleBackColor = True
        '
        'txtTestStockID
        '
        Me.txtTestStockID.Location = New System.Drawing.Point(688, 238)
        Me.txtTestStockID.Name = "txtTestStockID"
        Me.txtTestStockID.Size = New System.Drawing.Size(79, 22)
        Me.txtTestStockID.TabIndex = 28
        '
        'lblTestStockName
        '
        Me.lblTestStockName.AutoSize = True
        Me.lblTestStockName.Location = New System.Drawing.Point(710, 358)
        Me.lblTestStockName.Name = "lblTestStockName"
        Me.lblTestStockName.Size = New System.Drawing.Size(37, 12)
        Me.lblTestStockName.TabIndex = 29
        Me.lblTestStockName.Text = "Label1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.lblTestStockName)
        Me.Controls.Add(Me.txtTestStockID)
        Me.Controls.Add(Me.btnClearGroupStocks)
        Me.Controls.Add(Me.btnDeleteStock)
        Me.Controls.Add(Me.btnSaveGroupName)
        Me.Controls.Add(Me.txtGroupName)
        Me.Controls.Add(Me.ListBox2)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "Form1"
        Me.Text = "潛力股設定"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
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
    Friend WithEvents btnRestore As Button
    Friend WithEvents lstBackup As ListBox
    Friend WithEvents timerBackup As Timer
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents ListBox2 As ListBox
    Friend WithEvents txtGroupName As TextBox
    Friend WithEvents btnSaveGroupName As Button
    Friend WithEvents btnDeleteStock As Button
    Friend WithEvents btnClearGroupStocks As Button
    Friend WithEvents txtTestStockID As TextBox
    Friend WithEvents lblTestStockName As Label
End Class
