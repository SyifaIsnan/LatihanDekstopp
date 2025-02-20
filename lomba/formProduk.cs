using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lomba
{
    public partial class formProduk : Form
    {
        public formProduk()
        {
            InitializeComponent();
            tampildata();
        }

        private void tampildata()
        {
            using(var koneksi = Properti.koneksi())
            {
                SqlCommand cmd = new SqlCommand("select * from [Produk]", koneksi);
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

        private void button1_Click(object sender, EventArgs e)
        {
            using (var koneksi = Properti.koneksi())
            {
                try
                {
                    if (Properti.validasi(this.Controls))
                    {
                        MessageBox.Show("Harap isi semua data yang diperlukan!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO [Produk] VALUES (@kd_produk, @nama_produk, @kd_kategori , @satuan, @harga_modal, @harga_jual, @stok)", koneksi);
                        cmd.CommandType = CommandType.Text;
                        koneksi.Open();
                        cmd.Parameters.AddWithValue("@kd_produk", textBox1.Text);
                        cmd.Parameters.AddWithValue("@nama_produk", textBox2.Text);
                        cmd.Parameters.AddWithValue("@kd_kategori", textBox3.Text);
                        cmd.Parameters.AddWithValue("@satuan", textBox4.Text);
                        cmd.Parameters.AddWithValue("@harga_modal", float.Parse(textBox5.Text));
                        cmd.Parameters.AddWithValue("@harga_jual", float.Parse(textBox6.Text));
                        cmd.Parameters.AddWithValue("@stok", float.Parse(textBox7.Text));
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Berhasil menambahkan data!");
                        tampildata();
                        clear();
                        koneksi.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var koneksi = Properti.koneksi())
            {
                try
                {
                    if (Properti.validasi(this.Controls))
                    {
                        MessageBox.Show("Harap isi semua data yang diperlukan!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE [Produk] SET nama_produk = @nama_produk, kd_kategori = @kd_kategori , satuan = @satuan, harga_modal = @harga_modal, harga_jual = @harga_jual, stok = @stok where kd_produk = @kd_produk", koneksi);
                        cmd.CommandType = CommandType.Text;
                        koneksi.Open();
                        cmd.Parameters.AddWithValue("@kd_produk", textBox1.Text);
                        cmd.Parameters.AddWithValue("@nama_produk", textBox2.Text);
                        cmd.Parameters.AddWithValue("@kd_kategori", textBox3.Text);
                        cmd.Parameters.AddWithValue("@satuan", textBox4.Text);
                        cmd.Parameters.AddWithValue("@harga_modal", textBox5.Text);
                        cmd.Parameters.AddWithValue("@harga_jual", textBox6.Text);
                        cmd.Parameters.AddWithValue("@stok", textBox7.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Berhasil mengubah data!");
                        tampildata();
                        clear();
                        koneksi.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var koneksi = Properti.koneksi())
            {
                try
                {
                    if (Properti.validasi(this.Controls))
                    {
                        MessageBox.Show("Harap isi semua data yang diperlukan!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var mess = MessageBox.Show("Apakah anda yakin ingin menghapus data ini?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (mess == DialogResult.Yes) {

                            SqlCommand cmd = new SqlCommand("DELETE FROM [Produk] where kd_produk = @kd_produk", koneksi);
                            cmd.CommandType = CommandType.Text;
                            koneksi.Open();
                            cmd.Parameters.AddWithValue("@kd_produk", textBox1.Text);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Berhasil menghapus data!");
                            tampildata();
                            clear();
                            koneksi.Close();

                        }
                        

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
    }
}
