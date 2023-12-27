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
        //Declaration of connection path
        private readonly string connectionString;

        public AgregarServicio()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        }
        //Get Total Workers in Trabajadores table and returned as Integer to control the IDs of table when inserting new data
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
        //Insert Servicio in Service table 
        private void InsertService()
        {
            //Validate fields
            if (serviciotext.Text == "")
            {
                MessageBox.Show("Falta un campo por llenar");
                return;
            }

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                using (SQLiteCommand cmd = new SQLiteCommand(@"INSERT INTO Servicios (Servicio) VALUES (@Servicio)", con))

                {
                    con.Open();
                    cmd.Parameters.Add(new SQLiteParameter("@Servicio", serviciotext.Text));
                    //If return 1 the query executed sucessfully
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        MessageBox.Show("Servicio agregado correctamente");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //Clear the field
            serviciotext.Text = "";
        }
        //Insert Workers in trabajadores table
        private void InsertWorker()
        {
            //Validate Fields
            if (nombretext.Text == "" || telefonotext.Text == "")
            {
                MessageBox.Show("Falta un campo por llenar");
                return;
            }

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                using (SQLiteCommand cmd = new SQLiteCommand(@"INSERT INTO Trabajadores (id, Nombre, Telefono) VALUES (@id, @Nombre, @Telefono)", con))
                {
                    con.Open();

                    cmd.Parameters.Add(new SQLiteParameter("@id", GetTotalWorkers()+1));
                    cmd.Parameters.Add(new SQLiteParameter("@Nombre", nombretext.Text));
                    cmd.Parameters.Add(new SQLiteParameter("@Telefono", telefonotext.Text));

                    //If return 1 the query executed sucessfully
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        MessageBox.Show("Trabajador agregado correctamente");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //Clear fields 
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
        }
    }
}