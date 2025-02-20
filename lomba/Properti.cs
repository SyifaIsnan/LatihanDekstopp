using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lomba
{
    internal class Properti
    {

        public static SqlConnection koneksi()
        {
            return new SqlConnection("Data Source=localhost\\SQLEXPRESS01;Initial Catalog=lksKabupaten;Integrated Security=True;TrustServerCertificate=True;Encrypt=true");
        }

        public static bool validasi(Control.ControlCollection container, TextBox kosong = null)
        {
            foreach (Control c in container)
            {
                if (c is TextBoxBase textBox && string.IsNullOrWhiteSpace(textBox.Text) && textBox != kosong)
                {
                    return true;
                }

            }
            return false;

        }
    }
}