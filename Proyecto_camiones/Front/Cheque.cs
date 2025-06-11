using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Net.Mime.MediaTypeNames;
using Font = System.Drawing.Font;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;
using Button = System.Windows.Forms.Button;
using Proyecto_camiones.ViewModels;
using NPOI.SS.UserModel;

namespace Proyecto_camiones.Front
{
    public partial class Cheque : Home
    {
        //Form
        private NewRoundPanel formPanel = new NewRoundPanel(10);
        private FlowLayoutPanel formFLTextBox = new FlowLayoutPanel();
        private FlowLayoutPanel formFLLabel = new FlowLayoutPanel();

        private List<string> campos = new List<string>();


        //Button
        private Panel btnPanel = new Panel();
        private RoundButton btnCargar = new RoundButton();

        //Filter
        private Panel filterPanel = new Panel();
        private FlowLayoutPanel filterFL = new FlowLayoutPanel();

        private TextBox filterTextBox = new TextBox();
        private RoundButton filterBtn = new RoundButton();

        //Grid
        private DataGridView cheq = new DataGridView();
        private Panel panelGrid = new Panel();

        private DataGridViewButtonColumn eliminar = new DataGridViewButtonColumn();
        private DataGridViewButtonColumn modificar = new DataGridViewButtonColumn();

        private RoundButton btnMostrarTodos = new RoundButton();

        // ViewModel
        private readonly ChequeViewModel _chequeViewModel;

        //Constructor
        public Cheque()
        {
            _chequeViewModel = ViewModelFactory.CreateChequeViewModel();

            ShowInfoTable();
            ResaltarBoton(chequesMenu);

            InitializeUI();

            //ShowForm
            CargarFormularioCheque(10);

            //Hovers
            btnCargar.MouseEnter += (s, e) => HoverEffect(s, e, true);
            btnCargar.MouseLeave += (s, e) => HoverEffect(s, e, false);

            //Events
            btnCargar.Click += (s, e) => cargaClickEventAsync(s, e);

            cheq.CellClick += (s, e) => eliminarFila(s, e);
            cheq.CellClick += modificarFila;

            ConfigurarDataGridView();

            PositionGrid();
        }

