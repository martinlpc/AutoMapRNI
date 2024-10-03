Public Class PuntoMedicion
    Public Property Indice As Integer
    Public Property NivelPuro As Single
    Public Property NivelFinal As Single ' NivelPuro * Factor
    Public Property Unidad As String
    Public Property Incert As Single
    Public Property Factor As Single
    Public Property Lat As Double
    Public Property Lon As Double
    Public Property Fecha As Date
    Public Property Hora As DateTime
    Public Property Instrumento As String
    Public Property Sonda As String

    Public Sub New()
    End Sub

    Public Sub New(indice As Integer, nivelPuro As Single, unidad As String,
                   incert As Single, factor As Single, lat As Double, lon As Double,
                   fecha As Date, hora As DateTime, instr As String, sonda As String)

        Me.Indice = indice
        Me.NivelPuro = nivelPuro
        Me.NivelFinal = FormatNumber(nivelPuro * factor, 3)
        Me.Incert = incert
        Me.Factor = factor
        Me.Lat = lat
        Me.Lon = lon
        Me.Fecha = fecha
        Me.Hora = hora
        Me.Instrumento = instr
        Me.Sonda = sonda
    End Sub

    Public Overrides Function ToString() As String
        Return String.Format(
            "#{0}, Nivel s/incert: {1} {3}, Nivel c/incert: {2} {3}, Lat: {4}, Lon: {5}, Fecha: {6}, Hora: {7}, Incert: {8} dB, Factor: {9}, Instrumento: {10}, Sonda: {11}",
            Indice, NivelPuro, NivelFinal, Unidad, Lat, Lon, Fecha, Hora, Incert, Factor, Instrumento, Sonda)
    End Function
End Class
