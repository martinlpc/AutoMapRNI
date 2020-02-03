Module GestionCoor
    ''' <summary>
    ''' Estructura que aloja una componente de coordenadas en grados, minutos y segundos
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure CoordenadasGMS
        Public Grados As Integer
        Public Minutos As Integer
        Public Segundos As Double
        Public Hemisf As String '-- N/S/E/O-W
    End Structure
    ''' <summary>
    ''' Convierte una componente de coordenadas geográficas de Grados Decimales a Grados Minutos Segundos Decimales
    ''' </summary>
    ''' <param name="Posicion">Coordenada en formato decimal (por ej: -34.3456)</param>
    ''' <param name="esLng">Booleano que indica si se trata de una coordenada de longitud o no</param>
    ''' <returns>Devuelve una componente de coordenada en Grados Minutos Segundos Decimales</returns>
    ''' <remarks></remarks>
    Public Function ConvertirAGMS(Posicion As Double, esLng As Boolean) As CoordenadasGMS

        Dim ret As CoordenadasGMS
        ' Negativo: Sur
        ' Positivo: Norte
        '----SI ES LONG:
        ' Negativo: Oeste
        ' Positvo: Este
        Dim ValorAbs As Double = Math.Abs(Math.Round(Posicion * 1000000))
        Dim Signo As Integer = Math.Sign(Posicion)

        ret.Grados = Math.Floor(ValorAbs / 1000000)
        ret.Minutos = Math.Floor(((ValorAbs / 1000000) - Math.Floor(ValorAbs / 1000000)) * 60)
        ret.Segundos = Math.Round( _
                            Math.Floor(((((ValorAbs / 1000000) - Math.Floor(ValorAbs / 1000000)) * 60) - _
                            Math.Floor(((ValorAbs / 1000000) - Math.Floor(ValorAbs / 1000000)) * 60)) * 100000) * 60 / 100000, _
                            3)

        If esLng Then
            If Signo > 0 Then
                ret.Hemisf = "E"
            Else
                ret.Hemisf = "O"
            End If
        Else
            If Signo > 0 Then
                ret.Hemisf = "N"
            Else
                ret.Hemisf = "S"
            End If
        End If
        Return ret
        ' Por default se considera que es LATITUD
        ' EJEMPLO de datos entregados:
        ' Grados: 34
        ' Minutos: 45
        ' Segundos: 13,9
    End Function
    ''' <summary>
    ''' Convierte la componente geográfica de un punto de Grados Minutos Segundos Decimales a Grados Decimales
    ''' </summary>
    ''' <param name="Posicion">Latitud o Longitud en Grados Minutos Segundos Decimales</param>
    ''' <returns>Devuelve la componente en Grados Decimales</returns>
    ''' <remarks></remarks>
    Public Function ConvertirAGDec(Posicion As CoordenadasGMS) As Double
        Dim ret As Double
        With Posicion
            ret = Math.Abs(.Grados) + .Minutos / 60 + .Segundos / 3600
            If .Hemisf = "S" Or .Hemisf = "O" Or .Hemisf = "W" Then
                ret = 0 - Math.Abs(ret) ' Para hacerlo negativo
            End If
        End With
        Return ret
    End Function
    ''' <summary>
    ''' Calcula la distancia aproximada, en metros, entre dos puntos geográficos.
    ''' Se necesita una estructura GMS.
    ''' </summary>
    ''' <param name="GMSLat1">Latitud del punto 1 en estructura GradMinSeg decimales</param>
    ''' <param name="GMSLng1">Longitud del punto 1 en estructura GradMinSeg decimales</param>
    ''' <param name="GMSLat2">Latitud del punto 2 en estructura GradMinSeg decimales</param>
    ''' <param name="GMSLng2">Longitud del punto 2 en estructura GradMinSeg decimales</param>
    ''' <returns>Devuelve la distancia aproximada en metros</returns>
    ''' <remarks>El cálculo utilizado podría no ser tan exacto para distancias muy grandes (mas de 1000 KMs).</remarks>
    Public Function CalcularDist(GMSLat1 As CoordenadasGMS, GMSLng1 As CoordenadasGMS, GMSLat2 As CoordenadasGMS, GMSLng2 As CoordenadasGMS) As Single

        'Rad es la constante para convertir angulos (Grados decimales) a radianes
        Dim Rad As Single = Math.PI / 180

        Dim Lat1 As Double = ConvertirAGDec(GMSLat1)
        Dim Lng1 As Double = ConvertirAGDec(GMSLng1)

        Dim Lat2 As Double = ConvertirAGDec(GMSLat2)
        Dim Lng2 As Double = ConvertirAGDec(GMSLng2)

        'Para resultado en millas R = 3959
        'Para usar kilometros R = 6371  

        Dim R As Integer = 6371 'Radio Terrestre en KMs
        Dim DeltaLat As Double = (Lat2 * Rad - Lat1 * Rad)
        Dim DeltaLng As Double = (Lng2 * Rad - Lng1 * Rad)
        Dim A As Double = Math.Pow(Math.Sin(DeltaLat / 2), 2) + _
            Math.Cos(Lat1 * Rad) * _
            Math.Cos(Lat2 * Rad) * _
            Math.Pow(Math.Sin(DeltaLng / 2), 2)
        Dim C As Double = 2 * Math.Atan2(Math.Sqrt(A), Math.Sqrt(1 - A))
        Dim Dist As Single = Math.Round(R * C * 1000, 2)

        Return Dist 'en Metros
    End Function
    Public Function CalcMD5(strCheck As String) As String
        Dim checksum As Integer = 0
        For Each Caracter As Char In strCheck
            checksum = checksum Xor Convert.ToByte(Caracter)
        Next
        Return checksum.ToString("X2")
    End Function
End Module
