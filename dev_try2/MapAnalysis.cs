using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;

namespace dev_try2
{
    class MapAnalysis
    {
        //返回featureClass中与buffer区域(buffer可以为geometry或feature)相交的feature
        public static IFeatureCursor GetIntersect(IFeature buffer,IFeatureClass featureClass)
        {
            int count = featureClass.FeatureCount(null);
            IGeometry geom = buffer.Shape;
            ISpatialFilter spatialFilter = new SpatialFilter();
            spatialFilter.Geometry = geom;
            spatialFilter.SpatialRel = ESRI.ArcGIS.Geodatabase.esriSpatialRelEnum.esriSpatialRelIntersects;

            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            return featureCursor;
            
        }
        public static IFeatureCursor GetIntersect(IGeometry buffer, IFeatureClass featureClass)
        {
            IGeometry geom = buffer;
            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = geom;
            spatialFilter.SpatialRel = (ESRI.ArcGIS.Geodatabase.esriSpatialRelEnum)esriSpatialRelationEnum.esriSpatialRelationIntersection;
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, true);
            return featureCursor;
        }
        public static IFeatureCursor GetIntersect(IFeatureClass buffer,IFeatureClass featureClass)
        {
            int count;
            count = buffer.FeatureCount(null);
            IFeatureCursor bufferCursor = buffer.Search(null, false);
            //获取要素数据集中所有要素的所有集合体的并集
                  //定义IGeometry接口对象，存储每一步拓扑操作后得到的几何体
            IGeometry geometry = null;
                  //使用ITopologicalOperator接口进行几何体的拓扑操作
            ITopologicalOperator topologicalOperator;
            IFeature feature = bufferCursor.NextFeature();
            while (feature!=null)
            {
                count--;
                if (geometry != null)
                 {
                        //进行接口转换，使当前接口进行拓扑操作
                   topologicalOperator = geometry as ITopologicalOperator;
                       //执行拓扑合并操作，将当前要素的几何体与已有的几何体进行Union，返回新的合并后的几何体
                   geometry = topologicalOperator.Union(feature.Shape);
                  }
                else
                geometry = feature.Shape;
                feature = bufferCursor.NextFeature();
            }
            //进行求交操作
            return GetIntersect(geometry, featureClass);
        }

        //传入：某个点要素，buffer的范围（公里），传出：buffer要素
        public static IGeometry CreateBuffer(IFeature feature, double distance)
        {
            IGeometry buffer;
            IGeometry IGeom = feature.Shape;
            ITopologicalOperator ipTo = (ITopologicalOperator)IGeom;
            buffer = ipTo.Buffer(distance);
            return buffer;
        }

        //功能：输入站码，返回站点所在城市名
        public static string GetTown(string stcd)
        {
            string townName;
            //获得城市、站点的ifeatureclass
            IFeatureClass stands, towns;
            stands = DataOperator.GetFeatureClass(@"..\..\..\typhoondata\station", "station.shp");
            towns = DataOperator.GetFeatureClass(@"..\..\..\typhoondata\\BaseMap", "Towns_zj.shp");
            //得到待查询站
            IFeature stand = Query.FindFeatureByAttribute(stands, "stcd", stcd);
            if (stand == null)
            {
                MessageBox.Show("站点不存在");
                return null;
            }
            //以站点为buffer对城市进行空间查询
            IFeatureCursor result = GetIntersect(stand, towns); 
            IFeature town = result.NextFeature();
            if (town == null)
            {
                MessageBox.Show("站点不在城市范围内");
            }
            //获取城市名
            townName = DataOperator.GetFeatureValue(town, "name");
            return townName;
        }


        public static IFeatureCursor GetStandsInCity(string City)
        {
            IFeatureClass cities,stands;
            cities=  DataOperator.GetFeatureClass(@"..\..\..\typhoondata\BaseMap", "Towns_zj.shp");
            //得到待查询城市
            IFeature city = Query.FindFeatureByAttribute(cities, "name", City) ;
            //得到所有站点
            stands = DataOperator.GetFeatureClass(@"..\..\..\typhoondata\station", "station.shp");
            //得到相交站点
            return GetIntersect(city, stands);
        }

        //获取某点某距离buffer内的全部站点
        public static IFeatureCursor GetStandsInPointBuffer(IFeature point,double distance)
        {
            IGeometry buffer = CreateBuffer(point, distance);
            //获取站点的featureclass
            IFeatureClass stands = DataOperator.GetFeatureClass(@"..\..\..\typhoondata\station", "station.shp");
            return GetIntersect(buffer, stands);
        }

    }
}