        private async void ShowInfoTable()
        {
            cheq.Rows.Clear();
            var result = await _chequeViewModel.ObtenerTodosAsync();

            if (result.IsSuccess)
            {
                foreach (DataGridViewColumn col in cheq.Columns)
                {
                    if (col.HeaderText.Equals("Pesos")) // O el nombre correcto de tu columna de monto
                    {
                        col.DefaultCellStyle.Format = "N2";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }

                // SEGUNDO: Agregar las filas con los datos convertidos
                foreach (var cheque in result.Value)
                {
                    // Convertir el monto a decimal si viene como string
                    decimal pesosDecimal = 0;

                    if (cheque.Monto is float)
                    {
                        pesosDecimal = (decimal)cheque.Monto;
                    }
                    cheq.Rows.Add(cheque.FechaIngresoCheque, cheque.Banco, cheque.NumeroCheque, pesosDecimal, cheque.Nombre, cheque.NumeroPersonalizado, cheque.EntregadoA, cheque.FechaCobro, cheque.FechaVencimiento, cheque.Id);
                }
            }

        }

        private void ConfigurarDataGridView()
        {
            if (cheq.Columns["Eliminar"] == null)
            {
                DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
                btnEliminar.Name = "Eliminar";
                btnEliminar.HeaderText = "Eliminar";  // Puedes dejarlo vacío si prefieres
                btnEliminar.Text = "🗑️"; // Ícono de eliminar
                btnEliminar.UseColumnTextForButtonValue = true; // Hace que todas las celdas muestren "❌"
                cheq.Columns.Add(btnEliminar);
            }

            if (cheq.Columns["Modificar"] == null)
            {
                DataGridViewButtonColumn btnModificar = new DataGridViewButtonColumn();
                btnModificar.Name = "Modificar";
                btnModificar.HeaderText = "Modificar";  // Puedes dejarlo vacío si prefieres
                btnModificar.Text = "✏️"; // Ícono de modificar
                btnModificar.UseColumnTextForButtonValue = true; // Hace que todas las celdas muestren "M"
                cheq.Columns.Add(btnModificar);
            }
        }

        //Initializations
        private void InitializeUI()
        {
            AddItemsToGrid();
            GridChequesProperties();
            InitializeFilter();
            ButtonShowAllProperties();
        }

        private void ButtonShowAllProperties()
        {
            btnMostrarTodos.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
            btnMostrarTodos.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
            btnMostrarTodos.Size = new Size(150, 30);
            btnMostrarTodos.Text = "Mostrar todos";
            btnMostrarTodos.FlatStyle = FlatStyle.Flat;
            btnMostrarTodos.FlatAppearance.BorderSize = 0;
            btnMostrarTodos.Font = new Font("Nunito", 12, FontStyle.Bold);

            this.Resize += (s, e) =>
            {
                btnMostrarTodos.Location = new Point(50, 200);
            };

            btnMostrarTodos.Click += (s, e) =>
            {
                foreach (DataGridViewRow row in cheq.Rows)
                {
                    row.Visible = true;
                }
            };

            this.Controls.Add(btnMostrarTodos);
        }

        private void InitializeFormProperties(int cant, List<string> campos)
        {
            FormProperties(cant);
            LayoutFormProperties(cant);
            TextoBoxAndLabelProperties(cant, campos);
            ButtonsPropertiesForm();
            PanelButtonProperties();
            AddForm();
        }
        private void InitializeFilter()
        {
            AddFilter();
            filterProperties();
        }



        //Adds
        private void AddItemsToGrid()
        {
            cheq.Columns.Add("fRecibido", "F. Recibido");
            cheq.Columns.Add("banco", "Banco");
            cheq.Columns.Add("nroCheque", "Nro de cheque");
            cheq.Columns.Add("pesos", "Pesos");
            cheq.Columns.Add("nombre", "Recibido por");
            cheq.Columns.Add("nroPersonal", "Número personal de cheque");
            cheq.Columns.Add("entregadoA", "Entregado a");
            cheq.Columns.Add("fechaRetiro", "Fecha de retiro");
            cheq.Columns.Add("fechaVencimiento", "F. vencimiento");
            cheq.Columns.Add("id", "id");

            foreach (DataGridViewColumn col in cheq.Columns)
            {
                if (col.HeaderText.Equals("id"))
                {
                    col.Visible = false;
                }
            }

            panelGrid.Controls.Add(cheq);
            this.Controls.Add(panelGrid);

        }
 
        private void AddForm()
        {
            this.Controls.Add(formPanel);
        }
        private void AddFilter()
        {
            filterFL.Controls.Add(filterTextBox);
            filterFL.Controls.Add(filterBtn);

            filterPanel.Controls.Add(filterFL);

            this.Controls.Add(filterPanel);
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


        //FormInformation
        private void CargarFormularioCheque(int cant)
        {
            this.campos.Clear();
            this.campos = new List<string> { "F. Recibido", "Banco", "Nro. Cheque", "Pesos", "Recibido por", "Nro. Personal", "Entregado a", "Fecha de retiro", "F. vencimiento" };

            InitializeFormProperties(cant, campos);
        }


        //FilterProperties
        private void filterProperties()
        {

            filterPanel.Size = new Size(200, 50);
            filterPanel.BackColor = Color.Transparent;
            filterPanel.Location = new Point(cheq.Location.X + cheq.Width - (filterPanel.Width / 2), menuStrip.Height + formFLTextBox.Height + 30);

            filterFL.Dock = DockStyle.Fill;
            filterFL.FlowDirection = FlowDirection.LeftToRight;
            filterFL.WrapContents = false;
            filterFL.AutoSize = false;
            filterFL.Margin = new Padding(0);
            filterFL.Padding = new Padding(0);

            filterTextBox.Size = new Size(120, 30);
            filterTextBox.Font = new Font("Nunito", 10);
            filterTextBox.PlaceholderText = "Buscar";
            filterTextBox.Margin = new Padding(5, 10, 5, 10);

            filterBtn.Text = "🔍";
            filterBtn.ForeColor = System.Drawing.Color.FromArgb(201, 227, 247);
            filterBtn.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
            filterBtn.Size = new Size(30, 25);
            filterBtn.Margin = new Padding(0, 10, 5, 10);
            filterBtn.FlatStyle = FlatStyle.Flat;
            filterBtn.FlatAppearance.BorderColor = Color.FromArgb(48, 48, 48);
            filterBtn.FlatAppearance.BorderSize = 1;

            filterBtn.Click += (s, e) =>
            {
                foreach (DataGridViewRow row in cheq.Rows)
                {
                    if (row.Cells["nroPersonal"].Value != null && row.Cells["nroPersonal"].Value.ToString().ToLower().Contains(filterTextBox.Text.ToLower()))
                    {
                        row.Visible = true;
                    }
                    if (row.Cells["nroPersonal"].Value != null && !row.Cells["nroPersonal"].Value.ToString().ToLower().Contains(filterTextBox.Text.ToLower()))
                    {
                        row.Visible = false;
                    }
                    if (row.Cells["banco"].Value != null && row.Cells["banco"].Value.ToString().ToLower().Contains(filterTextBox.Text.ToLower()))
                    {
                        row.Visible = true;
                    }
                    if (row.Cells["banco"].Value != null && !row.Cells["banco"].Value.ToString().ToLower().Contains(filterTextBox.Text.ToLower()))
                    {
                        row.Visible = false;
                    }
                }
            };
        }


        //FormProperties
        private void FormProperties(int cant)
        {
            formPanel.Size = new Size(100 * cant, 70);
            formPanel.BackColor = Color.FromArgb(45, 45, 48); // Gris oscuro moderno
            formPanel.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.Resize += (s, e) =>
            {
                formPanel.Location = new Point((this.Width - formPanel.Width) / 2, 100);
            };
        }
        private void LayoutFormProperties(int cant)
        {
            formFLTextBox = PropertiesLayoutForm();
            formFLLabel = PropertiesLayoutForm();

            formFLTextBox.Size = new Size(formPanel.Width - 10, 55);
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
            formFL.Padding = new Padding(5, 0, 5, 0);
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

        //ButtonProperties
        private void PanelButtonProperties()
        {
            this.Resize += (s, e) =>
            {
                btnPanel.Location = new Point((this.Width - btnPanel.Width) - 120, 115);

                PositionGrid();
            };

            btnPanel.Size = new Size(110, 30);
            btnPanel.BackColor = Color.Transparent;
            this.Controls.Add(btnPanel);
        }
        private void ButtonsPropertiesForm()
        {
            btnCargar.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
            btnCargar.Size = new Size(110, 30);
            btnCargar.Text = "Cargar";
            btnCargar.FlatStyle = FlatStyle.Flat;
            btnCargar.FlatAppearance.BorderSize = 0;
            btnCargar.ForeColor = Color.White;
            btnCargar.Font = new Font("Nunito", 12, FontStyle.Bold);

            if (!btnPanel.Controls.Contains(btnCargar))
            {
                btnPanel.Controls.Add(btnCargar);
            }

            btnPanel.Resize += (s, e) =>
            {
                btnCargar.Location = new Point((btnPanel.Width - btnCargar.Width) / 2, (btnPanel.Height - btnCargar.Height) / 2);

                PositionGrid();
            };
            btnCargar.Cursor = Cursors.Hand;
        }

        //GridProperties
        private void GridChequesProperties()
        {
            panelGrid.Size = new Size(1200, 400);
            panelGrid.BackColor = Color.Transparent;

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

        private void PositionGrid()
        {
            panelGrid.Location = new Point((this.Width - panelGrid.Width) / 2, 250);

        }


        //Otros
        //CargaDeDatos
        private async Task cargaClickEventAsync(object sender, EventArgs e)
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
                                    if (campo == "fRecibido" || campo == "fechaRetiro" || campo == "fechaVencimiento")
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
                                }
                                if (textBox.Text == campo.ToString())
                                {
                                    datos.Add(" ");
                                }
                            }

                            datos.Add(textBox.Text); // Agregar el texto de cada TextBox
                        }
                    }
                }
            }
            var result = await _chequeViewModel.CrearAsync(DateOnly.Parse(datos[0]), int.Parse(datos[2]), float.Parse(datos[3]), datos[1], DateOnly.Parse(datos[7]), datos[4], int.Parse(datos[5]), DateOnly.Parse(datos[8]), datos[6]);

            if (result.IsSuccess)
            {
                LimpiarFormulario();
                ShowInfoTable();
            }

            //Verificar que los datos no estén vacíos
            if (datos.All(dato => !string.IsNullOrWhiteSpace(dato)))
            {

                eliminar.Text = "X";
                eliminar.UseColumnTextForButtonValue = true;

                datos.Add(eliminar.Text);

                modificar.Text = "M";
                modificar.UseColumnTextForButtonValue = true;

                datos.Add(modificar.Text);
            }
        }

        public void LimpiarFormulario()
        {
            foreach (Control control in formFLTextBox.Controls)
            {
                if (control is Panel panel && panel.Controls[0] is TextBox textBox)
                {
                    textBox.Text = "";
                    textBox.BackColor = Color.FromArgb(60, 60, 65);
                }
            }
        }

        private async Task eliminarFila(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar si la celda clickeada pertenece a la columna "Eliminar"
            if (e.ColumnIndex == cheq.Columns["Eliminar"].Index && e.RowIndex >= 0)
            {
                // Confirmar antes de eliminar (opcional)
                DialogResult resultado = MessageBox.Show("¿Desea eliminar esta fila?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    string id = cheq.Rows[e.RowIndex].Cells["id"].Value.ToString();
                   
                    var result = await _chequeViewModel.EliminarAsync(int.Parse(id));

                    if (result.IsSuccess)
                    {
                        ShowInfoTable();
                    }
                    else
                    {
                        MessageBox.Show(result.Error);
                    }
                }
            }
        }

        private async void modificarFila(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar si la celda clickeada pertenece a la columna "Modificar"
            if (e.ColumnIndex == cheq.Columns["Modificar"].Index && e.RowIndex >= 0)
            {
                // Confirmar antes de modificar (opcional)
                DialogResult resultado = MessageBox.Show("¿Desea modificar esta fila?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {

                    var idValue = cheq.Rows[e.RowIndex].Cells["id"].Value;
                    string id = idValue != null ? idValue.ToString() : string.Empty;

                    var recibidoValue = cheq.Rows[e.RowIndex].Cells["fRecibido"].Value;
                    string fechaRecibido = recibidoValue != null ? recibidoValue.ToString() : string.Empty;

                    var pesosValue = cheq.Rows[e.RowIndex].Cells["pesos"].Value;
                    string monto = pesosValue != null ? pesosValue.ToString() : string.Empty;

                    var bancoValue = cheq.Rows[e.RowIndex].Cells["banco"].Value;
                    string banco = bancoValue != null ? bancoValue.ToString() : string.Empty;

                    var chequeValue = cheq.Rows[e.RowIndex].Cells["nroCheque"].Value;
                    string nroCheque = chequeValue != null ? chequeValue.ToString() : string.Empty;

                    var personalValue = cheq.Rows[e.RowIndex].Cells["nroPersonal"].Value;
                    string nroPersonal = personalValue != null ? personalValue.ToString() : string.Empty;

                    var retiroValue = cheq.Rows[e.RowIndex].Cells["fechaRetiro"].Value;
                    string fechaRetiro = retiroValue != null ? retiroValue.ToString() : string.Empty;

                    var entragadoValue = cheq.Rows[e.RowIndex].Cells["entregadoA"].Value;
                    string entregadoA = entragadoValue != null ? entragadoValue.ToString() : string.Empty;

                    var nombreValue = cheq.Rows[e.RowIndex].Cells["nombre"].Value;
                    string entragadoPor = nombreValue != null ? nombreValue.ToString() : string.Empty;
                    
                    var vencimiento = cheq.Rows[e.RowIndex].Cells["fechaVencimiento"].Value;
                    string fechaVencimiento = vencimiento != null ? vencimiento.ToString() : string.Empty;

                    var result = await _chequeViewModel.ActualizarAsync(int.Parse(id), DateOnly.Parse(fechaRecibido), int.Parse(nroCheque), float.Parse(monto), banco, DateOnly.Parse(fechaRetiro), entragadoPor, int.Parse(nroPersonal), DateOnly.Parse(fechaVencimiento), entregadoA);

                    if (result.IsSuccess)
                    {
                        ShowInfoTable();
                    }
                    else
                    {
                        MessageBox.Show(result.Error);
                    }
                }
            }
        }
    }
}