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
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtLat = New System.Windows.Forms.TextBox()
        Me.txtLng = New System.Windows.Forms.TextBox()
        Me.opBuscarCoor = New System.Windows.Forms.RadioButton()
        Me.opBuscarLugar = New System.Windows.Forms.RadioButton()
        Me.txtLugar = New System.Windows.Forms.TextBox()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lblZoom = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboProvMapa = New System.Windows.Forms.ComboBox()
        Me.cboModoConexion = New System.Windows.Forms.ComboBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.lblTipoRes = New System.Windows.Forms.Label()
        Me.lblIncert = New System.Windows.Forms.Label()
        Me.chkPausa = New System.Windows.Forms.CheckBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.lblDistAct = New System.Windows.Forms.Label()
        Me.txtDistancia = New System.Windows.Forms.TextBox()
        Me.opDistancia = New System.Windows.Forms.RadioButton()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.opIntervalo = New System.Windows.Forms.RadioButton()
        Me.cboIntervalo = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblDisplay = New System.Windows.Forms.Label()
        Me.lblSonda = New System.Windows.Forms.Label()
        Me.lblSondaDetect = New System.Windows.Forms.Label()
        Me.lblInstrumento = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.picBateria = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnDetenerCamp = New System.Windows.Forms.Button()
        Me.btnConectar = New System.Windows.Forms.Button()
        Me.btnIniciarCamp = New System.Windows.Forms.Button()
        Me.lblCamp = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.tTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.PictureBox8 = New System.Windows.Forms.PictureBox()
        Me.PictureBox7 = New System.Windows.Forms.PictureBox()
        Me.PictureBox6 = New System.Windows.Forms.PictureBox()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.comGPS = New System.IO.Ports.SerialPort(Me.components)
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtEventos = New System.Windows.Forms.TextBox()
        Me.btnGEarth = New System.Windows.Forms.Button()
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
        Me.GroupBox1.SuspendLayout()
        CType(Me.trkZoom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.picBateria, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox8.SuspendLayout()
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox5.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.SuspendLayout()
        '
        'Mapa
        '
        Me.Mapa.Bearing = 0.0!
        Me.Mapa.CanDragMap = True
        Me.Mapa.EmptyTileColor = System.Drawing.Color.LightGray
        Me.Mapa.GrayScaleMode = False
        Me.Mapa.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow
        Me.Mapa.LevelsKeepInMemmory = 5
        Me.Mapa.Location = New System.Drawing.Point(4, 20)
        Me.Mapa.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
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
        Me.Mapa.Size = New System.Drawing.Size(1051, 612)
        Me.Mapa.TabIndex = 0
        Me.Mapa.Zoom = 10.0R
        '
        'GroupBox1
        '
        Me.GroupBox1.AutoSize = True
        Me.GroupBox1.Controls.Add(Me.lblCargado)
        Me.GroupBox1.Controls.Add(Me.Mapa)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 223)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Size = New System.Drawing.Size(1063, 655)
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
        Me.lblCargado.Location = New System.Drawing.Point(8, 25)
        Me.lblCargado.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCargado.Name = "lblCargado"
        Me.lblCargado.Size = New System.Drawing.Size(0, 15)
        Me.lblCargado.TabIndex = 1
        Me.lblCargado.Visible = False
        '
        'trkZoom
        '
        Me.trkZoom.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.trkZoom.LargeChange = 3
        Me.trkZoom.Location = New System.Drawing.Point(12, 123)
        Me.trkZoom.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.trkZoom.Maximum = 17
        Me.trkZoom.Name = "trkZoom"
        Me.trkZoom.Size = New System.Drawing.Size(307, 56)
        Me.trkZoom.TabIndex = 1
        Me.trkZoom.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.trkZoom.Value = 15
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnBuscar)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.txtLat)
        Me.GroupBox2.Controls.Add(Me.txtLng)
        Me.GroupBox2.Controls.Add(Me.opBuscarCoor)
        Me.GroupBox2.Controls.Add(Me.opBuscarLugar)
        Me.GroupBox2.Controls.Add(Me.txtLugar)
        Me.GroupBox2.Location = New System.Drawing.Point(352, 37)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox2.Size = New System.Drawing.Size(332, 186)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Búsqueda"
        '
        'btnBuscar
        '
        Me.btnBuscar.Location = New System.Drawing.Point(215, 44)
        Me.btnBuscar.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(105, 53)
        Me.btnBuscar.TabIndex = 6
        Me.btnBuscar.Text = "Buscar lugar"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(4, 73)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(63, 17)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Longitud"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 48)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 17)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Latitud"
        '
        'txtLat
        '
        Me.txtLat.Location = New System.Drawing.Point(68, 44)
        Me.txtLat.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtLat.Name = "txtLat"
        Me.txtLat.Size = New System.Drawing.Size(137, 22)
        Me.txtLat.TabIndex = 4
        '
        'txtLng
        '
        Me.txtLng.Location = New System.Drawing.Point(68, 69)
        Me.txtLng.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtLng.Name = "txtLng"
        Me.txtLng.Size = New System.Drawing.Size(137, 22)
        Me.txtLng.TabIndex = 5
        '
        'opBuscarCoor
        '
        Me.opBuscarCoor.AutoSize = True
        Me.opBuscarCoor.Location = New System.Drawing.Point(8, 23)
        Me.opBuscarCoor.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.opBuscarCoor.Name = "opBuscarCoor"
        Me.opBuscarCoor.Size = New System.Drawing.Size(160, 21)
        Me.opBuscarCoor.TabIndex = 3
        Me.opBuscarCoor.Text = "Buscar coordenadas"
        Me.opBuscarCoor.UseVisualStyleBackColor = True
        '
        'opBuscarLugar
        '
        Me.opBuscarLugar.AutoSize = True
        Me.opBuscarLugar.Checked = True
        Me.opBuscarLugar.Location = New System.Drawing.Point(8, 101)
        Me.opBuscarLugar.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.opBuscarLugar.Name = "opBuscarLugar"
        Me.opBuscarLugar.Size = New System.Drawing.Size(113, 21)
        Me.opBuscarLugar.TabIndex = 1
        Me.opBuscarLugar.TabStop = True
        Me.opBuscarLugar.Text = "Buscar lugar:"
        Me.opBuscarLugar.UseVisualStyleBackColor = True
        '
        'txtLugar
        '
        Me.txtLugar.Location = New System.Drawing.Point(8, 129)
        Me.txtLugar.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtLugar.Name = "txtLugar"
        Me.txtLugar.Size = New System.Drawing.Size(311, 22)
        Me.txtLugar.TabIndex = 2
        '
        'LinkLabel2
        '
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.Location = New System.Drawing.Point(853, 17)
        Me.LinkLabel2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(150, 17)
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
        Me.GroupBox3.Location = New System.Drawing.Point(12, 37)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox3.Size = New System.Drawing.Size(332, 186)
        Me.GroupBox3.TabIndex = 3
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Opciones del mapa"
        '
        'lblZoom
        '
        Me.lblZoom.AutoSize = True
        Me.lblZoom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblZoom.Location = New System.Drawing.Point(139, 158)
        Me.lblZoom.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblZoom.Name = "lblZoom"
        Me.lblZoom.Size = New System.Drawing.Size(26, 19)
        Me.lblZoom.TabIndex = 5
        Me.lblZoom.Text = "15"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(20, 110)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(97, 17)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Nivel de zoom"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 54)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(124, 17)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Servidor de mapa:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 23)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(127, 17)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Modo de conexión:"
        '
        'cboProvMapa
        '
        Me.cboProvMapa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProvMapa.FormattingEnabled = True
        Me.cboProvMapa.Items.AddRange(New Object() {"Google Maps", "Google Hybrid", "Open Street Map"})
        Me.cboProvMapa.Location = New System.Drawing.Point(143, 50)
        Me.cboProvMapa.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboProvMapa.Name = "cboProvMapa"
        Me.cboProvMapa.Size = New System.Drawing.Size(175, 24)
        Me.cboProvMapa.TabIndex = 1
        '
        'cboModoConexion
        '
        Me.cboModoConexion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboModoConexion.FormattingEnabled = True
        Me.cboModoConexion.Items.AddRange(New Object() {"Servidor y caché", "Leer desde caché", "Peticiones al servidor"})
        Me.cboModoConexion.Location = New System.Drawing.Point(143, 20)
        Me.cboModoConexion.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboModoConexion.Name = "cboModoConexion"
        Me.cboModoConexion.Size = New System.Drawing.Size(175, 24)
        Me.cboModoConexion.TabIndex = 0
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.lblTipoRes)
        Me.GroupBox4.Controls.Add(Me.lblIncert)
        Me.GroupBox4.Controls.Add(Me.chkPausa)
        Me.GroupBox4.Controls.Add(Me.GroupBox7)
        Me.GroupBox4.Controls.Add(Me.lblDisplay)
        Me.GroupBox4.Controls.Add(Me.lblSonda)
        Me.GroupBox4.Controls.Add(Me.lblSondaDetect)
        Me.GroupBox4.Controls.Add(Me.lblInstrumento)
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Controls.Add(Me.Panel1)
        Me.GroupBox4.Controls.Add(Me.btnDetenerCamp)
        Me.GroupBox4.Controls.Add(Me.btnConectar)
        Me.GroupBox4.Controls.Add(Me.btnIniciarCamp)
        Me.GroupBox4.Controls.Add(Me.lblCamp)
        Me.GroupBox4.Controls.Add(Me.Label12)
        Me.GroupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox4.Location = New System.Drawing.Point(1079, 37)
        Me.GroupBox4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox4.Size = New System.Drawing.Size(567, 309)
        Me.GroupBox4.TabIndex = 4
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Instrumento de medición RNI"
        '
        'lblTipoRes
        '
        Me.lblTipoRes.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTipoRes.Font = New System.Drawing.Font("Microsoft Sans Serif", 5.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTipoRes.Location = New System.Drawing.Point(357, 78)
        Me.lblTipoRes.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTipoRes.Name = "lblTipoRes"
        Me.lblTipoRes.Size = New System.Drawing.Size(73, 12)
        Me.lblTipoRes.TabIndex = 12
        Me.lblTipoRes.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblIncert
        '
        Me.lblIncert.AutoSize = True
        Me.lblIncert.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblIncert.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIncert.Location = New System.Drawing.Point(380, 106)
        Me.lblIncert.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblIncert.Name = "lblIncert"
        Me.lblIncert.Size = New System.Drawing.Size(0, 13)
        Me.lblIncert.TabIndex = 13
        Me.lblIncert.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'chkPausa
        '
        Me.chkPausa.Enabled = False
        Me.chkPausa.Location = New System.Drawing.Point(313, 266)
        Me.chkPausa.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkPausa.Name = "chkPausa"
        Me.chkPausa.Size = New System.Drawing.Size(93, 37)
        Me.chkPausa.TabIndex = 19
        Me.chkPausa.Text = "Pausar recorrido"
        Me.chkPausa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.tTip.SetToolTip(Me.chkPausa, "Presione esta casilla para pausar la captura" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "de puntos sin finalizar el recorrid" & _
        "o actual")
        Me.chkPausa.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.lblDistAct)
        Me.GroupBox7.Controls.Add(Me.txtDistancia)
        Me.GroupBox7.Controls.Add(Me.opDistancia)
        Me.GroupBox7.Controls.Add(Me.Label13)
        Me.GroupBox7.Controls.Add(Me.opIntervalo)
        Me.GroupBox7.Controls.Add(Me.cboIntervalo)
        Me.GroupBox7.Controls.Add(Me.Label14)
        Me.GroupBox7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox7.Location = New System.Drawing.Point(169, 123)
        Me.GroupBox7.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox7.Size = New System.Drawing.Size(389, 87)
        Me.GroupBox7.TabIndex = 18
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Criterios de medición"
        '
        'lblDistAct
        '
        Me.lblDistAct.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDistAct.Location = New System.Drawing.Point(251, 53)
        Me.lblDistAct.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDistAct.Name = "lblDistAct"
        Me.lblDistAct.Size = New System.Drawing.Size(113, 18)
        Me.lblDistAct.TabIndex = 23
        Me.lblDistAct.Text = "Actual:"
        '
        'txtDistancia
        '
        Me.txtDistancia.Location = New System.Drawing.Point(144, 49)
        Me.txtDistancia.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtDistancia.MaxLength = 4
        Me.txtDistancia.Name = "txtDistancia"
        Me.txtDistancia.Size = New System.Drawing.Size(67, 22)
        Me.txtDistancia.TabIndex = 22
        Me.txtDistancia.Text = "20"
        '
        'opDistancia
        '
        Me.opDistancia.AutoSize = True
        Me.opDistancia.Checked = True
        Me.opDistancia.Location = New System.Drawing.Point(8, 50)
        Me.opDistancia.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.opDistancia.Name = "opDistancia"
        Me.opDistancia.Size = New System.Drawing.Size(137, 21)
        Me.opDistancia.TabIndex = 21
        Me.opDistancia.TabStop = True
        Me.opDistancia.Text = "Dist entre puntos"
        Me.opDistancia.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(211, 53)
        Me.Label13.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(30, 17)
        Me.Label13.TabIndex = 20
        Me.Label13.Text = "mts"
        '
        'opIntervalo
        '
        Me.opIntervalo.AutoSize = True
        Me.opIntervalo.Location = New System.Drawing.Point(8, 21)
        Me.opIntervalo.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.opIntervalo.Name = "opIntervalo"
        Me.opIntervalo.Size = New System.Drawing.Size(182, 21)
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
        Me.cboIntervalo.Location = New System.Drawing.Point(192, 20)
        Me.cboIntervalo.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboIntervalo.Name = "cboIntervalo"
        Me.cboIntervalo.Size = New System.Drawing.Size(67, 24)
        Me.cboIntervalo.TabIndex = 16
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(269, 23)
        Me.Label14.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(70, 17)
        Me.Label14.TabIndex = 17
        Me.Label14.Text = "segundos"
        '
        'lblDisplay
        '
        Me.lblDisplay.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDisplay.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDisplay.Location = New System.Drawing.Point(169, 74)
        Me.lblDisplay.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDisplay.Name = "lblDisplay"
        Me.lblDisplay.Size = New System.Drawing.Size(265, 48)
        Me.lblDisplay.TabIndex = 11
        Me.lblDisplay.Text = "---"
        Me.lblDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.tTip.SetToolTip(Me.lblDisplay, "Valor medido actualmente mostrado en la pantalla del instrumento")
        '
        'lblSonda
        '
        Me.lblSonda.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSonda.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSonda.Location = New System.Drawing.Point(331, 47)
        Me.lblSonda.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSonda.Name = "lblSonda"
        Me.lblSonda.Size = New System.Drawing.Size(228, 22)
        Me.lblSonda.TabIndex = 10
        Me.lblSonda.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblSondaDetect
        '
        Me.lblSondaDetect.AutoSize = True
        Me.lblSondaDetect.Location = New System.Drawing.Point(173, 50)
        Me.lblSondaDetect.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSondaDetect.Name = "lblSondaDetect"
        Me.lblSondaDetect.Size = New System.Drawing.Size(120, 17)
        Me.lblSondaDetect.TabIndex = 9
        Me.lblSondaDetect.Text = "Sonda detectada:"
        '
        'lblInstrumento
        '
        Me.lblInstrumento.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInstrumento.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInstrumento.Location = New System.Drawing.Point(331, 23)
        Me.lblInstrumento.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblInstrumento.Name = "lblInstrumento"
        Me.lblInstrumento.Size = New System.Drawing.Size(228, 18)
        Me.lblInstrumento.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(168, 23)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(153, 17)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Instrumento detectado:"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.picBateria)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Location = New System.Drawing.Point(8, 37)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(153, 248)
        Me.Panel1.TabIndex = 5
        '
        'picBateria
        '
        Me.picBateria.Image = Global.AutoMapRNI.My.Resources.Resources.bat_10
        Me.picBateria.Location = New System.Drawing.Point(4, 193)
        Me.picBateria.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.picBateria.Name = "picBateria"
        Me.picBateria.Size = New System.Drawing.Size(15, 43)
        Me.picBateria.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picBateria.TabIndex = 1
        Me.picBateria.TabStop = False
        Me.picBateria.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(151, 246)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'btnDetenerCamp
        '
        Me.btnDetenerCamp.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnDetenerCamp.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnDetenerCamp.Enabled = False
        Me.btnDetenerCamp.Location = New System.Drawing.Point(420, 265)
        Me.btnDetenerCamp.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnDetenerCamp.Name = "btnDetenerCamp"
        Me.btnDetenerCamp.Size = New System.Drawing.Size(136, 37)
        Me.btnDetenerCamp.TabIndex = 4
        Me.btnDetenerCamp.Text = "Finalizar recorrido"
        Me.btnDetenerCamp.UseVisualStyleBackColor = False
        '
        'btnConectar
        '
        Me.btnConectar.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnConectar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnConectar.Location = New System.Drawing.Point(443, 74)
        Me.btnConectar.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnConectar.Name = "btnConectar"
        Me.btnConectar.Size = New System.Drawing.Size(117, 48)
        Me.btnConectar.TabIndex = 3
        Me.btnConectar.Text = "Conectar"
        Me.btnConectar.UseVisualStyleBackColor = True
        '
        'btnIniciarCamp
        '
        Me.btnIniciarCamp.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnIniciarCamp.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnIniciarCamp.Location = New System.Drawing.Point(168, 265)
        Me.btnIniciarCamp.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnIniciarCamp.Name = "btnIniciarCamp"
        Me.btnIniciarCamp.Size = New System.Drawing.Size(124, 37)
        Me.btnIniciarCamp.TabIndex = 2
        Me.btnIniciarCamp.Text = "Iniciar recorrido"
        Me.btnIniciarCamp.UseVisualStyleBackColor = False
        '
        'lblCamp
        '
        Me.lblCamp.BackColor = System.Drawing.Color.Silver
        Me.lblCamp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCamp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCamp.Location = New System.Drawing.Point(169, 226)
        Me.lblCamp.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCamp.Name = "lblCamp"
        Me.lblCamp.Size = New System.Drawing.Size(389, 34)
        Me.lblCamp.TabIndex = 14
        Me.lblCamp.Text = "INACTIVO"
        Me.lblCamp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(168, 210)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(170, 17)
        Me.Label12.TabIndex = 13
        Me.Label12.Text = "Estado de recorido actual"
        '
        'tTip
        '
        Me.tTip.AutoPopDelay = 10000
        Me.tTip.InitialDelay = 500
        Me.tTip.ReshowDelay = 100
        '
        'GroupBox8
        '
        Me.GroupBox8.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox8.Controls.Add(Me.Label20)
        Me.GroupBox8.Controls.Add(Me.Label19)
        Me.GroupBox8.Controls.Add(Me.Label18)
        Me.GroupBox8.Controls.Add(Me.Label17)
        Me.GroupBox8.Controls.Add(Me.Label16)
        Me.GroupBox8.Controls.Add(Me.Label15)
        Me.GroupBox8.Controls.Add(Me.Label8)
        Me.GroupBox8.Controls.Add(Me.PictureBox8)
        Me.GroupBox8.Controls.Add(Me.PictureBox7)
        Me.GroupBox8.Controls.Add(Me.PictureBox6)
        Me.GroupBox8.Controls.Add(Me.PictureBox5)
        Me.GroupBox8.Controls.Add(Me.PictureBox4)
        Me.GroupBox8.Controls.Add(Me.PictureBox3)
        Me.GroupBox8.Controls.Add(Me.PictureBox2)
        Me.GroupBox8.Location = New System.Drawing.Point(692, 37)
        Me.GroupBox8.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox8.Size = New System.Drawing.Size(141, 186)
        Me.GroupBox8.TabIndex = 8
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Escala (NO UIT)"
        Me.tTip.SetToolTip(Me.GroupBox8, "La escala presentada solo se utiliza durante el recorrido a modo informativo." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "En" & _
        " los reportes, se utiliza la escala basada en la Recomendación UIT-T K.113.")
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(29, 153)
        Me.Label20.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(48, 15)
        Me.Label20.TabIndex = 13
        Me.Label20.Text = "< 2 V/m"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(29, 130)
        Me.Label19.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(48, 15)
        Me.Label19.TabIndex = 12
        Me.Label19.Text = "≥ 2 V/m"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(29, 111)
        Me.Label18.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(48, 15)
        Me.Label18.TabIndex = 11
        Me.Label18.Text = "≥ 4 V/m"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(29, 92)
        Me.Label17.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(48, 15)
        Me.Label17.TabIndex = 10
        Me.Label17.Text = "≥ 8 V/m"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(29, 74)
        Me.Label16.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(55, 15)
        Me.Label16.TabIndex = 9
        Me.Label16.Text = "≥ 14 V/m"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(29, 53)
        Me.Label15.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(55, 15)
        Me.Label15.TabIndex = 8
        Me.Label15.Text = "≥ 20 V/m"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(29, 32)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(65, 15)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "≥ 27,5 V/m"
        '
        'PictureBox8
        '
        Me.PictureBox8.Image = Global.AutoMapRNI.My.Resources.Resources.blue
        Me.PictureBox8.Location = New System.Drawing.Point(8, 154)
        Me.PictureBox8.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox8.Name = "PictureBox8"
        Me.PictureBox8.Size = New System.Drawing.Size(13, 12)
        Me.PictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox8.TabIndex = 6
        Me.PictureBox8.TabStop = False
        '
        'PictureBox7
        '
        Me.PictureBox7.Image = Global.AutoMapRNI.My.Resources.Resources.lightblue
        Me.PictureBox7.Location = New System.Drawing.Point(8, 134)
        Me.PictureBox7.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox7.Name = "PictureBox7"
        Me.PictureBox7.Size = New System.Drawing.Size(13, 12)
        Me.PictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox7.TabIndex = 5
        Me.PictureBox7.TabStop = False
        '
        'PictureBox6
        '
        Me.PictureBox6.Image = Global.AutoMapRNI.My.Resources.Resources.green
        Me.PictureBox6.Location = New System.Drawing.Point(8, 114)
        Me.PictureBox6.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(13, 12)
        Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox6.TabIndex = 4
        Me.PictureBox6.TabStop = False
        '
        'PictureBox5
        '
        Me.PictureBox5.Image = Global.AutoMapRNI.My.Resources.Resources.purple
        Me.PictureBox5.Location = New System.Drawing.Point(8, 95)
        Me.PictureBox5.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(13, 12)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox5.TabIndex = 3
        Me.PictureBox5.TabStop = False
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = Global.AutoMapRNI.My.Resources.Resources.yellow
        Me.PictureBox4.Location = New System.Drawing.Point(8, 75)
        Me.PictureBox4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(13, 12)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox4.TabIndex = 2
        Me.PictureBox4.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.AutoMapRNI.My.Resources.Resources.orange
        Me.PictureBox3.Location = New System.Drawing.Point(8, 55)
        Me.PictureBox3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(13, 12)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox3.TabIndex = 1
        Me.PictureBox3.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.AutoMapRNI.My.Resources.Resources.red
        Me.PictureBox2.Location = New System.Drawing.Point(8, 36)
        Me.PictureBox2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(13, 12)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 0
        Me.PictureBox2.TabStop = False
        '
        'comGPS
        '
        Me.comGPS.BaudRate = 4800
        Me.comGPS.PortName = "COM2"
        Me.comGPS.WriteBufferSize = 1
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.btnExcel)
        Me.GroupBox5.Controls.Add(Me.LinkLabel1)
        Me.GroupBox5.Controls.Add(Me.Label9)
        Me.GroupBox5.Controls.Add(Me.txtEventos)
        Me.GroupBox5.Controls.Add(Me.btnGEarth)
        Me.GroupBox5.Controls.Add(Me.ListaResultados)
        Me.GroupBox5.Location = New System.Drawing.Point(1079, 353)
        Me.GroupBox5.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox5.Size = New System.Drawing.Size(567, 524)
        Me.GroupBox5.TabIndex = 5
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Resultados"
        '
        'btnExcel
        '
        Me.btnExcel.Image = Global.AutoMapRNI.My.Resources.Resources.microsoft_excel_128
        Me.btnExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExcel.Location = New System.Drawing.Point(173, 293)
        Me.btnExcel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(171, 47)
        Me.btnExcel.TabIndex = 9
        Me.btnExcel.Text = "Exportar a Excel"
        Me.btnExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(156, 343)
        Me.LinkLabel1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(151, 17)
        Me.LinkLabel1.TabIndex = 8
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Copiar al portapapeles"
        Me.LinkLabel1.Visible = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(9, 343)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(138, 17)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "Eventos del sistema:"
        '
        'txtEventos
        '
        Me.txtEventos.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.txtEventos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEventos.Location = New System.Drawing.Point(9, 363)
        Me.txtEventos.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtEventos.Multiline = True
        Me.txtEventos.Name = "txtEventos"
        Me.txtEventos.ReadOnly = True
        Me.txtEventos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtEventos.Size = New System.Drawing.Size(549, 149)
        Me.txtEventos.TabIndex = 6
        '
        'btnGEarth
        '
        Me.btnGEarth.Image = CType(resources.GetObject("btnGEarth.Image"), System.Drawing.Image)
        Me.btnGEarth.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGEarth.Location = New System.Drawing.Point(349, 293)
        Me.btnGEarth.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnGEarth.Name = "btnGEarth"
        Me.btnGEarth.Size = New System.Drawing.Size(211, 47)
        Me.btnGEarth.TabIndex = 5
        Me.btnGEarth.Text = "Exportar a Google Earth"
        Me.btnGEarth.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGEarth.UseVisualStyleBackColor = True
        '
        'ListaResultados
        '
        Me.ListaResultados.AllowColumnReorder = True
        Me.ListaResultados.AutoArrange = False
        Me.ListaResultados.CheckBoxes = True
        Me.ListaResultados.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.indice, Me.valor, Me.valPuro, Me.hora, Me.fecha, Me.lat, Me.lng, Me.instrum, Me.sond, Me.fechacalsond, Me.incert, Me.vecesDB})
        Me.ListaResultados.Cursor = System.Windows.Forms.Cursors.Default
        Me.ListaResultados.FullRowSelect = True
        Me.ListaResultados.GridLines = True
        Me.ListaResultados.Location = New System.Drawing.Point(11, 23)
        Me.ListaResultados.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ListaResultados.Name = "ListaResultados"
        Me.ListaResultados.Size = New System.Drawing.Size(548, 267)
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
        'MenuStrip1
        '
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ArchivoToolStripMenuItem, Me.OpcionesToolStripMenuItem, Me.AcercaDeToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(8, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(251, 28)
        Me.MenuStrip1.TabIndex = 6
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ArchivoToolStripMenuItem
        '
        Me.ArchivoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CargarArchivoDePuntosToolStripMenuItem, Me.ImportarDatosDeMapaToolStripMenuItem, Me.ExportarDatosDeMapaToolStripMenuItem, Me.PrefetchAreaSeleccionadaToolStripMenuItem})
        Me.ArchivoToolStripMenuItem.Name = "ArchivoToolStripMenuItem"
        Me.ArchivoToolStripMenuItem.Size = New System.Drawing.Size(71, 24)
        Me.ArchivoToolStripMenuItem.Text = "Archivo"
        '
        'CargarArchivoDePuntosToolStripMenuItem
        '
        Me.CargarArchivoDePuntosToolStripMenuItem.Name = "CargarArchivoDePuntosToolStripMenuItem"
        Me.CargarArchivoDePuntosToolStripMenuItem.Size = New System.Drawing.Size(339, 24)
        Me.CargarArchivoDePuntosToolStripMenuItem.Text = "Cargar resultado de recorrido anterior..."
        '
        'ImportarDatosDeMapaToolStripMenuItem
        '
        Me.ImportarDatosDeMapaToolStripMenuItem.Name = "ImportarDatosDeMapaToolStripMenuItem"
        Me.ImportarDatosDeMapaToolStripMenuItem.Size = New System.Drawing.Size(339, 24)
        Me.ImportarDatosDeMapaToolStripMenuItem.Text = "Importar datos de mapa..."
        '
        'ExportarDatosDeMapaToolStripMenuItem
        '
        Me.ExportarDatosDeMapaToolStripMenuItem.Name = "ExportarDatosDeMapaToolStripMenuItem"
        Me.ExportarDatosDeMapaToolStripMenuItem.Size = New System.Drawing.Size(339, 24)
        Me.ExportarDatosDeMapaToolStripMenuItem.Text = "Exportar datos de mapa..."
        '
        'PrefetchAreaSeleccionadaToolStripMenuItem
        '
        Me.PrefetchAreaSeleccionadaToolStripMenuItem.Name = "PrefetchAreaSeleccionadaToolStripMenuItem"
        Me.PrefetchAreaSeleccionadaToolStripMenuItem.Size = New System.Drawing.Size(339, 24)
        Me.PrefetchAreaSeleccionadaToolStripMenuItem.Text = "Descargar área seleccionada..."
        '
        'OpcionesToolStripMenuItem
        '
        Me.OpcionesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InstrumentoRNIToolStripMenuItem, Me.ToolStripMenuItem2, Me.ToolStripSeparator2, Me.EstablecerAlarmaToolStripMenuItem, Me.TipoDeResultadoToolStripMenuItem, Me.ModoDebug})
        Me.OpcionesToolStripMenuItem.Name = "OpcionesToolStripMenuItem"
        Me.OpcionesToolStripMenuItem.Size = New System.Drawing.Size(83, 24)
        Me.OpcionesToolStripMenuItem.Text = "Opciones"
        '
        'InstrumentoRNIToolStripMenuItem
        '
        Me.InstrumentoRNIToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.chkEMR300, Me.chkNBM550, Me.ToolStripSeparator1, Me.ToolStripMenuItem1})
        Me.InstrumentoRNIToolStripMenuItem.Name = "InstrumentoRNIToolStripMenuItem"
        Me.InstrumentoRNIToolStripMenuItem.Size = New System.Drawing.Size(205, 24)
        Me.InstrumentoRNIToolStripMenuItem.Text = "Instrumento RNI"
        '
        'chkEMR300
        '
        Me.chkEMR300.CheckOnClick = True
        Me.chkEMR300.Name = "chkEMR300"
        Me.chkEMR300.Size = New System.Drawing.Size(190, 24)
        Me.chkEMR300.Text = "NARDA EMR300"
        '
        'chkNBM550
        '
        Me.chkNBM550.Checked = True
        Me.chkNBM550.CheckOnClick = True
        Me.chkNBM550.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNBM550.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.USBToolStripMenuItem, Me.ÓpticoToolStripMenuItem})
        Me.chkNBM550.Name = "chkNBM550"
        Me.chkNBM550.Size = New System.Drawing.Size(190, 24)
        Me.chkNBM550.Text = "NARDA NBM550"
        '
        'USBToolStripMenuItem
        '
        Me.USBToolStripMenuItem.Checked = True
        Me.USBToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.USBToolStripMenuItem.Name = "USBToolStripMenuItem"
        Me.USBToolStripMenuItem.Size = New System.Drawing.Size(123, 24)
        Me.USBToolStripMenuItem.Text = "USB"
        '
        'ÓpticoToolStripMenuItem
        '
        Me.ÓpticoToolStripMenuItem.Name = "ÓpticoToolStripMenuItem"
        Me.ÓpticoToolStripMenuItem.Size = New System.Drawing.Size(123, 24)
        Me.ÓpticoToolStripMenuItem.Text = "Óptico"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(187, 6)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cboPuertoNarda})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(190, 24)
        Me.ToolStripMenuItem1.Text = "Puerto COM"
        '
        'cboPuertoNarda
        '
        Me.cboPuertoNarda.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPuertoNarda.Name = "cboPuertoNarda"
        Me.cboPuertoNarda.Size = New System.Drawing.Size(152, 28)
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Garmin18XUSBToolStripMenuItem, Me.DispositivoNMEAToolStripMenuItem})
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(205, 24)
        Me.ToolStripMenuItem2.Text = "Dispositivo GPS"
        '
        'Garmin18XUSBToolStripMenuItem
        '
        Me.Garmin18XUSBToolStripMenuItem.Checked = True
        Me.Garmin18XUSBToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Garmin18XUSBToolStripMenuItem.Name = "Garmin18XUSBToolStripMenuItem"
        Me.Garmin18XUSBToolStripMenuItem.Size = New System.Drawing.Size(198, 24)
        Me.Garmin18XUSBToolStripMenuItem.Text = "Garmin 18X"
        '
        'DispositivoNMEAToolStripMenuItem
        '
        Me.DispositivoNMEAToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cboCOMGlobalSat})
        Me.DispositivoNMEAToolStripMenuItem.Name = "DispositivoNMEAToolStripMenuItem"
        Me.DispositivoNMEAToolStripMenuItem.Size = New System.Drawing.Size(198, 24)
        Me.DispositivoNMEAToolStripMenuItem.Text = "Dispositivo NMEA"
        '
        'cboCOMGlobalSat
        '
        Me.cboCOMGlobalSat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCOMGlobalSat.Name = "cboCOMGlobalSat"
        Me.cboCOMGlobalSat.Size = New System.Drawing.Size(121, 28)
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(202, 6)
        '
        'EstablecerAlarmaToolStripMenuItem
        '
        Me.EstablecerAlarmaToolStripMenuItem.Name = "EstablecerAlarmaToolStripMenuItem"
        Me.EstablecerAlarmaToolStripMenuItem.Size = New System.Drawing.Size(205, 24)
        Me.EstablecerAlarmaToolStripMenuItem.Text = "Establecer alarma..."
        '
        'TipoDeResultadoToolStripMenuItem
        '
        Me.TipoDeResultadoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.chkMaxHold, Me.chkMaxAvg, Me.chkActual})
        Me.TipoDeResultadoToolStripMenuItem.Name = "TipoDeResultadoToolStripMenuItem"
        Me.TipoDeResultadoToolStripMenuItem.Size = New System.Drawing.Size(205, 24)
        Me.TipoDeResultadoToolStripMenuItem.Text = "Tipo de resultado"
        '
        'chkMaxHold
        '
        Me.chkMaxHold.Checked = True
        Me.chkMaxHold.CheckOnClick = True
        Me.chkMaxHold.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMaxHold.Name = "chkMaxHold"
        Me.chkMaxHold.Size = New System.Drawing.Size(211, 24)
        Me.chkMaxHold.Text = "Max Hold"
        '
        'chkMaxAvg
        '
        Me.chkMaxAvg.CheckOnClick = True
        Me.chkMaxAvg.Name = "chkMaxAvg"
        Me.chkMaxAvg.Size = New System.Drawing.Size(211, 24)
        Me.chkMaxAvg.Text = "Max Average"
        '
        'chkActual
        '
        Me.chkActual.Name = "chkActual"
        Me.chkActual.Size = New System.Drawing.Size(211, 24)
        Me.chkActual.Text = "Actual (instantáneo)"
        Me.chkActual.Visible = False
        '
        'ModoDebug
        '
        Me.ModoDebug.CheckOnClick = True
        Me.ModoDebug.Name = "ModoDebug"
        Me.ModoDebug.Size = New System.Drawing.Size(205, 24)
        Me.ModoDebug.Text = "Modo debug"
        Me.ModoDebug.Visible = False
        '
        'AcercaDeToolStripMenuItem
        '
        Me.AcercaDeToolStripMenuItem.Name = "AcercaDeToolStripMenuItem"
        Me.AcercaDeToolStripMenuItem.Size = New System.Drawing.Size(87, 24)
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
        Me.GroupBox6.Controls.Add(Me.txtLngActual)
        Me.GroupBox6.Controls.Add(Me.txtLatActual)
        Me.GroupBox6.Controls.Add(Me.Label11)
        Me.GroupBox6.Controls.Add(Me.Label10)
        Me.GroupBox6.Controls.Add(Me.chkAutoLoc)
        Me.GroupBox6.Controls.Add(Me.lblStatusGPS)
        Me.GroupBox6.Controls.Add(Me.Label6)
        Me.GroupBox6.Location = New System.Drawing.Point(841, 37)
        Me.GroupBox6.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox6.Size = New System.Drawing.Size(232, 186)
        Me.GroupBox6.TabIndex = 7
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Posición actual"
        '
        'txtLngActual
        '
        Me.txtLngActual.Location = New System.Drawing.Point(73, 110)
        Me.txtLngActual.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtLngActual.Name = "txtLngActual"
        Me.txtLngActual.ReadOnly = True
        Me.txtLngActual.Size = New System.Drawing.Size(143, 22)
        Me.txtLngActual.TabIndex = 27
        Me.txtLngActual.Text = "--"
        Me.txtLngActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtLatActual
        '
        Me.txtLatActual.Location = New System.Drawing.Point(73, 80)
        Me.txtLatActual.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtLatActual.Name = "txtLatActual"
        Me.txtLatActual.ReadOnly = True
        Me.txtLatActual.Size = New System.Drawing.Size(143, 22)
        Me.txtLatActual.TabIndex = 26
        Me.txtLatActual.Text = "--"
        Me.txtLatActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(4, 113)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(63, 17)
        Me.Label11.TabIndex = 25
        Me.Label11.Text = "Longitud"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(4, 84)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(51, 17)
        Me.Label10.TabIndex = 24
        Me.Label10.Text = "Latitud"
        '
        'chkAutoLoc
        '
        Me.chkAutoLoc.AutoSize = True
        Me.chkAutoLoc.Checked = True
        Me.chkAutoLoc.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAutoLoc.Location = New System.Drawing.Point(12, 153)
        Me.chkAutoLoc.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkAutoLoc.Name = "chkAutoLoc"
        Me.chkAutoLoc.Size = New System.Drawing.Size(181, 21)
        Me.chkAutoLoc.TabIndex = 23
        Me.chkAutoLoc.Text = "Seguimiento automático"
        Me.chkAutoLoc.UseVisualStyleBackColor = True
        '
        'lblStatusGPS
        '
        Me.lblStatusGPS.BackColor = System.Drawing.Color.Silver
        Me.lblStatusGPS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblStatusGPS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatusGPS.Location = New System.Drawing.Point(8, 44)
        Me.lblStatusGPS.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStatusGPS.Name = "lblStatusGPS"
        Me.lblStatusGPS.Size = New System.Drawing.Size(209, 23)
        Me.lblStatusGPS.TabIndex = 22
        Me.lblStatusGPS.Text = "Desconectado"
        Me.lblStatusGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 20)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(209, 21)
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
        Me.Label21.AutoSize = True
        Me.Label21.ForeColor = System.Drawing.Color.DimGray
        Me.Label21.Location = New System.Drawing.Point(1429, 11)
        Me.Label21.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(211, 17)
        Me.Label21.TabIndex = 11
        Me.Label21.Text = "Diseñado por Martín L. Pacheco"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(1661, 889)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.LinkLabel2)
        Me.Controls.Add(Me.GroupBox8)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MinimumSize = New System.Drawing.Size(1667, 853)
        Me.Name = "frmMain"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AutoMap RNI"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.trkZoom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        CType(Me.picBateria, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Mapa As GMap.NET.WindowsForms.GMapControl
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboProvMapa As System.Windows.Forms.ComboBox
    Friend WithEvents cboModoConexion As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtLat As System.Windows.Forms.TextBox
    Friend WithEvents txtLng As System.Windows.Forms.TextBox
    Friend WithEvents opBuscarCoor As System.Windows.Forms.RadioButton
    Friend WithEvents opBuscarLugar As System.Windows.Forms.RadioButton
    Friend WithEvents txtLugar As System.Windows.Forms.TextBox
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
    Friend WithEvents Label9 As System.Windows.Forms.Label
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
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents opDistancia As System.Windows.Forms.RadioButton
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents opIntervalo As System.Windows.Forms.RadioButton
    Friend WithEvents txtDistancia As System.Windows.Forms.TextBox
    Friend WithEvents comNarda As System.IO.Ports.SerialPort
    Friend WithEvents USBToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ÓpticoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImportarDatosDeMapaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportarDatosDeMapaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents btnExcel As System.Windows.Forms.Button
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox8 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox7 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
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
    Friend WithEvents btnBuscar As System.Windows.Forms.Button
    Friend WithEvents valPuro As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblDistAct As System.Windows.Forms.Label
    Friend WithEvents picBateria As System.Windows.Forms.PictureBox
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
End Class
