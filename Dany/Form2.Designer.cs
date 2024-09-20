using System;
using System.Windows.Forms;

namespace Dany
{
    public partial class OpcionAnidarAgregarForm : Form
    {
        // Declaramos los botones
        private Button btnAnidar;
        private Button btnAgregar;

        public string OpcionSeleccionada { get; private set; }

        public OpcionAnidarAgregarForm()
        {
            InitializeComponent();  // Asegúrate de que esta función se llame para inicializar los componentes
        }

        private void InitializeComponent()
        {
            // Inicializamos los controles manualmente
            this.btnAnidar = new Button();
            this.btnAgregar = new Button();

            // Configuración del botón Anidar
            this.btnAnidar.Location = new System.Drawing.Point(50, 50);
            this.btnAnidar.Name = "btnAnidar";
            this.btnAnidar.Size = new System.Drawing.Size(75, 23);
            this.btnAnidar.TabIndex = 0;
            this.btnAnidar.Text = "Anidar";
            this.btnAnidar.UseVisualStyleBackColor = true;
            this.btnAnidar.Click += new System.EventHandler(this.btnAnidar_Click);  // Asignamos el evento Click

            // Configuración del botón Agregar
            this.btnAgregar.Location = new System.Drawing.Point(150, 50);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(75, 23);
            this.btnAgregar.TabIndex = 1;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);  // Asignamos el evento Click

            // Configuración del formulario
            this.ClientSize = new System.Drawing.Size(300, 150);
            this.Controls.Add(this.btnAnidar);
            this.Controls.Add(this.btnAgregar);
            this.Name = "OpcionAnidarAgregarForm";
            this.Text = "Elija una opción";
        }

        // Manejadores de eventos para los botones
        private void btnAnidar_Click(object sender, EventArgs e)
        {
            OpcionSeleccionada = "Anidar";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            OpcionSeleccionada = "Agregar";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
