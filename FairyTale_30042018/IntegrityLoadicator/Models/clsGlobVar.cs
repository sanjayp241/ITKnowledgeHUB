using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

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

namespace ZebecLoadMaster.Models
{   
   public class clsGlobVar
    {
       public static string DamageCase="";
       public static string stdLoad = "";
       public static bool FlagDamageCases = false;
       public static bool flagLoadingCondition = false;

        public static DataTable dtRealBallastTanks { get; set; }
        public static DataTable dtRealFuelOilTanks { get; set; }
        public static DataTable dtRealFreshWaterTanks { get; set; }
        public static DataTable dtRealMiscTanks { get; set; }
        public static DataTable dtRealCompartments { get; set; }
        public static DataTable dtRealVariableItems { get; set; }
        public static DataTable dtRealEquillibriumValues { get; set; }
        public static DataTable dtRealLoadingSummary { get; set; }
        public static DataTable dtRealDrafts { get; set; }
        public static DataTable FloodingPoint_Damage { get; set; }
        public static DataTable dtRealGZ { get; set; }
        public static DataTable dtRealHydrostatics { get; set; }
        public static DataTable dtRealStabilitySummary { get; set; }
        public static DataTable dtRealStabilityCriteriaIntact { get; set; }
        public static DataTable dtRealStabilityCriteriaDamage { get; set; }
        public static DataTable dtRealLongitudinal { get; set; }

        public static DataTable dtDamageCriteria { get; set; }

        public static DataTable dtSimulationAllTanks { get; set; }
        public static DataTable dtSimulationAllTanksForDamage { get; set; }
        public static DataTable dtSimulationBallastTanks { get; set; }
        public static DataTable dtSimulationFuelOilTanks { get; set; }
        public static DataTable dtSimulationFreshWaterTanks { get; set; }
        public static DataTable dtSimulationMiscTanks { get; set; }
        public static DataTable dtSimulationCompartments { get; set; }
        public static DataTable dtSimulationVariableItems { get; set; }
        public static DataTable dtSimulationEquillibriumValues { get; set; }
        public static DataTable dtSimulationLoadingSummary { get; set; }
        public static DataTable dtSimulationDrafts { get; set; }
        public static DataTable dtSimulationGZ { get; set; }
        public static DataTable dtSimulationHydrostatics { get; set; }
        public static DataTable dtSimulationStabilitySummary { get; set; }
        public static DataTable dtSimulationStabilityCriteriaIntact { get; set; }
        public static DataTable dtSimulationStabilityCriteriaDamage { get; set; }
        public static DataTable dtSimulationLongitudinal { get; set; }
        public static DataTable dtSimulationSFBMMax { get; set; }

        public static DataSet dsSimulationDeadWeightDetails { get; set; }
        public static DataSet dsCoordinates { get; set; }

        public static DxfModel CentrelineProfile { get; set; }
        public static DxfModel BottomPlan { get; set; }
        public static DxfModel Deck40 { get; set; }
        public static DxfModel Deck30 { get; set; }
        public static DxfModel Deck20 { get; set; }

