using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Collections;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using Proyecto_camiones.ViewModels;
using Proyecto_camiones.Presentacion.Utils;
using Proyecto_camiones.Models;
using Proyecto_camiones.DTOs;
using NPOI.SS.Formula.Functions;
using Proyecto_camiones.Presentacion.Models;

namespace AplicacionCamiones.Front;

internal class Viaje : Home
{
    //Filter
    private NewRoundPanel filter = new NewRoundPanel(20);
    private FlowLayoutPanel filterFL = new FlowLayoutPanel();

    private ArrayList buttonsFilter = new ArrayList();
    private ArrayList buttonsNameFilter = new ArrayList();
    private RoundButton fleteFilter = new RoundButton();
    private RoundButton camionFilter = new RoundButton();
    private RoundButton clienteFilter = new RoundButton();

    private RoundButton buttonAddNew = new RoundButton();

    private ArrayList botonesRegistro = new ArrayList();
    private ArrayList nombreBotonesRegistro = new ArrayList();

    //Form
    private NewRoundPanel formCargarSection = new NewRoundPanel(40);
    private FlowLayoutPanel layourFormSection = new FlowLayoutPanel();
    private System.Windows.Forms.ComboBox select = new System.Windows.Forms.ComboBox();
    private System.Windows.Forms.TextBox textBoxNombre = new System.Windows.Forms.TextBox();
    private RoundButton buttonAcept = new RoundButton();
    private RoundButton buttonBack = new RoundButton();

    private List<string> camiones = new List<string>();
    private List<string> clientes = new List<string>();
    private List<string> fletes = new List<string>();

    private List<string> campos = new List<string>();

    private List<string> camposFaltantesTabla = new List<string>();


    private int cantCamposTabla = 0;


    //Card
    private Panel cardsContainer = new Panel();
    private FlowLayoutPanel cardsContainerFL = new FlowLayoutPanel();

    private CamionViewModel cvm = new CamionViewModel();



    //Constructor
    public Viaje()
    {
        //buttonAcept.Click += (s, e) => ButtonAcept_Click1(s, e);
        InitializeFilterCards();
        ResaltarBoton(viajesMenu);


        //Hovers
        fleteFilter.MouseEnter += (s, e) => HoverEffect(s, e, true);
        fleteFilter.MouseLeave += (s, e) => HoverEffect(s, e, false);
        //fleteFilter.Click += (s, e) => ClickEffects(s, e);

        clienteFilter.MouseEnter += (s, e) => HoverEffect(s, e, true);
        clienteFilter.MouseLeave += (s, e) => HoverEffect(s, e, false);
        //clienteFilter.Click += (s, e) => ClickEffects(s, e);

        camionFilter.MouseEnter += (s, e) => HoverEffect(s, e, true);
        camionFilter.MouseLeave += (s, e) => HoverEffect(s, e, false);
        //camionFilter.Click += (s, e) => ClickEffects(s, e);

        //Events
        fleteFilter.Click += (s, e) => ObtenerTodosSegunFiltro("Flete", " ");
        clienteFilter.Click += (s, e) => ObtenerTodosSegunFiltro("Cliente", " ");
        camionFilter.Click += (s, e) => ObtenerTodosSegunFiltro("Camion", " ");


    }

    public Viaje(string filtro)
    {
        InitializeFilterCards();
        ResaltarBoton(viajesMenu);
        if (filtro != null)
        {
            ObtenerTodosSegunFiltro(filtro, " ");
        }
    }

    //HoverFunction
    private void HoverEffect(object sender, EventArgs e, bool isHover)
    {
        var button = sender as RoundButton;
        if (button != null)
        {
            button.Font = new Font("Nunito", isHover ? 20 : 16, FontStyle.Regular);
            button.ForeColor = isHover ? Color.FromArgb(218, 218, 28) : Color.FromArgb(224, 224, 224);
        }
    }


    //Initializations

    private void InitializeFilterCards()
    {
        OptionsMenuProperties();
        LayoutOptionsMenuProperties();
        ButtonsProperties();
        CardProperties();
        CardFLProperties();
        AddItemsToFilter();
        ButtonNewAddProperties();
    }

