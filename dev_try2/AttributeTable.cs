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
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using System.Data.SqlClient;

namespace dev_try2
{
    //用来显示查询以后高亮显示的要素的属性
    public partial class AttributeTable : DevExpress.XtraEditors.XtraForm
    {
        public IMap currentmap;
        private IFeatureSelection featureSelection;
        private AxMapControl axmap;
        public AttributeTable(IFeatureSelection featureS)  //已选中的信息，列表列出来
        {
            InitializeComponent();
            featureSelection = featureS;
            start();
        }
   
        public AttributeTable(AxMapControl axmapcontrol, string start, string end)
        {
            InitializeComponent();
            axmap = axmapcontrol;

            DataLoad dataLoad = new DataLoad();
            string sql = "select 站码,SUM(雨量) as 总雨量 from 黑格比雨量 where 时间 BETWEEN '" + start + "' and '" + end + "' group by 站码 ;";
            SqlDataReader dataReader = dataLoad.ExecuteSQL(sql);
            DataTable pTable = new DataTable();
            DataColumn colName1 = new DataColumn("站码");
            colName1.DataType = System.Type.GetType("System.String");
            pTable.Columns.Add(colName1);
            DataColumn colName2 = new DataColumn("总雨量");
            colName2.DataType = System.Type.GetType("System.String");
            pTable.Columns.Add(colName2);
            while (dataReader.Read())
            {
                string ID = dataReader[0].ToString();
                string Rain = dataReader[1].ToString();
                DataRow pRow = pTable.NewRow();
                pRow[0] = ID;
                pRow[1] = Rain;
                pTable.Rows.Add(pRow);
            }
            dataGridView.DataSource = pTable;
        }

        public void start()
        {
            if (featureSelection == null)     //传进来的map不是空的
            {

            }
            IFeatureLayer currentLayer = featureSelection as IFeatureLayer;
            //建立表的列
            DataTable dataDt = new DataTable();
            DataRow pRow = null;
            DataColumn pCol = null;      //行和列
            IField field = null;
            for (int i = 0; i < currentLayer.FeatureClass.Fields.FieldCount; i++)  //当前图层的字段的数量
            {
                field = currentLayer.FeatureClass.Fields.get_Field(i);
                pCol = new DataColumn();
                pCol.ColumnName = field.AliasName;
                dataDt.Columns.Add(pCol);         //把这个列添加到表里

            }
            ISelectionSet selectionSet = featureSelection.SelectionSet;
            ICursor cursor;
            selectionSet.Search(null, false,out cursor);          //游标，用来后面显示行
            IFeatureCursor fcursor = cursor as IFeatureCursor;
            IFeature pfeature = fcursor.NextFeature();                   //获取一个要素，即数据表里的一行
            while (pfeature != null)
            {
                pRow = dataDt.NewRow();
                //把一个要素的所有字段值获取出来
                for (int i = 0; i < dataDt.Columns.Count; i++)
                {
                    pRow[i] = pfeature.get_Value(i); //当前要素的第i个值添加到一行里
                }
                dataDt.Rows.Add(pRow);
                pfeature = fcursor.NextFeature();

            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(fcursor);     //释放掉游标
            dataGridView.DataSource = dataDt;       //把数据表格绑定到这个变量
        }

        //点击OK，关掉这个窗口，同时取消高亮显示
        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            this.Close();
            //CancelHighlight
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //IFeatureLayer currentFeatureLayer = featureSelection as IFeatureLayer;
            ////获取当前所点击的行
            //DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            ////每行的第一列是要素的ObjectID，获取该信息
            //int objectID = Convert.ToInt32(row.Cells[0].Value);
            ////使用IFeatureClass接口的GetFeature方法根据ObjectID获取该要素
            //IFeature feature = currentFeatureLayer.FeatureClass.GetFeature(objectID);
            ////定义新的IEnvelope接口对象获取该要素的空间范围
            //ESRI.ArcGIS.Geometry.IEnvelope outEnvelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            ////通过IGeometry接口的QueryEnvelope方法获取要素的空间范围
            //feature.Shape.QueryEnvelope(outEnvelope);
            ////将主窗体地图的当前可视范围定义为要素的空间范围，并刷新地图
            //IActiveView activeView = currentmap as IActiveView;
            //activeView.Extent = outEnvelope;
            //activeView.Refresh();
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}