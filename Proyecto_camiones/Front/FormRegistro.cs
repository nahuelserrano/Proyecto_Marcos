
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Label = System.Windows.Forms.Label;
using Proyecto_camiones.ViewModels;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Models;
using Proyecto_camiones.DTOs;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using Proyecto_camiones.Presentacion.Models;
using MySqlX.XDevAPI.Common;
using Mysqlx.Session;
using System.Data;

namespace Proyecto_camiones.Front;

public class FormRegistro : Home
{
    //Form
    private NewRoundPanel formPanel = new NewRoundPanel(20);
    private FlowLayoutPanel formFLTextBox = new FlowLayoutPanel();
    private FlowLayoutPanel formFLLabel = new FlowLayoutPanel();

    private List<string> campos = new List<string>();

    //Button
    private Panel btnPanel = new Panel();
    private RoundButton btnCargar = new RoundButton();
    private RoundButton btnVolver = new RoundButton();
    private RoundButton btnCuentaCorriente = new RoundButton();
    private RoundButton btnSueldoMensual = new RoundButton();

    //Grid
    private DataGridView cheq = new DataGridView();
    private Panel panelGrid = new Panel();

    private DataGridViewButtonColumn eliminar = new DataGridViewButtonColumn();
    private DataGridViewButtonColumn modificar = new DataGridViewButtonColumn();
    private DataGridViewButtonColumn pagado = new DataGridViewButtonColumn();

    //¿Dónde estoy parado?
    private System.Windows.Forms.Label nombre = new System.Windows.Forms.Label();

    //cartel de aviso
    private NewRoundPanel avisoPanel = new NewRoundPanel(20);
    private Button btnAceptarAviso = new Button();


    //Constructor
    public FormRegistro(List<string> camposForm, int cant, string dato, string filtro, List<string> camposFaltantesTablas, string choferCamion)
    {
        InitializeUI(camposForm, cant, filtro, camposFaltantesTablas, dato, choferCamion);

        //ShowForm
        CargarFormulario(camposForm, cant, filtro);

        //Hovers
        btnCargar.MouseEnter += (s, e) => HoverEffect(s, e, true);
        btnCargar.MouseLeave += (s, e) => HoverEffect(s, e, false);

        //Events
        btnCargar.Click += (s, e) => BtnCargar_Click(s, e, filtro, dato);


        cheq.CellClick += (s, e) => EliminarFila(s, e, filtro, dato);
        cheq.CellClick += (s, e) => ModificarFilaAsync(s, e, dato, filtro);


        if (filtro == "sueldo")
        {
            cheq.CellClick += MarcarComoPagado;
        }

        ConfigurarDataGridView(filtro);

        LabelProperties(dato);

        AddButtonCuentaCorriente(filtro, dato);
        AddButtonSueldoMensual(filtro, dato, choferCamion);

        this.TopLevel = false;
        this.FormBorderStyle = FormBorderStyle.None;
        this.Dock = DockStyle.Fill; // (opcional, para ocupar todo el espacio disponible)
    }

    //Initializations
    private void InitializeUI(List<string> camposForm, int cant, string filtro, List<string> camposFaltantesTablas, string dato, string choferCamion)
    {
        this.AutoScaleMode = AutoScaleMode.Dpi;
        this.AutoSize = true;
        this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        AddItemsToGrid(camposForm, cant, camposFaltantesTablas, filtro);
        ResaltarBoton(viajesMenu);
        GridChequesProperties();
        ButtonProperties(filtro, dato);
        ShowInfoTable(filtro, dato, choferCamion);
    }

    private async void BtnCargar_Click(object sender, EventArgs e, string filtro, string dato)
    {
        btnCargar.Enabled = false;

        await cargaClickEvent(sender, e, filtro, dato); // Ejecuta tu lógica

        btnCargar.Enabled = true;
    }

