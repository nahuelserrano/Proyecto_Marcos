﻿using System;
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


    //Constructor
    public FormRegistro(List<string> camposForm, int cant, string dato, string filtro, List<string> camposFaltantesTablas)
    {
        InitializeUI(camposForm, cant, filtro, camposFaltantesTablas, dato);

        //ShowForm
        CargarFormulario(camposForm, cant, filtro);

        //Hovers
        btnCargar.MouseEnter += (s, e) => HoverEffect(s, e, true);
        btnCargar.MouseLeave += (s, e) => HoverEffect(s, e, false);

        //Events
        btnCargar.Click += (s, e) => cargaClickEvent(s, e, filtro, dato);

        cheq.CellClick += EliminarFila;
        cheq.CellClick += ModificarFila;

        if (filtro == "sueldo")
        {
            cheq.CellClick += MarcarComoPagado;
        }

        ConfigurarDataGridView(filtro);

        LabelProperties(dato);

        AddButtonCuentaCorriente(filtro, dato);
        AddButtonSueldoMensual(filtro, dato);

        this.TopLevel = false;
        this.FormBorderStyle = FormBorderStyle.None;
        this.Dock = DockStyle.Fill; // (opcional, para ocupar todo el espacio disponible)
    }

    //Initializations
    private void InitializeUI(List<string> camposForm, int cant, string filtro, List<string> camposFaltantesTablas, string dato)
    {
        this.AutoScaleMode = AutoScaleMode.Dpi;
        this.AutoSize = true;
        this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        AddItemsToGrid(camposForm, cant, camposFaltantesTablas, filtro);
        ResaltarBoton(viajesMenu);
        GridChequesProperties();
        ButtonProperties(filtro, dato);
        ShowInfoTable(filtro, dato);
    }

    private async void ShowInfoTable(string filtro, string dato)
    {
        cheq.Rows.Clear();

        if (filtro == "Camion")
        {
            ViajeViewModel vvm = new ViajeViewModel();
            var result = await vvm.ObtenerPorCamionAsync(dato);
            float montoChofer;

            if (result.IsSuccess)
            {
                foreach (var viaje in result.Value)
                {

                    montoChofer = viaje.PorcentajeChofer * viaje.Total;
                    cheq.Rows.Add(viaje.FechaInicio, viaje.LugarPartida, viaje.Destino, viaje.Remito, viaje.Carga, viaje.Km, viaje.Kg, viaje.Tarifa, viaje.PorcentajeChofer, viaje.NombreChofer, viaje.NombreCliente, viaje.Total, montoChofer);

                }
            }

            else
            {
                MessageBox.Show("Error al cargar el viaje");
            }
        }
        else if (filtro == "Cliente")
        {
            ClienteViewModel cvm = new ClienteViewModel();
            var resultClient = await cvm.ObtenerViajesDeUnCliente(dato);

            if (resultClient.IsSuccess)
            {
                foreach (var cliente in resultClient.Value)
                {
                    cheq.Rows.Add(cliente.Fecha_salida, cliente.Origen, cliente.Destino, cliente.Remito, cliente.Carga, cliente.Km, cliente.Kg, cliente.Tarifa, cliente.Nombre_chofer, cliente.Camion, cliente.Fletero, cliente.Total);
                }
            }

            else
            {
                MessageBox.Show(resultClient.Error);
                MessageBox.Show("Error al cargar cliente");
            }
        }
        else if (filtro == "Flete")
        {
            ViajeFleteViewModel fvm = new ViajeFleteViewModel();
            var resultFlete = await fvm.ObtenerViajesDeUnFleteroAsync(dato);

            if (resultFlete.IsSuccess)
            {
                foreach (var flete in resultFlete.Value)
                {
                    cheq.Rows.Add(flete.fecha_salida, flete.origen, flete.destino, flete.remito, flete.carga, flete.km, flete.kg, flete.tarifa, flete.factura, flete.comision, flete.cliente);
                }
            }

            else
            {
                MessageBox.Show(resultFlete.Error);
                MessageBox.Show("Error al cargar el flete");
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
        this.campos.Clear();
        foreach (string i in camposForm)
        {
            this.campos.Add(i);
        }
        InitializeFormProperties(cant, campos, filtro);
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
            formPanel.Size = new Size(this.Width - btnVolver.Width - btnCargar.Width - 120, 80);
            formPanel.AutoScroll = true;
            formPanel.HorizontalScroll.Enabled = true;
            formPanel.HorizontalScroll.Visible = true;
            formPanel.VerticalScroll.Enabled = false;
            formPanel.VerticalScroll.Visible = false;
        }
        else
        {
            formPanel.AutoScroll = false;
            formPanel.Size = new Size(110 * cant, 80);
        }
        this.Resize += (s, e) =>
        {
            formPanel.Location = new Point((this.Width - formPanel.Width) / 2 + 25, 100);
        };
        formPanel.BackColor = System.Drawing.Color.FromArgb(200, Color.Black);

    }
    private void LayoutFormProperties(int cant)
    {
        formFLTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        formFLTextBox = PropertiesLayoutForm();
        formFLLabel = PropertiesLayoutForm();

        formFLTextBox.Size = new Size(formPanel.Width, 50);
        formFLTextBox.Dock = DockStyle.Bottom;

        formFLLabel.Size = new Size(formPanel.Width, 25);

        formPanel.Controls.Add(formFLLabel);
        formPanel.Controls.Add(formFLTextBox);
    }

    private FlowLayoutPanel PropertiesLayoutForm()
    {
        FlowLayoutPanel formFL = new FlowLayoutPanel();

        formFL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        formFL.FlowDirection = FlowDirection.LeftToRight;
        formFL.WrapContents = false;
        formFL.BackColor = Color.Transparent;

        return formFL;
    }

    //TextBoxProperties
    private void TextoBoxAndLabelProperties(int cant, List<string> campos)
    {
        foreach (string campo in campos)
        {
            Panel campoPanel = PropertiesFormPanel();

            TextBox textBoxForm = CreateTextBoxAndProperties(campo, cant);
            Label labelForm = CreateLabelAndProperties(campo, cant);

            formFLLabel.Controls.Add(labelForm);

            campoPanel.Controls.Add(textBoxForm);
            formFLTextBox.Controls.Add(campoPanel);
        }
    }

    private Panel PropertiesFormPanel()
    {
        Panel campoTextBox = new Panel();
        campoTextBox.AutoSize = true;
        campoTextBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        return campoTextBox;
    }

    private Label CreateLabelAndProperties(object campo, int cant)
    {
        Label ll = new Label();

        ll.Text = campo.ToString();
        ll.Font = new Font("Nunito", 12, FontStyle.Regular);
        ll.ForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
        ll.BackColor = Color.Transparent;
        ll.Margin = new Padding(10, 0, 0, 0);
        ll.TextAlign = ContentAlignment.MiddleLeft;

        ll.AutoSize = false; // Muy importante: si no, el Size no se aplica
        ll.Size = new Size((formPanel.Width / cant) - 10, 20); // Ajustá el tamaño como quieras

        return ll;
    }
    private TextBox CreateTextBoxAndProperties(object campo, int cant)
    {
        TextBox textBoxCampo = new TextBox();
        textBoxCampo.Font = new Font("Nunito", 12, FontStyle.Regular);
        textBoxCampo.BackColor = System.Drawing.Color.FromArgb(153, 145, 145);
        textBoxCampo.Multiline = true;
        textBoxCampo.Width = (formPanel.Width / cant) - 10;
        textBoxCampo.Height = 30;
        textBoxCampo.BorderStyle = BorderStyle.FixedSingle;
        textBoxCampo.TextAlign = HorizontalAlignment.Left;
        textBoxCampo.ForeColor = System.Drawing.Color.FromArgb(81, 77, 77);

        string placeholderDefault = !string.IsNullOrWhiteSpace(campo?.ToString()) ? campo.ToString() : "Placeholder";

        string placeholderText = campo.ToString();
        textBoxCampo.Name = placeholderText;
        textBoxCampo.Text = placeholderText;

        textBoxCampo.GotFocus += (s, e) =>
        {
            if (textBoxCampo.Text == placeholderText)
            {
                textBoxCampo.Text = "";
                textBoxCampo.ForeColor = Color.Black;
            }
        };

        textBoxCampo.LostFocus += (s, e) =>
        {
            if (string.IsNullOrWhiteSpace(textBoxCampo.Text))
            {
                textBoxCampo.Text = placeholderText;
                textBoxCampo.ForeColor = System.Drawing.Color.FromArgb(81, 77, 77);
            }
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
                btnPagado.Text = "✔️"; // Ícono de modificar
                btnPagado.UseColumnTextForButtonValue = true; // Hace que todas las celdas muestren "✔️"

                cheq.Columns.Add(btnPagado);
            }
        }
        if (cheq.Columns["Eliminar"] == null)
        {
            DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
            btnEliminar.Name = "Eliminar";
            btnEliminar.HeaderText = "Eliminar";  // Puedes dejarlo vacío si prefieres
            btnEliminar.Text = "🗑️"; // Ícono de eliminar
            btnEliminar.UseColumnTextForButtonValue = true; // Hace que todas las celdas muestren "❌"
            btnEliminar.Width = 20; // Ajustar tamaño

            cheq.Columns.Add(btnEliminar);
        }

        if (cheq.Columns["Modificar"] == null)
        {
            DataGridViewButtonColumn btnModificar = new DataGridViewButtonColumn();
            btnModificar.Name = "Modificar";
            btnModificar.HeaderText = "Modificar";  // Puedes dejarlo vacío si prefieres
            btnModificar.Text = "✏️"; // Ícono de modificar
            btnModificar.UseColumnTextForButtonValue = true; // Hace que todas las celdas muestren "✏️"

            cheq.Columns.Add(btnModificar);
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
                            if (textBox.Text == campo.ToString())
                            {
                                MessageBox.Show("Complete todos los campos");
                                return;
                            }
                            if (textBox.Name == campo)
                            {
                                if (campo == "Fecha" || campo == "Fecha inicial" || campo == "Fecha final")
                                {
                                    TextBox campoFecha = textBox;
                                    DateTime fecha;
                                    if (!DateTime.TryParse(campoFecha.Text, out fecha))
                                    {
                                        MessageBox.Show("Por favor, ingrese una fecha válida.");
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
                                        MessageBox.Show("Por favor, ingrese un número válido.");
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

        if (filtro == "Camion")
        {
            ViajeViewModel viajeViewModel = new ViajeViewModel();
            var resultado = await viajeViewModel.CrearAsync(DateOnly.Parse(datos[0]), datos[1], datos[2], int.Parse(datos[3]), datos[4], float.Parse(datos[6]), datos[10], dato, float.Parse(datos[5]), float.Parse(datos[7]), datos[9], float.Parse(datos[8]));

            if (resultado.IsSuccess)
            {
                ShowInfoTable(filtro, dato);
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

    private void EliminarFila(object sender, DataGridViewCellEventArgs e)
    {
        // Verificar si la celda clickeada pertenece a la columna "Eliminar"
        if (e.ColumnIndex == cheq.Columns["Eliminar"].Index && e.RowIndex >= 0)
        {
            // Confirmar antes de eliminar (opcional)
            DialogResult resultado = MessageBox.Show("¿Desea eliminar esta fila?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                cheq.Rows.RemoveAt(e.RowIndex);
            }
        }
    }
    private void ModificarFila(object sender, DataGridViewCellEventArgs e)
    {
        ViajeViewModel vvm = new ViajeViewModel();
        // Verificar si la celda clickeada pertenece a la columna "Modificar"
        if (e.ColumnIndex == cheq.Columns["Modificar"].Index && e.RowIndex >= 0)
        {
            // Confirmar antes de modificar (opcional)
            DialogResult resultado = MessageBox.Show("¿Desea modificar esta fila?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                //Modificar
            }

        }
    }

    private void MarcarComoPagado(object sender, DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex == cheq.Columns["Pagado"].Index && e.RowIndex >= 0)
        {
            // Confirmar antes de modificar (opcional)
            DialogResult resultado = MessageBox.Show("¿Desea marcar cómo pagado este sueldo?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                cheq.CurrentRow.DefaultCellStyle.BackColor = Color.Green;
                //funcionPagado
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
            btnVolver.Size = new Size(140, 40);
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.Location = new Point(20, 120);
            btnVolver.Font = new Font("Nunito", 16, FontStyle.Regular);
            btnVolver.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
            btnVolver.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);

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

    private void AddButtonSueldoMensual(string filtro, string dato)
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
            btnSueldoMensual.Click += (s, e) =>
            {
                this.Hide();
                SueldoMensual sueldo = new SueldoMensual(dato, filtro);
                sueldo.TopLevel = true;
                sueldo.ShowDialog();
            };
        }

        this.Controls.Add(btnSueldoMensual);
    }

    private void ButtonsPropertiesForm(string filtro)
    {
        btnCargar.Anchor = AnchorStyles.Right | AnchorStyles.Top;


        btnCargar.BackColor = System.Drawing.Color.FromArgb(218, 218, 28);
        btnCargar.Size = new Size(110, 30);

        btnCargar.Text = "Cargar";
        btnCargar.FlatStyle = FlatStyle.Flat;
        btnCargar.FlatAppearance.BorderSize = 0;
        btnCargar.ForeColor = Color.Black;
        btnCargar.Font = new Font("Nunito", 12, FontStyle.Bold);

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
}