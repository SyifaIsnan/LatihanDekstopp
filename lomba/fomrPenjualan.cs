using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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

            dataGridView1.Columns["kd_produk"].HeaderText = "Kode Produk";
            dataGridView1.Columns["nama_produk"].HeaderText = "Nama Produk";
            dataGridView1.Columns["qty"].HeaderText = "Jumlah";
            dataGridView1.Columns["harga_jual"].HeaderText = "Harga Jual";
            dataGridView1.Columns["total"].HeaderText = "Total Harga";


        }

        private void tampildata()
        {
            //using (var koneksi = Properti.koneksi())
            //{
            //    SqlCommand cmd = new SqlCommand("select Produk.kd_produk, Produk.nama_produk, PenjualanDetail.qty, Produk.harga_jual, Penjualan.total \r\nfrom [PenjualanDetail]\r\ninner join [Produk] on [PenjualanDetail].kd_produk = [Produk].kd_produk\r\ninner join [Penjualan] on [PenjualanDetail].no_penjualan = Penjualan.no_penjualan\r\n", koneksi);
            //    cmd.CommandType = CommandType.Text;
            //    koneksi.Open();
            //    DataTable dt = new DataTable();
            //    SqlDataReader dr = cmd.ExecuteReader();
            //    dt.Load(dr);
            //    dataGridView1.DataSource = dt;
            //    koneksi.Close();
            //}

            using (var koneksi = Properti.koneksi())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT Produk.kd_produk, Produk.nama_produk, PenjualanDetail.qty, Produk.harga_jual, " +
                    "(Produk.harga_jual * PenjualanDetail.qty) AS 'total' " +
                    "FROM [PenjualanDetail] " +
                    "INNER JOIN [Produk] ON [PenjualanDetail].kd_produk = [Produk].kd_produk " +
                    "INNER JOIN [Penjualan] ON [PenjualanDetail].no_penjualan = Penjualan.no_penjualan",
                    koneksi);

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
                MessageBox.Show("uang bayar kurang!", "peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }


        private void total()
        {
            //using (var koneksi = Properti.koneksi())
            //{
            //    koneksi.Open();

            //    SqlCommand cmd = new SqlCommand("SELECT SUM(total) FROM [Penjualan]", koneksi);
            //    object result = cmd.ExecuteScalar();

            //    label1.Text = (result != null) ? "TOTAL = " + Convert.ToInt32(result).ToString("C", CultureInfo.GetCultureInfo("id-ID")) : "0";
            //    textBox2.\
            //    Text = (result != null) ? Convert.ToInt32(result).ToString() : "0";
            //}



            using (var koneksi = Properti.koneksi())
            {
                koneksi.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT SUM(Produk.harga_jual * PenjualanDetail.qty) " +
                    "FROM [PenjualanDetail] " +
                    "INNER JOIN [Produk] ON [PenjualanDetail].kd_produk = [Produk].kd_produk " +
                    "INNER JOIN [Penjualan] ON [PenjualanDetail].no_penjualan = Penjualan.no_penjualan",
                    koneksi);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    int total = Convert.ToInt32(result);
                    label1.Text = "TOTAL = " + total.ToString("C", CultureInfo.GetCultureInfo("id-ID"));
                    textBox2.Text = total.ToString();
                }
                else
                {
                    label1.Text = "TOTAL = Rp 0";
                    textBox2.Text = "0";
                }
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            kembali();
            using (var koneksi = Properti.koneksi())
            {
                try
                {
                    SqlCommand cmdPenjualan = new SqlCommand("INSERT INTO Penjualan (tanggal_jual, total) OUTPUT INSERTED.no_penjualan VALUES (@tanggal_jual, @total)", koneksi);
                    koneksi.Open();
                    cmdPenjualan.CommandType = CommandType.Text;
                    cmdPenjualan.Parameters.AddWithValue("@tanggal_jual", DateTime.Now);
                    cmdPenjualan.Parameters.AddWithValue("@total", textBox2.Text);
                    DataTable dt = new DataTable();
                    int noPenjualan = (int)cmdPenjualan.ExecuteScalar();

                    foreach (DataRow row in dt.Rows)
                    {
                        SqlCommand cmdDetail = new SqlCommand("INSERT INTO PenjualanDetail(no_penjualan, kd_produk, qty) VALUES (@no_penjualan, @kd_produk, @qty)", koneksi);
                        cmdDetail.Parameters.AddWithValue("@no_penjualan", noPenjualan);
                        cmdDetail.Parameters.AddWithValue("@kd_produk", row["kd_produk"].ToString());
                        cmdDetail.Parameters.AddWithValue("@qty", row["qty"]);
                        cmdDetail.ExecuteNonQuery();
                    }
                    koneksi.Close();




                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            using(var koneksi = Properti.koneksi())
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string keyword = textBox1.Text;
                    SqlCommand cekProduk = new SqlCommand(
                    "SELECT Produk.kd_produk, Produk.nama_produk, PenjualanDetail.qty, Produk.harga_jual, " +
                    "(Produk.harga_jual * PenjualanDetail.qty) AS 'total' " +
                    "FROM [PenjualanDetail] " +
                    "INNER JOIN [Produk] ON [PenjualanDetail].kd_produk = [Produk].kd_produk " +
                    "INNER JOIN [Penjualan] ON [PenjualanDetail].no_penjualan = Penjualan.no_penjualan where [Produk].kd_produk like @keyword",
                    koneksi);
                    cekProduk.CommandType = CommandType.Text;
                    koneksi.Open();
                    cekProduk.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    DataTable dt = new DataTable();
                    SqlDataReader dr = cekProduk.ExecuteReader();
                    dt.Load(dr);
                    if (dt.Rows.Count >= 0)
                    {
                        dataGridView1.DataSource = dt;
                    }
                    else if(dt.Rows.Count == null) 
                    {
                        MessageBox.Show("Data tidak ditemukan!");
                    }
                }
            }
        
        }
    }
}
