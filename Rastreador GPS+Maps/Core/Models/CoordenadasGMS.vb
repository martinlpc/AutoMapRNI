Public Class CoordenadasGMS
    Public Property Grados As Integer
    Public Property Minutos As Integer
    Public Property Segundos As Double
    Public Property Hemisferio As String

    Public Sub New()
    End Sub

    Public Sub New(grados As Integer, minutos As Integer, segundos As Double, hemisferio As String)
        Me.Grados = grados
        Me.Minutos = minutos
        Me.Segundos = segundos
        Me.Hemisferio = hemisferio
    End Sub
End Class
