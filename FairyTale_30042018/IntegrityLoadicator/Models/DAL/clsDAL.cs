using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Data.SqlClient;

namespace ZebecLoadMaster.Models.DAL
{
    class clsDAL
    {
        public static DataTable GetAllRecsDT(String sSelAllCmdText)
        {
            String sErrMsg = "";
            try
            {
                //Create Get All Records SQL Command
                DbCommand getAllCmd = clsDBUtilityMethods.GetCommand();
                getAllCmd.CommandText = sSelAllCmdText;
                getAllCmd.CommandType = CommandType.Text;
                return clsDBUtilityMethods.GetTable(getAllCmd, sErrMsg);
            }
            catch
            {
                return null;
            }
            finally
            {

            }
        }

    //
    // Get Records From Database
    //
        public static Boolean GetAllRecs(String sSelAllCmdText, DataTable dt)
        {
            String sErrMsg = "";
            try
            {
                DbCommand getAllCmd = clsDBUtilityMethods.GetCommand();
                getAllCmd.CommandText = sSelAllCmdText;
                getAllCmd.CommandType = CommandType.Text;
                dt = clsDBUtilityMethods.GetTable(getAllCmd, sErrMsg);
                return true;
            }
            catch
            {
                return false;
            }

            finally
            {
            }

        }
        public static Boolean isRecExist(String sSelCmdText)
        {
            String sErrMsg = "";
            try
            {
                DbCommand getRecCmd = clsDBUtilityMethods.GetCommand();
                getRecCmd.CommandText = sSelCmdText;
                getRecCmd.CommandType = CommandType.Text;
                Object retValue = clsDBUtilityMethods.ExecuteScalar(getRecCmd, sErrMsg);
                if (retValue != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }

        }

        public static Boolean GetRecCount(String sSelCmdText,Int32 nRecs)
        {
            String sErrMsg = "";
            try
            {
                DbCommand getRecCmd = clsDBUtilityMethods.GetCommand();
                getRecCmd.CommandText = sSelCmdText;
                getRecCmd.CommandType = CommandType.Text;
                nRecs =Convert.ToInt32(clsDBUtilityMethods.ExecuteScalar(getRecCmd, sErrMsg));
                return true;
               
            }
            catch
            {
                return false;
            }
            finally
            {
            }

        }
   

    //
    // Get Record From Database
    //

        public static Boolean GetRec(String sSelCmdText, DataTable dt)
        {
            String sErrMsg = "";
            try
            {
                DbCommand getRecCmd = clsDBUtilityMethods.GetCommand();
                getRecCmd.CommandText = sSelCmdText;
                getRecCmd.CommandType = CommandType.Text;
                dt = clsDBUtilityMethods.GetTable(getRecCmd, sErrMsg);
                return true;

            }
            catch
            {
                return false;
            }
            finally
            {
            }

        }

    //
    // Insert Record into Database
    //
        public static Int32 Insert(String sInsCmdText, Int32 nRecs)
        {
            String sErrMsg = "";
            try
            {
                DbCommand InsertCmd = clsDBUtilityMethods.GetCommand();
                InsertCmd.CommandText = sInsCmdText;
                InsertCmd.CommandType = CommandType.Text;
                nRecs = clsDBUtilityMethods.ExecuteNonQuery(InsertCmd, sErrMsg);
                return nRecs;

            }
            catch
            {
                return 0;
            }
            finally
            {
            }

        }

        public static Int32 Insert(String sInsCmdText, Int32 result,Int32 id)
        {
            String sErrMsg = "";
           
            try
            {
                DbCommand InsertCmd = clsDBUtilityMethods.GetCommand();
                InsertCmd.CommandText = sInsCmdText;
                InsertCmd.CommandType = CommandType.Text;
                object retval = clsDBUtilityMethods.ExecuteScalar(InsertCmd, sErrMsg);
                result = Convert.ToInt32(retval);
                return result;

            }
            catch
            {
                return 0;
            }
            finally
            {
            }

        }
   
    //
    // Insert Record into Database
    //
        public static Boolean InsertIdentityRec(String sInsCmdText, Int32 nIdentity)
        {
            String sErrMsg = "";
           
            try
            {
                DbCommand InsertCmd = clsDBUtilityMethods.GetCommand();
                InsertCmd.CommandText = sInsCmdText;
                InsertCmd.CommandType = CommandType.Text;
                object retval = clsDBUtilityMethods.ExecuteScalar(InsertCmd, sErrMsg);
                nIdentity = Convert.ToInt32(retval);
                return true;

            }
            catch
            {
                return false;
            }
            finally
            {
            }

        }

    //
    // Update Record into Database
    //

        public static Boolean Update(String sUpdtCmdText, Int32 nRecs)
        {
            String sErrMsg = "";

            try
            {
                DbCommand UpdateCmd = clsDBUtilityMethods.GetCommand();
                UpdateCmd.CommandText = sUpdtCmdText;
                UpdateCmd.CommandType = CommandType.Text;
                object retval = clsDBUtilityMethods.ExecuteScalar(UpdateCmd, sErrMsg);
                retval = Convert.ToInt32(retval);
                return true;

            }
            catch
            {
                return false;
            }
            finally
            {
            }

        }
    
    //
    // Delete Record From Database
    //
        public static Int32 Delete(String sDelCmdText, Int32 nRecs)
        {
            String sErrMsg = "";
           
            try
            {
                DbCommand DeleteCmd = clsDBUtilityMethods.GetCommand();
                DeleteCmd.CommandText = sDelCmdText;
                DeleteCmd.CommandType = CommandType.Text;
                object retval = clsDBUtilityMethods.ExecuteScalar(DeleteCmd, sErrMsg);
                nRecs = Convert.ToInt32(retval);
                return nRecs;

            }
            catch
            {
                return 0;
            }
            finally
            {
            }

        }

        public static DataTable SP_Execution(string Sp_Name)
        {

            string ErrorMessage = String.Empty;
            DataTable dtable = new DataTable();
            DbTransaction tran = null;
            DbConnection conn = null;
            DbDataReader reader = null;
            DbCommand cmd = null;

            SqlConnection Con = new SqlConnection();
            try
            {
                conn = clsSqlData.GetConnection();
                Con = (SqlConnection)conn;
                Con.Open();
                cmd = new System.Data.SqlClient.SqlCommand(Sp_Name, Con);
                cmd.CommandType = CommandType.StoredProcedure;

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dtable.Load(reader);
                }
                else
                {
                    ErrorMessage = "No Records found.";

                    return null;
                }
            }

            catch (DbException exp)
            {
                clsSqlData.ConnectionError = true;
                ErrorMessage = "An Exception has occured while executing the database transaction." + exp.Message;
                if (tran != null)
                {
                    tran.Rollback();
                }
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (tran != null)
                {
                    tran.Dispose();
                }
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }
                if (Con != null)
                {
                    Con.Dispose();
                }
            }
            return dtable;
        }


    }
}