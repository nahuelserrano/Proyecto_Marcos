using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto_camiones.Front
{
    internal class CuentaCorriente : FormRegistro
    {
        private RoundButton btnVolver = new RoundButton();

        public CuentaCorriente(string dato, string filtro)
            : base(new List<string> { "Fecha", "Nro factura", "Pagado", "Adeuda" }, 4, dato, "cuenta corriente", new List<string> { "Total adeudado" }, filtro)
        {
            InitializeUI(dato, filtro);
        }

        private void InitializeUI(string dato, string filtro)
        {
            this.Controls.Add(btnVolver);
            ResaltarBoton(viajesMenu);
            btnVolverProperties(dato, filtro);
        }

        private void btnVolverProperties(string dato, string  filtro)
        {
            btnVolver.Text = "Volver";
            btnVolver.Size = new Size(140, 40);
            btnVolver.FlatAppearance.BorderSize = 0;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.Location = new Point(40, 130);
            btnVolver.Font = new Font("Nunito", 16, FontStyle.Regular);
            btnVolver.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
            btnVolver.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);

            btnVolver.Click += (s, e) =>
            {
                this.Hide();

                List<string> campos = new List<string> { "Fecha", "Origen", "Destino", "RTO o CPE", "Carga", "Km", "Kg", "Tarifa", "Chofer", "Camión", "Flete" };
                int cantCamposTabla = campos.Count;

                List<string> camposFaltantesTabla = new List<string> { "Total" };

                List<string> camposLista = new List<string>();
                foreach (string i in campos)
                {
                    camposLista.Add(i);
                }
                FormRegistro form = new FormRegistro(camposLista, cantCamposTabla, dato, filtro, camposFaltantesTabla, " ");
                this.Hide();
                form.ShowDialog(); // Bloquea el anterior y no genera parpadeo
                this.Controls.Add(btnVolver);
            };
        }
    }
}