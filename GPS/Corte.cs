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
    public partial class Corte : MetroSetForm
    {
        private string connectionString, servicio, hora;
        private string fecha = DateTime.Now.ToLongDateString();
        private DataTable dt = new DataTable("Citas");
        private int controlventa;

        public Corte()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        }

        private void llenargrid()
        {
            string query = "SELECT Cl.Nombre, Cl.Apellido, Ci.Servicio, Ci.Hora FROM Clientes AS Cl INNER JOIN Citas AS Ci ON Ci.id_cliente = Cl.Id WHERE Fecha = @Fecha ORDER BY Cl.Nombre";

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            using (SQLiteCommand cmd = new SQLiteCommand(query, con))
            {
                cmd.Parameters.Add(new SQLiteParameter("@Fecha", fecha));

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }

            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void metroSetButton3_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            controlventa = 0;
            metroSetLabel1.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.servicio = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            this.hora = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void metroSetButton2_Click(object sender, EventArgs e)
        {
            int rows = dataGridView2.Rows.Count;

            if (rows > 0)
            {
                try
                {
                    string sql = "DELETE FROM Citas WHERE Fecha = @Fecha AND Servicio = @Servicio AND Hora = @Hora";

                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    using (SQLiteCommand deleteRecord = new SQLiteCommand(sql, con))
                    {
                        con.Open();

                        deleteRecord.Parameters.Add(new SQLiteParameter("@Servicio", servicio));
                        deleteRecord.Parameters.Add(new SQLiteParameter("@Hora", hora));
                        deleteRecord.Parameters.Add(new SQLiteParameter("@Fecha", fecha));

                        deleteRecord.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                dt.Clear();

                string query = "INSERT INTO Corte (Fecha, Total) VALUES (@Fecha, @Total)";

                try
                {
                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@Fecha", fecha));
                        cmd.Parameters.Add(new SQLiteParameter("@Total", controlventa));

                        con.Open();

                        int i = cmd.ExecuteNonQuery();
                        if (i == 1)
                        {
                            MessageBox.Show("Registrado con Éxito");
                        }

                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No hay registros");
            }
        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.controlventa += int.Parse(metroSetTextBox1.Text);
                dataGridView2.Rows.Add(servicio, metroSetTextBox1.Text);

                metroSetTextBox1.Text = "";
                metroSetLabel1.Text = "TOTAL = " + controlventa;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Selecciona una cita para procesar");
            }
        }

        private void Corte_Load(object sender, EventArgs e)
        {
            llenargrid();
        }
    }
}

