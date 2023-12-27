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
    public partial class EditarCita : MetroSetForm
    {
        private string connectionString;
        private string controlid = "";

        public EditarCita()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        }
        //Fill combobox Cliente with data from database
        private void fillComboBoxCliente()
        {
            comboBox1.Items.Clear();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            using (SQLiteCommand cmd = new SQLiteCommand("SELECT Nombre FROM Clientes", conn))
            {
                conn.Open();
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        comboBox1.Items.Add(dr["Nombre"]);
                    }
                }
            }
        }
        //Fill FEcha, Hora and checkbox upon loading the form
        private void fillBaseForm()
        {
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "hh:mm tt";
            fillComboBoxCliente();

            DataTable dt = new DataTable();

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            using (SQLiteCommand cmd = new SQLiteCommand("Select Servicio FROM Servicios", con))
            using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
            {
                da.Fill(dt);
                checkedListBox1.DataSource = dt;
                checkedListBox1.DisplayMember = "Servicio";
            }
        }
        private void EditarCita_Load(object sender, EventArgs e)
        {
            fillBaseForm();
        }

        //Fill the Citas del Cliente combo box with data from database
        private void fillComboBoxCitasCliente()
        {
            comboBox2.Items.Clear();
            string query = "SELECT Servicio FROM Citas WHERE id_cliente = @idCliente"
            int control1 = comboBox1.SelectedIndex + 1;

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            using (SQLiteCommand cmd = new SQLiteCommand(query,conn))
            {
                conn.Open();
                cmd.Parameters.Add(new SQLiteParameter("@idCliente", control1));

                using (SQLiteDataReader sdr1 = cmd.ExecuteReader())
                {
                    while (sdr1.Read())
                    {
                        comboBox2.Items.Add(sdr1["Servicio"]);
                    }
                }
            }
        }
        //Save the edited data into the data base
        private void saveEditedData()
        {
            string servicios = "";
            //Convert the Checkbox checked elements  into String with a , to insert in database
            try
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        servicios += ((DataRowView)checkedListBox1.Items[i])[0].ToString() + ",";
                    }
                }
                //Delete the last ,
                servicios = servicios.Remove(servicios.Length - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //Validate fields
            if (checkedListBox1.CheckedItems.Count == 0 || comboBox1.GetItemText(comboBox1.SelectedItem) == "" || comboBox2.GetItemText(comboBox2.SelectedItem) == "")
            {
                MessageBox.Show("Falta un campo por llenar");
            }
            else
            {
                //Insert the data into database
                try
                {
                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    using (SQLiteCommand cmd = new SQLiteCommand($"UPDATE Citas SET Servicio=@Servicio, Fecha=@Fecha, Hora=@Hora WHERE id = '{controlid}'", con))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@Servicio", servicios));
                        cmd.Parameters.Add(new SQLiteParameter("@Fecha", dateTimePicker1.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@Hora", dateTimePicker2.Text));

                        con.Open();

                        int i = cmd.ExecuteNonQuery();
                        if (i == 1)
                        {
                            MessageBox.Show("Editado con Éxito");
                        }

                        con.Close();
                    }
                }
                //Catch the exeption from database. This exeption is showed thanks to a constraint in database that states no duplicated date and time 
                catch (Exception ex)
                {
                    MessageBox.Show("Fecha Y Hora Ya tiene una cita previa!");
                }

                fillComboBoxCitasCliente();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillComboBoxCitasCliente();
        }

        //When we select an element from Combobox Citas Del Cliente fill the Fecha,hora and Servicios actuales with data from the element selected
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string control = comboBox2.Text;

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            using (SQLiteCommand cmd = new SQLiteCommand($"SELECT id, Fecha, Hora FROM Citas WHERE Servicio = '{control}'", conn))
            {
                conn.Open();
                using (SQLiteDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        controlid = sdr["id"].ToString();
                        metroSetTextBox1.Text = control;
                        dateTimePicker1.Text = sdr["Fecha"].ToString();
                        dateTimePicker2.Text = sdr["Hora"].ToString();
                    }
                }
            }
        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            saveEditedData();
        }

        private void metroSetLabel8_Click(object sender, EventArgs e)
        {
        }
    }
}
