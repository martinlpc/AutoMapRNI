<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDebug
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.debug = New System.Windows.Forms.CheckBox()
        Me.btnLeerDatos = New System.Windows.Forms.Button()
        Me.btnConectarNBM = New System.Windows.Forms.Button()
        Me.linkGetNMEAData = New System.Windows.Forms.LinkLabel()
        Me.txtNMEAPort = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(12, 114)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(530, 394)
        Me.TextBox1.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.debug)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(218, 36)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Datos de GPS"
        '
        'debug
        '
        Me.debug.AutoSize = True
        Me.debug.Location = New System.Drawing.Point(77, 13)
        Me.debug.Name = "debug"
        Me.debug.Size = New System.Drawing.Size(124, 17)
        Me.debug.TabIndex = 4
        Me.debug.Text = "mostrar raw gps data"
        Me.debug.UseVisualStyleBackColor = True
        '
        'btnLeerDatos
        '
        Me.btnLeerDatos.Location = New System.Drawing.Point(375, 70)
        Me.btnLeerDatos.Name = "btnLeerDatos"
        Me.btnLeerDatos.Size = New System.Drawing.Size(101, 23)
        Me.btnLeerDatos.TabIndex = 8
        Me.btnLeerDatos.Text = "obtener datos"
        Me.btnLeerDatos.UseVisualStyleBackColor = True
        '
        'btnConectarNBM
        '
        Me.btnConectarNBM.Location = New System.Drawing.Point(375, 41)
        Me.btnConectarNBM.Name = "btnConectarNBM"
        Me.btnConectarNBM.Size = New System.Drawing.Size(101, 23)
        Me.btnConectarNBM.TabIndex = 7
        Me.btnConectarNBM.Text = "conectar nbm"
        Me.btnConectarNBM.UseVisualStyleBackColor = True
        '
        'linkGetNMEAData
        '
        Me.linkGetNMEAData.AutoSize = True
        Me.linkGetNMEAData.Location = New System.Drawing.Point(116, 77)
        Me.linkGetNMEAData.Name = "linkGetNMEAData"
        Me.linkGetNMEAData.Size = New System.Drawing.Size(97, 13)
        Me.linkGetNMEAData.TabIndex = 4
        Me.linkGetNMEAData.TabStop = True
        Me.linkGetNMEAData.Text = "Leer NMEAReader"
        '
        'txtNMEAPort
        '
        Me.txtNMEAPort.Location = New System.Drawing.Point(38, 70)
        Me.txtNMEAPort.Name = "txtNMEAPort"
        Me.txtNMEAPort.Size = New System.Drawing.Size(66, 20)
        Me.txtNMEAPort.TabIndex = 5
        Me.txtNMEAPort.Text = "COM"
        '
        'frmDebug
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(554, 520)
        Me.Controls.Add(Me.txtNMEAPort)
        Me.Controls.Add(Me.linkGetNMEAData)
        Me.Controls.Add(Me.btnLeerDatos)
        Me.Controls.Add(Me.btnConectarNBM)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "frmDebug"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmDebug"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents debug As System.Windows.Forms.CheckBox
    Friend WithEvents btnLeerDatos As System.Windows.Forms.Button
    Friend WithEvents btnConectarNBM As System.Windows.Forms.Button
    Friend WithEvents linkGetNMEAData As System.Windows.Forms.LinkLabel
    Friend WithEvents txtNMEAPort As System.Windows.Forms.TextBox


End Class
