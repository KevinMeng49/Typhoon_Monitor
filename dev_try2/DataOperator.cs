using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using stdole;

namespace dev_try2
{
    class DataOperator
    {
        public IMap m_map;//用于保存当前地图对象
        public IMapControlDefault mapControlDefault;

        public DataOperator(IMap map)
        {
            m_map = map;
        }

        public DataOperator(IMapControlDefault mapControl)
        {
            mapControlDefault = mapControl;
        }

        //获取地图图层功能函数
        public ILayer GetLayerByName(String sLayerName)
        {
            if (sLayerName == "" || m_map == null)
                return null;
            for (int i = 0; i < m_map.LayerCount; i++)
                if (m_map.get_Layer(i).Name == sLayerName)
                    return m_map.get_Layer(i);
            return null;
        }

        public int GetLayerIndexByName(String sLayerName)
        {
            if (sLayerName == "" || m_map == null)
                return -1;
            for (int i = 0; i < m_map.LayerCount; i++)
                if (m_map.get_Layer(i).Name == sLayerName)
                    return i;
            return -1;
        }
        //用于删除已有工作空间中的内容和工作空间       这个有什么用还不太清楚
        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
        //获取某图层属性表函数,暂时没写
        public DataTable GetDataTableByName() { return null; }

        //加载Shapefile文件
        public void LoadNewShapefile(string pFilePath, string pFileName)
        {
            IWorkspaceFactory pWorkspaceFactory;
            IFeatureWorkspace pFeatureWorkspace;
            IFeatureLayer pFeatureLayer;

            //实例化ShapefileWorkspaceFactory工作空间，打开Shape文件
            pWorkspaceFactory = new ShapefileWorkspaceFactory();
            pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(pFilePath, 0);
            //创建并实例化要素集
            IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pFileName);
            pFeatureLayer = new FeatureLayer();
            pFeatureLayer.FeatureClass = pFeatureClass;
            pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;

            m_map.AddLayer(pFeatureLayer);
        }