    private void AddItemsToFilter()
    {
        this.Controls.Add(filter);
        this.Controls.Add(cardsContainer);
        this.cardsContainer.Controls.Add(cardsContainerFL);
        filter.Controls.Add(filterFL);
    }

    //FilterProperties
    private void OptionsMenuProperties()
    {
        filter.Size = new Size(800, 60);
        this.Resize += (s, e) =>
        {
            filter.Location = new Point((this.Width - filter.Width) / 2, 150);
        };
        filter.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
        filter.BorderStyle = BorderStyle.FixedSingle;
    }
    private void LayoutOptionsMenuProperties()
    {
        filterFL.AutoSize = true;
        filterFL.Width = fleteFilter.Width;
        filterFL.BackColor = Color.Transparent;
        filterFL.FlowDirection = FlowDirection.LeftToRight;
        filter.Resize += (s, e) =>
        {
            filterFL.Location = new Point((filter.Width - filterFL.Width) / 2, (filter.Height - filterFL.Height) / 2);
        };
    }

    private void AddButtonNewAdd()
    {
        cardsContainerFL.Controls.Add(buttonAddNew);
    }

    //InfoFunctions
    public async void ObtenerTodosSegunFiltro(string filtro, string info)
    {
        if (filtro == "Camion")
        {
            var resultado = await cvm.ObtenerTodosAsync();
            if (resultado.IsSuccess)
            {
                CardCamionGenerator("Camion", resultado);
            }
            else
            {
                MessageBox.Show("No se pudo obtener la lista de camiones");
            }
        }
        else if (filtro == "Cliente")
        {
            ClienteViewModel cvm = new ClienteViewModel();
            var resultado = await cvm.ObtenerTodosAsync();

            if (resultado.IsSuccess)
            {
                CardClienteGenerator("Cliente", resultado);
            }
            else
            {
                MessageBox.Show("No se pudo obtener la lista de camiones");
            }
        }
        else if (filtro == "Flete")
        {
            FleteViewModel fvm = new FleteViewModel();
            var resultado = await fvm.ObtenerTodosAsync();
            if (resultado.IsSuccess)
            {
                CardCamionGenerator("Flete", resultado);
            }
            else
            {
                MessageBox.Show("No se pudo obtener la lista de fletero");
            }
        }
    }

    private void CardCamionGenerator(string filtro, Result<List<Flete>> resultado)
    {
        cardsContainerFL.Controls.Clear();
        foreach (var item in resultado.Value)
        {
            GeneratorCard(item, filtro);
        }
        buttonsVisibleOrInvisible();
    }

    private void CardCamionGenerator(string filtro, Result<List<CamionDTO>> resultado)
    {
        cardsContainerFL.Controls.Clear();
        foreach (var item in resultado.Value)
        {
            GeneratorCard(item, filtro);
        }
        buttonsVisibleOrInvisible();
    }

    private void CardClienteGenerator(string filtro, Result<List<Cliente>> resultado)
    {
        cardsContainerFL.Controls.Clear();
        foreach (var item in resultado.Value)
        {
            GeneratorCard(item, filtro);
        }

        buttonsVisibleOrInvisible();
    }


    //GENERADOR DE CARDS

