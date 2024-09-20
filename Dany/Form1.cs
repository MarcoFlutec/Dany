using Microsoft.Data.SqlClient;
using Microsoft.Graph.Models;
using System.Data;

namespace Dany
{
    public partial class Form1 : Form
    {
        // Variable est�tica para contar el n�mero de m�quinas
        private static int maquinaCount = 0;
        // Variable est�tica para contar las columnas por cada m�quina
        private static int modCount = 0;

        int dias = 0;
        int fases = 0;



        public Form1()
        {
            InitializeComponent();
            guna2DataGridView1.CellValueChanged += guna2DataGridView1_CellValueChanged;

        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            // Alternar entre Mod1B y Mod2E
            string mod = (modCount % 2 == 0) ? "Mod1B" : "Mod2E";

            // Si modCount es 0, incrementa el contador de m�quina
            if (modCount % 2 == 0)
            {
                maquinaCount++;
            }

            // Incrementar modCount despu�s de agregar una columna
            modCount++;

            // Crear una nueva columna
            DataGridViewTextBoxColumn nuevaColumna = new DataGridViewTextBoxColumn();

            // Configurar las propiedades de la columna
            nuevaColumna.HeaderText = "Maquina " + maquinaCount + " " + mod; // T�tulo de la columna
            nuevaColumna.Name = "Maquina" + maquinaCount + " " + mod;       // Nombre de la columna (para referencia)
            nuevaColumna.ReadOnly = false;                                 // Definir si es de solo lectura o editable

            // Agregar la columna al DataGridView
            guna2DataGridView1.Columns.Add(nuevaColumna);
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            // Verificar si hay columnas en el DataGridView
            if (guna2DataGridView1.Columns.Count > 0)
            {
                // Remover la �ltima columna
                int lastIndex = guna2DataGridView1.Columns.Count - 1;
                string columnName = guna2DataGridView1.Columns[lastIndex].Name;

                // Eliminar la columna
                guna2DataGridView1.Columns.RemoveAt(lastIndex);

                // Actualizar modCount y maquinaCount
                if (columnName.Contains("Mod1B"))
                {
                    modCount--;
                }
                else if (columnName.Contains("Mod2E"))
                {
                    modCount--;
                }

                // Ajustar el contador de m�quinas si necesario
                if (modCount % 2 == 0 && modCount > 0)
                {
                    maquinaCount--;
                }
            }
            else
            {
                MessageBox.Show("No hay columnas para eliminar.");
            }
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            // Crear una instancia de la clase de conexi�n
            conexion cnn = new conexion();

            // Definir la consulta SQL para obtener todas las columnas de la tabla
            string query = "SELECT * FROM [Dany].[dbo].[Fases]";

            // Crear un DataTable para almacenar los datos
            DataTable dt = new DataTable();

            // Usar la conexi�n a la base de datos para ejecutar la consulta
            using (SqlConnection connection = cnn.GetConnection())
            {
                // Crear un SqlDataAdapter para llenar el DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                // Llenar el DataTable con los datos de la consulta
                adapter.Fill(dt);
            }

            // Cerrar la conexi�n
            cnn.CloseConnection();

            // Limpiar las columnas existentes en el DataGridView
            guna2DataGridView1.Columns.Clear();

            // Agregar las columnas del DataTable al DataGridView
            foreach (DataColumn column in dt.Columns)
            {
                DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn
                {
                    HeaderText = column.ColumnName,
                    Name = column.ColumnName,
                    DataPropertyName = column.ColumnName // Asegura que la columna se vincule con el DataTable
                };
                guna2DataGridView1.Columns.Add(dgvColumn);
            }

            // Configurar el DataGridView con los datos
            guna2DataGridView1.DataSource = dt;

            // Opcional: Renombrar las columnas si se necesita
            guna2DataGridView1.Columns["Fase"].HeaderText = "Fases";
            guna2DataGridView1.Columns["Dias"].HeaderText = "Dias";
            guna2DataGridView1.Columns["Nombre de la Fase"].HeaderText = "Fase";
            guna2DataGridView1.Columns["Sub Ensambles"].HeaderText = "Sub Ensamble";
        }


        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {
            // Obtener el n�mero del TextBox
            if (!int.TryParse(guna2TextBox1.Text, out int numero))
            {
                MessageBox.Show("Por favor, ingresa un n�mero v�lido.");
                return;
            }

            // Mostrar el formulario personalizado
            OpcionAnidarAgregarForm form = new OpcionAnidarAgregarForm();
            var result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                string opcion = form.OpcionSeleccionada;

                // Obtener el DataTable que alimenta el DataGridView
                DataTable dt = (DataTable)guna2DataGridView1.DataSource;

                if (opcion == "Anidar") // Anidar
                {
                    // Encontrar la primera aparici�n del n�mero en la tabla
                    int primeraPosicion = -1;
                    for (int i = 0; i < guna2DataGridView1.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(guna2DataGridView1.Rows[i].Cells["Fase"].Value) == numero)
                        {
                            primeraPosicion = i;
                            break;
                        }
                    }

                    if (primeraPosicion == -1)
                    {
                        MessageBox.Show($"El n�mero {numero} no se encuentra en la lista.");
                        return;
                    }

                    // Informar la posici�n de la primera aparici�n del n�mero
                    MessageBox.Show($"El n�mero {numero} se encuentra por primera vez en la posici�n {primeraPosicion + 1}.");

                    // Preguntar la posici�n en la que se desea anidar el n�mero
                    string inputPosition = Microsoft.VisualBasic.Interaction.InputBox($"Ingrese la posici�n (relativa al primer {numero}) para anidar el n�mero (1 a {guna2DataGridView1.Rows.Count - primeraPosicion + 1}):", "Posici�n", "1");

                    if (!int.TryParse(inputPosition, out int relativePosition) || relativePosition < 1 || relativePosition > guna2DataGridView1.Rows.Count - primeraPosicion + 1)
                    {
                        MessageBox.Show("Posici�n no v�lida.");
                        return;
                    }

                    // Calcular la posici�n absoluta donde se va a insertar el n�mero
                    int posicionAbsoluta = primeraPosicion + relativePosition - 1;

                    // Insertar una nueva fila en la posici�n calculada
                    DataRow newRow = dt.NewRow();
                    newRow["Fase"] = numero;
                    dt.Rows.InsertAt(newRow, posicionAbsoluta);

                    // Actualizar el DataGridView (no reconstruimos toda la tabla, solo agregamos una fila)
                    guna2DataGridView1.DataSource = dt;
                }
                else if (opcion == "Agregar") // Agregar
                {
                    // Encontrar la primera posici�n donde el n�mero es igual o mayor al que se quiere agregar
                    int indexToInsert = -1;
                    for (int i = 0; i < guna2DataGridView1.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(guna2DataGridView1.Rows[i].Cells["Fase"].Value) >= numero)
                        {
                            indexToInsert = i;
                            break;
                        }
                    }

