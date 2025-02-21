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
          
           

            dataGridView1.Columns.Add("kd_produk", "KD PRODUK");
            dataGridView1.Columns.Add("nama_produk", "NAMA PRODUK");
            dataGridView1.Columns.Add("qty", "QTY");
            dataGridView1.Columns.Add("harga_jual", "HARGA JUAL");
            dataGridView1.Columns.Add("total", "TOTAL");
            
            
            
            


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

        private void totalharga()
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            kembali();
            using (var koneksi = Properti.koneksi())
            {
                string no_penjualan_awal = "J001"; // nilai awal
                koneksi.Open();
                // order by karena kita mau sort
                // desc karena kita mau sort dari paling besar ke paling bawah
                // top 1 karena kita cuma mau ngambil satu nilai
                SqlCommand cmd = new SqlCommand("select top 1 no_penjualan from [penjualan] order by no_penjualan desc", koneksi);
                string no_penjualan_db = cmd.ExecuteScalar()?.ToString(); // ? karena nilai mungkin null

                if (no_penjualan_db == null)
                { // apabila null kita pake nilai no penjualan awal
                    SqlCommand cmdInsert = new SqlCommand("insert into penjualan (no_penjualan, tanggal_jual, kd_pelanggan, total) values (@no_penjualan, @tanggal_jual, @kd_pelanggan, @total )", koneksi);
                    cmdInsert.Parameters.AddWithValue("@no_penjualan", no_penjualan_awal);
                    cmdInsert.Parameters.AddWithValue("@tanggal_jual", DateTime.Now);
                    cmdInsert.Parameters.AddWithValue("@kd_pelanggan", "1");
                    cmdInsert.Parameters.AddWithValue("@total", textBox2.Text);
                    cmdInsert.ExecuteNonQuery();

                    SqlCommand cmdInsert2 = new SqlCommand("insert into penjualan_detail (no_penjualan, kd_produk, qty) values (@no_penjualan, @kd_produk, @qty)", koneksi);
                    cmdInsert2.Parameters.AddWithValue("@no_penjualan", no_penjualan_awal);
                    cmdInsert2.Parameters.AddWithValue("@kd_produk", textBox1.Text);
                    cmdInsert2.Parameters.AddWithValue("@qty", "1");
                    cmdInsert.ExecuteNonQuery();

                }
                else
                { // apabila tidak null kita hitung nilai baru
                  // ada tiga tahap di baris ini
                  // 1. menghilangkan huruf pertama dari string ("J")
                  // 2. convert hasil dari nilai di atas ke int
                  // 3. tambah nilai di atas dengan angka 1
                    int no_penjualan_db_int = Convert.ToInt32(no_penjualan_db.Remove(0, 1)) + 1;
                    // no_penjualan_db_int.ToString("D3"); bakal ngeubah angka jadi string yg udah diformat, misal 9 jadi "009", 11 jadi "011"
                    // nilai di atas ditambah "J"
                    string no_penjualan_final = "J" + no_penjualan_db_int.ToString("D3");
                    SqlCommand cmdInsert = new SqlCommand("insert into penjualan (no_penjualan, tanggal_jual, kd_pelanggan, total) values (@no_penjualan, @tanggal_jual, @kd_pelanggan, @total)", koneksi);
                    cmdInsert.Parameters.AddWithValue("@no_penjualan", no_penjualan_final);
                    cmdInsert.Parameters.AddWithValue("@tanggal_jual", DateTime.Now);
                    cmdInsert.Parameters.AddWithValue("@kd_pelanggan", "1");
                    cmdInsert.Parameters.AddWithValue("@total", textBox2.Text);
                    cmdInsert.ExecuteNonQuery();

                    SqlCommand cmdInsert2 = new SqlCommand("insert into penjualan_detail (no_penjualan, kd_produk, qty) values (@no_penjualan, @kd_produk, @qty)", koneksi);
                    cmdInsert2.Parameters.AddWithValue("@no_penjualan", no_penjualan_awal);
                    cmdInsert2.Parameters.AddWithValue("@kd_produk", textBox1.Text);
                    cmdInsert2.Parameters.AddWithValue("@qty", "1");
                    cmdInsert.ExecuteNonQuery();
                }

            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            using (var koneksi = Properti.koneksi())
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string keyword = textBox1.Text;
                    SqlCommand cekProduk = new SqlCommand("select kd_produk, nama_produk, harga_jual from [Produk] where kd_produk = @kd_produk", koneksi);
                    cekProduk.CommandType = CommandType.Text;
                    koneksi.Open();
                    cekProduk.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    cekProduk.Parameters.AddWithValue("@kd_produk", textBox1.Text);
                    DataTable dt = new DataTable();
                    SqlDataReader dr = cekProduk.ExecuteReader();
                    
                    dt.Load(dr);
                    if (dt.Rows.Count > 0)
                    {
                        string kd_produk = dt.Rows[0][0].ToString();
                        string nama_produk = dt.Rows[0][1].ToString();
                        int qty = 1;
                        int harga_jual = Convert.ToInt32(dt.Rows[0][2]);
                        int total = qty * harga_jual;
                        dataGridView1.Rows.Add(kd_produk, nama_produk, qty, harga_jual, total);

                        int sum = 0;
                        for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                        {
                            sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value);
                        }
                        label1.Text = sum.ToString();
                        textBox2.Text = sum.ToString();




                    }
                    else if (dt.Rows.Count == null)
                    {
                        MessageBox.Show("Data tidak ditemukan!");
                    }
                }
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int total = Convert.ToInt32(textBox2.Text);
            int bayar = Convert.ToInt32(textBox3.Text);

           
                int kembali = bayar - total;
                textBox4.Text = kembali.ToString();
          

            
        }
    }
}
