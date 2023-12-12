using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_try2
{
    public class Query
    {
        // IFeatureCursor QueryByAttribute(IFeatureClass,string Field,string element) { };
        public static IFeature FindFeatureByAttribute(IFeatureClass featureclass, string fieldname, string value)
        {
            IFeatureCursor featureCursor = featureclass.Search(null, false);
            IFeature feature;
            if (featureCursor != null)
            {
                //要素类每一个要素的字段都是相同的，所以只需要在FeatureClass里的字段里找相应字段就可以了
                int fieldIndex = featureclass.Fields.FindField(fieldname);
                while ((feature = featureCursor.NextFeature()) != null)
                {
                    if (feature.Value[fieldIndex].ToString() == value)
                    {
                        return feature;
                    }
                }
            }
            return null;
        }

        public static IFeatureCursor FindFeatureClassByAttribute(IFeatureClass featureclass, string fieldname, string value)
        {
            IQueryFilter queryFilter = new QueryFilter();
            queryFilter.WhereClause =fieldname +"=" + value;
            return featureclass.Search(queryFilter, false);
        }

        //获取某一个要素集中的某一个属性最大的要素,其中可以选择添加控制条件,（仅针对字符串）
        public static IFeature GetMaxFeatureByFeild(IFeatureClass featureClass, string feildName, IQueryFilter filter = null)
        {
            IFeatureCursor featureCursor = featureClass.Search(filter, false);
            IFeature feature = featureCursor.NextFeature();
            IFeature result = null;
            int index = featureClass.FindField(feildName);
            string max = null;
            if (feature != null)
            {
                result = feature;
                max = feature.get_Value(index).ToString();
            }

            while (feature != null)
            {
                string a = feature.get_Value(index).ToString();
                if (string.Compare(max, a) < 0)
                {
                    result = feature;
                    max = feature.get_Value(index).ToString();
                }
                feature = featureCursor.NextFeature();
            }
            return result;
        }
        public static IFeature GetMaxFeatureByFeild(IFeatureCursor featureCursor, string feildName, IQueryFilter filter = null)
        {
            IFeature feature = featureCursor.NextFeature();
            IFeature result = null;
            int index = featureCursor.FindField(feildName);
            string max = null;
            if (feature != null)
            {
                result = feature;
                max = feature.get_Value(index).ToString();
            }

            while (feature != null)
            {
                string a = feature.get_Value(index).ToString();
                if (string.Compare(max, a) < 0)
                {
                    result = feature;
                    max = feature.get_Value(index).ToString();
                }
                feature = featureCursor.NextFeature();
            }
            return result;
        }

        public static IFeature GetMinFeatureByFeild(IFeatureClass featureClass, string feildName, IQueryFilter filter = null)
        {
            IFeatureCursor featureCursor = featureClass.Search(filter, false);
            IFeature feature = featureCursor.NextFeature();
            IFeature result = null;
            int index = featureClass.FindField(feildName);
            string min = null;
            if (feature != null)
            {
                result = feature;
                min = feature.get_Value(index).ToString();
            }

            while (feature != null)
            {
                string a = feature.get_Value(index).ToString();
                if (string.Compare(min, a) > 0)
                {
                    result = feature;
                    min = feature.get_Value(index).ToString();
                }
                feature = featureCursor.NextFeature();
            }
            return result;
        }


        public static IFeature GetMinFeatureByFeild(IFeatureCursor featureCursor, string feildName, IQueryFilter filter = null)
        {
            IFeature feature = featureCursor.NextFeature();
            IFeature result = null;
            int index = featureCursor.FindField(feildName);
            string min = null;
            if (feature != null)
            {
                result = feature;
                min = feature.get_Value(index).ToString();
            }

            while (feature != null)
            {
                string a = feature.get_Value(index).ToString();
                if (string.Compare(min, a) > 0)
                {
                    result = feature;
                    min = feature.get_Value(index).ToString();
                }
                feature = featureCursor.NextFeature();
            }
            return result;
        }
    }
}
