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

        string connectionString;
        int er = 0;
        public AgregarServicio()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

        }
        private void update()
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            using (SQLiteCommand count = new SQLiteCommand("SELECT count (id) from Trabajadores as total", con))
            {
                con.Open();
                SQLiteDataReader qw = count.ExecuteReader();
                qw.Read();
                int qd = qw.GetInt32(0);
                this.er = qd + 1;

                count.Dispose();
                con.Close();

            }
        }
        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            if (serviciotext.Text == "")
            {
                MessageBox.Show("Falta un campo por llenar");
            }
            else
            {
                try
                {

                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    {
                        SQLiteCommand cmd = new SQLiteCommand();

                        cmd.CommandText = @"INSERT INTO Servicios (Servicio) VALUES (@Servicio)";
                        cmd.Connection = con;
                        cmd.Parameters.Add(new SQLiteParameter("@Servicio", serviciotext.Text));

                        con.Open();

                        int i = cmd.ExecuteNonQuery();
                        if (i == 1)

                        {
                            MessageBox.Show("Servicio agregado correctamente");
                        }
                        con.Close();
                        con.Dispose();



                    }




                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }
                serviciotext.Text = "";
            }

        }

        private void metroSetLabel2_Click(object sender, EventArgs e)
        {

        }

        private void metroSetButton2_Click(object sender, EventArgs e)
        {
            if (nombretext.Text == "" || telefonotext.Text == "")
            {
                MessageBox.Show("Falta un campo por llenar");
            }
            else
            {

                try
                {


                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    {
                        SQLiteCommand cmd = new SQLiteCommand();

                        cmd.CommandText = @"INSERT INTO Trabajadores (id,Nombre,Telefono) VALUES (@id,@Nombre,@Telefono)";
                        cmd.Connection = con;
                        cmd.Parameters.Add(new SQLiteParameter("@id", er));
                        cmd.Parameters.Add(new SQLiteParameter("@Nombre", nombretext.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@Telefono", telefonotext.Text));

                        con.Open();

                        int i = cmd.ExecuteNonQuery();
                        if (i == 1)

                        {
                            MessageBox.Show("Trabajador agregado correctamente");
                        }
                        con.Close();
                        con.Dispose();



                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }

                er++;
                nombretext.Text = "";
                telefonotext.Text = "";
            }
        }

        private void AgregarServicio_Load(object sender, EventArgs e)
        {
            update();
        }

        
    }
}
