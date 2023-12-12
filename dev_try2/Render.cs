using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dev_try2
{
    class Render
    {
        
        public static IFeatureRenderer GetSimpleRenderer(IFeatureClass featureClass, int r, int g, int b)
        {
            ISymbol symbol;
            IRgbColor pRgbColor = new RgbColor();
            pRgbColor = ColorHelper.makeRGBColor(r, g, b);
            esriGeometryType geometryType = featureClass.ShapeType;
            switch (geometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                    {
                        //ISimpleLineSymbol simpleLineSymbol = symbol as ISimpleLineSymbol;
                        //simpleLineSymbol.Color = color;
                        //break;
                        ISimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbol();
                        simpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;  //点转符号设置为圆形
                        simpleMarkerSymbol.Color = pRgbColor;
                        simpleMarkerSymbol.Size =3;
                        symbol = (ISymbol)simpleMarkerSymbol;
                       
                        break;
                    }
                case esriGeometryType.esriGeometryPolyline:
                    {
                        ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
                        simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                        simpleLineSymbol.Color = pRgbColor;
                        symbol = (ISymbol)simpleLineSymbol;
                        break;
                    }
                case esriGeometryType.esriGeometryPolygon:
                    {
                        ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
                        simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSHollow;
                        ILineSymbol lineSymbol = simpleFillSymbol.Outline;
                        lineSymbol.Width = 1.5;
                        lineSymbol.Color = pRgbColor;
                        simpleFillSymbol.Outline= lineSymbol;
                        //simpleFillSymbol.Color = pRgbColor;
                        symbol = (ISymbol)simpleFillSymbol;

                        break;
                    }
                default:
                    {
                        //这个其实没啥用
                        ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
                        simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                        simpleFillSymbol.Color = pRgbColor;
                        symbol = (ISymbol)simpleFillSymbol;
                        break;
                    }
                   
                  
            }
            ISimpleRenderer renderer = new SimpleRendererClass();
            renderer.Symbol = symbol;
            return renderer as IFeatureRenderer;
        }

        public static IRasterRenderer IDWRenderer(IRasterLayer pRasterLayer)
        {
            IRasterClassifyColorRampRenderer pRClassRend = new RasterClassifyColorRampRendererClass();
            IRasterRenderer pRRend = pRClassRend as IRasterRenderer;

            IRaster pRaster = pRasterLayer.Raster;
            ESRI.ArcGIS.DataSourcesRaster.IRasterBandCollection pRBandCol = pRaster as ESRI.ArcGIS.DataSourcesRaster.IRasterBandCollection;
            ESRI.ArcGIS.DataSourcesRaster.IRasterBand pRBand = pRBandCol.Item(0);
            if (pRBand.Histogram == null)
            {
                pRBand.ComputeStatsAndHist();
            }
            pRRend.Raster = pRaster;
            pRClassRend.ClassCount =9;
            pRRend.Update();

            IRgbColor pFromColor = new RgbColorClass();
            pFromColor.Red = 255;
            pFromColor.Green = 247;
            pFromColor.Blue = 251;
            IRgbColor pToColor = new RgbColorClass();
            pToColor.Red = 2;
            pToColor.Green = 56;
            pToColor.Blue = 88;

            IAlgorithmicColorRamp colorRamp = new AlgorithmicColorRampClass();
            colorRamp.Size =9;
            colorRamp.FromColor = pFromColor;
            colorRamp.ToColor = pToColor;
            bool createColorRamp;
            colorRamp.CreateRamp(out createColorRamp);

            IFillSymbol fillSymbol = new SimpleFillSymbolClass();
            
            for (int i = 0; i < pRClassRend.ClassCount; i++)
            {

                fillSymbol.Color = colorRamp.get_Color(i);
                pRClassRend.set_Symbol(i, fillSymbol as ISymbol);
                pRClassRend.set_Label(i, pRClassRend.get_Break(i).ToString("0.00"));
            }
            return pRRend;
        }

        //专门用于降水渲染,除了颜色其实和上面相同，应该改成一个函数的
        public static IRasterRenderer PrecipitationRenderer(IRasterLayer pRasterLayer)
        {
            IRasterClassifyColorRampRenderer pRClassRend = new RasterClassifyColorRampRendererClass();
            IRasterRenderer pRRend = pRClassRend as IRasterRenderer;

            IRaster pRaster = pRasterLayer.Raster;
            ESRI.ArcGIS.DataSourcesRaster.IRasterBandCollection pRBandCol = pRaster as ESRI.ArcGIS.DataSourcesRaster.IRasterBandCollection;
            ESRI.ArcGIS.DataSourcesRaster.IRasterBand pRBand = pRBandCol.Item(0);
            if (pRBand.Histogram == null)
            {
                pRBand.ComputeStatsAndHist();
            }
            pRRend.Raster = pRaster;
            pRClassRend.ClassCount = 6;
            pRRend.Update();

            IRgbColor pFromColor = new RgbColorClass();
            pFromColor.Red = 211;
            pFromColor.Green = 255;
            pFromColor.Blue = 255;
            IRgbColor pToColor = new RgbColorClass();
            pToColor.Red = 0;
            pToColor.Green = 35;
            pToColor.Blue = 255;

            IAlgorithmicColorRamp colorRamp = new AlgorithmicColorRampClass();
            colorRamp.Size = 6;
            colorRamp.FromColor = pFromColor;
            colorRamp.ToColor = pToColor;
            bool createColorRamp;
            colorRamp.CreateRamp(out createColorRamp);

            IFillSymbol fillSymbol = new SimpleFillSymbolClass();

            for (int i = 0; i < pRClassRend.ClassCount; i++)
            {

                fillSymbol.Color = colorRamp.get_Color(i);
                pRClassRend.set_Symbol(i, fillSymbol as ISymbol);
                pRClassRend.set_Label(i, pRClassRend.get_Break(i).ToString("0.00"));
            }
            return pRRend;
        }

        //字段必须是数值才可以
        public static IFeatureRenderer ProportionalRenderer(IFeatureClass featureClass ,string pFieldName)
        {


            //IGeoFeatureLayer geoFeatureLayer;
            //IFeatureLayer featureLayer = new FeatureLayer();
            //featureLayer.FeatureClass = featureClass;
            IProportionalSymbolRenderer proportionalSymbolRenderer;
            ITable table;
            ICursor cursor;
            IDataStatistics dataStatistics;
            IStatisticsResults statisticsResult;
            stdole.IFontDisp fontDisp;


            //geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            int fieldCount = featureClass.FeatureCount(null);
            
            table = featureClass as ITable;
            cursor = table.Search(null, true);
            dataStatistics = new DataStatistics();
            dataStatistics.Cursor = cursor;
            dataStatistics.Field = pFieldName;
            statisticsResult = dataStatistics.Statistics;
            if (statisticsResult != null)
            {
                IFillSymbol fillSymbol = new SimpleFillSymbol();
                fillSymbol.Color = ColorHelper.makeRGBColor(255, 0, 0);
                ICharacterMarkerSymbol characterMarkerSymbol = new CharacterMarkerSymbol();
                fontDisp = new stdole.StdFont() as stdole.IFontDisp;
                fontDisp.Name = "arial";
                fontDisp.Size = 15;
                characterMarkerSymbol.Font = fontDisp;
                characterMarkerSymbol.CharacterIndex = 0X83DC;
                characterMarkerSymbol.Color = ColorHelper.makeRGBColor(196, 10,10);
                characterMarkerSymbol.Size = 15;
                proportionalSymbolRenderer = (IProportionalSymbolRenderer)new ProportionalSymbolRenderer();
                proportionalSymbolRenderer.ValueUnit = esriUnits.esriUnknownUnits;
                proportionalSymbolRenderer.Field = pFieldName;
                proportionalSymbolRenderer.FlanneryCompensation = false;
                proportionalSymbolRenderer.MinDataValue = statisticsResult.Minimum;
                proportionalSymbolRenderer.MaxDataValue = statisticsResult.Maximum;
                proportionalSymbolRenderer.BackgroundSymbol = fillSymbol;
                proportionalSymbolRenderer.MinSymbol = characterMarkerSymbol as ISymbol;
                proportionalSymbolRenderer.LegendSymbolCount = (int)(statisticsResult.Maximum-statisticsResult.Minimum);//
                proportionalSymbolRenderer.CreateLegendSymbols();
                //geoFeatureLayer.Renderer = proportionalSymbolRenderer as IFeatureRenderer;
                return proportionalSymbolRenderer as IFeatureRenderer;
            }
            return null;

        }

        public static IFeatureRenderer GetDivideValueRenderer(IFeatureClass featureClass, string pFieldName)
        {
            //from fhx's precititation program
            if (featureClass != null)
            {    
                ITable table = featureClass as ITable;
                //首先获得符号的类型
                //ISymbol symbol = MapComposer.GetSymbolFromLayer(layer);
                int classCount = 5;  //等级数
                //实例化
                //BasicTableHistogramClass 采用表对象输入数据的结构（如自然断点、分位数）生成直方图
                ITableHistogram tableHistogram = new BasicTableHistogram() as ITableHistogram;
                tableHistogram.Table = table;
                tableHistogram.Field = pFieldName;

                IBasicHistogram basicHistogram = tableHistogram as IBasicHistogram;
                //先统计每个值出现的次数、输出结果为 vakues,frequencys
                object values;
                object frequencys;
                basicHistogram.GetHistogram(out values, out frequencys);
                //IClassifyGEN实现了对所有数据对象的分类(DefineInterval、EqualInterval、NatureBreaks、Quantile、StandardDeviation)
                //此处创建一平均分级对象，用统计结果进行分级
                IClassifyGEN classifyGEN = new Quantile();
                classifyGEN.Classify(values, frequencys, ref classCount);
                double[] classes = classifyGEN.ClassBreaks as double[];

                double[] myclasses;
                myclasses = new double[classCount];
                if (classes != null)
                {
                    for (int j = 0; j < classCount; j++)
                        myclasses[j] = classes[j + 1];
                }

                //定义一颜色枚举变量，通过函数获取色带
                IEnumColors enumColors = ColorHelper.makeAlgorithmicColorRamp(ColorHelper.makeRGBColor(255, 235, 214), ColorHelper.makeRGBColor(255, 10, 10), classCount).Colors;
                IColor color;
                IClassBreaksRenderer classBreaksRenderer = new ClassBreaksRenderer();
                classBreaksRenderer.BreakCount = classCount; //设置分级数
                classBreaksRenderer.Field = pFieldName;
                classBreaksRenderer.SortClassesAscending = true; //升序显示
               

                //给所有等级附上渲染颜色
                ISimpleMarkerSymbol simpleMarkerSymbol;
                for (int i = 0; i < myclasses.Length; i++)
                {
                    color = enumColors.Next();
                    simpleMarkerSymbol = new SimpleMarkerSymbol();
                    simpleMarkerSymbol.Color = color;
                    simpleMarkerSymbol.Size = 8;
                    classBreaksRenderer.Symbol[i] = simpleMarkerSymbol as ISymbol;
                    classBreaksRenderer.Break[i] = myclasses[i];
                }

                return classBreaksRenderer as IFeatureRenderer;
            }
            else
            {
                MessageBox.Show("error");
                return null;
            }
        }
    }
}