        public static decimal Tank1_PercentFill;
        public static decimal Tank2_PercentFill;
        public static decimal Tank3_PercentFill;
        public static decimal Tank4_PercentFill;
        public static decimal Tank5_PercentFill;
        public static decimal Tank6_PercentFill;
        public static decimal Tank7_PercentFill;
        public static decimal Tank8_PercentFill;
        public static decimal Tank9_PercentFill;
        public static decimal Tank10_PercentFill;
        public static decimal Tank11_PercentFill;
        public static decimal Tank12_PercentFill;
        public static decimal Tank13_PercentFill;
        public static decimal Tank14_PercentFill;
        public static decimal Tank15_PercentFill;
        public static decimal Tank16_PercentFill;
        public static decimal Tank17_PercentFill;
        public static decimal Tank18_PercentFill;
        public static decimal Tank19_PercentFill;
        public static decimal Tank20_PercentFill;
        public static decimal Tank21_PercentFill;
        public static decimal Tank22_PercentFill;
        public static decimal Tank23_PercentFill;
        public static decimal Tank24_PercentFill;
        public static decimal Tank25_PercentFill;
        public static decimal Tank26_PercentFill;
        public static decimal Tank27_PercentFill;
        public static decimal Tank28_PercentFill;
        public static decimal Tank29_PercentFill;
        public static decimal Tank30_PercentFill;
        public static decimal Tank31_PercentFill;
        public static decimal Tank32_PercentFill;
        public static decimal Tank33_PercentFill;
        public static decimal Tank34_PercentFill;
        public static decimal Tank35_PercentFill;
        public static decimal Tank36_PercentFill;
        public static decimal Tank37_PercentFill;
        public static decimal Tank38_PercentFill;
        public static decimal Tank39_PercentFill;
        public static decimal Tank40_PercentFill;
        public static decimal Tank41_PercentFill;
        public static decimal Tank42_PercentFill;
        public static decimal Tank43_PercentFill;
        public static decimal Tank44_PercentFill;
        public static decimal Tank45_PercentFill;
        public static decimal Tank46_PercentFill;
        public static decimal Tank47_PercentFill;
        public static decimal Tank48_PercentFill;
        public static decimal Tank49_PercentFill;
        public static decimal Tank50_PercentFill;
        public static decimal Tank51_PercentFill;
        public static decimal Tank52_PercentFill;
        public static decimal Tank53_PercentFill;
        public static decimal Tank54_PercentFill;
        public static decimal Tank55_PercentFill;
        public static decimal Tank56_PercentFill;
        public static decimal Tank57_PercentFill;
        public static decimal Tank58_PercentFill;
        public static decimal Tank59_PercentFill;
        public static decimal Tank60_PercentFill;
        public static decimal Tank61_PercentFill;
        public static decimal Tank62_PercentFill;
        public static decimal Tank63_PercentFill;
        public static decimal Tank64_PercentFill;
        public static decimal Tank65_PercentFill;
        public static decimal Tank66_PercentFill;
        public static decimal Tank67_PercentFill;
        public static decimal Tank68_PercentFill;
        public static decimal Tank69_PercentFill;
        public static decimal Tank70_PercentFill;
        public static decimal Tank71_PercentFill;


        public static double[] Tank1x = new double[18];
        public static double[] Tank2x = new double[18];
        public static double[] Tank3x = new double[18];
        public static double[] Tank4x = new double[18];
        public static double[] Tank5x = new double[18];
        public static double[] Tank6x = new double[18];
        public static double[] Tank7x = new double[18];
        public static double[] Tank8x = new double[18];
        public static double[] Tank9x = new double[18];
        public static double[] Tank10x = new double[18];
        public static double[] Tank11x = new double[18];
        public static double[] Tank12x = new double[18];
        public static double[] Tank13x = new double[18];
        public static double[] Tank14x = new double[18];
        public static double[] Tank15x = new double[18];
        public static double[] Tank16x = new double[18];
        public static double[] Tank17x = new double[18];
        public static double[] Tank18x = new double[18];
        public static double[] Tank19x = new double[18];
        public static double[] Tank20x = new double[18];
        public static double[] Tank21x = new double[18];
        public static double[] Tank22x = new double[18];
        public static double[] Tank23x = new double[18];
        public static double[] Tank24x = new double[18];
        public static double[] Tank25x = new double[18];
        public static double[] Tank26x = new double[18];
        public static double[] Tank27x = new double[18];
        public static double[] Tank28x = new double[18];
        public static double[] Tank29x = new double[18];
        public static double[] Tank30x = new double[18];
        public static double[] Tank31x = new double[18];
        public static double[] Tank32x = new double[18];
        public static double[] Tank33x = new double[18];
        public static double[] Tank34x = new double[18];
        public static double[] Tank35x = new double[18];
        public static double[] Tank36x = new double[18];
        public static double[] Tank37x = new double[18];
        public static double[] Tank38x = new double[18];
        public static double[] Tank39x = new double[18];
        public static double[] Tank40x = new double[18];
        public static double[] Tank41x = new double[18];
        public static double[] Tank42x = new double[18];
        public static double[] Tank43x = new double[18];
        public static double[] Tank44x = new double[18];
        public static double[] Tank45x = new double[18];
        public static double[] Tank46x = new double[18];
        public static double[] Tank47x = new double[18];
        public static double[] Tank48x = new double[18];
        public static double[] Tank49x = new double[18];
        public static double[] Tank50x = new double[18];
        public static double[] Tank51x = new double[18];
        public static double[] Tank52x = new double[18];
        public static double[] Tank53x = new double[18];
        public static double[] Tank54x = new double[18];
        public static double[] Tank55x = new double[18];
        public static double[] Tank56x = new double[18];
        public static double[] Tank57x = new double[18];
        public static double[] Tank58x = new double[18];
        public static double[] Tank59x = new double[18];
        public static double[] Tank60x = new double[18];
        public static double[] Tank61x = new double[18];
        public static double[] Tank62x = new double[18];
        public static double[] Tank63x = new double[18];
        public static double[] Tank64x = new double[18];
        public static double[] Tank65x = new double[18];
        public static double[] Tank66x = new double[18];
        public static double[] Tank67x = new double[18];
        public static double[] Tank68x = new double[18];

