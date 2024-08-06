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
Imports System.Configuration
Imports System.Threading
Imports System.Globalization
Imports System.IO
Imports System.IO.Ports
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
    Public CambioProv, CambioModoConexion, BuscoHome, BuscoCoor As Boolean
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

    Dim encryptionKey As String = ConfigurationSettings.AppSettings("EncryptionKey")

    Dim PaletaRNI(10) As Integer '0 el minimo, 9 el maximo
    'El hilo que se va a encargar de leer el NARDA evitando que se trabe el funcionamiento por arrastrar el mapa al mismo tiempo
    ' NECESITA DESHABILITAR EL CHECKILLEGALCROSSTHREAD
    'Dim LockThis As New Object
#End Region

#Region "Declaración de Threads (Subprocesos/hilos de ejecución) y delegados"
    Dim ProcesoLeerNarda As New Thread(AddressOf LeerNarda)
    Dim ProcesoEMRMax As New Thread(AddressOf t_EMRMax)
    Dim ProcesoUbicar As Thread
    Dim ProcesoGPS As New Thread(AddressOf LeerGPS)
    Delegate Sub LeerGPSthread()
    Delegate Sub LeerNardaThread()
    Delegate Sub MaxEMRThread()
#End Region

#Region "Subprocedimientos LoadForm y asociados a eventos de ventana"

    Sub nuevoMensajeEventos(msg As String)
        txtEventos.Invoke(Sub() txtEventos.Text &= "[" & Now & "] " & msg & vbNewLine)
    End Sub

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
            Me.Text = "AutoMap RNI - " & String.Format("v{0}.{1}.{2}", My.Application.Info.Version.Major.ToString, _
                                                       My.Application.Info.Version.Minor.ToString, My.Application.Info.Version.Build.ToString)
            Cursor = Cursors.WaitCursor
            nuevoMensajeEventos("Sistema iniciado")
            Try
                CargarSondas()
            Catch ex As Exception
                nuevoMensajeEventos("Error: no se encuentran los archivos de sondas. Contacte a Laboratorio Buenos Aires.")
                btnIniciarCamp.Enabled = False
                btnConectar.Enabled = False
            End Try

            cboIntervalo.SelectedItem = "5"
            comboGPSSelected.SelectedIndex = 0
            Mapa.IgnoreMarkerOnMouseWheel = True

            'Agrega todos los puertos COM disponibles en el ComboBox NARDA
            For Each port As String In My.Computer.Ports.SerialPortNames
                cboPuertoNarda.Items.Add(port)
                cboCOMGlobalSat.Items.Add(port)
            Next

            cboProvMapa.Text = "Google Maps"
            cboModoConexion.Text = "Servidor y caché"
            Mapa.MaxZoom = 19
            trkZoom.Maximum = Mapa.MaxZoom
            trkZoom.Minimum = Mapa.MinZoom

            ' Valores aleatorios para evitar el bloqueo de las APIs de mapa
            Dim random1 As Integer = CInt(Math.Floor(100 * Rnd())) + 1
            Dim random2 As Integer = CInt(Math.Floor(99 * Rnd())) + 1
            Dim random3 As Integer = CInt(Math.Floor(90 * Rnd())) + 1

            MapProviders.GMapProvider.UserAgent = _
                String.Format("Mozilla/5.0 (Windows NT {1}.0; {2}rv:{0}.0) Gecko/20100101 Firefox/{0}.0",
                              random1, random2, random3)

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
        Catch ex As Exception
            nuevoMensajeEventos("Excepción ocurrida en el inicio del sistema: " & ex.Message)
        Finally
            Cursor = Cursors.Arrow
        End Try
    End Sub

    Private Sub PrefetchAreaSeleccionadaToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PrefetchAreaSeleccionadaToolStripMenuItem.Click
        Try
            If ModoConexion = AccessMode.CacheOnly Then
                MsgBox("No es posible realizar la descarga si el modo de conexión es ''Solo caché''.", vbExclamation, "Aplicación en modo desconectado")
                Exit Sub
            End If
            If Mapa.SelectedArea.Size = New SizeLatLng(0, 0) Then
                MsgBox("Seleccione un área para descargar manteniendo SHIFT y arrastrando con el boton derecho del mouse.", vbInformation, "No hay area seleccionada")
                Exit Sub
            End If

            trkZoom.Value = Mapa.Zoom
            For zoom As Integer = Mapa.Zoom To Mapa.MaxZoom
                Using TP As TilePrefetcher = New GMap.NET.TilePrefetcher()
                    Mapa.Manager.Mode = AccessMode.ServerAndCache
                    TP.Overlay = ovlyPrefetch
                    TP.Owner = Me
                    TP.Start(Mapa.SelectedArea, zoom, Mapa.MapProvider, 10, 1)
                End Using

                ovlyPrefetch.Clear()
            Next
            Mapa.Manager.Mode = AccessMode.CacheOnly

        Catch ex As Exception
            nuevoMensajeEventos("Error en prefetching: " & ex.Message)
        Finally
            ovlyPrefetch.Clear()
        End Try
    End Sub

    Private Sub KeyCombination(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        ' Activación modo debug de GPS
        If e.Control And e.KeyCode = Keys.D Then
            If ModoDebug.Checked = False Then
                MsgBox("Modo debug activado. Generando GPS virtual e ignorando distancia mínima.")
                ModoDebug.Checked = True
            Else
                MsgBox("Modo debug desactivado.")
                ModoDebug.Checked = False
            End If
        End If

        ' Activación de ventana de debug de info GPS
        If e.Control And e.KeyCode = Keys.G Then 'And ModoDebug.Checked Then
            frmDebug.Show()
            frmDebug.BringToFront()
        End If

        ' Establecer o quitar pausa de recorrido
        If e.Control And e.KeyCode = Keys.P Then
            If chkPausa.Enabled Then
                chkPausa.Checked = Not chkPausa.Checked
            End If
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
        Dim outHeader As String
        Dim outReg As Registro
        Dim outBuffer As String
        Dim factorSonda As Single = SondaSel.Factor
        Dim outCrypt As String
        Dim Criptografo As Encriptador = New Encriptador(encryptionKey)

        ' Diccionario para crear los markers segun nivel
        Dim thresholds As Dictionary(Of Double, Tuple(Of GMarkerGoogleType, Integer))
        thresholds = New Dictionary(Of Double, Tuple(Of GMarkerGoogleType, Integer)) From {
            {27.5, Tuple.Create(GMarkerGoogleType.red, 0)},
            {20.0, Tuple.Create(GMarkerGoogleType.orange_dot, 1)},
            {14.0, Tuple.Create(GMarkerGoogleType.yellow_dot, 2)},
            {8.0, Tuple.Create(GMarkerGoogleType.purple_dot, 3)},
            {4.0, Tuple.Create(GMarkerGoogleType.green_dot, 4)},
            {2.0, Tuple.Create(GMarkerGoogleType.lightblue_dot, 5)},
            {0.0, Tuple.Create(GMarkerGoogleType.blue_dot, 6)}
        }

SeguirCampaña:
        Try
            'Se pone en cero la medicion del EMR para que empiece a tomar desde el arranque del recorido y no antes
            If chkEMR300.Checked Then boolResetMaxEMR = True

            Do While Not boolStopCamp And Conectado
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
                    nuevoMensajeEventos("Recorrido de medición iniciado")
                    btnConectar.Enabled = False
                    tmrCamp.Enabled = True
                    btnDetenerCamp.Enabled = True
                    btnIniciarCamp.Enabled = False
                    chkPausa.Enabled = True
                    CargarArchivoDePuntosToolStripMenuItem.Enabled = False
                End If

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
                        For i = 5 To 1
                            lblCamp.Text = "Reanudando en " & i & " segundos..."
                            Retardo(1000)
                            Application.DoEvents()
                        Next
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
                    Do Until tmrCamp.Enabled = False
                        Application.DoEvents()
                        If boolStopCamp Then GoTo Fin
                    Loop
                End If

                If Not PrimerBucle Then
                    Do
                        'DEBE SUPERAR LA DISTANCIA ESTABLECIDA EN EL CAMPO DE TEXTO
                        DistUltPunto = CalcularDist(UltimaLat, UltimaLng, LatitudActual, LongitudActual)
                        lblDistAct.Text = "Actual: " & Math.Round(DistUltPunto).ToString & " mts."

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
                    Loop
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
                    comNarda.DiscardInBuffer()
                End If
                boolResetMaxEMR = True
                Application.DoEvents()

                For Each threshold In thresholds
                    If NivelFinal >= threshold.Key Then
                        ColorMarker = threshold.Value.Item1
                        IndiceImg = threshold.Value.Item2
                        Exit For
                    End If
                Next

                If NivelFinal > 14 Then
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
                    nuevoMensajeEventos("El punto " & IndiceRes & " (" & NivelFinal & " V/m). requiere ser evaluado bajo Res. CNC 3690/04")
                End If

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
                ListaResultados.Items.Add(nReg)
                'Asigna la imagen correspondiente al item segun el nivel detectado
                ListaResultados.Items(IndiceRes - 1).ImageIndex = IndiceImg
                'Tilda el checkbox del item para que se muestre en mapa
                ListaResultados.Items(IndiceRes - 1).Checked = True
                nReg.EnsureVisible()
Fin:
                tmrCamp.Enabled = True
            Loop

            If boolStopCamp Then
                SW.Close()
                boolStopCamp = False
                lblCamp.BackColor = Color.Silver
                lblCamp.Text = "Recorrido finalizado, esperando nuevas instrucciones"
                lblCargado.Text = "Recorrido finalizado, guardado en " & RutaRaizRes
                nuevoMensajeEventos("Recorrido de medición finalizado con éxito por el usuario. Resultados guardados en """ & RutaRaizRes & """.")

                Dim SR As StreamReader = New StreamReader(RutaRaizRes)
                outCrypt = Criptografo.EncryptData(SR.ReadToEnd)
                SR.Close()
                SW = New StreamWriter(RutaRaizRes, False)
                SW.Write(outCrypt)
                SW.Close()

            Else
                MsgBox("No puede iniciarse un recorrido si no se encuentra conectado un instrumento de medición.", vbExclamation)
            End If

        Catch ex As Exception
            Try
                SW.Close()
                boolStopCamp = False
                lblCamp.BackColor = Color.Silver
                lblCamp.Text = "Recorrido finalizado, esperando nuevas instrucciones"
                lblCargado.Text = "Recorrido finalizado, guardado en " & RutaRaizRes
                nuevoMensajeEventos("Recorrido de medición finalizado con éxito por el usuario. Resultados guardados en """ & RutaRaizRes & """.")

                Dim SR As StreamReader = New StreamReader(RutaRaizRes)
                outCrypt = Criptografo.EncryptData(SR.ReadToEnd)
                SR.Close()
                SW = New StreamWriter(RutaRaizRes, False)
                SW.Write(outCrypt)
                SW.Close()
            Catch exc As Exception

            End Try
            If Not ex.Message.Contains("path") Then
                nuevoMensajeEventos("EXCEPCION OCURRIDA DURANTE EL RECORRIDO: " & ex.Message & vbNewLine & "INFO: " & ex.StackTrace)
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

            nuevoMensajeEventos("Proveedor de mapas cambiado a " & cboProvMapa.Text)
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
                nuevoMensajeEventos("Chequeando si existe conexión a internet...")
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
                nuevoMensajeEventos("Conexión a internet detectada. Activadas las peticiones al servidor seleccionado")
            End If
        Catch ex As Exception
            If ex.Message = "Host desconocido" Or ex.Message = "El nombre solicitado es válido pero no se encontraron datos del tipo solicitado" Then
                Mapa.Manager.Mode = AccessMode.CacheOnly
                nuevoMensajeEventos("No hay conexión a internet, estableciendo modo de conexión a ''Leer desde caché''")
                cboModoConexion.Text = "Leer desde caché"
            End If
        Finally
            Cursor = Cursors.Arrow
        End Try

    End Sub

    Private Sub IrAInicio(sender As System.Object, e As System.EventArgs)
        Cursor = Cursors.WaitCursor
        If LatitudActual.Grados = 0 Then
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


        End If
        Cursor = Cursors.Default
    End Sub

    Private Sub Mapa_LocationChanged(sender As Object, e As System.EventArgs) Handles Mapa.LocationChanged
        trkZoom.Value = Mapa.Zoom
        lblZoom.Text = Mapa.Zoom.ToString
    End Sub

    Private Sub CambioTrkZoom(sender As System.Object, e As System.EventArgs) Handles trkZoom.Scroll
        Mapa.Zoom = trkZoom.Value
        lblZoom.Text = Mapa.Zoom.ToString
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
                        Retardo(200)
                        Rta = .ReadExisting
                        If Rta <> "0;" & vbCr & "" And Not Rta.Contains("401;") Then
                            Beep()
                            nuevoMensajeEventos("El instrumento no responde, está apagado o no se encuentra conectado")
                            chkMaxAvg.Enabled = True
                            chkMaxHold.Enabled = True
                            btnIniciarCamp.Enabled = False
                            btnConectar.Text = "Conectar"
                            btnConectar.BackColor = SystemColors.Control
                            Exit Sub
                        Else
                            nuevoMensajeEventos("NBM-550 conectado. Activando el modo REMOTO.")
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
                            nuevoMensajeEventos("El instrumento no responde o no se encuentra conectado")
                            btnIniciarCamp.Enabled = False
                            Exit Sub
                        Else
                            nuevoMensajeEventos("EMR-300 conectado.")
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
                        nuevoMensajeEventos("Se desconectó correctamente el instrumento. Apagando el modo REMOTO.")
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
                        nuevoMensajeEventos("Se desconectó correctamente el instrumento.")
                    End If
                    actualizarBateria("OFF")
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
                    btnConectar.BackColor = SystemColors.Control
                    If .IsOpen Then .Close()
                    chkNBM550.Enabled = True
                    chkEMR300.Enabled = True
                End If
                InstrumentoRNIToolStripMenuItem.Enabled = True
            End With
        Catch ex As Exception
            If ex.Message.Contains("no existe") Then
                nuevoMensajeEventos("Error con instrumento - " & ex.Message)
            ElseIf ex.Message.Contains("Se ha denegado el acceso al puerto") Then
                nuevoMensajeEventos("Error con instrumento - " & ex.Message & " (El puerto está siendo utilizado por otro instrumento o dispositivo)")
            ElseIf ex.Message.Contains("no es correcto") Then
                nuevoMensajeEventos("Error con puerto seleccionado: " & ex.Message)
            Else
                nuevoMensajeEventos(String.Format("EXCEPCION OCURRIDA DURANTE LA CONEXIÓN: {0}" & vbNewLine & "Ubicación: {1}", ex.Message, ex.StackTrace))
            End If
            Beep()
            tmrNarda.Enabled = False
            PictureBox1.Image = Nothing
            btnIniciarCamp.Enabled = False
            lblDisplay.Text = ""
            Conectado = False
            btnConectar.Text = "Conectar"
            If comNarda.IsOpen Then comNarda.Close()
        Finally
            btnConectar.Enabled = True
        End Try
    End Sub

    Private Sub actualizarBateria(nivel As Object)
        If TypeOf nivel Is Integer Then
            ' ------ Bateria del NBM550
            barBateria.Invoke(Sub() barBateria.Value = CInt(nivel))
            lblBattery.Invoke(Sub() lblBattery.Text = nivel.ToString())
            If CInt(nivel) <= 10 Then
                My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
                nuevoMensajeEventos("La batería del instrumento por agotarse! Capacidad restante: " & CInt(nivel) & "%")
            End If
        ElseIf TypeOf nivel Is String Then
            ' ------ Procedimiento de desconexión de equipo
            If nivel.ToString.Contains("OFF") Then
                barBateria.Invoke(Sub() barBateria.Value = 0)
                lblBattery.Invoke(Sub() lblBattery.Text = "")
                Exit Sub
            End If

            ' ------ Bateria del EMR300
            If nivel.ToString.Contains("LOW") Then
                barBateria.Invoke(Sub() barBateria.Value = 10)
                lblBattery.Invoke(Sub() lblBattery.Text = "LOW")
                My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
                nuevoMensajeEventos("EL INSTRUMENTO REPORTA BATERÍA BAJA! Reemplazar o cargar baterías.")
            Else
                barBateria.Invoke(Sub() barBateria.Value = 100)
                lblBattery.Invoke(Sub() lblBattery.Text = "OK")
            End If
        End If


    End Sub

    Private Sub txtEventos_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtEventos.TextChanged
        txtEventos.Select(txtEventos.Text.Length, 0)
        txtEventos.ScrollToCaret()
    End Sub

    Private Sub cboPuertoNarda_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboPuertoNarda.SelectedIndexChanged
        If comNarda.IsOpen Then comNarda.Close()
        If cboPuertoNarda.Text <> "" Then
            comNarda.PortName = cboPuertoNarda.Text
        End If
    End Sub

    Private Sub cboIntervalo_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboIntervalo.SelectedIndexChanged
        tmrCamp.Interval = CInt(cboIntervalo.SelectedItem) * 1000 'segundos a milisegundos para setear en el timer de campaña
    End Sub

    Private Sub btnGEarth_Click(sender As System.Object, e As System.EventArgs) Handles btnGEarth.Click
        Dim Punto As Registro
        Dim i As Integer
        Dim Header1 As String
        Dim Header2 As String
        Dim Icon11 As String
        Dim PlaceMrk1 As String
        Dim PlaceMrk2 As String
        Dim PlaceMrk3 As String
        Dim PlaceMrk4 As String
        Dim PlaceMrk5 As String
        Dim ScreenOverlay As String
        Dim Footer As String
        Dim RutaKML As String
        Dim Latit() As String
        Dim Longit() As String
        Dim Nombre As String
        Dim InputText As String
        Dim OutputText As String
        Dim TipoResultado As String 'Depende de la extension del archivo cargado/creado en el ultimo recorrido
        Dim limitesNivel As Integer() = {1, 2, 4, 8, 15, 20, 35, 50, 100}
        Dim Intensidad(10) As String
        For n = 1 To 10
            Intensidad(n) = "#Nivel" & n
        Next

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

            Using SW As New StreamWriter(RutaKML)
                SW.AutoFlush = True
                ' Escribir encabezado(header) y codigo de colores de icono en el archivo
                OutputText = Header1 & Nombre & Header2 & Icon11
                SW.WriteLine(OutputText)
                Dim cont As Integer = 1
                Dim strInstrumento, strSonda As String
                For Each item As ListViewItem In ListaResultados.Items
                    With Punto
                        Dim aux() As String
                        .Fecha = item.SubItems.Item(4).Text
                        .Hora = item.SubItems.Item(3).Text.Substring(0, 8)
                        .Lat = item.SubItems.Item(5).Text
                        .Lon = item.SubItems.Item(6).Text
                        aux = Split(item.SubItems.Item(1).Text)
                        .Nivel = CSng(aux(0)) '/ 1000     'ES EL VALOR CON INCERTIDUMBRE INCLUIDA
                        .Unidad = "V/m"
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
                            Latit(i) = Latit(i).Substring(0, 2)
                        Else
                            Latit(i) = Latit(i).Substring(0, 1)
                        End If
                        If Len(Longit(i)) > 2 Then
                            Longit(i) = Longit(i).Substring(0, 2)
                        Else
                            Longit(i) = Longit(i).Substring(0, 1)
                        End If
                    Next i

                    Latit(2) = Latit(2).Substring(0, Len(Latit(2)) - 3)
                    Latit(2) = CSng(Latit(2))
                    Longit(2) = Longit(2).Substring(0, Len(Longit(2)) - 3)
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

                    ' Se establecen los colores según se definieron al inicio en 'limitesNivel' 
                    Dim j As Integer = 10 'Valor por defecto
                    For index = 0 To limitesNivel.Length - 1
                        If Punto.PorcentMEP <= limitesNivel(index) Then
                            j = index + 1
                            Exit For
                        End If
                    Next
                    Punto.Color = Intensidad(j)

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
                    End With
                    SW.WriteLine(OutputText)
                    cont += 1
                Next

                ' Escribir el segmento para añadir la imagen de la escala cromatica
                SW.WriteLine(ScreenOverlay)

                SW.WriteLine(Footer)
            End Using

            Using zip As ZipFile = New ZipFile
                zip.AddFile(RutaKML, "")
                zip.AddItem(Application.StartupPath & "/files", "files")
                zip.Save(RutaRaizKML & Nombre & ".kmz")
            End Using

            If File.Exists(RutaKML) Then
                File.Delete(RutaKML)
            End If

            nuevoMensajeEventos("Se ha creado el archivo de puntos en ''" & RutaKML & "''.")
        Catch ex As Exception
            MsgBox(ex.Message)
            nuevoMensajeEventos("Error al crear KMZ: " & ex.Message)
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
            objLibroExcel = m_Excel.Workbooks.Open(Application.StartupPath & "\rep\Modelo_reporte.xlsx")
            'Crear instancia de primera hoja de trabajo
            objHojaExcel = m_Excel.Worksheets(1)
            'Encabezado y pie de página del archivo:
            '------------------------------------------------------------------
            '   TODO ESTÁ PREFIJADO EN EL ARCHIVO .XLSX BASE INCLUIDO EN LA APP
            '------------------------------------------------------------------
            With objHojaExcel
                'Formato del título principal del formulario
                .Range("A7:L7").Value = "Tabla de mediciones - " & Application.ProductName & " - Versión " & Application.ProductVersion
                .Range("A7:L7").Font.Bold = True
                .Range("A7:L7").Font.Underline = True
                '--------------------------------------------
                Using SR As New StreamReader(RutaRaizRes)
                    inBuffer = SR.ReadLine
                    Dim MaxVal As Single = 0
                    Dim ItemsTot As Integer = ListaResultados.Items.Count
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

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' GENERACION DE PLANILLA CON VALORES EN POTENCIA ''''''''''''
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            M_Excel3 = New Excel.Application
            objLibro3 = M_Excel3.Workbooks.Open(Application.StartupPath & "\rep\Modelo_reporte_P.xlsx")
            objHoja3 = M_Excel3.Worksheets(1)
            With objHoja3
                'Formato del título principal del formulario
                .Range("A9:H9").Value = "Reporte de mediciones de Radiaciones No Ionizantes"
                .Range("A9:H9").Font.Bold = True
                .Range("A9:H9").Font.Underline = True
                '--------------------------------------------
                Using SR As New StreamReader(RutaRaizRes)
                    Dim MaxPot As Single = 0
                    inBuffer = SR.ReadLine
                    Dim ItemsTot As Integer = ListaResultados.Items.Count
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
                objLibro2 = m_Excel2.Workbooks.Open(Application.StartupPath & "\rep\Modelo_averif.xlsx")
                objHoja2 = m_Excel2.Worksheets(1)
                With objHoja2
                    'Formato del título principal del formulario
                    .Range("A7:L7").Value = "Tabla de mediciones a verificar - " & Application.ProductName & " - Versión " & Application.ProductVersion
                    .Range("A7:L7").Font.Bold = True
                    .Range("A7:L7").Font.Underline = True
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
            nuevoMensajeEventos("Reporte(s) generado(s) con éxito en la ruta seleccionada.")
        Catch ex As Exception
            nuevoMensajeEventos("Ha ocurrido una excepción: " & ex.Message & vbNewLine & vbNewLine & ex.StackTrace)
            MsgBox(ex.Message)
        Finally
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
            oDialog.Filter = "Mediciones RNI de valores máximos (*.mrni)|*.mrni"
            oDialog.FileName = ""
            If oDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
            RutaFichero = oDialog.FileName
            RutaRaizRes = RutaFichero
            Using SR As StreamReader = New StreamReader(RutaFichero)
                Dim Crypt As Encriptador = New Encriptador(encryptionKey)
                Dim InBuffer As String
                Dim PlainData As String

                ' Se lee todo el archivo cifrado
                InBuffer = SR.ReadToEnd

                ' Se chequea si el archivo está encriptado (no empieza con "#")
                If Not InBuffer.StartsWith("#") Then
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
                InBuffer = SR.ReadLine
                Dim EquipoNumSerie As String = InBuffer.Substring(33, Len(InBuffer) - 33)
                InBuffer = SR.ReadLine
                Dim SondaNom As String = InBuffer.Substring(18, Len(InBuffer) - 18)
                If SondaNom.Contains("EF") Then
                    UnidadActual = "V/m"
                Else
                    UnidadActual = "A/m"
                End If
                InBuffer = SR.ReadLine
                Dim SondaNumSerie As String = InBuffer.Substring(27, Len(InBuffer) - 27)
                InBuffer = SR.ReadLine
                Dim SondaFechaCal As String = InBuffer.Substring(39, Len(InBuffer) - 39)
                InBuffer = SR.ReadLine
                Dim SondaIncert As String = Replace(InBuffer.Substring(35, Len(InBuffer) - 35), ".", ",")
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

                    NuevoMarker.ToolTipMode = MarkerTooltipMode.OnMouseOver
                    OverlayCarga.Markers.Add(NuevoMarker)
                    ListaResultados.Sorting = SortOrder.None
                    'La columna principal es este dato (el item en si)
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
                    ListaResultados.Items.Add(nReg)
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
            nuevoMensajeEventos("Ha ocurrido una excepción: " & ex.Message)
        Finally
            boolCheck = True
            File.Delete(RutaBackup)
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

    Private Sub AcercaDeToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AcercaDeToolStripMenuItem.Click
        If About.Visible = True Then
            About.Close()
        End If
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

    Private Sub EstablecerAlarmaToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles EstablecerAlarmaToolStripMenuItem.Click
        Try
            If EstablecerAlarmaToolStripMenuItem.Checked = False Then
                NivelAlarma = CSng(InputBox("Ingrese el nivel en V/m a partir del cual hacer sonar la alarma (0 para desactivar):", "Alarma", "27,5"))
                If NivelAlarma > 0 Then
                    hayAlarma = True
                    EstablecerAlarmaToolStripMenuItem.Checked = True
                    'txtEventos.Text &= "[" & Now & "] " & "Alarma de nivel activada en " & NivelAlarma & " V/m." & vbNewLine
                    nuevoMensajeEventos("Alarma de nivel activada en " & NivelAlarma & " V/m.")
                Else
                    hayAlarma = False
                    EstablecerAlarmaToolStripMenuItem.Checked = False
                    'txtEventos.Text &= "[" & Now & "] " & "Alarma de nivel desactivada." & vbNewLine
                    nuevoMensajeEventos("Alarma de nivel desactivada.")
                End If
            Else
                'txtEventos.Text &= "[" & Now & "] " & "Alarma de nivel desactivada." & vbNewLine
                nuevoMensajeEventos("Alarma de nivel desactivada.")
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
            comGPS.Close()
        End If
        tmrGPS.Interval = 100
    End Sub

    Private Sub cboCOMGlobalSat_Click(sender As System.Object, e As System.EventArgs) Handles cboCOMGlobalSat.Click
        SetNMEASettings()
    End Sub

    Private Sub cboCOMGlobalSat_DropDownClosed(sender As Object, e As System.EventArgs) Handles cboCOMGlobalSat.DropDownClosed
        SetNMEASettings()
    End Sub

    Private Sub cboCOMGlobalSat_MouseDown(sender As Object, e As System.EventArgs) Handles cboCOMGlobalSat.MouseDown
        SetNMEASettings()
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

    Sub SetNMEASettings()
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
            'txtEventos.Text &= "[" & Now & "] " & "Ha ocurrido una excepción: " & ex.Message & vbNewLine
            nuevoMensajeEventos("Ha ocurrido una excepción: " & ex.Message)
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
            nuevoMensajeEventos(String.Format("Error en ProcesoLeerNarda: {0}" & vbNewLine & "Stacktrace: {1}", ex.Message, ex.StackTrace))
        End Try

    End Sub

    Private Sub tmrGPS_Tick(sender As System.Object, e As System.EventArgs) Handles tmrGPS.Tick
        'Setear el intervalo en 100 milisegundos para una respuesta rápida de localización
        'y actualización de posición en tiempo real (con el GARMIN 18X USB)
        'El GPS GlobalSat debería tener dwells de por lo menos 1000 ms
        Select Case ProcesoGPS.ThreadState
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
                        nuevoMensajeEventos("<GPS POSICIONADO EN MODO DEBUG>")
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
                            nuevoMensajeEventos("GPS desconectado.")
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
                                nuevoMensajeEventos("GPS sin posición.")
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
                                nuevoMensajeEventos("GPS posicionado.")
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
                            nuevoMensajeEventos("GPS desconectado.")
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
                            nuevoMensajeEventos("GPS sin posición.")
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
                            nuevoMensajeEventos("GPS posicionado.")
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

                latdec = ConvertirAGDec(LatitudActual)
                lngdec = ConvertirAGDec(LongitudActual)
                CoorAUbicar.Lat = latdec
                CoorAUbicar.Lng = lngdec

                OverlayPosActual.Markers.Clear()
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
            If ex.Message.Contains("no existe") Then
                If Not lblStatusGPS.Text.Contains("Desconectado") Then
                    nuevoMensajeEventos("No se puede detectar un GPS conectado (" & ex.Message & ")")
                    lblStatusGPS.Text = "Desconectado"
                    lblStatusGPS.BackColor = Color.LightGray
                End If
            ElseIf ex.Message.Contains("Datos no válidos") Then
                Application.DoEvents()
                Exit Sub
            ElseIf ex.Message.Contains("reintentos") Then
                comGPS.Close()
            End If
            Application.DoEvents()
        Finally

        End Try

    End Sub

    Sub LeerNarda()

        Dim VecNarda As String()
        Dim ContError As Integer = 0
        Dim inBuffer As String
        Dim nivelBateria As Integer = 0

        Try
            While Conectado
                If Conectado AndAlso comNarda IsNot Nothing Then
                    If chkNBM550.Checked Then
                        If chkPausa.Checked Then
                            ' SI ESTA EN PAUSA, PEDIR EL VALOR RESETEA EL MAX HOLD
                            comNarda.WriteLine("RESET_MAX;")
                            Retardo(200)
                            inBuffer = comNarda.ReadExisting
                            Exit Sub
                        End If

                        comNarda.WriteLine("MEAS?;")
                        Retardo(200)
                        VecNarda = Split(comNarda.ReadExisting, ",")
                        Application.DoEvents()

                        If chkActual.Checked Then
                            NivelNarda = Format(CSng(VecNarda(1) / 1000), "##0.000")  'LINEA PARA LEER ACTUAL
                            lblTipoRes.Invoke(Sub() lblTipoRes.Text = "Actual")
                        ElseIf chkMaxHold.Checked Then
                            NivelNarda = Format(CSng(VecNarda(0) / 1000), "##0.000")   'LINEA PARA LEER MAXHOLD
                            lblTipoRes.Invoke(Sub() lblTipoRes.Text = "Max hold")
                        End If

                        lblDisplay.Invoke(Sub() lblDisplay.Text = NivelNarda & " " & UnidadActual)

                        comNarda.WriteLine("BATTERY?;")
                        Retardo(200)
                        inBuffer = comNarda.ReadExisting
                        nivelBateria = CInt(inBuffer.Substring(0, Len(inBuffer) - 2))
                        actualizarBateria(nivelBateria)
                        ContError = 0

                    ElseIf chkEMR300.Checked Then
                        comNarda.WriteLine("M")
                        Retardo(400)
                        VecNarda = Split(comNarda.ReadExisting, ",")

                        comNarda.DiscardInBuffer()
                        NivelNarda = Format(CSng(VecNarda(0).Replace(".", ",")), "##0.000")

                        If chkMaxHold.Checked Then
                            lblDisplay.Invoke(Sub() lblDisplay.Text = NivMax & " " & UnidadActual) 'NivelNarda & " V/m"
                            lblTipoRes.Invoke(Sub() lblDisplay.Text = "Max hold")
                        ElseIf chkMaxAvg.Checked Then
                            lblDisplay.Invoke(Sub() lblDisplay.Text = NivelNarda & " " & UnidadActual)
                            lblTipoRes.Invoke(Sub() lblDisplay.Text = "Max Avg")
                        End If

                        comNarda.WriteLine("SYST:BAT?")
                        Retardo(100)
                        inBuffer = comNarda.ReadExisting
                        actualizarBateria(inBuffer.ToString)
                    End If

                    ContError = 0

                End If
            End While
        Catch ex As Exception
            If ex.Message.Contains("418;") OrElse ex.Message.Contains("Uno de los dispositivos conectados al sistema no funciona") Then
                MostrarMensajeError(ex.Message)
            End If
            If ex.Message.Contains("El puerto está cerrado") And Conectado Then
                MostrarMensajeError("Posible desconexión del cable.")
            End If
        End Try

    End Sub

    Private Sub MostrarMensajeError(mensaje As String)
        tmrNarda.Enabled = False
        nuevoMensajeEventos("Error con instrumento: " & mensaje)
        lblDisplay.Invoke(Sub() lblDisplay.Text = "---")
        NivelNarda = -1
        lblTipoRes.Invoke(Sub() lblTipoRes.Text = "")
        Conectado = False
        btnConectar.Invoke(Sub() btnConectar.Text = "Conectar")
        If mensaje.Contains("No hay sonda conectada al mismo") Then
            comNarda.WriteLine("REMOTE OFF;")
            comNarda.DiscardInBuffer()
            LimpiarConexion()
            MsgBox(mensaje, vbExclamation)
        ElseIf mensaje.Contains("desconexión") Then
            actualizarBateria("OFF")
            LimpiarConexion()
        End If
    End Sub

    Sub LimpiarConexion()
        lblSonda.Invoke(Sub() lblSonda.Text = "")
        lblInstrumento.Invoke(Sub() lblInstrumento.Text = "")
        lblIncert.Invoke(Sub() lblIncert.Text = "")
        PictureBox1.Invoke(Sub() PictureBox1.Image = Nothing)
        btnIniciarCamp.Invoke(Sub() btnIniciarCamp.Enabled = False)
    End Sub

    ''' <summary>
    ''' Actualiza el nivel máximo detectado por el EMR300 en un variable que la rutina de recorrido va a tomar como resultado final en un punto
    ''' </summary>
    ''' <remarks>Usar "boolResetMaxEMR" en la rutina de recorrido para finalizar la lectura actual y resetear el nivel a 0 V/m. Usar "boolKillThrMaxEMR" para terminar el thread</remarks>
    Sub t_EMRMAX()
        Do
            NivMax = 0
            boolResetMaxEMR = False
            Do While Not boolResetMaxEMR
                If NivelNarda > NivMax Then NivMax = NivelNarda
                Application.DoEvents()
            Loop
        Loop
    End Sub

#End Region


    Private Sub linkScanNardaPorts_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles linkScanNardaPorts.LinkClicked
        Dim ports As String() = SerialPort.GetPortNames()
        Dim nardaDetected As Boolean = False
        ' Buscamos el puerto que tiene conectado el NBM utilizando sus parámetros de conexión
        For Each port In ports
            Try
                Using serialPort As New SerialPort(port)
                    ' Config COM para el puerto USB
                    serialPort.BaudRate = 460800
                    serialPort.DataBits = 8
                    serialPort.Parity = Parity.None
                    serialPort.StopBits = StopBits.One
                    serialPort.Handshake = Handshake.None

                    If Not serialPort.IsOpen() Then serialPort.Open()

                    serialPort.DiscardInBuffer()
                    serialPort.WriteLine("REMOTE ON;")
                    Retardo(100)
                    Dim data As String = serialPort.ReadExisting()

                    If data.Equals("0;" & vbCr & "") Then
                        ' NBM-550 encontrado
                        comNarda.PortName = port
                        cboPuertoNarda.Text = port.ToString()
                        nuevoMensajeEventos("Se encontró NARDA NBM-550 en el puerto " & port)
                        nuevoMensajeEventos("Puerto " & port & " asignado al medidor de RNI, presione el botón CONECTAR para iniciar el enlace.")
                        btnConectar.BackColor = Color.YellowGreen
                        nardaDetected = True
                        Exit For
                    End If

                End Using
            Catch ex As Exception
                nuevoMensajeEventos("Error relevando puerto " & port & ": " & ex.Message)
            End Try
        Next
        If Not nardaDetected Then nuevoMensajeEventos("No se encontraron dispositivos conectados. Revise que esten conectados y encendidos para intentar nuevamente.")
    End Sub

    Private Sub linkScanGPSPorts_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles linkScanGPSPorts.LinkClicked
        Dim ports As String() = SerialPort.GetPortNames()
        Dim gpsDetected As Boolean = False
        Dim data As String

        If GPSSel <> 2 Then Exit Sub

        nuevoMensajeEventos("Buscando GPS en puertos COM...")

        For Each port In ports
            Try
                Using serialPort As New SerialPort(port)
                    serialPort.BaudRate = 4800
                    serialPort.DataBits = 8
                    serialPort.Parity = Parity.None
                    serialPort.StopBits = StopBits.One
                    serialPort.Handshake = Handshake.None

                    Try
                        serialPort.Open()
                    Catch ex As UnauthorizedAccessException
                        nuevoMensajeEventos("El puerto " & port & " está ocupado, continuando detección...")
                        Continue For
                    End Try

                    Retardo(300)

                    If Not gpsDetected Then
                        Try
                            data = serialPort.ReadExisting()
                            If data.Contains("$GP") Then
                                ' NMEA encontrado
                                comGPS.PortName = port
                                nuevoMensajeEventos("Se encontró GPS NMEA en el puerto " & port)
                                nuevoMensajeEventos("Puerto " & port & " asignado al GPS NMEA. Recibiendo información de posición.")
                                gpsDetected = True
                                cboCOMGlobalSat.Text = port
                                cboCOMGlobalSat.PerformClick() ' Acá se establecen timeouts y otros parámetros

                                comGPS.PortName = cboCOMGlobalSat.Text
                                Exit For
                            End If
                        Catch ex As TimeoutException
                            ' Hay equipo conectado pero no responde, puede ser el medidor RNI
                            nuevoMensajeEventos("[TimeoutException] No se recibieron datos del dispositivo en el puerto " & port & ".")
                        End Try
                    End If

                End Using
            Catch ex As Exception
                nuevoMensajeEventos("Error relevando puerto " & port & ": " & ex.Message)
            End Try
        Next

    End Sub

    Private Sub comboGPSSelected_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles comboGPSSelected.SelectedIndexChanged
        If comboGPSSelected.Text.Contains("Garmin") Then
            linkScanGPSPorts.Enabled = False
            Label6.Text = "Estado del GPS [18X USB]"
            GPSSel = 1
        End If

        If comboGPSSelected.Text.Contains("NMEA") Then
            linkScanGPSPorts.Enabled = True
            Label6.Text = "Estado del GPS [NMEA]"
            GPSSel = 2
        End If
    End Sub

End Class