using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WW.Actions;
using WW.Cad.Drawing;
using WW.Cad.Drawing.GDI;
using WW.Cad.Model;
using WW.Cad.Model.Entities;
using WW.Drawing;
using WW.Math;
using WW.Math.Geometry;
using System.Windows.Forms;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ZebecLoadMaster.Models
{
    class clsGetHatchDeckPlan
    {
        public DxfHatch getHatch_Deck(decimal percentfill, double[] x, double[] y, EntityColor hatchcolor)
        {
            DxfHatch hatch = new DxfHatch();
            hatch.Color = hatchcolor;

            if (percentfill != 0)
            {


                #region hatch
                DxfHatch.BoundaryPath.Polyline.Vertex[] Ver = new DxfHatch.BoundaryPath.Polyline.Vertex[13];

                if ((Convert.ToInt32(percentfill) > 0) & (Convert.ToInt32(percentfill) <= 100))
                {

                    for (int i = 1; i <= 12; i++)
                    {
                        Ver[i] = new DxfHatch.BoundaryPath.Polyline.Vertex(x[i], y[i]);
                    }

                }

                DxfHatch.BoundaryPath bp = new DxfHatch.BoundaryPath();
                bp.Type = BoundaryPathType.Polyline;
                hatch.BoundaryPaths.Add(bp);


                bp.PolylineData =
                           new DxfHatch.BoundaryPath.Polyline(
                                new DxfHatch.BoundaryPath.Polyline.Vertex[] {
                             Ver[1],Ver[2],Ver[3],Ver[4],Ver[5],Ver[6],Ver[7],Ver[8],Ver[9],Ver[10],Ver[11],Ver[12]

                           }

                           );


                bp.PolylineData.Closed = true;

                #endregion hatch
            }

            return hatch;
        }
    }
}
