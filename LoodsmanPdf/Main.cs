using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Loodsman;
using DataProvider;

namespace LoodsmanPdf
{
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [Guid("9F970E60-F4A3-4015-8C1F-6AE0F679858A")]
    [ComVisible(true)]
    public interface ILoodsmanPdf
    {
        void Command1(IPluginCall APlugin);
    }

    [ClassInterface(ClassInterfaceType.None)]
    [Guid("DB6DC3EB-4F28-4596-A11D-1977A8E598C3")]
    [ProgId("Loodsman.LoodsmanPdf")]
    [ComVisible(true)]
    public class Main : ILoodsmanPdf
    {
        public void Command1(IPluginCall APlugin)
        {
            MessageBox.Show("Hi!");
        }
    }
}