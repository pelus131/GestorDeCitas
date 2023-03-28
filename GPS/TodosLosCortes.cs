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
    public partial class TodosLosCortes : MetroSetForm
    {

        string connectionString,fecha,total;
        DataTable dt = new DataTable("Corte");
        int sum = 0;

        public TodosLosCortes()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

        }

        private void llenargrid(string mes)
        {

            

            string query = "SELECT Fecha,Total FROM Corte WHERE Fecha LIKE '%"+mes+"%' ";
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
                using (SQLiteCommand cmd = new SQLiteCommand(query, con))


            {
                cmd.Parameters.Add(new SQLiteParameter("@mes", mes));


                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                {






                    adapter.Fill(this.dt);
                    dataGridView1.DataSource = dt;


                }




            }

            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }
        private void TodosLosCortes_Load(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.fecha=dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            this.total = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            try
            {

                string sql = "DELETE FROM Corte WHERE Fecha LIKE '%" + this.metroSetComboBox1.GetItemText(this.metroSetComboBox1.SelectedItem) + "%' ";

                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                using (SQLiteCommand deleteRecord = new SQLiteCommand(sql, con))
                {
                    con.Open();

                    



                    deleteRecord.ExecuteNonQuery();

                    

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dt.Clear();

            this.sum = 0;

            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                this.sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value);
            }

            label5.Text = sum.ToString();

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.ClearSelection();

        }

        private void metroSetComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            dt.Clear();
            llenargrid(this.metroSetComboBox1.GetItemText(this.metroSetComboBox1.SelectedItem));

             this.sum = 0;

            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                this.sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value);
            }

            label5.Text = sum.ToString();

        }

        private void metroSetButton3_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("No hay una fecha seleccionada");
            }
            else
            {
                try
                {

                    string sql = "DELETE FROM Corte WHERE Fecha = @Fecha AND Total = @Total ";

                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    using (SQLiteCommand deleteRecord = new SQLiteCommand(sql, con))
                    {
                        con.Open();

                        deleteRecord.Parameters.Add(new SQLiteParameter("@Fecha",fecha));
                        deleteRecord.Parameters.Add(new SQLiteParameter("@Total", total));
                       


                        deleteRecord.ExecuteNonQuery();

                        int selectedIndex = dataGridView1.SelectedRows[0].Index;

                        dataGridView1.Rows.RemoveAt(selectedIndex);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.sum = 0;

                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    this.sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value);
                }

                
                label5.Text = sum.ToString();

            }
        }
    }
}
