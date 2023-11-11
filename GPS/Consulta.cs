using MetroSet_UI.Forms;
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

namespace GestorDeCitas
{
    public partial class Consulta : MetroSetForm
    {
        private string control, Fecha, Hora, estilista;
        private readonly string connectionString;
        private readonly DataTable dt = new DataTable("Citas");

        public Consulta()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void Consulta_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            string query = "SELECT Cl.Nombre, Cl.Apellido, Cl.Telefono, Ci.Servicio, Ci.Fecha, Ci.Hora, Ci.Estilista FROM Clientes AS Cl INNER JOIN Citas AS Ci ON Ci.id_cliente = Cl.Id ORDER BY Cl.Nombre";
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, con))
                {
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }

            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeleteSelectedRecord();
        }

        private void DeleteSelectedRecord()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("No hay una cita seleccionada");
                return;
            }

            try
            {
                string sql = "DELETE FROM Citas WHERE Servicio = @Servicio AND Fecha = @Fecha AND Hora = @Hora AND Estilista = @Estilista ";

                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                using (SQLiteCommand deleteRecord = new SQLiteCommand(sql, con))
                {
                    con.Open();

                    deleteRecord.Parameters.Add(new SQLiteParameter("@Servicio", control));
                    deleteRecord.Parameters.Add(new SQLiteParameter("@Fecha", Fecha));
                    deleteRecord.Parameters.Add(new SQLiteParameter("@Hora", Hora));
                    deleteRecord.Parameters.Add(new SQLiteParameter("@Estilista", estilista));

                    deleteRecord.ExecuteNonQuery();

                    int selectedIndex = dataGridView1.SelectedRows[0].Index;
                    dataGridView1.Rows.RemoveAt(selectedIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.Fecha = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                this.Hora = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                this.estilista = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                this.control = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}