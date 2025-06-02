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
using MySqlX.XDevAPI.Common;
using Mysqlx.Session;

namespace Proyecto_camiones.Front;

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
    private System.Windows.Forms.TextBox textBoxPatente = new System.Windows.Forms.TextBox();
    private System.Windows.Forms.TextBox textBoxChofer = new System.Windows.Forms.TextBox();

    private RoundButton buttonAcept = new RoundButton();
    private RoundButton buttonBack = new RoundButton();

    private List<string> camiones = new List<string>();
    private List<string> clientes = new List<string>();
    private List<string> fletes = new List<string>();

    private List<string> campos = new List<string>();

    private List<string> camposFaltantesTabla = new List<string>();


    private int cantCamposTabla = 0;

    private NewRoundPanel avisoPanel = new NewRoundPanel(20);
    private System.Windows.Forms.Button btnAceptarAviso = new System.Windows.Forms.Button();


    //Card
    private Panel cardsContainer = new Panel();
    private FlowLayoutPanel cardsContainerFL = new FlowLayoutPanel();

    private CamionViewModel cvm = new CamionViewModel();



    //Constructor
    public Viaje()
    {
        InitializeFilterCards(" ");
        ResaltarBoton(viajesMenu);


        //Hovers
        fleteFilter.MouseEnter += (s, e) => HoverEffect(s, e, true);
        fleteFilter.MouseLeave += (s, e) => HoverEffect(s, e, false);

        clienteFilter.MouseEnter += (s, e) => HoverEffect(s, e, true);
        clienteFilter.MouseLeave += (s, e) => HoverEffect(s, e, false);

        camionFilter.MouseEnter += (s, e) => HoverEffect(s, e, true);
        camionFilter.MouseLeave += (s, e) => HoverEffect(s, e, false);

        fleteFilter.Click += (s, e) => ObtenerTodosSegunFiltro("Flete");
        clienteFilter.Click += (s, e) => ObtenerTodosSegunFiltro("Cliente");
        camionFilter.Click += (s, e) => ObtenerTodosSegunFiltro("Camion");
    }

    public Viaje(string filtro)
    {
        InitializeFilterCards(filtro);
        FunctionFilter(filtro);
        ResaltarBoton(viajesMenu);
        if (filtro != null)
        {
            ObtenerTodosSegunFiltro(filtro);
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

    private void FunctionFilter(string filtro)
    {
        fleteFilter.Click += (s, e) => ObtenerTodosSegunFiltro("Flete");
        clienteFilter.Click += (s, e) => ObtenerTodosSegunFiltro("Cliente");
        camionFilter.Click += (s, e) => ObtenerTodosSegunFiltro("Camion");

        fleteFilter.MouseEnter += (s, e) => HoverEffect(s, e, true);
        fleteFilter.MouseLeave += (s, e) => HoverEffect(s, e, false);

        clienteFilter.MouseEnter += (s, e) => HoverEffect(s, e, true);
        clienteFilter.MouseLeave += (s, e) => HoverEffect(s, e, false);

        camionFilter.MouseEnter += (s, e) => HoverEffect(s, e, true);
        camionFilter.MouseLeave += (s, e) => HoverEffect(s, e, false);
    }

    //Initializations

    private void InitializeFilterCards(string filtro)
    {
        OptionsMenuProperties();
        LayoutOptionsMenuProperties();
        ButtonsProperties();
        CardProperties();
        CardFLProperties();
        AddItemsToFilter();
        ButtonNewAddProperties(filtro);
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
    public async void ObtenerTodosSegunFiltro(string filtro)
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
            FleteViewModel fvm = new FleteViewModel();
            var resultado = await fvm.ObtenerTodosAsync();
            if (resultado.IsSuccess)
            {
                CardFleteGenerator("Flete", resultado);
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
    }

    private void CardFleteGenerator(string filtro, Result<List<Flete>> resultado)
    {
        cardsContainerFL.Controls.Clear();
        foreach (var item in resultado.Value)
        {
            GeneratorCard(item, filtro);
        }
        buttonsVisibleOrInvisible(filtro);
    }

    private void CardCamionGenerator(string filtro, Result<List<CamionDTO>> resultado)
    {
        cardsContainerFL.Controls.Clear();
        foreach (var item in resultado.Value)
        {
            GeneratorCard(item, filtro);
        }
        buttonsVisibleOrInvisible(filtro);
    }

    private void CardClienteGenerator(string filtro, Result<List<Cliente>> resultado)
    {
        cardsContainerFL.Controls.Clear();
        foreach (var item in resultado.Value)
        {
            GeneratorCard(item, filtro);
        }

        buttonsVisibleOrInvisible(filtro);
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
        remove.Click += async (s, e) =>
        {
            DialogResult resultado = MessageBox.Show("Si elimina este flete todo lo relacionado también se eliminará. ¿Desea eliminarlo de todas formas?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                if (resultado == DialogResult.Yes)
                {
                    FleteViewModel fvm = new FleteViewModel();
                    var response = await fvm.EliminarAsync(item.Id);
                    if (response.IsSuccess)
                    {
                        ObtenerTodosSegunFiltro(filtro);
                    }
                    else
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() => CartelAviso(response.Error)));
                        }
                        else
                        {
                            CartelAviso(response.Error);
                        }
                    }
                }
            }
        };

        campos.Clear();
        camposFaltantesTabla.Clear();
        this.campos = new List<string> { "Fecha", "Origen", "Destino", "RTO o CPE", "Carga", "Km", "Kg", "Tarifa", "Factura", "Comisión", "Cliente", "Chofer" };
        cantCamposTabla = campos.Count();

        this.camposFaltantesTabla = new List<string> { "Total", "Total comisión" };

        card.Click += (s, e) =>
        {
            this.Hide();
            ViajeFiltro form = new ViajeFiltro(item.nombre, cantCamposTabla, campos, filtro, camposFaltantesTabla, " ");
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
            BackColor = Color.Transparent,
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

        label.Text = label.Text.ToUpper();
        remove.FlatAppearance.BorderColor = Color.FromArgb(48, 48, 48);
        remove.FlatAppearance.BorderSize = 1;

        card.Controls.Add(label);
        card.Controls.Add(remove);
        cardsContainerFL.Controls.Add(card);

        // Evento para eliminar la card
        remove.Click += async (s, e) =>
        {
            DialogResult resultado = MessageBox.Show("Si elimina este cliente todo lo relacionado también se eliminará. ¿Desea eliminarlo de todas formas?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                ClienteViewModel cvm = new ClienteViewModel();
                var response = await cvm.Eliminar(item.Id);
                if (response.IsSuccess)
                {
                    ObtenerTodosSegunFiltro(filtro);
                }
                else
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => CartelAviso(response.Error)));
                    }
                    else
                    {
                        CartelAviso(response.Error);
                    }
                }

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
            ViajeFiltro form = new ViajeFiltro(item.Nombre, cantCamposTabla, campos, filtro, camposFaltantesTabla, " ");
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

        Label labelPatente = new Label
        {
            Text = item.Patente,
            ForeColor = System.Drawing.Color.FromArgb(218, 218, 28),
            AutoSize = true,
            TextAlign = ContentAlignment.TopCenter,
            Location = new Point(10, 10),
            BackColor = Color.Transparent
        };

        Label labelChofer = new Label
        {
            Text = item.Nombre_Chofer,
            Font = new Font("Nunito", 10, FontStyle.Regular),
            ForeColor = System.Drawing.Color.FromArgb(218, 218, 28),
            AutoSize = true,
            TextAlign = ContentAlignment.TopCenter,
            Location = new Point(10, 40),
            BackColor = Color.Transparent
        };

        labelPatente.Text = labelPatente.Text.ToUpper();
        labelChofer.Text = labelChofer.Text.ToUpper();

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

        card.Controls.Add(labelPatente);
        card.Controls.Add(labelChofer);
        card.Controls.Add(remove);
        cardsContainerFL.Controls.Add(card);

        // Evento para eliminar la card
        remove.Click += async (s, e) =>
        {
            DialogResult resultado = MessageBox.Show("Si elimina este camión todo lo relacionado también se eliminará. ¿Desea eliminarlo de todas formas?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                CamionViewModel cvm = new CamionViewModel();
                var response = await cvm.EliminarAsync(item.Id);
                if (response.IsSuccess)
                {
                    ObtenerTodosSegunFiltro(filtro);
                }
                else
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => CartelAviso(response.Error)));
                    }
                    else
                    {
                        CartelAviso(response.Error);
                    }
                }

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
            ViajeFiltro form = new ViajeFiltro(item.Patente, cantCamposTabla, campos, filtro, camposFaltantesTabla, item.Nombre_Chofer);
            form.TopLevel = true;
            form.ShowDialog();
        };
    }


    private void buttonsVisibleOrInvisible(string filtro)
    {
        buttonBack.Click -= (s, e) => ButtonNewAddProperties(filtro);
        buttonBack.Click += (s, e) => ButtonNewAddProperties(filtro);
        buttonBack.Visible = true;
        buttonAddNew.Visible = false;
        formCargarSection.Visible = false;
        this.Controls.Add(buttonBack);
        ButtonProperties();
    }

    //AGREGAR CARD
    public async void GetFilterInfoAsync(string filtro, string info, string text)
    {
        if (!string.IsNullOrWhiteSpace(info))
        {
            if (filtro == "Camion")
            {
                var idCamion = await cvm.InsertarAsync(info, text);

                if (idCamion.IsSuccess)
                {
                    var resultado = await cvm.ObtenerPorId(idCamion.Value);

                    if (resultado.IsSuccess)
                    {
                        ObtenerTodosSegunFiltro(filtro);
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
                else
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => CartelAviso(idCamion.Error)));
                    }
                    else
                    {
                        CartelAviso(idCamion.Error);
                    }
                }
            }
            else if (filtro == "Cliente")
            {
                ClienteViewModel cmv = new ClienteViewModel();
                var idCliente = await cmv.InsertarCliente(info);

                if (idCliente.IsSuccess)
                {

                    var resultado = await cmv.ObtenerById(idCliente.Value);

                    if (resultado.IsSuccess)
                    {
                        ObtenerTodosSegunFiltro(filtro);
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
                else
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => CartelAviso(idCliente.Error)));
                    }
                    else
                    {
                        CartelAviso(idCliente.Error);
                    }
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
                        ObtenerTodosSegunFiltro(filtro);
                    }
                    else
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() => CartelAviso(fletero.Error)));
                        }
                        else
                        {
                            CartelAviso(fletero.Error);
                        }
                    }
                }
                else
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => CartelAviso(response.Error)));
                    }
                    else
                    {
                        CartelAviso(response.Error);
                    }
                }
            }
        }
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

            btn.Click -= (s, e) =>
            {
                ObtenerTodosSegunFiltro(btn.Text);
            };

            btn.Click += (s, e) =>
            {
                ObtenerTodosSegunFiltro(btn.Text);
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

    private void ButtonNewAddProperties(string filtro)
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

        buttonAddNew.Click -= (s, e) => FormAddNew(s, e, filtro);
        buttonAddNew.Click += (s, e) => FormAddNew(s, e, filtro);
    }


    //FROM NEW ELEMENT
    private void FormAddNew(object sender, EventArgs e, string filtro)
    {
        layourFormSection.Controls.Clear();
        buttonAddNew.Visible = false;
        formCargarSection.Visible = true;
        FormProperties();
        LayoutFormProperties(filtro);
        TextBoxProperties();
        ComboBoxProperties();
        ButtonAceptProperties();
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

    private void LayoutFormProperties(string filtro)
    {
        layourFormSection.BackColor = Color.Transparent;
        layourFormSection.FlowDirection = FlowDirection.TopDown;
        layourFormSection.AutoSize = true;
        layourFormSection.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        // Centramos después de la carga
        formCargarSection.Resize += (s, e) => CenterLayoutFormSection();

        // Llamar al método después de agregar los elementos
        AddElementsOfLayoutFrom(filtro);
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

        select.SelectedIndexChanged += Select_SelectedIndexChanged;

    }

    private void Select_SelectedIndexChanged(object sender, EventArgs e)
    {
        string seleccion = select.SelectedItem?.ToString();


        if (seleccion == "Camion")
        {
            AddElementsOfLayoutFrom("Camion");
        } else
        {
            AddElementsOfLayoutFrom(" ");
        }
         CenterLayoutFormSection(); // Volvés a centrar
    }


    private void AddElementsOfLayoutFrom(string filtro)
    {
        layourFormSection.Controls.Clear();
        List<string> campo = new List<string>();

        campo.Add("Seleccionar");

        if (filtro != "Camion")
        {
            campo.Add("Nombre");
        }

        if(filtro == "Camion")
        {
            campo.Add("Patente");
            campo.Add("Chofer");
        }

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
        else if(nombreCampo == "Nombre" || nombreCampo == "Patente")
        {
            layourFormSection.Controls.Add(textBoxNombre);
        }
        else if (nombreCampo == "Chofer")
        {
            layourFormSection.Controls.Add(textBoxChofer);
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

        textBoxChofer.Font = new Font("Nunito", 10, FontStyle.Regular);
        textBoxChofer.BackColor = Color.White;
        textBoxChofer.Multiline = true;
        textBoxChofer.Width = 120;
        textBoxChofer.Height = 20;
        textBoxChofer.BorderStyle = BorderStyle.None;
        textBoxChofer.ForeColor = System.Drawing.Color.FromArgb(81, 77, 77);
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

        buttonAcept.Click -= (s, e) => ButtonAcept_Click1(s, e);
        buttonAcept.Click += (s, e) => ButtonAcept_Click1(s, e);

        CenterButtonFormSection(); // Para posicionar correctamente al inicio
    }

    private void ButtonAcept_Click1(object sender, EventArgs e)
    {
        if (select.SelectedItem.ToString() == null)
        {
            CartelAviso("Seleccione una opción");
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => CartelAviso("Seccione una opción")));
            }
            else
            {
                CartelAviso("Seleccione una opción");
            }

        }
        string seleccion = select.SelectedItem.ToString();

        GetFilterInfoAsync(seleccion, textBoxNombre.Text, textBoxChofer.Text);
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

    private void CartelAviso(string mensaje)
    {
        ButtonAceptAvisoProperties();
        this.Controls.Add(avisoPanel);

        Label ll = new Label();
        ll.Text = mensaje;
        ll.Font = new Font("Nunito", 14, FontStyle.Regular);
        ll.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
        ll.AutoSize = false; // permite controlar el tamaño manualmente
        ll.Width = 300; // ajustá el ancho según el diseño
        ll.Height = 100; // ajustá la altura
        ll.TextAlign = ContentAlignment.MiddleCenter;
        ll.MaximumSize = new Size(300, 0); // límite de ancho, altura auto
        ll.AutoSize = true; // activa ajuste automático de altura


        // Centrar el label dentro del panel
        ll.Resize += (s, e) =>
        {
            ll.Location = new Point(
            (avisoPanel.Width - ll.Width) / 2, 25);
        };

        avisoPanel.BringToFront();  // Traer al frente
        avisoPanel.Visible = true;  // Asegurar que sea visible


        avisoPanel.Size = new Size(300, 150);
        avisoPanel.BackColor =  System.Drawing.Color.FromArgb(32, 32, 32);

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

    private void ButtonAceptAvisoProperties()
    {
        avisoPanel.Visible = true;

        btnAceptarAviso.BackColor = System.Drawing.Color.FromArgb(218, 218, 28);
        btnAceptarAviso.ForeColor = System.Drawing.Color.FromArgb(48, 48, 48);
        btnAceptarAviso.Size = new Size(110, 30);

        btnAceptarAviso.Text = "Aceptar";
        btnAceptarAviso.FlatStyle = FlatStyle.Flat;
        btnAceptarAviso.FlatAppearance.BorderSize = 0;
        btnAceptarAviso.Font = new Font("Nunito", 12, FontStyle.Bold);

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
