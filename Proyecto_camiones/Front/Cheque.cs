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

namespace Proyecto_camiones.Front
{
    public partial class Cheque : Home
    {
        //Form
        private Panel formPanel = new Panel();
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



        //Constructor
        public Cheque()
        {
            ResaltarBoton(chequesMenu);

            InitializeUI();

            //ShowForm
            CargarFormularioCheque(9);

            //Hovers
            btnCargar.MouseEnter += (s, e) => HoverEffect(s, e, true);
            btnCargar.MouseLeave += (s, e) => HoverEffect(s, e, false);

            //Events
            btnCargar.Click += cargaClickEvent;

            cheq.CellClick += eliminarFila;
            cheq.CellClick += modificarFila;

            ConfigurarDataGridView();

            PositionGrid();
        }

        private void ConfigurarDataGridView()
        {
            if (cheq.Columns["Eliminar"] == null)
            {
                DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
                btnEliminar.Name = "Eliminar";
                btnEliminar.HeaderText = "X";  // Puedes dejarlo vacío si prefieres
                btnEliminar.Text = "x"; // Ícono de eliminar
                btnEliminar.UseColumnTextForButtonValue = true; // Hace que todas las celdas muestren "❌"
                btnEliminar.Width = 40; // Ajustar tamaño
                cheq.Columns.Add(btnEliminar);
            }

            if (cheq.Columns["Modificar"] == null)
            {
                DataGridViewButtonColumn btnModificar = new DataGridViewButtonColumn();
                btnModificar.Name = "Modificar";
                btnModificar.HeaderText = "X";  // Puedes dejarlo vacío si prefieres
                btnModificar.Text = "x"; // Ícono de modificar
                btnModificar.UseColumnTextForButtonValue = true; // Hace que todas las celdas muestren "M"
                btnModificar.Width = 40; // Ajustar tamaño
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
            cheq.Columns.Add("nombre", "Nombre");
            cheq.Columns.Add("nroPersonal", "Número personal de cheque");
            cheq.Columns.Add("entregadoA", "Entregado a");
            cheq.Columns.Add("fechaRetiro", "Fecha de retiro");
            cheq.Columns.Add("eliminar", "Eliminar");
            cheq.Columns.Add("modificar", "Modificar");

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
            this.campos = new List<string> { "F. Recibido", "Banco", "Nro. Cheque", "Pesos", "Nombre", "Nro. Personal", "Entregado a", "Fecha de retiro" };

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
            filterTextBox.PlaceholderText = "Buscar por Nro. Cheque...";
            filterTextBox.Margin = new Padding(5, 10, 5, 10);

            filterBtn.Text = "🔍";
            filterBtn.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
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
                }
            };
        }


        //FormProperties
        private void FormProperties(int cant)
        {
            formPanel.Size = new Size(100 * cant, 80);
            formPanel.AutoScroll = true;
            formPanel.HorizontalScroll.Enabled = true;
            formPanel.HorizontalScroll.Visible = true;
            formPanel.VerticalScroll.Enabled = false;
            formPanel.VerticalScroll.Visible = false;

            this.Resize += (s, e) =>
            {
                formPanel.Location = new Point((this.Width - formPanel.Width) / 2, 100);
            };

            formPanel.BackColor = System.Drawing.Color.FromArgb(200, Color.Black);
        }
        private void LayoutFormProperties(int cant)
        {
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

            formFL.FlowDirection = FlowDirection.LeftToRight;
            formFL.WrapContents = false;
            formFL.BackColor = Color.Transparent;

            return formFL;
        }

        //TextBoxProperties

        //TextBoxProperties
        private void TextoBoxAndLabelProperties(int cant, List<string> campos)
        {
            foreach (string campo in campos)
            {
                Panel campoPanel = PropertiesFormPanel();

                TextBox textBoxForm = CreateTextBoxAndProperties(campo);
                Label labelForm = CreateLabelAndProperties(campo);

                formFLLabel.Controls.Add(labelForm);

                campoPanel.Controls.Add(textBoxForm);
                formFLTextBox.Controls.Add(campoPanel);
            }
        }



        private Panel PropertiesFormPanel()
        {
            Panel campoTextBox = new Panel();
            campoTextBox.Size = new Size(105, 30);
            campoTextBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //campoTextBox.Dock = DockStyle.Top;


            return campoTextBox;
        }

