Imports GMap.NET
Imports GMap.NET.CacheProviders
Imports GMap.NET.MapProviders
Imports GMap.NET.WindowsForms
Imports GMap.NET.WindowsForms.Markers
Imports GMap.NET.Internals
Imports Microsoft.Office
Imports GPS_Martin
Imports Microsoft.Office.Interop
Imports System
Imports System.Threading
Imports System.Globalization
Imports System.IO
Imports Ionic.Zip
Imports System.Net

Public Class frmMain
#Region "Variables comunes de programa"
    Dim ModoConexion As String ' Usar servidor, cache o ambos
    Dim ProvMapa As GMapProvider ' El tipo de mapa que se quiere visualizar
    Public RutaRaizRes As String
    Public RutaRaizKML As String ' DEFINIR ESTA RUTA
    Dim NivelAlarma As Single
    Dim hayAlarma As Boolean = False
    Dim Instrumento As Equipo
    Dim separadores() As String = {",", ";"}
    Dim trimmers() As Char = {vbCr, Chr(34), "'", "°", " """}
    Dim ContSondas500 As Integer
    Public NivelNarda As Single
    Public UnidadActual As String
    Public NivMax As Single
    Dim LatitudActual, LongitudActual As CoordenadasGMS 'Lo último que tomó el GPS
    Dim PosicionStr As String
    Dim PosicionCoor As PointLatLng
    Dim CoorAUbicar As PointLatLng
    Public mkrActual As GMapMarker = Nothing 'Marker que se va a mostrar/ocultar desde ListaResultados (por su checkbox)
    'Ultimo punto geografico donde se centró el mapa (sea por haber realizado una busqueda, ir al inicio o a donde se haya arrastrado con el mouse)
    Dim UltimoPunto As PointLatLng
    Dim UltimoZoomLvl As Integer
    Dim OverlayCarga As GMapOverlay = New GMapOverlay("OverlayCarga")
    Dim OverlayResultados As GMapOverlay = New GMapOverlay("OverlayResultados")
    Dim OverlayPosActual As GMapOverlay = New GMapOverlay("OverlayPosActual")
    Dim OverlayBusqueda As GMapOverlay = New GMapOverlay("OverlayBusqueda")
    Dim OverlayClick As GMapOverlay = New GMapOverlay("OverlayClick")
    Dim ovlyPrefetch As GMapOverlay = New GMapOverlay("ovlyPrefetch")
    'Banderas que determinan que se presionó o que combo cambió
    'BuscoCoor sirve para ver si se busco coordenada o lugar (por letras)
    Public Conectado As Boolean = False 'true = se presionó el botón "CONECTAR" y se conectó correctamente. Tomando lecturas.
    Public CambioProv, CambioModoConexion, BuscoHome, BuscoCoor, PrimerMuestreo As Boolean
    Public boolSprinter As Boolean = False
    Public ArranqueApp As Boolean = True
    Public StatusGpS As Boolean = False
    Public boolStopCamp As Boolean = False
    Public boolCheck As Boolean = False
    Public boolKillThrMaxEMR As Boolean = True
    Public boolResetMaxEMR As Boolean = False
    Public BaudRate As Integer = 460800 'Por default, está seteado para USB
    Public GPSSel As Integer = 1 ' 1 = GARMIN ; 2 = NMEA0183
    Public user, pass As String

    Dim PaletaRNI(10) As Integer '0 el minimo, 9 el maximo
    'El hilo que se va a encargar de leer el NARDA evitando que se trabe el funcionamiento por arrastrar el mapa al mismo tiempo
    ' NECESITA DESHABILITAR EL CHECKILLEGALCROSSTHREAD
    'Dim LockThis As New Object
#End Region

#Region "Declaración de Threads (Subprocesos/hilos de ejecución) y delegados"
    Dim ProcesoLeerNarda As New Thread(AddressOf LeerNarda)
    Dim ProcesoEMRMax As New Thread(AddressOf t_EMRMax)
    'Dim ProcesoNarda As New Thread(AddressOf t_Narda)
    Dim ProcesoUbicar As Thread
    Dim ProcesoGPS As New Thread(AddressOf LeerGPS)
    Delegate Sub LeerGPSthread()
    Delegate Sub LeerNardaThread()
    Delegate Sub MaxEMRThread()
#End Region

#Region "Delegados y variables asociados al control del GPS USB"
    'Public GPS As New GarminUSBDevice
    'Dim GPS_State As String = "Desconocido"
    'Dim saved_pvt_data As pvt_data_type
    'Delegate Sub EstadoConexionThread(Conectado As Boolean)
    'Delegate Sub ProcesarDatosPVTThread(ByRef pvt_data As pvt_data_type)
    'Delegate Sub ProcesarDatosSATThread(ByRef sat_data_array As sat_data_type())
#End Region

