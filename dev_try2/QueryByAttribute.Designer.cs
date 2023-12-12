namespace dev_try2
{
    partial class QueryByAttribute
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.listBoxFields = new System.Windows.Forms.ListBox();
            this.listBoxValues = new System.Windows.Forms.ListBox();
            this.buttonEqual = new System.Windows.Forms.Button();
            this.buttonNotEqual = new System.Windows.Forms.Button();
            this.buttonLike = new System.Windows.Forms.Button();
            this.buttonGreater = new System.Windows.Forms.Button();
            this.buttonGreaterEqual = new System.Windows.Forms.Button();
            this.buttonAnd = new System.Windows.Forms.Button();
            this.buttonLess = new System.Windows.Forms.Button();
            this.buttonLessEqual = new System.Windows.Forms.Button();
            this.buttonOr = new System.Windows.Forms.Button();
            this.buttonUnderLine = new System.Windows.Forms.Button();
            this.buttonPercent = new System.Windows.Forms.Button();
            this.buttonBrackets = new System.Windows.Forms.Button();
            this.buttonNot = new System.Windows.Forms.Button();
            this.buttonIs = new System.Windows.Forms.Button();
            this.textBoxWhere = new System.Windows.Forms.TextBox();
            this.buttonApply = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.comboBoxLayerName = new System.Windows.Forms.ComboBox();
            this.comboBoxSelectMethod = new System.Windows.Forms.ComboBox();
            this.buttonGetUniqueValue = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 29);
            this.label1.TabIndex = 5;
            this.label1.Text = "选择查询属性";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(509, 599);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(89, 42);
            this.simpleButton1.TabIndex = 7;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // listBoxFields
            // 
            this.listBoxFields.FormattingEnabled = true;
            this.listBoxFields.ItemHeight = 29;
            this.listBoxFields.Location = new System.Drawing.Point(58, 159);
            this.listBoxFields.Name = "listBoxFields";
            this.listBoxFields.Size = new System.Drawing.Size(203, 265);
            this.listBoxFields.TabIndex = 9;
            this.listBoxFields.Click += new System.EventHandler(this.listBoxFields_DoubleClick);
            // 
            // listBoxValues
            // 
            this.listBoxValues.FormattingEnabled = true;
            this.listBoxValues.ItemHeight = 29;
            this.listBoxValues.Location = new System.Drawing.Point(667, 166);
            this.listBoxValues.Name = "listBoxValues";
            this.listBoxValues.Size = new System.Drawing.Size(199, 265);
            this.listBoxValues.TabIndex = 10;
            this.listBoxValues.Click += new System.EventHandler(this.listBoxValues_DoubleClick);
            // 
            // buttonEqual
            // 
            this.buttonEqual.Location = new System.Drawing.Point(304, 126);
            this.buttonEqual.Name = "buttonEqual";
            this.buttonEqual.Size = new System.Drawing.Size(91, 48);
            this.buttonEqual.TabIndex = 11;
            this.buttonEqual.Text = "=";
            this.buttonEqual.UseVisualStyleBackColor = true;
            this.buttonEqual.Click += new System.EventHandler(this.buttonEqual_Click);
            // 
            // buttonNotEqual
            // 
            this.buttonNotEqual.Location = new System.Drawing.Point(416, 126);
            this.buttonNotEqual.Name = "buttonNotEqual";
            this.buttonNotEqual.Size = new System.Drawing.Size(100, 48);
            this.buttonNotEqual.TabIndex = 12;
            this.buttonNotEqual.Text = "<>";
            this.buttonNotEqual.UseVisualStyleBackColor = true;
            this.buttonNotEqual.Click += new System.EventHandler(this.buttonNotEqual_Click);
            // 
            // buttonLike
            // 
            this.buttonLike.Location = new System.Drawing.Point(542, 126);
            this.buttonLike.Name = "buttonLike";
            this.buttonLike.Size = new System.Drawing.Size(94, 48);
            this.buttonLike.TabIndex = 13;
            this.buttonLike.Text = "Like";
            this.buttonLike.UseVisualStyleBackColor = true;
            this.buttonLike.Click += new System.EventHandler(this.buttonLike_Click);
            // 
            // buttonGreater
            // 
            this.buttonGreater.Location = new System.Drawing.Point(304, 194);
            this.buttonGreater.Name = "buttonGreater";
            this.buttonGreater.Size = new System.Drawing.Size(91, 47);
            this.buttonGreater.TabIndex = 14;
            this.buttonGreater.Text = ">";
            this.buttonGreater.UseVisualStyleBackColor = true;
            this.buttonGreater.Click += new System.EventHandler(this.buttonGreater_Click);
            // 
            // buttonGreaterEqual
            // 
            this.buttonGreaterEqual.Location = new System.Drawing.Point(416, 194);
            this.buttonGreaterEqual.Name = "buttonGreaterEqual";
            this.buttonGreaterEqual.Size = new System.Drawing.Size(100, 47);
            this.buttonGreaterEqual.TabIndex = 15;
            this.buttonGreaterEqual.Text = ">=";
            this.buttonGreaterEqual.UseVisualStyleBackColor = true;
            this.buttonGreaterEqual.Click += new System.EventHandler(this.buttonGreaterEqual_Click);
            // 
            // buttonAnd
            // 
            this.buttonAnd.Location = new System.Drawing.Point(542, 194);
            this.buttonAnd.Name = "buttonAnd";
            this.buttonAnd.Size = new System.Drawing.Size(94, 47);
            this.buttonAnd.TabIndex = 16;
            this.buttonAnd.Text = "And";
            this.buttonAnd.UseVisualStyleBackColor = true;
            this.buttonAnd.Click += new System.EventHandler(this.buttonAnd_Click);
            // 
            // buttonLess
            // 
            this.buttonLess.Location = new System.Drawing.Point(304, 265);
            this.buttonLess.Name = "buttonLess";
            this.buttonLess.Size = new System.Drawing.Size(91, 47);
            this.buttonLess.TabIndex = 17;
            this.buttonLess.Text = "<";
            this.buttonLess.UseVisualStyleBackColor = true;
            this.buttonLess.Click += new System.EventHandler(this.buttonLess_Click);
            // 
            // buttonLessEqual
            // 
            this.buttonLessEqual.Location = new System.Drawing.Point(416, 265);
            this.buttonLessEqual.Name = "buttonLessEqual";
            this.buttonLessEqual.Size = new System.Drawing.Size(100, 47);
            this.buttonLessEqual.TabIndex = 18;
            this.buttonLessEqual.Text = "<=";
            this.buttonLessEqual.UseVisualStyleBackColor = true;
            this.buttonLessEqual.Click += new System.EventHandler(this.buttonLessEqual_Click);
            // 
            // buttonOr
            // 
            this.buttonOr.Location = new System.Drawing.Point(542, 265);
            this.buttonOr.Name = "buttonOr";
            this.buttonOr.Size = new System.Drawing.Size(94, 47);
            this.buttonOr.TabIndex = 19;
            this.buttonOr.Text = "Or";
            this.buttonOr.UseVisualStyleBackColor = true;
            this.buttonOr.Click += new System.EventHandler(this.buttonOr_Click);
            // 
            // buttonUnderLine
            // 
            this.buttonUnderLine.Location = new System.Drawing.Point(304, 330);
            this.buttonUnderLine.Name = "buttonUnderLine";
            this.buttonUnderLine.Size = new System.Drawing.Size(37, 49);
            this.buttonUnderLine.TabIndex = 20;
            this.buttonUnderLine.Text = "_";
            this.buttonUnderLine.UseVisualStyleBackColor = true;
            this.buttonUnderLine.Click += new System.EventHandler(this.buttonUnderLine_Click);
            // 
            // buttonPercent
            // 
            this.buttonPercent.Location = new System.Drawing.Point(347, 330);
            this.buttonPercent.Name = "buttonPercent";
            this.buttonPercent.Size = new System.Drawing.Size(48, 49);
            this.buttonPercent.TabIndex = 21;
            this.buttonPercent.Text = "%";
            this.buttonPercent.UseVisualStyleBackColor = true;
            this.buttonPercent.Click += new System.EventHandler(this.buttonPercent_Click);
            // 
            // buttonBrackets
            // 
            this.buttonBrackets.Location = new System.Drawing.Point(416, 330);
            this.buttonBrackets.Name = "buttonBrackets";
            this.buttonBrackets.Size = new System.Drawing.Size(100, 49);
            this.buttonBrackets.TabIndex = 22;
            this.buttonBrackets.Text = "()";
            this.buttonBrackets.UseVisualStyleBackColor = true;
            this.buttonBrackets.Click += new System.EventHandler(this.buttonBrackets_Click);
            // 
            // buttonNot
            // 
            this.buttonNot.Location = new System.Drawing.Point(542, 330);
            this.buttonNot.Name = "buttonNot";
            this.buttonNot.Size = new System.Drawing.Size(94, 49);
            this.buttonNot.TabIndex = 23;
            this.buttonNot.Text = "Not";
            this.buttonNot.UseVisualStyleBackColor = true;
            this.buttonNot.Click += new System.EventHandler(this.buttonNot_Click);
            // 
            // buttonIs
            // 
            this.buttonIs.Location = new System.Drawing.Point(304, 388);
            this.buttonIs.Name = "buttonIs";
            this.buttonIs.Size = new System.Drawing.Size(91, 45);
            this.buttonIs.TabIndex = 24;
            this.buttonIs.Text = "Is";
            this.buttonIs.UseVisualStyleBackColor = true;
            this.buttonIs.Click += new System.EventHandler(this.buttonIs_Click);
            // 
            // textBoxWhere
            // 
            this.textBoxWhere.Location = new System.Drawing.Point(58, 492);
            this.textBoxWhere.Multiline = true;
            this.textBoxWhere.Name = "textBoxWhere";
            this.textBoxWhere.Size = new System.Drawing.Size(793, 101);
            this.textBoxWhere.TabIndex = 25;
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(631, 599);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(96, 42);
            this.buttonApply.TabIndex = 26;
            this.buttonApply.Text = "应用";
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(755, 599);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(96, 42);
            this.simpleButton3.TabIndex = 27;
            this.simpleButton3.Text = "取消";
            this.simpleButton3.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Location = new System.Drawing.Point(58, 599);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(96, 42);
            this.simpleButton4.TabIndex = 28;
            this.simpleButton4.Text = "清除";
            this.simpleButton4.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // comboBoxLayerName
            // 
            this.comboBoxLayerName.FormattingEnabled = true;
            this.comboBoxLayerName.Location = new System.Drawing.Point(152, 12);
            this.comboBoxLayerName.Name = "comboBoxLayerName";
            this.comboBoxLayerName.Size = new System.Drawing.Size(714, 37);
            this.comboBoxLayerName.TabIndex = 29;
            this.comboBoxLayerName.Click += new System.EventHandler(this.comboBoxLayerName_SelectedIndexChanged);
            // 
            // comboBoxSelectMethod
            // 
            this.comboBoxSelectMethod.FormattingEnabled = true;
            this.comboBoxSelectMethod.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.comboBoxSelectMethod.Location = new System.Drawing.Point(152, 59);
            this.comboBoxSelectMethod.Name = "comboBoxSelectMethod";
            this.comboBoxSelectMethod.Size = new System.Drawing.Size(714, 37);
            this.comboBoxSelectMethod.TabIndex = 30;
            // 
            // buttonGetUniqueValue
            // 
            this.buttonGetUniqueValue.Location = new System.Drawing.Point(667, 437);
            this.buttonGetUniqueValue.Name = "buttonGetUniqueValue";
            this.buttonGetUniqueValue.Size = new System.Drawing.Size(199, 36);
            this.buttonGetUniqueValue.TabIndex = 31;
            this.buttonGetUniqueValue.Text = "获取唯一值";
            this.buttonGetUniqueValue.UseVisualStyleBackColor = true;
            this.buttonGetUniqueValue.Click += new System.EventHandler(this.buttonGetUniqueValue_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 29);
            this.label2.TabIndex = 32;
            this.label2.Text = "图层名称";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 29);
            this.label3.TabIndex = 33;
            this.label3.Text = "选择方式";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(69, 460);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 29);
            this.label4.TabIndex = 34;
            this.label4.Text = "Select";
            // 
            // QueryByAttribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 677);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonGetUniqueValue);
            this.Controls.Add(this.comboBoxSelectMethod);
            this.Controls.Add(this.comboBoxLayerName);
            this.Controls.Add(this.simpleButton4);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.textBoxWhere);
            this.Controls.Add(this.buttonIs);
            this.Controls.Add(this.buttonNot);
            this.Controls.Add(this.buttonBrackets);
            this.Controls.Add(this.buttonPercent);
            this.Controls.Add(this.buttonUnderLine);
            this.Controls.Add(this.buttonOr);
            this.Controls.Add(this.buttonLessEqual);
            this.Controls.Add(this.buttonLess);
            this.Controls.Add(this.buttonAnd);
            this.Controls.Add(this.buttonGreaterEqual);
            this.Controls.Add(this.buttonGreater);
            this.Controls.Add(this.buttonLike);
            this.Controls.Add(this.buttonNotEqual);
            this.Controls.Add(this.buttonEqual);
            this.Controls.Add(this.listBoxValues);
            this.Controls.Add(this.listBoxFields);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.label1);
            this.Name = "QueryByAttribute";
            this.Text = "QueryByAttribute";
            this.Load += new System.EventHandler(this.QueryByAttribute_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.ListBox listBoxFields;
        private System.Windows.Forms.ListBox listBoxValues;
        private System.Windows.Forms.Button buttonEqual;
        private System.Windows.Forms.Button buttonNotEqual;
        private System.Windows.Forms.Button buttonLike;
        private System.Windows.Forms.Button buttonGreater;
        private System.Windows.Forms.Button buttonGreaterEqual;
        private System.Windows.Forms.Button buttonAnd;
        private System.Windows.Forms.Button buttonLess;
        private System.Windows.Forms.Button buttonLessEqual;
        private System.Windows.Forms.Button buttonOr;
        private System.Windows.Forms.Button buttonUnderLine;
        private System.Windows.Forms.Button buttonPercent;
        private System.Windows.Forms.Button buttonBrackets;
        private System.Windows.Forms.Button buttonNot;
        private System.Windows.Forms.Button buttonIs;
        private System.Windows.Forms.TextBox textBoxWhere;
        private DevExpress.XtraEditors.SimpleButton buttonApply;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private System.Windows.Forms.ComboBox comboBoxLayerName;
        private System.Windows.Forms.ComboBox comboBoxSelectMethod;
        private System.Windows.Forms.Button buttonGetUniqueValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}