    private void GeneratorCard(Flete item, string filtro)
    {
        Panel card = new Panel
        {
            Size = new Size(200, 100),
            BackColor = System.Drawing.Color.FromArgb(48, 48, 48),
            Margin = new Padding(10),
            Font = new Font("Nunito", 16, FontStyle.Regular),
        };

        Label label = new Label
        {

            Text = item.nombre,
            ForeColor = System.Drawing.Color.FromArgb(218, 218, 28),
            AutoSize = true,
            TextAlign = ContentAlignment.TopCenter,
            Location = new Point(10, 10),
            BackColor = Color.Transparent
        };

        RoundButton remove = new RoundButton
        {
            Text = "🗑️",
            BackColor = System.Drawing.Color.FromArgb(48, 48, 48),
            ForeColor = System.Drawing.Color.FromArgb(218, 218, 28),
            Size = new Size(30, 40),
            Location = new Point(170, 60),

            FlatStyle = FlatStyle.Flat,
        };

        remove.FlatAppearance.BorderColor = Color.FromArgb(48, 48, 48);
        remove.FlatAppearance.BorderSize = 1;

        card.Controls.Add(label);
        card.Controls.Add(remove);
        cardsContainerFL.Controls.Add(card);



        // Evento para eliminar la card
        remove.Click += (s, e) =>
        {
            DialogResult resultado = MessageBox.Show("Si elimina este flete todo lo relacionado también se eliminará. ¿Desea eliminarlo de todas formas?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                RoundButton btn = (RoundButton)s;
                Panel parentCard = (Panel)btn.Parent;
                cardsContainerFL.Controls.Remove(parentCard);
                parentCard.Dispose(); // opcional
            }
        };

        campos.Clear();
        camposFaltantesTabla.Clear();
        this.campos = new List<string> { "Fecha", "Origen", "Destino", "RTO o CPE", "Carga", "Km", "Kg", "Tarifa", "Factura", "Comisión", "Cliente" };
        cantCamposTabla = campos.Count();

        this.camposFaltantesTabla = new List<string> { "Total", "Total comisión" };


        card.Click += (s, e) =>
        {
            this.Hide();
            ViajeFiltro form = new ViajeFiltro(item.nombre, cantCamposTabla, campos, filtro, camposFaltantesTabla);
            form.TopLevel = true;
            form.ShowDialog();
        };
    }

    private void GeneratorCard(Cliente item, string filtro)
    {
        Panel card = new Panel
        {
            Size = new Size(200, 100),
            BackColor = System.Drawing.Color.FromArgb(48, 48, 48),
            Margin = new Padding(10),
            Font = new Font("Nunito", 16, FontStyle.Regular),
        };

        Label label = new Label
        {

            Text = item.Nombre,
            ForeColor = System.Drawing.Color.FromArgb(218, 218, 28),
            AutoSize = true,
            TextAlign = ContentAlignment.TopCenter,
            Location = new Point(10, 10),
            BackColor = Color.Transparent
        };

        RoundButton remove = new RoundButton
        {
            Text = "🗑️",
            BackColor = System.Drawing.Color.FromArgb(48, 48, 48),
            ForeColor = System.Drawing.Color.FromArgb(218, 218, 28),
            Size = new Size(30, 40),
            Location = new Point(170, 60),

            FlatStyle = FlatStyle.Flat,
        };

        remove.FlatAppearance.BorderColor = Color.FromArgb(48, 48, 48);
        remove.FlatAppearance.BorderSize = 1;

        card.Controls.Add(label);
        card.Controls.Add(remove);
        cardsContainerFL.Controls.Add(card);


        // Evento para eliminar la card
        remove.Click += (s, e) =>
        {
            DialogResult resultado = MessageBox.Show("Si elimina este cliente todo lo relacionado también se eliminará. ¿Desea eliminarlo de todas formas?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                RoundButton btn = (RoundButton)s;
                Panel parentCard = (Panel)btn.Parent;
                cardsContainerFL.Controls.Remove(parentCard);
                parentCard.Dispose(); // opcional
            }
        };

        campos.Clear();
        camposFaltantesTabla.Clear();
        this.campos = new List<string> { "Fecha", "Origen", "Destino", "RTO o CPE", "Carga", "Km", "Kg", "Tarifa", "Chofer", "Camión", "Flete" };
        cantCamposTabla = campos.Count();


        this.camposFaltantesTabla = new List<string> { "Total" };


        card.Click += (s, e) =>
        {
            this.Hide();
            ViajeFiltro form = new ViajeFiltro(item.Nombre, cantCamposTabla, campos, filtro, camposFaltantesTabla);
            form.TopLevel = true;
            form.ShowDialog();
        };
    }

