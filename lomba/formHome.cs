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
    public partial class formHome : Form
    {
        public formHome()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var mess = MessageBox.Show("Apakah anda yakin ingin logout", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mess == DialogResult.Yes)
            {
                this.Close();
                Login login = new Login();
                login.Show();
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            formProduk fm = new formProduk();
            fm.Show();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fomrPenjualan fr = new fomrPenjualan();
            fr.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            formPembelian fr = new formPembelian();
            fr.Show();
        }
    }
}
