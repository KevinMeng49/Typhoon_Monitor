using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using System.Data.SqlClient;
using ESRI.ArcGIS.Geometry;
using System.Data.OleDb;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using System.Timers;
using DevExpress.Charts.Model;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesRaster;
using System.IO;

namespace dev_try2
{
    public partial class RibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        #region 类变量
        public IFeatureSelection featureSelection;
        private System.Timers.Timer timer1;
        private System.Timers.Timer timer2;
        private Boolean timerflag = true;//标记计时器状态
        private Boolean startflag = false;//标记播放状态
        private Boolean loadflag = false;//标记播放按钮是否被点击过
        int count = 0;//记录当前显示的feature个数
        const int typhooncount = 97;
        const int hourlyrain = 12;
        private int cloudcount;
        Boolean display = true;
        Boolean display2 = true;
        Boolean wholeclick = true;
        Boolean zjclick = true;
        int drawOrNot = 0; //是否绘制
        int idwdraw = 0; //是否绘制浙江降水插值图
        int contourFlag = 0;
        IGraphicsContainer pContainer; //绘制图案的临时容器
        IFeatureCursor cursorOfDraw = null;
        #endregion
        public RibbonForm()
        {
           
            InitializeComponent();
           
            InitializeMap();
            MapComposer.ZoomtoLayers("ChinaProvince", axMapControl1);
            pContainer = axMapControl1.Map as IGraphicsContainer;
        }

