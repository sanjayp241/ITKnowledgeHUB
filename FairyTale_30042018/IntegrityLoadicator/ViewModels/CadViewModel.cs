//-----------------------------------------------------------------------------------
//.dwg/.dxf file Reader through CADLIB
//
//-----------------------------------------------------------------------------------

namespace ZebecLoadMaster.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.Windows.Media.Media3D;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using System.Data;
    using System.Reflection;
    
    #region CADLIB
    using WW.Cad.Base;
    using WW.Cad.Drawing;
    using WW.Cad.Drawing.GDI;
    using WW.Cad.IO;
    using WW.Cad.Model;
    using WW.Math;
    using WW.Cad.Drawing.Wpf;
    using WW.Cad.Model.Objects;
    using WW.Math.Geometry;
    #endregion 
    class CadViewModel 
    {
       
        public static void Cad2dModels()
        {
            DxfModel model;
            Assembly assembly = Assembly.GetExecutingAssembly();

           
            var input = assembly.GetManifestResourceStream("ZebecLoadMaster.Images.TankPlan_FairyTale.dwg");
            model = DwgReader.Read(input);
            Models.clsGlobVar.CentrelineProfile = model;

            model = null;
            input = null;
            assembly = null;

        }
    }
}
