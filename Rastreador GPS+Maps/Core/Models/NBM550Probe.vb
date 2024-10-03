Public Class NBM550Probe
    Public model As String
    Public serialNumber As String
    Public dateOfCalibration As String
    Sub New()
    End Sub

    Sub New(model As String, serialNumber As String, dateOfCalibration As String)
        Me.model = model
        Me.serialNumber = serialNumber
        Me.dateOfCalibration = dateOfCalibration
    End Sub
End Class
