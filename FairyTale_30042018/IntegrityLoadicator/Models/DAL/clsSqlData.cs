using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Xml.XPath;
using System.Data.SqlClient;
using System.Configuration;

namespace ZebecLoadMaster.Models.DAL
{
    public class clsSqlData
    {
       
        static string ServerName, ConString;
        public static string Mode = "";
        public static string StabilityType = "";
        public static bool ConnectionError=false;
        public static int calculationResult;
    
        public static string GetConnectionString()
        {
            //Reading Sql connection string(Server name) from XML file
            ReadValuefromXml();
            //ServerName = ConfigurationManager.AppSettings.Get("ServerName");
            ConString = "Data Source=" + ServerName + ";Initial Catalog=FairyTale_Stability;Integrated Security=SSPI";
            return ConString;
        }
        public static DbConnection GetConnection()
        {
            try
            {
                DbProviderFactory Dbfactory = clsSqlData.GetDbFactory();
                DbConnection conn = Dbfactory.CreateConnection();
                conn.ConnectionString = clsSqlData.GetConnectionString();
                return conn;
            }
            catch
            {
                throw new Exception("An exception has occured while creating the connection. Please check Connection String settings in the web.config file.");
            }
           
        }

         public static DbProviderFactory  GetDbFactory()
         {  
             try
             {
             String  ProviderName=ConfigurationManager.AppSettings.Get("ProviderName");
             DbProviderFactory Dbfactory =DbProviderFactories.GetFactory(ProviderName);
             return Dbfactory;
             }
                      
             catch(DbException generatedExceptionName)
             { 
                 throw new Exception("An exception has occured while creating the database provider factory. Please check the ProviderName specified in the app.config file."+generatedExceptionName.Message);
             }
                 
          
         }
         public static void ReadValuefromXml()
         {
             try
             {
                 string path = System.IO.Directory.GetCurrentDirectory() + "\\Settings\\StabilityConfig.xml";
                 XPathDocument document = new XPathDocument(path);
                 XPathNavigator navigator = document.CreateNavigator();
                 XPathNodeIterator node = navigator.Select("/Settings/ServerName");
                 foreach (XPathNavigator item in node)
                 {
                     ServerName = item.Value;
                     //GServerName = ServerName;
                     MainWindow._servername = item.Value;
                 }
             }
             catch
             {
             }
         }
     
     
      
    }



}