#Region "Subprocedimientos LoadForm y asociados a eventos de ventana"

    Private Sub frmMain_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyValue
            Case Is = 107
                Mapa.Zoom += 1
            Case Is = 109
                Mapa.Zoom -= 1
        End Select
    End Sub

    Private Sub Inicio(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' - ESTO NO ES RECOMENDADO PORQUE PUEDE ACARREAR NUEVOS ERRORES
        ' - Evita las excepciones por interbloqueo entre hilos y procesos
        'CheckForIllegalCrossThreadCalls = False
        '---------------------------------------------------------------
        Try
            ' Mapa.CacheLocation = Environment.CurrentDirectory & "\Data.gmdb"
            'Me.Icon = My.Resources.ENACOM_sintexto_final
            Me.Text = "AutoMap RNI - " & String.Format("v{0}", My.Application.Info.Version.ToString)
            Cursor = Cursors.WaitCursor
            txtEventos.Text &= "[" & Now & "] " & "Sistema iniciado" & vbNewLine
            Try
                CargarSondas()
            Catch ex As Exception
                txtEventos.Text &= "[" & Now & "] " & "Excepción ocurrida: no se encuentran los archivos de sondas. Contacte a Mantenimiento Radioeléctrico." & vbNewLine
                btnIniciarCamp.Enabled = False
                btnConectar.Enabled = False
            End Try
            CargarPaleta()
            PrimerMuestreo = True
            cboIntervalo.SelectedItem = "5"
            Mapa.IgnoreMarkerOnMouseWheel = True
            'Mapa.LevelsKeepInMemmory = 6

            'Agrega todos los puertos COM disponibles en el ComboBox NARDA
            For Each sp As String In My.Computer.Ports.SerialPortNames
                cboPuertoNarda.Items.Add(sp)
                cboCOMGlobalSat.Items.Add(sp)
            Next

            cboPuertoNarda.SelectedItem = "COM7" 'COM7 USB - COM8 OPTICO
            cboProvMapa.Text = "Google Maps"
            cboModoConexion.Text = "Servidor y caché"
            Mapa.MaxZoom = 19
            trkZoom.Maximum = Mapa.MaxZoom
            trkZoom.Minimum = Mapa.MinZoom

            Dim random1 As Integer = CInt(Math.Floor((100 - 1 + 1) * Rnd())) + 1
            Dim random2 As Integer = CInt(Math.Floor((99 - 1 + 1) * Rnd())) + 1
            Dim random3 As Integer = CInt(Math.Floor((90 - 1 + 1) * Rnd())) + 1

            MapProviders.GMapProvider.UserAgent = _
                String.Format("Mozilla/5.0 (Windows NT {1}.0; {2}rv:{0}.0) Gecko/20100101 Firefox/{0}.0",
                              random1, random2, random3)


            'Stuff.random.Next(DateTime.Today.Year - 1969 - 5, DateTime.Today.Year - 1969),
            'Stuff.random.Next(0, 10) % 2 == 0 ? 10 : 6,
            'Stuff.random.Next(0, 10) % 2 == 1 ? string.Empty : "WOW64; ")


            GMapProvider.Language = LanguageType.Spanish
            BuscoCoor = True
            IrAInicio(Mapa, e)
            trkZoom.Value = Mapa.Zoom
            lblZoom.Text = Mapa.Zoom.ToString
            Mapa.DragButton = Windows.Forms.MouseButtons.Left
            Mapa.Overlays.Add(OverlayCarga)
            Mapa.Overlays.Add(OverlayPosActual)
            Mapa.Overlays.Add(OverlayClick)
            Mapa.Overlays.Add(OverlayResultados)
            Mapa.Overlays.Add(OverlayBusqueda)
            Mapa.Overlays.Add(ovlyPrefetch)
            ' ProcesoNarda = New Thread(AddressOf t_Narda)
            'With ProcesoNarda
            '.IsBackground = True
            '.Start()
            'End With
        Catch ex As Exception
            txtEventos.Text &= "[" & Now & "] " & "Excepción ocurrida en el inicio del sistema: " & ex.Message & vbNewLine
        Finally
            Cursor = Cursors.Arrow
        End Try
    End Sub

    Private Sub PrefetchAreaSeleccionadaToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PrefetchAreaSeleccionadaToolStripMenuItem.Click
        'Dim loopshechos As Integer
        Try
            If ModoConexion = AccessMode.CacheOnly Then
                MsgBox("No es posible realizar la descarga si el modo de conexión es ''Solo caché''.", vbExclamation, "Aplicación en modo desconectado")
                Exit Sub
            Else
                If Mapa.SelectedArea.Size = New SizeLatLng(0, 0) Then
                    MsgBox("Seleccione un área para descargar manteniendo SHIFT y arrastrando con el boton derecho del mouse.", vbInformation, "No hay area seleccionada")
                    Exit Sub
                End If
            End If
            trkZoom.Value = Mapa.Zoom
            For zoom As Integer = Mapa.Zoom To Mapa.MaxZoom
                Using coso As TilePrefetcher = New GMap.NET.TilePrefetcher()
                    Mapa.Manager.Mode = AccessMode.ServerAndCache
                    coso.Overlay = ovlyPrefetch
                    coso.Owner = Me

                    'coso.ShowCompleteMessage = True
                    'For zoom As Integer = Mapa.Zoom To Mapa.MaxZoom
                    coso.Start(Mapa.SelectedArea, zoom, Mapa.MapProvider, 10, 1)

                    'loopshechos += 1
                    'Next
                End Using
                ovlyPrefetch.Clear()
            Next
            Mapa.Manager.Mode = AccessMode.CacheOnly

        Catch ex As Exception
            txtEventos.Text &= "[" & Now & "] " & "Error en prefetching: " & ex.Message & vbNewLine '" (loops: " & loopshechos & ")." & vbNewLine
        Finally
            ovlyPrefetch.Clear()
        End Try
    End Sub

    Private Sub Debugging(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control And e.KeyCode = Keys.D Then
            If ModoDebug.Checked = False Then
                MsgBox("Modo debug activado. Generando GPS virtual e ignorando distancia mínima.")
                ModoDebug.Checked = True
            Else
                MsgBox("Modo debug desactivado.")
                ModoDebug.Checked = False
            End If
        End If
        If e.Control And e.KeyCode = Keys.G And ModoDebug.Checked Then
            frmDebug.Show()
            frmDebug.BringToFront()
        End If
    End Sub

    Private Sub IniciarRecorrido(sender As System.Object, e As System.EventArgs) Handles btnIniciarCamp.Click
        Dim ColorMarker As GMarkerGoogleType
        Dim UltimaLat, UltimaLng As CoordenadasGMS
        Dim ActLatGDEC, ActLngGDEC As Double
        Dim IndiceRes As Integer = 0
        Dim nReg As ListViewItem
        Dim DistUltPunto As Single
        Dim PrimerBucle As Boolean = True
        Dim NivelPuro, NivelFinal As Single
        Dim IndiceImg As Integer
        Dim SW As StreamWriter
        'Dim NombreCampaña As String
        Dim outHeader As String
        Dim outReg As Registro
        Dim outBuffer As String
        Dim factorSonda As Single = SondaSel.Factor
        Dim outCrypt As String
        Dim Criptografo As Encriptador = New Encriptador("029112")

SeguirCampaña:
        Try
            'Se pone en cero la medicion del EMR para que empiece a tomar desde el arranque del recorido y no antes
            If chkEMR300.Checked Then boolResetMaxEMR = True
            Do While Not boolStopCamp And Conectado 'And tmrNarda.Enabled 'If Not boolStopCamp And tmrNarda.Enabled = True Then
                If PrimerBucle Then
                    Try
                        If SeleccionarDestino() = False Then
                            MsgBox("Se canceló el lanzamiento del recorrido.", MsgBoxStyle.Information, "Recorrido no iniciado")
                            Throw New ApplicationException("CanceladaRuta")
                        End If
                    Catch exDestino As Exception
                        If exDestino.Message.Contains("CanceladaRuta") Then Exit Sub
                    End Try

                    lblCargado.Text = "Guardando registros de recorrido en " & RutaRaizRes
                    lblCargado.Visible = True
                    SW = New StreamWriter(RutaRaizRes, False)
                    SW.AutoFlush = True
                    outHeader = "#Nombre de recorrido: " & RutaRaizRes.Substring(RutaRaizRes.LastIndexOf("\") + 1, RutaRaizRes.LastIndexOf(".") - 1 - RutaRaizRes.LastIndexOf("\")) & " - " & lblTipoRes.Text & " [" & String.Format("v{0}", My.Application.Info.Version.ToString) & "]" & vbNewLine & _
                        "#Fecha de inicio: " & Now & vbNewLine & _
                        "#Instrumento de medición: " & Instrumento.Marca & " " & Instrumento.Modelo & vbNewLine & _
                        "#Número de serie de instrumento: " & Instrumento.NumSerie & vbNewLine & _
                        "#Sonda utilizada: " & SondaSel.Marca & " " & SondaSel.Modelo & vbNewLine & _
                        "#Número de serie de sonda: " & SondaSel.NumSerie & vbNewLine & _
                        "#Fecha de última calibración de sonda: " & SondaSel.FechaCal & vbNewLine & _
                        "#Incertidumbre máxima de la sonda: " & SondaSel.IncertDB & vbNewLine & _
                        "#Factor: " & SondaSel.Factor
                    SW.Write(outHeader & vbNewLine)
                    ListaResultados.Items.Clear()
                    OverlayCarga.Clear()
                    txtEventos.Text &= "[" & Now & "] Recorrido de medición iniciado" & vbNewLine
                    btnConectar.Enabled = False
                    tmrCamp.Enabled = True
                    btnDetenerCamp.Enabled = True
                    btnIniciarCamp.Enabled = False
                    chkPausa.Enabled = True
                    CargarArchivoDePuntosToolStripMenuItem.Enabled = False
                End If
                'If chkEMR300.Checked Then NivelPuro = 0

                If chkPausa.Checked Then 'Do While chkPausa.Checked
                    lblCamp.Text = "Aguardando reanudación de recorrido"
                    lblCamp.BackColor = Color.Orange
                    ' Se pone a cero el nivel leido del narda para evitar
                    ' retención de valores sobre lugares que no se desea relevar
                    Do While chkPausa.Checked
                        NivelNarda = 0
                        lblDisplay.Text = "..."
                        Application.DoEvents()
                        If boolStopCamp Then Exit Do
                    Loop
                    NivelNarda = 0
                    lblDisplay.Text = "..."
                    If Not boolStopCamp Then
                        lblCamp.Text = "Reanudando en 5 segundos..."
                        Retardo(1000)
                        Application.DoEvents()
                        lblCamp.Text = "Reanudando en 4 segundos..."
                        Retardo(1000)
                        Application.DoEvents()
                        lblCamp.Text = "Reanudando en 3 segundos..."
                        Retardo(1000)
                        Application.DoEvents()
                        lblCamp.Text = "Reanudando en 2 segundos..."
                        Retardo(1000)
                        Application.DoEvents()
                        lblCamp.Text = "Reanudando en 1 segundo..."
                        Retardo(1000)
                        Application.DoEvents()
                    End If
                End If

                Do While Not boolStopCamp And Not StatusGpS
                    lblCamp.BackColor = Color.LightBlue
                    lblCamp.Text = "Aguardando determinar la posición geográfica actual"
                    If boolStopCamp Then Exit Do 'Va a revisar la condicion inicial y va a salir porque se detuvo la campaña
                    Application.DoEvents()
                Loop
                If boolStopCamp Then Exit Do

                lblCamp.BackColor = Color.GreenYellow
                lblCamp.Text = "Ejecutando medición"

                If opIntervalo.Checked Then
                    'lblDisplay.Text = "0 V/m"
                    Do Until tmrCamp.Enabled = False
                        'If chkEMR300.Checked = True And chkMaxHold.Checked = True Then
                        'If NivelNarda > NivelPuro Then NivelPuro = NivelNarda
                        'End If
                        Application.DoEvents()
                        If boolStopCamp Then GoTo Fin
                    Loop
                End If

                If Not PrimerBucle Then
                    Do
                        'DEBE SUPERAR LA DISTANCIA ESTABLECIDA EN EL CAMPO DE TEXTO
                        DistUltPunto = CalcularDist(UltimaLat, UltimaLng, LatitudActual, LongitudActual)
                        lblDistAct.Text = "Actual: " & Math.Round(DistUltPunto).ToString & " mts."
                        'If chkEMR300.Checked = True And chkMaxHold.Checked = True Then
                        'If NivelNarda > NivelPuro Then NivelPuro = NivelNarda
                        'End If

                        If opDistancia.Checked Then
                            If DistUltPunto > CSng(txtDistancia.Text) Then Exit Do
                            'La distancia que da como resultado está en METROS
                            lblCamp.Text = "Ejecutando - aguardando distancia seleccionada"
                            lblCamp.BackColor = Color.GreenYellow
                            Application.DoEvents()
                        Else
                            If ModoDebug.Checked Then
                                DistUltPunto = 10 'PARA PRUEBAS EN LABORATORIO DONDE NO HAY MOVIMIENTO
                                lblDistAct.Text = "-DEBUGGING-"
                            End If
                            ' SE ASEGURA SI O SI DE QUE NO SE GRABEN MEDICIONES EN EL MISMO PUNTO HASTA CAMBIAR DE POSICION (dist min = 1 metro)
                            If DistUltPunto > 1 Then Exit Do
                            lblCamp.Text = "Esperando cambio de posición"
                            lblCamp.BackColor = Color.LightBlue
                            Application.DoEvents()
                        End If
                        If boolStopCamp Then GoTo Fin
                    Loop 'Until DistUltPunto > 1
                    lblCamp.BackColor = Color.GreenYellow
                    lblCamp.Text = "Ejecutando medición"
                Else
                    OverlayResultados.Clear()
                    PrimerBucle = False
                End If

                IndiceRes += 1
                If chkEMR300.Checked = True Then
                    If chkMaxHold.Checked Then NivelPuro = NivMax
                Else
                    NivelPuro = NivelNarda
                End If
                'NivelPuro = NivelNarda

                Application.DoEvents()


                '----------------------------------------------------------------------------------------------'
                '-- PROBANDO DESCARTE DE PUNTOS QUE TENGAN EN SUS GRADOS DE LATITUD O LONGITUD EL VALOR CERO --'
                If LatitudActual.Grados.Equals(0) Or LongitudActual.Grados.Equals(0) Then
                    GoTo SeguirCampaña ' Si los grados estan en cero, vuelve al inicio. Como las banderas estan en orden, deberia volver a medir sin problemas
                End If
                '----------------------------------------------------------------------------------------------'



                ActLatGDEC = ConvertirAGDec(LatitudActual)
                ActLngGDEC = ConvertirAGDec(LongitudActual)
                'Se guarda en una variable local CON INCERTIDUMBRE INCLUIDA para que sea lo mas preciso posible en cuanto a posicion y tiempo
                NivelFinal = FormatNumber(NivelPuro * factorSonda, 3)

                If chkNBM550.Checked Then
                    comNarda.WriteLine("RESET_MMA;") 'RESETEA CUALQUIER TIPO DE VALOR EN EL INSTRUMENTO (MAX, MIN, AVG, MAX_AVG)
                    Retardo(300)
                    'Dim rta As String = comNarda.ReadExisting()
                    'txtEventos.Text &= "[" & Now & "] rta a RESET:" & rta & "." & vbNewLine
                    comNarda.DiscardInBuffer()
                    'ElseIf chkEMR300.Checked Then
                    'NivelNarda = 0
                End If
                boolResetMaxEMR = True
                Application.DoEvents()
                Select Case NivelFinal
                    Case Is >= 27.5
                        ColorMarker = GMarkerGoogleType.red
                        IndiceImg = 0
                    Case Is >= 20
                        ColorMarker = GMarkerGoogleType.orange_dot
                        IndiceImg = 1
                    Case Is >= 14
                        ColorMarker = GMarkerGoogleType.yellow_dot
                        IndiceImg = 2
                    Case Is >= 8
                        ColorMarker = GMarkerGoogleType.purple_dot
                        IndiceImg = 3
                        If NivelFinal > 14 Then ' 13.75 Then
                            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
                            txtEventos.Text &= "[" & Now & "] El punto " & IndiceRes & " (" & NivelFinal & " V/m). requiere ser evaluado bajo Res. CNC 3690/04" & vbNewLine
                        End If
                    Case Is >= 4
                        ColorMarker = GMarkerGoogleType.green_dot
                        IndiceImg = 4
                    Case Is >= 2
                        ColorMarker = GMarkerGoogleType.lightblue_dot
                        IndiceImg = 5
                    Case Else
                        ColorMarker = GMarkerGoogleType.blue_dot
                        IndiceImg = 6
                End Select
                If hayAlarma And NivelFinal >= NivelAlarma Then
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
                End If
                Dim Marker As GMarkerGoogle = New GMarkerGoogle(New PointLatLng(ActLatGDEC, ActLngGDEC), ColorMarker)
                Marker.ToolTipMode = MarkerTooltipMode.OnMouseOver

                Marker.ToolTipText = "Punto " & IndiceRes & vbNewLine & _
                    "Nivel c/incert.: " & NivelFinal & " " & UnidadActual & vbNewLine & _
                    "Nivel s/incert.: " & NivelPuro & " " & UnidadActual & vbNewLine & _
                    "Porcentaje de la MEP: " & Math.Round(((NivelFinal ^ 2) / 3770) * 100 / 0.2, 2).ToString & "%" & vbNewLine & _
                    Now & vbNewLine & _
                    LatitudActual.Grados & "° " & LatitudActual.Minutos & "' " & LatitudActual.Segundos & Chr(34) & " " & LatitudActual.Hemisf & vbNewLine & _
                    LongitudActual.Grados & "° " & LongitudActual.Minutos & "' " & LongitudActual.Segundos & Chr(34) & " " & LongitudActual.Hemisf

                OverlayResultados.Markers.Add(Marker)

                'Para calcular la distancia hacia el ultimo marker
                UltimaLat = LatitudActual
                UltimaLng = LongitudActual

                With outReg
                    .Indice = Format(IndiceRes, "0000")
                    .Nivel = NivelPuro 'sin incertidumbre
                    .Unidad = UnidadActual
                    .Fecha = Now.ToString("d")
                    .Hora = Now.TimeOfDay.ToString.Substring(0, 8)
                    .Lat = ActLatGDEC
                    .Lon = ActLngGDEC

                    outBuffer =
                        Format(.Indice, "0000") & "," & .Nivel.ToString.Replace(",", ".") & " " & .Unidad & "," & .Fecha & "," & .Hora & "," & _
                        .Lat.Replace(",", ".") & "," & .Lon.Replace(",", ".")
                End With

                SW.WriteLine(outBuffer)

                'La columna principal es este dato (el item en si)

                nReg = New ListViewItem(Format(IndiceRes, "0000"))
                With nReg
                    'Cada subitem es la columna 2 en adelante (los atributos del item)
                    .SubItems.Add(NivelFinal & " " & UnidadActual)  '2
                    .SubItems.Add(NivelPuro & " " & UnidadActual)   '3
                    .SubItems.Add(TimeOfDay)            '4
                    .SubItems.Add(Today)                '5
                    .SubItems.Add(LatitudActual.Grados & "° " & LatitudActual.Minutos & "' " & LatitudActual.Segundos & Chr(34) & " " & LatitudActual.Hemisf)       '6
                    .SubItems.Add(LongitudActual.Grados & "° " & LongitudActual.Minutos & "' " & LongitudActual.Segundos & Chr(34) & " " & LongitudActual.Hemisf)   '7
                    .SubItems.Add(Instrumento.Marca & " " & Instrumento.Modelo & " - " & Instrumento.NumSerie)  '8
                    .SubItems.Add(SondaSel.Marca & " " & SondaSel.Modelo & " - " & SondaSel.NumSerie)   '9
                    .SubItems.Add(SondaSel.FechaCal) '10
                    .SubItems.Add(SondaSel.IncertDB) '11
                    .SubItems.Add(SondaSel.Factor) '12
                End With
                'Agrega el item a la lista
                ListaResultados.Items.Add(nReg)
                'Asigna la imagen correspondiente al item segun el nivel detectado
                ListaResultados.Items(IndiceRes - 1).ImageIndex = IndiceImg
                'Tilda el checkbox del item para que se muestre en mapa
                ListaResultados.Items(IndiceRes - 1).Checked = True
                nReg.EnsureVisible()
Fin:
                tmrCamp.Enabled = True
            Loop    'Else

            If boolStopCamp Then
                SW.Close()
                boolStopCamp = False
                lblCamp.BackColor = Color.Silver
                lblCamp.Text = "Recorrido finalizado, esperando nuevas instrucciones"
                lblCargado.Text = "Recorrido finalizado, guardado en " & RutaRaizRes
                txtEventos.Text &= "[" & Now & "] Recorrido de medición finalizado con éxito por el usuario. Resultados guardados en """ & RutaRaizRes & """." & vbNewLine

                Dim SR As StreamReader = New StreamReader(RutaRaizRes)
                outCrypt = Criptografo.EncryptData(SR.ReadToEnd)
                SR.Close()
                SW = New StreamWriter(RutaRaizRes, False)
                SW.Write(outCrypt)
                SW.Close()

            Else
                MsgBox("No puede iniciarse un recorrido si no se encuentra conectado un instrumento de medición.", vbExclamation)
            End If
            'End If
        Catch ex As Exception
            Try
                SW.Close()
                boolStopCamp = False
                lblCamp.BackColor = Color.Silver
                lblCamp.Text = "Recorrido finalizado, esperando nuevas instrucciones"
                lblCargado.Text = "Recorrido finalizado, guardado en " & RutaRaizRes
                txtEventos.Text &= "[" & Now & "] Recorrido de medición finalizado con éxito por el usuario. Resultados guardados en """ & RutaRaizRes & """." & vbNewLine

                Dim SR As StreamReader = New StreamReader(RutaRaizRes)
                outCrypt = Criptografo.EncryptData(SR.ReadToEnd)
                SR.Close()
                SW = New StreamWriter(RutaRaizRes, False)
                SW.Write(outCrypt)
                SW.Close()
            Catch exc As Exception

            End Try
            If Not ex.Message.Contains("path") Then
                txtEventos.Text &= "[" & Now & "] EXCEPCION OCURRIDA DURANTE EL RECORRIDO: " & ex.Message & vbNewLine
            End If
        Finally
            btnIniciarCamp.Enabled = True
            btnDetenerCamp.Enabled = False
            btnConectar.Enabled = True
            chkPausa.Checked = False
            chkPausa.Enabled = False
            CargarArchivoDePuntosToolStripMenuItem.Enabled = True
            lblDistAct.Text = "Actual: --"
        End Try
    End Sub

    Private Sub frmMain_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try
            End
        Catch ex As Exception
            End
        Finally
            End
        End Try
    End Sub

    Private Sub cboProvMapa_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboProvMapa.SelectedIndexChanged
        Cursor = Cursors.WaitCursor
        PosicionCoor = Mapa.Position
        BuscoCoor = True
        UltimoZoomLvl = Mapa.Zoom
        SetProvMapa()
        If ArranqueApp Then
            ArranqueApp = False
        Else
            txtEventos.Text &= "[" & Now & "] " & "Proveedor de mapas cambiado a " & cboProvMapa.Text & vbNewLine
            ProcesoBuscar()
        End If
        Cursor = Cursors.Default
    End Sub

    Private Sub cboModoConexion_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboModoConexion.SelectedIndexChanged
        Try
            SetModoConexion()
            '-----------------------------------------------------------------------------
            If cboModoConexion.Text <> "Leer desde caché" Then
                Cursor = Cursors.WaitCursor
                txtEventos.Text &= "[" & Now & "] " & "Chequeando si existe conexión a internet..." & vbNewLine
                Application.DoEvents()

                '-----------------------------------------------------------------------------
                ' Configuracion para proxy 
                GMapProvider.WebProxy = WebRequest.GetSystemWebProxy
                'PIDE CREDENCIALES PARA USAR EN EL PROXY. SI NO HAY PROXY, PRESIONAR CANCELAR E IGNORAR ESTOS DATOS
                If user = "" Or pass = "" Then
                    frmLogin.ShowDialog()
                End If
                '-----------------------------------------------------------------------------
                'Testea si hay conexión a internet disponible tratando de acceder a la web de openstreetmap
                'Si no encuentra conexión, se setea en el catcher que se use el cache unicamente
                Dim d As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry("www.google.com")
                GMapProvider.WebProxy.Credentials = New NetworkCredential(user, pass)
                txtEventos.Text &= "[" & Now & "] " & "Conexión a internet detectada. Activadas las peticiones al servidor seleccionado" & vbNewLine
                opBuscarLugar.Enabled = True
                opBuscarCoor.Checked = True
                btnBuscar.Enabled = True
            End If
        Catch ex As Exception
            If ex.Message = "Host desconocido" Or ex.Message = "El nombre solicitado es válido pero no se encontraron datos del tipo solicitado" Then
                Mapa.Manager.Mode = AccessMode.CacheOnly
                txtEventos.Text &= "[" & Now & "] " & "No hay conexión a internet, estableciendo modo de conexión a ''Leer desde caché''" & vbNewLine
                cboModoConexion.Text = "Leer desde caché"
                opBuscarLugar.Enabled = False
                opBuscarCoor.Checked = False
                btnBuscar.Enabled = False
            End If
        Finally
            Cursor = Cursors.Arrow
        End Try

    End Sub

    Private Sub IrAInicio(sender As System.Object, e As System.EventArgs)
        Cursor = Cursors.WaitCursor
        If LatitudActual.Grados = 0 Then
            'PosicionStr = "Buenos Aires"
            'Posicionar(False)
            Dim coorlat, coorlon As CoordenadasGMS
            With coorlat
                .Grados = 40
                .Minutos = 14
                .Segundos = 0
                .Hemisf = "S"
            End With
            With coorlon
                .Grados = 63
                .Minutos = 16
                .Segundos = 42
                .Hemisf = "O"
            End With
            CoorAUbicar = New PointLatLng(ConvertirAGDec(coorlat), ConvertirAGDec(coorlon))
            Posicionar(True)
            Mapa.Zoom = 4
        Else
            PosicionCoor = New PointLatLng(ConvertirAGDec(LatitudActual), ConvertirAGDec(LongitudActual))
            UltimoZoomLvl = trkZoom.Value
            If Not ArranqueApp Then
                BuscoCoor = True
                ProcesoBuscar()
            End If
            ArranqueApp = False
            Mapa.Overlays.Clear()
            OverlayResultados.Markers.Clear()
            Dim MarkersOverlay As GMapOverlay = New GMapOverlay("Markers")
            Dim Marker As GMarkerGoogle = New GMarkerGoogle(New PointLatLng(ConvertirAGDec(LatitudActual), ConvertirAGDec(LongitudActual)), GMarkerGoogleType.green)
            MarkersOverlay.Markers.Add(Marker)
            Mapa.Overlays.Add(MarkersOverlay)

            OverlayResultados.Markers.Clear()
            Mapa.Overlays.Clear()
            Marker.ToolTipText = LatitudActual.Grados & "° " & LatitudActual.Minutos & "' " & Math.Round(LatitudActual.Segundos, 2) & Chr(34) & " " & LatitudActual.Hemisf & vbNewLine & _
                LongitudActual.Grados & "° " & LongitudActual.Minutos & "' " & Math.Round(LongitudActual.Segundos, 2) & Chr(34) & " " & LongitudActual.Hemisf

            Marker.ToolTipMode = MarkerTooltipMode.Always
            Mapa.Overlays.Add(OverlayResultados)
            'OverlayResultados.Markers.Add(Marker)


        End If
        Cursor = Cursors.Default
    End Sub

    Private Sub Mapa_LocationChanged(sender As Object, e As System.EventArgs) Handles Mapa.LocationChanged
        trkZoom.Value = Mapa.Zoom
        lblZoom.Text = Mapa.Zoom.ToString
    End Sub

    Private Sub BorrarMarker(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Mapa.MouseClick
        '----------------------------------------------------------------------------------------------------------
        'A IMPLEMENTAR: CON UN CLICK SOBRE UN MARKER ESTABLECIDO (MANUALMENTE, O NO), SE PODRÁ BORRARLO DEL OVERLAY

        ' El codigo a continuacion no tiene nada que ver con esto
        '----------------------------------------------------------------------------------------------------------


        'Dim PosIni As New Point(e.X, e.Y)

        'If e.Button = Windows.Forms.MouseButtons.Left Then
        'Dim LatEnGMS As CoordenadasGMS
        'Dim LngEnGMS As CoordenadasGMS
        'Dim MousePosLatLng As PointLatLng = Mapa.FromLocalToLatLng(e.X, e.Y)
        'UltimoPunto = MousePosLatLng
        '
        'LatEnGMS = ConvertirAGMS(UltimoPunto.Lat, False)
        'LngEnGMS = ConvertirAGMS(UltimoPunto.Lng, True)

        '- El overlay se hace en el global y por ahora usamos uno solo para todos los markers
        '- Proximamente, hay que hacer un overlay PARA CADA TIPO de objeto (marker, ruta, poligono, etc.)
        'OverlayClick.Markers.Clear()
        'Mapa.Overlays.Clear()
        'Dim NuevoMarker As GMapMarker = New GMarkerGoogle(UltimoPunto, GMarkerGoogleType.red)
        'NuevoMarker.ToolTipText = LatEnGMS.Grados & "° " & LatEnGMS.Minutos & "' " & Math.Round(LatEnGMS.Segundos, 2) & Chr(34) & " " & LatEnGMS.Hemisf & vbNewLine & _
        '    LngEnGMS.Grados & "° " & LngEnGMS.Minutos & "' " & Math.Round(LngEnGMS.Segundos, 2) & Chr(34) & " " & LngEnGMS.Hemisf

        'NuevoMarker.ToolTipMode = MarkerTooltipMode.OnMouseOver
        'Mapa.Overlays.Add(OverlayClick)
        'OverlayClick.Markers.Add(NuevoMarker)
        'End If
    End Sub

    Private Sub BotonBuscar(sender As System.Object, e As System.EventArgs) Handles btnBuscar.Click
        Cursor = Cursors.WaitCursor
        If opBuscarLugar.Checked = True Then
            PosicionStr = txtLugar.Text
            BuscoCoor = False
        Else
            If txtLat.Text.Contains(" ") Then 'Si hay espacios en el textbox, es porque son coordenadas GMS
                Dim LatConvertir, LngConvertir As CoordenadasGMS
                With LatConvertir
                    .Grados = Trim(txtLat.Text.Substring(0, 4))
                    .Minutos = Trim(txtLat.Text.Substring(4, 2))
                    .Segundos = Trim(CDbl(txtLat.Text.Substring(7, txtLat.TextLength - 7)))
                    If .Grados > 0 Then
                        .Hemisf = "N"
                    Else
                        .Hemisf = "S"
                    End If
                End With
                PosicionCoor.Lat = ConvertirAGDec(LatConvertir)
                With LngConvertir
                    .Grados = Trim(txtLng.Text.Substring(0, 4))
                    .Minutos = Trim(txtLng.Text.Substring(4, 2))
                    .Segundos = Trim(CSng(txtLng.Text.Substring(7, txtLat.TextLength - 7)))
                    If Math.Sign(.Grados) > 0 Then
                        .Hemisf = "E"
                    Else
                        .Hemisf = "O"
                    End If
                End With
                PosicionCoor.Lng = ConvertirAGDec(LngConvertir)
            Else 'Sino, las coordenadas estan en decimales....
                PosicionCoor = New PointLatLng(CDbl(txtLat.Text), CDbl(txtLng.Text))
            End If
            BuscoCoor = True
        End If
        ProcesoBuscar()
        'Mapa.Overlays.Clear()
        'OverlayResultados.Markers.Clear()
        Dim LatEnGMS, LngEnGMS As CoordenadasGMS
        'Dim MarkersOverlay As GMapOverlay = New GMapOverlay("Markers")
        Dim Marker As GMarkerGoogle = New GMarkerGoogle(Mapa.Position, GMarkerGoogleType.yellow)

        'Mapa.Overlays.Add(MarkersOverlay)

        LatEnGMS = ConvertirAGMS(Mapa.Position.Lat, False)
        LngEnGMS = ConvertirAGMS(Mapa.Position.Lng, True)
        'OverlayResultados.Markers.Clear()
        'Mapa.Overlays.Clear()
        Dim TituloMarker As String
        If BuscoCoor = False Then
            TituloMarker = txtLugar.Text
        Else
            TituloMarker = "Coordenadas:"
        End If
        Marker.ToolTipText = TituloMarker & vbNewLine & vbNewLine & LatEnGMS.Grados & "° " & LatEnGMS.Minutos & "' " & Math.Round(LatEnGMS.Segundos, 2) & Chr(34) & " " & LatEnGMS.Hemisf & vbNewLine & _
            LngEnGMS.Grados & "° " & LngEnGMS.Minutos & "' " & Math.Round(LngEnGMS.Segundos, 2) & Chr(34) & " " & LngEnGMS.Hemisf

        Marker.ToolTipMode = MarkerTooltipMode.Always

        OverlayBusqueda.Markers.Add(Marker)

        'OverlayResultados.Markers.Add(Marker)
        Cursor = Cursors.Default
    End Sub

    Private Sub SeleccionadoBuscarCoor(sender As System.Object, e As System.EventArgs) Handles opBuscarCoor.CheckedChanged
        btnBuscar.Text = "Buscar coordenadas"
        BuscoCoor = True

        tTip.SetToolTip(btnBuscar, "Ingrese las coordenadas que desea buscar de las siguientes formas posibles:" & vbNewLine & vbNewLine & _
                        "   -34.609327" & vbNewLine & _
                        "ó" & vbNewLine & _
                        "   -34 36 33")
    End Sub

    Private Sub SeleccionadoBuscarLugar(sender As System.Object, e As System.EventArgs) Handles opBuscarLugar.CheckedChanged
        btnBuscar.Text = "Buscar lugar"
        BuscoCoor = False

        tTip.SetToolTip(btnBuscar, "Ingrese el lugar que desea buscar y presione este botón." & vbNewLine & vbNewLine & _
                        "Ejemplos:" & vbNewLine & _
                        "  ''Avellaneda, Buenos Aires''" & vbNewLine & _
                        "  ''Misiones, Argentina''" & vbNewLine & _
                        "  ''Obelisco''")
    End Sub

    Private Sub CambioTrkZoom(sender As System.Object, e As System.EventArgs) Handles trkZoom.Scroll
        Mapa.Zoom = trkZoom.Value
        lblZoom.Text = Mapa.Zoom.ToString
    End Sub

    Private Sub txtLat_GotFocus(sender As Object, e As System.EventArgs) Handles txtLat.GotFocus
        SeleccionadoBuscarCoor(opBuscarCoor, e)
        opBuscarCoor.Checked = True
    End Sub

    Private Sub txtLng_GotFocus(sender As Object, e As System.EventArgs) Handles txtLng.GotFocus
        SeleccionadoBuscarCoor(opBuscarCoor, e)
        opBuscarCoor.Checked = True
    End Sub

    Private Sub txtLugar_GotFocus(sender As Object, e As System.EventArgs) Handles txtLugar.GotFocus
        SeleccionadoBuscarLugar(opBuscarLugar, e)
        opBuscarLugar.Checked = True
    End Sub

    Private Sub Mapa_MouseWheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Mapa.MouseWheel
        Try
            trkZoom.Value = Mapa.Zoom
            lblZoom.Text = Mapa.Zoom.ToString
        Catch
        End Try
    End Sub

    Private Sub NARDAEMR300ToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles chkEMR300.Click
        chkNBM550.Checked = False
        chkEMR300.Checked = True
        lblSondaDetect.Text = "Sonda seleccionada:"
        frmSondaEMR.Show(Me)
    End Sub

    Private Sub chkNBM550_Click(sender As System.Object, e As System.EventArgs) Handles chkNBM550.Click
        chkEMR300.Checked = False
        chkNBM550.Checked = True
        chkMaxAvg.Enabled = True
        lblSondaDetect.Text = "Sonda detectada:"
    End Sub

    Private Sub ConectarNarda(sender As System.Object, e As System.EventArgs) Handles btnConectar.Click
        Try
            Dim Rta As String
            If Not chkNBM550.Checked And Not chkEMR300.Checked Then
                MsgBox("Seleccione en el menú ''Opciones -> Instrumento RNI'' el modelo del equipo que desea utilizar", MsgBoxStyle.Exclamation, "Seleccione un equipo antes de continuar")
                Exit Sub
            End If
            With comNarda
                btnConectar.Enabled = False
                If Not Conectado Then
                    btnConectar.Text = "Conectando..."
                    'SE DESHABILITA PODER CAMBIAR EL TIPO DE RESULTADO MIENTRAS EL EQUIPO ENVIE INFORMACION
                    'HAY QUE DESCONECTAR, SELECCIONAR EL DESEADO Y VOLVER A CONECTAR PARA CAMBIARLO
                    chkMaxAvg.Enabled = False
                    chkMaxHold.Enabled = False
                    If chkNBM550.Checked Then
                        ' ACA SE VA A SELECCIONAR EL VEHICULO QUE SE VA A UTILIZAR, ESTO DETERMINA EL ARCHIVO DE SONDA YA QUE CAMBIAN LAS INCERTIDUMBRES

                        frmSondaNBM.ShowDialog(Me)

                        '----------------------------------------
                        'CONFIGURACION DEL PUERTO COM PARA NBM550
                        '----------------------------------------
                        .BaudRate = BaudRate '115200 para óptico, 460800 para USB
                        .DataBits = 8
                        .Parity = Ports.Parity.None
                        .StopBits = Ports.StopBits.One
                        .Handshake = Ports.Handshake.None
                        '----------------------------------------
                        '----------------------------------------
                        If Not .IsOpen Then .Open()
                        .DiscardInBuffer()
                        .WriteLine("REMOTE ON;")
                        Retardo(100)
                        Rta = .ReadExisting
                        If Rta <> "0;" & vbCr & "" And Not Rta.Contains("401;") Then
                            Beep()
                            txtEventos.Text &= "[" & Now & "] " & "El instrumento no responde, está apagado o no se encuentra conectado" & vbNewLine
                            chkMaxAvg.Enabled = True
                            chkMaxHold.Enabled = True
                            btnIniciarCamp.Enabled = False
                            btnConectar.Text = "Conectar"
                            Exit Sub
                        Else
                            txtEventos.Text &= "[" & Now & "] " & "NBM-550 conectado. Activando el modo REMOTO." & vbNewLine
                        End If
                        PictureBox1.Image = AutoMapRNI.My.Resources.nbm550
                        .WriteLine("DEVICE_INFO?;")
                        Retardo(200)
                        Rta = .ReadExisting
                        Dim Vec1() As String = Rta.Split(separadores, StringSplitOptions.None)
                        lblInstrumento.Text = Vec1(0).Trim(trimmers) & " - S/N: " & Vec1(2).Trim(trimmers)
                        With Instrumento
                            .Marca = "NARDA"
                            .Modelo = Vec1(0).Trim(trimmers)
                            .NumSerie = Vec1(2).Trim(trimmers)
                        End With
                        Rta = vbNullChar
                        .WriteLine("PROBE_INFO?;")
                        Retardo(200)
                        Rta = .ReadExisting
                        Dim Vec2() As String = Rta.Split(separadores, StringSplitOptions.None)
                        lblSonda.Text = Vec2(0).Trim(trimmers) & " - S/N: " & Vec2(2).Trim(trimmers)
                        With SondaSel
                            .Marca = "NARDA"
                            .Modelo = Vec2(0).Trim(trimmers)
                            .NumSerie = Vec2(2).Trim(trimmers)
                            Dim auxfecha As String = Vec2(3).Trim.Replace(".", "/")
                            auxfecha = auxfecha.Substring(3, 3) & auxfecha.Substring(0, 3) & auxfecha.Substring(6, 2)

                            '>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                            '.FechaCal = Convert.ToDateTime(auxfecha) 'auxfecha.ToString("dd/mm/yyyy") 
                            .FechaCal = auxfecha
                            '>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                            '- ACA SE BUSCA QUE MODELO DE SONDA ES PARA AGREGARLE EL FACTOR
                            '- SE DISCRIMINA SEGUN ESTE SELECCIONADA LA KANGOO O LA SPRINTER
                            For i As Integer = 0 To ContSondas500
                                If Not boolSprinter Then
                                    If Sondas550(i).Nombre = .Modelo Then
                                        .IncertDB = Sondas550(i).Incert
                                        .Factor = Sondas550(i).VecesDB
                                        Exit For
                                    End If
                                Else
                                    If Sondas550_SP(i).Nombre = .Modelo Then
                                        .IncertDB = Sondas550_SP(i).Incert
                                        .Factor = Sondas550_SP(i).VecesDB
                                        Exit For
                                    End If
                                End If

                            Next
                            If .Factor = 1 Then
                                lblIncert.Text = "-NC-"
                            Else
                                lblIncert.Text = .IncertDB & " dB"
                            End If
                        End With
                        Select Case SondaSel.Modelo.Contains("EF")
                            Case True
                                UnidadActual = "V/m"
                                .WriteLine("RESULT_UNIT V/m;")
                            Case False
                                UnidadActual = "A/m"
                                .WriteLine("RESULT_UNIT A/m;")
                        End Select
                        Retardo(100)
                        .DiscardInBuffer()
                        If chkMaxHold.Checked = True Then
                            .WriteLine("RESULT_TYPE MAX;")
                            Retardo(100)
                            .DiscardInBuffer()
                            .WriteLine("RESET_MAX;") 'RESETEA EL VALOR MAXIMO DEL DISPLAY
                            lblTipoRes.Text = "Max Hold"
                        ElseIf chkMaxAvg.Checked = True Then
                            .WriteLine("RESULT_TYPE MAX_AVG;")
                            Retardo(100)
                            .DiscardInBuffer()
                            .WriteLine("RESET_MAXAVG;")
                            Retardo(100)
                            lblTipoRes.Text = "Max Avg"
                            .DiscardInBuffer()
                            .WriteLine("AVG_TIME 3;") 'SETEO DE TIEMPO DE PROMEDIADO en segundos
                        End If
                        Retardo(100)
                        .DiscardInBuffer()
                        .WriteLine("MEAS_VIEW NORMAL;")
                        Retardo(100)
                        .DiscardInBuffer()
                        .WriteLine("AUTO_ZERO OFF;") 'APAGA EL AUTOCERO 
                        Retardo(100)
                        .DiscardInBuffer()
                        'If Not SondaSel.Modelo.Contains("0391") Then
                        ' MsgBox("El protocolo de medición exige que las muestras se efectúen con la sonda EF-0391 para campo eléctrico. De otra manera, los resultados no serán válidos." _
                        '    , MsgBoxStyle.Exclamation, "Sonda no apta para medición bajo protocolo")
                        'txtEventos.Text &= "[" & Now & "] " & "La sonda detectada no es apta para medición bajo protocolo!" & vbNewLine
                        'End If

                    ElseIf chkEMR300.Checked Then
                        '----------------------------------------
                        'CONFIGURACION DEL PUERTO COM PARA EMR300
                        '----------------------------------------
                        .BaudRate = 4800
                        .DataBits = 8
                        .Parity = Ports.Parity.None
                        .StopBits = Ports.StopBits.One
                        .Handshake = Ports.Handshake.XOnXOff
                        '----------------------------------------
                        '----------------------------------------
                        If Not .IsOpen Then .Open()
                        .DiscardInBuffer()
                        .WriteLine("*IDN?")
                        Retardo(500)
                        Rta = .ReadExisting
                        If Not Rta.Contains("EMR-300") Then
                            Beep()
                            txtEventos.Text &= "[" & Now & "] " & "El instrumento no responde o no se encuentra conectado" & vbNewLine
                            btnIniciarCamp.Enabled = False
                            Exit Sub
                        Else
                            txtEventos.Text &= "[" & Now & "] " & "EMR-300 conectado." & vbNewLine
                        End If
                        Dim Vec1() As String = Rta.Split(separadores, StringSplitOptions.None)
                        lblInstrumento.Text = Vec1(1).Trim(trimmers) & " - S/N: " & Vec1(2).Trim(trimmers)
                        With Instrumento
                            .Marca = "NARDA"
                            .Modelo = Vec1(1).Trim(trimmers)
                            .NumSerie = Vec1(2).Trim(trimmers)
                        End With

                        PictureBox1.Image = AutoMapRNI.My.Resources.emr300
                        .WriteLine("CU E_Field") 'Setear V/m
                        Retardo(100)
                        Rta = .ReadExisting()

                        If chkMaxHold.Checked = True Then
                            lblTipoRes.Text = "Max Hold"
                        ElseIf chkActual.Checked = True Then
                            lblTipoRes.Text = "Actual"
                        End If
                        .WriteLine("FAST:MODE ON") 'Modo de respuesta rápida (en intervalos de 400 ms) y desactiva avg y max
                        Retardo(100)
                        Rta = .ReadExisting()

                        .WriteLine("CAX EFF") 'Setear INTEGRAR EJES
                        Retardo(100)
                        .DiscardInBuffer()
                        Try
                            Select Case ProcesoEMRMax.ThreadState
                                Case ThreadState.Stopped
                                    ProcesoEMRMax = Nothing
                                    ProcesoEMRMax = New Thread(AddressOf t_EMRMAX)
                                    Application.DoEvents()
                                    ProcesoEMRMax.Start()
                                Case ThreadState.Aborted
                                    ProcesoEMRMax = Nothing
                                    ProcesoEMRMax = New Thread(AddressOf t_EMRMAX)
                                    Application.DoEvents()
                                    ProcesoEMRMax.Start()
                                Case ThreadState.Unstarted
                                    ProcesoEMRMax.Start()
                                Case ThreadState.AbortRequested
                                    While ProcesoGPS.IsAlive
                                        Application.DoEvents()
                                    End While
                                Case ThreadState.WaitSleepJoin
                                    ProcesoEMRMax.Abort()
                                    While ProcesoEMRMax.IsAlive
                                        Application.DoEvents()
                                    End While
                            End Select
                        Catch ThrEx As ThreadStartException
                            MsgBox(ThrEx.Message)
                        End Try
                    End If
                    'Se pone en true la bandera de conectado para que cuando se vuelva a presionar este boton
                    'se "desconecte" el equipo
                    Conectado = True
                    tmrNarda.Interval = 1500
                    tmrNarda.Enabled = True
                    btnConectar.Text = "Desconectar"
                    btnIniciarCamp.Enabled = True
                    InstrumentoRNIToolStripMenuItem.Enabled = False
                    chkNBM550.Enabled = False
                    chkEMR300.Enabled = False
                Else
                    tmrNarda.Enabled = False
                    If chkNBM550.Checked Then
                        .WriteLine("AUTO_ZERO ON;")
                        .DiscardInBuffer()
                        .WriteLine("REMOTE OFF;")
                        .DiscardInBuffer()
                        txtEventos.Text &= "[" & Now & "] " & "Se desconectó correctamente el instrumento. Apagando el modo REMOTO." & vbNewLine
                        With SondaSel
                            .Marca = ""
                            .Modelo = ""
                            .NumSerie = ""
                        End With
                        lblSonda.Text = ""
                    Else
                        'NO DEBERIA SER NECESARIO SUSPENDER T_EMRMAX, YA QUE AL TERMINAR EL LOOP EL THREAD MUERE (usando boolKillThrMaxEMR)
                        boolKillThrMaxEMR = True
                        .WriteLine("AZ ON")
                        Retardo(100)
                        .DiscardInBuffer()
                        .WriteLine("FAST:MODE OFF")
                        Retardo(100)
                        .DiscardInBuffer()
                        txtEventos.Text &= "[" & Now & "] " & "Se desconectó correctamente el instrumento." & vbNewLine
                    End If
                    picBateria.Visible = False
                    lblInstrumento.Text = ""
                    lblTipoRes.Text = ""
                    PictureBox1.Image = Nothing
                    With Instrumento
                        .Marca = ""
                        .Modelo = ""
                        .NumSerie = ""
                    End With
                    lblDisplay.Text = ""
                    lblIncert.Text = ""
                    Conectado = False
                    btnConectar.Text = "Conectar"
                    'chkMaxAvg.Enabled = True
                    'chkMaxHold.Enabled = True
                    If .IsOpen Then .Close()
                    chkNBM550.Enabled = True
                    chkEMR300.Enabled = True
                End If
                InstrumentoRNIToolStripMenuItem.Enabled = True
            End With
        Catch ex As Exception
            If ex.Message.Contains("no existe") Then
                txtEventos.Text &= "[" & Now & "] " & "Error con instrumento - " & ex.Message & vbNewLine
            ElseIf ex.Message.Contains("Se ha denegado el acceso al puerto 'COM2'.") Then
                txtEventos.Text &= "[" & Now & "] " & "Error con instrumento - " & ex.Message & " (El puerto está siendo utilizado por otro instrumento o dispositivo)" & vbNewLine
            ElseIf ex.Message.Contains("no es correcto") Then
                txtEventos.Text &= "[" & Now & "] " & "Error con puerto seleccionado: " & ex.Message & vbNewLine
            Else
                txtEventos.Text &= "[" & Now & "] " & "EXCEPCION OCURRIDA DURANTE LA CONEXIÓN: " & ex.Message & vbNewLine
            End If
            Beep()
            tmrNarda.Enabled = False
            PictureBox1.Image = Nothing
            btnIniciarCamp.Enabled = False
            lblDisplay.Text = ""
            Conectado = False
            btnConectar.Text = "Conectar"
            'chkMaxAvg.Enabled = True
            'chkMaxHold.Enabled = True
            If comNarda.IsOpen Then comNarda.Close()
        Finally
            btnConectar.Enabled = True
        End Try
    End Sub

    Private Sub txtEventos_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtEventos.TextChanged
        txtEventos.Select(txtEventos.Text.Length, 0)
        txtEventos.ScrollToCaret()
    End Sub

    Private Sub cboPuertoNarda_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboPuertoNarda.SelectedIndexChanged
        If comNarda.IsOpen Then comNarda.Close()
        If cboPuertoNarda.Text <> "" Then
            comNarda.PortName = cboPuertoNarda.Text
            PrimerMuestreo = False
        End If
    End Sub

    Private Sub cboIntervalo_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboIntervalo.SelectedIndexChanged
        tmrCamp.Interval = CInt(cboIntervalo.SelectedItem) * 1000 'segundos a milisegundos para setear en el timer de campaña
    End Sub

    Private Sub btnGEarth_Click(sender As System.Object, e As System.EventArgs) Handles btnGEarth.Click
        'Dim Color As String
        Dim Punto As Registro
        Dim i As Integer
        Dim Header1 As String
        Dim Header2 As String
        'Dim Icon1 As String
        Dim Icon11 As String
        'Dim IconAdy As String
        Dim PlaceMrk1 As String
        Dim PlaceMrk2 As String
        Dim PlaceMrk3 As String
        Dim PlaceMrk4 As String
        Dim PlaceMrk5 As String
        Dim ScreenOverlay As String
        Dim Footer As String
        Dim RutaKML As String
        'Dim Intensidad(0 To 9) As String ' Aca se definen las gamas de colores
        'Dim VecPunto() As String
        'Dim VecPunto2() As String
        'Dim VecPunto3() As String
        Dim Latit() As String
        Dim Longit() As String
        'Dim Latit2() As String
        'Dim Longit2() As String
        Dim Nombre As String
        'Dim Ruta As String
        Dim InputText As String
        'Dim InputText2 As String
        'Dim InputText3 As String
        Dim OutputText As String
        'Dim Resta As Double
        Dim TipoResultado As String 'Depende de la extension del archivo cargado/creado en el ultimo recorrido

        Try
            If ListaResultados.Items.Count = 0 Then
                MsgBox("Antes de crear un archivo .kml debe cargar un archivo de resultados .mrni el sistema.", MsgBoxStyle.Exclamation)
                Exit Sub
            Else
                If RutaRaizRes.EndsWith("mrni") Then
                    TipoResultado = "Valor máximo detectado (Max Hold)"
                Else
                    TipoResultado = "Máximo de promedios (Max Average)"
                End If
            End If
            sDialog.FileName = ""
            sDialog.Filter = "Capa de puntos Google Earth (*.kmz)|*.kmz"
            If sDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            Else
                RutaKML = sDialog.FileName
                RutaKML = RutaKML.Substring(0, Len(RutaKML) - 3) & "kml" 'Se creará en realidad un KML para después comprimirlo
                RutaRaizKML = RutaKML.Substring(0, RutaKML.LastIndexOf("\") + 1)
            End If
            ' Setear nombre de archivo .kml
            Nombre = InputBox("Ingrese el nombre que desea asignarle a la capa (se mostrará dentro de Google Earth):", "Nombre de capa")
            If Nombre = vbNullString Then
                MsgBox("Se canceló la creación del archivo .kml", vbExclamation, "KML cancelado")
                Exit Sub
            End If
            ' Indicadores para el color de icono que se usaran google earth para saber que color asignar
            'Intensidad(0) = "#MenorMin"
            'Intensidad(1) = "#NivelUno"
            'Intensidad(2) = "#NivelDos"
            'Intensidad(3) = "#NivelTres"
            'Intensidad(4) = "#NivelCuatro"
            'Intensidad(5) = "#NivelCinco"
            'Intensidad(6) = "#Maximo"
            'Intensidad(0) = "#Maximo"
            'Intensidad(1) = "#NivelCinco"
            'Intensidad(2) = "#NivelCuatro"
            'Intensidad(3) = "#NivelTres"
            'Intensidad(4) = "#NivelDos"
            'Intensidad(5) = "#NivelUno"
            'Intensidad(6) = "#MenorMin"

            Using SR As New StringReader(AutoMapRNI.My.Resources.header_1.ToString)
                ' Asignar a variables los pedazos de código que se van a usar mas adelante
                InputText = SR.ReadLine
                Do Until InputText = Nothing
                    Header1 &= InputText & vbNewLine
                    InputText = SR.ReadLine
                Loop
            End Using

            Using SR As New StringReader(AutoMapRNI.My.Resources.header_2)
                InputText = SR.ReadLine
                Do Until InputText = Nothing
                    Header2 &= InputText & vbNewLine
                    InputText = SR.ReadLine
                Loop
            End Using

            Using SR As New StringReader(AutoMapRNI.My.Resources.icon_11)
                InputText = SR.ReadLine
                Do Until InputText = Nothing
                    Icon11 &= InputText & vbNewLine
                    InputText = SR.ReadLine
                Loop
            End Using

            Using SR As New StringReader(AutoMapRNI.My.Resources.placemark_1)
                InputText = SR.ReadLine
                Do Until InputText = Nothing
                    PlaceMrk1 &= InputText & vbNewLine
                    InputText = SR.ReadLine
                Loop
            End Using

            Using SR As New StringReader(AutoMapRNI.My.Resources.placemark_2)
                InputText = SR.ReadLine
                Do Until InputText = Nothing
                    PlaceMrk2 &= InputText & vbNewLine
                    InputText = SR.ReadLine
                Loop
            End Using

            Using SR As New StringReader(AutoMapRNI.My.Resources.placemark_3)
                InputText = SR.ReadLine
                Do Until InputText = Nothing
                    PlaceMrk3 &= InputText & vbNewLine
                    InputText = SR.ReadLine
                Loop

            End Using

            Using SR As New StringReader(AutoMapRNI.My.Resources.placemark_4)
                InputText = SR.ReadLine
                Do Until InputText = Nothing
                    PlaceMrk4 &= InputText & vbNewLine
                    InputText = SR.ReadLine
                Loop
            End Using

            Using SR As New StringReader(AutoMapRNI.My.Resources.placemark_5)
                InputText = SR.ReadLine
                Do Until InputText = Nothing
                    PlaceMrk5 &= InputText & vbNewLine
                    InputText = SR.ReadLine
                Loop
            End Using

            Using SR As New StringReader(AutoMapRNI.My.Resources.screenoverlay)
                InputText = SR.ReadLine
                Do Until InputText = Nothing
                    If InputText.StartsWith("<href>") Then
                        InputText = "<href>files\Escala.png</href>"
                    End If
                    ScreenOverlay &= InputText & vbNewLine
                    InputText = SR.ReadLine
                Loop
            End Using

            Using SR As New StringReader(AutoMapRNI.My.Resources.footer)
                InputText = SR.ReadLine
                Do Until InputText = Nothing
                    Footer &= InputText & vbNewLine
                    InputText = SR.ReadLine
                Loop
            End Using

            'FileOpen(1, RutaKML, OpenMode.Output)
            'Using SW As StreamWriter = New StreamWriter(RutaKML)
            Using SW As New StreamWriter(RutaKML)
                SW.AutoFlush = True
                'FileOpen(2, txtruta, OpenMode.Input)
                'Open txtRuta For Input As #2

                ' Escribir encabezado(header) y codigo de colores de icono en el archivo
                OutputText = Header1 & Nombre & Header2 & Icon11
                SW.WriteLine(OutputText)
                'Print(1, OutputText)
                Dim cont As Integer = 1
                Dim strInstrumento, strSonda As String
                For Each item As ListViewItem In ListaResultados.Items
                    With Punto
                        Dim aux() As String
                        '.Indice = CInt(item.Text) 'VecPunto(0).Substring(Len(VecPunto(0)) - 5, 5) 'Right(VecPunto(0), 5)
                        .Fecha = item.SubItems.Item(4).Text 'VecPunto(1)
                        .Hora = item.SubItems.Item(3).Text.Substring(0, 8) 'Format(VecPunto(2), "hh:mm:ss")
                        .Lat = item.SubItems.Item(5).Text 'VecPunto(4).Substring(0, 11) 'Left(VecPunto(4), 11)
                        .Lon = item.SubItems.Item(6).Text 'VecPunto(5).Substring(0, 11) 'Left(VecPunto(5), 11)
                        aux = Split(item.SubItems.Item(1).Text)
                        'aux = Split(VecPunto(1), " ")
                        .Nivel = CSng(aux(0)) '/ 1000     'ES EL VALOR CON INCERTIDUMBRE INCLUIDA
                        .Unidad = "V/m" 'VecPunto(7).Substring(0, 4) 'Left(VecPunto(7), 4)
                        .PorcentMEP = Math.Round(((.Nivel ^ 2) / 3770) * 100 / 0.2, 4)
                        aux = Split(item.SubItems.Item(2).Text)
                        .NivelPuro = CSng(aux(0))   'SIN LA INCERTIDUMBRE INCLUIDA
                    End With
                    strInstrumento = item.SubItems.Item(7).Text
                    strSonda = item.SubItems.Item(8).Text

                    'Separar en tres posiciones de un vector dinamico los grados, min y seg de cada coordenada
                    Latit = Split(Punto.Lat, " ", 3) ' Separacion entre numeros: 1 espacio
                    Longit = Split(Punto.Lon, " ", 3) ' Separacion entre numeros: 1 espacio

                    'Acotar los valores a los primeros dos caracteres (se eliminan espacios y signos de unidades)
                    For i = 0 To 1
                        If Len(Latit(i)) > 2 Then
                            Latit(i) = Latit(i).Substring(0, 2) 'Left(Latit(i), 2)
                        Else
                            Latit(i) = Latit(i).Substring(0, 1)
                        End If
                        If Len(Longit(i)) > 2 Then
                            Longit(i) = Longit(i).Substring(0, 2) 'Left(Longit(i), 2)
                        Else
                            Longit(i) = Longit(i).Substring(0, 1) 'Left(Longit(i), 2)
                        End If
                    Next i

                    Latit(2) = Latit(2).Substring(0, Len(Latit(2)) - 3) 'Latit(2).Substring(0, 5).TrimEnd(trimmers)
                    Latit(2) = CSng(Latit(2))
                    Longit(2) = Longit(2).Substring(0, Len(Longit(2)) - 3) 'Longit(2).Substring(0, 5).TrimEnd(trimmers)
                    Longit(2) = CSng(Longit(2))

                    'Se convierte de coordenadas en grados, min y seg a grados decimales
                    'Se antepone el signo "-" en los grados porque siempre se va a utilizar en el hemisferio sur-occidental

                    'CHEQUEAR ESTO CUANDO SE PUEDA, HAY QUE HACERLO AUTOMATIZADO Y NO PRESUPONER NADA
                    With Punto
                        .Lat = (-CSng(Latit(0))) - (CSng(Latit(1)) / 60) - (CSng(Latit(2)) / 3600)
                        .Lon = (-CSng(Longit(0))) - (CSng(Longit(1)) / 60) - (CSng(Longit(2)) / 3600)
                        .Lat = Replace(Punto.Lat, ",", ".")
                        .Lon = Replace(Punto.Lon, ",", ".")
                    End With

                    ' Se establece la intensidad (color de icono) del punto segun el nivel que se haya medido ahi
                    ' Los valores en cuestión se definen en la magnitud de dBuV

                    '---------------------------------------------------------------------------------------------------------
                    '>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    '>>>>>>>>>>>> DEFINIR ACA LOS VALORES QUE DETERMINAN DONDE HAY ACTIVIDAD O SOLO PISO DE RUIDO <<<<><<<<<<<
                    '|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
                    'VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV
                    'Intensidad(0) = "#Maximo"
                    'Intensidad(1) = "#NivelCinco"
                    'Intensidad(2) = "#NivelCuatro"
                    'Intensidad(3) = "#NivelTres"
                    'Intensidad(4) = "#NivelDos"
                    'Intensidad(5) = "#NivelUno"
                    'Intensidad(6) = "#MenorMin"
                    'Select Case Punto.Nivel 'CON INCERTIDUMBRE
                    'Case Is >= 27.5
                    '    i = 0
                    'Case Is >= 20
                    '    i = 1
                    'Case Is >= 14
                    '    i = 2
                    'Case Is >= 8
                    '    i = 3
                    'Case Is >= 4
                    '    i = 4
                    'Case Is >= 2
                    '    i = 5
                    'Case Else
                    '    i = 6
                    'End Select
                    Select Case Punto.PorcentMEP
                        Case Is <= 1
                            i = 1
                        Case Is <= 2
                            i = 2
                        Case Is <= 4
                            i = 3
                        Case Is <= 8
                            i = 4
                        Case Is <= 15
                            i = 5
                        Case Is <= 20
                            i = 6
                        Case Is <= 35
                            i = 7
                        Case Is <= 50
                            i = 8
                        Case Is <= 100
                            i = 9
                        Case Else
                            i = 10
                    End Select
                    Dim Intensidad(10) As String
                    For n = 1 To 10
                        Intensidad(n) = "#Nivel" & n
                    Next
                    Punto.Color = Intensidad(i)
                    '---------------------------------------------------------------------------------------------------------
                    '>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    '---------------------------------------------------------------------------------------------------------

                    'Crear (escribir) el codigo de punto
                    'EL NIVEL EXPRESADO ES EL QUE CONTIENE LA INCERTIDUMBRE
                    With Punto
                        OutputText = PlaceMrk1 & PlaceMrk2 & .Color & PlaceMrk3 & .Lon & "," & .Lat & _
                                     PlaceMrk4 & vbNewLine & _
                                     "Fecha: " & .Fecha & vbNewLine & _
                                     "Hora: " & .Hora & vbNewLine & vbNewLine & _
                                     "Porcentaje MEP mas estricta: " & .PorcentMEP.ToString & "%" & vbNewLine & _
                                     "Tipo de resultado: " & TipoResultado & " sobre móvil" & vbNewLine & _
                                     "Instrumento - N°/S: " & strInstrumento & vbNewLine & _
                                     "Sonda - N°/S: " & strSonda & PlaceMrk5
                        'Math.Round(((.Nivel ^ 2) / 3770) * 100 / 0.2, 2).ToString & "%" & vbNewLine & _
                        ' "<u>Fecha:</u> " & .Fecha & vbNewLine & _
                        '"<u>Hora:</u> " & .Hora & vbNewLine & vbNewLine & _
                        '"<b>Nivel:</b> " & .Nivel & " V/m" & vbNewLine & _
                        '"<b>Porcentaje de la MEP:</b> " & Math.Round(.Nivel * 100 / 27.5, 2).ToString & "%" & vbNewLine & _
                        '"<u>Tipo de resultado:</u> " & TipoResultado & vbNewLine & _
                        '"<u>Instrumento - N°/S:</u> " & strInstrumento & vbNewLine & _
                        '"<u>Sonda - N°/S:</u> " & strSonda & PlaceMrk5
                    End With
                    'Print #1, OutputText
                    SW.WriteLine(OutputText)
                    cont += 1
                Next
                'Loop1

                ' Escribir el segmento para añadir la imagen de la escala cromatica
                SW.WriteLine(ScreenOverlay)

                ' Escribir terminador(footer) del archivo
                'Print #1, Footer
                SW.WriteLine(Footer)
                'Close()
            End Using

            Using zip As ZipFile = New ZipFile
                zip.AddFile(RutaKML, "")
                zip.AddItem(Application.StartupPath & "/files", "files")
                zip.Save(RutaRaizKML & Nombre & ".kmz")
            End Using

            If File.Exists(RutaKML) Then
                File.Delete(RutaKML)
            End If

            txtEventos.Text &= "[" & Now & "] " & "Se ha creado el archivo de puntos en ''" & RutaKML & "''." & vbNewLine
        Catch ex As Exception
            MsgBox(ex.Message)
            txtEventos.Text &= "[" & Now & "] " & "Excepción al crear KMZ: " & ex.Message & vbNewLine
        End Try
    End Sub

    Private Sub ExportarAEXcel(sender As System.Object, e As System.EventArgs) Handles btnExcel.Click
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' ACTUALIZANDO INSTRUCCIONES PARA OFFICE2010 Y SU RESPECTIVA LIBRERIA INTEROP (EXCEL14)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim m_Excel, m_Excel2, M_Excel3 As Excel.Application ' Obj Excel
        Dim objLibroExcel, objLibro2, objLibro3 As Excel.Workbook 'Obj Workbook
        Dim objHojaExcel, objHoja2, objHoja3 As Excel.Worksheet 'Obj Worksheet
        Dim inBuffer As String
        Dim contNivAltos As Integer = 0 'Si se detecta que un registro supero el 50% de la MEP, se va a generar un nuevo reporte con todos los puntos que lo hagan

        If ListaResultados.Items.Count = 0 Then
            MsgBox("Antes de crear un archivo .xls debe cargar un archivo de resultados .mrni en el sistema.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Try
            If Conectado Then
                btnConectar.PerformClick()

            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & vbNewLine & ex.StackTrace)
        End Try

        Application.DoEvents()

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   ACA COMIENZAN LOS CAMBIOS DE INSTRUCCIONES DE INTEROP
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        Try
            m_Excel = New Excel.Application
            'objLibroExcel = m_Excel.Workbooks.Open(Application.StartupPath & "\rep\repexc1.xlsx")
            objLibroExcel = m_Excel.Workbooks.Open(Application.StartupPath & "\rep\Modelo_reporte.xlsx")
            'Crear instancia de workbook
            'objLibroExcel = m_Excel.Workbooks.Add
            'Crear instancia de primera hoja de trabajo
            objHojaExcel = m_Excel.Worksheets(1)
            'm_Excel.Calculation = Excel.XlCalculation.xlCalculationManual
            'm_Excel.ScreenUpdating = False
            'm_Excel.EnableLivePreview = False
            'm_Excel.Visible = True
            'Encabezado y pie de página del archivo:
            '------------------------------------------------------------------
            '   TODO ESTÁ PREFIJADO EN EL ARCHIVO .XLSX BASE INCLUIDO EN LA APP
            '------------------------------------------------------------------
            With objHojaExcel
                'Formato del título principal del formulario
                '.Range("A7:L7").Merge()
                .Range("A7:L7").Value = "Tabla de mediciones - " & Application.ProductName & " - Versión " & Application.ProductVersion
                .Range("A7:L7").Font.Bold = True
                .Range("A7:L7").Font.Underline = True
                '.Range("A7:L7").Font.Size = 14
                '--------------------------------------------
                Using SR As New StreamReader(RutaRaizRes)
                    inBuffer = SR.ReadLine
                    Dim MaxVal As Single = 0
                    Dim ItemsTot As Integer = ListaResultados.Items.Count
                    'Dim i As Integer = 0
                    Dim auxstrSonda, auxstrNivel As String()
                    Dim miArray(ItemsTot, 11) As String

                    auxstrSonda = Split(ListaResultados.Items(1).SubItems.Item(8).Text, " ")
                    Dim auxstrMed As String() = Split(ListaResultados.Items(1).SubItems.Item(7).Text, " ")
                    For Each item As ListViewItem In ListaResultados.Items
                        miArray(item.Index, 0) = CSng(item.Text).ToString 'INDICE
                        miArray(item.Index, 1) = item.SubItems.Item(4).Text 'FECHA
                        miArray(item.Index, 2) = item.SubItems.Item(3).Text 'HORA
                        miArray(item.Index, 3) = item.SubItems.Item(5).Text 'LAT
                        miArray(item.Index, 4) = item.SubItems.Item(6).Text 'LON
                        If auxstrMed(1).Contains("300") Then
                            miArray(item.Index, 5) = "TYPE " & auxstrSonda(1) 'MODELO DE LA SONDA
                        Else
                            miArray(item.Index, 5) = auxstrSonda(1) 'MODELO DE LA SONDA
                        End If
                        miArray(item.Index, 6) = auxstrSonda(3) 'NUMSERIE DE LA SONDA
                        miArray(item.Index, 7) = item.SubItems.Item(9).Text  'FechaCal
                        miArray(item.Index, 8) = item.SubItems.Item(10).Text  'Incert db
                        auxstrNivel = Split(item.SubItems.Item(1).Text, " ")
                        miArray(item.Index, 9) = CSng(auxstrNivel(0)).ToString 'RESULTADO CON INCERTIDUMBRE
                        If CSng(miArray(item.Index, 9)) > MaxVal Then
                            MaxVal = miArray(item.Index, 9)
                        End If
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        ''ACA SE DETECTA SI SE SUPERA EL VALOR LIMITE PARA VERIFICAR POR 3690 EN AL MENOS 1 CASO''
                        If CSng(auxstrNivel(0)) >= 14 Then
                            contNivAltos += 1
                        End If
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        miArray(item.Index, 10) = "V/m"
                        If RutaRaizRes.EndsWith("mrni") Then
                            miArray(item.Index, 11) = "Max Hold"
                        ElseIf RutaRaizRes.EndsWith("prni") Then
                            miArray(item.Index, 11) = "Max Avg"
                        End If
                        Application.DoEvents()
                    Next
                    With objHojaExcel
                        .Range("A10:L" & ItemsTot + 10).Value = miArray
                        .Range("A10:L" & ItemsTot + 10).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    End With

                    'Borde alrededor de todo el cuadro, incluido nombres de campos y registros
                    .Range("A9:P" & ItemsTot + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                    .Range("A9:P" & ItemsTot + 9).Font.Size = 8
                    .Range("M9").Font.Size = 7
                    '-------------------------------------------------------------------------
                    'Bordes internos separadores de columnas
                    .Range("B9:B" & ItemsTot + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                    .Range("D9:D" & ItemsTot + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                    .Range("F9:F" & ItemsTot + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                    .Range("H9:H" & ItemsTot + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                    .Range("J9:J" & ItemsTot + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                    .Range("L9:L" & ItemsTot + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                    .Range("N9:N" & ItemsTot + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                    .Range("O9:O" & ItemsTot + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                    '----------------------------------------

                    'No importa que item se use para obtener el medidor, ya que en una misma medición siempre se va a tratar del mismo

                    .Range("A" & ItemsTot + 12).Value = _
                        "Valor máximo detectado en este recorrido: " & MaxVal & " V/m.-"

                    .Range("A" & ItemsTot + 13).Value = _
                        "INSTRUMENTAL UTILIZADO: Medidor de Radiación Electromagnética marca " & auxstrMed(0) & _
                        ", modelo " & auxstrMed(1) & ", número de serie: " & auxstrMed(3) & ".-"

                End Using
            End With
            'objHojaExcel = Nothing
            'objLibroExcel = Nothing

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' GENERACION DE PLANILLA CON VALORES EN POTENCIA ''''''''''''
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            M_Excel3 = New Excel.Application
            'objLibro3 = M_Excel3.Workbooks.Open(Application.StartupPath & "\rep\repexc3.xlsx")
            objLibro3 = M_Excel3.Workbooks.Open(Application.StartupPath & "\rep\Modelo_reporte_P.xlsx")
            objHoja3 = M_Excel3.Worksheets(1)
            With objHoja3
                'Formato del título principal del formulario
                '.Range("A7:L7").Merge()
                .Range("A9:H9").Value = "Reporte de mediciones de Radiaciones No Ionizantes"
                .Range("A9:H9").Font.Bold = True
                .Range("A9:H9").Font.Underline = True
                '.Range("A7:L7").Font.Size = 14
                '--------------------------------------------
                Using SR As New StreamReader(RutaRaizRes)
                    Dim MaxPot As Single = 0
                    inBuffer = SR.ReadLine
                    Dim ItemsTot As Integer = ListaResultados.Items.Count
                    'Dim i As Integer = 0
                    Dim auxstrSonda, auxstrNivel As String()
                    Dim miArray(ItemsTot, 5) As String

                    auxstrSonda = Split(ListaResultados.Items(1).SubItems.Item(8).Text, " ")
                    Dim auxstrMed As String() = Split(ListaResultados.Items(1).SubItems.Item(7).Text, " ")
                    For Each item As ListViewItem In ListaResultados.Items
                        miArray(item.Index, 0) = CSng(item.Text).ToString 'INDICE
                        miArray(item.Index, 1) = item.SubItems.Item(4).Text 'FECHA
                        miArray(item.Index, 2) = item.SubItems.Item(3).Text 'HORA
                        miArray(item.Index, 3) = item.SubItems.Item(5).Text 'LAT
                        miArray(item.Index, 4) = item.SubItems.Item(6).Text 'LON
                        auxstrNivel = Split(item.SubItems.Item(1).Text, " ")
                        miArray(item.Index, 5) = _
                            Math.Round((Math.Pow(CSng(auxstrNivel(0)), 2) / 3774) * 100 / 0.2, 4) 'PORCENTAJE DE POTENCIA RESPECTO A LA MEP MAS ESTRICTA

                        '
                        '   La operación es  ((((V|m)^2) / 10z0) * 100) / 0.2mW|cm2
                        '                           
                        '   No se calcula en base a Volts por metro, sino por miliwatts sobre cm2
                        '
                        '
                        Application.DoEvents()
                    Next
                    With objHoja3
                        .Range("B12:G" & ItemsTot + 11).Value = miArray
                        .Range("B12:G" & ItemsTot + 11).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    End With

                    'Borde alrededor de todo el cuadro, incluido nombres de campos y registros
                    .Range("B11:G" & ItemsTot + 11).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                    .Range("B11:G" & ItemsTot + 11).Font.Size = 8
                    '.Range("M9").Font.Size = 7
                    '-------------------------------------------------------------------------
                    'Bordes internos separadores de columnas
                    .Range("B11:B" & ItemsTot + 11).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                    .Range("D11:D" & ItemsTot + 11).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                    .Range("F11:F" & ItemsTot + 11).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                    '----------------------------------------

                    'No importa que item se use para obtener el medidor, ya que en una misma medición siempre se va a tratar del mismo

                    .Range("A" & ItemsTot + 13).Value = _
                        "INSTRUMENTAL UTILIZADO: Medidor de Radiación Electromagnética marca " & auxstrMed(0) & _
                         " " & auxstrMed(1) & ", N°/S: " & auxstrMed(3) & " - con sonda " & auxstrSonda(1) & " " & ", N°/S: " & auxstrSonda(3) & ".-"

                End Using
            End With

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'SI SE DETECTO ALGUN VALOR QUE SUPERE EL 50% DE LA MEP....
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If contNivAltos > 0 Then
                m_Excel2 = New Excel.Application
                'objLibro2 = m_Excel2.Workbooks.Open(Application.StartupPath & "\rep\repexc2.xlsx")
                objLibro2 = m_Excel2.Workbooks.Open(Application.StartupPath & "\rep\Modelo_averif.xlsx")
                objHoja2 = m_Excel2.Worksheets(1)
                With objHoja2
                    'Formato del título principal del formulario
                    '.Range("A7:L7").Merge()
                    .Range("A7:L7").Value = "Tabla de mediciones a verificar - " & Application.ProductName & " - Versión " & Application.ProductVersion
                    .Range("A7:L7").Font.Bold = True
                    .Range("A7:L7").Font.Underline = True
                    '.Range("A7:L7").Font.Size = 14
                    '--------------------------------------------
                    Using SR As New StreamReader(RutaRaizRes)
                        inBuffer = SR.ReadLine
                        Dim i As Integer = 0
                        Dim auxstrSonda, auxstrNivel As String()
                        Dim miArray(contNivAltos, 11) As String

                        auxstrSonda = Split(ListaResultados.Items(1).SubItems.Item(8).Text, " ")
                        Dim auxstrMed As String() = Split(ListaResultados.Items(1).SubItems.Item(7).Text, " ")
                        For Each item As ListViewItem In ListaResultados.Items
                            auxstrNivel = Split(item.SubItems.Item(1).Text, " ")
                            If CSng(auxstrNivel(0)) >= 14 Then 'RESULTADO CON INCERTIDUMBRE
                                miArray(i, 0) = CSng(item.Text).ToString 'INDICE
                                miArray(i, 1) = item.SubItems.Item(4).Text 'FECHA
                                miArray(i, 2) = item.SubItems.Item(3).Text 'HORA
                                miArray(i, 3) = item.SubItems.Item(5).Text 'LAT
                                miArray(i, 4) = item.SubItems.Item(6).Text 'LON
                                If auxstrMed(1).Contains("300") Then
                                    miArray(i, 5) = "TYPE " & auxstrSonda(1) 'MODELO DE LA SONDA
                                Else
                                    miArray(i, 5) = auxstrSonda(1) 'MODELO DE LA SONDA
                                End If
                                miArray(i, 6) = auxstrSonda(3) 'NUMSERIE DE LA SONDA
                                miArray(i, 7) = item.SubItems.Item(9).Text  'FechaCal
                                miArray(i, 8) = item.SubItems.Item(10).Text  'Incert db vehiculo
                                miArray(i, 9) = CSng(auxstrNivel(0)).ToString 'RESULTADO CON INCERTIDUMBRE VEHICULO en V/m
                                'miArray(i, 10) = "V/m"
                                'If RutaRaizRes.EndsWith("mrni") Then
                                'miArray(i, 11) = "Max Hold"
                                'ElseIf RutaRaizRes.EndsWith("prni") Then
                                'miArray(i, 11) = "Max Avg"
                                'End If
                                Application.DoEvents()
                                i += 1
                            End If
                        Next
                        With objHoja2
                            .Range("A10:L" & contNivAltos + 10).Value = miArray
                            .Range("A10:L" & contNivAltos + 10).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                        End With

                        'Borde alrededor de todo el cuadro, incluido nombres de campos y registros
                        .Range("A9:P" & contNivAltos + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                        .Range("A9:P" & contNivAltos + 9).Font.Size = 8
                        .Range("M9").Font.Size = 7
                        '-------------------------------------------------------------------------
                        'Bordes internos separadores de columnas
                        .Range("B9:B" & contNivAltos + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                        .Range("D9:D" & contNivAltos + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                        .Range("F9:F" & contNivAltos + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                        .Range("H9:H" & contNivAltos + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                        .Range("J9:J" & contNivAltos + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                        .Range("L9:L" & contNivAltos + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                        .Range("N9:N" & contNivAltos + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                        .Range("O9:O" & contNivAltos + 9).BorderAround(1, Excel.XlBorderWeight.xlMedium)
                        '----------------------------------------

                        'No importa que item se use para obtener el medidor, ya que en una misma medición siempre se va a tratar del mismo

                        .Range("A" & contNivAltos + 12).Value = _
                            "INSTRUMENTAL UTILIZADO: Medidor de Radiación Electromagnética marca " & auxstrMed(0) & _
                            ", modelo " & auxstrMed(1) & ", número de serie: " & auxstrMed(3) & ".-"
                    End Using
                End With
            End If

            sDialog.Filter = "Hoja de cálculo de Microsoft Excel (*.xlsx)|*.xlsx"
            sDialog.FileName = ""
            If sDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
            Dim RutaReporte As String = sDialog.FileName
            objHojaExcel.SaveAs(RutaReporte)
            Dim RutaRep3 As String = sDialog.FileName.Substring(0, Len(sDialog.FileName) - 5) & "_P.xlsx"
            objHoja3.SaveAs(RutaRep3)
            If contNivAltos > 0 Then
                Dim RutaRep2 As String = sDialog.FileName.Substring(0, Len(sDialog.FileName) - 5) & " (A VERIFICAR).xlsx"
                objHoja2.SaveAs(RutaRep2)
            End If
            'm_Excel.Visible = True
            'm_Excel2.Visible = True
            txtEventos.Text &= "[" & Now & "] " & "Reporte(s) generado(s) con éxito en la ruta seleccionada." & vbNewLine
        Catch ex As Exception
            txtEventos.Text &= "[" & Now & "] " & "Ha ocurrido una excepción: " & ex.Message & vbNewLine & vbNewLine & ex.StackTrace
            MsgBox(ex.Message)
        Finally
            'm_Excel.Calculation = Excel.XlCalculation.xlCalculationAutomatic
            'm_Excel.ScreenUpdating = True
            'm_Excel.EnableLivePreview = True
            ' Cerrado de la aplicación
            If Not m_Excel Is Nothing Then
                objHojaExcel = Nothing
                objLibroExcel = Nothing
                m_Excel.Quit()
                m_Excel = Nothing
            End If
            If Not m_Excel2 Is Nothing Then
                objHoja2 = Nothing
                objLibro2 = Nothing
                m_Excel2.Quit()
                m_Excel2 = Nothing
            End If
            If Not M_Excel3 Is Nothing Then
                objHoja3 = Nothing
                objLibro3 = Nothing
                M_Excel3.Quit()
                M_Excel3 = Nothing
            End If
            If ProcesoGPS.ThreadState = ThreadState.Suspended Then ProcesoGPS.Resume()
            If ProcesoLeerNarda.ThreadState = ThreadState.Suspended Then ProcesoLeerNarda.Resume()
            GC.Collect()
        End Try

    End Sub

    Private Sub Mapa_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Mapa.KeyUp
        Select Case e.KeyValue
            Case Is = 45
                Mapa.Zoom -= 1
            Case Is = 43
                Mapa.Zoom += 1
        End Select
    End Sub

    Private Sub DetenerCampaña(sender As System.Object, e As System.EventArgs) Handles btnDetenerCamp.Click
        boolStopCamp = True
    End Sub

    Private Sub PonerMarker(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Mapa.MouseDoubleClick
        Dim PosIni As New Point(e.X, e.Y)

        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim LatEnGMS As CoordenadasGMS
            Dim LngEnGMS As CoordenadasGMS
            Dim MousePosLatLng As PointLatLng = Mapa.FromLocalToLatLng(e.X, e.Y)
            UltimoPunto = MousePosLatLng

            LatEnGMS = ConvertirAGMS(UltimoPunto.Lat, False)
            LngEnGMS = ConvertirAGMS(UltimoPunto.Lng, True)

            '- El overlay se hace en el global y por ahora usamos uno solo para todos los markers
            '- Proximamente, hay que hacer un overlay PARA CADA TIPO de objeto (marker, ruta, poligono, etc.)
            OverlayClick.Markers.Clear()
            'Mapa.Overlays.Clear()
            Dim NuevoMarker As GMapMarker = New GMarkerGoogle(UltimoPunto, GMarkerGoogleType.red)
            NuevoMarker.ToolTipText = LatEnGMS.Grados & "° " & LatEnGMS.Minutos & "' " & Math.Round(LatEnGMS.Segundos, 2) & Chr(34) & " " & LatEnGMS.Hemisf & vbNewLine & _
                LngEnGMS.Grados & "° " & LngEnGMS.Minutos & "' " & Math.Round(LngEnGMS.Segundos, 2) & Chr(34) & " " & LngEnGMS.Hemisf

            NuevoMarker.ToolTipMode = MarkerTooltipMode.OnMouseOver
            'Mapa.Overlays.Add(OverlayClick)
            OverlayClick.Markers.Add(NuevoMarker)
            'ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            'OverlayClick.Clear()
        End If
    End Sub

    Private Sub CargarArchivo(sender As System.Object, e As System.EventArgs) Handles CargarArchivoDePuntosToolStripMenuItem.Click
        Dim RutaBackup As String = My.Computer.FileSystem.SpecialDirectories.Desktop & "/backuprni"
        Dim RutaFichero As String
        Dim alertaNivel As Boolean = False
        Try

            boolCheck = False
            'oDialog.Filter = "Mediciones RNI de valores máximos (*.mrni)|*.mrni|Mediciones RNI de valores promediados (*.prni)|*.prni"
            oDialog.Filter = "Mediciones RNI de valores máximos (*.mrni)|*.mrni"
            oDialog.FileName = ""
            If oDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
            RutaFichero = oDialog.FileName
            RutaRaizRes = RutaFichero
            Using SR As StreamReader = New StreamReader(RutaFichero)
                'FileOpen(1, RutaFichero, OpenMode.Input)
                Dim Crypt As Encriptador = New Encriptador("029112")
                Dim InBuffer As String
                Dim PlainData As String

                'Dim VecesDB As Single

                ' Se lee todo el archivo cifrado
                InBuffer = SR.ReadToEnd

                ' Se chequea si el archivo está encriptado (no empieza con "#")
                If Not InBuffer.StartsWith("#") Then
                    ' Se desencripta
                    PlainData = Crypt.DecryptData(InBuffer)
                Else
                    PlainData = InBuffer
                End If


                Dim respaldo As StreamWriter = New StreamWriter(RutaBackup)
                respaldo.Write(PlainData)
                respaldo.Close()
            End Using

            Using SR As StreamReader = New StreamReader(RutaBackup)
                Dim Punto As Registro
                Dim LatEnGMS, LngEnGMS As CoordenadasGMS
                Dim InBuffer As String
                Dim VecPunto() As String
                'Extraer nombre de campaña
                'InBuffer = LineInput(1)
                InBuffer = SR.ReadLine
                InBuffer = InBuffer.Substring(20, Len(InBuffer) - 20)
                Dim NombreCamp As String = InBuffer
                lblCargado.Text = "Recorrido cargado: " & Chr(34) & NombreCamp & Chr(34) & " (" & RutaFichero & ")"
                lblCargado.Visible = True

                'Saltear la fecha de inicio
                InBuffer = SR.ReadLine
                'Extraer equipo utilizado
                InBuffer = SR.ReadLine
                Dim EquipoNom As String = InBuffer.Substring(26, Len(InBuffer) - 26)
                'Extraer numero de serie del equipo
                InBuffer = SR.ReadLine
                Dim EquipoNumSerie As String = InBuffer.Substring(33, Len(InBuffer) - 33)
                'Extraer sonda utilizada
                InBuffer = SR.ReadLine
                Dim SondaNom As String = InBuffer.Substring(18, Len(InBuffer) - 18)
                If SondaNom.Contains("EF") Then
                    UnidadActual = "V/m"
                Else
                    UnidadActual = "A/m"
                End If
                'Extraer numero de serie de la sonda
                InBuffer = SR.ReadLine
                Dim SondaNumSerie As String = InBuffer.Substring(27, Len(InBuffer) - 27)
                'Extrar fecha de ultima calibracion de la sonda
                InBuffer = SR.ReadLine
                Dim SondaFechaCal As String = InBuffer.Substring(39, Len(InBuffer) - 39)
                'Extraer incertidumbre de la sonda
                InBuffer = SR.ReadLine
                Dim SondaIncert As String = Replace(InBuffer.Substring(35, Len(InBuffer) - 35), ".", ",")
                'Extraer factor de multiplicacion
                InBuffer = SR.ReadLine
                Dim SondaFactor As Single = CSng(Replace(InBuffer.Substring(9, Len(InBuffer) - 9), ".", ",")) 'queremos las veces (FACTOR)
                'Se borran todos los markers de la capa de puntos cargados desde archivo
                OverlayCarga.Markers.Clear()

                'EVALUAR SI ES EFICIENTE BORRAR LOS MARKERS DE RESULTADOS RECIENTEMENTE TOMADOS (PARA NO MEZCLAR)
                'Y SI CONVIENE TENER MEZCLADOS EN LA LISTA LOS DE AMBOS EVENTOS
                'OverlayResultados.Clear()
                'Se limpia la lista de markers
                ListaResultados.Items.Clear()

                InBuffer = SR.ReadLine
                Do Until InBuffer = Nothing
                    Dim aux() As String
                    'InBuffer = LineInput(1)
                    VecPunto = Split(InBuffer, ",")
                    'vecpunto(i):
                    '0= numero de registro
                    '1= nivel y unidad
                    '2= hora
                    '3= fecha
                    '4= lat gdec
                    '5= lng gdec
                    With Punto
                        .Indice = CInt(VecPunto(0))
                        aux = Split(VecPunto(1), " ")

                        If Len(aux(0)) > 1 Then
                            If aux(0).Contains(".") Then
                                If aux(0).Substring(2, 1) = "." Then
                                    .Nivel = CSng(aux(0).Replace(".", ","))
                                ElseIf aux(0).Substring(1, 1) = "." Then
                                    .Nivel = CSng(aux(0).Replace(".", ","))
                                End If
                            End If
                        Else
                            .Nivel = CSng(aux(0))
                        End If
                        .NivelPuro = FormatNumber(.Nivel, 3)
                        .Nivel = .NivelPuro * SondaFactor
                        .Hora = VecPunto(3).Substring(0, 8)
                        .Fecha = VecPunto(2)
                        .Lat = VecPunto(4)
                        .Lon = VecPunto(5)

                    End With

                    Dim latfactor As Double = Convert.ToDouble(Replace(Punto.Lat, ".", ","))
                    Dim lonfactor As Double = Convert.ToDouble(Replace(Punto.Lon, ".", ","))
                    LatEnGMS = ConvertirAGMS(latfactor, False)
                    LngEnGMS = ConvertirAGMS(lonfactor, True)

                    If Punto.Indice = 1 Then
                        Mapa.Position = New PointLatLng(latfactor, lonfactor)
                        Mapa.Zoom = 14
                        lblZoom.Text = Mapa.Zoom.ToString
                    End If

                    'Dim Pos As New PointLatLng(latfactor, lonfactor)
                    Dim ColorMarker As GMarkerGoogleType
                    Dim IndiceImg As Integer

                    Select Case Punto.Nivel
                        Case Is >= 27.5
                            ColorMarker = GMarkerGoogleType.red_dot
                            IndiceImg = 0
                            alertaNivel = True
                        Case Is >= 20
                            ColorMarker = GMarkerGoogleType.orange_dot
                            IndiceImg = 1
                        Case Is >= 14
                            ColorMarker = GMarkerGoogleType.yellow_dot
                            IndiceImg = 2
                        Case Is >= 8
                            ColorMarker = GMarkerGoogleType.purple_dot
                            IndiceImg = 3
                        Case Is >= 4
                            ColorMarker = GMarkerGoogleType.green_dot
                            IndiceImg = 4
                        Case Is >= 2
                            ColorMarker = GMarkerGoogleType.lightblue_dot
                            IndiceImg = 5
                        Case Else
                            ColorMarker = GMarkerGoogleType.blue_dot
                            IndiceImg = 6
                    End Select

                    Dim NuevoMarker As GMarkerGoogle = New GMarkerGoogle(New PointLatLng(latfactor, lonfactor), ColorMarker)
                    NuevoMarker.Tag = Punto.Indice
                    NuevoMarker.ToolTipText = "Punto " & Punto.Indice & vbNewLine & _
                    "Nivel c/incert.: " & Punto.Nivel & " " & UnidadActual & vbNewLine & _
                    "Nivel s/incert.: " & Punto.NivelPuro & " " & UnidadActual & vbNewLine & _
                    "Porcentaje de la MEP: " & Math.Round(((Punto.Nivel ^ 2) / 3770) * 100 / 0.2, 2).ToString & "%" & vbNewLine & _
                    Punto.Fecha & " - " & Punto.Hora & vbNewLine & _
                    LatEnGMS.Grados & "° " & LatEnGMS.Minutos & "' " & Math.Round(LatEnGMS.Segundos, 3) & Chr(34) & " " & LatEnGMS.Hemisf & vbNewLine & _
                    LngEnGMS.Grados & "° " & LngEnGMS.Minutos & "' " & Math.Round(LngEnGMS.Segundos, 3) & Chr(34) & " " & LngEnGMS.Hemisf

                    '"Punto " & Punto.Indice & vbNewLine & _
                    '"Nivel c/incert: " & Punto.Nivel * SondaFactor & " V/m" & vbNewLine & _
                    '"Fecha: " & Punto.Fecha & " - Hora: " & Punto.Hora & vbNewLine & vbNewLine & _
                    'LatEnGMS.Grados & "° " & LatEnGMS.Minutos & "' " & Math.Round(LatEnGMS.Segundos, 3) & Chr(34) & " " & LatEnGMS.Hemisf & vbNewLine & _
                    'LngEnGMS.Grados & "° " & LngEnGMS.Minutos & "' " & Math.Round(LngEnGMS.Segundos, 3) & Chr(34) & " " & LngEnGMS.Hemisf

                    NuevoMarker.ToolTipMode = MarkerTooltipMode.OnMouseOver
                    OverlayCarga.Markers.Add(NuevoMarker)
                    ListaResultados.Sorting = SortOrder.None
                    'La columna principal es este dato (el item en si)
                    'Dim nReg As New ListViewItem(Format(Punto.Indice, "0000"))
                    Dim nReg As New ListViewItem(Punto.Indice)
                    With nReg
                        'Cada subitem es la columna 2 en adelante (los atributos del item)
                        .SubItems.Add(FormatNumber(Punto.Nivel, 3) & " " & UnidadActual)   'con incert
                        .SubItems.Add(Punto.NivelPuro & " " & UnidadActual)                 'sin incert
                        .SubItems.Add(Punto.Hora)
                        .SubItems.Add(Punto.Fecha)
                        .SubItems.Add(LatEnGMS.Grados & "° " & LatEnGMS.Minutos & "' " & LatEnGMS.Segundos & Chr(34) & " " & LatEnGMS.Hemisf)
                        .SubItems.Add(LngEnGMS.Grados & "° " & LngEnGMS.Minutos & "' " & LngEnGMS.Segundos & Chr(34) & " " & LngEnGMS.Hemisf)
                        .SubItems.Add(EquipoNom & " - " & EquipoNumSerie)
                        .SubItems.Add(SondaNom & " - " & SondaNumSerie)
                        .SubItems.Add(SondaFechaCal)
                        .SubItems.Add(SondaIncert.ToString & " dB")
                        .SubItems.Add(SondaFactor.ToString)
                    End With
                    'Agrega el item a la lista
                    ListaResultados.Items.Add(nReg)
                    'Asigna la imagen correspondiente al item segun el nivel detectado
                    ListaResultados.Items(Punto.Indice - 1).ImageIndex = IndiceImg
                    'Tilda el checkbox del item para que se muestre en mapa
                    ListaResultados.Items(Punto.Indice - 1).Checked = True
                    nReg.EnsureVisible()

                    InBuffer = SR.ReadLine
                    Application.DoEvents()
HacerLoop:      Loop
                SR.Close()
            End Using
            If alertaNivel Then
                MsgBox("Atención: Hay puntos con energía que superan la Máxima Exposición Permitida.", MsgBoxStyle.Exclamation, "Se detectaron puntos calientes")
            End If
        Catch ex As Exception
            lblCargado.Text = ""
            lblCargado.Visible = False
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
            txtEventos.Text &= "[" & Now & "] " & "Ha ocurrido una excepción: " & ex.Message & vbNewLine
        Finally
            'FileClose(1)
            boolCheck = True
            File.Delete(RutaBackup)
        End Try
    End Sub

    Private Sub OLDCargarArchivo(sender As System.Object, e As System.EventArgs)
        Try
            Dim RutaFichero As String
            Dim alertaNivel As Boolean = False
            boolCheck = False
            oDialog.Filter = "Mediciones RNI de valores máximos (*.mrni)|*.mrni|Mediciones RNI de valores promediados (*.prni)|*.prni"
            oDialog.FileName = ""
            If oDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
            RutaFichero = oDialog.FileName
            RutaRaizRes = RutaFichero
            Using SR As StreamReader = New StreamReader(RutaFichero)
                'FileOpen(1, RutaFichero, OpenMode.Input)
                Dim Punto As Registro
                Dim LatEnGMS, LngEnGMS As CoordenadasGMS
                Dim InBuffer As String
                Dim VecPunto() As String
                'Dim VecesDB As Single

                'Extraer nombre de campaña
                'InBuffer = LineInput(1)
                InBuffer = SR.ReadLine
                InBuffer = InBuffer.Substring(20, Len(InBuffer) - 20)
                Dim NombreCamp As String = InBuffer
                lblCargado.Text = "Recorrido cargado: " & Chr(34) & NombreCamp & Chr(34) & " (" & RutaFichero & ")"
                lblCargado.Visible = True

                'Saltear la fecha de inicio
                InBuffer = SR.ReadLine
                'Extraer equipo utilizado
                InBuffer = SR.ReadLine
                Dim EquipoNom As String = InBuffer.Substring(26, Len(InBuffer) - 26)
                'Extraer numero de serie del equipo
                InBuffer = SR.ReadLine
                Dim EquipoNumSerie As String = InBuffer.Substring(33, Len(InBuffer) - 33)
                'Extraer sonda utilizada
                InBuffer = SR.ReadLine
                Dim SondaNom As String = InBuffer.Substring(18, Len(InBuffer) - 18)
                If SondaNom.Contains("EF") Then
                    UnidadActual = "V/m"
                Else
                    UnidadActual = "A/m"
                End If
                'Extraer numero de serie de la sonda
                InBuffer = SR.ReadLine
                Dim SondaNumSerie As String = InBuffer.Substring(27, Len(InBuffer) - 27)
                'Extrar fecha de ultima calibracion de la sonda
                InBuffer = SR.ReadLine
                Dim SondaFechaCal As String = InBuffer.Substring(39, Len(InBuffer) - 39)
                'Extraer incertidumbre de la sonda
                InBuffer = SR.ReadLine
                Dim SondaIncert As String = Replace(InBuffer.Substring(35, Len(InBuffer) - 35), ".", ",")
                'Extraer factor de multiplicacion
                InBuffer = SR.ReadLine
                Dim SondaFactor As Single = CSng(Replace(InBuffer.Substring(9, Len(InBuffer) - 9), ".", ",")) 'queremos las veces (FACTOR)
                'Se borran todos los markers de la capa de puntos cargados desde archivo
                OverlayCarga.Markers.Clear()

                'EVALUAR SI ES EFICIENTE BORRAR LOS MARKERS DE RESULTADOS RECIENTEMENTE TOMADOS (PARA NO MEZCLAR)
                'Y SI CONVIENE TENER MEZCLADOS EN LA LISTA LOS DE AMBOS EVENTOS
                'OverlayResultados.Clear()
                'Se limpia la lista de markers
                ListaResultados.Items.Clear()

                InBuffer = SR.ReadLine
                Do Until InBuffer = Nothing
                    Dim aux() As String
                    'InBuffer = LineInput(1)
                    VecPunto = Split(InBuffer, ",")
                    'vecpunto(i):
                    '0= numero de registro
                    '1= nivel y unidad
                    '2= hora
                    '3= fecha
                    '4= lat gdec
                    '5= lng gdec
                    With Punto
                        .Indice = CInt(VecPunto(0))
                        aux = Split(VecPunto(1), " ")
                        '.Nivel = CDbl(aux(0)) / 1000
                        If Len(aux(0)) > 1 Then
                            If aux(0).Substring(2, 1) = "." Then
                                .Nivel = CSng(aux(0).Replace(".", ","))
                            ElseIf aux(0).Substring(1, 1) = "." Then
                                .Nivel = CSng(aux(0).Replace(".", ","))
                            End If
                        Else
                            .Nivel = CSng(aux(0))
                        End If
                        .NivelPuro = FormatNumber(.Nivel, 3)
                        .Nivel = .NivelPuro * SondaFactor
                        .Hora = VecPunto(3).Substring(0, 8)
                        .Fecha = VecPunto(2)
                        .Lat = VecPunto(4)
                        .Lon = VecPunto(5)

                    End With

                    Dim latfactor As Double = Convert.ToDouble(Replace(Punto.Lat, ".", ","))
                    Dim lonfactor As Double = Convert.ToDouble(Replace(Punto.Lon, ".", ","))
                    LatEnGMS = ConvertirAGMS(latfactor, False)
                    LngEnGMS = ConvertirAGMS(lonfactor, True)

                    If Punto.Indice = 1 Then
                        Mapa.Position = New PointLatLng(latfactor, lonfactor)
                        Mapa.Zoom = 14
                        lblZoom.Text = Mapa.Zoom.ToString
                    End If

                    'Dim Pos As New PointLatLng(latfactor, lonfactor)
                    Dim ColorMarker As GMarkerGoogleType
                    Dim IndiceImg As Integer

                    Select Case Punto.Nivel
                        Case Is >= 27.5
                            ColorMarker = GMarkerGoogleType.red_dot
                            IndiceImg = 0
                            alertaNivel = True
                        Case Is >= 20
                            ColorMarker = GMarkerGoogleType.orange_dot
                            IndiceImg = 1
                        Case Is >= 14
                            ColorMarker = GMarkerGoogleType.yellow_dot
                            IndiceImg = 2
                        Case Is >= 8
                            ColorMarker = GMarkerGoogleType.purple_dot
                            IndiceImg = 3
                        Case Is >= 4
                            ColorMarker = GMarkerGoogleType.green_dot
                            IndiceImg = 4
                        Case Is >= 2
                            ColorMarker = GMarkerGoogleType.lightblue_dot
                            IndiceImg = 5
                        Case Else
                            ColorMarker = GMarkerGoogleType.blue_dot
                            IndiceImg = 6
                    End Select

                    Dim NuevoMarker As GMarkerGoogle = New GMarkerGoogle(New PointLatLng(latfactor, lonfactor), ColorMarker)
                    NuevoMarker.Tag = Punto.Indice
                    NuevoMarker.ToolTipText = "Punto " & Punto.Indice & vbNewLine & _
                    "Nivel c/incert.: " & Punto.Nivel & " " & UnidadActual & vbNewLine & _
                    "Nivel s/incert.: " & Punto.NivelPuro & " " & UnidadActual & vbNewLine & _
                    "Porcentaje de la MEP: " & Math.Round(((Punto.Nivel ^ 2) / 3770) * 100 / 0.2, 2).ToString & "%" & vbNewLine & _
                    Punto.Fecha & " - " & Punto.Hora & vbNewLine & _
                    LatEnGMS.Grados & "° " & LatEnGMS.Minutos & "' " & Math.Round(LatEnGMS.Segundos, 3) & Chr(34) & " " & LatEnGMS.Hemisf & vbNewLine & _
                    LngEnGMS.Grados & "° " & LngEnGMS.Minutos & "' " & Math.Round(LngEnGMS.Segundos, 3) & Chr(34) & " " & LngEnGMS.Hemisf

                    '"Punto " & Punto.Indice & vbNewLine & _
                    '"Nivel c/incert: " & Punto.Nivel * SondaFactor & " V/m" & vbNewLine & _
                    '"Fecha: " & Punto.Fecha & " - Hora: " & Punto.Hora & vbNewLine & vbNewLine & _
                    'LatEnGMS.Grados & "° " & LatEnGMS.Minutos & "' " & Math.Round(LatEnGMS.Segundos, 3) & Chr(34) & " " & LatEnGMS.Hemisf & vbNewLine & _
                    'LngEnGMS.Grados & "° " & LngEnGMS.Minutos & "' " & Math.Round(LngEnGMS.Segundos, 3) & Chr(34) & " " & LngEnGMS.Hemisf

                    NuevoMarker.ToolTipMode = MarkerTooltipMode.OnMouseOver
                    OverlayCarga.Markers.Add(NuevoMarker)
                    ListaResultados.Sorting = SortOrder.None
                    'La columna principal es este dato (el item en si)
                    'Dim nReg As New ListViewItem(Format(Punto.Indice, "0000"))
                    Dim nReg As New ListViewItem(Punto.Indice)
                    With nReg
                        'Cada subitem es la columna 2 en adelante (los atributos del item)
                        .SubItems.Add(FormatNumber(Punto.Nivel, 3) & " " & UnidadActual)   'con incert
                        .SubItems.Add(Punto.NivelPuro & " " & UnidadActual)                 'sin incert
                        .SubItems.Add(Punto.Hora)
                        .SubItems.Add(Punto.Fecha)
                        .SubItems.Add(LatEnGMS.Grados & "° " & LatEnGMS.Minutos & "' " & LatEnGMS.Segundos & Chr(34) & " " & LatEnGMS.Hemisf)
                        .SubItems.Add(LngEnGMS.Grados & "° " & LngEnGMS.Minutos & "' " & LngEnGMS.Segundos & Chr(34) & " " & LngEnGMS.Hemisf)
                        .SubItems.Add(EquipoNom & " - " & EquipoNumSerie)
                        .SubItems.Add(SondaNom & " - " & SondaNumSerie)
                        .SubItems.Add(SondaFechaCal)
                        .SubItems.Add(SondaIncert.ToString & " dB")
                        .SubItems.Add(SondaFactor.ToString)
                    End With
                    'Agrega el item a la lista
                    ListaResultados.Items.Add(nReg)
                    'Asigna la imagen correspondiente al item segun el nivel detectado
                    ListaResultados.Items(Punto.Indice - 1).ImageIndex = IndiceImg
                    'Tilda el checkbox del item para que se muestre en mapa
                    ListaResultados.Items(Punto.Indice - 1).Checked = True
                    nReg.EnsureVisible()

                    InBuffer = SR.ReadLine
                    Application.DoEvents()
HacerLoop:      Loop
            End Using
            If alertaNivel Then
                MsgBox("Atención: Hay puntos con energía que superan la Máxima Exposición Permitida!", MsgBoxStyle.Exclamation, "Se detectaron puntos calientes")
            End If
        Catch ex As Exception
            lblCargado.Text = ""
            lblCargado.Visible = False
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            txtEventos.Text &= "[" & Now & "] " & "Ha ocurrido una excepción: " & ex.Message & vbNewLine
        Finally
            'FileClose(1)
            boolCheck = True
        End Try
    End Sub

    Private Sub ListaResultados_ItemChecked(sender As Object, e As System.Windows.Forms.ItemCheckedEventArgs) Handles ListaResultados.ItemChecked

        Dim mID As String = e.Item.SubItems.Item(0).Text

        If boolCheck = False Then Exit Sub

        If e.Item.Checked = True Then
            Dim Punto As Registro
            With Punto
                .Indice = CInt(e.Item.Text)
                Dim vecniv() As String = Split(e.Item.SubItems.Item(1).Text)
                .Nivel = CSng(vecniv(0))
                Dim vecnivpuro() As String = Split(e.Item.SubItems.Item(2).Text)
                .NivelPuro = CSng(vecnivpuro(0))
                .Hora = e.Item.SubItems.Item(3).Text
                .Fecha = Convert.ToDateTime(e.Item.SubItems.Item(4).Text)
                Dim vec() As String = Split(e.Item.SubItems.Item(5).Text, " ")
                Dim latgms As CoordenadasGMS
                latgms.Grados = vec(0).Trim(trimmers)
                latgms.Minutos = vec(1).Trim(trimmers)
                latgms.Segundos = vec(2).Trim(trimmers)
                latgms.Hemisf = vec(3)
                .Lat = ConvertirAGDec(latgms)
                Dim vecc() As String = Split(e.Item.SubItems.Item(6).Text, " ")
                Dim longms As CoordenadasGMS
                longms.Grados = vecc(0).Trim(trimmers)
                longms.Minutos = vecc(1).Trim(trimmers)
                longms.Segundos = vecc(2).Trim(trimmers)
                longms.Hemisf = vecc(3)
                .Lon = ConvertirAGDec(longms)
            End With
            'Dim latfactor As Double = Convert.ToDouble(Replace(Punto.Lat, ".", ","))
            'Dim lonfactor As Double = Convert.ToDouble(Replace(Punto.Lon, ".", ","))
            Dim LatEnGMS As CoordenadasGMS = ConvertirAGMS(Punto.Lat, False)
            Dim LngEnGMS As CoordenadasGMS = ConvertirAGMS(Punto.Lon, True)
            Dim ColorMarker As GMarkerGoogleType
            Dim IndiceImg As Integer

            Select Case Punto.Nivel
                Case Is >= 27.5
                    ColorMarker = GMarkerGoogleType.red_dot
                    IndiceImg = 0
                Case Is >= 20
                    ColorMarker = GMarkerGoogleType.orange_dot
                    IndiceImg = 1
                Case Is >= 14
                    ColorMarker = GMarkerGoogleType.yellow_dot
                    IndiceImg = 2
                Case Is >= 8
                    ColorMarker = GMarkerGoogleType.purple_dot
                    IndiceImg = 3
                Case Is >= 4
                    ColorMarker = GMarkerGoogleType.green_dot
                    IndiceImg = 4
                Case Is >= 2
                    ColorMarker = GMarkerGoogleType.lightblue_dot
                    IndiceImg = 5
                Case Else
                    ColorMarker = GMarkerGoogleType.blue_dot
                    IndiceImg = 6
            End Select
            Dim NuevoMarker As GMarkerGoogle = New GMarkerGoogle(New PointLatLng(Punto.Lat, Punto.Lon), ColorMarker)
            NuevoMarker.Tag = Punto.Indice
            NuevoMarker.ToolTipText = "Punto " & Punto.Indice & vbNewLine & _
                    "Nivel c/incert.: " & FormatNumber(Punto.Nivel, 3) & " " & UnidadActual & vbNewLine & _
                    "Nivel s/incert.: " & Punto.NivelPuro & " " & UnidadActual & vbNewLine & _
                    "Porcentaje de la MEP mas estricta: " & Math.Round(((Punto.Nivel ^ 2) / 3770) * 100 / 0.2, 2).ToString & "%" & vbNewLine & _
                    Punto.Fecha & " - " & Punto.Hora & vbNewLine & _
                    LatEnGMS.Grados & "° " & LatEnGMS.Minutos & "' " & Math.Round(LatEnGMS.Segundos, 2) & Chr(34) & " " & LatEnGMS.Hemisf & vbNewLine & _
                    LngEnGMS.Grados & "° " & LngEnGMS.Minutos & "' " & Math.Round(LngEnGMS.Segundos, 2) & Chr(34) & " " & LngEnGMS.Hemisf

            NuevoMarker.ToolTipMode = MarkerTooltipMode.OnMouseOver
            OverlayCarga.Markers.Add(NuevoMarker)
            Mapa.Position = New PointLatLng(CDbl(Punto.Lat), CDbl(Punto.Lon))
        Else
            For Each marker As GMapMarker In OverlayCarga.Markers
                If CInt(marker.Tag) = mID Then
                    If e.Item.Checked = False Then
                        OverlayCarga.Markers.Remove(marker)
                        Exit For
                    End If
                End If
            Next
        End If

        ' mkrActual()
        'OverlayCarga.Markers.Remove()
    End Sub

    Private Sub Mapa_OnMarkerClick(item As GMap.NET.WindowsForms.GMapMarker, e As System.Windows.Forms.MouseEventArgs) Handles Mapa.OnMarkerClick

        Dim nomOverlay As String = item.Overlay.Id

        Select Case nomOverlay
            Case "OverlayBusqueda"
                OverlayBusqueda.Markers.Remove(item)
            Case "OverlayClick"
                OverlayClick.Markers.Remove(item)
        End Select

    End Sub

    Private Sub txtLugar_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtLugar.KeyPress
        If e.KeyChar = Chr(13) Then
            BotonBuscar(sender, e)
        End If
    End Sub

    Private Sub AcercaDeToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AcercaDeToolStripMenuItem.Click
        About.Show(Me)
    End Sub

    Private Sub txtDistancia_LostFocus(sender As Object, e As System.EventArgs) Handles txtDistancia.LostFocus
        If txtDistancia.Text = "" Then
            txtDistancia.Text = "10"
        End If
    End Sub

    Private Sub USBToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles USBToolStripMenuItem.Click
        If ÓpticoToolStripMenuItem.Checked Then
            USBToolStripMenuItem.Checked = True
            ÓpticoToolStripMenuItem.Checked = False
        End If
        chkNBM550.Checked = True
        BaudRate = 460800
    End Sub

    Private Sub ÓpticoToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ÓpticoToolStripMenuItem.Click
        If USBToolStripMenuItem.Checked Then
            USBToolStripMenuItem.Checked = False
            ÓpticoToolStripMenuItem.Checked = True
        End If
        chkNBM550.Checked = True
        BaudRate = 115200
    End Sub

    Private Sub ImportarDatosDeMapaToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ImportarDatosDeMapaToolStripMenuItem.Click
        Mapa.ShowImportDialog()
        Mapa.ReloadMap()
        'Mapa.Refresh()
    End Sub

    Private Sub ExportarDatosDeMapaToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExportarDatosDeMapaToolStripMenuItem.Click
        Mapa.ShowExportDialog()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Clipboard.SetText(txtEventos.Text)
    End Sub

    Private Sub EstablecerAlarmaToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EstablecerAlarmaToolStripMenuItem.Click
        Try
            If EstablecerAlarmaToolStripMenuItem.Checked = False Then
                NivelAlarma = CSng(InputBox("Ingrese el nivel en V/m a partir del cual hacer sonar la alarma (0 para desactivar):", "Alarma", "27,5"))
                If NivelAlarma > 0 Then
                    hayAlarma = True
                    EstablecerAlarmaToolStripMenuItem.Checked = True
                    txtEventos.Text &= "[" & Now & "] " & "Alarma de nivel activada en " & NivelAlarma & " V/m." & vbNewLine
                Else
                    hayAlarma = False
                    EstablecerAlarmaToolStripMenuItem.Checked = False
                    txtEventos.Text &= "[" & Now & "] " & "Alarma de nivel desactivada." & vbNewLine
                End If
            Else
                txtEventos.Text &= "[" & Now & "] " & "Alarma de nivel desactivada." & vbNewLine
                EstablecerAlarmaToolStripMenuItem.Checked = False
                hayAlarma = False
            End If
        Catch ex As Exception
            If ex.Message.Contains("en el tipo 'Single' no es válida") Then
                MsgBox("Se ha desactivado la alarma. Para activarla, introduzca un valor mayor a cero", vbExclamation)
                hayAlarma = False
                EstablecerAlarmaToolStripMenuItem.Checked = False
            End If
        End Try
    End Sub

    Private Sub Garmin18XUSBToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles Garmin18XUSBToolStripMenuItem.Click
        If Garmin18XUSBToolStripMenuItem.Checked = False Then
            Garmin18XUSBToolStripMenuItem.Checked = True
            DispositivoNMEAToolStripMenuItem.Checked = False
            GPSSel = 1
            Label6.Text = "Estado del GPS [18X USB]"
            'comGPS.Close()
        End If
        tmrGPS.Interval = 100
    End Sub

    Private Sub cboCOMGlobalSat_Click(sender As System.Object, e As System.EventArgs) Handles cboCOMGlobalSat.Click
        If cboCOMGlobalSat.Text = "" Then Exit Sub
        tmrGPS.Interval = 1200
        If DispositivoNMEAToolStripMenuItem.Checked = False Then
            DispositivoNMEAToolStripMenuItem.Checked = True
            Garmin18XUSBToolStripMenuItem.Checked = False
            Label6.Text = "Estado del GPS [NMEA]"
            GPSSel = 2
        End If
        If comGPS.IsOpen Then
            comGPS.Close()
        End If
        comGPS.PortName = cboCOMGlobalSat.Text
    End Sub

    Private Sub cboCOMGlobalSat_DropDownClosed(sender As Object, e As System.EventArgs) Handles cboCOMGlobalSat.DropDownClosed
        If cboCOMGlobalSat.Text = "" Then Exit Sub
        tmrGPS.Interval = 1200
        If DispositivoNMEAToolStripMenuItem.Checked = False Then
            DispositivoNMEAToolStripMenuItem.Checked = True
            Garmin18XUSBToolStripMenuItem.Checked = False
            Label6.Text = "Estado del GPS [NMEA]"
            GPSSel = 2
        End If
        If comGPS.IsOpen Then
            comGPS.Close()
        End If
        comGPS.PortName = cboCOMGlobalSat.Text
    End Sub

    Private Sub cboCOMGlobalSat_MouseDown(sender As Object, e As System.EventArgs) Handles cboCOMGlobalSat.MouseDown
        If cboCOMGlobalSat.Text = "" Then Exit Sub
        tmrGPS.Interval = 1200
        If DispositivoNMEAToolStripMenuItem.Checked = False Then
            DispositivoNMEAToolStripMenuItem.Checked = True
            Garmin18XUSBToolStripMenuItem.Checked = False
            Label6.Text = "Estado del GPS [NMEA]"
            GPSSel = 2
        End If
        If comGPS.IsOpen Then
            comGPS.Close()
        End If
        comGPS.PortName = cboCOMGlobalSat.Text
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        MsgBox("Directorio del caché: " & Mapa.CacheLocation, MsgBoxStyle.Information)
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        frmDebug.Show(Me)
    End Sub

    Private Sub txtDistancia_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtDistancia.TextChanged
        If txtDistancia.Text = "" Then
            txtDistancia.Text = "10"
        End If
    End Sub

    Private Sub chkMaxHold_Click(sender As System.Object, e As System.EventArgs) Handles chkMaxHold.Click
        chkMaxAvg.Checked = False
        chkActual.Checked = False
    End Sub

    Private Sub chkMaxAvg_Click(sender As System.Object, e As System.EventArgs) Handles chkMaxAvg.Click
        chkMaxHold.Checked = False
        chkActual.Checked = False
    End Sub

    Private Sub chkActual_Click(sender As System.Object, e As System.EventArgs) Handles chkActual.Click
        chkMaxAvg.Checked = False
        chkMaxHold.Checked = False
    End Sub
#End Region

#Region "Procedimientos varios"
    ''' <summary>
    ''' Carga los números de serie de las sondas de ambos modelos para determinar las incertidumbres y límites de integración de frecuencia
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarSondas()
        Dim i As Integer
        Dim veclinea(), inBuffer, ruta As String

        '- SONDAS DEL EMR300
        ruta = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\e300amrni.txt" 'Path.GetDirectoryName(Application.ExecutablePath) & "\sondasemr300.txt"
        Using SR As StreamReader = New StreamReader(ruta)
            'La primer linea se ignora porque son los titulos de los campos en el txt
            'Los datos de un registro están separados entre si por UNA (1) TABULACION
            inBuffer = SR.ReadLine
            For i = 0 To 17
                inBuffer = SR.ReadLine
                veclinea = Split(inBuffer, Chr(9))
                With Sondas300(i)
                    .Tipo = CInt(veclinea(0))
                    .NumSerie = veclinea(1)
                    .LimInf = CSng(veclinea(2).Replace(".", ","))
                    .LimSup = CSng(veclinea(3).Replace(".", ","))
                    .Incert = CSng(veclinea(4).Replace(".", ","))
                    .Factor = CSng(veclinea(5).Replace(".", ","))
                End With
            Next
        End Using

        '-SONDAS DEL NBM550
        i = 0
        ruta = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\n550amrni.txt" 'Path.GetDirectoryName(Application.ExecutablePath) & "\sondasnbm550.txt"
        Using SR As StreamReader = New StreamReader(ruta)
            'La primer linea se ignora porque son los titulos de los campos en el txt
            'Los datos de un registro están separados entre si por UNA (1) TABULACION
            inBuffer = SR.ReadLine
            inBuffer = SR.ReadLine 'ESTA LINEA YA SE TIENE EN CUENTA
            Do Until inBuffer = Nothing
                veclinea = Split(inBuffer, Chr(9))
                With Sondas550(i)
                    .Nombre = veclinea(0)
                    .LimInf = CSng(veclinea(1))
                    .LimSup = CSng(veclinea(2))
                    .Incert = CSng(veclinea(3).Replace(".", ",")) 'en dBs
                    .VecesDB = CSng(veclinea(4).Replace(".", ",")) 'Factor para multiplicar al valor medido
                End With
                inBuffer = SR.ReadLine
                i += 1
            Loop
        End Using

        '-SONDAS DEL NBM550 PARA SPRINTER
        Try
            i = 0
            ruta = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\n550_spamrni.txt" 'Path.GetDirectoryName(Application.ExecutablePath) & "\sondasnbm550.txt"
            Using SR As StreamReader = New StreamReader(ruta)
                'La primer linea se ignora porque son los titulos de los campos en el txt
                'Los datos de un registro están separados entre si por UNA (1) TABULACION
                inBuffer = SR.ReadLine
                inBuffer = SR.ReadLine 'ESTA LINEA YA SE TIENE EN CUENTA
                Do Until inBuffer = Nothing
                    veclinea = Split(inBuffer, Chr(9))
                    With Sondas550_SP(i)
                        .Nombre = veclinea(0)
                        .LimInf = CSng(veclinea(1))
                        .LimSup = CSng(veclinea(2))
                        .Incert = CSng(veclinea(3).Replace(".", ",")) 'en dBs
                        .VecesDB = CSng(veclinea(4).Replace(".", ",")) 'Factor para multiplicar al valor medido
                    End With
                    inBuffer = SR.ReadLine
                    i += 1
                Loop
            End Using
        Catch ex As Exception

        End Try
        ContSondas500 = i - 1
    End Sub
    ''' <summary>
    ''' Carga los colores predeterminados para la paleta de puntos (AUN NO IMPLEMENTADO)
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarPaleta()
        'Los niveles para los colores se consideran "MAYOR O IGUAL QUE"
        'Rojo si nivel >= 27.5
        'Azul si nivel >= 0 , etc..
        Paleta(0).Color = "Rojo"
        Paleta(0).Nivel = 27.5

        Paleta(1).Color = "Naranja"
        Paleta(1).Nivel = 19

        Paleta(2).Color = "Amarillo"
        Paleta(2).Nivel = 13

        Paleta(3).Color = "Púrpura"
        Paleta(3).Nivel = 9

        Paleta(4).Color = "Verde"
        Paleta(4).Nivel = 6

        Paleta(5).Color = "Celeste"
        Paleta(5).Nivel = 4

        Paleta(6).Color = "Azul"
        Paleta(6).Nivel = 0

        PaletaRNI(0) = &HFF73C2FB ' < 1%
        PaletaRNI(1) = &HFF1E90FF ' <= 2%
        PaletaRNI(2) = &HFF2A52BE ' <= 4%
        PaletaRNI(3) = &HFF90EE90 ' <= 8%
        PaletaRNI(4) = &HFF32CD32 ' <= 15%
        PaletaRNI(5) = &HFF008000 ' <= 20%
        PaletaRNI(6) = &HFFFFDF00 ' <= 35%
        PaletaRNI(7) = &HFFFFA500 ' <= 50%
        PaletaRNI(8) = &HFFFF4500 ' <= 100%
        PaletaRNI(9) = &HFFFF0000 ' > 100%
    End Sub

    Function SeleccionarDestino() As Boolean
        Try
            Dim RutaFichero As String

            If chkMaxHold.Checked = True Then
                sDialog.Filter = "Mediciones RNI de valores máximos (*.mrni)|*.mrni"
            ElseIf chkMaxAvg.Checked = True Then
                sDialog.Filter = "Mediciones RNI de valores promediados (*.prni)|*.prni"
            End If
            If sDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then
                Return False
            End If
            
            RutaFichero = sDialog.FileName
            FileSystem.FileOpen(1, RutaFichero, OpenMode.Output)
            FileClose(1)
            'Se asigna a la variable publica la ruta donde se van a guardar los resultados de las mediciones
            RutaRaizRes = RutaFichero
            Return True
        Catch ex As Exception
            Beep()
            txtEventos.Text &= "[" & Now & "] " & "Ha ocurrido una excepción: " & ex.Message & vbNewLine
            Return False
        End Try
    End Function

    Sub ProcesoBuscar()
        Mapa.Manager.Mode = ModoConexion
        Mapa.Zoom = UltimoZoomLvl
        GMapProvider.Language = LanguageType.Spanish
        ' AHORA SE BUSCA EL LUGAR PARA POSICIONAR EL MAPA
        If BuscoCoor Then
            Mapa.Position = PosicionCoor
        Else
            Mapa.SetPositionByKeywords(PosicionStr)
        End If
        BuscoCoor = False
    End Sub

    Sub ProcesoBuscarHome()
        SetProvMapa()
        SetModoConexion()
        ProcesoUbicar = New Thread(AddressOf UbicarHome)
        ProcesoUbicar.IsBackground = True
        ProcesoUbicar.Start()
    End Sub

    Sub Ubicar()
        Mapa.Manager.Mode = ModoConexion
        Mapa.Zoom = trkZoom.Value
        lblZoom.Text = Mapa.Zoom.ToString
        Mapa.Dock = DockStyle.Fill
        GMapProvider.Language = LanguageType.Spanish
        ' AHORA SE BUSCA EL LUGAR PARA POSICIONAR EL MAPA
        If BuscoCoor Then
            Mapa.Position = UltimoPunto
        Else
            Mapa.SetPositionByKeywords(PosicionStr)
        End If
        BuscoCoor = False
        BuscoHome = False
    End Sub

    Sub SetModoConexion()
        If Me.cboModoConexion.Text = "Peticiones al servidor" Then
            ModoConexion = AccessMode.ServerOnly
        ElseIf Me.cboModoConexion.Text = "Leer desde caché" Then
            ModoConexion = AccessMode.CacheOnly
        Else
            ModoConexion = AccessMode.ServerAndCache
        End If
    End Sub

    Sub SetProvMapa()
        If Me.cboProvMapa.Text = "Google Maps" Then
            ProvMapa = GMapProviders.GoogleMap
        ElseIf Me.cboProvMapa.Text = "Google Hybrid" Then
            ProvMapa = GMapProviders.GoogleHybridMap
        Else
            ProvMapa = GMapProviders.OpenStreetMap
        End If
        Mapa.MapProvider = ProvMapa
    End Sub

    Sub UbicarHome()
        Mapa.Manager.Mode = ModoConexion
        Mapa.MapProvider = ProvMapa
        Mapa.Zoom = trkZoom.Value
        lblZoom.Text = Mapa.Zoom.ToString
        Mapa.Dock = DockStyle.Fill
        GMapProvider.Language = LanguageType.Spanish
        Posicionar(BuscoCoor)
    End Sub

    Sub Configurar()
        Mapa.Manager.Mode = ModoConexion
        Mapa.MapProvider = ProvMapa
        Mapa.Zoom = 15
        lblZoom.Text = Mapa.Zoom.ToString
        Mapa.Dock = DockStyle.Fill
        GMapProvider.Language = LanguageType.Spanish
    End Sub

    Sub Posicionar(esCoordenada As Boolean)
        If esCoordenada Then
            Mapa.Position = CoorAUbicar
            'Mapa.Position = UltimoPunto
        Else
            Mapa.SetPositionByKeywords(PosicionStr)
        End If
        BuscoCoor = False
        BuscoHome = False
    End Sub

    Sub Retardo(MiliSegundos As Integer)
        tmrRetardo.Interval = MiliSegundos
        tmrRetardo.Enabled = True
        Do Until tmrRetardo.Enabled = False
            Application.DoEvents()
        Loop
    End Sub

    ''' <summary>
    ''' Función que hizo MD para completar las coordenadas del gps GARMIN cuando le faltan dígitos y evitar errores
    ''' </summary>
    ''' <param name="latLon"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CompletarDesdeString(latLon As String) As CoordenadasGMS
        Dim resultado As New CoordenadasGMS()

        If (String.IsNullOrWhiteSpace(latLon)) Or latLon = Nothing Then
            Return resultado
        End If

        REM Ejemplo 1º 22' 33" N
        Dim vector As String() = latLon.Split(New Char() {Chr(&HB0)}) REM Chr(&HB0) = grados "\u00B0"
        resultado.Grados = Math.Abs(CInt(vector(0)))
        vector = vector(1).Split(New Char() {"'"})
        resultado.Minutos = Math.Abs(CInt(vector(0)))
        vector = vector(1).Split(New Char() {""""})
        resultado.Segundos = Math.Round(Math.Abs(CDbl(vector(0))), 3)
        resultado.Hemisf = vector(1).Trim()

        Return resultado
    End Function

#End Region

#Region "Timers"

    Private Sub tmrNarda_Tick(sender As System.Object, e As System.EventArgs) Handles tmrNarda.Tick
        'PRUEBA PARA VER SI USANDO HILOS SE SOLUCIONA EL PROBLEMA DE TILDAR EL TMR QUE LEE AL NARDA CUANDO SE ARRASTRA
        'LA VISTA DE MAPA CON EL MOUSE
        Try
            Select Case ProcesoLeerNarda.ThreadState
                Case ThreadState.Stopped
                    ProcesoLeerNarda = Nothing
                    ProcesoLeerNarda = New Thread(AddressOf LeerNarda)
                    Application.DoEvents()
                    ProcesoLeerNarda.Start()
                Case ThreadState.Aborted
                    ProcesoLeerNarda = Nothing
                    ProcesoLeerNarda = New Thread(AddressOf LeerNarda)
                    Application.DoEvents()
                    ProcesoLeerNarda.Start()
                Case ThreadState.Unstarted
                    ProcesoLeerNarda.Start()
                Case ThreadState.AbortRequested
                    While ProcesoGPS.IsAlive
                        Application.DoEvents()
                    End While
                Case ThreadState.WaitSleepJoin
                    ProcesoLeerNarda.Abort()
                    While ProcesoLeerNarda.IsAlive
                        Application.DoEvents()
                    End While
            End Select
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & vbNewLine & ex.StackTrace)
        End Try

    End Sub

    Private Sub tmrGPS_Tick(sender As System.Object, e As System.EventArgs) Handles tmrGPS.Tick
        'Setear el intervalo en 100 milisegundos para una respuesta rápida de localización
        'y actualización de posición en tiempo real (con el GARMIN 18X USB)
        'El GPS GlobalSat debería tener dwells de por lo menos 1000 ms
        Select ProcesoGPS.ThreadState
            Case ThreadState.Stopped
                ProcesoGPS = Nothing
                ProcesoGPS = New Thread(AddressOf LeerGPS)
                ProcesoGPS.Start()
            Case ThreadState.Aborted
                ProcesoGPS = Nothing
                ProcesoGPS = New Thread(AddressOf LeerGPS)
                ProcesoGPS.Start()
            Case ThreadState.Unstarted
                ProcesoGPS.Start()
            Case ThreadState.AbortRequested
                While ProcesoGPS.IsAlive
                    Application.DoEvents()
                End While
            Case ThreadState.WaitSleepJoin
                ProcesoGPS.Abort()
                While ProcesoGPS.IsAlive
                    Application.DoEvents()
                End While
        End Select
    End Sub

    Private Sub tmrCamp_Tick(sender As System.Object, e As System.EventArgs) Handles tmrCamp.Tick
        tmrCamp.Enabled = False
    End Sub

    Private Sub tmrRetardo_Tick(sender As System.Object, e As System.EventArgs) Handles tmrRetardo.Tick
        tmrRetardo.Enabled = False
    End Sub

#End Region

#Region "Threads - Hilos"

    Sub LeerGPS()

        Try
            Dim latdec, lngdec As Double
            Dim lat, lng, status As String
            Dim MarkerPosActual As GMapMarker
            If Me.InvokeRequired Then
                Application.DoEvents()
                Me.Invoke(New LeerGPSthread(AddressOf LeerGPS))
            Else
                ' ------------------------------------------------------------------------------------------------------------
                If ModoDebug.Checked Then
                    ' SE GENERA UNA POSICIÓN FIJA EN EL CCTEBA PARA SIMULAR POSICION ESTABLECIDA, SOLAMENTE PARA PRUEBAS
                    lblStatusGPS.Text = "DEBUGGING"
                    lblStatusGPS.BackColor = Color.BlueViolet
                    With LatitudActual
                        .Grados = 34
                        .Minutos = 45
                        .Segundos = 2
                        .Hemisf = "S"
                    End With
                    With LongitudActual
                        .Grados = 58
                        .Minutos = 29
                        .Segundos = 56
                        .Hemisf = "O"
                    End With

                    OverlayPosActual.Markers.Clear()

                    latdec = ConvertirAGDec(LatitudActual)
                    lngdec = ConvertirAGDec(LongitudActual)

                    CoorAUbicar.Lat = latdec
                    CoorAUbicar.Lng = lngdec

                    MarkerPosActual = New GMarkerGoogle(CoorAUbicar, GMarkerGoogleType.arrow)
                    MarkerPosActual.ToolTipText = "Posición actual" & vbNewLine & vbNewLine & _
                        LatitudActual.Grados & "° " & LatitudActual.Minutos & "' " & Math.Round(LatitudActual.Segundos, 1) & Chr(34) & " " & LatitudActual.Hemisf & vbNewLine & _
                        LongitudActual.Grados & "° " & LongitudActual.Minutos & "' " & Math.Round(LongitudActual.Segundos, 1) & Chr(34) & " " & LongitudActual.Hemisf
                    MarkerPosActual.ToolTipMode = MarkerTooltipMode.OnMouseOver

                    OverlayPosActual.Markers.Add(MarkerPosActual)
                    If chkAutoLoc.Checked = True Then
                        BuscoCoor = True
                        Posicionar(BuscoCoor)
                    End If
                    With LatitudActual
                        txtLatActual.Text = .Grados & "° " & .Minutos & "' " & .Segundos & Chr(34) & " " & .Hemisf
                    End With
                    With LongitudActual
                        txtLngActual.Text = .Grados & "° " & .Minutos & "' " & .Segundos & Chr(34) & " " & .Hemisf
                    End With
                    Application.DoEvents()
                    If Not StatusGpS Then
                        My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
                        txtEventos.Text &= "[" & Now & "] " & "<GPS POSICIONADO EN MODO DEBUG>" & vbNewLine
                        StatusGpS = True
                        Application.DoEvents()
                    End If
                    Exit Sub
                End If
                ' ------------------------------------------------------------------------------------------------------------

                If GPSSel = 1 Then '1 = USB Garmin
                    status = EquipoGpsNmea.InfoGPSConectado
                    lat = EquipoGpsNmea.LatitudStr
                    lng = EquipoGpsNmea.LongitudStr
                    If frmDebug.debug.Checked = True Then frmDebug.TextBox1.Text &= ">>GARMIN Status: " & status & " - LAT:" & lat & " - LNG:" & lng & vbNewLine
                    If status = Nothing Or status = "Demo" Then
                        lat = "--"
                        lng = "--"
                        lblStatusGPS.Text = "Desconectado"
                        lblStatusGPS.BackColor = Color.LightGray
                        If StatusGpS Then
                            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Hand)
                            Beep()
                            OverlayPosActual.Markers.Clear()
                            StatusGpS = False
                            txtEventos.Text &= "[" & Now & "] " & "GPS desconectado." & vbNewLine
                        End If
                        txtLatActual.Text = "--"
                        txtLngActual.Text = "--"
                        Application.DoEvents()
                        Exit Sub
                    Else
                        If lat = Nothing Or lat = "?" Then
                            lblStatusGPS.Text = "Buscando posición"
                            lblStatusGPS.BackColor = Color.Orange
                            If StatusGpS Then
                                OverlayPosActual.Markers.Clear()
                                Beep()
                                txtEventos.Text &= "[" & Now & "] " & "GPS sin posición." & vbNewLine
                                txtLatActual.Text = "--"
                                txtLngActual.Text = "--"
                                StatusGpS = False
                            End If
                            Application.DoEvents()
                            Exit Sub
                        Else
                            lblStatusGPS.Text = "Posicionado"
                            lblStatusGPS.BackColor = Color.GreenYellow
                            If Not StatusGpS Then
                                My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
                                txtEventos.Text &= "[" & Now & "] " & "GPS posicionado." & vbNewLine
                                StatusGpS = True
                                Application.DoEvents()
                            End If
                        End If
                    End If
                    '---------------------------------------------------------------------------
                    '---------------------------------------------------------------------------
                    'ACA SE GUARDA LA POSICION GEOGRAFICA ACTUAL
                    'EN LATITUDACTUAL Y LONGITUDACTUAL
                    LatitudActual = CompletarDesdeString(lat)
                    LongitudActual = CompletarDesdeString(lng)
                    '---------------------------------------------------------------------------
                    '---------------------------------------------------------------------------

                ElseIf GPSSel = 2 Then '2 = NMEA GlobalSat BU-353S4
                    Dim reintHechos As Integer
                    If Not comGPS.IsOpen Then
                        comGPS.Open()
                    End If

                    Dim inBufferGPS As String
                    'Dim lat, lng, status As String
ReIntGSAT:
                    If reintHechos > 5 Then
                        Throw New ApplicationException("Demasiados reintentos de conexión del GPS.")
                    End If
                    inBufferGPS &= comGPS.ReadExisting
                    If frmDebug.debug.Checked = True Then frmDebug.TextBox1.Text &= ">>GlobalSAT inbuffer: " & inBufferGPS & vbNewLine
                    If inBufferGPS = "" Then
                        lblStatusGPS.Text = "Desconectado"
                        lblStatusGPS.BackColor = Color.LightGray
                        If StatusGpS Then
                            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Hand)
                            Beep()
                            OverlayPosActual.Markers.Clear()
                            StatusGpS = False
                            txtEventos.Text &= "[" & Now & "] " & "GPS desconectado." & vbNewLine
                        End If
                        txtLatActual.Text = "--"
                        txtLngActual.Text = "--"
                        Exit Sub
                    End If
                    'Se busca la ultima aparicion de la sentencia $GPRMC y se corta a partir de ahi para obtener informacion de posicion
                    'EJ: 
                    '(0)$GPRMC,         Sentencia RMC
                    '(1)123519,         Hora UTC
                    '(2)A,              A=Activo - V= Void/inactivo/sin fix
                    '(3)(4)4807.038,N,  Lat (GGMM,MMM), Norte
                    '(5)(6)01131.000,E, Lon (GGGMM,MMM), Este
                    '(5)022.4,          Velocidad en nudos
                    '(6)084.4,          Acimut de avance?
                    '(7)230394,         Fecha
                    '(8)003.1,W,        Variacion magnetica
                    '(9)*6A             CheckSum
                    '--------------------------------------------------------------------------

                    'En vez de definir un largo predeterminado de string, se buscan los limites para usar la cantidad
                    'de caracteres exactos
                    Dim posRMC As Integer = inBufferGPS.LastIndexOf("$GPRMC")
                    If posRMC = -1 Then
                        reintHechos = +1
                        GoTo ReIntGSAT
                    End If
                    inBufferGPS = inBufferGPS.Substring(posRMC)
                    Dim finRMC As Integer = inBufferGPS.IndexOf(vbCrLf)
                    If finRMC = -1 Then
                        reintHechos = +1
                        GoTo ReIntGSAT
                    End If
                    inBufferGPS = inBufferGPS.Substring(0, finRMC)
                    Dim arrayGPS As String() = Split(inBufferGPS, ",")
                    '--------------------------------------------------------------------------------------
                    'CHECKSUM MD5 PARA COMPROBAR SI HUBO ERROR DE TRANSMISIÓN
                    'SI SE DETECTA ERROR, SE DESCARTA LA LECTURA Y ESPERA A UNA NUEVA LLEGADA DE DATOS NMEA
                    Dim strAChequear As String = inBufferGPS.Substring(1, Len(inBufferGPS) - 4)
                    Dim md5 As String = CalcMD5(strAChequear)
                    If md5 <> arrayGPS(12).Substring(2, 2) Then Exit Sub
                    '--------------------------------------------------------------------------------------
                    status = arrayGPS(2)
                    If status = "V" Then
                        lblStatusGPS.Text = "Buscando posición"
                        lblStatusGPS.BackColor = Color.Orange
                        If StatusGpS Then
                            OverlayPosActual.Markers.Clear()
                            Beep()
                            txtEventos.Text &= "[" & Now & "] " & "GPS sin posición." & vbNewLine
                            txtLatActual.Text = "--"
                            txtLngActual.Text = "--"
                            StatusGpS = False
                        End If
                        Exit Sub
                    ElseIf status = "A" Then
                        Dim gra, dec As String
                        gra = arrayGPS(3).Substring(0, 2)
                        dec = arrayGPS(3).Substring(2, Len(arrayGPS(3)) - 2).Replace(".", ",")
                        dec = CSng(dec) / 60
                        If arrayGPS(4).Contains("S") Then
                            lat = "-" & gra & "," & dec.Substring(2, Len(dec) - 2).ToString
                        Else
                            lat = gra & "," & dec.Substring(2, Len(dec) - 2).ToString
                        End If

                        gra = arrayGPS(5).Substring(0, 3)
                        dec = arrayGPS(5).Substring(3, Len(arrayGPS(5)) - 3).Replace(".", ",")
                        dec = CSng(dec) / 60
                        If arrayGPS(6).Contains("W") Then
                            lng = "-" & gra & "," & dec.Substring(2, Len(dec) - 2).ToString
                        Else
                            lng = gra & "," & dec.Substring(2, Len(dec) - 2).ToString
                        End If

                        LatitudActual = ConvertirAGMS(CDbl(lat), False)
                        LongitudActual = ConvertirAGMS(CDbl(lng), True)
                        lblStatusGPS.Text = "Posicionado"
                        lblStatusGPS.BackColor = Color.GreenYellow
                        If Not StatusGpS Then
                            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
                            txtEventos.Text &= "[" & Now & "] " & "GPS posicionado." & vbNewLine
                            StatusGpS = True
                        End If
                    End If
                End If

                '---------------------------------------------------------------------------
                '---------------------------------------------------------------------------
                'ACA SE GUARDA LA POSICION GEOGRAFICA ACTUAL
                'EN LATITUDACTUAL Y LONGITUDACTUAL
                'LatitudActual = CompletarDesdeString(lat)
                'LongitudActual = CompletarDesdeString(lng)
                '---------------------------------------------------------------------------
                '---------------------------------------------------------------------------

                'Dim latdec As Double = ConvertirAGDec(LatitudActual)
                'Dim lngdec As Double = ConvertirAGDec(LongitudActual)
                latdec = ConvertirAGDec(LatitudActual)
                lngdec = ConvertirAGDec(LongitudActual)
                CoorAUbicar.Lat = latdec
                CoorAUbicar.Lng = lngdec

                OverlayPosActual.Markers.Clear()
                'Dim MarkerPosActual As GMapMarker = New GMarkerGoogle(CoorAUbicar, GMarkerGoogleType.arrow)
                MarkerPosActual = New GMarkerGoogle(CoorAUbicar, GMarkerGoogleType.arrow)
                MarkerPosActual.ToolTipText = "Posición actual" & vbNewLine & vbNewLine & _
                    LatitudActual.Grados & "° " & LatitudActual.Minutos & "' " & Math.Round(LatitudActual.Segundos, 1) & Chr(34) & " " & LatitudActual.Hemisf & vbNewLine & _
                    LongitudActual.Grados & "° " & LongitudActual.Minutos & "' " & Math.Round(LongitudActual.Segundos, 1) & Chr(34) & " " & LongitudActual.Hemisf
                MarkerPosActual.ToolTipMode = MarkerTooltipMode.OnMouseOver

                OverlayPosActual.Markers.Add(MarkerPosActual)
                If chkAutoLoc.Checked = True Then
                    BuscoCoor = True
                    Posicionar(BuscoCoor)
                End If
                With LatitudActual
                    txtLatActual.Text = .Grados & "° " & .Minutos & "' " & .Segundos & Chr(34) & " " & .Hemisf
                End With
                With LongitudActual
                    txtLngActual.Text = .Grados & "° " & .Minutos & "' " & .Segundos & Chr(34) & " " & .Hemisf
                End With
                Application.DoEvents()
                End If
        Catch ex As Exception
            If ex.Message.Contains("Datos no válidos") Then
                Application.DoEvents()
                Exit Sub
            ElseIf ex.Message.Contains("reintentos") Then
                comGPS.Close()
            End If
            'txtEventos.Text &= "[" & Now & "] " & "Excepción ocurrida: " & ex.Message & vbNewLine
            Application.DoEvents()
            'trace.GetFrame(0).GetFileLineNumber().ToString & vbNewLine  'ex.TargetSite.ToString & vbNewLine
        Finally

        End Try

    End Sub

    Sub LeerNarda()

        Dim VecNarda As String()
        Dim ContError As Integer
        Dim inBuffer As String
        'Dim bufferNivel As Single 'Se guarda localmente la lectura actual y se compara para saber si reemplazar el máximo (con maxhold = on)
        Try
ReStartNarda:
            If Me.InvokeRequired Then
                Me.Invoke(New LeerNardaThread(AddressOf LeerNarda))
            Else
                If Conectado Then
                    With comNarda
                        
                        If chkNBM550.Checked Then
                            If chkPausa.Checked Then
                                ' SI ESTA EN PAUSA, PEDIR EL VALOR RESETEA EL MAX HOLD
                                .WriteLine("RESET_MAX;")
                                Retardo(200)
                                'Application.DoEvents()
                                inBuffer = .ReadExisting
                                Exit Sub
                            End If
                            .WriteLine("MEAS?;")
                            Retardo(200)
                            VecNarda = Split(.ReadExisting, ",")
                            Application.DoEvents()
                            If chkActual.Checked Then
                                NivelNarda = Format(CSng(VecNarda(1) / 1000), "##0.000")  'LINEA PARA LEER ACTUAL
                                lblTipoRes.Text = "Actual"
                            ElseIf chkMaxHold.Checked Then
                                NivelNarda = Format(CSng(VecNarda(0) / 1000), "##0.000")   'LINEA PARA LEER MAXHOLD
                                lblTipoRes.Text = "Max hold"
                            End If
                            lblDisplay.Text = NivelNarda & " " & UnidadActual
                            .WriteLine("BATTERY?;")
                            Retardo(200)
                            inBuffer = .ReadExisting
                            Application.DoEvents()
                            Select Case CSng(inBuffer.Substring(0, Len(inBuffer) - 2))
                                Case Is <= 10
                                    txtEventos.Text &= "[" & Now & "] " & _
                                        "EL INSTRUMENTO REPORTA BATERÍA BAJA! Reemplace o cargue las baterías cuanto antes - Capacidad restante: " & _
                                        inBuffer & " %" & vbNewLine
                                    picBateria.Image = My.Resources.bat_10
                                Case Is <= 25
                                    picBateria.Image = My.Resources.bat_25
                                Case Is <= 50
                                    picBateria.Image = My.Resources.bat_50
                                Case Is <= 75
                                    picBateria.Image = My.Resources.bat_75
                                Case Else
                                    picBateria.Image = My.Resources.bat_full
                            End Select
                            picBateria.Visible = True
                            ContError = 0
                            Application.DoEvents()
                        ElseIf chkEMR300.Checked Then
                            Try
                                .WriteLine("M")
                                Retardo(400)
                                VecNarda = Split(.ReadExisting, ",")
                            Catch exc As Exception
                                Exit Sub
                            End Try
                            Application.DoEvents()
                            .DiscardInBuffer()
                            'If chkMaxHold.Checked Then
                            'Try
                            'bufferNivel = Format(CSng(VecNarda(0).Replace(".", ",")), "##0.000")
                            'If bufferNivel > NivelNarda Then NivelNarda = bufferNivel
                            'Catch
                            'Exit Sub
                            'End Try
                            'Else
                            'Try
                            NivelNarda = Format(CSng(VecNarda(0).Replace(".", ",")), "##0.000")
                            'Catch
                            'Exit Sub
                            'End Try

                            If chkMaxHold.Checked Then
                                lblDisplay.Text = NivMax & " " & UnidadActual 'NivelNarda & " V/m"
                                lblTipoRes.Text = "Max hold"
                            ElseIf chkMaxAvg.Checked Then
                                lblDisplay.Text = NivelNarda & " " & UnidadActual
                                lblTipoRes.Text = "Max Avg"
                            End If
                            Try
                                .WriteLine("SYST:BAT?")
                                Retardo(100)
                                inBuffer = .ReadExisting
                            Catch
                            End Try
                            Application.DoEvents()
                            If inBuffer.Contains("LOW") Then
                                Try
                                    My.Computer.Audio.Play("C:\Windows\Media\Impresión completa de Windows.wav")
                                Catch
                                End Try
                                txtEventos.Text &= "[" & Now & "] " & "EL INSTRUMENTO REPORTA BATERÍA BAJA! Reemplazar o cargar baterías." & vbNewLine
                            End If
                            'If boolResetMaxEMR Then
                            'NivelNarda = 0
                            'bufferNivel = 0
                            'boolResetMaxEMR = False
                            'End If
                        End If
                        ContError = 0
                    End With
                End If
            End If
            'End If
        Catch ex As Exception
            'Beep()
            If ex.Message.Contains("418;") Then
                tmrNarda.Enabled = False
                txtEventos.Text &= "[" & Now & "] " & "Error con instrumento: No hay sonda conectada al mismo. Desconectando." & vbNewLine
                MsgBox("No hay sonda conectada al instrumento. Apague el instrumento, conéctela al mismo y reintente conexión remota.", vbExclamation)
                comNarda.WriteLine("REMOTE OFF;")
                comNarda.DiscardInBuffer()
                lblSonda.Text = ""
                lblInstrumento.Text = ""
                lblTipoRes.Text = ""
                btnConectar.Text = "Conectar"
                picBateria.Visible = False
                btnIniciarCamp.Enabled = False
                PictureBox1.Image = Nothing
                Conectado = False
                Exit Try
            ElseIf ex.Message.Contains("Uno de los dispositivos conectados al sistema no funciona") Then
                tmrNarda.Enabled = False
                txtEventos.Text &= "[" & Now & "] " & "Error con instrumento: Posible desconexión del cable." & vbNewLine
                MsgBox("No puede encontrarse instrumento conectado, el mismo parece haberse desconectado.", vbExclamation)
                lblSonda.Text = ""
                lblInstrumento.Text = ""
                lblTipoRes.Text = ""
                lblDisplay.Text = ""
                btnConectar.Text = "Conectar"
                btnIniciarCamp.Enabled = False
                PictureBox1.Image = Nothing
                picBateria.Visible = False
                Conectado = False
                Exit Try
            End If
            Application.DoEvents()
            ContError += 1
            If ContError > 3 Then '3 veces es alrededor de 3 segundos (con un intervalo de timer de 1000 ms entre lecturas)
                If Not ex.Message.Contains("de la matriz") Then
                    Try
                        If VecNarda(0).Contains("418;") Then
                            Application.DoEvents()
                            tmrNarda.Enabled = False
                            txtEventos.Text &= "[" & Now & "] " & "Error con instrumento: No hay sonda conectada al mismo. Desconectando." & vbNewLine
                            lblDisplay.Text = "- error -"
                            ' NivelNarda =-1 indica que ocurrio un error de lectura y que no está disponible el valor actual
                            NivelNarda = -1
                            lblTipoRes.Text = ""
                            PictureBox1.Image = Nothing
                            Conectado = False
                            btnConectar.Text = "Conectar"
                            MsgBox("No hay sonda conectada al instrumento.", vbExclamation)
                        End If
                    Catch exc As Exception
                        MsgBox(exc.Message)
                    End Try
                Else
                    Application.DoEvents()
                    txtEventos.Text &= "[" & Now & "] " & "Error con instrumento: " & ex.Message & " Deteniendo adquisición" & vbNewLine
                    tmrNarda.Enabled = False
                    lblDisplay.Text = "- error -"
                    ' NivelNarda =-1 indica que ocurrio un error de lectura y que no está disponible el valor actual
                    NivelNarda = -1
                    lblTipoRes.Text = ""
                    Conectado = False
                    MsgBox("No hay sonda conectada al instrumento. Apague el instrumento, conéctela al mismo y reintente conexión remota.", vbExclamation)
                    lblSonda.Text = ""
                    lblInstrumento.Text = ""
                    btnConectar.Text = "Conectar"
                End If
            Else
                GoTo ReStartNarda
            End If
        End Try

    End Sub
    ''' <summary>
    ''' Loop infinito paralelo a la ejecución de la app. Si hay un NARDA conectado, lee el nivel que el mismo arroja en el instante que se hace la lectura
    ''' </summary>
    ''' <remarks></remarks>
    Sub t_Narda()
        Dim VecNarda As String()
        Dim ContError As Integer
        Dim inBuffer As String
        Dim bufferNivel As Single 'Se guarda localmente la lectura actual y se compara para saber si reemplazar el máximo (con maxhold = on)
restart:
        Do
            If Me.InvokeRequired Then
                Me.Invoke(New LeerNardaThread(AddressOf t_Narda))
            Else
                Cursor = Cursors.Default
                If Conectado Then
                    With comNarda
                        If chkEMR300.Checked Then
                            If chkPausa.Checked Then
                                NivelNarda = 0
                                lblDisplay.Text = "..."
                                Exit Sub
                            End If
                            Try
                                .WriteLine("M")
                                Retardo(500)
                                VecNarda = Split(.ReadExisting, ",")
                            Catch ex As Exception
                                'MsgBox(ex.Message & " - " & ex.StackTrace)
                                Exit Do
                            End Try
                            'bufferNivel = 0
                            Application.DoEvents()
                            If chkMaxHold.Checked = True Then
                                Try
                                    bufferNivel = Format(CSng(VecNarda(0).Replace(".", ",")), "##0.000")
                                    If bufferNivel > NivelNarda Then NivelNarda = bufferNivel
                                Catch
                                    Exit Do
                                End Try
                            Else
                                Try
                                    NivelNarda = Format(CSng(VecNarda(0).Replace(".", ",")), "##0.000")
                                Catch ex As InvalidCastException
                                    Exit Do
                                End Try
                            End If
                            lblDisplay.Text = NivelNarda & " V/m"
                            lblTipoRes.Text = "Max hold"
                            Try
                                .WriteLine("SYST:BAT?")
                                Retardo(100)
                                inBuffer = .ReadExisting
                            Catch ex As Exception
                                'MsgBox(ex.Message & " - " & ex.StackTrace)
                                Exit Do
                            End Try
                            Application.DoEvents()
                            If inBuffer.Contains("LOW") Then
                                'Beep()
                                Try
                                    My.Computer.Audio.Play("C:\Windows\Media\Impresión completa de Windows.wav")
                                Catch
                                End Try
                                txtEventos.Text &= "[" & Now & "] " & "EL INSTRUMENTO REPORTA BATERÍA BAJA! Reemplazar o cargar baterías." & vbNewLine
                            End If
                            If boolResetMaxEMR Then
                                NivelNarda = 0
                                bufferNivel = 0
                                boolResetMaxEMR = False
                            End If
                            ContError = 0
                        ElseIf chkNBM550.Checked Then
                            If chkPausa.Checked Then
                                NivelNarda = 0
                                lblDisplay.Text = "..."
                                Exit Sub
                            End If
                            Try
                                .WriteLine("MEAS?;")
                                Retardo(200)
                                VecNarda = Split(.ReadExisting, ",")
                            Catch ex As Exception
                                'MsgBox(ex.Message & " - " & ex.StackTrace)
                                Exit Do
                            End Try
                            Application.DoEvents()
                            If chkActual.Checked Then
                                NivelNarda = Format(CSng(VecNarda(1) / 1000), "##0.000")  'LINEA PARA LEER ACTUAL
                            ElseIf chkMaxHold.Checked Then
                                NivelNarda = Format(CSng(VecNarda(0) / 1000), "##0.000")   'LINEA PARA LEER MAXHOLD
                            End If
                            lblDisplay.Text = NivelNarda & " V/m"
                            .WriteLine("BATTERY?;")
                            Retardo(200)
                            inBuffer = .ReadExisting
                            Application.DoEvents()
                            Select Case CSng(inBuffer.Substring(0, Len(inBuffer) - 2))
                                Case Is <= 10
                                    Try
                                        My.Computer.Audio.Play("C:\Windows\Media\Impresión completa de Windows.wav")
                                    Catch
                                    End Try
                                    txtEventos.Text &= "[" & Now & "] " & _
                                        "EL INSTRUMENTO REPORTA BATERÍA BAJA! Reemplace o cargue las baterías cuanto antes - Capacidad restante: " & _
                                        inBuffer & " %" & vbNewLine
                                    picBateria.Image = My.Resources.bat_10
                                Case Is <= 25
                                    picBateria.Image = My.Resources.bat_25
                                Case Is <= 50
                                    picBateria.Image = My.Resources.bat_50
                                Case Is <= 75
                                    picBateria.Image = My.Resources.bat_75
                                Case Else
                                    picBateria.Image = My.Resources.bat_full
                            End Select
                            picBateria.Visible = True
                            ContError = 0
                            Application.DoEvents()
                        End If
                    End With
                Else
                    NivelNarda = 0
                End If
                Application.DoEvents()
            End If
        Loop
        GoTo restart
    End Sub

    ''' <summary>
    ''' Actualiza el nivel máximo detectado por el EMR300 en un variable que la rutina de recorrido va a tomar como resultado final en un punto
    ''' </summary>
    ''' <remarks>Usar "boolResetMaxEMR" en la rutina de recorrido para finalizar la lectura actual y resetear el nivel a 0 V/m. Usar "boolKillThrMaxEMR" para terminar el thread</remarks>
    Sub t_EMRMAX()
        'If Me.InvokeRequired Then
        'Me.Invoke(New MaxEMRThread(AddressOf t_EMRMAX))
        'Else
        Do 'Until boolKillThrMaxEMR
            NivMax = 0
            boolResetMaxEMR = False
            Do While Not boolResetMaxEMR
                If NivelNarda > NivMax Then NivMax = NivelNarda
                Application.DoEvents()
            Loop
        Loop
        'End If
    End Sub

#End Region

End Class