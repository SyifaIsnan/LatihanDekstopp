using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lomba
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string username = "admin";
                string password = "admin";

                if (Properti.validasi(this.Controls))
                {
                    MessageBox.Show("Harap isi semua data yang diperlukan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (textBox1.Text != username || textBox2.Text != password)
                {
                    MessageBox.Show("Username atau password yang anda masukkan salah!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (textBox1.Text == username && textBox2.Text == password)
                {
                    this.Hide();
                    formHome fh = new formHome();
                    fh.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
