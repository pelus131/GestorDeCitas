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
        string connectionString,servicio,Nombre,telefono,NombreC,ApellidoC,TelefonoC;
        string fecha = DateTime.Now.ToLongDateString();


        DataTable dt = new DataTable("Citas");
        DataTable dt2 = new DataTable("Servicios");
        DataTable dt3 = new DataTable("Trabajadores");
        DataTable dt4 = new DataTable("Clientes");


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


        


        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void InicioResumen_Load(object sender, EventArgs e)
        {
            string  query = "SELECT Cl.Nombre,Cl.Apellido,Cl.Telefono,Ci.Servicio,Ci.Hora FROM Clientes  AS Cl INNER JOIN Citas AS Ci ON Ci.id_cliente = Cl.Id WHERE Fecha = @Fecha ORDER BY Cl.Nombre  ";
            string query2 = "Select Servicio FROM Servicios";
            string query3 = "Select Nombre,Telefono FROM Trabajadores";
            string query4 = "Select Nombre,Apellido,Telefono FROM Clientes";
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            using (SQLiteCommand cmd = new SQLiteCommand(query, con))
            using (SQLiteCommand cmd2 = new SQLiteCommand(query2,con))  
            using (SQLiteCommand cmd3 = new SQLiteCommand(query3,con))    
            using (SQLiteCommand cmd4 = new SQLiteCommand(query4,con))

            {
                cmd.Parameters.Add(new SQLiteParameter("@Fecha", fecha));

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

                    con.Close();

                }




            }

            CitasDeHoy.AutoResizeColumns();
            CitasDeHoy.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void Servicios_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
         
            Servicios.ClearSelection();
        }

        private void Estilistas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Estilistas.ClearSelection();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.ClearSelection();

        }

        private void CitasDeHoy_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            CitasDeHoy.ClearSelection();
        }

        private void Estilistas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.Nombre = Estilistas.Rows[e.RowIndex].Cells[0].Value.ToString();
            this.telefono = Estilistas.Rows[e.RowIndex].Cells[1].Value.ToString();


        }

        private void metroSetButton3_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("No hay un Cliente seleccionado");
            }
            else
            {
                try
                {

                    string sql = "DELETE FROM Clientes WHERE Nombre = @Nombre AND Apellido = @Apellido AND Telefono = @Telefono";

                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    using (SQLiteCommand deleteRecord = new SQLiteCommand(sql, con))
                    {
                        con.Open();

                        deleteRecord.Parameters.Add(new SQLiteParameter("@Nombre", NombreC));
                        deleteRecord.Parameters.Add(new SQLiteParameter("@Apellido", ApellidoC));
                        deleteRecord.Parameters.Add(new SQLiteParameter("@Telefono", TelefonoC));





                        deleteRecord.ExecuteNonQuery();

                        int selectedIndex = dataGridView1.SelectedRows[0].Index;

                        dataGridView1.Rows.RemoveAt(selectedIndex);
                        con.Close();
                        con.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.NombreC = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            this.ApellidoC = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            this.TelefonoC = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            if (Servicios.SelectedRows.Count == 0)
            {
                MessageBox.Show("No hay un servicio seleccionado");
            }
            else
            {
                try
                {

                    string sql = "DELETE FROM Servicios WHERE Servicio = @Servicio ";

                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    using (SQLiteCommand deleteRecord = new SQLiteCommand(sql, con))
                    {
                        con.Open();

                        deleteRecord.Parameters.Add(new SQLiteParameter("@Servicio", servicio));
                        


                        deleteRecord.ExecuteNonQuery();

                        int selectedIndex = Servicios.SelectedRows[0].Index;

                        Servicios.Rows.RemoveAt(selectedIndex);
                        con.Close();
                        con.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void metroSetButton2_Click(object sender, EventArgs e)
        {
            if (Estilistas.SelectedRows.Count == 0)
            {
                MessageBox.Show("No hay un Estilista seleccionado");
            }
            else
            {
                try
                {

                    string sql = "DELETE FROM Trabajadores WHERE Nombre = @Nombre AND Telefono = @Telefono  ";

                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    using (SQLiteCommand deleteRecord = new SQLiteCommand(sql, con))
                    {
                        con.Open();

                        deleteRecord.Parameters.Add(new SQLiteParameter("@Nombre", Nombre));
                        deleteRecord.Parameters.Add(new SQLiteParameter("@Telefono", telefono));



                        deleteRecord.ExecuteNonQuery();

                        int selectedIndex = Estilistas.SelectedRows[0].Index;

                        Estilistas.Rows.RemoveAt(selectedIndex);
                        con.Close();
                        con.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Servicios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.servicio= Servicios.Rows[e.RowIndex].Cells[0].Value.ToString();
        }
    }
}