        public static double[] Tank1y = new double[18];
        public static double[] Tank2y = new double[18];
        public static double[] Tank3y = new double[18];
        public static double[] Tank4y = new double[18];
        public static double[] Tank5y = new double[18];
        public static double[] Tank6y = new double[18];
        public static double[] Tank7y = new double[18];
        public static double[] Tank8y = new double[18];
        public static double[] Tank9y = new double[18];
        public static double[] Tank10y = new double[18];
        public static double[] Tank11y = new double[18];
        public static double[] Tank12y = new double[18];
        public static double[] Tank13y = new double[18];
        public static double[] Tank14y = new double[18];
        public static double[] Tank15y = new double[18];
        public static double[] Tank16y = new double[18];
        public static double[] Tank17y = new double[18];
        public static double[] Tank18y = new double[18];
        public static double[] Tank19y = new double[18];
        public static double[] Tank20y = new double[18];
        public static double[] Tank21y = new double[18];
        public static double[] Tank22y = new double[18];
        public static double[] Tank23y = new double[18];
        public static double[] Tank24y = new double[18];
        public static double[] Tank25y = new double[18];
        public static double[] Tank26y = new double[18];
        public static double[] Tank27y = new double[18];
        public static double[] Tank28y = new double[18];
        public static double[] Tank29y = new double[18];
        public static double[] Tank30y = new double[18];
        public static double[] Tank31y = new double[18];
        public static double[] Tank32y = new double[18];
        public static double[] Tank33y = new double[18];
        public static double[] Tank34y = new double[18];
        public static double[] Tank35y = new double[18];
        public static double[] Tank36y = new double[18];
        public static double[] Tank37y = new double[18];
        public static double[] Tank38y = new double[18];
        public static double[] Tank39y = new double[18];
        public static double[] Tank40y = new double[18];
        public static double[] Tank41y = new double[18];
        public static double[] Tank42y = new double[18];
        public static double[] Tank43y = new double[18];
        public static double[] Tank44y = new double[18];
        public static double[] Tank45y = new double[18];
        public static double[] Tank46y = new double[18];
        public static double[] Tank47y = new double[18];
        public static double[] Tank48y = new double[18];
        public static double[] Tank49y = new double[18];
        public static double[] Tank50y = new double[18];
        public static double[] Tank51y = new double[18];
        public static double[] Tank52y = new double[18];
        public static double[] Tank53y = new double[18];
        public static double[] Tank54y = new double[18];
        public static double[] Tank55y = new double[18];
        public static double[] Tank56y = new double[18];
        public static double[] Tank57y = new double[18];
        public static double[] Tank58y = new double[18];
        public static double[] Tank59y = new double[18];
        public static double[] Tank60y = new double[18];
        public static double[] Tank61y = new double[18];
        public static double[] Tank62y = new double[18];
        public static double[] Tank63y = new double[18];
        public static double[] Tank64y = new double[18];
        public static double[] Tank65y = new double[18];
        public static double[] Tank66y = new double[18];
        public static double[] Tank67y = new double[18];
        public static double[] Tank68y = new double[18];

