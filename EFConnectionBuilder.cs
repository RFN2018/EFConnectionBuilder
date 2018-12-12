using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.EntityClient;
using System.Configuration;

namespace DaemonAnalytics.Util
{
    class EFConnectionBuilder
    {
        internal ConnectionStringsSection csSection;
        internal Configuration config;
        internal AppSettingsSection asSection;

        public EFConnectionBuilder()
        {
            // Get the application configuration file.
            config =
                ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.None);

            // Get the conectionStrings section.
            csSection =
              config.ConnectionStrings;

            asSection = config.AppSettings;
        }

        public void EncryptData()
        {
            asSection.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
            csSection.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        public void ClearConnectionStrings()
        {
            //Clear the connection
            csSection.ConnectionStrings.Clear();
        }
        public void AddConnectionString(string name,  string connectionstring)
        {                         
            //Create your connection string into a connectionStringSettings object
            ConnectionStringSettings connection = new ConnectionStringSettings(name, connectionstring, "System.Data.EntityClient");

            //Add the object to the configuration
            csSection.ConnectionStrings.Add(connection);

            //Save the configuration
            config.Save(ConfigurationSaveMode.Modified);

            //Refresh the Section
            ConfigurationManager.RefreshSection("connectionStrings");
        }
        public string GetOracleConnString(string metadata, string database, string username, string password)
        {
            EntityConnectionStringBuilder conn = new EntityConnectionStringBuilder()
            {
                Metadata = metadata,
                Provider = "Oracle.ManagedDataAccess.Client",
                ProviderConnectionString = "DATA SOURCE = " + database + "; PASSWORD = " + password + "; USER ID =" + username
            };
            return conn.ConnectionString;
        }

        public string GetSqlServerConnString(string metadata, string database, string calalog, string username, string password)
        {
            EntityConnectionStringBuilder conn = new EntityConnectionStringBuilder()
            {
                Metadata = metadata,
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = "data source=" + database + ";initial catalog=" + calalog + "; user id=" + username + ";password=" + password + ";MultipleActiveResultSets=True;App=EntityFramework"
            };
            return conn.ConnectionString;
        }
    }

}    
    

