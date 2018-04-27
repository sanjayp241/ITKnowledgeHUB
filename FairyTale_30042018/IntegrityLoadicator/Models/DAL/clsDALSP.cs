using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections;

namespace ZebecLoadMaster.Models.DAL
{
    class clsDALSP
    {
        #region Methods For Database Operation
    /// <summary> 
    /// Returns a DataTable filled with data from the execution of the given Command. 
    /// </summary> 
    /// <param name="strSelStoreProcName">Stored Procedure Name</param> 
    /// <returns>Returns a DataTable filled with data from the execution of the given Command.</returns> 
        public static DataTable GetAllRecsDT(String strSelStoreProcName)
        {
            String sErrMsg = "";
            try
            {
                //Create Get All records SQL Command
                DbCommand getAllCmd = clsDBUtilityMethods.GetCommand();
                getAllCmd.CommandText = strSelStoreProcName;
                getAllCmd.CommandType = CommandType.StoredProcedure;
                return clsDBUtilityMethods.GetTable(getAllCmd, sErrMsg);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
            }
        }
    
    /// <summary> 
    /// Returns a DataTable filled with data from the execution of the given Command. 
    /// </summary> 
    /// <param name="strSelStoreProcName">Stored Procedure Name</param> 
    /// <param name="SPparmaCollection">Parameters Collections</param> 
    /// <returns>Returns a HashTable that has dataTable and command object filled with data from the execution of the given Command.</returns> 

        public static Hashtable GetAllrecsDT(String strSelStoreProcName, SqlParameter[] SPparmaCollection)
        {
            String sErrmsg = "";
            DataTable dt = new DataTable();
            Hashtable ht = new Hashtable();
            DbCommand getAllCmd = clsDBUtilityMethods.GetCommand();
            getAllCmd.CommandText = strSelStoreProcName;
            getAllCmd.CommandType = CommandType.StoredProcedure;
            try
            {
                //fetch parameter from parameter collection
                if (SPparmaCollection != null)
                {
                    foreach (SqlParameter aParameter in SPparmaCollection)
                    {
                        if (aParameter != null)
                        {
                            getAllCmd.Parameters.Add(aParameter);
                        }
                    }
                }
                dt = clsDBUtilityMethods.GetTable(getAllCmd, sErrmsg);
                ht.Add("datatable", dt);
                ht.Add("comobj", getAllCmd);
            }
            catch
            {
                ht.Clear();
                return null;
            }
            finally
            {
            }
            return ht;
        }


   
    //''' <summary> 
    //''' Returns a Boolean filled with data from the execution of the given Command. 
    //''' </summary> 
    //''' <param name="strStoreProcName">Stored Procedure Name</param> 
    //''' <returns>Returns a Boolean filled with data from the execution of the given Command.</returns> 
    //Public Overloads Function GetAllRecs(ByVal strStoreProcName As String) As Boolean
    //    '
    //    Dim sErrMsg As String = ""
    //    '
    //    Try
    //        ' Create Get Record  SQL Command
    //        Dim getRecCmd = clsDBUtilityMethods.GetCommand()
    //        getRecCmd.CommandText = strStoreProcName
    //        getRecCmd.CommandType = CommandType.StoredProcedure
    //        '

    //        Dim retValue As Object = clsDBUtilityMethods.ExecuteScalar(getRecCmd, sErrMsg)

    //        If Not retValue Is Nothing Then

    //            Return True

    //        Else
    //            Return False
    //        End If

    //    Catch ex As Exception
    //        Throw
    //        Return False
    //    Finally
    //        '
    //    End Try
    //    ''
    //End Function


    //''' <summary> 
    //''' Returns a Boolean filled with data from the execution of the given Command. 
    //''' </summary> 
    //''' <param name="strStoreProcName">Stored Procedure Name</param> 
    //''' <param name="SPparmaCollection">Parameters Collections</param> 
    //''' <returns>Returns a Boolean filled with data from the execution of the given Command.</returns> 
    //Public Overloads Function GetAllRecs(ByVal strStoreProcName As String, ByVal SPparmaCollection As SqlParameter()) As Boolean
    //    '
    //    Dim sErrMsg As String = ""
    //    '
    //    Try
    //        ' Create Get Record  SQL Command
    //        Dim getRecCmd = clsDBUtilityMethods.GetCommand()
    //        getRecCmd.CommandText = strStoreProcName
    //        getRecCmd.CommandType = CommandType.StoredProcedure
    //        '

    //        ' fetch parameter from parameter collection.

    //        For Each aParameter As SqlParameter In SPparmaCollection
    //            If Not aParameter Is Nothing Then

    //                getRecCmd.Parameters.Add(aParameter)
    //            End If
    //        Next


    //        Dim retValue As Object = clsDBUtilityMethods.ExecuteScalar(getRecCmd, sErrMsg)

    //        If Not retValue Is Nothing Then

    //            Return True

    //        Else
    //            Return False
    //        End If

    //    Catch ex As Exception
    //        Return False
    //    Finally
    //        '
    //    End Try
    //    ''
    //End Function


    //''' <summary> 
    //''' Returns a Boolean filled with data from the execution of the given Command. 
    //''' </summary> 
    //''' <param name="strStoreProcName">Stored Procedure Name</param> 
    //''' <param name="dt">DataTable</param> 
    //''' <returns>Returns a Boolean filled with data from the execution of the given Command.</returns>
    //Public Overloads Function GetRec(ByVal strStoreProcName As String, ByRef dt As DataTable) As Boolean
    //    '
    //    Dim sErrMsg As String = ""
    //    '
    //    Try
    //        ' Create Get Record  SQL Command
    //        Dim getRecCmd = clsDBUtilityMethods.GetCommand()
    //        getRecCmd.CommandText = strStoreProcName
    //        getRecCmd.CommandType = CommandType.StoredProcedure
    //        '

