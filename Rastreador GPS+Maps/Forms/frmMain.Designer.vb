<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.Mapa = New GMap.NET.WindowsForms.GMapControl()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblCargado = New System.Windows.Forms.Label()
        Me.trkZoom = New System.Windows.Forms.TrackBar()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lblZoom = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboProvMapa = New System.Windows.Forms.ComboBox()
        Me.cboModoConexion = New System.Windows.Forms.ComboBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.lblBattery = New System.Windows.Forms.Label()
        Me.linkScanNardaPorts = New System.Windows.Forms.LinkLabel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.groupBoxRecorrido = New System.Windows.Forms.GroupBox()
        Me.opK83 = New System.Windows.Forms.RadioButton()
        Me.lblCamp = New System.Windows.Forms.Label()
        Me.lblDistAct = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtDistancia = New System.Windows.Forms.TextBox()
        Me.btnIniciarCamp = New System.Windows.Forms.Button()
        Me.opDistancia = New System.Windows.Forms.RadioButton()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.btnDetenerCamp = New System.Windows.Forms.Button()
        Me.chkPausa = New System.Windows.Forms.CheckBox()
        Me.opIntervalo = New System.Windows.Forms.RadioButton()
        Me.cboIntervalo = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.barBateria = New System.Windows.Forms.ProgressBar()
        Me.lblTipoRes = New System.Windows.Forms.Label()
        Me.lblIncert = New System.Windows.Forms.Label()
        Me.lblDisplay = New System.Windows.Forms.Label()
        Me.lblSonda = New System.Windows.Forms.Label()
        Me.lblSondaDetect = New System.Windows.Forms.Label()
        Me.lblInstrumento = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnConectar = New System.Windows.Forms.Button()
        Me.tTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.comGPS = New System.IO.Ports.SerialPort(Me.components)
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.btnGEarth = New System.Windows.Forms.Button()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.ListaResultados = New System.Windows.Forms.ListView()
        Me.indice = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.valor = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.valPuro = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.hora = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.fecha = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lat = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lng = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.instrum = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.sond = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.fechacalsond = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.incert = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.vecesDB = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ListaImgs = New System.Windows.Forms.ImageList(Me.components)
        Me.txtEventos = New System.Windows.Forms.TextBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ArchivoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CargarArchivoDePuntosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportarDatosDeMapaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportarDatosDeMapaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PrefetchAreaSeleccionadaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpcionesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InstrumentoRNIToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.chkEMR300 = New System.Windows.Forms.ToolStripMenuItem()
        Me.chkNBM550 = New System.Windows.Forms.ToolStripMenuItem()
        Me.USBToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ÓpticoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.cboPuertoNarda = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Garmin18XUSBToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DispositivoNMEAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cboCOMGlobalSat = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.EstablecerAlarmaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TipoDeResultadoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.chkMaxHold = New System.Windows.Forms.ToolStripMenuItem()
        Me.chkMaxAvg = New System.Windows.Forms.ToolStripMenuItem()
        Me.chkActual = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModoDebug = New System.Windows.Forms.ToolStripMenuItem()
        Me.AcercaDeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmrGPS = New System.Windows.Forms.Timer(Me.components)
        Me.tmrNarda = New System.Windows.Forms.Timer(Me.components)
        Me.tmrRetardo = New System.Windows.Forms.Timer(Me.components)
        Me.tmrCamp = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.comboGPSSelected = New System.Windows.Forms.ComboBox()
        Me.linkScanGPSPorts = New System.Windows.Forms.LinkLabel()
        Me.txtLngActual = New System.Windows.Forms.TextBox()
        Me.txtLatActual = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.chkAutoLoc = New System.Windows.Forms.CheckBox()
        Me.lblStatusGPS = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.sDialog = New System.Windows.Forms.SaveFileDialog()
        Me.oDialog = New System.Windows.Forms.OpenFileDialog()
        Me.comNarda = New System.IO.Ports.SerialPort(Me.components)
        Me.Label21 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox1.SuspendLayout()
        CType(Me.trkZoom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.groupBoxRecorrido.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox5.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Mapa
        '
        Me.Mapa.Bearing = 0.0!
        Me.Mapa.CanDragMap = True
        Me.Mapa.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Mapa.EmptyTileColor = System.Drawing.Color.LightGray
        Me.Mapa.GrayScaleMode = False
        Me.Mapa.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow
        Me.Mapa.LevelsKeepInMemmory = 5
        Me.Mapa.Location = New System.Drawing.Point(3, 16)
        Me.Mapa.MarkersEnabled = True
        Me.Mapa.MaxZoom = 22
        Me.Mapa.MinZoom = 0
        Me.Mapa.MouseWheelZoomEnabled = True
        Me.Mapa.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter
        Me.Mapa.Name = "Mapa"
        Me.Mapa.NegativeMode = False
        Me.Mapa.PolygonsEnabled = True
        Me.Mapa.RetryLoadTile = 0
        Me.Mapa.RoutesEnabled = True
        Me.Mapa.ScaleMode = GMap.NET.WindowsForms.ScaleModes.[Integer]
        Me.Mapa.SelectedAreaFillColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.Mapa.ShowTileGridLines = False
        Me.Mapa.Size = New System.Drawing.Size(802, 436)
        Me.Mapa.TabIndex = 0
        Me.Mapa.Zoom = 10.0R
        '
        'GroupBox1
        '
        Me.GroupBox1.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.GroupBox1, 3)
        Me.GroupBox1.Controls.Add(Me.lblCargado)
        Me.GroupBox1.Controls.Add(Me.Mapa)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(3, 289)
        Me.GroupBox1.Name = "GroupBox1"
        Me.TableLayoutPanel1.SetRowSpan(Me.GroupBox1, 2)
        Me.GroupBox1.Size = New System.Drawing.Size(808, 455)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Mapa (Shift + arrastre con Click derecho para seleccionar área)"
        '
        'lblCargado
        '
        Me.lblCargado.AutoSize = True
        Me.lblCargado.BackColor = System.Drawing.Color.Transparent
        Me.lblCargado.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.lblCargado.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCargado.ForeColor = System.Drawing.Color.Blue
        Me.lblCargado.Location = New System.Drawing.Point(6, 20)
        Me.lblCargado.Name = "lblCargado"
        Me.lblCargado.Size = New System.Drawing.Size(0, 13)
        Me.lblCargado.TabIndex = 1
        Me.lblCargado.Visible = False
        '
        'trkZoom
        '
        Me.trkZoom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkZoom.LargeChange = 3
        Me.trkZoom.Location = New System.Drawing.Point(57, 106)
        Me.trkZoom.Maximum = 17
        Me.trkZoom.Name = "trkZoom"
        Me.trkZoom.Size = New System.Drawing.Size(206, 45)
        Me.trkZoom.TabIndex = 1
        Me.trkZoom.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.trkZoom.Value = 15
        '
        'LinkLabel2
        '
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.Location = New System.Drawing.Point(640, 14)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(113, 13)
        Me.LinkLabel2.TabIndex = 10
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "Mostrar ruta del caché"
        Me.LinkLabel2.Visible = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lblZoom)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Controls.Add(Me.trkZoom)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.cboProvMapa)
        Me.GroupBox3.Controls.Add(Me.cboModoConexion)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox3.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(299, 280)
        Me.GroupBox3.TabIndex = 3
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Opciones del mapa"
        '
        'lblZoom
        '
        Me.lblZoom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblZoom.AutoSize = True
        Me.lblZoom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblZoom.Location = New System.Drawing.Point(30, 118)
        Me.lblZoom.Name = "lblZoom"
        Me.lblZoom.Size = New System.Drawing.Size(21, 15)
        Me.lblZoom.TabIndex = 5
        Me.lblZoom.Text = "15"
        '
        'Label7
        '
        Me.Label7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(104, 90)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(74, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Nivel de zoom"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Servidor de mapa:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Modo de conexión:"
        '
        'cboProvMapa
        '
        Me.cboProvMapa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProvMapa.FormattingEnabled = True
        Me.cboProvMapa.Items.AddRange(New Object() {"Google Maps", "Google Hybrid", "Open Street Map"})
        Me.cboProvMapa.Location = New System.Drawing.Point(107, 41)
        Me.cboProvMapa.Name = "cboProvMapa"
        Me.cboProvMapa.Size = New System.Drawing.Size(132, 21)
        Me.cboProvMapa.TabIndex = 1
        '
        'cboModoConexion
        '
        Me.cboModoConexion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboModoConexion.FormattingEnabled = True
        Me.cboModoConexion.Items.AddRange(New Object() {"Servidor y caché", "Leer desde caché", "Peticiones al servidor"})
        Me.cboModoConexion.Location = New System.Drawing.Point(107, 16)
        Me.cboModoConexion.Name = "cboModoConexion"
        Me.cboModoConexion.Size = New System.Drawing.Size(132, 21)
        Me.cboModoConexion.TabIndex = 0
        '
        'GroupBox4
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.GroupBox4, 2)
        Me.GroupBox4.Controls.Add(Me.lblBattery)
        Me.GroupBox4.Controls.Add(Me.linkScanNardaPorts)
        Me.GroupBox4.Controls.Add(Me.Label4)
        Me.GroupBox4.Controls.Add(Me.Label3)
        Me.GroupBox4.Controls.Add(Me.groupBoxRecorrido)
        Me.GroupBox4.Controls.Add(Me.barBateria)
        Me.GroupBox4.Controls.Add(Me.lblTipoRes)
        Me.GroupBox4.Controls.Add(Me.lblIncert)
        Me.GroupBox4.Controls.Add(Me.lblDisplay)
        Me.GroupBox4.Controls.Add(Me.lblSonda)
        Me.GroupBox4.Controls.Add(Me.lblSondaDetect)
        Me.GroupBox4.Controls.Add(Me.lblInstrumento)
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Controls.Add(Me.Panel1)
        Me.GroupBox4.Controls.Add(Me.btnConectar)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox4.Location = New System.Drawing.Point(591, 3)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(703, 280)
        Me.GroupBox4.TabIndex = 4
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Instrumento de medición RNI"
        '
        'lblBattery
        '
        Me.lblBattery.BackColor = System.Drawing.Color.Transparent
        Me.lblBattery.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblBattery.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBattery.Location = New System.Drawing.Point(84, 241)
        Me.lblBattery.Name = "lblBattery"
        Me.lblBattery.Size = New System.Drawing.Size(35, 23)
        Me.lblBattery.TabIndex = 29
        Me.lblBattery.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'linkScanNardaPorts
        '
        Me.linkScanNardaPorts.AutoSize = True
        Me.linkScanNardaPorts.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.linkScanNardaPorts.Location = New System.Drawing.Point(175, 198)
        Me.linkScanNardaPorts.Name = "linkScanNardaPorts"
        Me.linkScanNardaPorts.Size = New System.Drawing.Size(198, 17)
        Me.linkScanNardaPorts.TabIndex = 28
        Me.linkScanNardaPorts.TabStop = True
        Me.linkScanNardaPorts.Text = "Buscar puerto del instrumento"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(127, 168)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(74, 13)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "Incertidumbre:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(127, 142)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(92, 13)
        Me.Label3.TabIndex = 22
        Me.Label3.Text = "Tipo de resultado:"
        '
        'groupBoxRecorrido
        '
        Me.groupBoxRecorrido.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.groupBoxRecorrido.Controls.Add(Me.opK83)
        Me.groupBoxRecorrido.Controls.Add(Me.lblCamp)
        Me.groupBoxRecorrido.Controls.Add(Me.lblDistAct)
        Me.groupBoxRecorrido.Controls.Add(Me.Label12)
        Me.groupBoxRecorrido.Controls.Add(Me.txtDistancia)
        Me.groupBoxRecorrido.Controls.Add(Me.btnIniciarCamp)
        Me.groupBoxRecorrido.Controls.Add(Me.opDistancia)
        Me.groupBoxRecorrido.Controls.Add(Me.Label13)
        Me.groupBoxRecorrido.Controls.Add(Me.btnDetenerCamp)
        Me.groupBoxRecorrido.Controls.Add(Me.chkPausa)
        Me.groupBoxRecorrido.Controls.Add(Me.opIntervalo)
        Me.groupBoxRecorrido.Controls.Add(Me.cboIntervalo)
        Me.groupBoxRecorrido.Controls.Add(Me.Label14)
        Me.groupBoxRecorrido.Location = New System.Drawing.Point(408, 0)
        Me.groupBoxRecorrido.Name = "groupBoxRecorrido"
        Me.groupBoxRecorrido.Size = New System.Drawing.Size(295, 280)
        Me.groupBoxRecorrido.TabIndex = 21
        Me.groupBoxRecorrido.TabStop = False
        Me.groupBoxRecorrido.Text = "Controles de la medición"
        '
        'opK83
        '
        Me.opK83.AutoSize = True
        Me.opK83.Enabled = False
        Me.opK83.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.opK83.Location = New System.Drawing.Point(6, 70)
        Me.opK83.Name = "opK83"
        Me.opK83.Size = New System.Drawing.Size(249, 19)
        Me.opK83.TabIndex = 24
        Me.opK83.TabStop = True
        Me.opK83.Text = "(disabled) Medición puntual (UIT-R K.83)"
        Me.opK83.UseVisualStyleBackColor = True
        '
        'lblCamp
        '
        Me.lblCamp.BackColor = System.Drawing.Color.Silver
        Me.lblCamp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCamp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCamp.Location = New System.Drawing.Point(6, 123)
        Me.lblCamp.Name = "lblCamp"
        Me.lblCamp.Size = New System.Drawing.Size(280, 28)
        Me.lblCamp.TabIndex = 14
        Me.lblCamp.Text = "INACTIVO"
        Me.lblCamp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDistAct
        '
        Me.lblDistAct.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDistAct.Location = New System.Drawing.Point(197, 44)
        Me.lblDistAct.Name = "lblDistAct"
        Me.lblDistAct.Size = New System.Drawing.Size(89, 18)
        Me.lblDistAct.TabIndex = 23
        Me.lblDistAct.Text = "Actual:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(60, 106)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(128, 13)
        Me.Label12.TabIndex = 13
        Me.Label12.Text = "Estado de recorido actual"
        '
        'txtDistancia
        '
        Me.txtDistancia.Location = New System.Drawing.Point(129, 44)
        Me.txtDistancia.MaxLength = 4
        Me.txtDistancia.Name = "txtDistancia"
        Me.txtDistancia.Size = New System.Drawing.Size(36, 20)
        Me.txtDistancia.TabIndex = 22
        Me.txtDistancia.Text = "20"
        '
        'btnIniciarCamp
        '
        Me.btnIniciarCamp.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnIniciarCamp.Location = New System.Drawing.Point(48, 154)
        Me.btnIniciarCamp.Name = "btnIniciarCamp"
        Me.btnIniciarCamp.Size = New System.Drawing.Size(205, 35)
        Me.btnIniciarCamp.TabIndex = 2
        Me.btnIniciarCamp.Text = "Iniciar recorrido"
        Me.btnIniciarCamp.UseVisualStyleBackColor = False
        '
        'opDistancia
        '
        Me.opDistancia.AutoSize = True
        Me.opDistancia.Checked = True
        Me.opDistancia.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.opDistancia.Location = New System.Drawing.Point(6, 44)
        Me.opDistancia.Name = "opDistancia"
        Me.opDistancia.Size = New System.Drawing.Size(117, 19)
        Me.opDistancia.TabIndex = 21
        Me.opDistancia.TabStop = True
        Me.opDistancia.Text = "Dist entre puntos"
        Me.opDistancia.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(164, 46)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(27, 15)
        Me.Label13.TabIndex = 20
        Me.Label13.Text = "mts"
        '
        'btnDetenerCamp
        '
        Me.btnDetenerCamp.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDetenerCamp.Enabled = False
        Me.btnDetenerCamp.Location = New System.Drawing.Point(48, 233)
        Me.btnDetenerCamp.Name = "btnDetenerCamp"
        Me.btnDetenerCamp.Size = New System.Drawing.Size(205, 34)
        Me.btnDetenerCamp.TabIndex = 4
        Me.btnDetenerCamp.Text = "Finalizar recorrido"
        Me.btnDetenerCamp.UseVisualStyleBackColor = False
        '
        'chkPausa
        '
        Me.chkPausa.Enabled = False
        Me.chkPausa.Location = New System.Drawing.Point(70, 195)
        Me.chkPausa.Name = "chkPausa"
        Me.chkPausa.Size = New System.Drawing.Size(165, 30)
        Me.chkPausa.TabIndex = 19
        Me.chkPausa.Text = "Pausar recorrido (Ctrl + P)"
        Me.chkPausa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.tTip.SetToolTip(Me.chkPausa, "Presione esta casilla para pausar la captura" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "de puntos sin finalizar el recorrid" & _
        "o actual")
        Me.chkPausa.UseVisualStyleBackColor = True
        '
        'opIntervalo
        '
        Me.opIntervalo.AutoSize = True
        Me.opIntervalo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.opIntervalo.Location = New System.Drawing.Point(6, 20)
        Me.opIntervalo.Name = "opIntervalo"
        Me.opIntervalo.Size = New System.Drawing.Size(156, 19)
        Me.opIntervalo.TabIndex = 18
        Me.opIntervalo.TabStop = True
        Me.opIntervalo.Text = "Intervalo entre muestras"
        Me.opIntervalo.UseVisualStyleBackColor = True
        '
        'cboIntervalo
        '
        Me.cboIntervalo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboIntervalo.FormattingEnabled = True
        Me.cboIntervalo.Items.AddRange(New Object() {"3", "4", "5", "6", "7", "8", "9", "10", "15", "20", "25", "30"})
        Me.cboIntervalo.Location = New System.Drawing.Point(176, 16)
        Me.cboIntervalo.Name = "cboIntervalo"
        Me.cboIntervalo.Size = New System.Drawing.Size(51, 21)
        Me.cboIntervalo.TabIndex = 16
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(233, 22)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(53, 13)
        Me.Label14.TabIndex = 17
        Me.Label14.Text = "segundos"
        '
        'barBateria
        '
        Me.barBateria.Location = New System.Drawing.Point(6, 241)
        Me.barBateria.Name = "barBateria"
        Me.barBateria.Size = New System.Drawing.Size(72, 23)
        Me.barBateria.Step = 1
        Me.barBateria.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.barBateria.TabIndex = 20
        '
        'lblTipoRes
        '
        Me.lblTipoRes.BackColor = System.Drawing.SystemColors.Control
        Me.lblTipoRes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTipoRes.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblTipoRes.Location = New System.Drawing.Point(226, 137)
        Me.lblTipoRes.Name = "lblTipoRes"
        Me.lblTipoRes.Size = New System.Drawing.Size(78, 21)
        Me.lblTipoRes.TabIndex = 12
        Me.lblTipoRes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblIncert
        '
        Me.lblIncert.BackColor = System.Drawing.SystemColors.Control
        Me.lblIncert.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblIncert.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblIncert.Location = New System.Drawing.Point(226, 162)
        Me.lblIncert.Name = "lblIncert"
        Me.lblIncert.Size = New System.Drawing.Size(78, 21)
        Me.lblIncert.TabIndex = 13
        Me.lblIncert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDisplay
        '
        Me.lblDisplay.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDisplay.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDisplay.Location = New System.Drawing.Point(129, 90)
        Me.lblDisplay.Name = "lblDisplay"
        Me.lblDisplay.Size = New System.Drawing.Size(264, 39)
        Me.lblDisplay.TabIndex = 11
        Me.lblDisplay.Text = "---"
        Me.lblDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.tTip.SetToolTip(Me.lblDisplay, "Valor medido actualmente mostrado en la pantalla del instrumento")
        '
        'lblSonda
        '
        Me.lblSonda.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSonda.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSonda.Location = New System.Drawing.Point(130, 67)
        Me.lblSonda.Name = "lblSonda"
        Me.lblSonda.Size = New System.Drawing.Size(171, 18)
        Me.lblSonda.TabIndex = 10
        Me.lblSonda.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblSondaDetect
        '
        Me.lblSondaDetect.AutoSize = True
        Me.lblSondaDetect.Location = New System.Drawing.Point(128, 54)
        Me.lblSondaDetect.Name = "lblSondaDetect"
        Me.lblSondaDetect.Size = New System.Drawing.Size(92, 13)
        Me.lblSondaDetect.TabIndex = 9
        Me.lblSondaDetect.Text = "Sonda detectada:"
        '
        'lblInstrumento
        '
        Me.lblInstrumento.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInstrumento.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstrumento.Location = New System.Drawing.Point(129, 39)
        Me.lblInstrumento.Name = "lblInstrumento"
        Me.lblInstrumento.Size = New System.Drawing.Size(171, 15)
        Me.lblInstrumento.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(126, 22)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(116, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Instrumento detectado:"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Location = New System.Drawing.Point(6, 27)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(115, 202)
        Me.Panel1.TabIndex = 5
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(113, 200)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'btnConectar
        '
        Me.btnConectar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnConectar.Location = New System.Drawing.Point(171, 225)
        Me.btnConectar.Name = "btnConectar"
        Me.btnConectar.Size = New System.Drawing.Size(205, 39)
        Me.btnConectar.TabIndex = 3
        Me.btnConectar.Text = "Conectar instrumento"
        Me.btnConectar.UseVisualStyleBackColor = True
        '
        'tTip
        '
        Me.tTip.AutoPopDelay = 10000
        Me.tTip.InitialDelay = 500
        Me.tTip.ReshowDelay = 100
        '
        'comGPS
        '
        Me.comGPS.BaudRate = 4800
        Me.comGPS.PortName = "COM2"
        Me.comGPS.WriteBufferSize = 1
        '
        'GroupBox5
        '
        Me.GroupBox5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox5.Controls.Add(Me.btnGEarth)
        Me.GroupBox5.Controls.Add(Me.btnExcel)
        Me.GroupBox5.Controls.Add(Me.ListaResultados)
        Me.GroupBox5.Location = New System.Drawing.Point(817, 289)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(477, 314)
        Me.GroupBox5.TabIndex = 5
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Resultados"
        '
        'btnGEarth
        '
        Me.btnGEarth.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGEarth.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGEarth.Image = CType(resources.GetObject("btnGEarth.Image"), System.Drawing.Image)
        Me.btnGEarth.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGEarth.Location = New System.Drawing.Point(245, 236)
        Me.btnGEarth.Name = "btnGEarth"
        Me.btnGEarth.Size = New System.Drawing.Size(207, 51)
        Me.btnGEarth.TabIndex = 5
        Me.btnGEarth.Text = "Exportar a Google Earth"
        Me.btnGEarth.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGEarth.UseVisualStyleBackColor = True
        '
        'btnExcel
        '
        Me.btnExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExcel.Image = Global.AutoMapRNI.My.Resources.Resources.microsoft_excel_128
        Me.btnExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExcel.Location = New System.Drawing.Point(32, 236)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(165, 51)
        Me.btnExcel.TabIndex = 9
        Me.btnExcel.Text = "Exportar a Excel"
        Me.btnExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'ListaResultados
        '
        Me.ListaResultados.AllowColumnReorder = True
        Me.ListaResultados.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListaResultados.AutoArrange = False
        Me.ListaResultados.CheckBoxes = True
        Me.ListaResultados.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.indice, Me.valor, Me.valPuro, Me.hora, Me.fecha, Me.lat, Me.lng, Me.instrum, Me.sond, Me.fechacalsond, Me.incert, Me.vecesDB})
        Me.ListaResultados.Cursor = System.Windows.Forms.Cursors.Default
        Me.ListaResultados.FullRowSelect = True
        Me.ListaResultados.GridLines = True
        Me.ListaResultados.Location = New System.Drawing.Point(3, 16)
        Me.ListaResultados.Name = "ListaResultados"
        Me.ListaResultados.Size = New System.Drawing.Size(471, 214)
        Me.ListaResultados.SmallImageList = Me.ListaImgs
        Me.ListaResultados.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListaResultados.TabIndex = 0
        Me.ListaResultados.UseCompatibleStateImageBehavior = False
        Me.ListaResultados.View = System.Windows.Forms.View.Details
        '
        'indice
        '
        Me.indice.Text = "Índice"
        Me.indice.Width = 80
        '
        'valor
        '
        Me.valor.Text = "Nivel c/incert"
        Me.valor.Width = 80
        '
        'valPuro
        '
        Me.valPuro.Text = "Nivel s/incert"
        Me.valPuro.Width = 80
        '
        'hora
        '
        Me.hora.Text = "Hora"
        '
        'fecha
        '
        Me.fecha.Text = "Fecha"
        Me.fecha.Width = 63
        '
        'lat
        '
        Me.lat.Text = "Latitud"
        Me.lat.Width = 74
        '
        'lng
        '
        Me.lng.Text = "Longitud"
        Me.lng.Width = 75
        '
        'instrum
        '
        Me.instrum.Text = "Instrumento"
        '
        'sond
        '
        Me.sond.Text = "Sonda"
        '
        'fechacalsond
        '
        Me.fechacalsond.Text = "Fecha Cal Sonda"
        '
        'incert
        '
        Me.incert.Text = "Incert. [dB]"
        '
        'vecesDB
        '
        Me.vecesDB.Text = "Factor incert."
        '
        'ListaImgs
        '
        Me.ListaImgs.ImageStream = CType(resources.GetObject("ListaImgs.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ListaImgs.TransparentColor = System.Drawing.Color.Transparent
        Me.ListaImgs.Images.SetKeyName(0, "red.png")
        Me.ListaImgs.Images.SetKeyName(1, "orange.png")
        Me.ListaImgs.Images.SetKeyName(2, "yellow.png")
        Me.ListaImgs.Images.SetKeyName(3, "purple.png")
        Me.ListaImgs.Images.SetKeyName(4, "green.png")
        Me.ListaImgs.Images.SetKeyName(5, "lightblue.png")
        Me.ListaImgs.Images.SetKeyName(6, "blue.png")
        '
        'txtEventos
        '
        Me.txtEventos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtEventos.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEventos.Location = New System.Drawing.Point(817, 609)
        Me.txtEventos.Multiline = True
        Me.txtEventos.Name = "txtEventos"
        Me.txtEventos.ReadOnly = True
        Me.txtEventos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtEventos.Size = New System.Drawing.Size(477, 135)
        Me.txtEventos.TabIndex = 6
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ArchivoToolStripMenuItem, Me.OpcionesToolStripMenuItem, Me.AcercaDeToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1297, 25)
        Me.MenuStrip1.TabIndex = 6
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ArchivoToolStripMenuItem
        '
        Me.ArchivoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CargarArchivoDePuntosToolStripMenuItem, Me.ImportarDatosDeMapaToolStripMenuItem, Me.ExportarDatosDeMapaToolStripMenuItem, Me.PrefetchAreaSeleccionadaToolStripMenuItem})
        Me.ArchivoToolStripMenuItem.Name = "ArchivoToolStripMenuItem"
        Me.ArchivoToolStripMenuItem.Size = New System.Drawing.Size(63, 21)
        Me.ArchivoToolStripMenuItem.Text = "Archivo"
        '
        'CargarArchivoDePuntosToolStripMenuItem
        '
        Me.CargarArchivoDePuntosToolStripMenuItem.Name = "CargarArchivoDePuntosToolStripMenuItem"
        Me.CargarArchivoDePuntosToolStripMenuItem.Size = New System.Drawing.Size(312, 22)
        Me.CargarArchivoDePuntosToolStripMenuItem.Text = "Cargar resultado de recorrido anterior..."
        '
        'ImportarDatosDeMapaToolStripMenuItem
        '
        Me.ImportarDatosDeMapaToolStripMenuItem.Name = "ImportarDatosDeMapaToolStripMenuItem"
        Me.ImportarDatosDeMapaToolStripMenuItem.Size = New System.Drawing.Size(312, 22)
        Me.ImportarDatosDeMapaToolStripMenuItem.Text = "Importar datos de mapa..."
        '
        'ExportarDatosDeMapaToolStripMenuItem
        '
        Me.ExportarDatosDeMapaToolStripMenuItem.Name = "ExportarDatosDeMapaToolStripMenuItem"
        Me.ExportarDatosDeMapaToolStripMenuItem.Size = New System.Drawing.Size(312, 22)
        Me.ExportarDatosDeMapaToolStripMenuItem.Text = "Exportar datos de mapa..."
        '
        'PrefetchAreaSeleccionadaToolStripMenuItem
        '
        Me.PrefetchAreaSeleccionadaToolStripMenuItem.Name = "PrefetchAreaSeleccionadaToolStripMenuItem"
        Me.PrefetchAreaSeleccionadaToolStripMenuItem.Size = New System.Drawing.Size(312, 22)
        Me.PrefetchAreaSeleccionadaToolStripMenuItem.Text = "Descargar área seleccionada..."
        '
        'OpcionesToolStripMenuItem
        '
        Me.OpcionesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InstrumentoRNIToolStripMenuItem, Me.ToolStripMenuItem2, Me.ToolStripSeparator2, Me.EstablecerAlarmaToolStripMenuItem, Me.TipoDeResultadoToolStripMenuItem, Me.ModoDebug})
        Me.OpcionesToolStripMenuItem.Name = "OpcionesToolStripMenuItem"
        Me.OpcionesToolStripMenuItem.Size = New System.Drawing.Size(75, 21)
        Me.OpcionesToolStripMenuItem.Text = "Opciones"
        '
        'InstrumentoRNIToolStripMenuItem
        '
        Me.InstrumentoRNIToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.chkEMR300, Me.chkNBM550, Me.ToolStripSeparator1, Me.ToolStripMenuItem1})
        Me.InstrumentoRNIToolStripMenuItem.Name = "InstrumentoRNIToolStripMenuItem"
        Me.InstrumentoRNIToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.InstrumentoRNIToolStripMenuItem.Text = "Instrumento RNI"
        '
        'chkEMR300
        '
        Me.chkEMR300.CheckOnClick = True
        Me.chkEMR300.Name = "chkEMR300"
        Me.chkEMR300.Size = New System.Drawing.Size(173, 22)
        Me.chkEMR300.Text = "NARDA EMR300"
        '
        'chkNBM550
        '
        Me.chkNBM550.Checked = True
        Me.chkNBM550.CheckOnClick = True
        Me.chkNBM550.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNBM550.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.USBToolStripMenuItem, Me.ÓpticoToolStripMenuItem})
        Me.chkNBM550.Name = "chkNBM550"
        Me.chkNBM550.Size = New System.Drawing.Size(173, 22)
        Me.chkNBM550.Text = "NARDA NBM550"
        '
        'USBToolStripMenuItem
        '
        Me.USBToolStripMenuItem.Checked = True
        Me.USBToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.USBToolStripMenuItem.Name = "USBToolStripMenuItem"
        Me.USBToolStripMenuItem.Size = New System.Drawing.Size(115, 22)
        Me.USBToolStripMenuItem.Text = "USB"
        '
        'ÓpticoToolStripMenuItem
        '
        Me.ÓpticoToolStripMenuItem.Name = "ÓpticoToolStripMenuItem"
        Me.ÓpticoToolStripMenuItem.Size = New System.Drawing.Size(115, 22)
        Me.ÓpticoToolStripMenuItem.Text = "Óptico"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(170, 6)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cboPuertoNarda})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(173, 22)
        Me.ToolStripMenuItem1.Text = "Puerto COM"
        '
        'cboPuertoNarda
        '
        Me.cboPuertoNarda.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPuertoNarda.Name = "cboPuertoNarda"
        Me.cboPuertoNarda.Size = New System.Drawing.Size(152, 25)
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Garmin18XUSBToolStripMenuItem, Me.DispositivoNMEAToolStripMenuItem})
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(189, 22)
        Me.ToolStripMenuItem2.Text = "Dispositivo GPS"
        '
        'Garmin18XUSBToolStripMenuItem
        '
        Me.Garmin18XUSBToolStripMenuItem.Checked = True
        Me.Garmin18XUSBToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Garmin18XUSBToolStripMenuItem.Name = "Garmin18XUSBToolStripMenuItem"
        Me.Garmin18XUSBToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.Garmin18XUSBToolStripMenuItem.Text = "Garmin 18X"
        '
        'DispositivoNMEAToolStripMenuItem
        '
        Me.DispositivoNMEAToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cboCOMGlobalSat})
        Me.DispositivoNMEAToolStripMenuItem.Name = "DispositivoNMEAToolStripMenuItem"
        Me.DispositivoNMEAToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.DispositivoNMEAToolStripMenuItem.Text = "Dispositivo NMEA"
        '
        'cboCOMGlobalSat
        '
        Me.cboCOMGlobalSat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCOMGlobalSat.Name = "cboCOMGlobalSat"
        Me.cboCOMGlobalSat.Size = New System.Drawing.Size(121, 25)
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(186, 6)
        '
        'EstablecerAlarmaToolStripMenuItem
        '
        Me.EstablecerAlarmaToolStripMenuItem.Name = "EstablecerAlarmaToolStripMenuItem"
        Me.EstablecerAlarmaToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.EstablecerAlarmaToolStripMenuItem.Text = "Establecer alarma..."
        '
        'TipoDeResultadoToolStripMenuItem
        '
        Me.TipoDeResultadoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.chkMaxHold, Me.chkMaxAvg, Me.chkActual})
        Me.TipoDeResultadoToolStripMenuItem.Name = "TipoDeResultadoToolStripMenuItem"
        Me.TipoDeResultadoToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.TipoDeResultadoToolStripMenuItem.Text = "Tipo de resultado"
        '
        'chkMaxHold
        '
        Me.chkMaxHold.Checked = True
        Me.chkMaxHold.CheckOnClick = True
        Me.chkMaxHold.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMaxHold.Name = "chkMaxHold"
        Me.chkMaxHold.Size = New System.Drawing.Size(190, 22)
        Me.chkMaxHold.Text = "Max Hold"
        '
        'chkMaxAvg
        '
        Me.chkMaxAvg.CheckOnClick = True
        Me.chkMaxAvg.Name = "chkMaxAvg"
        Me.chkMaxAvg.Size = New System.Drawing.Size(190, 22)
        Me.chkMaxAvg.Text = "Max Average"
        '
        'chkActual
        '
        Me.chkActual.Name = "chkActual"
        Me.chkActual.Size = New System.Drawing.Size(190, 22)
        Me.chkActual.Text = "Actual (instantáneo)"
        Me.chkActual.Visible = False
        '
        'ModoDebug
        '
        Me.ModoDebug.CheckOnClick = True
        Me.ModoDebug.Name = "ModoDebug"
        Me.ModoDebug.Size = New System.Drawing.Size(189, 22)
        Me.ModoDebug.Text = "Modo debug"
        Me.ModoDebug.Visible = False
        '
        'AcercaDeToolStripMenuItem
        '
        Me.AcercaDeToolStripMenuItem.Name = "AcercaDeToolStripMenuItem"
        Me.AcercaDeToolStripMenuItem.Size = New System.Drawing.Size(78, 21)
        Me.AcercaDeToolStripMenuItem.Text = "Acerca de"
        '
        'tmrGPS
        '
        Me.tmrGPS.Enabled = True
        '
        'tmrNarda
        '
        Me.tmrNarda.Interval = 1000
        '
        'tmrRetardo
        '
        '
        'tmrCamp
        '
        Me.tmrCamp.Interval = 5000
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.comboGPSSelected)
        Me.GroupBox6.Controls.Add(Me.linkScanGPSPorts)
        Me.GroupBox6.Controls.Add(Me.txtLngActual)
        Me.GroupBox6.Controls.Add(Me.txtLatActual)
        Me.GroupBox6.Controls.Add(Me.Label11)
        Me.GroupBox6.Controls.Add(Me.Label10)
        Me.GroupBox6.Controls.Add(Me.chkAutoLoc)
        Me.GroupBox6.Controls.Add(Me.lblStatusGPS)
        Me.GroupBox6.Controls.Add(Me.Label6)
        Me.GroupBox6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox6.Location = New System.Drawing.Point(308, 3)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(277, 280)
        Me.GroupBox6.TabIndex = 7
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "GPS"
        '
        'comboGPSSelected
        '
        Me.comboGPSSelected.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.comboGPSSelected.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboGPSSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.comboGPSSelected.FormattingEnabled = True
        Me.comboGPSSelected.Items.AddRange(New Object() {"Garmin 18X USB", "NMEA0183 (Globalsat)"})
        Me.comboGPSSelected.Location = New System.Drawing.Point(63, 28)
        Me.comboGPSSelected.Name = "comboGPSSelected"
        Me.comboGPSSelected.Size = New System.Drawing.Size(164, 23)
        Me.comboGPSSelected.TabIndex = 30
        '
        'linkScanGPSPorts
        '
        Me.linkScanGPSPorts.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.linkScanGPSPorts.AutoSize = True
        Me.linkScanGPSPorts.Enabled = False
        Me.linkScanGPSPorts.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.linkScanGPSPorts.Location = New System.Drawing.Point(35, 212)
        Me.linkScanGPSPorts.Name = "linkScanGPSPorts"
        Me.linkScanGPSPorts.Size = New System.Drawing.Size(218, 17)
        Me.linkScanGPSPorts.TabIndex = 29
        Me.linkScanGPSPorts.TabStop = True
        Me.linkScanGPSPorts.Text = "Detectar GPS conectado (NMEA)"
        '
        'txtLngActual
        '
        Me.txtLngActual.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLngActual.Location = New System.Drawing.Point(119, 142)
        Me.txtLngActual.Name = "txtLngActual"
        Me.txtLngActual.ReadOnly = True
        Me.txtLngActual.Size = New System.Drawing.Size(108, 20)
        Me.txtLngActual.TabIndex = 27
        Me.txtLngActual.Text = "--"
        Me.txtLngActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtLatActual
        '
        Me.txtLatActual.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLatActual.Location = New System.Drawing.Point(119, 118)
        Me.txtLatActual.Name = "txtLatActual"
        Me.txtLatActual.ReadOnly = True
        Me.txtLatActual.Size = New System.Drawing.Size(108, 20)
        Me.txtLatActual.TabIndex = 26
        Me.txtLatActual.Text = "--"
        Me.txtLatActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(67, 145)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(48, 13)
        Me.Label11.TabIndex = 25
        Me.Label11.Text = "Longitud"
        '
        'Label10
        '
        Me.Label10.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(67, 121)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(39, 13)
        Me.Label10.TabIndex = 24
        Me.Label10.Text = "Latitud"
        '
        'chkAutoLoc
        '
        Me.chkAutoLoc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkAutoLoc.AutoSize = True
        Me.chkAutoLoc.Checked = True
        Me.chkAutoLoc.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAutoLoc.Location = New System.Drawing.Point(73, 177)
        Me.chkAutoLoc.Name = "chkAutoLoc"
        Me.chkAutoLoc.Size = New System.Drawing.Size(139, 17)
        Me.chkAutoLoc.TabIndex = 23
        Me.chkAutoLoc.Text = "Seguimiento automático"
        Me.chkAutoLoc.UseVisualStyleBackColor = True
        '
        'lblStatusGPS
        '
        Me.lblStatusGPS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatusGPS.BackColor = System.Drawing.Color.Silver
        Me.lblStatusGPS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStatusGPS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatusGPS.Location = New System.Drawing.Point(70, 89)
        Me.lblStatusGPS.Name = "lblStatusGPS"
        Me.lblStatusGPS.Size = New System.Drawing.Size(157, 19)
        Me.lblStatusGPS.TabIndex = 22
        Me.lblStatusGPS.Text = "Desconectado"
        Me.lblStatusGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.Location = New System.Drawing.Point(70, 69)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(157, 17)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "Estado del GPS [18X USB]"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'oDialog
        '
        Me.oDialog.FileName = "OpenFileDialog1"
        '
        'Label21
        '
        Me.Label21.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label21.AutoSize = True
        Me.Label21.BackColor = System.Drawing.Color.Transparent
        Me.Label21.ForeColor = System.Drawing.Color.DimGray
        Me.Label21.Location = New System.Drawing.Point(1072, 9)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(162, 13)
        Me.Label21.TabIndex = 11
        Me.Label21.Text = "Diseñado por Martín L. Pacheco"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.53816!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.82854!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.46779!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.16551!))
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox3, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox5, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.txtEventos, 3, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox6, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox4, 2, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 25)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 286.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 69.41431!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.58568!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1297, 747)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(1297, 772)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.LinkLabel2)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(1252, 699)
        Me.Name = "frmMain"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AutoMap RNI"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.trkZoom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.groupBoxRecorrido.ResumeLayout(False)
        Me.groupBoxRecorrido.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox5.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Mapa As GMap.NET.WindowsForms.GMapControl
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboProvMapa As System.Windows.Forms.ComboBox
    Friend WithEvents cboModoConexion As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents btnIniciarCamp As System.Windows.Forms.Button
    Friend WithEvents trkZoom As System.Windows.Forms.TrackBar
    Friend WithEvents tTip As System.Windows.Forms.ToolTip
    Friend WithEvents comGPS As System.IO.Ports.SerialPort
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ArchivoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpcionesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnDetenerCamp As System.Windows.Forms.Button
    Friend WithEvents btnConectar As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblInstrumento As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tmrNarda As System.Windows.Forms.Timer
    Friend WithEvents ListaResultados As System.Windows.Forms.ListView
    Friend WithEvents indice As System.Windows.Forms.ColumnHeader
    Friend WithEvents valor As System.Windows.Forms.ColumnHeader
    Friend WithEvents hora As System.Windows.Forms.ColumnHeader
    Friend WithEvents fecha As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnGEarth As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblSonda As System.Windows.Forms.Label
    Friend WithEvents lblSondaDetect As System.Windows.Forms.Label
    Friend WithEvents lblDisplay As System.Windows.Forms.Label
    Friend WithEvents txtEventos As System.Windows.Forms.TextBox
    Friend WithEvents InstrumentoRNIToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkEMR300 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkNBM550 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents lblTipoRes As System.Windows.Forms.Label
    Friend WithEvents tmrRetardo As System.Windows.Forms.Timer
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cboPuertoNarda As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents lblCamp As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents tmrCamp As System.Windows.Forms.Timer
    Friend WithEvents cboIntervalo As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents tmrGPS As System.Windows.Forms.Timer
    Friend WithEvents lat As System.Windows.Forms.ColumnHeader
    Friend WithEvents lng As System.Windows.Forms.ColumnHeader
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents txtLngActual As System.Windows.Forms.TextBox
    Friend WithEvents txtLatActual As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents chkAutoLoc As System.Windows.Forms.CheckBox
    Friend WithEvents lblStatusGPS As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ListaImgs As System.Windows.Forms.ImageList
    Friend WithEvents sDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents CargarArchivoDePuntosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents oDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents instrum As System.Windows.Forms.ColumnHeader
    Friend WithEvents sond As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblCargado As System.Windows.Forms.Label
    Friend WithEvents AcercaDeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents opDistancia As System.Windows.Forms.RadioButton
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents opIntervalo As System.Windows.Forms.RadioButton
    Friend WithEvents txtDistancia As System.Windows.Forms.TextBox
    Friend WithEvents comNarda As System.IO.Ports.SerialPort
    Friend WithEvents USBToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ÓpticoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImportarDatosDeMapaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportarDatosDeMapaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnExcel As System.Windows.Forms.Button
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents EstablecerAlarmaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Garmin18XUSBToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DispositivoNMEAToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cboCOMGlobalSat As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents fechacalsond As System.Windows.Forms.ColumnHeader
    Friend WithEvents vecesDB As System.Windows.Forms.ColumnHeader
    Friend WithEvents incert As System.Windows.Forms.ColumnHeader
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents valPuro As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblDistAct As System.Windows.Forms.Label
    Friend WithEvents chkPausa As System.Windows.Forms.CheckBox
    Friend WithEvents TipoDeResultadoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkMaxHold As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkMaxAvg As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblIncert As System.Windows.Forms.Label
    Friend WithEvents PrefetchAreaSeleccionadaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblZoom As System.Windows.Forms.Label
    Friend WithEvents chkActual As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModoDebug As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents linkScanNardaPorts As System.Windows.Forms.LinkLabel
    Friend WithEvents groupBoxRecorrido As System.Windows.Forms.GroupBox
    Friend WithEvents barBateria As System.Windows.Forms.ProgressBar
    Friend WithEvents opK83 As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents linkScanGPSPorts As System.Windows.Forms.LinkLabel
    Friend WithEvents comboGPSSelected As System.Windows.Forms.ComboBox
    Friend WithEvents lblBattery As System.Windows.Forms.Label
End Class
