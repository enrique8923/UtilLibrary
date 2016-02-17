using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilLibrary;

namespace UnitTestUtil
{

    
    [TestClass]
    public class JobLoggerTest
    {
       [TestMethod]
        public void Add_ErrorLog_ToDatabase()
        {

            JobLogger jobLogger = new JobLogger(false, false, true);
            bool result = jobLogger.LogMessage("Testing error message", JobLogger.LogType.Error);
            bool expected = true;

            Assert.AreEqual(expected, result , "Function not executed correctly");
        }

        [TestMethod]
        public void Add_WarningLog_ToDatabase()
        {
            JobLogger jobLogger = new JobLogger(false, false, true);
            bool result = jobLogger.LogMessage("Testing warning message", JobLogger.LogType.Warning);
            bool expected = true;

            Assert.AreEqual(expected, result, "Function not executed correctly");
        }

        [TestMethod]
        public void Add_MessageLog_ToDatabase()
        {
            JobLogger jobLogger = new JobLogger(false, false, true);
            bool result = jobLogger.LogMessage("Testing message", JobLogger.LogType.Message);
            bool expected = true;

            Assert.AreEqual(expected, result, "Function not executed correctly");
        }

        [TestMethod]
        public void Add_ErrorLog_ToFile()
        {
            JobLogger jobLogger = new JobLogger(true, false, false);
            bool result = jobLogger.LogMessage("Testing Error message", JobLogger.LogType.Error);
            bool expected = true;

            Assert.AreEqual(expected, result, "Function not executed correctly");
        }

        [TestMethod]
        public void Add_WarningLog_ToFile()
        {
            JobLogger jobLogger = new JobLogger(true, false, false);
            bool result = jobLogger.LogMessage("Testing Warning message", JobLogger.LogType.Warning);
            bool expected = true;

            Assert.AreEqual(expected, result, "Function not executed correctly");
        }

        [TestMethod]
        public void Add_ErrorLog_ToConsole()
        {
            JobLogger jobLogger = new JobLogger(false, true, false);
            bool result = jobLogger.LogMessage("Testing Error message", JobLogger.LogType.Message);
            bool expected = true;

            Assert.AreEqual(expected, result, "Function not executed correctly");
        }


    }

}
