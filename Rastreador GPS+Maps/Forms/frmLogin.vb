Public Class frmLogin

    ' TODO: inserte el código para realizar autenticación personalizada usando el nombre de usuario y la contraseña proporcionada 
    ' (Consulte http://go.microsoft.com/fwlink/?LinkId=35339).  
    ' El objeto principal personalizado se puede adjuntar al objeto principal del subproceso actual como se indica a continuación: 
    '     My.User.CurrentPrincipal = CustomPrincipal
    ' donde CustomPrincipal es la implementación de IPrincipal utilizada para realizar la autenticación. 
    ' Posteriormente, My.User devolverá la información de identidad encapsulada en el objeto CustomPrincipal
    ' como el nombre de usuario, nombre para mostrar, etc.

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        frmMain.user = txtUser.Text
        frmMain.pass = txtPass.Text
        Me.Close()
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

    Private Sub txtUser_TextChanged(sender As Object, e As System.EventArgs) Handles txtUser.TextChanged
        If txtUser.Text.Contains("\") Then
            Dim vec(2) As String
            vec = Split(txtUser.Text, "\")
            lblDominio.Text = "Dominio: " & vec(0)
        Else
            lblDominio.Text = ""
        End If
    End Sub

    Private Sub frmLogin_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If Environment.UserName = "laboratorio" Then
            txtUser.Text = "CNC\mpacheco"
        Else
            txtUser.Text = Environment.UserDomainName & "\" & Environment.UserName
        End If
    End Sub
End Class
