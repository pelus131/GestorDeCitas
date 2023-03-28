
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

namespace GestorDeCitas

{
    public partial class Login : MetroSetForm
    {
        
        

        public Login()
        {
            InitializeComponent();
           


        }
        

        
        private void metroSetButton1_Click(object sender, EventArgs e)
        {
         if (metroSetTextBox1.Text == "Salon" && metroSetTextBox2.Text == "Salon")
            {
                new Form1().Show();
                this.Hide();
            }
            else
            {

                MessageBox.Show("Usuario o contraseña incorrecta");
                metroSetTextBox1.Text = "";
                metroSetTextBox2.Text = "";
                metroSetTextBox1.Focus();


            }
        }

        private void metroSetButton2_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
