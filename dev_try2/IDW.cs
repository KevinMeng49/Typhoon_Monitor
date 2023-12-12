using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SpatialAnalyst;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Controls;

namespace dev_try2
{
    class IDW
    {
        private IRasterAnalysisEnvironment rasterEnv;//分析环境
        private IInterpolationOp2 interOp;//空间插值对象
        private IFeatureClass feaClass;
        private IFeatureClassDescriptor feaDes;
        private IGeoDataset inGeodataset;//输入栅格
        private IGeoDataset outGeodataset;//输出栅格
        private double cellSize;//输出像元大小
        private object Missing = Type.Missing;
        private object cellSizeObj;
        private object extentProObj;//处理范围
        private IRasterRadius radius;
        private double power;
        private DataOperator dt;


        public IDW(DataOperator dataoperator)
        {
            dt = dataoperator;
            interOp = new RasterInterpolationOpClass();
            //rasterEnv = new RasterInterpolationOpClass();
            
            ILayer layer = dataoperator.GetLayerByName("station");
            IFeatureLayer fl = layer as IFeatureLayer;
            feaClass = fl.FeatureClass;

            radius = new RasterRadiusClass();
            //radius.SetFixed(2500, Missing);
            radius.SetVariable(12, Missing);

            feaDes = new FeatureClassDescriptorClass();
            feaDes.Create(feaClass, null, "p");
            inGeodataset = feaDes as IGeoDataset;

            rasterEnv = new RasterAnalysis();

            cellSize = 0.0159;
            cellSizeObj = cellSize;

            IGeoDataset geo = dataoperator.GetLayerByName("Towns") as IGeoDataset;
            rasterEnv = interOp as IRasterAnalysisEnvironment;
            rasterEnv.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, ref cellSizeObj);
            extentProObj = layer;
            rasterEnv.Mask = geo;
            rasterEnv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, ref extentProObj, Missing);
            
            power = 2;
            
            //interOp = rasterEnv as IInterpolationOp2;
            outGeodataset = interOp.IDW(inGeodataset, power, radius, ref Missing);//反距离权重法
        }

        public IDW(DataOperator dataoperator, string lyrname)
        {
            dt = dataoperator;
            interOp = new RasterInterpolationOpClass();
            //rasterEnv = new RasterInterpolationOpClass();
            
            ILayer layer = dataoperator.GetLayerByName(lyrname);
            IFeatureLayer fl = layer as IFeatureLayer;
            feaClass = fl.FeatureClass;
            //feaClass = featureclass;

            radius = new RasterRadiusClass();
            //radius.SetFixed(2500, Missing);
            radius.SetVariable(12, Missing);

            feaDes = new FeatureClassDescriptorClass();
            feaDes.Create(feaClass, null, "p");
            inGeodataset = feaDes as IGeoDataset;

            rasterEnv = new RasterAnalysis();

            cellSize = 0.002;
            cellSizeObj = cellSize;

            rasterEnv = interOp as IRasterAnalysisEnvironment;
            rasterEnv.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, ref cellSizeObj);
            extentProObj = feaClass;
            rasterEnv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, ref extentProObj, Missing);

            power = 2;

            //interOp = rasterEnv as IInterpolationOp2;
            outGeodataset = interOp.IDW(inGeodataset, power, radius, ref Missing);//反距离权重法
        }

        //显示分析结果
        public void ShowResult(AxMapControl axMapControl1)
        { 
            IRasterLayer rasterLayer = new RasterLayer();
            IRaster raster = new Raster();
            raster = (IRaster)outGeodataset;
            rasterLayer.CreateFromRaster(raster);
            rasterLayer.Name = "IDW";
            rasterLayer.Renderer = Render.IDWRenderer(rasterLayer);
            axMapControl1.AddLayer((IRasterLayer)rasterLayer, 1);
            MapComposer.ZoomtoLayers("IDW", axMapControl1);
            IViewRefresh viewRefresh = axMapControl1.Map as IViewRefresh;
            viewRefresh.ProgressiveDrawing = true;
            viewRefresh.RefreshItem(rasterLayer);
            //axMapControl1.ActiveView.Refresh();
        }
      

        //降水等值线操作
        public IGeoDataset GetInterpolateRaster()
        {
            return outGeodataset;
        }
        public void Contour(AxMapControl axMapControl)
        {
            ISurfaceOp surfaceOp = new RasterSurfaceOpClass();
            IGeoDataset geo = surfaceOp.Contour(outGeodataset, 20, Missing);
            ShowVectorResult(axMapControl, geo, "Contour");
        }

        private void ShowVectorResult(AxMapControl ax,IGeoDataset geo, string name)
        {
            IFeatureClass fc = geo as IFeatureClass;
            IFeatureLayer fl = new FeatureLayerClass();
            fl.FeatureClass = fc;
            fl.Name = name;
            ax.AddLayer((ILayer)fl, 0);
            IViewRefresh viewRefresh = ax.Map as IViewRefresh;
            viewRefresh.ProgressiveDrawing = true;
            viewRefresh.RefreshItem(fl);
            //ax.ActiveView.Refresh();

        }
    }
}
