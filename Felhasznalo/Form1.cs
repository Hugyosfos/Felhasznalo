using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Felhasznalo
{
    public partial class Form1 : Form
    {
        MySqlConnection conn = null;
        MySqlCommand cmd = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "felhasznalo";
            conn = new MySqlConnection(builder.ConnectionString);
            try
            {
                conn.Open();
                cmd = conn.CreateCommand();
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message + Environment.NewLine + "A program leáll");

                Environment.Exit(0);
            }
            finally
            {
                conn.Close();
            }
            listBox1_update();
        }

        private MySqlConnection GetConn()
        {
            return conn;
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Nincs kijelölve felhasználó!");
                return;
            }
            cmd.Parameters.Clear();
            FELHASZNALO kivalasztott_user = (FELHASZNALO)listBox1.SelectedItem;
            cmd.CommandText = "UPDATE `felhasznalo` SET `Név`=@name,`SzulDatum`=@date,`profilkep`=@profilkep WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", kivalasztott_user.Id);
            cmd.Parameters.AddWithValue("@name", textBox1.Text);
            cmd.Parameters.AddWithValue("@date", dateTimePicker1.Value);
            if (File.Exists(kivalasztott_user.Profilkep))
            {
                cmd.Parameters.AddWithValue("@profilkep", kivalasztott_user.Profilkep);

            }
            else
            {
                cmd.Parameters.AddWithValue("@profilkep", openFileDialog1.FileName);
            }

            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Módosítás sikeres votl!");
                textBox1.Text = "";
                dateTimePicker1.Value = DateTime.Today;
                openFileDialog1.FileName = "";
                pictureBox1.Image = null;

                listBox1_update();
            }
            else
            {
                MessageBox.Show("Az adatok módosítása sikertelen!");
            }
        }



        private void listBox1_update()
        {
            conn.Close();
            listBox1.Items.Clear();
            cmd.CommandText = "SELECT * FROM `felhasznalo`;";
            conn.Open();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    FELHASZNALO uj = new FELHASZNALO(dr.GetInt32("id"), dr.GetString("Név"), dr.GetDateTime("SzulDatum"), dr.GetString("profilkep"));
                    listBox1.Items.Add(uj);
                }
            }
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Close();
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Adjon meg nevet");
                textBox1.Focus();
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Adjon meg kep nevet");
                textBox2.Focus();
                return;
            }
            cmd.CommandText = "INSERT INTO `felhasznalo`(`Név`, `SzulDatum` ,`profilkep`) VALUES (@Nev ,@SzulDatum,@profilkep)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Szuldatum",dateTimePicker1.Value );
            cmd.Parameters.AddWithValue("@Nev", textBox1.Text);
            cmd.Parameters.AddWithValue("@profilkep", openFileDialog1.SafeFileName);
         
            conn.Open();
            try
            {
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Sikeresen rögzítve!");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    listBox1_update();
                }
                else
                {
                    MessageBox.Show("sikertelen rögzítés!");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {         
                openFileDialog1.Filter = "kurva|*.png;*.jpg;*.webp";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string kepFájl = openFileDialog1.FileName;
                    pictureBox1.Image = Image.FromFile(kepFájl);
                textBox2.Text = openFileDialog1.SafeFileName;
                }           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                return;
            }
            FELHASZNALO felhasznalo = (FELHASZNALO)listBox1.SelectedItem;
            textBox1.Text = felhasznalo.Név;
            textBox2.Text = felhasznalo.Profilkep;
            dateTimePicker1.Value = felhasznalo.SzulDatum;
        }

        private void button3_Click(object sender, EventArgs e)
        {


            if (listBox1.SelectedIndex < 0)
            {
                return;
            }
            cmd.CommandText = "DELETE FROM `felhasznalo` WHERE `id` = @id";
            cmd.Parameters.Clear();
            FELHASZNALO felhasznalo = (FELHASZNALO)listBox1.SelectedItem;
            cmd.Parameters.AddWithValue("@id", felhasznalo.Id);
            conn.Open();
            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Törlés sikeres");
                conn.Close();
                textBox1.Text = "";
                textBox2.Text = "";

                listBox1_update();
              
            }
            else
            {
                MessageBox.Show("Törlés sikertelen");
            }
           
        }
            
    }  
    }

    
