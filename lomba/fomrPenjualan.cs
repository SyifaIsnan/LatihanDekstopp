using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lomba
{
    public partial class fomrPenjualan : Form
    {
        public fomrPenjualan()
        {
            InitializeComponent();
            tampildata();
            total();
            kembali();
            textBox3.Text = "0";

        }

        private void tampildata()
        {
            using (var koneksi = Properti.koneksi())
            {
                SqlCommand cmd = new SqlCommand("select Produk.kd_produk, Produk.nama_produk, PenjualanDetail.qty, Produk.harga_jual, Penjualan.total \r\nfrom [PenjualanDetail]\r\ninner join [Produk] on [PenjualanDetail].kd_produk = [Produk].kd_produk\r\ninner join [Penjualan] on [PenjualanDetail].no_penjualan = Penjualan.no_penjualan\r\n", koneksi);
                cmd.CommandType = CommandType.Text;
                koneksi.Open();
                DataTable dt = new DataTable();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
                koneksi.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
            
        }

        private void kembali()
        {
            int total = Convert.ToInt32(textBox2.Text);
            int bayar = Convert.ToInt32(textBox3.Text);

            if (bayar >= total)
            {
                int kembali = bayar - total;
                textBox4.Text = kembali.ToString();
            }
            else
            {
                MessageBox.Show("Uang bayar kurang!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox4.Text = "0";
            }

        }


        private void total()
        {
            using (var koneksi = Properti.koneksi())
            {
                koneksi.Open(); 

                SqlCommand cmd = new SqlCommand("SELECT SUM(total) FROM [Penjualan]", koneksi);
                object result = cmd.ExecuteScalar();

                label1.Text = (result != null) ? "TOTAL = " + Convert.ToInt32(result).ToString("C", CultureInfo.GetCultureInfo("id-ID")) : "0";
                textBox2.Text = (result != null) ? Convert.ToInt32(result).ToString() : "0";
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
