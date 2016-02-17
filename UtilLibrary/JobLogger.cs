using System;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace UtilLibrary
{
    public class JobLogger
    {

        private bool logToFile, logToConsole, logToDatabase, logMessage, logWarning, logError;

        public enum LogType
        {
            Message,
            Warning,
            Error
        };

        public JobLogger(bool logToFile, bool logToConsole, bool logToDatabase)
        {

            this.logToDatabase = logToDatabase;
            this.logToFile = logToFile;
            this.logToConsole = logToConsole;

        }


        public bool LogMessage(string description, LogType logType)// bool isMessage, bool isWarning, bool isError)
        {
            description.Trim();
            bool success = false;

            if (String.IsNullOrEmpty(description))
            {
                //return;
                throw new Exception("A Description must be specified");
            }

            if (!logToConsole && !logToFile && !logToDatabase)
            {
                throw new Exception("Invalid configuration");
            }

            if (!Enum.IsDefined(typeof(LogType), logType))
            {
                throw new Exception("Error or Warning or Message must be specified");
            }

            int type = 0;

            switch (logType)
            {
                case LogType.Message:
                    type = 1;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogType.Warning:
                    type = 2;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogType.Error:
                    type = 3;
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }


            if (logToDatabase)
            {
                if (SaveToDatabase(description, type))
                    success = true;
            }

            if (logToFile)
            {
                if (SaveToFile(description))
                    success = true;
            }

            if (logToConsole)
            {
                SaveToConsole(description);
                success = true;
            }

            return success;

        }




        private bool SaveToDatabase(string description, int t)
        {
            
            try
            {
                string myConnectionString = GetSettingValue("ConnectionString");

                string query = "insert into Log values('" + description + "', " + t.ToString() + ")";
                int affectedRows;
                using (SqlConnection sqlConn = new SqlConnection(myConnectionString))
                using (SqlCommand insertInfo = new SqlCommand(query, sqlConn))
                {
                    sqlConn.Open();
                    affectedRows = insertInfo.ExecuteNonQuery();
                }

                if (affectedRows == 0)
                {
                    return false;
                }
                else {
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public bool SaveToFile(string description)
        {

            string myValue = GetSettingValue("LogFileDirectory");

            if (!String.IsNullOrEmpty(myValue))
            {

                string log = DateTime.Now.ToShortDateString() + ", " + description;
                string filePath = myValue + "LogFile" + DateTime.Now.ToShortDateString().Replace(@"/", "") + ".txt";

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(log);
                }

                return true;
            }
            else { return false; }

        }





        public void SaveToConsole(string description)
        {
            Console.WriteLine(DateTime.Now.ToShortDateString() + ", " + description);
        }


        string GetAppSetting(Configuration config, string key)
        {
            KeyValueConfigurationElement element = config.AppSettings.Settings[key];
            if (element != null)
            {
                string value = element.Value;
                if (!string.IsNullOrEmpty(value))
                    return value;
            }
            return string.Empty;
        }


        string GetSettingValue(string key)
        {

            Configuration config = null;
            string exeConfigPath = this.GetType().Assembly.Location;
            try
            {
                config = ConfigurationManager.OpenExeConfiguration(exeConfigPath);

                if (config != null)
                {
                    string myValue = GetAppSetting(config, key);
                    return myValue;

                }
                else { return ""; }

            }
            catch (Exception ex)
            {
                return "";
            }

        }




    }
}