        public static double[,] mulDeckPlan = new double[69, 6];
        public static int[] Tank_IDDeckPlan = new int[69];
        public static double[] TankX_maxDeckPlan = new double[69];
        public static double[] TankX_minDeckPlan = new double[69];
        public static double[] TankY_maxDeckPlan = new double[69];
        public static double[] TankY_minDeckPlan = new double[69];
        public static string[] Tank_NameDeckPlan = new string[69];
        public static double[,] MxMnCurvedDeckPlan = new double[13, 5];


        public class ProfileCoordinate
        {
            public static double[] Tank1x = new double[5];
            public static double[] Tank2x = new double[5];
            public static double[] Tank3x = new double[5];
            public static double[] Tank4x = new double[5];
            public static double[] Tank5x = new double[5];
            public static double[] Tank6x = new double[5];
            public static double[] Tank7x = new double[5];
            public static double[] Tank8x = new double[5];
            public static double[] Tank9x = new double[5];
            public static double[] Tank10x = new double[5];
            public static double[] Tank11x = new double[5];
            public static double[] Tank12x = new double[5];
            public static double[] Tank13x = new double[5];
            public static double[] Tank14x = new double[5];
            public static double[] Tank15x = new double[5];
            public static double[] Tank16x = new double[5];
            public static double[] Tank17x = new double[5];
            public static double[] Tank18x = new double[5];
            public static double[] Tank19x = new double[5];
            public static double[] Tank20x = new double[5];
            public static double[] Tank21x = new double[5];
            public static double[] Tank22x = new double[5];
            public static double[] Tank23x = new double[5];
            public static double[] Tank24x = new double[5];
            public static double[] Tank25x = new double[5];
            public static double[] Tank26x = new double[5];
            public static double[] Tank27x = new double[5];
            public static double[] Tank28x = new double[5];
            public static double[] Tank29x = new double[5];
            public static double[] Tank30x = new double[5];
            public static double[] Tank31x = new double[5];
            public static double[] Tank32x = new double[5];
            public static double[] Tank33x = new double[5];
            public static double[] Tank34x = new double[5];
            public static double[] Tank35x = new double[5];
            public static double[] Tank36x = new double[5];
            public static double[] Tank37x = new double[5];
            public static double[] Tank38x = new double[5];
            public static double[] Tank39x = new double[5];
            public static double[] Tank40x = new double[5];
            public static double[] Tank41x = new double[5];
            public static double[] Tank42x = new double[5];
            public static double[] Tank43x = new double[5];
            public static double[] Tank44x = new double[5];
            public static double[] Tank45x = new double[5];
            public static double[] Tank46x = new double[5];
            public static double[] Tank47x = new double[5];
            public static double[] Tank48x = new double[5];
            public static double[] Tank49x = new double[5];
            public static double[] Tank50x = new double[5];
            public static double[] Tank51x = new double[5];
            public static double[] Tank52x = new double[5];
            public static double[] Tank53x = new double[5];
            public static double[] Tank54x = new double[5];
            public static double[] Tank55x = new double[5];
            public static double[] Tank56x = new double[5];
            public static double[] Tank57x = new double[5];
            public static double[] Tank58x = new double[5];
            public static double[] Tank59x = new double[5];
            public static double[] Tank60x = new double[5];
            public static double[] Tank61x = new double[5];
            public static double[] Tank62x = new double[5];
            public static double[] Tank63x = new double[5];
            public static double[] Tank64x = new double[5];
            public static double[] Tank65x = new double[5];
            public static double[] Tank66x = new double[5];
            public static double[] Tank67x = new double[5];
            public static double[] Tank68x = new double[5];
           

