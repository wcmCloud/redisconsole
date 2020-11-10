using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RedisConsoleDesktop.Core
{

    public static class AssemblyHelpers
    {

        public static Assembly GetFromFile(string filepath)
        {

            Assembly res = Assembly.LoadFile(filepath);
            return res;
        }


        /// <summary>
        /// Get the Assembly description info
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string AssDescription(this Assembly a)
        {
            AssemblyDescriptionAttribute asm = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(a, typeof(AssemblyDescriptionAttribute));
            return asm.Description;
        }

        /// <summary>
        /// Get the Assembly Copyright info
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string AssCopyright(this Assembly a)
        {
            AssemblyCopyrightAttribute asm = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(a, typeof(AssemblyCopyrightAttribute));
            return asm.Copyright;
        }

        /// <summary>
        /// Get the Assembly Company info
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string AssCompany(this Assembly a)
        {
            AssemblyCompanyAttribute asm = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(a, typeof(AssemblyCompanyAttribute));
            return asm.Company;
        }

        /// <summary>
        /// Get the Assembly Title info
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string AssTitle(this Assembly a)
        {
            AssemblyProductAttribute asm = (AssemblyProductAttribute)Attribute.GetCustomAttribute(a, typeof(AssemblyProductAttribute));
            return asm.Product;
        }

        /// <summary>
        /// Get the Assembly Trademark info
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string AssTrademark(this Assembly a)
        {
            AssemblyTrademarkAttribute asm = (AssemblyTrademarkAttribute)Attribute.GetCustomAttribute(a, typeof(AssemblyTrademarkAttribute));
            return asm.Trademark;
        }

        /// <summary>
        /// Get the Assembly major version
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int VersionMajor(this Assembly a)
        {
            return a.GetName().Version.Major;
        }
        /// <summary>
        /// Get the Assembly minor version
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int VersionMinor(this Assembly a)
        {
            return a.GetName().Version.Minor;
        }
        /// <summary>
        /// Get the Assembly build version
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int VersionBuild(this Assembly a)
        {
            return a.GetName().Version.Build;
        }

        /// <summary>
        /// Get the Assembly Revision version
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int VersionRevision(this Assembly a)
        {
            return a.GetName().Version.Revision;
        }

        /// <summary>
        /// Get the Assembly version string
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string Version(this Assembly a)
        {
            //return a.VersionMajor().ToString() + "." + a.VersionMinor().ToString() + "." + a.VersionBuild().ToString() + "." + a.VersionRevision().ToString();
            return a.VersionMajor().ToString() + "." + a.VersionMinor().ToString() + "." + a.VersionBuild().ToString();
        }

        /// <summary>
        /// Get the Assembly short version string
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string ShortVersion(this Assembly a)
        {
            return a.VersionMajor().ToString() + "." + a.VersionMinor().ToString() + "." + a.VersionBuild().ToString();
        }

        /// <summary>
        /// Returns true if the assembly is build in debug mode
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool IsDebug(this Assembly a)
        {
            bool res = false;
#if DEBUG
            res = true;
#endif
            return res;
        }

        /// <summary>
        /// Return a string with assembly information
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string AssemblyInfoStringShort(this Assembly a)
        {
            string res = "";

            res += a.AssTitle() + " v " + a.ShortVersion();
            if (a.IsDebug())
            {
                res += " Debug";
            }
            else
            {
                //res += " Release";
            }

            return res;
        }

        public static string AssemblyInfoString(this Assembly a)
        {
            string res = "";

            //res += a.AssTitle() + " v " + a.Version();
            res += " v " + a.Version();
            if (a.IsDebug())
            {
                res += " Debug";
            }
            else
            {
                //res += " Release";
            }

            return res;
        }


        /// <summary>
        /// Get assembly build date
        /// The most reliable method turns out to be retrieving the linker timestamp from the PE header embedded in the executable file 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }

        public static DateTime GetBuildDate(this Assembly assembly)
        {
            const string BuildVersionMetadataPrefix = "+build";

            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (attribute?.InformationalVersion != null)
            {
                var value = attribute.InformationalVersion;
                var index = value.IndexOf(BuildVersionMetadataPrefix);
                if (index > 0)
                {
                    value = value.Substring(index + BuildVersionMetadataPrefix.Length);
                    if (DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                    {
                        return result;
                    }
                }
            }

            return default;
        }


    }
}
