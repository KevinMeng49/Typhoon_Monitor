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

namespace dev_try2
{
    public partial class QueryByAttribute : DevExpress.XtraEditors.XtraForm
    {
        private IMap currentMap;    //当前MapControl控件中的Map对象
        private IFeatureLayer currentFeatureLayer;  //设置临时类变量来使用IFeatureLayer接口的当前图层对象
        private string currentFieldName;    //设置临时类变量来存储字段名称
        private IFeatureSelection featureSelection;


        // <summary>
        /// 获得当前MapControl控件中的Map对象。
        /// </summary>
        public IMap CurrentMap
        {
            set
            {
                currentMap = value;
            }
        }


        public QueryByAttribute(IFeatureSelection selection)
        {
            InitializeComponent();
            featureSelection = selection;

        }

        //将所有的图层名称加载进入comboBoxLayerName.Items(No problem)
        private void QueryByAttribute_Load(object sender, EventArgs e)
        {
            try
            {
                //将当前图层列表清空
                comboBoxLayerName.Items.Clear();

                string layerName;   //设置临时变量存储图层名称

                //对Map中的每个图层进行判断并加载名称
                for (int i = 0; i < currentMap.LayerCount; i++)
                {
                    //如果该图层为图层组类型，则分别对所包含的每个图层进行操作
                    if (currentMap.get_Layer(i) is GroupLayer)
                    {
                        //使用ICompositeLayer接口进行遍历操作
                        ICompositeLayer compositeLayer = currentMap.get_Layer(i) as ICompositeLayer;
                        for (int j = 0; j < compositeLayer.Count; j++)
                        {
                            //将图层的名称添加到comboBoxLayerName控件中
                            layerName = compositeLayer.get_Layer(j).Name;
                            comboBoxLayerName.Items.Add(layerName);
                        }
                    }
                    //如果图层不是图层组类型，则直接添加名称
                    else
                    {
                        layerName = currentMap.get_Layer(i).Name;
                        comboBoxLayerName.Items.Add(layerName);
                    }
                }

                //将comboBoxLayerName控件的默认选项设置为第一个图层的名称
                comboBoxLayerName.SelectedIndex = 0;
                //将comboBoxSelectMethod控件的默认选项设置为第一种选择方式
                comboBoxSelectMethod.SelectedIndex = 0;
            }
            catch { }
        }
        //将选中图层的所以满足条件的字段加载进入listBoxFields控件中(No problem)
        private void comboBoxLayerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //首先将字段列表和字段值列表清空
            listBoxFields.Items.Clear();
            listBoxValues.Items.Clear();

            IField field;   //设置临时变量来存储图层的字段