                    // Si no se encontr� una posici�n, agregar al final
                    if (indexToInsert == -1)
                    {
                        indexToInsert = guna2DataGridView1.Rows.Count;
                    }

                    // Insertar el n�mero en la posici�n encontrada
                    DataRow newRow = dt.NewRow();
                    newRow["Fase"] = numero;
                    dt.Rows.InsertAt(newRow, indexToInsert);

                    // Aumentar en uno los n�meros posteriores
                    for (int i = indexToInsert + 1; i < dt.Rows.Count; i++)
                    {
                        int valorActual = Convert.ToInt32(dt.Rows[i]["Fase"]);
                        dt.Rows[i]["Fase"] = valorActual + 1;
                    }

                    // Actualizar el DataGridView
                    guna2DataGridView1.DataSource = dt;
                }
            }
        }

        private void ActualizarBD()
        {
            conexion cnn = new conexion();
            using (SqlConnection connection = cnn.GetConnection())
            {
                // Verificar si la conexi�n est� cerrada antes de intentar abrirla
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                // Obtener las columnas actuales de la base de datos
                string queryObtenerColumnas = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Fases'";
                SqlCommand cmdObtenerColumnas = new SqlCommand(queryObtenerColumnas, connection);
                SqlDataReader reader = cmdObtenerColumnas.ExecuteReader();

                List<string> columnasBD = new List<string>();
                while (reader.Read())
                {
                    columnasBD.Add(reader.GetString(0));  // Agregar nombre de la columna
                }
                reader.Close();

                // Lista de columnas en el DataGridView
                List<string> columnasDGV = guna2DataGridView1.Columns
                    .Cast<DataGridViewColumn>()
                    .Select(c => c.Name)
                    .ToList();

                // Determinar las columnas que est�n en el DataGridView pero no en la BD (Nuevas columnas)
                var columnasParaAgregar = columnasDGV.Except(columnasBD).ToList();

                // Determinar las columnas que est�n en la BD pero no en el DataGridView (Columnas para eliminar)
                var columnasParaEliminar = columnasBD.Except(columnasDGV).ToList();

                // Agregar columnas nuevas a la base de datos
                foreach (var columna in columnasParaAgregar)
                {
                    string queryAgregarColumna = $"ALTER TABLE [Dany].[dbo].[Fases] ADD [{columna}] NVARCHAR(MAX)";
                    SqlCommand cmdAgregarColumna = new SqlCommand(queryAgregarColumna, connection);
                    cmdAgregarColumna.ExecuteNonQuery();
                }

                // Eliminar columnas que ya no est�n en el DataGridView de la base de datos
                foreach (var columna in columnasParaEliminar)
                {
                    string queryEliminarColumna = $"ALTER TABLE [Dany].[dbo].[Fases] DROP COLUMN [{columna}]";
                    SqlCommand cmdEliminarColumna = new SqlCommand(queryEliminarColumna, connection);
                    cmdEliminarColumna.ExecuteNonQuery();
                }
            }
        }



        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            ActualizarBD();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Desactivar temporalmente el evento para evitar llamadas recursivas
            guna2DataGridView1.CellValueChanged -= guna2DataGridView1_CellValueChanged;

            try
            {
                // Verifica si la celda editada pertenece a una columna "Maquina"
                if (guna2DataGridView1.Columns[e.ColumnIndex].HeaderText.Contains("Maquina") &&
                    DateTime.TryParse(guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString(), out DateTime fechaInicial))
                {
                    // Variable para rastrear la fecha actual (comienza con la fecha ingresada)
                    DateTime fechaActual = fechaInicial;

                    //////////Aqui inicia el ciclo y se supone que se hace por maquina 

                    // Iterar sobre las columnas de las m�quinas (m�ltiples columnas de m�quinas)
                    for (int colIndex = e.ColumnIndex; colIndex < guna2DataGridView1.ColumnCount; colIndex++)
                    {
                        if (guna2DataGridView1.Columns[colIndex].HeaderText.Contains("Maquina"))
                        {
                            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                            {                            
                                foreach (DataGridViewColumn col in guna2DataGridView1.Columns)
                                {
                                    if (col.HeaderText == "Dias" && int.TryParse(row.Cells[col.Index].Value?.ToString(), out dias))
                                    {
                                        // Fase es opcional y se puede omitir si no es necesaria
                                    }
                                    if (col.HeaderText == "Fases" && int.TryParse(row.Cells[col.Index].Value?.ToString(), out fases))
                                    {
                                        // Fase le�da exitosamente
                                    }
                                }
                                ///////////Tengo que checar de aqui a 
                                // Si no es la primera m�quina, obtener la maquina anterior
                                if (!guna2DataGridView1.Columns[colIndex].HeaderText.Contains("Maquina 1 Mod1B"))
                                {
                                    MessageBox.Show("no soy la primera maquina");
                                    MessageBox.Show("Estoy en la fase: " + fases.ToString());
                                    MessageBox.Show("Tengo una duracion de:" + dias.ToString() + "dias");
                                    var colAnterior = colIndex - 1;

                                    // Verificar si la columna anterior contiene el nombre de m�quina
                                    if (guna2DataGridView1.Columns[colIndex].HeaderText.Contains("Maquina"))
                                    {

                                        // Iterar para ver cu�l es el �ltimo rengl�n de la primera fase

                                        DataGridViewRow lastPhaseRow = null;

                                        // Aqui en este for se empieza a iterar 
                                        
                                        //defino desde cual index iniciare a contar las filas
                                        for (int rowIndex = 1; rowIndex < guna2DataGridView1.Rows.Count; rowIndex++)
                                        {    //r va a ser el index normal 
                                            DataGridViewRow r = guna2DataGridView1.Rows[rowIndex];
                                            //previous row va a ser una fila menos que el index normal
                                            DataGridViewRow previousRow = guna2DataGridView1.Rows[rowIndex - 1];

                                            foreach (DataGridViewColumn col in guna2DataGridView1.Columns)
                                            {
                                                if (col.HeaderText == "Fases" &&
                                                    int.TryParse(previousRow.Cells[col.Index].Value?.ToString(), out int fasesPrev) &&
                                                    int.TryParse(r.Cells[col.Index].Value?.ToString(), out int fasesCurrent) &&
                                                    fasesCurrent > fasesPrev)
                                                {
                                                    MessageBox.Show("Esta es la current fase"+fasesCurrent);
                                                    MessageBox.Show("Esta es la previous fase" + fases);

                                                    // Aqu� puedes realizar la acci�n que necesitas con 'fases'
                                                    lastPhaseRow = previousRow; // Guarda la �ltima fila donde se encontr� un valor de fases
                                                    MessageBox.Show("Hasta aqui se acaba la fase:" + fases + "En el renglon: " + lastPhaseRow);

                                                    if (lastPhaseRow != null && DateTime.TryParse(lastPhaseRow.Cells[colAnterior].Value?.ToString(), out DateTime fechaFinAnterior))
                                                    {
                                                        MessageBox.Show("Fecha fin anterior: " + fechaFinAnterior.ToString());
                                                        // La fecha actual ser� un d�a despu�s de la fecha de fin de la fase anterior
                                                        fechaActual = fechaFinAnterior.AddDays(1);
                                                        MessageBox.Show("Fecha que se supone deberia de ponerse:" + fechaActual);
                                                    }

                                                }

                                                // Llenar la celda con la fecha actual
                                                row.Cells[colIndex].Value = fechaActual.ToString("MM/dd/yyyy");

                                                // Ajustar la fecha actual sumando los d�as de la fase actual
                                                fechaActual = fechaActual.AddDays(dias);

                                                // Si la fase cambia, avanzamos un d�a m�s (ejemplo de c�mo ajustarlo)
                                                if (fases == 1) // Cambiar el valor de la fase seg�n tu l�gica
                                                {
                                                    fechaActual = fechaActual.AddDays(1);
                                                }

                                                MessageBox.Show($"Fecha despu�s de la fase: {fechaActual.ToString("MM/dd/yyyy")}");

                                            }

                                        }
                                        //////////////////Hasta aqui voy bien 
                                    }
                                }
                                ////////////////////////////////////hasta aqui 

                                // Llenar la celda con la fecha actual
                                row.Cells[colIndex].Value = fechaActual.ToString("MM/dd/yyyy");

                                // Ajustar la fecha actual sumando los d�as de la fase actual
                                fechaActual = fechaActual.AddDays(dias);

                                // Si la fase cambia, avanzamos un d�a m�s (ejemplo de c�mo ajustarlo)
                                if (fases == 1) // Cambiar el valor de la fase seg�n tu l�gica
                                {
                                    fechaActual = fechaActual.AddDays(1);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                // Volver a habilitar el evento
                guna2DataGridView1.CellValueChanged += guna2DataGridView1_CellValueChanged;
            }
        }



        private void guna2DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Este evento se activa cuando se agregan nuevas filas
            // Verifica si hay fechas en la columna de "Maquina" y actualiza las fechas autom�ticamente
            UpdateMachineDates();
        }

        private void UpdateMachineDates()
        {
            // Variable para rastrear la fecha de fin de la �ltima fase en la columna anterior (si existe)
            DateTime fechaAnterior = DateTime.MinValue;

            // Iterar sobre las columnas para encontrar las columnas de m�quinas
            for (int colIndex = 0; colIndex < guna2DataGridView1.ColumnCount; colIndex++)
            {
                if (guna2DataGridView1.Columns[colIndex].HeaderText.Contains("Maquina"))
                {
                    // Iterar sobre las filas para llenar fechas basadas en la columna "Fases" y "D�as"
                    foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                    {
                        if (int.TryParse(row.Cells["Dias"].Value?.ToString(), out int dias) &&
                            int.TryParse(row.Cells["Fases"].Value?.ToString(), out int fase)) 
                        {
                            // Si estamos en la primera columna de m�quina, usamos la fecha almacenada
                            if (colIndex == 0)
                            {
                                // Si la celda ya tiene una fecha v�lida, la usamos como punto de partida
                                if (DateTime.TryParse(row.Cells[colIndex].Value?.ToString(), out DateTime fechaInicial))
                                {
                                    fechaAnterior = fechaInicial;
                                }
                                else
                                {
                                    // Si no hay fecha, usar la fecha actual o alguna fecha base
                                    fechaAnterior = DateTime.Now;
                                    row.Cells[colIndex].Value = fechaAnterior.ToString("MM/DD/YYYY");
                                }
                            }
                            else
                            {
                                // Si no es la primera columna, ajustamos la fecha con base en la columna anterior
                                var colAnterior = colIndex - 1;
                                if (DateTime.TryParse(row.Cells[colAnterior].Value?.ToString(), out DateTime fechaFinAnterior))
                                {
                                    // La nueva fecha ser� el d�a despu�s de la �ltima fecha en la columna anterior
                                    fechaAnterior = fechaFinAnterior.AddDays(1);
                                }

                                // Llenar la celda actual con la nueva fecha ajustada
                                row.Cells[colIndex].Value = fechaAnterior.ToString("MM/dd/yyyy");
                            }

                            // Ajustar la fecha actual sumando los d�as de la fase actual
                            fechaAnterior = fechaAnterior.AddDays(dias);
                        }
                    }
                }
            }
        }






    }
}

