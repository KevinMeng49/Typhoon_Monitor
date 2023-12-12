using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;

namespace dev_try2
{
    class MapComposer
    {

        public static String GetRendererTypeByLayer(ILayer layer)
        {
            if (layer == null)
                return ("获取图层失败");
            IGeoFeatureLayer GeoFeatureLayer = layer as IGeoFeatureLayer;
            IFeatureRenderer featureRenderer = GeoFeatureLayer.Renderer;
            if (featureRenderer is ISimpleRenderer)
            {
                return ("SimpleRenderer");
            }
            return ("not coded yet");
        }

        //用于获取指定图层的符号信息
        public static ISymbol GetSymbolFromLayer(ILayer layer)
        {
            if (layer == null)
                return null;
            //利用ifeaturelayer访问指定图层，获取其是否有要素，若失败，返回空
            IFeatureLayer featureLayer = layer as IFeatureLayer;
            IFeatureCursor featureCursor = featureLayer.Search(null, false);
            IFeature feature = featureCursor.NextFeature();
            if (feature == null)
                return null;
            //利用IGeoFeatureLayer访问指定图层，获取其渲染器，并判断是否成功
            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            IFeatureRenderer featureRenderer = geoFeatureLayer.Renderer;
            if (featureRenderer == null)
                return null;
            ISymbol symbol = featureRenderer.get_SymbolByFeature(feature);
            return symbol;
        }



        //用于统一设置指定图层符号的颜色，并进行简单渲染
        public static bool RenderSimply(ILayer layer, IColor color)
        {
            if (layer == null || color == null)
                return false;
            ISymbol symbol = GetSymbolFromLayer(layer);
            IFeatureLayer featureLayer = layer as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            if (featureClass == null)
                return false;
            //根据不同形状设置颜色
            esriGeometryType geometryType = featureClass.ShapeType;
            switch (geometryType)
            {
                case esriGeometryType.esriGeometryPolyline:
                    {
                        ISimpleLineSymbol simpleLineSymbol = symbol as ISimpleLineSymbol;
                        simpleLineSymbol.Color = color;
                        break;
                    }
                default:
                    return false;
            }

            //创建简单渲染器，设置其符号，实现渲染
            ISimpleRenderer simpleRenderer = new SimpleRenderer();
            simpleRenderer.Symbol = symbol;
            IFeatureRenderer featureRenderer = simpleRenderer as IFeatureRenderer;
            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            geoFeatureLayer.Renderer = featureRenderer;
            return true;
        }

        public void HighlightSelect(IFeatureSelection featureSelection,ILayer layer)
        {
            

        }

        public void CancelHighlight(IFeatureSelection featureSelection, ILayer layer)
        {

        }

        //缩放到layer
        public static void ZoomtoLayers(string lyrname, AxMapControl axMapControl1)
        {
            DataOperator dataOperator = new DataOperator(axMapControl1.Map);
            ILayer layer = dataOperator.GetLayerByName(lyrname);
            IGeoDataset pGeoDataset = layer as IGeoDataset;
            IEnvelope pEnvelope = pGeoDataset.Extent;
            axMapControl1.Extent = pEnvelope;
        }

        public static void DrawFengQuan(IMap map,IPoint point,string value,IGraphicsContainer graphicsContainer)
        {
            IActiveView pActiveView = (IActiveView)map;
            if (value == null)
                return;
            double []angle = { 0, 90, 180, 270 };
            double dist=0;
            string str = "";

            //先定义线的符号，之后都要用
            ISimpleLineSymbol lineSymbol = new SimpleLineSymbolClass();
            lineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            lineSymbol.Color = ColorHelper.makeRGBColor(253, 245, 37);
            lineSymbol.Width = 3.5;

            int j = 0;
            value=value+'\0';
            for (int i=0;i<4;i++)
            {
                IPolyline circularFeatPolyline = new PolylineClass();
                ISegmentCollection segementColl = (ISegmentCollection)circularFeatPolyline; //这个会导致弧线连起来
                str = "";
                while (value[j] != '\0' && value[j] != '|')
                {
                    str += value[j];
                    j++;
                }
                j++;
                dist = Convert.ToDouble(str);
                //画直线，即扇形的两边
                for(int k=0;k<2;k++)
                {
                    IPolyline polyline = new PolylineClass();
                    IPointCollection pointColl = polyline as IPointCollection;
                    pointColl.AddPoint(point);
                    IPoint pointEnd = new PointClass();
                    pointEnd.X = point.X + Math.Cos(angle[i] / 180 * Math.PI+k*Math.PI/2) * dist/1000;
                    pointEnd.Y = point.Y + Math.Sin(angle[i] / 180 * Math.PI+k*Math.PI/2) * dist/1000;
                    pointColl.AddPoint(pointEnd);
                    ILineElement ele = new LineElementClass();
                    ele.Symbol = lineSymbol;
                    ((IElement)ele).Geometry = polyline;
                    graphicsContainer.AddElement((IElement)ele, 0);
                }

                //画弧
                ICircularArc circular = new CircularArcClass();
                //弧的参数
                circular.PutCoordsByAngle(point, angle[i]/180*Math.PI, Math.PI/2, dist / 1000);
                ISegment segment = (ISegment)circular;
                //把弧添加到最终的线元素里，通过segment的方式添加
                segementColl.AddSegment(segment);
                ILineElement element = new LineElementClass();
                element.Symbol = lineSymbol;
                ((IElement)element).Geometry = circularFeatPolyline;
                graphicsContainer.AddElement((IElement)element, 0);
                
            }
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
    }
}
