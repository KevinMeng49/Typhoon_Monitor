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
using DevExpress.XtraCharts;

namespace dev_try2
{
    public partial class RegionPrecipitation : DevExpress.XtraEditors.XtraForm
    {
        public RegionPrecipitation()
        {
            InitializeComponent();
        }

        public void LoadAll(DataTable dt, string city, string date)
        {
            ChartTitle chartTitle = new ChartTitle();
            chartTitle.Text =date+city+"降雨过程";
            chartTitle.Font = new Font("Tahoma", 12, FontStyle.Regular);
            chartControl1.Titles.AddRange(new ChartTitle[] { chartTitle });

            Series s = this.chartControl1.Series[0];//新建一个series类并给控件赋值
            s.DataSource = dt;//设置实例对象s的数据源
            s.ArgumentDataMember = "时间";//绑定图表的横坐标
            s.ValueDataMembers[0] = "降雨量"; //绑定图表的纵坐标坐标
            
        }
    }
}