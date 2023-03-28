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



        string connectionString;
        string  controlid="";
        public EditarCita()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;



        }



        protected void fillcombo()
        {

            SQLiteConnection conn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.CommandText = @"SELECT Nombre FROM Clientes  ";
            cmd.Connection = conn;
            conn.Open();
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["Nombre"]);

                
            }


            dr.Dispose();
            conn.Close();




        }
        

        private void EditarCita_Load(object sender, EventArgs e)
        {
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "hh:mm tt";
            fillcombo();

            DataTable dt = new DataTable();

            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                string query = "Select Servicio FROM Servicios";

                SQLiteCommand cmd = new SQLiteCommand(query, con);

                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(dt);
                checkedListBox1.DataSource = dt;
                checkedListBox1.DisplayMember = "Servicio";

            }

        }

        private void fillcombo2()
        {
            comboBox2.Items.Clear();
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            conn.Open();

            int control1 = comboBox1.SelectedIndex + 1;

            string query1 = "SELECT Servicio FROM Citas WHERE id_cliente = " + control1;
            SQLiteCommand cmd1 = new SQLiteCommand(query1, conn);
            SQLiteDataReader sdr1 = cmd1.ExecuteReader();
            while (sdr1.Read())
            {
                comboBox2.Items.Add(sdr1["Servicio"]);


            }


            conn.Close();


        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillcombo2();


            
            
            
        }



        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string control = comboBox2.Text;

            SQLiteConnection conn = new SQLiteConnection(connectionString);
            conn.Open();
            string query = "SELECT id,Fecha,Hora FROM Citas WHERE Servicio = '"+control+"'";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                this.controlid = sdr["id"].ToString();
                metroSetTextBox1.Text = control;
                dateTimePicker1.Text = sdr["Fecha"].ToString();
                dateTimePicker2.Text = sdr["Hora"].ToString();
                //metroSetTextBox4.Text = control;
                //metroSetTextBox3.Text = controlid;
            }
            conn.Close();
        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            string servicios = "";

            try
            {
                

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {

                    if (checkedListBox1.GetItemChecked(i))
                    {

                        servicios += ((DataRowView)checkedListBox1.Items[i])[0].ToString() + ",";
                    }

                }
                servicios = servicios.Remove(servicios.Length - 1);
            }catch (Exception ex)
            { 
            }

            if (checkedListBox1.CheckedItems.Count == 0 || this.comboBox1.GetItemText(this.comboBox1.SelectedItem) == "" || this.comboBox2.GetItemText(this.comboBox2.SelectedItem) == "")
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


                        cmd.CommandText = @"UPDATE Citas SET Servicio=@Servicio,Fecha=@Fecha,Hora=@Hora  WHERE id = '" + controlid + "'";
                        cmd.Connection = con;

                        cmd.Parameters.Add(new SQLiteParameter("@Servicio", servicios));
                        //cmd.Parameters.Add(new SQLiteParameter("@idcliente", metroSetTextBox2.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@Fecha", dateTimePicker1.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@Hora", dateTimePicker2.Text));

                        con.Open();



                        int i = cmd.ExecuteNonQuery();
                        if (i == 1)
                        {

                            MessageBox.Show("Editado con Exito");


                        }
                        con.Close();
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fecha Y Hora Ya tiene una cita previa!");

                }

                fillcombo2();
            }
        }

        private void metroSetLabel8_Click(object sender, EventArgs e)
        {

        }
    }
}
