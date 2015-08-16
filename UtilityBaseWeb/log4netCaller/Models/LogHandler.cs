using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using log4net.Config;

namespace log4netPlay.Models
{
    public class LogHandler
    {
        private static LogHandler _instance = null;

        public static LogHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LogHandler();
            }

            return _instance;
        }

        public void WriteLog(Guid guid, string ip, string info, string desc, string exception, string type, Dictionary<string, string> properties)
        {
            //this.WriteFileLog(guid, ip, info, desc, exception, type, properties);
            this.WriteDBLog(guid, ip, info, desc, exception, type, properties);
        }
        private void WriteFileLog(Guid guid, string ip, string info, string desc, string exception, string type, Dictionary<string, string> properties)
        {
            log4net.ThreadContext.Properties["GUID"] = guid;
            log4net.ThreadContext.Properties["ServerIP"] = GetServerIP();
            log4net.ThreadContext.Properties["ClientIP"] = ip;
            log4net.ThreadContext.Properties["System"] = info;
            log4net.ThreadContext.Properties["Description"] = desc;
            log4net.ThreadContext.Properties["Exception"] = exception;
            log4net.ThreadContext.Properties["DateTime"] = DateTime.Now;
            log4net.ThreadContext.Properties["Properties"] = ""; // Json.serilize(properties);

            string logMsg = string.Empty;
            foreach (KeyValuePair<string, string> pair in properties)
            {
                logMsg += pair.Key + ": " + pair.Value + System.Environment.NewLine;
            }

            ILog rolling = LogManager.GetLogger("Rolling"); // logger

            switch (type)
            {
                case "Info":
                    rolling.Info(logMsg);
                    break;
                case "Error":
                    rolling.Error(logMsg);
                    break;
            }
        }
        private void WriteDBLog(Guid guid, string ip, string info, string desc, string exception, string type, Dictionary<string, string> properties)
        {
            log4net.ThreadContext.Properties["GUID"] = guid;
            log4net.ThreadContext.Properties["ServerIP"] = GetServerIP();
            log4net.ThreadContext.Properties["ClientIP"] = ip;
            log4net.ThreadContext.Properties["System"] = info;
            log4net.ThreadContext.Properties["Description"] = desc;
            log4net.ThreadContext.Properties["Exception"] = exception;
            log4net.ThreadContext.Properties["DateTime"] = DateTime.Now;
            log4net.ThreadContext.Properties["Properties"] = ""; // Json.serilize(properties);
            log4net.ThreadContext.Properties["MessageId"] = 1;
            log4net.ThreadContext.Properties["Message"] = "Hey ~ Yo ~";

            string logMsg = string.Empty;
            foreach (KeyValuePair<string, string> pair in properties)
            {
                logMsg += pair.Key + ": " + pair.Value + System.Environment.NewLine;
            }

            ILog db = LogManager.GetLogger("DB");

            switch (type)
            {
                case "Info":
                    db.Info("");
                    break;
                case "Error":
                    db.Error("");
                    break;
            }
        }

        public static string GetServerIP()
        {
            string result = string.Empty;

            // Web Server
            if (HttpContext.Current != null)
            {
                result = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            }

            // WCF Server
            // class System.ServiceModel.OperationContext
            //if (OperationContext.Current != null)
            //{
            //    result = ((RemoteEndpointMessageProperty)OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name]).Address;
            //}

            return result;
        }
    }
}