// -----------------------------------------------------------------------
// <copyright file="Country.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MTO.Practices.MaxMindExportDataBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Country
    {
        private string code;
        private string name;

        public Country(string code, string name)
        {
            this.code = code;
            this.name = name;
        }

        public string getCode()
        {
            return this.code;
        }

        public string getName()
        {
            return this.name;
        }
    }

    public class DatabaseInfo
    {
        public static int COUNTRY_EDITION = 1;
        public static int REGION_EDITION_REV0 = 7;
        public static int REGION_EDITION_REV1 = 3;
        public static int CITY_EDITION_REV0 = 6;
        public static int CITY_EDITION_REV1 = 2;
        public static int ORG_EDITION = 5;
        public static int ISP_EDITION = 4;
        public static int PROXY_EDITION = 8;
        public static int ASNUM_EDITION = 9;
        public static int NETSPEED_EDITION = 10;
        private string info;

        static DatabaseInfo()
        {
        }

        public DatabaseInfo(string info)
        {
            this.info = info;
        }

        public int getType()
        {
            if (this.info == null | this.info == "")
                return DatabaseInfo.COUNTRY_EDITION;
            else
                return Convert.ToInt32(this.info.Substring(4, 7)) - 105;
        }

        public bool isPremium()
        {
            return this.info.IndexOf("FREE") < 0;
        }

        public DateTime getDate()
        {
            for (int index = 0; index < this.info.Length - 9; ++index)
            {
                if (char.IsWhiteSpace(this.info[index]))
                {
                    string s = this.info.Substring(index + 1, index + 9);
                    try
                    {
                        return DateTime.ParseExact(s, "yyyyMMdd", (IFormatProvider)null);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        break;
                    }
                }
            }
            return DateTime.Now;
        }

        public string toString()
        {
            return this.info;
        }
    }

    public class Location
    {
        private static double EARTH_DIAMETER = 12756.4;
        private static double PI = 3.14159265;
        private static double RAD_CONVERT = Location.PI / 180.0;
        public string countryCode;
        public string countryName;
        public string region;
        public string city;
        public string postalCode;
        public double latitude;
        public double longitude;
        public int dma_code;
        public int area_code;

        static Location()
        {
        }

        public double distance(Location loc)
        {
            double num1 = this.latitude;
            double num2 = this.longitude;
            double num3 = loc.latitude;
            double num4 = loc.longitude;
            double d1 = num1 * Location.RAD_CONVERT;
            double d2 = num3 * Location.RAD_CONVERT;
            double num5 = d2 - d1;
            double num6 = (num4 - num2) * Location.RAD_CONVERT;
            double d3 = Math.Pow(Math.Sin(num5 / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num6 / 2.0), 2.0);
            return Location.EARTH_DIAMETER * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3));
        }
    }
}