        private Label CreateLabelAndProperties(object campo)
        {
            Label ll = new Label();

            ll.Text = campo.ToString();
            ll.Font = new Font("Nunito", 12, FontStyle.Regular);
            ll.ForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
            ll.BackColor = Color.Transparent;
            ll.Margin = new Padding(10, 0, 0, 0);
            ll.TextAlign = ContentAlignment.MiddleCenter;

            return ll;
        }
        private TextBox CreateTextBoxAndProperties(object campo)
        {
            TextBox textBoxCampo = new TextBox();
            textBoxCampo.Font = new Font("Nunito", 10, FontStyle.Regular);
            textBoxCampo.BackColor = System.Drawing.Color.FromArgb(153, 145, 145);
            textBoxCampo.Multiline = true;
            textBoxCampo.Width = 200;
            textBoxCampo.Height = 20;
            textBoxCampo.MinimumSize = new Size(200, 40);
            textBoxCampo.BorderStyle = BorderStyle.FixedSingle;
            textBoxCampo.Margin = new Padding(0, 0, 0, 20);
            textBoxCampo.ForeColor = System.Drawing.Color.Gray;
            textBoxCampo.TextAlign = HorizontalAlignment.Left;
            textBoxCampo.ForeColor = System.Drawing.Color.FromArgb(81, 77, 77);

            string placeholderDefault = !string.IsNullOrWhiteSpace(campo?.ToString()) ? campo.ToString() : "Placeholder";

            //PlaceHolersProperties
            string placeholderText = campo.ToString();
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

            textBoxCampo.SizeChanged += (s, e) =>
            {
                textBoxCampo.Height = 40;
            };

            return textBoxCampo;
        }



        //ButtonProperties
        private void PanelButtonProperties()
        {
            this.Resize += (s, e) =>
            {
                btnPanel.Location = new Point((this.Width - btnPanel.Width) - 50, 110);

                PositionGrid();
            };

            btnPanel.Size = new Size(110, 30);
            btnPanel.BackColor = Color.Transparent;
            this.Controls.Add(btnPanel);
        }
        private void ButtonsPropertiesForm()
        {
            btnCargar.BackColor = System.Drawing.Color.FromArgb(218, 218, 28);
            btnCargar.Size = new Size(110, 30);
            btnCargar.Text = "Cargar";
            btnCargar.FlatStyle = FlatStyle.Flat;
            btnCargar.FlatAppearance.BorderSize = 0;
            btnCargar.ForeColor = Color.Black;
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
        private void cargaClickEvent(object sender, EventArgs e)
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
                                    if (campo == "Fecha")
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
                            }

                            datos.Add(textBox.Text); // Agregar el texto de cada TextBox
                        }
                    }
                }
            }
            ChequeViewModel cvm = new ChequeViewModel();
            //cvm.CrearAsync(1, DateOnly.Parse(datos[0]), int.Parse(datos[1]), float.Parse(datos[2]), datos[3], DateOnly.Parse(datos[4]), DateOnly.Parse(datos[5]), datos[6], DateOnly.Parse(datos[7]));

            //var result = cvm.CrearAsync(1, DateOnly.Parse(datos[0]), int.Parse(datos[1]),);
            // Verificar que los datos no estén vacíos
            //if (datos.All(dato => !string.IsNullOrWhiteSpace(dato)))
            //{

            //    eliminar.Text = "X";
            //    eliminar.UseColumnTextForButtonValue = true;

            //    datos.Add(eliminar.Text);

            //    modificar.Text = "M";
            //    modificar.UseColumnTextForButtonValue = true;

            //    datos.Add(modificar.Text);

            //    cheq.Rows.Add(datos.ToArray());


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
            //}
        }

        private void eliminarFila(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar si la celda clickeada pertenece a la columna "Eliminar"
            if (e.ColumnIndex == cheq.Columns["Eliminar"].Index && e.RowIndex >= 0)
            {
                // Confirmar antes de eliminar (opcional)
                DialogResult resultado = MessageBox.Show("¿Desea eliminar esta fila?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    cheq.Rows.RemoveAt(e.RowIndex); //funcionEliminar
                }

            }
        }

        private void modificarFila(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar si la celda clickeada pertenece a la columna "Modificar"
            if (e.ColumnIndex == cheq.Columns["Modificar"].Index && e.RowIndex >= 0)
            {
                // Confirmar antes de modificar (opcional)
                DialogResult resultado = MessageBox.Show("¿Desea modificar esta fila?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    //funcionModificar
                }

            }
        }
    }
}