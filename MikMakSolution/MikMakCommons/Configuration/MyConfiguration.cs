using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MikMak.Configuration
{
    /// <summary>
    /// See : http://www.olivettom.com/?p=481
    /// </summary>
    public static class MyConfiguration
    {
        public static bool GetBool(string myField, bool defaultValue)
        {
            bool result;
            string value = ConfigurationManager.AppSettings[myField];
            if (value == null || !bool.TryParse(value, out result))
            {
                return defaultValue;
            }
            return result;
        }

        public static string GetString(string myField, string defaultValue)
        {
            string value = ConfigurationManager.AppSettings[myField];
            if (value == null)
            {
                return defaultValue;
            }
            return value;
        }

        public static int GetInt(string myField, int defaultValue)
        {
            int result;
            string value = ConfigurationManager.AppSettings[myField];
            if (value == null || !Int32.TryParse(value, out result))
            {
                return defaultValue;
            }
            return result;
        }
    }
}
