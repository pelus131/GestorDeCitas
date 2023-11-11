using MetroSet_UI.Controls;
using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace GestorDeCitas
{
    public partial class RegistrarCita : MetroSetForm
    {
        private string connectionString;
        private int er = 0;
        private int idClientSelected = 0;

        public RegistrarCita()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        }

        private void update()
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            using (SQLiteCommand count = new SQLiteCommand("SELECT COUNT(Id) FROM Clientes", con))
            {
                con.Open();
                er = Convert.ToInt32(count.ExecuteScalar()) + 1;
            }
        }

        private void fillcombo(string tableName, MetroSetComboBox comboBox)
        {
            comboBox.Items.Clear();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            using (SQLiteCommand cmd = new SQLiteCommand($"SELECT Nombre FROM {tableName}", conn))
            {
                conn.Open();
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        comboBox.Items.Add(dr["Nombre"]);
                    }
                }
            }
        }

        private void fillcheck()
        {
            DataTable dt = new DataTable();

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                string query = "SELECT Servicio FROM Servicios";

                SQLiteCommand cmd = new SQLiteCommand(query, con);

                using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                {
                    da.Fill(dt);
                    checkedListBox1.DataSource = dt;
                    checkedListBox1.DisplayMember = "Servicio";
                }
            }
        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            if (TextboxClienteNombre.Text == "" || textboxApellido.Text == "" || TextboxNumero.Text == "")
            {
                MessageBox.Show("Falta un campo por llenar");
            }
            else
            {
                try
                {
                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    {
                        using (SQLiteCommand cmc = new SQLiteCommand(@"INSERT INTO Clientes (Id, Nombre, Apellido, Telefono) VALUES (@id, @Nombre, @Apellido, @Telefono)", con))
                        {
                            con.Open();

                            cmc.Parameters.Add(new SQLiteParameter("@id", metroSetTextBox1.Text));
                            cmc.Parameters.Add(new SQLiteParameter("@Nombre", TextboxClienteNombre.Text));
                            cmc.Parameters.Add(new SQLiteParameter("@Apellido", textboxApellido.Text));
                            cmc.Parameters.Add(new SQLiteParameter("@Telefono", TextboxNumero.Text));

                            int i = cmc.ExecuteNonQuery();
                            if (i == 1)
                            {
                                MessageBox.Show("Cliente Registrado con Éxito");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                er++;
                metroSetTextBox1.Text = er.ToString();
                fillcombo("Clientes", metroSetComboBox1);
                TextboxClienteNombre.Text = "";
                textboxApellido.Text = "";
                TextboxNumero.Text = "";
            }
        }

        private void RegistrarCita_Load(object sender, EventArgs e)
        {
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "hh:mm tt";
            fillcombo("Clientes", metroSetComboBox1);
            fillcombo("Trabajadores", metroSetComboBox2);
            fillcheck();
            update();
            metroSetTextBox1.Text = er.ToString();
        }

        private void metroSetComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            idClientSelected = metroSetComboBox1.SelectedIndex + 1;
            metroSetTextBox2.Text = idClientSelected.ToString();
        }

        private void metroSetButton2_Click(object sender, EventArgs e)
        {
            if (metroSetComboBox1.GetItemText(metroSetComboBox1.SelectedItem) == "" || metroSetComboBox2.GetItemText(metroSetComboBox2.SelectedItem) == "" || checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Faltan campos por llenar");
            }
            else
            {
                string servicios = "";

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        servicios += ((DataRowView)checkedListBox1.Items[i])[0].ToString() + ",";
                    }
                }
                servicios = servicios.Remove(servicios.Length - 1);

                try
                {
                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand())
                        {
                            cmd.CommandText = @"INSERT INTO Citas (Servicio, id_cliente, Fecha, Hora, id_trabajador, Estilista) VALUES (@Servicio, @idcliente, @Fecha, @Hora, @idtrabajador, @Estilista)";
                            cmd.Connection = con;

                            cmd.Parameters.Add(new SQLiteParameter("@Servicio", servicios));
                            cmd.Parameters.Add(new SQLiteParameter("@idcliente", metroSetTextBox2.Text));
                            cmd.Parameters.Add(new SQLiteParameter("@Fecha", dateTimePicker1.Text));
                            cmd.Parameters.Add(new SQLiteParameter("@Hora", dateTimePicker2.Text));
                            cmd.Parameters.Add(new SQLiteParameter("@idtrabajador", metroSetComboBox2.SelectedIndex + 1));
                            cmd.Parameters.Add(new SQLiteParameter("@Estilista", metroSetComboBox2.Text));

                            con.Open();

                            int i = cmd.ExecuteNonQuery();
                            if (i == 1)
                            {
                                MessageBox.Show("Cita Registrada con Éxito");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fecha y Hora ya tienen una cita previa!");
                }
            }
        }
    }
}