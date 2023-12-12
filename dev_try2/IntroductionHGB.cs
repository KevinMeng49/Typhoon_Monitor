using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;

namespace dev_try2
{
    public partial class IntroductionHGB : SplashScreen
    {
        public IntroductionHGB()
        {
            InitializeComponent();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum SplashScreenCommand
        {
        }

        private void IntroductionHGB_MouseDown(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void IntroductionHGB_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}