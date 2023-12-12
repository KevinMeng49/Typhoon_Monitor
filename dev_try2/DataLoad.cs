using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dev_try2
{
    class DataLoad
    {
        private SqlConnection conn;
        private void ConnectDatabase()
        {
            String conString = "server=47.99.196.197;database=CLASS;uid=class;pwd=ae2021";
            conn = new SqlConnection(conString);
            conn.Open();
            if (conn.State != ConnectionState.Open)
                MessageBox.Show("ERROR!!!");
            else
            {
               // MessageBox.Show("connection succeed!!!");
            }
        }
        public SqlDataReader ExecuteSQL(string sql)
        {
            SqlCommand com = new SqlCommand(sql, conn);
            SqlDataReader dr = com.ExecuteReader();
            return dr;
        }

        //构造函数
        public DataLoad()
        {
            ConnectDatabase();
        }

        public void Close()
        {
            conn.Close();
        }
        ~DataLoad()
        {
            //conn.Close();
        }

    }
}
