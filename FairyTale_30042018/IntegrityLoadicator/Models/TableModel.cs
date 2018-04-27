using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace ZebecLoadMaster.Models
{
    class TableModel
    {
        public static void RealModeData()
        {

        }
        public static void SimulationModeData()
        {
            Models.clsGlobVar.dtSimulationAllTanks = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationModeAllTankFillDetails");
            Models.clsGlobVar.dtSimulationAllTanksForDamage = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationModeAllTankFillDetailsForDamage");
            Models.clsGlobVar.dtSimulationBallastTanks = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationBallastTankLoadingStatusDetails");
            Models.clsGlobVar.dtSimulationFuelOilTanks = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationFuelOilTankLoadingStatusDetails");
            Models.clsGlobVar.dtSimulationFreshWaterTanks = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationFreshWaterTankLoadingStatusDetails");
            Models.clsGlobVar.dtSimulationMiscTanks = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationMiscTankLoadingStatusDetails");
            Models.clsGlobVar.dtSimulationCompartments = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationCompartmentLoadingStatusDetails");
            Models.clsGlobVar.dtSimulationVariableItems = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationModeVariableDetails");
            Models.clsGlobVar.dtSimulationEquillibriumValues = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationModeEquillibriumValues");
            Models.clsGlobVar.dtSimulationLoadingSummary = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationModeLoadingSummaryCurrent");
            Models.clsGlobVar.dtSimulationDrafts = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationModeDraftsCurrent");
            Models.clsGlobVar.dtSimulationHydrostatics = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationModeHydrostaticDataCurrent");
            Models.clsGlobVar.dtSimulationStabilityCriteriaIntact = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationModeIntactStabilityCriteriaCurrent");
            Models.clsGlobVar.dtSimulationStabilityCriteriaDamage = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationModeDamageStabilityCriteriaCurrent");
            Models.clsGlobVar.dtSimulationStabilitySummary = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationModeStabilitySummary");
            Models.clsGlobVar.dtSimulationLongitudinal = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationModeLongitudinalDataCurrent");
            Models.clsGlobVar.dtSimulationSFBMMax = Models.BLL.clsBLL.GetEnttyDBRecs("vsGetSimulationModeSFBMMax");
            #region SP_Calling
            {
                Models.clsGlobVar.FloodingPoint_Damage = Models.DAL.clsDAL.SP_Execution("[spGet_flooding_points_damage]");
            }
            #endregion

            string sCmd = "Select Sum(Weight) From [tblSimulationMode_Loading_Condition] Where [USER] = 'dbo' and Tank_ID between 1 and 10";
            sCmd += " Select Sum(Weight) From [tblSimulationMode_Loading_Condition] Where [USER] = 'dbo' and Tank_ID between 11 and 22";
            sCmd += " Select Sum(Weight) From [tblSimulationMode_Loading_Condition] Where [USER] = 'dbo' and Tank_ID between 23 and 24";
            sCmd += " Select Sum(Weight) From [tblSimulationMode_Loading_Condition] Where [USER] = 'dbo' and Tank_ID between 25 and 35";
            sCmd += " Select Sum(Weight) From [tblSimulationMode_Loading_Condition] Where [USER] = 'dbo' and Tank_ID between 36 and 41";
            sCmd += " Select Sum(Weight) From [tblSimulationMode_Loading_Condition] Where [USER] = 'dbo' and Tank_ID between 42 and 46";
            sCmd += " Select Sum(Weight) From [tblSimulationMode_Loading_Condition] Where [USER] = 'dbo' and Tank_ID between 47 and 52";
            sCmd += " Select Sum(Weight) From [tblSimulationMode_Loading_Condition] Where [USER] = 'dbo' and Tank_ID=53 ";
             DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
             command.CommandText = sCmd;
             command.CommandType = CommandType.Text;
             string Err = "";
            Models.clsGlobVar.dsSimulationDeadWeightDetails = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
        }

        public static void CoordinateData()
        {
           string  sCmd = "Select * from [DeckPlan] ";
           sCmd += "Select * from [Profile_View]";
            DbCommand command = Models.DAL.clsDBUtilityMethods.GetCommand();
            command.CommandText = sCmd;
            command.CommandType = CommandType.Text;
            string Err = "";
            Models.clsGlobVar.dsCoordinates = Models.DAL.clsDBUtilityMethods.GetDataSet(command, Err);
            DataTable dtCoordinatesDeckPlan = new DataTable();
            dtCoordinatesDeckPlan = Models.clsGlobVar.dsCoordinates.Tables[0];
            for (int i = 1; i <= 17; i++)
            {
                string sc = Convert.ToString("X" + i);
                string sr = Convert.ToString("Y" + i);

                Models.clsGlobVar.Tank1x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[0][sc]);
                Models.clsGlobVar.Tank2x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[1][sc]);
                Models.clsGlobVar.Tank3x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[2][sc]);
                Models.clsGlobVar.Tank4x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[3][sc]);
                Models.clsGlobVar.Tank5x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[4][sc]);
                Models.clsGlobVar.Tank6x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[5][sc]);
                Models.clsGlobVar.Tank7x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[6][sc]);
                Models.clsGlobVar.Tank8x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[7][sc]);
                Models.clsGlobVar.Tank9x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[8][sc]);
                Models.clsGlobVar.Tank10x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[9][sc]);
                Models.clsGlobVar.Tank11x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[10][sc]);
                Models.clsGlobVar.Tank12x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[11][sc]);
                Models.clsGlobVar.Tank13x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[12][sc]);
                Models.clsGlobVar.Tank14x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[13][sc]);
                Models.clsGlobVar.Tank15x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[14][sc]);
                Models.clsGlobVar.Tank16x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[15][sc]);
                Models.clsGlobVar.Tank17x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[16][sc]);
                Models.clsGlobVar.Tank18x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[17][sc]);
                Models.clsGlobVar.Tank19x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[18][sc]);
                Models.clsGlobVar.Tank20x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[19][sc]);
                Models.clsGlobVar.Tank21x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[20][sc]);
                Models.clsGlobVar.Tank22x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[21][sc]);
                Models.clsGlobVar.Tank23x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[22][sc]);
                Models.clsGlobVar.Tank24x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[23][sc]);
                Models.clsGlobVar.Tank25x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[24][sc]);
                Models.clsGlobVar.Tank26x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[25][sc]);
                Models.clsGlobVar.Tank27x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[26][sc]);
                Models.clsGlobVar.Tank28x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[27][sc]);
                Models.clsGlobVar.Tank29x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[28][sc]);
                Models.clsGlobVar.Tank30x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[29][sc]);
                Models.clsGlobVar.Tank31x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[30][sc]);
                Models.clsGlobVar.Tank32x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[31][sc]);
                Models.clsGlobVar.Tank33x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[32][sc]);
                Models.clsGlobVar.Tank34x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[33][sc]);
                Models.clsGlobVar.Tank35x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[34][sc]);
                Models.clsGlobVar.Tank36x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[35][sc]);
                Models.clsGlobVar.Tank37x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[36][sc]);
                Models.clsGlobVar.Tank38x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[37][sc]);
                Models.clsGlobVar.Tank39x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[38][sc]);
                Models.clsGlobVar.Tank40x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[39][sc]);
                Models.clsGlobVar.Tank41x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[40][sc]);
                Models.clsGlobVar.Tank42x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[41][sc]);
                Models.clsGlobVar.Tank43x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[42][sc]);
                Models.clsGlobVar.Tank44x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[43][sc]);
                Models.clsGlobVar.Tank45x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[44][sc]);
                Models.clsGlobVar.Tank46x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[45][sc]);
                Models.clsGlobVar.Tank47x[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[46][sc]);

                                                                                           

                Models.clsGlobVar.Tank1y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[0][sr]);
                Models.clsGlobVar.Tank2y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[1][sr]);
                Models.clsGlobVar.Tank3y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[2][sr]);
                Models.clsGlobVar.Tank4y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[3][sr]);
                Models.clsGlobVar.Tank5y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[4][sr]);
                Models.clsGlobVar.Tank6y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[5][sr]);
                Models.clsGlobVar.Tank7y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[6][sr]);
                Models.clsGlobVar.Tank8y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[7][sr]);
                Models.clsGlobVar.Tank9y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[8][sr]);
                Models.clsGlobVar.Tank10y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[9][sr]);
                Models.clsGlobVar.Tank11y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[10][sr]);
                Models.clsGlobVar.Tank12y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[11][sr]);
                Models.clsGlobVar.Tank13y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[12][sr]);
                Models.clsGlobVar.Tank14y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[13][sr]);
                Models.clsGlobVar.Tank15y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[14][sr]);
                Models.clsGlobVar.Tank16y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[15][sr]);
                Models.clsGlobVar.Tank17y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[16][sr]);
                Models.clsGlobVar.Tank18y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[17][sr]);
                Models.clsGlobVar.Tank19y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[18][sr]);
                Models.clsGlobVar.Tank20y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[19][sr]);
                Models.clsGlobVar.Tank21y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[20][sr]);
                Models.clsGlobVar.Tank22y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[21][sr]);
                Models.clsGlobVar.Tank23y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[22][sr]);
                Models.clsGlobVar.Tank24y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[23][sr]);
                Models.clsGlobVar.Tank25y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[24][sr]);
                Models.clsGlobVar.Tank26y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[25][sr]);
                Models.clsGlobVar.Tank27y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[26][sr]);
                Models.clsGlobVar.Tank28y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[27][sr]);
                Models.clsGlobVar.Tank29y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[28][sr]);
                Models.clsGlobVar.Tank30y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[29][sr]);
                Models.clsGlobVar.Tank31y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[30][sr]);
                Models.clsGlobVar.Tank32y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[31][sr]);
                Models.clsGlobVar.Tank33y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[32][sr]);
                Models.clsGlobVar.Tank34y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[33][sr]);
                Models.clsGlobVar.Tank35y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[34][sr]);
                Models.clsGlobVar.Tank36y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[35][sr]);
                Models.clsGlobVar.Tank37y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[36][sr]);
                Models.clsGlobVar.Tank38y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[37][sr]);
                Models.clsGlobVar.Tank39y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[38][sr]);
                Models.clsGlobVar.Tank40y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[39][sr]);
                Models.clsGlobVar.Tank41y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[40][sr]);
                Models.clsGlobVar.Tank42y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[41][sr]);
                Models.clsGlobVar.Tank43y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[42][sr]);
                Models.clsGlobVar.Tank44y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[43][sr]);
                Models.clsGlobVar.Tank45y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[44][sr]);
                Models.clsGlobVar.Tank46y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[45][sr]);
                Models.clsGlobVar.Tank47y[i] = Convert.ToDouble(dtCoordinatesDeckPlan.Rows[46][sr]);

            }
            

            DataTable dtCoordinatesProfile = new DataTable();
            dtCoordinatesProfile = Models.clsGlobVar.dsCoordinates.Tables[1];
            for (int i = 1; i <= 2; i++)
            {
                //DataTable dt = new DataTable();
                string sc = Convert.ToString("X" + i);
                string sr = Convert.ToString("Y" + i);
                Models.clsGlobVar.ProfileCoordinate.Tank1x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[0][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank2x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[1][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank3x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[2][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank4x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[3][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank5x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[4][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank6x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[5][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank7x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[6][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank8x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[7][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank9x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[8][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank10x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[9][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank11x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[10][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank12x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[11][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank13x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[12][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank14x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[13][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank15x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[14][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank16x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[15][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank17x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[16][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank18x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[17][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank19x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[18][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank20x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[19][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank21x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[20][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank22x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[21][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank23x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[22][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank24x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[23][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank25x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[24][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank26x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[25][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank27x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[26][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank28x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[27][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank29x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[28][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank30x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[29][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank31x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[30][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank32x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[31][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank33x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[32][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank34x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[33][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank35x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[34][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank36x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[35][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank37x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[36][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank38x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[37][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank39x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[38][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank40x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[39][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank41x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[40][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank42x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[41][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank43x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[42][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank44x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[43][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank45x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[44][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank46x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[45][sc]);
                Models.clsGlobVar.ProfileCoordinate.Tank47x[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[46][sc]);

    

                Models.clsGlobVar.ProfileCoordinate.Tank1y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[0][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank2y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[1][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank3y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[2][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank4y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[3][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank5y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[4][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank6y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[5][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank7y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[6][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank8y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[7][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank9y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[8][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank10y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[9][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank11y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[10][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank12y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[11][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank13y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[12][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank14y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[13][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank15y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[14][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank16y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[15][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank17y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[16][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank18y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[17][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank19y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[18][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank20y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[19][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank21y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[20][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank22y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[21][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank23y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[22][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank24y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[23][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank25y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[24][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank26y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[25][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank27y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[26][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank28y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[27][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank29y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[28][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank30y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[29][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank31y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[30][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank32y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[31][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank33y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[32][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank34y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[33][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank35y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[34][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank36y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[35][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank37y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[36][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank38y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[37][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank39y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[38][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank40y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[39][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank41y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[40][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank42y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[41][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank43y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[42][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank44y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[43][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank45y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[44][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank46y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[45][sr]);
                Models.clsGlobVar.ProfileCoordinate.Tank47y[i] = Convert.ToDouble(dtCoordinatesProfile.Rows[46][sr]);

            }
        }
    }
}
