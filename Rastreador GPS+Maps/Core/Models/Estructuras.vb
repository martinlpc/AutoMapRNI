Module Estructuras
    Class resultCargado
        'NO IMPLEMENTADO, RESERVADO PARA PRUEBAS FUTURAS
        'SE PREVEE CARGAR EN MEMORIA EL ARCHIVO DE RESULTADOS PARA CREAR INFORMES Y KMLS
        Private strRuta As String
        Private nomCamp As String
        Private iniCamp As Date
        Private Equipo As String
        Private serieEquipo As String
        Private Sonda As String
        Private serieSonda As String
        Private incertDB As Single
        Private Factor As Single
        Private regArray As Array
    End Class
    Public Structure Equipo
        Public Marca As String
        Public Modelo As String
        Public NumSerie As String
    End Structure
    Public Structure Sonda 'GENERICO PARA ESCRIBIR LOS RESULTADOS
        Public Marca As String
        Public Modelo As String
        Public NumSerie As String
        Public FechaCal As String 'Date
        Public IncertDB As Single
        Public Factor As Single
    End Structure
    Public SondaSel As Sonda
    Public Structure SondaEMR300
        Public Tipo As Integer
        Public NumSerie As String
        Public Incert As Single
        Public Factor As Single
        Public LimInf As Single
        Public LimSup As Single
        Public FechaCal As String 'Date
    End Structure
    'Hay 18 sondas para el emr300 en todo el pais y hay que cargar cada una porque no se puede ver cual esta conectada desde el equipo
    Public Sondas300(0 To 17) As SondaEMR300
    Public Structure SondaNBM550
        Public Nombre As String
        Public LimInf As Single
        Public LimSup As Single
        Public Incert As Single
        Public VecesDB As Single
        Public FechaCal As String 'Date
    End Structure
    'El equipo informa la sonda que está conectada, por lo que se compara el modelo con el que se cargo en el txt y se obtiene la incert
    Public Sondas550(0 To 5) As SondaNBM550
    Public Sondas550_SP(0 To 5) As SondaNBM550
    Public Structure Registro
        Public Indice As Integer
        Public Nivel As Single
        Public NivelPuro As Single
        Public PorcentMEP As Single
        Public Unidad As String
        Public Fecha As String 'Date
        Public Hora As String
        Public Lat As String
        Public Lon As String
        Public FechaCalSonda As String 'Date
        Public IncertdB As Single
        Public VecesdB As Single
        Public Color As String
    End Structure

    Public Structure ColorNiv
        Public Nivel As Single
        Public Color As String
    End Structure
    'Acá se gestionan los colores para los distintos niveles detectados
    Public Paleta(6) As ColorNiv
End Module