    //        ' fetch parameter from parameter collection.

    //        dt = clsDBUtilityMethods.GetTable(getRecCmd, sErrMsg)

    //        If Not dt Is Nothing Then
    //            Return True
    //        Else
    //            Return False
    //        End If

    //    Catch ex As Exception
    //        Return False
    //    Finally
    //        '
    //    End Try
    //    ''
    //End Function


    //''' <summary> 
    //''' Returns a Boolean filled with data from the execution of the given Command. 
    //''' </summary> 
    //''' <param name="strInsStoreProcName">Stored Procedure Name</param> 
    //''' <param name="SPparmaCollection">DataTable</param> 
    //''' <returns>Returns a HashTable that has result and command object filled with data from the execution of the given Command.</returns>
    //Public Overloads Function Insert(ByVal strInsStoreProcName As String, ByVal SPparmaCollection As SqlParameter()) As Hashtable
    //    '

    //    Dim sErrMsg As String = ""

    //    ' Create Insert SQL Command  
    //    Dim InsertCmd = clsDBUtilityMethods.GetCommand()
    //    InsertCmd.CommandText = strInsStoreProcName
    //    InsertCmd.CommandType = CommandType.StoredProcedure
    //    Dim result As Integer = 0
    //    Dim ht As Hashtable = New Hashtable()
    //    '

    //    ' fetch parameter from parameter collection.
    //    Try
    //        For Each aParameter As SqlParameter In SPparmaCollection
    //            If Not aParameter Is Nothing Then


    //                InsertCmd.Parameters.Add(aParameter)
    //            End If
    //        Next

    //        ''''''''''''''''''''''''''''''''''''''''''''''''''''
    //        result = clsDBUtilityMethods.ExecuteNonQuery(InsertCmd, sErrMsg)
    //        ht.Add("result", result)
    //        ht.Add("comObj", InsertCmd)

    //    Catch ex As Exception

    //        ht.Clear()

    //    End Try

    //    Return ht
    //End Function


    //''' <summary> 
    //''' Returns a Boolean filled with data from the execution of the given Command. 
    //''' </summary> 
    //''' <param name="strUptStoreProcName">Stored Procedure Name</param> 
    //''' <param name="SPparmaCollection">DataTable</param> 
    //''' <returns>Returns a HashTable that has result and command object filled with data from the execution of the given Command.</returns>
    //Public Function Update(ByVal strUptStoreProcName As String, ByVal SPparmaCollection As SqlParameter()) As Hashtable
    //    '
    //    Dim sErrMsg As String = ""
    //    Dim result As Integer = 0
    //    Dim ht As Hashtable = New Hashtable()
    //    Dim UpdateCmd = clsDBUtilityMethods.GetCommand()
    //    UpdateCmd.CommandText = strUptStoreProcName
    //    UpdateCmd.CommandType = CommandType.StoredProcedure
    //    '
    //    '
    //    Try

    //        ' fetch parameter from parameter collection.

    //        For Each aParameter As SqlParameter In SPparmaCollection

    //            If Not aParameter Is Nothing Then
    //                UpdateCmd.Parameters.Add(aParameter)
    //            End If

    //        Next

    //        '
    //        result = clsDBUtilityMethods.ExecuteNonQuery(UpdateCmd, sErrMsg)
    //        ht.Add("result", result)
    //        ht.Add("comObj", UpdateCmd)
    //        Return ht
    //        '
    //    Catch ex As Exception
    //        ht.Clear()
    //        Return Nothing
    //    Finally
    //        '
    //    End Try
    //    ''
    //End Function


    //''' <summary> 
    //''' Returns a Boolean filled with data from the execution of the given Command. 
    //''' </summary> 
    //''' <param name="strDelStoreProcName">Stored Procedure Name</param> 
    //''' <param name="SPparmaCollection">DataTable</param> 
    //''' <returns>Returns a HashTable that has result and command object filled with data from the execution of the given Command.</returns>
    //Public Function Delete(ByVal strDelStoreProcName As String, ByVal SPparmaCollection As SqlParameter()) As Hashtable
    //    '
    //    Dim sErrMsg As String = ""
    //    Dim result As Integer = 0
    //    Dim ht As Hashtable = New Hashtable()
    //    Dim DeleteCmd = clsDBUtilityMethods.GetCommand()
    //    DeleteCmd.CommandText = strDelStoreProcName
    //    DeleteCmd.CommandType = CommandType.StoredProcedure

    //    Try
    //        '
    //        ' fetch parameter from parameter collection.

    //        For Each aParameter As SqlParameter In SPparmaCollection

    //            If Not aParameter Is Nothing Then
    //                DeleteCmd.Parameters.Add(aParameter)
    //            End If

    //        Next

    //        '
    //        '
    //        result = clsDBUtilityMethods.ExecuteNonQuery(DeleteCmd, sErrMsg)
    //        ht.Add("result", result)
    //        ht.Add("comObj", DeleteCmd)
    //        Return ht
    //        '
    //    Catch ex As Exception
    //        ht.Clear()
    //        Return Nothing
    //    Finally
    //        '
    //    End Try

    //End Function
        #endregion Methods For Database Operation
    }
}
