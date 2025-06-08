using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_camiones.Front;

internal class SueldoMensual : FormRegistro
{
    private RoundButton btnVolver = new RoundButton();

    public SueldoMensual(string dato, string filtro, string nombreChofer)
        : base(new List<string> { "Fecha inicial", "Fecha final", "Chofer" }, 3, dato, "sueldo", new List<string> { "Rango fechas", "Chofer", "Sueldo" }, nombreChofer)
    {
        InitializeUI(dato, filtro);
    }

    private void InitializeUI(string dato, string filtro)
    {
        this.Controls.Add(btnVolver);
        ResaltarBoton(viajesMenu);
        BtnVolverProperties(dato, filtro);
    }

    private void BtnVolverProperties(string dato, string filtro)
    {
        btnVolver.Text = "Volver";
        btnVolver.Size = new Size(140, 40);
        btnVolver.FlatAppearance.BorderSize = 0;
        btnVolver.FlatStyle = FlatStyle.Flat;
        btnVolver.Location = new Point(40, 100);
        btnVolver.Font = new Font("Nunito", 16, FontStyle.Regular);
        btnVolver.BackColor = System.Drawing.Color.FromArgb(48, 48, 48);
        btnVolver.ForeColor = System.Drawing.Color.FromArgb(218, 218, 28);
        //btnVolver.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(218, 218, 28);
        btnVolver.Click += (s, e) =>
        {
            this.Hide();

            List<string> campos = new List<string> { "Fecha", "Origen", "Destino", "RTO o CPE", "Carga", "Km", "Kg", "Tarifa", "Porcentaje", "Chofer", "Cliente" };
            int cantCamposTabla = campos.Count;

            List<string> camposFaltantesTabla = new List<string> { "Total", "Monto chofer" };

            ViajeFiltro form = new ViajeFiltro(dato, cantCamposTabla, campos, filtro, camposFaltantesTabla, null);
        };  
    }
}