        //创建shapefile文件
        public void CreateShapefile()
        {
            ISpatialReference pSpatialReference = m_map.SpatialReference;

            string strShapeFolder = "..\\..\\..\\typhoondata\\BaseMap\\";

            string strShapeFile = "BaseLayer.shp";

            string shapeFileFullName = strShapeFolder + strShapeFile;
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();
            IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(strShapeFolder, 0);
            IFeatureClass pFeatureClass;

            if (File.Exists(shapeFileFullName))
            {
                pFeatureClass = pFeatureWorkspace.OpenFeatureClass(strShapeFile);
                IDataset pDataset = pFeatureClass as IDataset;
                pDataset.Delete();
            }

            IFields pFields = new Fields();
            IFieldsEdit pFieldsEdit = (IFieldsEdit)pFields;

            IField pField = new Field();
            IFieldEdit pFieldEdit = (IFieldEdit)pField;

            pFieldEdit.Name_2 = "SHAPE";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            IGeometryDefEdit pGeoDef = new GeometryDefClass();
            IGeometryDefEdit pGeoDefEdit = (IGeometryDefEdit)pGeoDef;
            pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
            pGeoDefEdit.SpatialReference_2 = pSpatialReference;
            pFieldEdit.GeometryDef_2 = pGeoDef;
            pFieldsEdit.AddField(pField);

            pFeatureClass = pFeatureWorkspace.CreateFeatureClass(strShapeFile, pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");

            IFeatureLayer pFeaturelayer = new FeatureLayerClass();
            pFeaturelayer.FeatureClass = pFeatureClass;
            pFeaturelayer.Name = "layer";

            m_map.AddLayer(pFeaturelayer);
        }

        //用途：以某图层名称和某渲染方式将shp添加至地图
        public bool AddFeatureClassToMap(IFeatureClass featureClass, string sLayerName, IFeatureRenderer renderer)
        {
            if (featureClass == null || sLayerName == "" || m_map == null)
            {
                return false;
            }
            IFeatureLayer featureLayer = new FeatureLayer();
            featureLayer.FeatureClass = featureClass;
            featureLayer.Name = sLayerName;
            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            geoFeatureLayer.Renderer = renderer;       //这个哪来的？参数里面的
            ILayer layer = geoFeatureLayer as ILayer;
            if (layer == null)
            {
                return false;
            }
            m_map.AddLayer(layer);
            IActiveView activeView = m_map as IActiveView;
            if (activeView == null)
                return false;
            activeView.Refresh();
            return true;
        }

        //从filename,filepath获得featureclass
        public static IFeatureClass GetFeatureClass(string filePath, string fileName)
        {

            IWorkspaceFactory WorkspaceFactory;//using ESRI.ArcGIS.Geodatabase;
            IFeatureWorkspace FeatureWorkspace;
            //实例化ShapefileWorkspaceFactory工作空间，打开Shape文件
            WorkspaceFactory = new ShapefileWorkspaceFactory();//using ESRI.ArcGIS.DataSourcesFile;
            FeatureWorkspace = (IFeatureWorkspace)WorkspaceFactory.OpenFromFile(filePath, 0);
            //创建并实例化要素集
            IFeatureClass FeatureClass = FeatureWorkspace.OpenFeatureClass(fileName);
            return FeatureClass;
        }

        //从已经加载的图层中获取要素
        public IFeatureClass GetFeatureClass(string layername)
        {
            IFeatureLayer featureLayer= GetLayerByName(layername) as IFeatureLayer;
            return featureLayer.FeatureClass;
        }



        public void InitializeMap()
        {
            string filePath1 = "..//typhoondata//BaseMap";
            string filePath2 = "..//typhoondata//BaseMap";
            mapControlDefault.AddShapeFile(filePath1, "Country.shp");
            mapControlDefault.AddShapeFile(filePath1, "ChinaProvince.shp");
            mapControlDefault.AddShapeFile(filePath1, "SouthSeeIsland.shp");
            mapControlDefault.AddShapeFile(filePath1, "NineSegmentLine.shp");
            mapControlDefault.AddShapeFile(filePath2, "Typhoon.shp");
        }


        //清空除底图外的所有图层，默认底图的名字为basemap
        public void ClearLayer()
        {

        }

        //清空所有图层
        public void ClearAll()
        {
            m_map.ClearLayers();
        }

        //播放与暂停相关函数
        public void AddFeatureWithPause(IFeatureClass featureClass)
        {

        }

        public void AddFeatureWithPause(IRasterDataset rasterDataset)
        {

        }

        public void AddPrecipitation(DataOperator dataOperator, string s)
        {
            IFeature feature;
            DataLoad dataLoad = new DataLoad();
            //定义一循环游标
            IGeoFeatureLayer geoFeatureLayer = dataOperator.GetLayerByName("station") as IGeoFeatureLayer;
            IFeatureCursor featureCursor = geoFeatureLayer.FeatureClass.Search(null, false);
            if (featureCursor != null)
            {
                //查找降水字段、站点编号字段的索引号
                int fieldIndexp = geoFeatureLayer.FeatureClass.Fields.FindField("p");
                int fieldIndexs = geoFeatureLayer.FeatureClass.Fields.FindField("站码");
                //遍历
                while ((feature = featureCursor.NextFeature()) != null)
                {
                    string ID = feature.Value[fieldIndexs].ToString();
                    SqlDataReader dataReader = dataLoad.ExecuteSQL(s + "'"+ ID+ "'");
                    while (dataReader.Read())
                    {
                        string temp_p = dataReader["sump"].ToString().Trim();
                        if (temp_p == "")
                            continue;
                        double p = Convert.ToDouble(temp_p);
                        feature.set_Value(fieldIndexp, p);
                        feature.Store();
                    }
                    dataReader.Close();
                }

            }
        }
        public static string GetFeatureValue(IFeature feature, string feildName)
        {
            IFeatureClass iFeatureClass = feature.Class as IFeatureClass;
            int index = iFeatureClass.Fields.FindField(feildName);
            string result = feature.get_Value(index).ToString();
            return result;
        }

        
        //获得字段所有唯一值
        public static List<string> GetUniqueValues(IFeatureClass featureClass,string field)
        {
            List<string> list = null;
            try
            {
                int index = featureClass.Fields.FindField(field);
                IField pField = featureClass.Fields.get_Field(index);
                esriFieldType type = pField.Type;
                ICursor pCursor = null;
                //IQueryFilter pQueryFilter = new QueryFilter();
                //pQueryFilter.WhereClause = WHERE_CAUSE;
                //IQueryFilterDefinition pQueryFilterDefinition = pQueryFilter as IQueryFilterDefinition;
                //pQueryFilterDefinition.PostfixClause = " order by DLBM";
                pCursor = featureClass.Search(null, false) as ICursor;
                IDataStatistics pDataStaticstics = new DataStatistics();
                pDataStaticstics.Cursor = pCursor;
                pDataStaticstics.Field = pField.Name;
                System.Collections.IEnumerator pEnumerator = pDataStaticstics.UniqueValues;
                pEnumerator.Reset();
                list = new List<string>();
                while (pEnumerator.MoveNext())
                {
                    list.Add(pEnumerator.Current.ToString());
                }
            }
       
            catch (Exception)
            {

                throw;
            }
            return list;
        }


        public IFeatureClass LoadShpFromDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.Title = "Open Shapefile";
            openFileDialog.Filter = "Shape 文件(*.shp)| *.shp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fullpath = openFileDialog.FileName;
                if (fullpath == "")
                    return null;
                int index = fullpath.LastIndexOf("\\");
                string filepath = fullpath.Substring(0, index);
                string filename = fullpath.Substring(index + 1);
                //从文件中获取
                IFeatureClass featureClass = GetFeatureClass(filepath, filename);
                
                //加载该图层
                IFeatureRenderer renderer = Render.GetSimpleRenderer(featureClass, 77, 77, 77);
                AddFeatureClassToMap(featureClass, "RiverRegion",renderer);

                return featureClass;
            }
            return null;
        }

        public static void AddAllDataset(IWorkspace pWorkspace, AxMapControl mapControl)
        {
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTAny);
            pEnumDataset.Reset();
            //将Enum数据集中的数据一个个读到DataSet中
            IDataset pDataset = pEnumDataset.Next();
            //判断数据集是否有数据
            while (pDataset != null)
            {
                if (pDataset is IFeatureDataset)  //要素数据集
                {
                    IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                    IFeatureDataset pFeatureDataset = pFeatureWorkspace.OpenFeatureDataset(pDataset.Name);
                    IEnumDataset pEnumDataset1 = pFeatureDataset.Subsets;
                    pEnumDataset1.Reset();
                    IGroupLayer pGroupLayer = new GroupLayerClass();
                    pGroupLayer.Name = pFeatureDataset.Name;
                    IDataset pDataset1 = pEnumDataset1.Next();
                    while (pDataset1 != null)
                    {
                        if (pDataset1 is IFeatureClass)  //要素类
                        {
                            IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                            pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(pDataset1.Name);
                            if (pFeatureLayer.FeatureClass != null)
                            {
                                pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                                pGroupLayer.Add(pFeatureLayer);
                                mapControl.Map.AddLayer(pFeatureLayer);
                            }
                        }
                        pDataset1 = pEnumDataset1.Next();
                    }
                }
                else if (pDataset is IFeatureClass) //要素类
                {
                    IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                    IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(pDataset.Name);

                    pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                    mapControl.Map.AddLayer(pFeatureLayer);
                }
                else if (pDataset is IRasterDataset) //栅格数据集
                {
                    IRasterWorkspaceEx pRasterWorkspace = (IRasterWorkspaceEx)pWorkspace;
                    IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(pDataset.Name);
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
                    ILayer pLayer = pRasterLayer as ILayer;
                    mapControl.AddLayer(pLayer, 0);
                }
                pDataset = pEnumDataset.Next();
            }

            mapControl.ActiveView.Refresh();
            
        }

        //通过对话框选择加载GDB数据库
        public static IWorkspace GetFileGDBWorkspace()
        {
            IWorkspaceFactory pFileGDBWorkspaceFactory;

            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != DialogResult.OK) return null;
            string pFullPath = dlg.SelectedPath;

            if (pFullPath == "")
            {
                return null;
            }
            pFileGDBWorkspaceFactory = new FileGDBWorkspaceFactoryClass(); //using ESRI.ArcGIS.DataSourcesGDB;

            //获取工作空间
            IWorkspace pWorkspace = pFileGDBWorkspaceFactory.OpenFromFile(pFullPath, 0);
            return pWorkspace;
        }

        public static void DeleteLayer(AxMapControl axMapControl, string name)
        {

            int i=-1;
            if (name == "" || axMapControl.Map == null)
                i = -1;
            else
                for (int j = 0; j < axMapControl.Map.LayerCount; j++)
                    if (axMapControl.Map.get_Layer(j).Name == name)
                    {
                        i = j;
                        break;
                    }
            if(i!=-1)
                axMapControl.DeleteLayer(i);
        }

        public static void OutputAsPhoto(IActiveView m_pActiveView)
        {
            //获取保存文件的路径/建立导出类
            System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.tif|*.tif|*.jpeg|*.jpeg|*.pdf|*.pdf|*.bmp|*.bmp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                IExport pExport = null;
                if (1 == sfd.FilterIndex)
                {
                    pExport = new ExportTIFFClass();
                }
                else if (2 == sfd.FilterIndex)
                {
                    pExport = new ExportJPEGClass();
                }
                else if (3 == sfd.FilterIndex)
                {
                    pExport = new ExportPDFClass();
                }
                else if (4 == sfd.FilterIndex)
                {
                    pExport = new ExportBMPClass();
                }
                pExport.ExportFileName = sfd.FileName;
                // 设置参数
                //默认精度              
                int reslution = 96;
                pExport.Resolution = reslution;
                //获取导出范围
                tagRECT exportRECT = m_pActiveView.ExportFrame;
                IEnvelope pPixelBoundsEnv = new EnvelopeClass();
                pPixelBoundsEnv.PutCoords(exportRECT.left, exportRECT.top, exportRECT.right, exportRECT.bottom);
                pExport.PixelBounds = pPixelBoundsEnv;
                //开始导出，获取DC  
                int hDC = pExport.StartExporting();
                IEnvelope pVisbounds = null;
                ITrackCancel ptrac = null;
                //导出
                m_pActiveView.Output(hDC, (int)pExport.Resolution, ref exportRECT, pVisbounds, ptrac);
                //结束导出
                pExport.FinishExporting();
                //清理导出类
                pExport.Cleanup();
            }
        }


        //图层标注
        public static void AddLable(AxMapControl axMapControl, ILayer layer, string fieldName)

        {

            IRgbColor pColor = new RgbColorClass()

            {

                Red = 0,

                Blue = 0,
                Green = 0
            };
            IFontDisp pFont = new StdFont()
            {
                Name = "宋体",
                Size = 5
            } as IFontDisp;

            ITextSymbol pTextSymbol = new TextSymbolClass()
            {
                Color = pColor,
                Font = pFont,
                Size = 8,
            };

            IGraphicsContainer pGraContainer = axMapControl.Map as IGraphicsContainer;

            //遍历要标注的要素
            IFeatureLayer pFeaLayer = layer as IFeatureLayer;
            IFeatureClass pFeaClass = pFeaLayer.FeatureClass;
            IFeatureCursor pFeatCur = pFeaClass.Search(null, false);
            IFeature pFeature = pFeatCur.NextFeature();
            int index = pFeature.Fields.FindField(fieldName);//要标注的字段的索引
            IEnvelope pEnv = null;
            ITextElement pTextElment = null;
            IElement pEle = null;
            while (pFeature != null)
            {
                //使用地理对象的中心作为标注的位置
                pEnv = pFeature.Extent;
                IPoint pPoint = new PointClass();
                pPoint.PutCoords(pEnv.XMin + pEnv.Width * 0.5, pEnv.YMin + pEnv.Height * 0.5);

                pTextElment = new TextElementClass()
                {
                    Symbol = pTextSymbol,
                    ScaleText = true,
                    Text = pFeature.get_Value(index).ToString()
                };
                pEle = pTextElment as IElement;
                pEle.Geometry = pPoint;
                //添加标注
                pGraContainer.AddElement(pEle, 0);
                pFeature = pFeatCur.NextFeature();
            }
            (axMapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, axMapControl.Extent);
        }

        public void AddRasterToMap (string filepath, string filename, string name)
        {
            IWorkspaceFactory pWorkspaceFactory = new RasterWorkspaceFactoryClass();//利用工厂对象去生成一个raster文件的工作空间
            IRasterWorkspace pRasterWorkspace = (IRasterWorkspace)pWorkspaceFactory.OpenFromFile(filepath, 0);//到指定路径下
            IRasterDataset pRasterDataset = (IRasterDataset)pRasterWorkspace.OpenRasterDataset(filename);//利用要素集去接收对应的raster文件
            IRasterLayer pRasterLayer = new RasterLayerClass();//生成一个矢量图层对象
            pRasterLayer.CreateFromDataset(pRasterDataset);//利用矢量图层对象去创建对应的raster文件
            pRasterLayer.Name = name;
            ILayerEffects myRasterEffects = pRasterLayer as ILayerEffects;
            myRasterEffects.Contrast = 0;
            myRasterEffects.Transparency = 50;
            m_map.AddLayer(pRasterLayer);//添加对应的图层
        }
    }
}
