using System.Drawing;
using System.IO;
using System;
using System.Windows.Forms;

namespace AppCamiones
{
    public class Home : Form
    {
        //NavBar
        protected MenuStrip menuStrip = new MenuStrip();

        //protected ToolStripMenuItem homeMenu = new ToolStripMenuItem("home");
        protected ToolStripMenuItem viajesMenu = new ToolStripMenuItem("viajes");
        protected ToolStripMenuItem chequesMenu = new ToolStripMenuItem("cheques");
        private string filtro = " ";
        private Form activeForm;

        //Constructor
        public Home()
        {
            InitializeUI();
            this.WindowState = FormWindowState.Maximized;


            //Redirections
            chequesMenu.Click += (sender, e) => OpenForm<Cheque>();
            viajesMenu.Click += (sender, e) => OpenForm<Viaje>();
            //homeMenu.Click += (sender, e) => OpenForm<Form1>();
        }


        //RedirectionalFunctions
        private void OpenForm<T>() where T : Form, new()
        {
            // Cierra el formulario activo si existe
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = null;
            }

            // Abre el nuevo formulario
            activeForm = new T();
            activeForm.TopLevel = false;
            activeForm.FormBorderStyle = FormBorderStyle.None;
            activeForm.Dock = DockStyle.Fill;

            this.Controls.Add(activeForm); // Si no usás panel, agregalo directo al Form
            activeForm.Show();
        }


        //HoverFunctions
        public void ResaltarBoton(ToolStripMenuItem menuItem)
        {
            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                item.BackColor = Color.Transparent;
                item.Font = new Font(item.Font, FontStyle.Regular);
            }

            if (menuItem != null)
            {
                menuItem.Font = new Font("Nunito", 18, FontStyle.Regular);
                menuItem.ForeColor = System.Drawing.Color.FromArgb(194, 194, 119);
            }
        }



        //Initializations
        private void InitializeUI()
        {
            InitializeToolBar();
            InitializeBackImage();
            InitializeIconoApp();
            //InitializeIconoUser();
        }
        private void InitializeToolBar()
        {
            ItemsCapitalLetter();
            MenuProperties();
            ItemsColor();
            MarginToItems();
            AddItemsToMenu();
        }
        private void InitializeBackImage()
        {
            string imagePath = Path.Combine(Application.StartupPath, "Resources", "goma.jpg");

            if (File.Exists(imagePath))
            {
                Image img = Image.FromFile(imagePath);

                if (Array.Exists(img.PropertyIdList, id => id == 0x0112)) 
                {
                    int orientation = BitConverter.ToUInt16(img.GetPropertyItem(0x0112).Value, 0);

                    switch (orientation)
                    {
                        case 1:
                            break;
                        case 3:
                            img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case 6:
                            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 8:
                            img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }
                }

                this.BackgroundImage = img;
            }
            else
            {
                MessageBox.Show("La imagen no se encuentra: " + imagePath);
            }
        }
        private void InitializeIconoApp()
        {
            string iconoApp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "icono_camion.ico");

            if (File.Exists(iconoApp))
            {
                this.Icon = new Icon(iconoApp);
            }
            else
            {
                MessageBox.Show("La imagen no se encuentra: " + iconoApp);
            }
        }
        //private void InitializeIconoUser()
        //{
        //    string icono_user = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "icono_user.png");


        //    if (File.Exists(icono_user))
        //    {
        //        userMenu.Image = Image.FromFile(icono_user);
        //    }
        //    else
        //    {
        //        MessageBox.Show("La imagen no se encuentra: " + icono_user);
        //    }
        //}




        //Adds
        private void AddItemsToMenu()
        {
            //menuStrip.Items.Add(homeMenu);
            menuStrip.Items.Add(viajesMenu);
            menuStrip.Items.Add(chequesMenu);
            //menuStrip.Items.Add(userMenu);

            //userMenu.DropDownItems.Add(closeSession);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }




        //NavProperties
        private void ItemsCapitalLetter()
        {
            //homeMenu.Text = homeMenu.Text.ToUpper();
            viajesMenu.Text = viajesMenu.Text.ToUpper();
            chequesMenu.Text = chequesMenu.Text.ToUpper();
            //userMenu.Text = userMenu.Text.ToUpper();
        }
        private void MenuProperties()
        {
            menuStrip.Font = new Font("Arial", 14, FontStyle.Regular);
            menuStrip.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
            menuStrip.ImageScalingSize = new Size(34, 34);
            menuStrip.AutoSize = false;
            menuStrip.Width = this.Width;
            menuStrip.Height = 80;
            menuStrip.Dock = DockStyle.Top;
        }
        private void ItemsColor()
        {
            //homeMenu.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
            viajesMenu.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
            chequesMenu.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
            //userMenu.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
            //closeSession.ForeColor = System.Drawing.Color.FromArgb(141, 138, 138);
        }
        private void MarginToItems()
        {
            int x = this.Width;
            int y = x / 10;
            //int t = (menuStrip.Width - userMenu.Width);

            //homeMenu.Margin = new Padding(y, 0, 0, 0);
            viajesMenu.Margin = new Padding(y, 0, 0, 0);
            chequesMenu.Margin = new Padding(y, 0, 0, 0);
            //userMenu.Margin = new Padding(t, 0, 0, 0);
            //closeSession.Margin = new Padding(0, 10, 0, 0);
        }
    }
}