            public static double[] Tank1y = new double[5];
            public static double[] Tank2y = new double[5];
            public static double[] Tank3y = new double[5];
            public static double[] Tank4y = new double[5];
            public static double[] Tank5y = new double[5];
            public static double[] Tank6y = new double[5];
            public static double[] Tank7y = new double[5];
            public static double[] Tank8y = new double[5];
            public static double[] Tank9y = new double[5];
            public static double[] Tank10y = new double[5];
            public static double[] Tank11y = new double[5];
            public static double[] Tank12y = new double[5];
            public static double[] Tank13y = new double[5];
            public static double[] Tank14y = new double[5];
            public static double[] Tank15y = new double[5];
            public static double[] Tank16y = new double[5];
            public static double[] Tank17y = new double[5];
            public static double[] Tank18y = new double[5];
            public static double[] Tank19y = new double[5];
            public static double[] Tank20y = new double[5];
            public static double[] Tank21y = new double[5];
            public static double[] Tank22y = new double[5];
            public static double[] Tank23y = new double[5];
            public static double[] Tank24y = new double[5];
            public static double[] Tank25y = new double[5];
            public static double[] Tank26y = new double[5];
            public static double[] Tank27y = new double[5];
            public static double[] Tank28y = new double[5];
            public static double[] Tank29y = new double[5];
            public static double[] Tank30y = new double[5];
            public static double[] Tank31y = new double[5];
            public static double[] Tank32y = new double[5];
            public static double[] Tank33y = new double[5];
            public static double[] Tank34y = new double[5];
            public static double[] Tank35y = new double[5];
            public static double[] Tank36y = new double[5];
            public static double[] Tank37y = new double[5];
            public static double[] Tank38y = new double[5];
            public static double[] Tank39y = new double[5];
            public static double[] Tank40y = new double[5];
            public static double[] Tank41y = new double[5];
            public static double[] Tank42y = new double[5];
            public static double[] Tank43y = new double[5];
            public static double[] Tank44y = new double[5];
            public static double[] Tank45y = new double[5];
            public static double[] Tank46y = new double[5];
            public static double[] Tank47y = new double[5];
            public static double[] Tank48y = new double[5];
            public static double[] Tank49y = new double[5];
            public static double[] Tank50y = new double[5];
            public static double[] Tank51y = new double[5];
            public static double[] Tank52y = new double[5];
            public static double[] Tank53y = new double[5];
            public static double[] Tank54y = new double[5];
            public static double[] Tank55y = new double[5];
            public static double[] Tank56y = new double[5];
            public static double[] Tank57y = new double[5];
            public static double[] Tank58y = new double[5];
            public static double[] Tank59y = new double[5];
            public static double[] Tank60y = new double[5];
            public static double[] Tank61y = new double[5];
            public static double[] Tank62y = new double[5];
            public static double[] Tank63y = new double[5];
            public static double[] Tank64y = new double[5];
            public static double[] Tank65y = new double[5];
            public static double[] Tank66y = new double[5];
            public static double[] Tank67y = new double[5];
            public static double[] Tank68y = new double[5];


            public static double[,] mul = new double[69,6];
            public static int[] Tank_ID = new int[69];
            public static double[] TankX_max = new double[69];
            public static double[] TankX_min = new double[69];
            public static double[] TankY_max = new double[69];
            public static double[] TankY_min = new double[69];
            public static string[] Tank_Name = new string[69];
            public static double[,] MxMnCurved = new double[18,5];

        }

    }
}
