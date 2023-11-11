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
    public partial class AgregarServicio : MetroSetForm
    {
        private readonly string connectionString;
        private int workerId = 0;

        public AgregarServicio()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        }

        private int GetTotalWorkers()
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            using (SQLiteCommand count = new SQLiteCommand("SELECT COUNT(id) FROM Trabajadores", con))
            {
                con.Open();
                int totalWorkers = Convert.ToInt32(count.ExecuteScalar());
                return totalWorkers;
            }
        }

        private void InsertService()
        {
            if (serviciotext.Text == "")
            {
                MessageBox.Show("Falta un campo por llenar");
                return;
            }

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(@"INSERT INTO Servicios (Servicio) VALUES (@Servicio)", con))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@Servicio", serviciotext.Text));

                        int i = cmd.ExecuteNonQuery();
                        if (i == 1)
                        {
                            MessageBox.Show("Servicio agregado correctamente");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            serviciotext.Text = "";
        }

        private void InsertWorker()
        {
            if (nombretext.Text == "" || telefonotext.Text == "")
            {
                MessageBox.Show("Falta un campo por llenar");
                return;
            }

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(@"INSERT INTO Trabajadores (id, Nombre, Telefono) VALUES (@id, @Nombre, @Telefono)", con))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@id", workerId));
                        cmd.Parameters.Add(new SQLiteParameter("@Nombre", nombretext.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@Telefono", telefonotext.Text));

                        int i = cmd.ExecuteNonQuery();
                        if (i == 1)
                        {
                            MessageBox.Show("Trabajador agregado correctamente");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            workerId++;
            nombretext.Text = "";
            telefonotext.Text = "";
        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            InsertService();
        }

        private void metroSetButton2_Click(object sender, EventArgs e)
        {
            InsertWorker();
        }

        private void AgregarServicio_Load(object sender, EventArgs e)
        {
            workerId = GetTotalWorkers();
        }
    }
}