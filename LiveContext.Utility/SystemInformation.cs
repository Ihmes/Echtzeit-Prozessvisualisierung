using System;
using System.Management;

namespace LiveContext.Utility
{
    public static class SystemInformation
    {
        public static string DotNetVersion()
        {
            return Environment.Version.ToString();
        }

        public static string CpuName()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_Processor");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["Name"].ToString();
                }
            }
            catch {}
            return "";
        }

        public static string CpuNumberOfCores()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["NumberOfCores"].ToString();
                }
            }
            catch { }
            return "";
        }

        public static string CpuType()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["Description"].ToString();
                }
            }
            catch { }
            return "";
        }

        public static string CpuClockSpeed()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["MaxClockSpeed"].ToString();
                }
            }
            catch { }
            return "";
        }

        public static string TotalPhysicalMemory()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["TotalPhysicalMemory"].ToString();
                }
            }
            catch { }
            return "";
        }

        public static string FreePhysicalMemory()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["FreePhysicalMemory"].ToString();
                }
            }
            catch { }
            return "";
        }

        public static string WindowsVersion()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["Version"].ToString();
                }
            }
            catch { }
            return "";
        }
    }
}