            for (int i = 0; i < currentMap.LayerCount; i++)
            {
                if (currentMap.get_Layer(i) is GroupLayer)
                {
                    ICompositeLayer compositeLayer = currentMap.get_Layer(i) as ICompositeLayer;
                    for (int j = 0; j < compositeLayer.Count; j++)
                    {
                        //判断图层的名称是否与comboBoxLayerName控件中选择的图层名称相同
                        if (compositeLayer.get_Layer(j).Name == comboBoxLayerName.SelectedItem.ToString())
                        {
                            //如果相同则设置为整个窗体所使用的IFeatureLayer接口对象
                            currentFeatureLayer = compositeLayer.get_Layer(j) as IFeatureLayer;
                            break;
                        }
                    }
                }
                else
                {
                    //判断图层的名称是否与comboBoxLayerName中选择的图层名称相同
                    if (currentMap.get_Layer(i).Name == comboBoxLayerName.SelectedItem.ToString())
                    {
                        //如果相同则设置为整个窗体所使用的IFeatureLayer接口对象
                        currentFeatureLayer = currentMap.get_Layer(i) as IFeatureLayer;
                        break;
                    }
                }
            }
            //这里已经确定了是哪一个图层
            //使用IFeatureClass接口对该图层的所有属性字段进行遍历，并填充listBoxFields控件
            for (int i = 0; i < currentFeatureLayer.FeatureClass.Fields.FieldCount; i++)
            {
                //根据索引值获取图层的字段
                field = currentFeatureLayer.FeatureClass.Fields.get_Field(i);
                //排除SHAPE字段，并在其它字段名称前后添加字符"和字符"，注意这里用到了转义府
                if (field.Name.ToUpper() != "SHAPE")
                    listBoxFields.Items.Add("\"" + field.Name + "\"");
            }
            //将显示where语句的文本框内容清空
            textBoxWhere.Clear();

        }
        //当选中或改变listBoxFields控件中的字段(主要是进行字符串的操作)(No problem)
        private void listBoxFields_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //将选中字段的值加入listBoxValues控件中(No problem)
        private void buttonGetUniqueValue_Click(object sender, EventArgs e)
        {
            try
            {
                //使用FeatureClass对象的IDataset接口来获取dataset和workspace的信息
                IDataset dataset = (IDataset)currentFeatureLayer.FeatureClass;
                //使用IQueryDef接口的对象来定义和查询属性信息。通过IWorkspace接口的CreateQueryDef()方法创建该对象。
                IQueryDef queryDef = ((IFeatureWorkspace)dataset.Workspace).CreateQueryDef();
                //设置所需查询的表格名称为dataset的名称
                queryDef.Tables = dataset.Name;
                //设置查询的字段名称。可以联合使用SQL语言的关键字，如查询唯一值可以使用DISTINCT关键字。
                queryDef.SubFields = "DISTINCT (" + currentFieldName + ")";
                //执行查询并返回ICursor接口的对象来访问整个结果的集合
                ICursor cursor = queryDef.Evaluate();
                //使用IField接口获取当前所需要使用的字段的信息
                IFields fields = currentFeatureLayer.FeatureClass.Fields;
                IField field = fields.get_Field(fields.FindField(currentFieldName));

                //对整个结果集合进行遍历，从而添加所有的唯一值
                //使用IRow接口来操作结果集合。首先定位到第一个查询结果。
                IRow row = cursor.NextRow();
                //如果查询结果非空，则一直进行添加操作
                while (row != null)
                {
                    //对String类型的字段，唯一值的前后添加'和'，以符合SQL语句的要求
                    if (field.Type == esriFieldType.esriFieldTypeString)
                    {
                        listBoxValues.Items.Add("\'" + row.get_Value(0).ToString() + "\'");
                    }
                    else
                    {
                        listBoxValues.Items.Add(row.get_Value(0).ToString());
                    }
                    //继续执行下一个结果的添加
                    row = cursor.NextRow();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        //关闭窗体
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //清空查询语句
        private void buttonClear_Click(object sender, EventArgs e)
        {
            // 清空textBoxWhere控件中的文本
            textBoxWhere.Clear();
        }
        //将字段名加入查询语句中
        private void listBoxFields_DoubleClick(object sender, EventArgs e)
        {
            //在where语句中加入字段名称信息
            textBoxWhere.Text += listBoxFields.SelectedItem.ToString();
        }
        //将字段值加入查询语句中
        private void listBoxValues_DoubleClick(object sender, EventArgs e)
        {
            //在where语句中加入字段值信息
            textBoxWhere.Text += listBoxValues.SelectedItem.ToString();
        }
        //将等号加入查询语句中
        private void buttonEqual_Click(object sender, EventArgs e)
        {
            //在where语句中加入运算符信息
            textBoxWhere.Text += " " + buttonEqual.Text + " ";
        }
        //将不等号加入查询语句中
        private void buttonNotEqual_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonNotEqual.Text + " ";
        }
        //将Like加入查询语句中
        private void buttonLike_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonLike.Text + " ";
        }
        //将大于号加入查询语句中
        private void buttonGreater_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonGreater.Text + " ";
        }
        //将大于等于号加入查询语句中
        private void buttonGreaterEqual_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonGreaterEqual.Text + " ";
        }
        //将And加入查询语句中
        private void buttonAnd_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonAnd.Text + " ";
        }
        //将小于号加入查询语句中
        private void buttonLess_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonLess.Text + " ";
        }
        //将小于等于号加入查询语句中
        private void buttonLessEqual_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonLessEqual.Text + " ";
        }
        //将Or加入查询语句中
        private void buttonOr_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonOr.Text + " ";
        }
        //将下划线加入查询语句中
        private void buttonUnderLine_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonUnderLine.Text + " ";
        }
        //将百分号加入查询语句中
        private void buttonPercent_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonPercent.Text + " ";
        }
        //将括号加入查询语句中
        private void buttonBrackets_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonBrackets.Text + " ";
        }
        //将Not加入查询语句中
        private void buttonNot_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonNot.Text + " ";
        }
        //将Is加入查询语句中
        private void buttonIs_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text += " " + buttonIs.Text + " ";
        }
        //确定按钮
        private void buttonOK_Click(object sender, EventArgs e)
        {
            SelectFeaturesByAttribute();
            this.Close();
        }
        //应用按钮
        private void buttonApply_Click(object sender, EventArgs e)
        {
            try
            {
                //执行属性查询操作
                SelectFeaturesByAttribute();
            }
            catch
            { }
        }
        //按属性查询函数
        private void SelectFeaturesByAttribute()
        {
            //使用FeatureLayer对象的IFeatureSelection接口来执行查询操作。这里有一个接口转换操作。

            featureSelection = currentFeatureLayer as IFeatureSelection;

            //新建IQueryFilter接口的对象来进行where语句的定义
            IQueryFilter queryFilter = new QueryFilter();
            //设置where语句内容
            queryFilter.WhereClause = textBoxWhere.Text;
            //通过接口转换使用Map对象的IActiveView接口来部分刷新地图窗口，从而高亮显示查询的结果
            IActiveView activeView = currentMap as IActiveView;

            //根据查询选择方式的不同，得到不同的选择集
            switch (comboBoxSelectMethod.SelectedIndex)
            {
                //在新建选择集的情况下
                case 0:
                    //首先使用IMap接口的ClearSelection()方法清空地图选择集
                    currentMap.ClearSelection();
                    //根据定义的where语句使用IFeatureSelection接口的SelectFeatures方法选择要素，并将其添加到选择集中
                    featureSelection.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    break;
                //添加到当前选择集的情况
                case 1:
                    featureSelection.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultAdd, false);
                    break;
                //从当前选择集中删除的情况
                case 2:
                    featureSelection.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultXOR, false);
                    break;
                //从当前选择集中选择的情况
                case 3:
                    featureSelection.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultAnd, false);
                    break;
                //默认为新建选择集的情况
                default:
                    currentMap.ClearSelection();
                    featureSelection.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    break;
            }

            //部分刷新操作，只刷新地理选择集的内容
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, activeView.Extent);
            //上面要换成高亮显示
        }

        ~QueryByAttribute()
        {
            MessageBox.Show("What??");
        }

        //确定按钮，点完后消失
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SelectFeaturesByAttribute();
            AttributeTable table = new AttributeTable(featureSelection);
            table.currentmap = currentMap;
            table.Show();
            //HighlightSelect()
            this.Close();
        }


        //应用按钮，点完后不关闭，只是高亮显示
        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }
    }
}