        #region 初始化地图
        public void InitializeMap()
        {
            string str1 = System.Environment.CurrentDirectory;
            //加载底图，创建底图simple renderer，调用dataoperator加载shp
            DataOperator dataOperator = new DataOperator(axMapControl1.Map);

            

            dataOperator.AddRasterToMap(@"..\..\..\typhoondata\NE1_50M_SR_W", "NE1_50M_SR_W.tif", "basetif");
            dataOperator.AddRasterToMap(@"..\..\..\typhoondata\NE1_50M_SR_W", "zhejiangbase.tif", "zhejiangbasetif");


            IFeatureClass CountryBasemapFeature = DataOperator.GetFeatureClass(@"..\..\..\typhoondata\BaseMap", "Country.shp");   
            IFeatureRenderer CountryRenderer = Render.GetSimpleRenderer(CountryBasemapFeature,222,240,209);
            bool bRes2 = dataOperator.AddFeatureClassToMap(CountryBasemapFeature, "Country", CountryRenderer as IFeatureRenderer);

            IFeatureClass ChinaProvinceBasemapFeature = DataOperator.GetFeatureClass(@"..\..\..\typhoondata\BaseMap", "中国国界.shp");
            IFeatureRenderer ChinaProvinceRenderer = Render.GetSimpleRenderer(ChinaProvinceBasemapFeature, 128, 128, 128);
            bool bRes1 = dataOperator.AddFeatureClassToMap(ChinaProvinceBasemapFeature, "ChinaProvince", ChinaProvinceRenderer as IFeatureRenderer);

            IFeatureClass NineSegmentLineBasemapFeature = DataOperator.GetFeatureClass(@"..\..\..\typhoondata\BaseMap", "NineSegmentLine.shp");
            IFeatureRenderer NineSegmentLineRenderer = Render.GetSimpleRenderer(NineSegmentLineBasemapFeature, 108, 103, 103);
            bool bRes3 = dataOperator.AddFeatureClassToMap(NineSegmentLineBasemapFeature, "NineSegmentLine", NineSegmentLineRenderer as IFeatureRenderer);

            IFeatureClass SouthSeaIslandBasemapFeature = DataOperator.GetFeatureClass(@"..\..\..\typhoondata\BaseMap", "SouthSeeIsland.shp");
            IFeatureRenderer SouthSeaIslandRenderer = Render.GetSimpleRenderer(SouthSeaIslandBasemapFeature, 150, 200, 150);
            bool bRes4 = dataOperator.AddFeatureClassToMap(SouthSeaIslandBasemapFeature, "SouthSeaIsland", SouthSeaIslandRenderer as IFeatureRenderer);


            IFeatureClass townsBasemapFeature = DataOperator.GetFeatureClass(@"..\..\..\typhoondata\BaseMap", "Towns_zj.shp");            IFeatureRenderer townsRenderer = Render.GetSimpleRenderer(townsBasemapFeature, 80, 80, 80);
            bool bRes5 = dataOperator.AddFeatureClassToMap(townsBasemapFeature, "Towns", townsRenderer as IFeatureRenderer);

            IFeatureClass stationBasemapFeature = DataOperator.GetFeatureClass(@"..\..\..\typhoondata\station", "station.shp");
            IFeatureRenderer stationRenderer = Render.GetSimpleRenderer(stationBasemapFeature, 152, 152, 152);
            bool bRes6 = dataOperator.AddFeatureClassToMap(stationBasemapFeature, "station", stationRenderer as IFeatureRenderer);

            //加载台风路径

            IFeatureClass typhoonBasemapFeature = DataOperator.GetFeatureClass(@"..\..\..\typhoondata\typhoon", "typhoon.shp");
            IFeatureRenderer typhoonRenderer = Render.GetDivideValueRenderer(typhoonBasemapFeature, "风力");//Render.GetSimpleRenderer(typhoonBasemapFeature, 0,183,255);

            bool bRes7 = dataOperator.AddFeatureClassToMap(typhoonBasemapFeature, "Typhoon", typhoonRenderer as IFeatureRenderer);

            ILayer layer1 = dataOperator.GetLayerByName("Typhoon");
            layer1.Visible = false;
            ILayer layer2 = dataOperator.GetLayerByName("station");
            layer2.Visible = false;
            ILayer layer3 = dataOperator.GetLayerByName("Country");
            layer3.Visible = false;
            
            

            //加载所有下拉框里的值
            //浙江-城市下拉框
            repositoryItemComboBox5.Items.Clear();
            DataLoad dataLoad = new DataLoad();
            IGeoFeatureLayer geoFeatureLayer = dataOperator.GetLayerByName("Towns") as IGeoFeatureLayer;
            //定义一循环游标
            IFeatureCursor featureCursor = geoFeatureLayer.FeatureClass.Search(null, false);
            IFeature feature;
            if (featureCursor != null)
            {
                int fieldIndex = geoFeatureLayer.FeatureClass.Fields.FindField("name");
                //遍历
                while ((feature = featureCursor.NextFeature()) != null)
                {
                    string name = feature.Value[fieldIndex].ToString();
                    repositoryItemComboBox5.Items.Add(name);
                }
            }
            //浙江-时间下拉框
            repositoryItemComboBox4.Items.Add("8月2日");
            repositoryItemComboBox4.Items.Add("8月3日");
            repositoryItemComboBox4.Items.Add("8月4日");
            repositoryItemComboBox4.Items.Add("8月5日");

            //流域时间下拉框
            repositoryItemComboBox6.Items.Add("8月2日");
            repositoryItemComboBox6.Items.Add("8月3日");
            repositoryItemComboBox6.Items.Add("8月4日");
            repositoryItemComboBox6.Items.Add("8月5日");

            repositoryItemDateEdit4.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            repositoryItemDateEdit4.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemDateEdit4.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.repositoryItemDateEdit4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit4.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.repositoryItemDateEdit4.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit4.Mask.EditMask = "yyyy-MM-dd HH";

            repositoryItemDateEdit1.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            repositoryItemDateEdit1.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemDateEdit1.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.repositoryItemDateEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit1.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.repositoryItemDateEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit1.Mask.EditMask = "yyyy-MM-dd HH";

            repositoryItemDateEdit2.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            repositoryItemDateEdit2.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemDateEdit2.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.repositoryItemDateEdit2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit2.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.repositoryItemDateEdit2.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit2.Mask.EditMask = "yyyy-MM-dd HH";

            //状态栏清空
            barStaticItem1.Caption = "";
            ILayer ly = dataOperator.GetLayerByName("ChinaProvince");
            DataOperator.AddLable(axMapControl1, ly, "NAME");

        }
        #endregion
        //浙江--云图
        void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if(display2==true)
            {
                DataOperator dataOperator = new DataOperator(axMapControl1.Map);
                timer2 = new System.Timers.Timer();
                timer2.Interval = 1000;//时间间隔
                MapComposer.ZoomtoLayers("Typhoon", axMapControl1);
                //云图路径记得写（云图所在文件夹）
                string pPath = "..\\..\\..\\typhoondata\\ExtractPrecip";
                IWorkspaceFactory pWorkspaceFactory = new RasterWorkspaceFactory();
                IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(pPath, 0);
                
                for(int i=0;i<24;i+=2)
                {
                    IRasterWorkspace pRasterWorkspace = (IRasterWorkspace)pWorkspace;
                    IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset("hour"+i+".tif");
                    //影像金字塔判断与创建
                    IRasterPyramid3 pRasPyrmid;
                    pRasPyrmid = pRasterDataset as IRasterPyramid3;
                    if (pRasPyrmid != null)
                    {
                        if (!(pRasPyrmid.Present))
                        {
                            pRasPyrmid.Create(); //创建金字塔
                        }
                    }
                    IRasterLayer pRasterLayer = new RasterLayerClass();
                    pRasterLayer.CreateFromDataset(pRasterDataset);
                    pRasterLayer.Renderer = Render.PrecipitationRenderer(pRasterLayer);
                    ILayer pLayer = pRasterLayer as ILayer;
                    axMapControl1.AddLayer(pLayer);
                }
                
                //所有图层设置为不可见
                for (int i = 0; i < hourlyrain; i++)
                {
                    axMapControl1.get_Layer(i).Visible = false;
                }
                cloudcount = hourlyrain;
                //计时器启动
                timer2.Elapsed += new ElapsedEventHandler((s, ev) => OnCloudEvent(s, ev));
                timer2.AutoReset = true;
                timer2.Enabled = true;
            }
            display2 = false;
        }

        private void OnCloudEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            string[] raininfo = { "8月4日22时降水雷达图", "8月4日20时降水雷达图", "8月4日18时降水雷达图", "8月4日16时降水雷达图", "8月4日14时降水雷达图", "8月4日12时降水雷达图", "8月4日10时降水雷达图", "8月4日8时降水雷达图", "8月4日6时降水雷达图", "8月4日4时降水雷达图", "8月4日2时降水雷达图", "8月4日0时降水雷达图" };
            if (cloudcount == 0)
            {
                cloudcount = hourlyrain;
                timer2.Enabled = false;
                barStaticItem1.Caption = "";
                display2 = true;
                for (int i = 0; i < hourlyrain; i++)
                {
                    axMapControl1.get_Layer(i).Visible = false;
                }
                for (int i = 0; i < hourlyrain; i++)
                {
                    ILayer layer=axMapControl1.get_Layer(0);
                    axMapControl1.Map.DeleteLayer(layer);
                }
                this.axMapControl1.ActiveView.Refresh();
            }
            else
            {
                for (int i = hourlyrain; i > 0; i--)
                {
                    if (i == cloudcount)
                    {
                        axMapControl1.get_Layer(i - 1).Visible = true;
                    }
                }
                barStaticItem1.Caption = raininfo[cloudcount-1];
                cloudcount--;
                //刷新选中的图层
                //axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, axMapControl1.get_Layer(cloudcount), axMapControl1.ActiveView.Extent);
                IViewRefresh viewRefresh = axMapControl1.Map as IViewRefresh;
                viewRefresh.ProgressiveDrawing = true;
                viewRefresh.RefreshItem(axMapControl1.get_Layer(cloudcount));
            }
        }

        //浙江--雨量等值线
        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(contourFlag==0)
            {
                DataOperator dt = new DataOperator(axMapControl1.Map);
                IDW iDW = new IDW(dt);
                iDW.Contour(axMapControl1);     //生成等值线并加载到axMapControl上
                contourFlag = 1;
                MapComposer.ZoomtoLayers("Contour", axMapControl1);
            }
            else
            {
                contourFlag = 0;
                DataOperator.DeleteLayer(axMapControl1,"Contour");  //删掉之前存在的
            }
            

        }
        
        //浙江--起始日期
        private void barEditItem1_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        void barButtonExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Exit
            this.Close();
        }

        //全局窗口的播放按钮
        private void barButtonDisplay_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(display==true)
            {
                DataOperator dataOperator = new DataOperator(axMapControl1.Map);
                MapComposer.ZoomtoLayers("Typhoon", axMapControl1);
                if (loadflag == false)
                {
                    dataOperator.GetLayerByName("Typhoon").Visible = true;
                    ILayer pLayer = axMapControl1.get_Layer(0);
                    IGeoFeatureLayer pGeoFl = pLayer as IGeoFeatureLayer;
                    IFeatureIDSet pIdSet = new FeatureIDSetClass();
                    for (int i = 0; i <= typhooncount; i++)
                    {
                        pIdSet.Add(i);
                    }
                    pGeoFl.ExclusionSet = pIdSet;
                    //axMapControl1.Refresh();
                    IViewRefresh viewRefresh = axMapControl1.Map as IViewRefresh;
                    viewRefresh.ProgressiveDrawing = true;
                    viewRefresh.RefreshItem(pLayer);
                    loadflag = true;
                }
                //加载空白底图
                if (startflag == false)
                {
                    startflag = true;
                }
                timerflag = true;
                timer1 = new System.Timers.Timer();
                timer1.Interval = 600;//时间间隔
                //全局台风shp的文件路径和文件名
                string pFilePath = "..\\..\\..\\typhoondata\\typhoon", pFileName = "Typhoon.shp";
                //获取全局台风shp文件的要素集
                IFeatureClass featureClass = DataOperator.GetFeatureClass(pFilePath, pFileName);
                IFeatureCursor featureCursor = featureClass.Search(null, false);
                int indexlat = featureClass.FindField("纬度");
                int indexlon = featureClass.FindField("经度");
                int indextype = featureClass.FindField("type");
                int indexpower = featureClass.FindField("风力");
                int indexpressure = featureClass.FindField("气压");
                int indexspeed = featureClass.FindField("风速");
                //计时器启动
                timer1.Elapsed += new ElapsedEventHandler((s, ev) => OnDisplayEvent(s, ev, featureClass, indexlat, indexlon, indextype, indexpower, indexpressure, indexspeed));
                timer1.AutoReset = true;
                timer1.Enabled = true;
            }
            display = false;
        }

        //添加点到BaseLayer上
        private void OnDisplayEvent(object source, System.Timers.ElapsedEventArgs e, IFeatureClass featureClass, int indexlat, int indexlon, int indextype, int indexpower, int indexpressure, int indexspeed)
        {
            if (timerflag == false) timer1.Enabled = false;
            else
            {
                if (count <= typhooncount)
                {
                    IFeature pfeature;
                    ILayer pLayer = axMapControl1.get_Layer(0);
                    IGeoFeatureLayer pGeoFl = pLayer as IGeoFeatureLayer;
                    IFeatureIDSet pIdSet = new FeatureIDSetClass();
                    for (int i = count; i <= typhooncount; i++)
                    {
                        pIdSet.Add(i);
                    }
                    pGeoFl.ExclusionSet = pIdSet;
                    //axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography,axMapControl1.get_Layer(0), axMapControl1.ActiveView.Extent);
                    IViewRefresh viewRefresh = axMapControl1.Map as IViewRefresh;
                    viewRefresh.ProgressiveDrawing = true;
                    viewRefresh.RefreshItem(pLayer);
                    IFeatureCursor featureCursor = featureClass.Search(null, false);
                    for (int i = 0; i < count; i++)
                    {
                        featureCursor.NextFeature();
                    }
                    pfeature = featureCursor.NextFeature();
                    string lat = pfeature.get_Value(indexlat).ToString();
                    string lon = pfeature.get_Value(indexlon).ToString();
                    string type = pfeature.get_Value(indextype).ToString();
                    string power = pfeature.get_Value(indexpower).ToString();
                    string pressure = pfeature.get_Value(indexpressure).ToString();
                    string speed = pfeature.get_Value(indexspeed).ToString();
                    barStaticItem1.Caption = String.Format("当前台风位置：" + "经度 " + lon + "° 纬度 " + lat + "°，风力：" + power + "，气压：" + pressure + "，风速：" + speed + "，类型：" + type);
                    count++;
                }
                else
                {
                    count = 0;
                    startflag = false;
                    timerflag = false;
                    barStaticItem1.Caption = "";
                    display = true;
                }
            }
        }

        //显示路径
        private void barButtonPathDisplay_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataOperator dataOperator = new DataOperator(axMapControl1.Map);
            ILayer layer = dataOperator.GetLayerByName("Typhoon");
            if (wholeclick==true)
            {
                layer.Visible = true;
                //处理播放到一半的情况
                ILayer pLayer = axMapControl1.get_Layer(0);
                IGeoFeatureLayer pGeoFl = pLayer as IGeoFeatureLayer;
                IFeatureIDSet pIdSet = new FeatureIDSetClass();
                pIdSet.Clear();
                pGeoFl.ExclusionSet = pIdSet;
                //axMapControl1.Extent = axMapControl1.FullExtent;
                wholeclick = false;
            }
            else
            {
                wholeclick = true;
                layer.Visible = false;
            }
            this.axMapControl1.ActiveView.Refresh();
            //IViewRefresh viewRefresh = axMapControl1.Map as IViewRefresh;
            //viewRefresh.ProgressiveDrawing = true;
            //viewRefresh.RefreshItem(layer);
        }


        //按属性查询，要弹出一个新的框
        private void barButtonByAttribute_ItemClick(object sender, ItemClickEventArgs e)
        {
            //新创建属性查询窗体

            QueryByAttribute formQueryByAttribute = new QueryByAttribute(featureSelection);
  

            //在主窗体中进行了参数的传递
            formQueryByAttribute.CurrentMap = axMapControl1.Map;
            //显示属性查询窗体
            formQueryByAttribute.Show();

        }

        //浙江--路径
        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            DataOperator dataOperator = new DataOperator(axMapControl1.Map);
            ILayer layer = dataOperator.GetLayerByName("Typhoon");
            if (zjclick==true)
            {
                layer.Visible = true;
                //处理播放到一半的情况
                ILayer pLayer = axMapControl1.get_Layer(0);
                IGeoFeatureLayer pGeoFl = pLayer as IGeoFeatureLayer;
                IFeatureIDSet pIdSet = new FeatureIDSetClass();
                pIdSet.Clear();
                pGeoFl.ExclusionSet = pIdSet;
                //缩放到浙江范围
                IGraphicsContainer pGraContainer = axMapControl1.Map as IGraphicsContainer;
                pGraContainer.DeleteAllElements();
                MapComposer.ZoomtoLayers("Towns", axMapControl1);
                ILayer ly = dataOperator.GetLayerByName("Towns");
                DataOperator.AddLable(axMapControl1, ly, "name");
                zjclick = false;
            }
            else
            {
                IGraphicsContainer pGraContainer = axMapControl1.Map as IGraphicsContainer;
                pGraContainer.DeleteAllElements();
                zjclick = true;
                layer.Visible = false;
            }
            axMapControl1.Refresh();
        }

        //前24小时降水最多的10个站点柱状图显示
        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            string sql = "select 站码, sum(雨量) as sump from 黑格比雨量 where 时间 between '2020/8/2 0:00:00' and '2020/8/3 0:00:00' group by 站码 order by sum(雨量) desc";
            DataLoad dataLoad = new DataLoad();
            DataTable dt = new DataTable();
            dt.Columns.Add("站码", typeof(string));
            dt.Columns.Add("降雨量", typeof(double));
            SqlDataReader dataReader = dataLoad.ExecuteSQL(sql);
            for(int i=0;i<10;i++)
            {
                dataReader.Read();
                dt.Rows.Add(dataReader["站码"].ToString().Trim(), 
                    Convert.ToDouble(dataReader["sump"].ToString().Trim()));              
            }
            PrecipitationRank prank = new PrecipitationRank();
            prank.LoadAll(dt, "前24小时雨量最大的10个测站");
            prank.Visible = true;
        }
        
        //浙江--时间
        private void barEditItem4_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
        
        //浙江--地区
        private void barEditItem5_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        //浙江降水
        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            DataOperator dataOperator = new DataOperator(axMapControl1.Map);
            DataLoad dataLoad = new DataLoad();

            string s = "select sum(雨量) as sump from 黑格比雨量 where 时间 between '2020/8/2 0:00:00' and '2020/8/3 0:00:00', ";
            //dataOperator.AddPrecipitation(dataOperator, s); //第一次运行时执行

            if(idwdraw==0)
            {
                IDW idw = new IDW(dataOperator);
                idw.ShowResult(axMapControl1);
                idwdraw = 1;
            }
            else
            {
                int index = dataOperator.GetLayerIndexByName("IDW");
                axMapControl1.DeleteLayer(index);
                idwdraw = 0;
            }


        }

        //地市24小时降水过程
        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataLoad dataLoad = new DataLoad();
            string city=null;
            string date=null;
            int dateindex=-1;
            try
            {
                city = barEditItem5.EditValue.ToString();
                date = barEditItem4.EditValue.ToString();
                dateindex=repositoryItemComboBox4.Items.IndexOf(date);
            }
            catch
            {
                MessageBox.Show("请选择地区和日期！");
            }
            string[] dateref ={"2020/8/2 0:00:00", "2020/8/3 0:00:00", "2020/8/4 0:00:00", "2020/8/5 0:00:00", "2020/8/5 23:00:00" };
            if (city!=null)
            {
                IFeatureCursor featureCursor = MapAnalysis.GetStandsInCity(city);
                IFeature feature;
                string sql = "select 时间,sum(雨量) as sump from 黑格比雨量 where 站码 in(";
                if((feature = featureCursor.NextFeature()) != null)
                {
                    sql = sql +"'"+ DataOperator.GetFeatureValue(feature, "站码") + "'";
                }
                else
                {
                    MessageBox.Show("该区域无站点");
                    return;
                }
                while((feature = featureCursor.NextFeature()) != null)
                {
                    sql = sql + "," + "'" + DataOperator.GetFeatureValue(feature, "站码") + "'";
                }
                sql = sql+") group by 时间 having 时间 between '"+dateref[dateindex]+"' and '"+ dateref[dateindex+1] + "' order by 时间";
                //sql = sql + ") group by 时间";
                DataTable dt = new DataTable();
                dt.Columns.Add("时间", typeof(string));
                dt.Columns.Add("降雨量", typeof(double));
                SqlDataReader dataReader = dataLoad.ExecuteSQL(sql);
                while (dataReader.Read())
                {
                    dt.Rows.Add(dataReader["时间"].ToString().Trim(),
                        Convert.ToDouble(dataReader["sump"].ToString().Trim()));
                }
                RegionPrecipitation chart = new RegionPrecipitation();
                chart.LoadAll(dt, city, date);
                chart.Visible = true;
            }
        }
        //浙江--起始日期
        private void barEditItem1_ItemClick_1(object sender, ItemClickEventArgs e)
        {

        }

        //按节点--登陆按钮函数
        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            //获得大陆的ifeature，获得路径点的ifeatureclass
            DataOperator dataOperator= new DataOperator(axMapControl1.Map);
            IFeatureClass lands= dataOperator.GetFeatureClass("Country");
            IFeatureClass featureClass = dataOperator.GetFeatureClass("Typhoon");

            //取中国大陆上的点
            //取中国
            IFeature China = Query.FindFeatureByAttribute(lands, "CNTRY_NAME", "China");
            //IFeatureLayer featureLayer = dataOperator.GetLayerByName("Country") as IFeatureLayer;
            //featureSelection = featureLayer as IFeatureSelection;
            //featureSelection.Add(China);
            //axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, axMapControl1.ActiveView.Extent);
            IFeatureCursor candidate = MapAnalysis.GetIntersect(China, featureClass);
            //找时间最小的点
            IFeature feature = Query.GetMinFeatureByFeild(candidate, "Time");
            //高亮显示此feature
            IFeatureLayer featureLayer = dataOperator.GetLayerByName("Typhoon") as IFeatureLayer;
            featureSelection = featureLayer as IFeatureSelection;
            featureSelection.Clear();
            featureSelection.Add(feature);
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, axMapControl1.ActiveView.Extent);
        }



        //实现地图移动，如果处于绘制状态则进行绘制并获得与绘制区域相交的站点cursor
        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if(drawOrNot==0)
                axMapControl1.Pan();
            else
            {
                DataOperator dt = new DataOperator(axMapControl1.Map);
               
               //绘制多边形 
                IActiveView pActiveView = (IActiveView)axMapControl1.Map;
                IPolygon pPolygon = axMapControl1.TrackPolygon() as IPolygon;   //鼠标双击完成绘制
                ISimpleFillSymbol pSimpleFillsym = new SimpleFillSymbolClass();
                pSimpleFillsym.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                pSimpleFillsym.Color = ColorHelper.makeRGBColor(255, 0, 0);
                IFillShapeElement pPolygonEle = new PolygonElementClass();
                pPolygonEle.Symbol = pSimpleFillsym;
                ((IElement)pPolygonEle).Geometry = pPolygon;
                //将绘制的多边形保存并显示在地图上
                pContainer.AddElement((IElement)pPolygonEle, 0);
                drawOrNot = 0;
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                

                //获取多边形内的监测站点
                IGeometry geometry = pPolygon;
                IFeatureLayer featureLayer = dt.GetLayerByName("station")as IFeatureLayer;
                IFeatureClass featureClass = featureLayer.FeatureClass;
                //返回选中站点的cursor，这个是类中变量，可以在函数间使用
                cursorOfDraw = MapAnalysis.GetIntersect(geometry, featureClass);


                //高亮显示区域内的站点
              
                int fieldIndexp = featureClass.Fields.FindField("p");
                IFeature feature=null;
                int count = 0;
                while ((feature = cursorOfDraw.NextFeature()) != null)
                {
                    featureSelection.Add(feature);
                    count++;
                }
                cursorOfDraw = MapAnalysis.GetIntersect(geometry, featureClass);
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, axMapControl1.ActiveView.Extent);
            }

        }

        //播放暂停
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            //修改计时器状态
            if (timerflag == true)
                timerflag = false;
            display = true;
        }

        //自定义polygon，这里只是一个启动按钮，绘制是在OnMouseDown那里
        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            drawOrNot = drawOrNot == 1 ? 0 : 1;
            MapComposer.ZoomtoLayers("Towns", axMapControl1);

            //清空之前绘制的
            pContainer.DeleteAllElements(); //清除容器中的图层
            //取消高亮显示并将之前的cursor变为null
            DataOperator dt = new DataOperator(axMapControl1.Map);
            IFeatureLayer featureLayer = dt.GetLayerByName("station") as IFeatureLayer;
            featureSelection = featureLayer as IFeatureSelection;
            featureSelection.Clear();
            cursorOfDraw = null;
        } 

        //高亮显示离开大陆点
        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            //获得大陆的ifeature，获得路径点的ifeatureclass
            DataOperator dataOperator = new DataOperator(axMapControl1.Map);
            IFeatureClass lands = dataOperator.GetFeatureClass("Country");
            IFeatureClass featureClass = dataOperator.GetFeatureClass("Typhoon");

            //取中国大陆上的点
            //取中国
            IFeature China = Query.FindFeatureByAttribute(lands, "CNTRY_NAME", "China");
            //IFeatureLayer featureLayer = dataOperator.GetLayerByName("Country") as IFeatureLayer;
            //featureSelection = featureLayer as IFeatureSelection;
            //featureSelection.Add(China);
            //axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, axMapControl1.ActiveView.Extent);
            IFeatureCursor candidate = MapAnalysis.GetIntersect(China, featureClass);
            //找时间最大的点
            IFeature feature = Query.GetMaxFeatureByFeild(candidate, "Time");
            //高亮显示此feature
            IFeatureLayer featureLayer = dataOperator.GetLayerByName("Typhoon") as IFeatureLayer;
            featureSelection = featureLayer as IFeatureSelection;
            featureSelection.Clear();
            featureSelection.Add(feature);
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, axMapControl1.ActiveView.Extent);
        }
        //高亮显示生成点
        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //获得大陆的ifeature，获得路径点的ifeatureclass
            DataOperator dataOperator = new DataOperator(axMapControl1.Map);
            IFeatureClass featureClass = dataOperator.GetFeatureClass("Typhoon");
            //找时间最大的点
            IFeature feature = Query.GetMinFeatureByFeild(featureClass, "Time");
            //高亮显示此feature
            IFeatureLayer featureLayer = dataOperator.GetLayerByName("Typhoon") as IFeatureLayer;
            featureSelection = featureLayer as IFeatureSelection;
            featureSelection.Clear();
            featureSelection.Add(feature);
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, axMapControl1.ActiveView.Extent);
        }
        //高亮级别变化点
        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataOperator dataOperator = new DataOperator(axMapControl1.Map);
            IFeatureClass typhoon = dataOperator.GetFeatureClass("Typhoon");
            //获得等级唯一值列表
            List<string> values = null;
            values = DataOperator.GetUniqueValues(typhoon, "type");
            //根据属性筛选出每个等级的featureclass，并对此筛选出时间最小值，添加到selection中
            IFeatureLayer featureLayer = dataOperator.GetLayerByName("Typhoon") as IFeatureLayer;
            featureSelection = featureLayer as IFeatureSelection;
            featureSelection.Clear();
            foreach (string s in values)
            {
                IFeatureCursor cursor = Query.FindFeatureClassByAttribute(typhoon, "type", s);
                IFeature point = Query.GetMinFeatureByFeild(cursor, "Time");
                //高亮显示此feature
                featureSelection.Add(point);
            }
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, axMapControl1.ActiveView.Extent);

        }

        //导入矢量文件shp,并获得与该文件相交的站点cursor
        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {

            MapComposer.ZoomtoLayers("Towns", axMapControl1);
            DataOperator dt = new DataOperator(axMapControl1.Map);
            //规定一次只能导入一个流域，把之前导入的删掉,流域名字都叫RiverRegion
            DataOperator.DeleteLayer(axMapControl1, "RiverRegion");
            

            IFeatureClass buffer = dt.LoadShpFromDialog();
            if (buffer != null)
            {
                IFeatureLayer featureLayer = dt.GetLayerByName("station") as IFeatureLayer;
                IFeatureClass featureClass = featureLayer.FeatureClass;
                cursorOfDraw = MapAnalysis.GetIntersect(buffer, featureClass);
                IFeature feature;
                int count = 0;
                while ((feature = cursorOfDraw.NextFeature()) != null)
                {
                    count++;
                }
            }
        }

        //导入文件地理数据库，但暂时无法实现查询
        private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            //IWorkspace workspace = DataOperator.GetFileGDBWorkspace();
            //DataOperator.AddAllDataset(workspace, axMapControl1);

            //怎么弄成cursor还没搞，featureClass有点多
        }

        //30千米内站点10个最大站点
        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            string date = null;
            try
            {
                //date = ConvertDate(barEditItem7.EditValue.ToString());
                date = barEditItem7.EditValue.ToString();
                date = ConvertDate(date);
            }
            catch
            {
                MessageBox.Show("请选择时间！");return;
            }
            
            DataLoad dataLoad = new DataLoad();
            DataOperator dataOperator = new DataOperator(axMapControl1.Map);
            IFeatureClass featureClass1 = dataOperator.GetFeatureClass("Typhoon");
            IFeature point = Query.FindFeatureByAttribute(featureClass1, "Time", date);
            IFeatureCursor featureCursor = MapAnalysis.GetStandsInPointBuffer(point,0.3);
            IFeature feature;
            IFeatureLayer featureLayer = dataOperator.GetLayerByName("station") as IFeatureLayer;
            featureSelection = featureLayer as IFeatureSelection;
            featureSelection.Clear();
           

            string sql = "select 站码, sum(雨量) as sump from 黑格比雨量 where 站码 in(";
            if ((feature = featureCursor.NextFeature()) != null)
            {
                sql = sql + "'" + DataOperator.GetFeatureValue(feature, "站码") + "'";
                featureSelection.Add(feature);
            }
            else
            {
                MessageBox.Show("该区域无站点");
                return;
            }
            while ((feature = featureCursor.NextFeature()) != null)
            {
                sql = sql + "," + "'" + DataOperator.GetFeatureValue(feature, "站码") + "'";
                featureSelection.Add(feature);
            }
            sql = sql + ") group by 站码 order by sum(雨量) desc";

            DataTable dt = new DataTable();
            dt.Columns.Add("站码", typeof(string));
            dt.Columns.Add("降雨量", typeof(double));
            SqlDataReader dataReader = dataLoad.ExecuteSQL(sql);
            int i = 0;
            while ((dataReader.Read()) & i < 10)
            {
                dt.Rows.Add(dataReader["站码"].ToString().Trim(),
                    Convert.ToDouble(dataReader["sump"].ToString().Trim()));
                i++;
            }
            PrecipitationRank prank = new PrecipitationRank();
            prank.LoadAll(dt, barEditItem7.EditValue.ToString() + "台风周围30公里雨量最大的10个站点");
            prank.Visible = true;
            MapComposer.ZoomtoLayers("station", axMapControl1);
        }

        private void barButtonItem22_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataLoad dataLoad = new DataLoad();
            string date = null;
            int dateindex = -1;
            IFeature feature;
            try
            {
                date = barEditItem6.EditValue.ToString();
                dateindex = repositoryItemComboBox6.Items.IndexOf(date);
            }
            catch
            {
                MessageBox.Show("请选择日期！");return;
            }
            string[] dateref = { "2020/8/2 0:00:00", "2020/8/3 0:00:00", "2020/8/4 0:00:00", "2020/8/5 0:00:00", "2020/8/5 23:00:00" };
            if (cursorOfDraw != null)
            {
                string sql = "select 时间,sum(雨量) as sump from 黑格比雨量 where 站码 in(";
                if ((feature = cursorOfDraw.NextFeature()) != null)
                {
                    sql = sql + "'" + DataOperator.GetFeatureValue(feature, "站码") + "'";
                }
                else
                {
                    MessageBox.Show("该区域无站点");
                    return;
                }
                while ((feature = cursorOfDraw.NextFeature()) != null)
                {
                    sql = sql + "," + "'" + DataOperator.GetFeatureValue(feature, "站码") + "'";
                }
                sql = sql + ") group by 时间 having 时间 between '" + dateref[dateindex] + "' and '" + dateref[dateindex + 1] + "' order by 时间";
                //sql = sql + ") group by 时间";
                DataTable dt = new DataTable();
                dt.Columns.Add("时间", typeof(string));
                dt.Columns.Add("降雨量", typeof(double));
                SqlDataReader dataReader = dataLoad.ExecuteSQL(sql);
                while (dataReader.Read())
                {
                    dt.Rows.Add(dataReader["时间"].ToString().Trim(),
                        Convert.ToDouble(dataReader["sump"].ToString().Trim()));
                }
                RegionPrecipitation chart = new RegionPrecipitation();
                chart.LoadAll(dt, "流域", date);
                chart.Visible = true;
            }
            else
            {
                MessageBox.Show("请绘制流域范围！");
            }
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            pContainer.DeleteAllElements();
            string date=null;
            try
            {
                 date = ConvertDate(barEditItem7.EditValue.ToString());
            }
            catch
            {
                MessageBox.Show("请选择时间！");
                return;
            }
            DataOperator dataOperator = new DataOperator(axMapControl1.Map);
            if (axMapControl1.get_Layer(0).Name != "typhoon")
            {
                //全局台风shp的文件路径和文件名
                ILayer layer = dataOperator.GetLayerByName("Typhoon");
                layer.Visible = true;
            }
            IFeatureClass featureClass = dataOperator.GetFeatureClass("Typhoon");
            IFeature feature = Query.FindFeatureByAttribute(featureClass, "Time", date);
            IFeatureLayer featureLayer2 = dataOperator.GetLayerByName("station") as IFeatureLayer;
            featureSelection = featureLayer2 as IFeatureSelection;
            featureSelection.Clear();
            //MapComposer.ZoomtoLayers("Typhoon", axMapControl1);


            //画风圈
            for(int i=0;i<3;i++)
            {
                string fengquan = feature.get_Value(12+i).ToString();
                if (fengquan != " ")
                {
                    IPoint point = feature.Shape as IPoint;
                    MapComposer.DrawFengQuan(axMapControl1.Map, point, fengquan, pContainer);
                }
            }
            //定义新的IEnvelope接口对象获取该要素的空间范围
            ESRI.ArcGIS.Geometry.IEnvelope outEnvelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            //通过IGeometry接口的QueryEnvelope方法获取要素的空间范围
            feature.Shape.QueryEnvelope(outEnvelope);
            outEnvelope.Expand(1, 1, false);
            
            //将主窗体地图的当前可视范围定义为要素的空间范围，并刷新地图
            IActiveView activeView = axMapControl1.Map as IActiveView;
            activeView.Extent = outEnvelope;
            activeView.Refresh();
        }

        private string ConvertDate(string str)
        {
            //对日期的格式进行调整，使之与属性表中的日期格式一致
            string date = null;
            string str1 = null;
            string str2 = null;
            string str3 = null;
            string str4 = null;
            int i;
            for (i = 0; i < 4; i++)
            {
                str1 = str1 + str[i];
            }
            i++;
            while (str[i] != '/')
            {
                str2 = str2 + str[i];
                i++;
            }
            if (str2.Length == 1) str2 = '0' + str2;
            i++;
            while (str[i] != ' ')
            {
                str3 = str3 + str[i];
                i++;
            }
            if (str3.Length == 1) str3 = '0' + str3;
            i++;
            while (str[i] != ':')
            {
                str4 = str4 + str[i];
                i++;
            }
            if (str4.Length == 1) str4 = '0' + str4;
            date = str1 + str2 + str3 + str4;


            DataOperator dataOperator = new DataOperator(axMapControl1.Map);
            IFeatureClass featureClass = dataOperator.GetFeatureClass("Typhoon");
            IFeature feature = Query.FindFeatureByAttribute(featureClass, "Time", date);
            string TIME = feature.get_Value(3).ToString();
            string lat = feature.get_Value(4).ToString();
            string lon = feature.get_Value(5).ToString();
            string Type = feature.get_Value(6).ToString();
            string power = feature.get_Value(9).ToString();
            string pressure = feature.get_Value(10).ToString();
            string position = "不在浙江境内";
            IFeatureClass fc = dataOperator.GetFeatureClass("Towns");
            IFeatureCursor featureCursor = MapAnalysis.GetIntersect(feature, fc);
            IFeature temp;
            if ((temp = featureCursor.NextFeature()) != null)
            {
                position = temp.get_Value(4).ToString();
            }
            CenterInfo centerpos = new CenterInfo();
            centerpos.TIME = TIME;
            centerpos.lat = lat;
            centerpos.lon = lon;
            centerpos.Type = Type;
            centerpos.power = power;
            centerpos.pressure = pressure;
            centerpos.position = position;
            centerpos.UpdateInfo();
            centerpos.Show();
            IFeatureLayer featureLayer = dataOperator.GetLayerByName("Typhoon") as IFeatureLayer;
            featureSelection = featureLayer as IFeatureSelection;
            featureSelection.Clear();
            featureSelection.Add(feature);
            return date;

        }
        
        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            string start = "";
            string end = "";
            try
            {
                start = this.barEditItem1.EditValue.ToString();
                end = this.barEditItem2.EditValue.ToString();
            }
            catch
            {
                MessageBox.Show("请选择起始和终止日期！");
                return;
            }
            AttributeTable rainTable = new AttributeTable(axMapControl1, start, end);
            rainTable.Text = "雨量分布表";
            rainTable.Show();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            IntroductionHGB hGB = new IntroductionHGB();
            hGB.Show();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataOperator.OutputAsPhoto(axMapControl1.ActiveView);
        }
    }
}