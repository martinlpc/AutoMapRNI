<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSondaEMR
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ListaSondasEMR300 = New System.Windows.Forms.ListView()
        Me.Tipo = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.NumSerie = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LimInf = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LimSup = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Incert = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Factor = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnAceptar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(474, 33)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Seleccione la sonda que utilizará en conjunto con el medidor NARDA EMR-300:"
        '
        'ListaSondasEMR300
        '
        Me.ListaSondasEMR300.CheckBoxes = True
        Me.ListaSondasEMR300.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Tipo, Me.NumSerie, Me.LimInf, Me.LimSup, Me.Incert, Me.Factor})
        Me.ListaSondasEMR300.GridLines = True
        Me.ListaSondasEMR300.Location = New System.Drawing.Point(12, 45)
        Me.ListaSondasEMR300.Name = "ListaSondasEMR300"
        Me.ListaSondasEMR300.Size = New System.Drawing.Size(469, 322)
        Me.ListaSondasEMR300.TabIndex = 0
        Me.ListaSondasEMR300.UseCompatibleStateImageBehavior = False
        Me.ListaSondasEMR300.View = System.Windows.Forms.View.Details
        '
        'Tipo
        '
        Me.Tipo.Text = "Tipo"
        Me.Tipo.Width = 40
        '
        'NumSerie
        '
        Me.NumSerie.Text = "Número de serie"
        Me.NumSerie.Width = 95
        '
        'LimInf
        '
        Me.LimInf.Text = "FrecMin [MHz]"
        Me.LimInf.Width = 85
        '
        'LimSup
        '
        Me.LimSup.Text = "FrecMax [MHz]"
        Me.LimSup.Width = 85
        '
        'Incert
        '
        Me.Incert.Text = "Incertidumbre [dB]"
        Me.Incert.Width = 102
        '
        'Factor
        '
        Me.Factor.Text = "Factor"
        '
        'btnAceptar
        '
        Me.btnAceptar.Location = New System.Drawing.Point(298, 373)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(125, 26)
        Me.btnAceptar.TabIndex = 1
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(90, 373)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(113, 26)
        Me.btnCancelar.TabIndex = 2
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmSondaEMR
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(493, 411)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.ListaSondasEMR300)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmSondaEMR"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sondas para el EMR-300"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ListaSondasEMR300 As System.Windows.Forms.ListView
    Friend WithEvents Tipo As System.Windows.Forms.ColumnHeader
    Friend WithEvents LimInf As System.Windows.Forms.ColumnHeader
    Friend WithEvents NumSerie As System.Windows.Forms.ColumnHeader
    Friend WithEvents LimSup As System.Windows.Forms.ColumnHeader
    Friend WithEvents Incert As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents Factor As System.Windows.Forms.ColumnHeader
End Class
