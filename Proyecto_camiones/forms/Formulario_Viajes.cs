using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_camiones.Presentacion
{
    public partial class Formulario_Viajes: Form
    {
        public Formulario_Viajes()
        {
            InitializeComponent();
        }

        #region "mis metodos"
        private void Formato_pagos() 
        {
            
            sueldos.Columns[0].Width = 80;
            sueldos.Columns[0].HeaderText = "CODIGO";
            sueldos.Columns[1].Width = 80;
            sueldos.Columns[1].HeaderText = "MONTO";
            sueldos.Columns[2].Width = 80;
            sueldos.Columns[2].HeaderText = "PAGADO";
        }
        private void listado_pagos(String cTexto) 
        {
            /*Datos_pagos Datos = new Datos_pagos();
            sueldos.DataSource = Datos.Listado_pagos(cTexto);
            this.Formato_pagos();
            */
        }
        private void Estado_texto(bool lEstado)
        {
            txt_monto.ReadOnly = !lEstado;
            txt_pagado.ReadOnly = !lEstado;
        }
        private void Estado_botones_procesos(bool lestado)
        {
            btn_cancelar.Visible = lestado;
            btn_agregar.Visible = lestado;
        }

        private void Estado_botones_principales(bool lestado)
        {
            Btn_nuevo.Enabled = lestado;
            Btn_actualizar.Enabled = lestado;
            Btn_eliminar.Enabled = lestado;
            Btn_salir.Enabled = lestado;


        }
        #endregion

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Formulario_Viajes_Load(object sender, EventArgs e)
        {
            this.listado_pagos("%");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            this.listado_pagos("%"+txt_buscar.Text.Trim()+"%");
        }

        private void Btn_nuevo_Click(object sender, EventArgs e)
        {
            this.Estado_texto(true);
            this.Estado_botones_procesos(true);
            this.Estado_botones_principales(false);
            txt_monto.Focus();
        }

        private void Btn_actualizar_Click(object sender, EventArgs e)
        {
           
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            this.Estado_texto(false);
            this.Estado_botones_procesos(false);
            this.Estado_botones_principales(true);
        }
    }
}
