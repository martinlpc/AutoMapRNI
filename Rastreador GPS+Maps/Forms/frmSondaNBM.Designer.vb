<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSondaNBM
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
        Me.btnKangoo = New System.Windows.Forms.Button()
        Me.btnSprinter = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnKangoo
        '
        Me.btnKangoo.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnKangoo.Location = New System.Drawing.Point(12, 67)
        Me.btnKangoo.Name = "btnKangoo"
        Me.btnKangoo.Size = New System.Drawing.Size(147, 23)
        Me.btnKangoo.TabIndex = 0
        Me.btnKangoo.Text = "Kangoo/Amarok/Ranger"
        '
        'btnSprinter
        '
        Me.btnSprinter.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSprinter.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnSprinter.Location = New System.Drawing.Point(191, 67)
        Me.btnSprinter.Name = "btnSprinter"
        Me.btnSprinter.Size = New System.Drawing.Size(165, 23)
        Me.btnSprinter.TabIndex = 1
        Me.btnSprinter.Text = "Sprinter / Vehiculo techo alto"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(48, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(267, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Seleccione el vehículo que va a utilizar en la medición:"
        '
        'frmSondaNBM
        '
        Me.AcceptButton = Me.btnKangoo
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnSprinter
        Me.ClientSize = New System.Drawing.Size(368, 110)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnSprinter)
        Me.Controls.Add(Me.btnKangoo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSondaNBM"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Seleccionar vehículo"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnKangoo As System.Windows.Forms.Button
    Friend WithEvents btnSprinter As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
