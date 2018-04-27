using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Windows;
namespace ZebecLoadMaster.Models.DAL
{
    class clsDBUtilityMethods
    {
        #region UtilityMethodz
        ///  <summary> 
        ///Returns a DataSet filled with data from the execution of the given Command. 
        ///</summary> 
        ///<param name="command">Command Object filled with necessary parameters.</param> 
        ///<param name="ErrorMessage">Output parameter for getting the error message if any.</param> 
        ///<returns>Returns a DataSet filled with data from the execution of the given Command.</returns> 
        public static DataTable GetTable(DbCommand command, String ErrorMessage)
        {
            ErrorMessage = String.Empty;
            DataTable dtable = new DataTable();
            DbTransaction tran = null;
            DbConnection conn = null;
            DbDataReader reader = null;

            if (command == null)
            {
                ErrorMessage = "Please initilise the command object.";
                return dtable;
            }

            try
            {
                conn = clsSqlData.GetConnection();
                command.Connection = conn;
                conn.Open();
                tran = conn.BeginTransaction();
                command.Transaction = tran;
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
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
                if (command != null)
                {
                    command.Dispose();
                }
                if (tran != null)
                {
                    tran.Dispose();
                }
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
            return dtable;
        }

        
       /// <summary> 
       /// Returns a DbCommand object. 
       /// </summary> 
       /// <returns>Returns a DbCommand object.</returns> 
        public static DbCommand GetCommand()
        {
            DbCommand comm = clsSqlData.GetDbFactory().CreateCommand();
            return comm;
        }



        ///<summary> 
        ///Returns a DataSet filled with data from the execution of the given Command. 
        ///</summary> 
        ///<param name="command">Command Object filled with necessary parameters.</param> 
        ///<param name="ErrorMessage">Output parameter for getting the error message if any.</param> 
        ///<returns>Returns a DataSet filled with data from the execution of the given Command.</returns> 
        public static DataSet GetDataSet(DbCommand command, String ErrorMessage)
        {
            ErrorMessage = String.Empty;
            DataSet ds = new DataSet();
            DbDataAdapter dbDap = null;

            if (command == null)
            {
                ErrorMessage = "Please initilise the command object.";
                return ds;
            }

            try
            {
                DbConnection conn = clsSqlData.GetConnection();
                command.Connection = conn;
                conn.Open();
                DbProviderFactory Dbfactory = clsSqlData.GetDbFactory();
                dbDap = Dbfactory.CreateDataAdapter();
                dbDap.SelectCommand = command;
                dbDap.Fill(ds);
            }
            catch (DbException exp)
            {
                ErrorMessage = "An Exception has occured while executing the database transaction." + exp.Message;
                
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }
                if (dbDap != null)
                {
                    dbDap.Dispose();
                }
              
            }
            return ds;
        }
        
        ///<summary> 
        ///Executes the command object returning an int value. 
        ///</summary> 
        ///<param name="command">Command object filled with the necessary parameters.</param> 
        ///<param name="ErrorMessage">Error Message if any.</param> 
        ///<returns>Int32 value indicating the error.</returns> 
        public static Int32 ExecuteNonQuery(DbCommand command, String ErrorMessage)
        {
            ErrorMessage = String.Empty;
            DbTransaction tran = null;
            DbConnection conn = null;
            Int32 result = 0;
            if (command == null)
            {
                ErrorMessage = "Please initilise the command object.";
                return -1;
            }

            try
            {
                conn = clsSqlData.GetConnection();
                command.Connection = conn;
                command.CommandTimeout = 1000;
                conn.Open();
                tran = conn.BeginTransaction();
                command.Transaction = tran;
                result = command.ExecuteNonQuery();
                tran.Commit();
               
            }
            catch (DbException exp)
            {
              //  MessageBox.Show("An Exception has occured while executing the database transaction.Please Check Configuration Settings");
                ErrorMessage = "An Exception has occured while executing the database transaction." + exp.Message;
                if (tran != null)
                {
                    tran.Rollback();
                }
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }
                if (tran != null)
                {
                    tran.Dispose();
                }
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
            return result;
        }
 
        /// <summary> 
        /// Executes the command object returning the first column of the first row. 
        /// </summary> 
        /// <param name="command">Command object filled with the necessary parameters.</param> 
        /// <param name="ErrorMessage">Error Message if any.</param> 
        /// <returns>An object containing the first column of the first row.</returns> 
        public static object ExecuteScalar(DbCommand command, String ErrorMessage)
        {
            ErrorMessage = String.Empty;
            DbTransaction tran = null;
            DbConnection conn = null;
            object result=null;
            if (command == null)
            {
                ErrorMessage = "Please initilise the command object.";
                return null;
            }

            try
            {
                conn = clsSqlData.GetConnection();
                command.Connection = conn;
                conn.Open();
                tran = conn.BeginTransaction();
                command.Transaction = tran;
                result = command.ExecuteScalar();
                tran.Commit();

            }
            catch (DbException exp)
            {
                ErrorMessage = "An Exception has occured while executing the database transaction." + exp.Message;
                if (tran != null)
                {
                    tran.Rollback();
                }
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }
                if (tran != null)
                {
                    tran.Dispose();
                }
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                if (conn != null)
                {
                    conn.Dispose();
                }
            }
            return result;
        }

        /// <summary> 
        /// Returns an array of DbParameter objects. 
        /// </summary> 
        /// <param name="count">Denotes the size of the required parameter array. Should be greater than 0.</param> 
        /// <returns>Returns an array of DbParameter objects.</returns> 
        public static DbParameter GetParameter()
        {
            DbProviderFactory Dbfactory = clsSqlData.GetDbFactory();
            DbParameter parameter = Dbfactory.CreateParameter();
            return parameter;
        }

        public static DbParameter[] GetParameters(ushort count)
        {
            DbProviderFactory Dbfactory = clsSqlData.GetDbFactory();
            DbParameter[] parameters = new DbParameter[] { };
            try
            {
                for(int i=0;i<=count-1;i++)
                {
                    parameters[i]=Dbfactory.CreateParameter();
                }
            }
            catch
            {
                throw new Exception("Count has to be greater than zero and less than 100", new IndexOutOfRangeException());
            }
              return parameters;
        }
      
        #endregion UtilityMethod
    }
}
