using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SQLite;
using System.Data;


namespace GestorDeCitas
{
    public partial class InicioResumen : Form
    {
        private string connectionString, servicio, Nombre, telefono, NombreC, ApellidoC, TelefonoC;
        private readonly DataTable dt = new DataTable("Citas");
        private readonly DataTable dt2 = new DataTable("Servicios");
        private readonly DataTable dt3 = new DataTable("Trabajadores");
        private readonly DataTable dt4 = new DataTable("Clientes");

        public InicioResumen()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblhora.Text = DateTime.Now.ToString("hh:mm:ss ");
            lblFecha.Text = DateTime.Now.ToLongDateString();
        }

        private void InicioResumen_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        //Load all the data from database to all datagridviews upon loading the form
        private void LoadData()
        {
            string query = "SELECT Cl.Nombre, Cl.Apellido, Cl.Telefono, Ci.Servicio, Ci.Hora FROM Clientes AS Cl INNER JOIN Citas AS Ci ON Ci.id_cliente = Cl.Id WHERE Fecha = @Fecha ORDER BY Cl.Nombre";
            string query2 = "SELECT Servicio FROM Servicios";
            string query3 = "SELECT Nombre, Telefono FROM Trabajadores";
            string query4 = "SELECT Nombre, Apellido, Telefono FROM Clientes";

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            using (SQLiteCommand cmd = new SQLiteCommand(query, con))
            using (SQLiteCommand cmd2 = new SQLiteCommand(query2, con))
            using (SQLiteCommand cmd3 = new SQLiteCommand(query3, con))
            using (SQLiteCommand cmd4 = new SQLiteCommand(query4, con))
            {
                cmd.Parameters.Add(new SQLiteParameter("@Fecha", DateTime.Now.ToLongDateString()));

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                using (SQLiteDataAdapter adapter2 = new SQLiteDataAdapter(cmd2))
                using (SQLiteDataAdapter adapter3 = new SQLiteDataAdapter(cmd3))
                using (SQLiteDataAdapter adapter4 = new SQLiteDataAdapter(cmd4))
                {
                    adapter.Fill(dt);
                    adapter2.Fill(dt2);
                    adapter3.Fill(dt3);
                    adapter4.Fill(dt4);

                    CitasDeHoy.DataSource = dt;
                    Servicios.DataSource = dt2;
                    Estilistas.DataSource = dt3;
                    dataGridView1.DataSource = dt4;
                }
            }

            CitasDeHoy.AutoResizeColumns();
            CitasDeHoy.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        //Clear the selection of the datagridviews at the moment that the datagridview finishes the databinding to avoid information deleted by accident
        private void ClearSelections()
        {
            Servicios.ClearSelection();
            Estilistas.ClearSelection();
            dataGridView1.ClearSelection();
            CitasDeHoy.ClearSelection();
        }

        private void Servicios_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ClearSelections();
        }

        private void Estilistas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ClearSelections();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ClearSelections();
        }

        private void CitasDeHoy_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ClearSelections();
        }

        private void Estilistas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Nombre = Estilistas.Rows[e.RowIndex].Cells[0].Value.ToString();
            telefono = Estilistas.Rows[e.RowIndex].Cells[1].Value.ToString();
        }
        //Function to delete data from database ,recieves the target table and the condition WHERE
        private void DeleteRecord(string tableName, string condition)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                using (SQLiteCommand deleteRecord = new SQLiteCommand($"DELETE FROM {tableName} WHERE {condition}", con))
                {
                    con.Open();
                    deleteRecord.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Delete Clientes record
        private void metroSetButton3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("No hay un Cliente seleccionado");
            }
            else
            {
                string condition = $"Nombre = '{NombreC}' AND Apellido = '{ApellidoC}' AND Telefono = '{TelefonoC}'";
                DeleteRecord("Clientes", condition);
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                dataGridView1.Rows.RemoveAt(selectedIndex);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            NombreC = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            ApellidoC = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            TelefonoC = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
        }
        //Delete servicios record
        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            if (Servicios.SelectedRows.Count == 0)
            {
                MessageBox.Show("No hay un servicio seleccionado");
            }
            else
            {
                string condition = $"Servicio = '{servicio}'";
                DeleteRecord("Servicios", condition);
                int selectedIndex = Servicios.SelectedRows[0].Index;
                Servicios.Rows.RemoveAt(selectedIndex);
            }
        }
        //Delete Estilistas record
        private void metroSetButton2_Click(object sender, EventArgs e)
        {
            if (Estilistas.SelectedRows.Count == 0)
            {
                MessageBox.Show("No hay un Estilista seleccionado");
            }
            else
            {
                string condition = $"Nombre = '{Nombre}' AND Telefono = '{telefono}'";
                DeleteRecord("Trabajadores", condition);
                int selectedIndex = Estilistas.SelectedRows[0].Index;
                Estilistas.Rows.RemoveAt(selectedIndex);
            }
        }

        private void Servicios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.servicio = Servicios.Rows[e.RowIndex].Cells[0].Value.ToString();
        }
    }
}
