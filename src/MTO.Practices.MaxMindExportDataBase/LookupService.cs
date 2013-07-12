// -----------------------------------------------------------------------
// <copyright file="LookupService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MTO.Practices.MaxMindExportDataBase
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LookupService
    {

        #region propriedades
        private static Country UNKNOWN_COUNTRY = new Country("--", "N/A");
        private static int COUNTRY_BEGIN = 16776960;
        private static int STATE_BEGIN = 16700000;
        private static int STRUCTURE_INFO_MAX_SIZE = 20;
        private static int DATABASE_INFO_MAX_SIZE = 100;
        private static int FULL_RECORD_LENGTH = 100;
        private static int SEGMENT_RECORD_LENGTH = 3;
        private static int STANDARD_RECORD_LENGTH = 3;
        private static int ORG_RECORD_LENGTH = 4;
        private static int MAX_RECORD_LENGTH = 4;
        private static int MAX_ORG_RECORD_LENGTH = 1000;
        private static int FIPS_RANGE = 360;
        private static int STATE_BEGIN_REV0 = 16700000;
        private static int STATE_BEGIN_REV1 = 16000000;
        private static int US_OFFSET = 1;
        private static int CANADA_OFFSET = 677;
        private static int WORLD_OFFSET = 1353;
        public static int GEOIP_MEMORY_CACHE = 1;
        public static int GEOIP_DIALUP_SPEED = 1;
        public static int GEOIP_CABLEDSL_SPEED = 2;
        public static int GEOIP_CORPORATE_SPEED = 3;
        private static string[] countryCode = new string[253]
    {
      "--",
      "AP",
      "EU",
      "AD",
      "AE",
      "AF",
      "AG",
      "AI",
      "AL",
      "AM",
      "AN",
      "AO",
      "AQ",
      "AR",
      "AS",
      "AT",
      "AU",
      "AW",
      "AZ",
      "BA",
      "BB",
      "BD",
      "BE",
      "BF",
      "BG",
      "BH",
      "BI",
      "BJ",
      "BM",
      "BN",
      "BO",
      "BR",
      "BS",
      "BT",
      "BV",
      "BW",
      "BY",
      "BZ",
      "CA",
      "CC",
      "CD",
      "CF",
      "CG",
      "CH",
      "CI",
      "CK",
      "CL",
      "CM",
      "CN",
      "CO",
      "CR",
      "CU",
      "CV",
      "CX",
      "CY",
      "CZ",
      "DE",
      "DJ",
      "DK",
      "DM",
      "DO",
      "DZ",
      "EC",
      "EE",
      "EG",
      "EH",
      "ER",
      "ES",
      "ET",
      "FI",
      "FJ",
      "FK",
      "FM",
      "FO",
      "FR",
      "FX",
      "GA",
      "GB",
      "GD",
      "GE",
      "GF",
      "GH",
      "GI",
      "GL",
      "GM",
      "GN",
      "GP",
      "GQ",
      "GR",
      "GS",
      "GT",
      "GU",
      "GW",
      "GY",
      "HK",
      "HM",
      "HN",
      "HR",
      "HT",
      "HU",
      "ID",
      "IE",
      "IL",
      "IN",
      "IO",
      "IQ",
      "IR",
      "IS",
      "IT",
      "JM",
      "JO",
      "JP",
      "KE",
      "KG",
      "KH",
      "KI",
      "KM",
      "KN",
      "KP",
      "KR",
      "KW",
      "KY",
      "KZ",
      "LA",
      "LB",
      "LC",
      "LI",
      "LK",
      "LR",
      "LS",
      "LT",
      "LU",
      "LV",
      "LY",
      "MA",
      "MC",
      "MD",
      "MG",
      "MH",
      "MK",
      "ML",
      "MM",
      "MN",
      "MO",
      "MP",
      "MQ",
      "MR",
      "MS",
      "MT",
      "MU",
      "MV",
      "MW",
      "MX",
      "MY",
      "MZ",
      "NA",
      "NC",
      "NE",
      "NF",
      "NG",
      "NI",
      "NL",
      "NO",
      "NP",
      "NR",
      "NU",
      "NZ",
      "OM",
      "PA",
      "PE",
      "PF",
      "PG",
      "PH",
      "PK",
      "PL",
      "PM",
      "PN",
      "PR",
      "PS",
      "PT",
      "PW",
      "PY",
      "QA",
      "RE",
      "RO",
      "RU",
      "RW",
      "SA",
      "SB",
      "SC",
      "SD",
      "SE",
      "SG",
      "SH",
      "SI",
      "SJ",
      "SK",
      "SL",
      "SM",
      "SN",
      "SO",
      "SR",
      "ST",
      "SV",
      "SY",
      "SZ",
      "TC",
      "TD",
      "TF",
      "TG",
      "TH",
      "TJ",
      "TK",
      "TM",
      "TN",
      "TO",
      "TL",
      "TR",
      "TT",
      "TV",
      "TW",
      "TZ",
      "UA",
      "UG",
      "UM",
      "US",
      "UY",
      "UZ",
      "VA",
      "VC",
      "VE",
      "VG",
      "VI",
      "VN",
      "VU",
      "WF",
      "WS",
      "YE",
      "YT",
      "RS",
      "ZA",
      "ZM",
      "ME",
      "ZW",
      "A1",
      "A2",
      "O1",
      "AX",
      "GG",
      "IM",
      "JE",
      "BL",
      "MF"
    };
        private static string[] countryName = new string[253]
    {
      "N/A",
      "Asia/Pacific Region",
      "Europe",
      "Andorra",
      "United Arab Emirates",
      "Afghanistan",
      "Antigua and Barbuda",
      "Anguilla",
      "Albania",
      "Armenia",
      "Netherlands Antilles",
      "Angola",
      "Antarctica",
      "Argentina",
      "American Samoa",
      "Austria",
      "Australia",
      "Aruba",
      "Azerbaijan",
      "Bosnia and Herzegovina",
      "Barbados",
      "Bangladesh",
      "Belgium",
      "Burkina Faso",
      "Bulgaria",
      "Bahrain",
      "Burundi",
      "Benin",
      "Bermuda",
      "Brunei Darussalam",
      "Bolivia",
      "Brazil",
      "Bahamas",
      "Bhutan",
      "Bouvet Island",
      "Botswana",
      "Belarus",
      "Belize",
      "Canada",
      "Cocos (Keeling) Islands",
      "Congo, The Democratic Republic of the",
      "Central African Republic",
      "Congo",
      "Switzerland",
      "Cote D'Ivoire",
      "Cook Islands",
      "Chile",
      "Cameroon",
      "China",
      "Colombia",
      "Costa Rica",
      "Cuba",
      "Cape Verde",
      "Christmas Island",
      "Cyprus",
      "Czech Republic",
      "Germany",
      "Djibouti",
      "Denmark",
      "Dominica",
      "Dominican Republic",
      "Algeria",
      "Ecuador",
      "Estonia",
      "Egypt",
      "Western Sahara",
      "Eritrea",
      "Spain",
      "Ethiopia",
      "Finland",
      "Fiji",
      "Falkland Islands (Malvinas)",
      "Micronesia, Federated States of",
      "Faroe Islands",
      "France",
      "France, Metropolitan",
      "Gabon",
      "United Kingdom",
      "Grenada",
      "Georgia",
      "French Guiana",
      "Ghana",
      "Gibraltar",
      "Greenland",
      "Gambia",
      "Guinea",
      "Guadeloupe",
      "Equatorial Guinea",
      "Greece",
      "South Georgia and the South Sandwich Islands",
      "Guatemala",
      "Guam",
      "Guinea-Bissau",
      "Guyana",
      "Hong Kong",
      "Heard Island and McDonald Islands",
      "Honduras",
      "Croatia",
      "Haiti",
      "Hungary",
      "Indonesia",
      "Ireland",
      "Israel",
      "India",
      "British Indian Ocean Territory",
      "Iraq",
      "Iran, Islamic Republic of",
      "Iceland",
      "Italy",
      "Jamaica",
      "Jordan",
      "Japan",
      "Kenya",
      "Kyrgyzstan",
      "Cambodia",
      "Kiribati",
      "Comoros",
      "Saint Kitts and Nevis",
      "Korea, Democratic People's Republic of",
      "Korea, Republic of",
      "Kuwait",
      "Cayman Islands",
      "Kazakstan",
      "Lao People's Democratic Republic",
      "Lebanon",
      "Saint Lucia",
      "Liechtenstein",
      "Sri Lanka",
      "Liberia",
      "Lesotho",
      "Lithuania",
      "Luxembourg",
      "Latvia",
      "Libyan Arab Jamahiriya",
      "Morocco",
      "Monaco",
      "Moldova, Republic of",
      "Madagascar",
      "Marshall Islands",
      "Macedonia, the Former Yugoslav Republic of",
      "Mali",
      "Myanmar",
      "Mongolia",
      "Macau",
      "Northern Mariana Islands",
      "Martinique",
      "Mauritania",
      "Montserrat",
      "Malta",
      "Mauritius",
      "Maldives",
      "Malawi",
      "Mexico",
      "Malaysia",
      "Mozambique",
      "Namibia",
      "New Caledonia",
      "Niger",
      "Norfolk Island",
      "Nigeria",
      "Nicaragua",
      "Netherlands",
      "Norway",
      "Nepal",
      "Nauru",
      "Niue",
      "New Zealand",
      "Oman",
      "Panama",
      "Peru",
      "French Polynesia",
      "Papua New Guinea",
      "Philippines",
      "Pakistan",
      "Poland",
      "Saint Pierre and Miquelon",
      "Pitcairn",
      "Puerto Rico",
      "Palestinian Territory, Occupied",
      "Portugal",
      "Palau",
      "Paraguay",
      "Qatar",
      "Reunion",
      "Romania",
      "Russian Federation",
      "Rwanda",
      "Saudi Arabia",
      "Solomon Islands",
      "Seychelles",
      "Sudan",
      "Sweden",
      "Singapore",
      "Saint Helena",
      "Slovenia",
      "Svalbard and Jan Mayen",
      "Slovakia",
      "Sierra Leone",
      "San Marino",
      "Senegal",
      "Somalia",
      "Suriname",
      "Sao Tome and Principe",
      "El Salvador",
      "Syrian Arab Republic",
      "Swaziland",
      "Turks and Caicos Islands",
      "Chad",
      "French Southern Territories",
      "Togo",
      "Thailand",
      "Tajikistan",
      "Tokelau",
      "Turkmenistan",
      "Tunisia",
      "Tonga",
      "Timor-Leste",
      "Turkey",
      "Trinidad and Tobago",
      "Tuvalu",
      "Taiwan",
      "Tanzania, United Republic of",
      "Ukraine",
      "Uganda",
      "United States Minor Outlying Islands",
      "United States",
      "Uruguay",
      "Uzbekistan",
      "Holy See (Vatican City State)",
      "Saint Vincent and the Grenadines",
      "Venezuela",
      "Virgin Islands, British",
      "Virgin Islands, U.S.",
      "Vietnam",
      "Vanuatu",
      "Wallis and Futuna",
      "Samoa",
      "Yemen",
      "Mayotte",
      "Serbia",
      "South Africa",
      "Zambia",
      "Montenegro",
      "Zimbabwe",
      "Anonymous Proxy",
      "Satellite Provider",
      "Other",
      "Aland Islands",
      "Guernsey",
      "Isle of Man",
      "Jersey",
      "Saint Barthelemy",
      "Saint Martin"
    };
        private byte databaseType = Convert.ToByte(DatabaseInfo.COUNTRY_EDITION);
        private FileStream file;
        private DatabaseInfo databaseInfo;
        private int[] databaseSegments;
        private int recordLength;
        private int dboptions;
        private byte[] dbbuffer;
        private string licenseKey;
        private int dnsService;
        public static int GEOIP_STANDARD;
        public static int GEOIP_UNKNOWN_SPEED;
        #endregion

        static LookupService()
        {
        }

        public LookupService(string databaseFile, int options)
        {
            try
            {
                this.file = new FileStream(databaseFile, FileMode.Open, FileAccess.Read);
                this.dboptions = options;
                this.init();
            }
            catch (SystemException ex)
            {
                Console.Write("cannot open file " + databaseFile + "\n");
            }
        }

        public LookupService(string databaseFile)
            : this(databaseFile, LookupService.GEOIP_STANDARD)
        {
        }


        private void init()
        {
            byte[] buffer1 = new byte[3];
            byte[] buffer2 = new byte[LookupService.SEGMENT_RECORD_LENGTH];
            this.databaseType = (byte)DatabaseInfo.COUNTRY_EDITION;
            this.recordLength = LookupService.STANDARD_RECORD_LENGTH;
            this.file.Seek(-3L, SeekOrigin.End);
            for (int index1 = 0; index1 < LookupService.STRUCTURE_INFO_MAX_SIZE; ++index1)
            {
                this.file.Read(buffer1, 0, 3);
                if ((int)buffer1[0] == (int)byte.MaxValue && (int)buffer1[1] == (int)byte.MaxValue && (int)buffer1[2] == (int)byte.MaxValue)
                {
                    this.databaseType = Convert.ToByte(this.file.ReadByte());
                    if ((int)this.databaseType >= 106)
                        this.databaseType -= (byte)105;
                    if ((int)this.databaseType == DatabaseInfo.REGION_EDITION_REV0)
                    {
                        this.databaseSegments = new int[1];
                        this.databaseSegments[0] = LookupService.STATE_BEGIN_REV0;
                        this.recordLength = LookupService.STANDARD_RECORD_LENGTH;
                        break;
                    }
                    else if ((int)this.databaseType == DatabaseInfo.REGION_EDITION_REV1)
                    {
                        this.databaseSegments = new int[1];
                        this.databaseSegments[0] = LookupService.STATE_BEGIN_REV1;
                        this.recordLength = LookupService.STANDARD_RECORD_LENGTH;
                        break;
                    }
                    else if ((int)this.databaseType == DatabaseInfo.CITY_EDITION_REV0 || (int)this.databaseType == DatabaseInfo.CITY_EDITION_REV1 || ((int)this.databaseType == DatabaseInfo.ORG_EDITION || (int)this.databaseType == DatabaseInfo.ISP_EDITION) || (int)this.databaseType == DatabaseInfo.ASNUM_EDITION)
                    {
                        this.databaseSegments = new int[1];
                        this.databaseSegments[0] = 0;
                        this.recordLength = (int)this.databaseType != DatabaseInfo.CITY_EDITION_REV0 && (int)this.databaseType != DatabaseInfo.CITY_EDITION_REV1 ? LookupService.ORG_RECORD_LENGTH : LookupService.STANDARD_RECORD_LENGTH;
                        this.file.Read(buffer2, 0, LookupService.SEGMENT_RECORD_LENGTH);
                        for (int index2 = 0; index2 < LookupService.SEGMENT_RECORD_LENGTH; ++index2)
                            this.databaseSegments[0] += LookupService.unsignedByteToInt(buffer2[index2]) << index2 * 8;
                        break;
                    }
                    else
                        break;
                }
                else
                    this.file.Seek(-4L, SeekOrigin.Current);
            }
            if ((int)this.databaseType == DatabaseInfo.COUNTRY_EDITION | (int)this.databaseType == DatabaseInfo.PROXY_EDITION | (int)this.databaseType == DatabaseInfo.NETSPEED_EDITION)
            {
                this.databaseSegments = new int[1];
                this.databaseSegments[0] = LookupService.COUNTRY_BEGIN;
                this.recordLength = LookupService.STANDARD_RECORD_LENGTH;
            }
            if ((this.dboptions & LookupService.GEOIP_MEMORY_CACHE) != 1)
                return;
            int count = (int)this.file.Length;
            this.dbbuffer = new byte[count];
            this.file.Seek(0L, SeekOrigin.Begin);
            this.file.Read(this.dbbuffer, 0, count);
        }

        public void close()
        {
            try
            {
                this.file.Close();
                this.file = (FileStream)null;
            }
            catch (Exception ex)
            {
            }
        }

        private static int unsignedByteToInt(byte b)
        {
            return (int)b & (int)byte.MaxValue;
        }

        private static long bytestoLong(byte[] address)
        {
            long num1 = 0L;
            for (int index = 0; index < 4; ++index)
            {
                long num2 = (long)address[index];
                if (num2 < 0L)
                    num2 += 256L;
                num1 += num2 << (3 - index) * 8;
            }
            return num1;
        }

        public Location getLocation(string str)
        {
            IPAddress ipAddress;
            try
            {
                ipAddress = IPAddress.Parse(str);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return (Location)null;
            }
            return this.getLocation(LookupService.bytestoLong(ipAddress.GetAddressBytes()));
        }

        public Location getLocation(long ipnum)
        {
            byte[] buffer = new byte[FULL_RECORD_LENGTH];
            char[] chArray = new char[FULL_RECORD_LENGTH];
            int num1 = 0;
            Location location = new Location();
            int length1 = 0;
            double num2 = 0.0;
            double num3 = 0.0;
            try
            {
                int num4 = this.SeekCountry(ipnum);
                if (num4 == this.databaseSegments[0])
                    return (Location)null;
                int num5 = num4 + (2 * this.recordLength - 1) * this.databaseSegments[0];
                if ((this.dboptions & LookupService.GEOIP_MEMORY_CACHE) == 1)
                {
                    for (int index = 0; index < FULL_RECORD_LENGTH; ++index)
                        buffer[index] = this.dbbuffer[index + num5];
                }
                else
                {
                    this.file.Seek((long)num5, SeekOrigin.Begin);
                    this.file.Read(buffer, 0, FULL_RECORD_LENGTH);
                }
                for (int index = 0; index < FULL_RECORD_LENGTH; ++index)
                    chArray[index] = Convert.ToChar(buffer[index]);
                location.countryCode = countryCode[unsignedByteToInt(buffer[0])];
                location.countryName = countryName[unsignedByteToInt(buffer[0])];
                int startIndex1 = num1 + 1;
                while ((int)buffer[startIndex1 + length1] != 0)
                    ++length1;
                if (length1 > 0)
                    location.region = new string(chArray, startIndex1, length1);
                int startIndex2 = startIndex1 + (length1 + 1);
                int length2 = 0;
                while ((int)buffer[startIndex2 + length2] != 0)
                    ++length2;
                if (length2 > 0)
                    location.city = new string(chArray, startIndex2, length2);
                int startIndex3 = startIndex2 + (length2 + 1);
                int length3 = 0;
                while ((int)buffer[startIndex3 + length3] != 0)
                    ++length3;
                if (length3 > 0)
                    location.postalCode = new string(chArray, startIndex3, length3);
                int num6 = startIndex3 + (length3 + 1);
                for (int index = 0; index < 3; ++index)
                    num2 += (double)(unsignedByteToInt(buffer[num6 + index]) << index * 8);
                location.latitude = num2 / 10000.0 - 180.0;
                int num7 = num6 + 3;
                for (int index = 0; index < 3; ++index)
                    num3 += (double)(unsignedByteToInt(buffer[num7 + index]) << index * 8);
                location.longitude = num3 / 10000.0 - 180.0;
                location.dma_code = 0;
                location.area_code = 0;
                if ((int)this.databaseType == DatabaseInfo.CITY_EDITION_REV1)
                {
                    int num8 = 0;
                    if (location.countryCode == "US")
                    {
                        int num9 = num7 + 3;
                        for (int index = 0; index < 3; ++index)
                            num8 += unsignedByteToInt(buffer[num9 + index]) << index * 8;
                        location.dma_code = num8 / 1000;
                        location.area_code = num8 % 1000;
                    }
                }
            }
            catch (IOException ex)
            {
                Console.Write("IO Exception while seting up segments");
            }
            return location;
        }

        private int SeekCountry(long ipAddress)
        {
            byte[] buffer = new byte[2 * MAX_RECORD_LENGTH];
            int[] numArray = new int[2];
            int num1 = 0;
            for (int index1 = 31; index1 >= 0; --index1)
            {
                try
                {
                    if ((this.dboptions & LookupService.GEOIP_MEMORY_CACHE) == 1)
                    {
                        for (int index2 = 0; index2 < 2 * MAX_RECORD_LENGTH; ++index2)
                            buffer[index2] = this.dbbuffer[index2 + 2 * this.recordLength * num1];
                    }
                    else
                    {
                        this.file.Seek((long)(2 * this.recordLength * num1), SeekOrigin.Begin);
                        this.file.Read(buffer, 0, 2 * MAX_RECORD_LENGTH);
                    }
                }
                catch (IOException ex)
                {
                    Console.Write("IO Exception");
                }
                for (int index2 = 0; index2 < 2; ++index2)
                {
                    numArray[index2] = 0;
                    for (int index3 = 0; index3 < this.recordLength; ++index3)
                    {
                        int num2 = (int)buffer[index2 * this.recordLength + index3];
                        if (num2 < 0)
                            num2 += 256;
                        numArray[index2] += num2 << index3 * 8;
                    }
                }
                if ((ipAddress & (long)(1 << index1)) > 0L)
                {
                    if (numArray[1] >= this.databaseSegments[0])
                        return numArray[1];
                    num1 = numArray[1];
                }
                else
                {
                    if (numArray[0] >= this.databaseSegments[0])
                        return numArray[0];
                    num1 = numArray[0];
                }
            }
            Console.Write("Error Seeking country while Seeking " + (object)ipAddress);
            return 0;
        }
    }
}
