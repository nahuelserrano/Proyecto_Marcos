namespace Proyecto_camiones.Presentacion
{
    partial class Formulario_Viajes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sueldos = new System.Windows.Forms.DataGridView();
            this.cifraDeMonto = new System.Windows.Forms.Label();
            this.txt_monto = new System.Windows.Forms.TextBox();
            this.txt_pagado = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_agregar = new System.Windows.Forms.Button();
            this.Btn_nuevo = new System.Windows.Forms.Button();
            this.Btn_salir = new System.Windows.Forms.Button();
            this.Btn_eliminar = new System.Windows.Forms.Button();
            this.Btn_actualizar = new System.Windows.Forms.Button();
            this.btn_cancelar = new System.Windows.Forms.Button();
            this.txt_buscar = new System.Windows.Forms.TextBox();
            this.buscar = new System.Windows.Forms.Label();
            this.btn_buscar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sueldos)).BeginInit();
            this.SuspendLayout();
            // 
            // sueldos
            // 
            this.sueldos.AllowUserToAddRows = false;
            this.sueldos.AllowUserToDeleteRows = false;
            this.sueldos.AllowUserToOrderColumns = true;
            this.sueldos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sueldos.Location = new System.Drawing.Point(88, 260);
            this.sueldos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sueldos.Name = "sueldos";
            this.sueldos.ReadOnly = true;
            this.sueldos.RowHeadersWidth = 82;
            this.sueldos.RowTemplate.Height = 33;
            this.sueldos.Size = new System.Drawing.Size(910, 442);
            this.sueldos.TabIndex = 0;
            // 
            // cifraDeMonto
            // 
            this.cifraDeMonto.AutoSize = true;
            this.cifraDeMonto.Location = new System.Drawing.Point(104, 117);
            this.cifraDeMonto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cifraDeMonto.Name = "cifraDeMonto";
            this.cifraDeMonto.Size = new System.Drawing.Size(71, 25);
            this.cifraDeMonto.TabIndex = 1;
            this.cifraDeMonto.Text = "monto";
            // 
            // txt_monto
            // 
            this.txt_monto.Location = new System.Drawing.Point(196, 117);
            this.txt_monto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_monto.Name = "txt_monto";
            this.txt_monto.ReadOnly = true;
            this.txt_monto.Size = new System.Drawing.Size(300, 31);
            this.txt_monto.TabIndex = 2;
            // 
            // txt_pagado
            // 
            this.txt_pagado.Location = new System.Drawing.Point(696, 117);
            this.txt_pagado.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_pagado.Name = "txt_pagado";
            this.txt_pagado.ReadOnly = true;
            this.txt_pagado.Size = new System.Drawing.Size(300, 31);
            this.txt_pagado.TabIndex = 4;
            this.txt_pagado.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(584, 117);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "pagado";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btn_agregar
            // 
            this.btn_agregar.Location = new System.Drawing.Point(272, 188);
            this.btn_agregar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_agregar.Name = "btn_agregar";
            this.btn_agregar.Size = new System.Drawing.Size(126, 48);
            this.btn_agregar.TabIndex = 5;
            this.btn_agregar.Text = "Agregar";
            this.btn_agregar.UseVisualStyleBackColor = true;
            this.btn_agregar.Visible = false;
            // 
            // Btn_nuevo
            // 
            this.Btn_nuevo.Location = new System.Drawing.Point(1142, 37);
            this.Btn_nuevo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Btn_nuevo.Name = "Btn_nuevo";
            this.Btn_nuevo.Size = new System.Drawing.Size(192, 142);
            this.Btn_nuevo.TabIndex = 6;
            this.Btn_nuevo.Text = "Nuevo";
            this.Btn_nuevo.UseVisualStyleBackColor = true;
            this.Btn_nuevo.Click += new System.EventHandler(this.Btn_nuevo_Click);
            // 
            // Btn_salir
            // 
            this.Btn_salir.Location = new System.Drawing.Point(1142, 560);
            this.Btn_salir.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Btn_salir.Name = "Btn_salir";
            this.Btn_salir.Size = new System.Drawing.Size(192, 142);
            this.Btn_salir.TabIndex = 7;
            this.Btn_salir.Text = "salir";
            this.Btn_salir.UseVisualStyleBackColor = true;
            // 
            // Btn_eliminar
            // 
            this.Btn_eliminar.Location = new System.Drawing.Point(1142, 381);
            this.Btn_eliminar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Btn_eliminar.Name = "Btn_eliminar";
            this.Btn_eliminar.Size = new System.Drawing.Size(192, 142);
            this.Btn_eliminar.TabIndex = 8;
            this.Btn_eliminar.Text = "Eliminar";
            this.Btn_eliminar.UseVisualStyleBackColor = true;
            // 
            // Btn_actualizar
            // 
            this.Btn_actualizar.Location = new System.Drawing.Point(1142, 206);
            this.Btn_actualizar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Btn_actualizar.Name = "Btn_actualizar";
            this.Btn_actualizar.Size = new System.Drawing.Size(192, 142);
            this.Btn_actualizar.TabIndex = 9;
            this.Btn_actualizar.Text = "Actualizar";
            this.Btn_actualizar.UseVisualStyleBackColor = true;
            this.Btn_actualizar.Click += new System.EventHandler(this.Btn_actualizar_Click);
            // 
            // btn_cancelar
            // 
            this.btn_cancelar.Location = new System.Drawing.Point(118, 188);
            this.btn_cancelar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_cancelar.Name = "btn_cancelar";
            this.btn_cancelar.Size = new System.Drawing.Size(126, 48);
            this.btn_cancelar.TabIndex = 10;
            this.btn_cancelar.Text = "Cancelar";
            this.btn_cancelar.UseVisualStyleBackColor = true;
            this.btn_cancelar.Visible = false;
            this.btn_cancelar.Click += new System.EventHandler(this.btn_cancelar_Click);
            // 
            // txt_buscar
            // 
            this.txt_buscar.Location = new System.Drawing.Point(664, 198);
            this.txt_buscar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_buscar.Name = "txt_buscar";
            this.txt_buscar.Size = new System.Drawing.Size(300, 31);
            this.txt_buscar.TabIndex = 12;
            this.txt_buscar.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // buscar
            // 
            this.buscar.AutoSize = true;
            this.buscar.Location = new System.Drawing.Point(466, 202);
            this.buscar.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.buscar.Name = "buscar";
            this.buscar.Size = new System.Drawing.Size(190, 25);
            this.buscar.TabIndex = 11;
            this.buscar.Text = "pagados/no pagos";
            this.buscar.Click += new System.EventHandler(this.label2_Click);
            // 
            // btn_buscar
            // 
            this.btn_buscar.Location = new System.Drawing.Point(968, 194);
            this.btn_buscar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_buscar.Name = "btn_buscar";
            this.btn_buscar.Size = new System.Drawing.Size(104, 44);
            this.btn_buscar.TabIndex = 13;
            this.btn_buscar.Text = "buscar";
            this.btn_buscar.UseVisualStyleBackColor = true;
            this.btn_buscar.Click += new System.EventHandler(this.btn_buscar_Click);
            // 
            // Formulario_Viajes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1359, 779);
            this.Controls.Add(this.btn_buscar);
            this.Controls.Add(this.txt_buscar);
            this.Controls.Add(this.buscar);
            this.Controls.Add(this.btn_cancelar);
            this.Controls.Add(this.Btn_actualizar);
            this.Controls.Add(this.Btn_eliminar);
            this.Controls.Add(this.Btn_salir);
            this.Controls.Add(this.Btn_nuevo);
            this.Controls.Add(this.btn_agregar);
            this.Controls.Add(this.txt_pagado);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_monto);
            this.Controls.Add(this.cifraDeMonto);
            this.Controls.Add(this.sueldos);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Formulario_Viajes";
            this.Text = "Formulario_Viajes";
            this.Load += new System.EventHandler(this.Formulario_Viajes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sueldos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView sueldos;
        private System.Windows.Forms.Label cifraDeMonto;
        private System.Windows.Forms.TextBox txt_monto;
        private System.Windows.Forms.TextBox txt_pagado;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_agregar;
        private System.Windows.Forms.Button Btn_nuevo;
        private System.Windows.Forms.Button Btn_salir;
        private System.Windows.Forms.Button Btn_eliminar;
        private System.Windows.Forms.Button Btn_actualizar;
        private System.Windows.Forms.Button btn_cancelar;
        private System.Windows.Forms.TextBox txt_buscar;
        private System.Windows.Forms.Label buscar;
        private System.Windows.Forms.Button btn_buscar;
    }
}