    private void GeneratorCard(CamionDTO item, string filtro)
    {
        Panel card = new Panel
        {
            Size = new Size(200, 100),
            BackColor = System.Drawing.Color.FromArgb(48, 48, 48),
            Margin = new Padding(10),
            Font = new Font("Nunito", 16, FontStyle.Regular),
        };

        Label label = new Label
        {

            Text = item.Patente,
            ForeColor = System.Drawing.Color.FromArgb(218, 218, 28),
            AutoSize = true,
            TextAlign = ContentAlignment.TopCenter,
            Location = new Point(10, 10),
            BackColor = Color.Transparent
        };

        RoundButton remove = new RoundButton
        {
            Text = "🗑️",
            BackColor = System.Drawing.Color.FromArgb(48, 48, 48),
            ForeColor = System.Drawing.Color.FromArgb(218, 218, 28),
            Size = new Size(30, 40),
            Location = new Point(170, 60),

            FlatStyle = FlatStyle.Flat,
        };

        remove.FlatAppearance.BorderColor = Color.FromArgb(48, 48, 48);
        remove.FlatAppearance.BorderSize = 1;

        card.Controls.Add(label);
        card.Controls.Add(remove);
        cardsContainerFL.Controls.Add(card);



        // Evento para eliminar la card
        remove.Click += (s, e) =>
        {
            DialogResult resultado = MessageBox.Show("Si elimina este camión todo lo relacionado también se eliminará. ¿Desea eliminarlo de todas formas?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                RoundButton btn = (RoundButton)s;
                Panel parentCard = (Panel)btn.Parent;
                cardsContainerFL.Controls.Remove(parentCard);
                parentCard.Dispose(); // opcional
            }
        };


        campos.Clear();
        camposFaltantesTabla.Clear();
        this.campos = new List<string> { "Fecha", "Origen", "Destino", "RTO o CPE", "Carga", "Km", "Kg", "Tarifa", "Porcentaje", "Chofer", "Cliente" };
        cantCamposTabla = campos.Count();

        this.camposFaltantesTabla = new List<string> { "Total", "Monto chofer" };


        card.Click += (s, e) =>
        {
            this.Hide();
            ViajeFiltro form = new ViajeFiltro(item.Patente, cantCamposTabla, campos, filtro, camposFaltantesTabla);
            form.TopLevel = true;
            form.ShowDialog();
        };
    }


    private void buttonsVisibleOrInvisible()
    {
        buttonBack.Click += (s, e) => ButtonNewAddProperties();
        buttonBack.Visible = true;
        buttonAddNew.Visible = false;
        formCargarSection.Visible = false;
        this.Controls.Add(buttonBack);
        ButtonProperties();
    }

    //AGREGAR CARD
    public async void GetFilterInfoAsync(string filtro, string info)
    {

        if (!string.IsNullOrWhiteSpace(info))
        {
            int i = 0;
            if (filtro == "Camion")
            {
                var idCamion = await cvm.InsertarAsync(info, "nombre");

                if (idCamion.IsSuccess)
                {
                    var resultado = await cvm.ObtenerPorId(idCamion.Value);

                    if (resultado.IsSuccess && i == 0)
                    {
                        i = 1;
                        CamionDTO camion = resultado.Value;

                    }
                    else
                    {
                        MessageBox.Show("No se pudo obtener el camión");
                    }

                }
                else
                {
                    MessageBox.Show(idCamion.Error + " ");
                    Console.WriteLine(idCamion.Error);
                }

            }
            else if (filtro == "Cliente")
            {

                ClienteViewModel cmv = new ClienteViewModel();
                var idCliente = await cmv.InsertarCliente(info);

                if (idCliente.IsSuccess)
                {

                    var resultado = await cmv.ObtenerById(idCliente.Value);

                    if (resultado.IsSuccess && i == 0)
                    {
                        i = 1;
                        Cliente cliente = resultado.Value;
                        MessageBox.Show(cliente.ToString());
                    }
                    else
                    {
                        MessageBox.Show("No se pudo obtener el cliente");
                        MessageBox.Show(resultado.Error);
                    }
                }
                else
                {
                    MessageBox.Show(idCliente.Error + " ");
                    Console.WriteLine(idCliente.Error);
                }
            }
            else if (filtro == "Flete")
            {
                FleteViewModel fvm = new FleteViewModel();
                var response = await fvm.InsertarAsync(info);
                if (response.IsSuccess)
                {
                    int idfletero = response.Value;
                    var fletero = await fvm.ObtenerPorIdAsync(idfletero);
                    if (fletero.IsSuccess)
                    {
                        i = 1;
                        MessageBox.Show("Fletero con el nombre: " + fletero.Value.nombre + " y id: " + fletero.Value.Id + " insertado correctamente");
                    }
                    else
                    {
                        MessageBox.Show(fletero.Error);
                    }
                }
                else
                {
                    MessageBox.Show(response.Error);
                }
            }
        }
        ObtenerTodosSegunFiltro(filtro, info);
        // Retornar la lista correspondiente
    }