    private async void ShowInfoTable(string filtro, string dato, string choferCamion)
    {
        cheq.Rows.Clear();

        if (filtro == "Camion")
        {
            ViajeViewModel vvm = new ViajeViewModel();
            var result = await vvm.ObtenerPorCamionAsync(dato);

            if (result.IsSuccess)
            {
                foreach (DataGridViewColumn col in cheq.Columns)
                {
                    if (col.HeaderText.Equals("Tarifa") || col.HeaderText.Equals("Total") || col.HeaderText.Equals("Monto chofer")) // O el nombre correcto de tu columna de monto
                    {
                        col.DefaultCellStyle.Format = "N2";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }

                // SEGUNDO: Agregar las filas con los datos convertidos
                foreach (var viaje in result.Value)
                {
                    // Convertir el monto a decimal si viene como string
                    decimal tarifaDecimal = 0;
                    decimal totalDecimal = 0;
                    decimal totalGananciaChofer = 0;

                    if (viaje.Tarifa is float || viaje.Total is float)
                    {
                        tarifaDecimal = (decimal)viaje.Tarifa;
                        totalDecimal = (decimal)viaje.Total;
                        totalGananciaChofer = (decimal)viaje.GananciaChofer;
                    }
                    cheq.Rows.Add(viaje.FechaInicio, viaje.LugarPartida, viaje.Destino, viaje.Remito, viaje.Carga, viaje.Km, viaje.Kg, tarifaDecimal, viaje.PorcentajeChofer, viaje.NombreCliente, viaje.NombreChofer, totalDecimal, totalGananciaChofer, viaje.Id);

                }
            }

            else
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => CartelAviso(result.Error)));
                }
                else
                {
                    CartelAviso(result.Error);
                }
            }
        }
        else if (filtro == "Cliente")
        {
            ClienteViewModel cvm = new ClienteViewModel();
            var resultClient = await cvm.ObtenerViajesDeUnCliente(dato);

            if (resultClient.IsSuccess)
            {
                foreach (DataGridViewColumn col in cheq.Columns)
                {
                    if (col.HeaderText.Equals("Tarifa") || col.HeaderText.Equals("Total")) // O el nombre correcto de tu columna de monto
                    {
                        col.DefaultCellStyle.Format = "N2";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }

                // SEGUNDO: Agregar las filas con los datos convertidos
                foreach (var cliente in resultClient.Value)
                {
                    // Convertir el monto a decimal si viene como string
                    decimal tarifaDecimal = 0;
                    decimal totalDecimal = 0;

                    if (cliente.Tarifa is float || cliente.Total is float)
                    {
                        tarifaDecimal = (decimal)cliente.Tarifa;
                        totalDecimal = (decimal)cliente.Total;
                    }
                    cheq.Rows.Add(cliente.Fecha_salida, cliente.Origen, cliente.Destino, cliente.Remito, cliente.Carga, cliente.Km, cliente.Kg, tarifaDecimal, cliente.Nombre_chofer, cliente.Camion, cliente.Fletero, totalDecimal, cliente.Id);
                }
            }

            else
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => CartelAviso(resultClient.Error)));
                }
                else
                {
                    CartelAviso(resultClient.Error);
                }
            }
        }
        else if (filtro == "Flete")
        {
            ViajeFleteViewModel fvm = new ViajeFleteViewModel();
            var resultFlete = await fvm.ObtenerViajesDeUnFleteroAsync(dato);

            if (resultFlete.IsSuccess)
            {
                foreach (DataGridViewColumn col in cheq.Columns)
                {
                    if (col.HeaderText.Equals("Tarifa") || col.HeaderText.Equals("Total") || col.HeaderText.Equals("Total comisión")) // O el nombre correcto de tu columna de monto
                    {
                        col.DefaultCellStyle.Format = "N2";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }

                // SEGUNDO: Agregar las filas con los datos convertidos
                foreach (var flete in resultFlete.Value)
                {
                    // Convertir el monto a decimal si viene como string
                    decimal tarifaDecimal = 0;
                    decimal totalComisionDecimal = 0;
                    decimal totalDecimal = 0;

                    if (flete.tarifa is float || flete.total_comision is float || flete.total is float)
                    {
                        tarifaDecimal = (decimal)flete.tarifa;
                        totalComisionDecimal = (decimal)flete.total_comision;
                        totalDecimal = (decimal)flete.total;
                    }
                    cheq.Rows.Add(flete.fecha_salida, flete.origen, flete.destino, flete.remito, flete.carga, flete.km, flete.kg, tarifaDecimal, flete.factura, flete.comision, flete.cliente, flete.nombre_chofer, totalDecimal, totalComisionDecimal, flete.idViajeFlete);
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => CartelAviso(resultFlete.Error)));
                }
                else
                {
                    CartelAviso(resultFlete.Error);
                }
            }
        }
        else if (filtro == "cuenta corriente")
        {
            CuentaCorrienteViewModel ccvm = new CuentaCorrienteViewModel();
            var resultCuentaCorriente = await ccvm.ObtenerCuentasByClienteAsync(dato);

            if (resultCuentaCorriente.IsSuccess)
            {
                foreach (DataGridViewColumn col in cheq.Columns)
                {
                    if (col.HeaderText.Equals("Pagado") || col.HeaderText.Equals("Adeuda") || col.HeaderText.Equals("Total adeudado")) // O el nombre correcto de tu columna de monto
                    {
                        col.DefaultCellStyle.Format = "N2";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }

                // SEGUNDO: Agregar las filas con los datos convertidos
                foreach (var cuenta in resultCuentaCorriente.Value)
                {
                    // Convertir el monto a decimal si viene como string
                    decimal pagadoDecimal = 0;
                    decimal adeudaDecimal = 0;
                    decimal totalDecimal = 0;

                    if (cuenta.Pagado is float || cuenta.Adeuda is float || cuenta.Saldo_Total is float)
                    {
                        pagadoDecimal = (decimal)cuenta.Pagado;
                        adeudaDecimal = (decimal)cuenta.Adeuda;
                        totalDecimal = (decimal)cuenta.Saldo_Total;
                    }


                    cheq.Rows.Add(cuenta.Fecha_factura, cuenta.Nro_factura, pagadoDecimal, adeudaDecimal, totalDecimal, cuenta.idCuenta);
                }

            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => CartelAviso(resultCuentaCorriente.Error)));
                }
                else
                {
                    CartelAviso(resultCuentaCorriente.Error);
                }
            }
        }
        else if (filtro == "sueldo")
        {
            SueldoViewModel svm = new SueldoViewModel();
            SueldoDTO sdto = new SueldoDTO();
            var resultSueldo = await svm.ObtenerTodosAsync(dato, choferCamion);

            if (resultSueldo.IsSuccess)
            {
                foreach (DataGridViewColumn col in cheq.Columns)
                {
                    if (col.HeaderText.Equals("Sueldo"))  // O el nombre correcto de tu columna de monto
                    {
                        col.DefaultCellStyle.Format = "N2";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        break; // Salir del bucle una vez encontrada la columna
                    }
                }

                // SEGUNDO: Agregar las filas con los datos convertidos
                foreach (var sueldo in resultSueldo.Value)
                {
                    string fechas = sueldo.PagadoDesde + " - " + sueldo.PagadoHasta;

                    // Convertir el monto a decimal si viene como string
                    decimal montoDecimal = 0;

                    if (sueldo.Monto_Pagado is float)
                    {
                        montoDecimal = (decimal)sueldo.Monto_Pagado;
                    }

                    // Agregar la fila con el valor decimal
                    int rowIndex = cheq.Rows.Add(fechas, sueldo.chofer, montoDecimal, sueldo.idSueldo);

                    // Aplicar color si está pagado
                    if (sdto.Pagado)  // Asumo que quieres usar alguna propiedad del sueldo actual
                    {
                        cheq.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Green;
                    }
                }
            }

            else
            {
                CartelAviso(resultSueldo.Error);
            }
        }
    }

    //Adds
    private void AddItemsToGrid(List<string> camposForm, int cant, List<string> camposFaltantesTabla, string filtro)
    {
        if (filtro != "sueldo")
        {
            foreach (string campos in camposForm)
            {
                cheq.Columns.Add(campos, campos);
            }
        }
        if (camposFaltantesTabla != null)
        {
            foreach (string campos in camposFaltantesTabla)
            {
                cheq.Columns.Add(campos, campos);
            }
        }
        cheq.Columns.Add("Id", "Id");

        foreach (DataGridViewColumn col in cheq.Columns)
        {
            if (col.HeaderText.Equals("Id"))
            {
                col.Visible = false;
            }
        }

        panelGrid.Controls.Add(cheq);
        this.Controls.Add(panelGrid);
    }
    public void addColumn(string s)
    {
        cheq.Columns.Add(s, s);
    }


    //HoverFunction
    private void HoverEffect(object sender, EventArgs e, bool isHover)
    {
        var button = sender as Button;
        if (button != null)
        {
            button.ForeColor = isHover ? Color.FromArgb(48, 48, 48) : Color.Black;
        }
    }
    private void CargarFormulario(List<string> camposForm, int cant, string filtro)
    {
        if (filtro != "Cliente")
        {
            this.campos.Clear();
            foreach (string i in camposForm)
            {
                this.campos.Add(i);
            }
            InitializeFormProperties(cant, campos, filtro);
        }
    }
    private void InitializeFormProperties(int cant, List<string> campos, string filtro)
    {
        FormProperties(cant);
        LayoutFormProperties(cant);
        TextoBoxAndLabelProperties(cant, campos);
        ButtonsPropertiesForm(filtro);
        AddControls();
    }

    //FormProperties

    private void FormProperties(int cant)
    {
        if (cant > 8)
        {
            formPanel.Size = new Size(this.Width - btnVolver.Width - btnCargar.Width - 120, 70);
        }
        else
        {
            formPanel.Size = new Size(100 * cant, 70);
        
        }
        formPanel.BackColor = Color.FromArgb(45, 45, 48); // Gris oscuro moderno

        this.Resize += (s, e) =>
        {
            formPanel.Location = new Point((this.Width - formPanel.Width + 15) / 2, 115);
        };
    }
    private void LayoutFormProperties(int cant)
    {
        formFLTextBox = PropertiesLayoutForm();
        formFLLabel = PropertiesLayoutForm();

        formFLTextBox.Size = new Size(formPanel.Width - 10, 55);
        formFLTextBox.BackColor = Color.Transparent;
        formFLTextBox.Dock = DockStyle.Bottom;

        formFLLabel.Size = new Size(formPanel.Width - 10, 30);
        formFLLabel.Dock = DockStyle.Top;

        formPanel.Controls.Add(formFLLabel);
        formPanel.Controls.Add(formFLTextBox);
    }

    private FlowLayoutPanel PropertiesLayoutForm()
    {
        FlowLayoutPanel formFL = new FlowLayoutPanel();
        formFL.FlowDirection = FlowDirection.LeftToRight;
        formFL.WrapContents = false;
        formFL.BackColor = Color.Transparent;
        formFL.AutoScroll = true;
        return formFL;
    }

    //TextBoxProperties
    private void TextoBoxAndLabelProperties(int cant, List<string> campos)
    {
        foreach (string campo in campos)
        {
            FlowLayoutPanel campoPanel = PropertiesFormPanel();
            TextBox textBoxForm = CreateTextBoxAndProperties(campo);
            Label labelForm = CreateLabelAndProperties(campo);

            formFLLabel.Controls.Add(labelForm);

            campoPanel.Controls.Add(textBoxForm);
            formFLTextBox.Controls.Add(campoPanel);
        }
    }

    private FlowLayoutPanel PropertiesFormPanel()
    {
        FlowLayoutPanel campoTextBox = new FlowLayoutPanel();
        campoTextBox.Size = new Size(105, 40);
        campoTextBox.Margin = new Padding(2, 0, 2, 0);
        campoTextBox.BackColor = Color.Transparent;

        return campoTextBox;
    }

    private Label CreateLabelAndProperties(object campo)
    {
        Label ll = new Label();
        ll.Text = campo.ToString();
        ll.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        ll.ForeColor = Color.FromArgb(220, 220, 220);
        ll.BackColor = Color.Transparent;
        ll.Size = new Size(105, 25);
        ll.TextAlign = ContentAlignment.MiddleCenter;
        ll.Margin = new Padding(2, 0, 2, 0);

        // Efecto de sombra del texto
        ll.FlatStyle = FlatStyle.Flat;

        return ll;
    }
    private TextBox CreateTextBoxAndProperties(object campo)
    {
        TextBox textBoxCampo = new TextBox();

        // Estilo base
        textBoxCampo.Font = new Font("Segoe UI", 9, FontStyle.Regular);
        textBoxCampo.BackColor = Color.FromArgb(60, 60, 65);
        textBoxCampo.ForeColor = Color.FromArgb(220, 220, 220);
        textBoxCampo.Margin = new Padding(0, 18, 0, 0);
        textBoxCampo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        textBoxCampo.Dock = DockStyle.None;
        textBoxCampo.Anchor = AnchorStyles.None; // o la configuración que necesites

        // Placeholder mejorado
        string placeholderText = campo.ToString();
        textBoxCampo.Text = placeholderText;
        textBoxCampo.ForeColor = Color.FromArgb(150, 150, 150);

        // Eventos mejorados
        textBoxCampo.GotFocus += (s, e) =>
        {
            if (textBoxCampo.Text == placeholderText)
            {
                textBoxCampo.Text = "";
                textBoxCampo.ForeColor = Color.FromArgb(220, 220, 220);
            }
            // Efecto de foco
            textBoxCampo.BackColor = Color.FromArgb(70, 70, 75);
        };

        textBoxCampo.LostFocus += (s, e) =>
        {
            if (string.IsNullOrWhiteSpace(textBoxCampo.Text))
            {
                textBoxCampo.Text = placeholderText;
                textBoxCampo.ForeColor = Color.FromArgb(150, 150, 150);
            }
            // Restaurar color
            textBoxCampo.BackColor = Color.FromArgb(60, 60, 65);
        };
        return textBoxCampo;
    }
   

    //GridProperties
    private void GridChequesProperties()
    {
        int anchoPantalla = Screen.FromControl(this).Bounds.Width;
        int altoPantalla = Screen.FromControl(this).Bounds.Height;

        panelGrid.Size = new Size(anchoPantalla - 200, altoPantalla);
        panelGrid.BackColor = Color.Transparent;

        this.Resize += (s, e) =>
        {
            panelGrid.Location = new Point((this.Width - panelGrid.Width) / 2, 320);
        };

        cheq.Size = new Size(panelGrid.Width, panelGrid.Height);
        cheq.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        cheq.Height = 400;
        cheq.BackgroundColor = Color.DarkGray;
        cheq.GridColor = Color.Black;
        cheq.Font = new Font("Nunito", 12, FontStyle.Bold);

        cheq.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
        cheq.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        cheq.EnableHeadersVisualStyles = false;
        cheq.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        cheq.AllowUserToResizeRows = false;


        panelGrid.Controls.Add(cheq);
        this.Controls.Add(panelGrid);
    }

    //Otros
    //CargaDeDatos

    private void ConfigurarDataGridView(string filtro)
    {
        if (filtro == "sueldo")
        {
            if (cheq.Columns["Pagado"] == null)
            {
                DataGridViewButtonColumn btnPagado = new DataGridViewButtonColumn();
                btnPagado.Name = "Pagado";
                btnPagado.HeaderText = "Pagado";  // Puedes dejarlo vacío si prefieres
                btnPagado.Text = "✔"; // Ícono de modificar
                btnPagado.UseColumnTextForButtonValue = true; // Hace que todas las celdas muestren "✔"

                cheq.Columns.Add(btnPagado);
            }
        }
        if (filtro != "Cliente")
        {
            if (cheq.Columns["Eliminar"] == null)
            {
                DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
                btnEliminar.Name = "Eliminar";
                btnEliminar.HeaderText = "Eliminar";  // Puedes dejarlo vacío si prefieres
                btnEliminar.Text = "🗑"; // Ícono de eliminar
                btnEliminar.UseColumnTextForButtonValue = true; // Hace que todas las celdas muestren "❌"
                btnEliminar.Width = 20; // Ajustar tamaño

                cheq.Columns.Add(btnEliminar);
            }

            if (filtro != "sueldo")
            {
                if (cheq.Columns["Modificar"] == null)
                {
                    DataGridViewButtonColumn btnModificar = new DataGridViewButtonColumn();
                    btnModificar.Name = "Modificar";
                    btnModificar.HeaderText = "Modificar";  // Puedes dejarlo vacío si prefieres
                    btnModificar.Text = "✏"; // Ícono de modificar
                    btnModificar.UseColumnTextForButtonValue = true; // Hace que todas las celdas muestren "✏"

                    cheq.Columns.Add(btnModificar);
                }
            }
        }
    }

    private async Task cargaClickEvent(object sender, EventArgs e, string filtro, string dato)
    {
        // Obtener los valores de los TextBox
        List<string> datos = new List<string>();

        foreach (Control control in formFLTextBox.Controls)
        {
            if (control is Panel panel)
            {
                foreach (Control child in panel.Controls)
                {
                    if (child is TextBox textBox)
                    {
                        foreach (string campo in campos)
                        {

                            if (textBox.Name == campo)
                            {
                                if (campo == "Fecha" || campo == "Fecha inicial" || campo == "Fecha final")
                                {
                                    TextBox campoFecha = textBox;
                                    DateTime fecha;
                                    if (!DateTime.TryParse(campoFecha.Text, out fecha))
                                    {

                                        if (this.InvokeRequired)
                                        {
                                            this.Invoke(new Action(() => CartelAviso("Por favor, ingrese una fecha válida")));
                                        }
                                        else
                                        {
                                            CartelAviso("Ingrese una fecha válida");
                                        }

                                        textBox.Focus();
                                        return;
                                    }
                                }
                                else if (campo == "Km" || campo == "Kg" || campo == "Tarifa" || campo == "RTO o CPE")
                                {
                                    TextBox campoKm = textBox;
                                    int km;
                                    if (!int.TryParse(campoKm.Text, out km))
                                    {
                                        if (this.InvokeRequired)
                                        {
                                            this.Invoke(new Action(() => CartelAviso("Por favor, ingrese un número válido")));
                                        }
                                        else
                                        {
                                            CartelAviso("Ingrese un número válido");
                                        }

                                        textBox.Focus();
                                        return;
                                    }
                                }
                            }
                        }
                        datos.Add(textBox.Text); // Agregar el texto de cada TextBox
                    }
                }
            }
        }

        if (datos != null)
        {

            if (filtro == "Camion")
            {
                ViajeViewModel viajeViewModel = new ViajeViewModel();
                if (datos[10] == "Chofer")
                {
                    datos[10] = null;
                }
                var resultado = await viajeViewModel.CrearAsync(DateOnly.Parse(datos[0]), datos[1], datos[2], int.Parse(datos[3]), datos[4], float.Parse(datos[6]), datos[9], dato, float.Parse(datos[5]), float.Parse(datos[7]), datos[10], float.Parse(datos[8]));

                if (resultado.IsSuccess)
                {
                    ShowInfoTable(filtro, dato, " ");
                }
                else
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => CartelAviso(resultado.Error)));
                    }
                    else
                    {
                        CartelAviso(resultado.Error);
                    }
                }
            }
            else if (filtro == "cuenta corriente")
            {
                CuentaCorrienteViewModel ccvm = new CuentaCorrienteViewModel();
                var resultado = await ccvm.InsertarAsync(dato, null, DateOnly.Parse(datos[0]), int.Parse(datos[1]), float.Parse(datos[3]), float.Parse(datos[2]));

                if (resultado.IsSuccess)
                {
                    ShowInfoTable(filtro, dato, " ");
                }
                else
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => CartelAviso(resultado.Error)));
                    }
                    else
                    {
                        CartelAviso(resultado.Error);
                    }
                }
            }
            else if (filtro == "Flete")
            {
                ViajeFleteViewModel vfvm = new ViajeFleteViewModel();
                var resultado = await vfvm.InsertarAsync(datos[1], datos[2], float.Parse(datos[3]), datos[4], float.Parse(datos[5]), float.Parse(datos[6]), float.Parse(datos[7]), int.Parse(datos[8]), datos[10], dato, datos[11], float.Parse(datos[9]), DateOnly.Parse(datos[0]));

                if (resultado.IsSuccess)
                {
                    ShowInfoTable(filtro, dato, " ");
                }
                else
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => CartelAviso(resultado.Error)));
                    }
                    else
                    {
                        CartelAviso(resultado.Error);
                    }
                }
            }
            else if (filtro == "sueldo")
            {
                SueldoViewModel svm = new SueldoViewModel();
                var resultado = await svm.CrearAsync(datos[2], DateOnly.Parse(datos[0]), DateOnly.Parse(datos[1]), null, dato);
                if (resultado.IsSuccess)
                {
                    ShowInfoTable(filtro, dato, datos[2]);
                }
                else
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => CartelAviso(resultado.Error)));
                    }
                    else
                    {
                        CartelAviso(resultado.Error);
                    }
                }
            }

            foreach (Control control in formFLTextBox.Controls)
            {
                if (control is Panel panel)
                {
                    foreach (Control child in panel.Controls)
                    {
                        if (child is TextBox textBox)
                        {
                            string placeholderText = textBox.Text;
                            textBox.Clear();
                            textBox.Text = placeholderText; // Restaurar el placeholder??????????
                            textBox.ForeColor = Color.Black;
                        }
                    }
                }
            }
        }
    }

    private async void EliminarFila(object sender, DataGridViewCellEventArgs e, string filtro, string dato)
    {
        ViajeViewModel vvm = new ViajeViewModel();
        ViajeFleteViewModel vfvm = new ViajeFleteViewModel();
        CuentaCorrienteViewModel ccvm = new CuentaCorrienteViewModel();
        SueldoViewModel svm = new SueldoViewModel();

        // Verificar si la celda clickeada pertenece a la columna "Eliminar"
        if (e.ColumnIndex == cheq.Columns["Eliminar"]?.Index && e.RowIndex >= 0)
        {
            // Confirmar antes de eliminar (opcional)
            DialogResult resultado = MessageBox.Show("¿Desea eliminar esta fila?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                if (filtro == "Camion")
                {
                    string id = cheq.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                    var result = await vvm.EliminarAsync(int.Parse(id));

                    if (result.IsSuccess)
                    {
                        ShowInfoTable(filtro, dato, " ");
                    }
                    else
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() => CartelAviso(result.Error)));
                        }
                        else
                        {
                            CartelAviso(result.Error);
                        }
                    }
                }
                else if (filtro == "Cliente")
                {
                    string id = cheq.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                    var result = await vvm.EliminarAsync(int.Parse(id));

                    if (result.IsSuccess)
                    {
                        ShowInfoTable(filtro, dato, " ");
                    }
                    else
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() => CartelAviso(result.Error)));
                        }
                        else
                        {
                            CartelAviso(result.Error);
                        }
                    }
                }
                else if (filtro == "Flete")
                {
                    string id = cheq.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                    var result = await vfvm.EliminarAsync(int.Parse(id));

                    if (result.IsSuccess)
                    {
                        ShowInfoTable(filtro, dato, " ");
                    }
                    else
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() => CartelAviso(result.Error)));
                        }
                        else
                        {
                            CartelAviso(result.Error);
                        }
                    }
                }
                else if (filtro == "cuenta corriente")
                {
                    //COMPARA CON UN CLIENTE
                    string id = cheq.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                    var result = await ccvm.EliminarAsync(int.Parse(id));

                    if (result.IsSuccess)
                    {
                        ShowInfoTable(filtro, dato, " ");
                    }
                    else
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() => CartelAviso(result.Error)));
                        }
                        else
                        {
                            CartelAviso(result.Error);
                        }
                    }
                }
                else if (filtro == "sueldo")
                {
                    string id = cheq.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                    var result = await svm.EliminarAsync(int.Parse(id));
                    MessageBox.Show(id);

                    if (result.IsSuccess)
                    {
                        ShowInfoTable(filtro, dato, " ");
                    }
                    else
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() => CartelAviso(result.Error)));
                        }
                        else
                        {
                            CartelAviso(result.Error);
                        }
                    }
                }
            }
        }
    }
    private async Task ModificarFilaAsync(object sender, DataGridViewCellEventArgs e, string dato, string filtro)
    {
        if (filtro != "sueldo")
        {
            ViajeViewModel vvm = new ViajeViewModel();
            CuentaCorrienteViewModel ccvm = new CuentaCorrienteViewModel();
            ViajeFleteViewModel fvm = new ViajeFleteViewModel();
            SueldoViewModel svm = new SueldoViewModel();

            // Verificar si la celda clickeada pertenece a la columna "Modificar"
            if (e.ColumnIndex == cheq.Columns["Modificar"].Index && e.RowIndex >= 0)
            {
                // Confirmar antes de modificar (opcional)
                DialogResult resultado = MessageBox.Show("¿Desea modificar esta fila?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    if (filtro == "Camion")
                    {
                        // Obtener los valores de la fila seleccionada
                        string fecha = cheq.Rows[e.RowIndex].Cells["Fecha"].Value.ToString();
                        string origen = cheq.Rows[e.RowIndex].Cells["Origen"].Value.ToString();
                        string destino = cheq.Rows[e.RowIndex].Cells["Destino"].Value.ToString();
                        string remito = cheq.Rows[e.RowIndex].Cells["RTO o CPE"].Value.ToString();
                        string carga = cheq.Rows[e.RowIndex].Cells["Carga"].Value.ToString();
                        string km = cheq.Rows[e.RowIndex].Cells["Km"].Value.ToString();
                        string kg = cheq.Rows[e.RowIndex].Cells["Kg"].Value.ToString();
                        string tarifa = cheq.Rows[e.RowIndex].Cells["Tarifa"].Value.ToString();
                        string chofer = cheq.Rows[e.RowIndex].Cells["Chofer"].Value.ToString();
                        string cliente = cheq.Rows[e.RowIndex].Cells["Cliente"].Value.ToString();
                        string porcentaje = cheq.Rows[e.RowIndex].Cells["Porcentaje"].Value.ToString();
                        string id = cheq.Rows[e.RowIndex].Cells["Id"].Value.ToString();

                        var result = await vvm.ActualizarAsync(int.Parse(id), DateOnly.Parse(fecha), origen, destino, int.Parse(remito), carga, int.Parse(kg), null, dato, float.Parse(km), float.Parse(tarifa), chofer, float.Parse(porcentaje));

                        if (result.IsSuccess)
                        {
                            ShowInfoTable(filtro, dato, " ");
                        }
                        else
                        {
                            if (this.InvokeRequired)
                            {
                                this.Invoke(new Action(() => CartelAviso(result.Error)));
                            }
                            else
                            {
                                CartelAviso(result.Error);
                            }
                        }
                    }
                    else if (filtro == "cuenta corriente")
                    {
                        string fecha = cheq.Rows[e.RowIndex].Cells["Fecha"].Value.ToString();
                        string factura = cheq.Rows[e.RowIndex].Cells["Nro factura"].Value.ToString();
                        string pagado = cheq.Rows[e.RowIndex].Cells["Pagado"].Value.ToString();
                        string adeuda = cheq.Rows[e.RowIndex].Cells["Adeuda"].Value.ToString();
                        string id = cheq.Rows[e.RowIndex].Cells["Id"].Value.ToString();

                        var result = await ccvm.ActualizarAsync(int.Parse(id), DateOnly.Parse(fecha), int.Parse(factura), float.Parse(adeuda), float.Parse(pagado), dato, null);
                        if (result.IsSuccess)
                        {
                            if (this.InvokeRequired)
                            {
                                this.Invoke(new Action(() => CartelAviso("El registro ha sido modificado")));
                            }
                            else
                            {
                                CartelAviso("El registro ha sido modificado");
                            }

                            ShowInfoTable(filtro, dato, " ");
                        }
                        else
                        {
                            if (this.InvokeRequired)
                            {
                                this.Invoke(new Action(() => CartelAviso(result.Error)));
                            }
                            else
                            {
                                CartelAviso(result.Error);
                            }
                        }
                    }
                    else if (filtro == "Flete")
                    {
                        MessageBox.Show("flete");
                        string fecha = cheq.Rows[e.RowIndex].Cells["Fecha"].Value.ToString();
                        string origen = cheq.Rows[e.RowIndex].Cells["Origen"].Value.ToString();
                        string destino = cheq.Rows[e.RowIndex].Cells["Destino"].Value.ToString();
                        string remito = cheq.Rows[e.RowIndex].Cells["RTO o CPE"].Value.ToString();
                        string carga = cheq.Rows[e.RowIndex].Cells["Carga"].Value.ToString();
                        string km = cheq.Rows[e.RowIndex].Cells["Km"].Value.ToString();
                        string kg = cheq.Rows[e.RowIndex].Cells["Kg"].Value.ToString();
                        string tarifa = cheq.Rows[e.RowIndex].Cells["Tarifa"].Value.ToString();
                        string factura = cheq.Rows[e.RowIndex].Cells["Factura"].Value.ToString();
                        string cliente = cheq.Rows[e.RowIndex].Cells["Cliente"].Value.ToString();
                        string porcentaje = cheq.Rows[e.RowIndex].Cells["Cliente"].Value.ToString();
                        string chofer = cheq.Rows[e.RowIndex].Cells["Chofer"].Value.ToString();
                        string comision = cheq.Rows[e.RowIndex].Cells["Comisión"].Value.ToString();
                        string id = cheq.Rows[e.RowIndex].Cells["Id"].Value.ToString();

                        var result = await fvm.ActualizarAsync(int.Parse(id), origen, destino, float.Parse(remito), carga, float.Parse(km), float.Parse(kg), float.Parse(tarifa), int.Parse(factura), cliente, chofer, float.Parse(comision), DateOnly.Parse(fecha));

                        if (result.IsSuccess)
                        {
                            ShowInfoTable(filtro, dato, " ");
                        }
                        else
                        {
                            if (this.InvokeRequired)
                            {
                                this.Invoke(new Action(() => CartelAviso(result.Error)));
                            }
                            else
                            {
                                CartelAviso(result.Error);
                            }
                        }
                    }
                }
            }
        }
    }

    private async void MarcarComoPagado(object sender, DataGridViewCellEventArgs e)
    {
        SueldoViewModel svm = new SueldoViewModel();
        var cellValue = cheq.Rows[e.RowIndex].Cells["Id"].Value;
        string id = cellValue != null ? cellValue.ToString() : string.Empty;

        if (e.ColumnIndex == cheq.Columns["Pagado"].Index && e.RowIndex >= 0)
        {
            // Confirmar antes de modificar (opcional)
            DialogResult resultado = MessageBox.Show("¿Desea marcar cómo pagado este sueldo?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                var result = await svm.marcarPago(int.Parse(id));
                if (result.IsSuccess)
                {
                    cheq.CurrentRow.DefaultCellStyle.BackColor = Color.Green;
                }
                else
                {
                    CartelAviso(result.Error);
                }
            }
        }
    }

    private void AddControls()
    {
        this.Controls.Add(formPanel);
        formPanel.Controls.Add(formFLTextBox);
    }

    //Button for back properties
    private void ButtonProperties(string filtro, string dato)
    {
        if (filtro != "cuenta corriente" && filtro != "sueldo")
        {
            btnVolver.Text = "Volver";
            btnVolver.Size = new Size(130, 40);
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.Location = new Point(20, 120);
            btnVolver.Font = new Font("Nunito", 16, FontStyle.Regular);
            btnVolver.BackColor = System.Drawing.Color.FromArgb(218, 218, 28);
            btnVolver.ForeColor = Color.Black;
            btnVolver.Cursor = Cursors.Hand;

            btnVolver.Click += (s, e) =>
            {
                this.Hide();
                Viaje vv = new Viaje(filtro);
                vv.TopLevel = true;
                vv.ShowDialog();
            };
            this.Controls.Add(btnVolver);
        }
    }

    //Label
    private void LabelProperties(string dato)
    {
        nombre.Text = dato;
        nombre.Text = nombre.Text.ToUpper();
        nombre.Font = new Font("Nunito", 20, FontStyle.Bold);
        nombre.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
        nombre.AutoSize = true;
        nombre.Location = new Point(120, 280);
        nombre.BackColor = Color.Transparent;

        this.Controls.Add(nombre);
    }

    private void AddButtonCuentaCorriente(string filtro, string dato)
    {
        if (filtro == "Cliente" || filtro == "Flete")
        {
            btnCuentaCorriente.Text = "Cuenta corriente";
            btnCuentaCorriente.Size = new Size(180, 40);
            btnCuentaCorriente.FlatAppearance.BorderSize = 0;
            btnCuentaCorriente.FlatStyle = FlatStyle.Flat;
            btnCuentaCorriente.Location = new Point(20, 200);
            btnCuentaCorriente.Font = new Font("Nunito", 16, FontStyle.Regular);
            btnCuentaCorriente.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
            btnCuentaCorriente.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
            btnCuentaCorriente.Cursor = Cursors.Hand;

            btnCuentaCorriente.Click += (s, e) =>
            {
                this.Hide();
                CuentaCorriente cuentaCorriente = new CuentaCorriente(dato, filtro);
                cuentaCorriente.TopLevel = true;
                cuentaCorriente.ShowDialog();
            };
        }

        this.Controls.Add(btnCuentaCorriente);
    }

    private void AddButtonSueldoMensual(string filtro, string dato, string nombreChofer)
    {
        if (filtro == "Camion")
        {
            btnSueldoMensual.Text = "Ver sueldo mensual";
            btnSueldoMensual.Size = new Size(180, 40);
            btnSueldoMensual.FlatAppearance.BorderSize = 0;
            btnSueldoMensual.FlatStyle = FlatStyle.Flat;
            btnSueldoMensual.Location = new Point(20, 200);
            btnSueldoMensual.Font = new Font("Nunito", 16, FontStyle.Regular);
            btnSueldoMensual.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
            btnSueldoMensual.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
            btnSueldoMensual.Cursor = Cursors.Hand;
            btnSueldoMensual.Click += (s, e) =>
            {
                this.Hide();
                SueldoMensual sueldo = new SueldoMensual(dato, filtro, nombreChofer);
                sueldo.TopLevel = true;
                sueldo.ShowDialog();
            };
        }

        this.Controls.Add(btnSueldoMensual);
    }

    private void ButtonsPropertiesForm(string filtro)
    {
        btnCargar.Anchor = AnchorStyles.Right | AnchorStyles.Top;
        btnCargar.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
        btnCargar.Size = new Size(110, 30);
        btnCargar.Text = "Cargar";
        btnCargar.FlatStyle = FlatStyle.Flat;
        btnCargar.FlatAppearance.BorderSize = 0;
        btnCargar.ForeColor = Color.White;
        btnCargar.Font = new Font("Nunito", 12, FontStyle.Bold);
        btnCargar.Cursor = Cursors.Hand;

        int marginRight = 20;


        if (!this.Controls.Contains(btnCargar))
        {
            this.Controls.Add(btnCargar);
        }

        if (filtro == "cuenta corriente" || filtro == "sueldo")
        {
            this.Resize += (s, e) =>
            {
                btnCargar.Location = new Point((this.ClientSize.Width / 2) + formPanel.Width - marginRight, 125);
            };
        }
        else
        {
            this.Resize += (s, e) =>
            {
                btnCargar.Location = new Point(this.ClientSize.Width - btnCargar.Width - marginRight, 125);
            };
        }
    }

    private void CartelAviso(string mensaje)
    {
        avisoPanel.Controls.Clear();
        ButtonAceptProperties();
        this.Controls.Add(avisoPanel);

        Label ll = new Label();
        ll.Text = mensaje;
        ll.Font = new Font("Nunito", 14, FontStyle.Regular);
        ll.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
        ll.AutoSize = true;

        // Centrar el label dentro del panel
        ll.Resize += (s, e) =>
        {
            ll.Location = new Point(
            (avisoPanel.Width - ll.Width) / 2, 50);
        };

        avisoPanel.BringToFront();  // Traer al frente
        avisoPanel.Visible = true;  // Asegurar que sea visible


        avisoPanel.Size = new Size(300, 150);
        avisoPanel.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);

        avisoPanel.Controls.Add(ll);
        avisoPanel.Controls.Add(btnAceptarAviso);

        // Centrar el panel en el formulario
        avisoPanel.Location = new Point(
            (this.ClientSize.Width - avisoPanel.Width) / 2,
            (this.ClientSize.Height - avisoPanel.Height) / 2
        );


        // Asegurar centrado dinámico si el formulario se redimensiona
        this.Resize += (s, e) =>
        {
            avisoPanel.Location = new Point(
                (this.ClientSize.Width - avisoPanel.Width) / 2,
                (this.ClientSize.Height - avisoPanel.Height) / 2
            );
        };
    }

    private void ButtonAceptProperties()
    {
        avisoPanel.Visible = true;

        btnAceptarAviso.BackColor = System.Drawing.Color.FromArgb(218, 218, 28);
        btnAceptarAviso.ForeColor = System.Drawing.Color.FromArgb(48, 48, 48);
        btnAceptarAviso.Size = new Size(110, 30);

        btnAceptarAviso.Text = "Aceptar";
        btnAceptarAviso.FlatStyle = FlatStyle.Flat;
        btnAceptarAviso.FlatAppearance.BorderSize = 0;
        btnAceptarAviso.Font = new Font("Nunito", 12, FontStyle.Bold);
        btnAceptarAviso.Cursor = Cursors.Hand;

        //btnAceptarAviso.Resize += (s, e) =>
        //{
        btnAceptarAviso.Location = new Point(avisoPanel.Width / 2, 100);
        //};

        btnAceptarAviso.Click += (s, e) =>
        {
            avisoPanel.Visible = false;
        };
    }
}
