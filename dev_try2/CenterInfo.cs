using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace dev_try2
{
    public partial class CenterInfo : DevExpress.XtraEditors.XtraForm
    {
        public CenterInfo()
        {
            InitializeComponent();
            
        }
        public string TIME;
        public string lat;
        public string lon;
        public string Type;
        public string power;
        public string pressure;
        public string position;

        public void UpdateInfo()
        {
            date.Text = TIME;
            jingdu.Text = lat;
            weidu.Text = lon;
            type.Text = Type;
            fengli.Text = power;
            qiya.Text = pressure;
            area.Text = position;
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}