    private void ButtonProperties()
    {
        buttonBack.Text = "Volver";
        buttonBack.Size = new Size(140, 40);
        buttonBack.FlatAppearance.BorderSize = 0;
        buttonBack.FlatStyle = FlatStyle.Flat;
        buttonBack.Location = new Point(40, 100);
        buttonBack.Font = new Font("Nunito", 16, FontStyle.Regular);
        buttonBack.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
        buttonBack.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
    }

    private void ButtonsProperties()
    {
        int j = 0;

        buttonsFilter.Add(fleteFilter);
        buttonsFilter.Add(clienteFilter);
        buttonsFilter.Add(camionFilter);

        buttonsNameFilter.Add("Flete");
        buttonsNameFilter.Add("Cliente");
        buttonsNameFilter.Add("Camion");

        for (int i = 0; i < buttonsFilter.Count; i++)
        {
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)buttonsFilter[i];

            btn.Size = new Size(150, 50);
            btn.ForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
            btn.Font = new Font("Nunito", 16, FontStyle.Regular);
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.Transparent;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.Margin = new Padding(0, 20, 0, 0);
            btn.TextAlign = ContentAlignment.MiddleCenter;

            if (j < buttonsNameFilter.Count)
            {
                btn.Text = buttonsNameFilter[j].ToString().ToUpper();
                j++;
            }

            btn.Click += (s, e) =>
            {
                ObtenerTodosSegunFiltro(btn.Text, " ");
            };

            filterFL.Controls.Add(btn);
        }
    }


    //CardProperties
    private void CardProperties()
    {
        cardsContainer.Size = new Size(800, 400);
        cardsContainer.BackColor = System.Drawing.Color.FromArgb(200, Color.Black);

        this.Resize += (s, e) =>
        {
            cardsContainer.Location = new Point((this.Width - cardsContainer.Width) / 2, ((this.Height - cardsContainer.Height) / 2) + 50);
        };
    }

    private void CardFLProperties()
    {
        cardsContainerFL.Size = new Size(700, 300);
        cardsContainerFL.FlowDirection = FlowDirection.LeftToRight;
        cardsContainerFL.WrapContents = true;
        cardsContainerFL.BackColor = Color.Transparent;
        cardsContainerFL.AutoScroll = true;
        cardsContainerFL.Visible = true;
        cardsContainerFL.Location = new Point((cardsContainer.Width - cardsContainerFL.Width) / 2, ((cardsContainer.Height - cardsContainerFL.Height) / 2));
    }

    private void ButtonNewAddProperties()
    {
        cardsContainerFL.Controls.Clear();

        buttonAddNew.Visible = true;
        buttonAddNew.Size = new Size(200, 150);
        buttonAddNew.BackColor = System.Drawing.Color.FromArgb(200, Color.Black);
        buttonAddNew.Text = "+";
        buttonAddNew.FlatStyle = FlatStyle.Flat;
        buttonAddNew.FlatAppearance.BorderSize = 3;  // Grosor del borde
        buttonAddNew.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(218, 218, 28); // Color del borde
        buttonAddNew.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
        buttonAddNew.Font = new Font("Nunito", 24, FontStyle.Bold);
        buttonAddNew.Margin = new Padding((cardsContainerFL.Width - buttonAddNew.Width) / 2, (cardsContainerFL.Height - buttonAddNew.Height) / 2, 0, 0);

        buttonBack.Visible = false;

        AddButtonNewAdd();

        buttonAddNew.Click += (s, e) => FormAddNew();
    }


    //FROM NEW ELEMENT
    private void FormAddNew()
    {
        layourFormSection.Controls.Clear();
        buttonAddNew.Visible = false;
        formCargarSection.Visible = true;
        FormProperties();
        LayoutFormProperties();
        TextBoxProperties();
        ButtonAceptProperties();
        ComboBoxProperties();
    }

    private void FormProperties()
    {
        formCargarSection.Size = new Size(200, 300);
        formCargarSection.Margin = new Padding((cardsContainerFL.Width - formCargarSection.Width) / 2, (cardsContainerFL.Height - formCargarSection.Height) / 2, 0, 0);
        formCargarSection.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
        cardsContainerFL.Controls.Add(formCargarSection);
        formCargarSection.Controls.Add(layourFormSection);
        formCargarSection.Controls.Add(buttonAcept);
    }

    private void LayoutFormProperties()
    {
        layourFormSection.BackColor = Color.Transparent;
        layourFormSection.FlowDirection = FlowDirection.TopDown;
        layourFormSection.AutoSize = true;
        layourFormSection.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        // Centramos después de la carga
        formCargarSection.Resize += (s, e) => CenterLayoutFormSection();

        // Llamar al método después de agregar los elementos
        AddElementsOfLayoutFrom();
        CenterLayoutFormSection(); // Para posicionar correctamente al inicio
    }

    private void CenterLayoutFormSection()
    {
        if (layourFormSection.Parent != null)
        {
            layourFormSection.Location = new Point(
                (formCargarSection.Width - layourFormSection.Width) / 2, 60
            );
        }
    }
    private void ComboBoxProperties()
    {
        select.Size = new Size(120, 30);
        select.Items.Clear();
        select.Items.Add("Cliente");
        select.Items.Add("Flete");
        select.Items.Add("Camion");

        select.SelectedIndex = 0;
    }

    private void AddElementsOfLayoutFrom()
    {
        List<string> campo = new List<string>();
        campo.Add("Seleccionar");
        campo.Add("Nombre");

        foreach (string i in campo)
        {
            Label labelForm = LabelProperties(i);

            layourFormSection.Controls.Add(labelForm);
            AddTheRestComponents(i);
        }
    }

    private Label LabelProperties(string nombreCampo)
    {
        Label ll = new Label();

        ll.Text = nombreCampo;
        ll.Font = new Font("Nunito", 10, FontStyle.Regular);
        ll.ForeColor = System.Drawing.Color.FromArgb(217, 217, 217);
        ll.BackColor = Color.Transparent;
        ll.AutoSize = true;

        return ll;
    }

    private void AddTheRestComponents(string nombreCampo)
    {
        if (nombreCampo == "Seleccionar")
        {
            layourFormSection.Controls.Add(select);
        }
        else
        {
            layourFormSection.Controls.Add(textBoxNombre);
        }
    }

    private void TextBoxProperties()
    {
        textBoxNombre.Font = new Font("Nunito", 10, FontStyle.Regular);
        textBoxNombre.BackColor = Color.White;
        textBoxNombre.Multiline = true;
        textBoxNombre.Width = 120;
        textBoxNombre.Height = 20;
        textBoxNombre.BorderStyle = BorderStyle.None;
        textBoxNombre.ForeColor = System.Drawing.Color.FromArgb(81, 77, 77);
    }

    private void ButtonAceptProperties()
    {
        buttonAcept.BackColor = System.Drawing.Color.FromArgb(218, 218, 28);
        buttonAcept.AutoSize = true;
        buttonAcept.Height = 30;
        buttonAcept.Text = "Cargar";
        buttonAcept.FlatStyle = FlatStyle.Flat;
        buttonAcept.FlatAppearance.BorderSize = 0;
        buttonAcept.Margin = new Padding(132, 10, 0, 0);
        buttonAcept.ForeColor = System.Drawing.Color.FromArgb(32, 32, 32);
        buttonAcept.Font = new Font("Nunito", 12, FontStyle.Bold);

        buttonAcept.Click += (s, e) => ButtonAcept_Click1(s, e);

        CenterButtonFormSection(); // Para posicionar correctamente al inicio
    }

    private void ButtonAcept_Click1(object sender, EventArgs e)
    {
        if (select.SelectedItem == null)
        {
            MessageBox.Show("Por favor, selecciona una opción antes de continuar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }
        string seleccion = select.SelectedItem.ToString();
        GetFilterInfoAsync(seleccion, textBoxNombre.Text);
    }

    private void CenterButtonFormSection()
    {
        if (buttonAcept.Parent != null)
        {
            buttonAcept.Location = new Point(
                (formCargarSection.Width - buttonAcept.Width) / 2, 210
            );
        }
    }
}
