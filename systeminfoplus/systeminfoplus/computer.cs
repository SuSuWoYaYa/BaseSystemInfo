using System;
using System.Collections.Generic;
using System.Text;

 
using System.Runtime.InteropServices;
 
 
using System.Management;

namespace systeminfoplus
{   /// <summary> 
    /// 计算机信息类
    /// </summary> 
    //internal 
     public class Computer
    {
        public string CpuID;
        public string MacAddress;
        public string DiskTypeName;
        public string IpAddress;
        public string LoginUserName;
        public string ComputerName;
        public string OperatingSystem;
        public string SystemType;
        public string InstallDate;
        public string PCsn;
        public string HardDiskID;
        public string HardDiskSerialNumber;
        public string TotalPhysicalMemory; //单位：M 
        private static Computer _instance;



        private const string Windows2000 = "5.0";
        private const string WindowsXP = "5.1";
        private const string Windows2003 = "5.2";
        private const string Windows2008 = "6.0";
        private const string Windows7 = "6.1";
        private const string Windows8OrWindows81 = "6.2";
        private const string Windows10 = "10.0";


         //internal
        public static Computer Instance()
        {
            if (_instance == null)
                _instance = new Computer();
            return _instance;
        }

        internal Computer()
        {
            //CpuID = GetCpuID();
            MacAddress = GetMacAddress();
            //DiskTypeName = GetDiskTypeName();
            IpAddress = GetIPAddress();
            //LoginUserName = GetUserName();
            OperatingSystem = GetOperatingSystem();
            //SystemType = GetSystemType();
            InstallDate = GetInstallDate();
            HardDiskID = GetHardDiskID();
            //TotalPhysicalMemory = GetTotalPhysicalMemory();
            //ComputerName = GetComputerName();
            PCsn = GetPcsnString();
            HardDiskSerialNumber = GetHardDiskSerialNumber();
        }





        /// <summary>
        /// 获取硬盘唯一序列号（不是卷标号），可能需要以管理员身份运行程序
        /// </summary>
        /// <returns></returns>
        public  string GetHdId()
        {
            ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher();
            /*
             * PNPDeviceID   的数据是由四部分组成的：   
  1、接口，通常有   IDE，ATA，SCSI；   
  2、型号   
  3、（可能）驱动版本号   
  4、（可能）硬盘的出厂序列号   
             * 
             * 
             */
            //signature 需要程序以管理员身份运行（经过测试，2003系统上非管理员身份也可以运行，查相关资料说，可能在2000系统上获取的值为空）
            wmiSearcher.Query = new SelectQuery(
            "Win32_DiskDrive",
            "",
            new string[] { "PNPDeviceID", "signature" }
            );
            ManagementObjectCollection myCollection = wmiSearcher.Get();
            ManagementObjectCollection.ManagementObjectEnumerator em =
            myCollection.GetEnumerator();
            em.MoveNext();
            ManagementBaseObject mo = em.Current;
            //string id = mo.Properties["PNPDeviceID"].Value.ToString().Trim();
            string id = mo.Properties["signature"].Value.ToString().Trim();
            return id;
        }
        //--------------------- 
        //作者：bluedoctor 
        //来源：CSDN 
        //原文：https://blog.csdn.net/bluedoctor/article/details/3201686 
        //版权声明：本文为博主原创文章，转载请附上博文链接！

        string GetCpuID()
        {
            try
            {
                //获取CPU序列号代码 
                string cpuInfo = "";//cpu序列号 
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
        string GetMacAddress()
        {
            try
            {
                //获取网卡硬件地址 
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
        string GetIPAddress()
        {
            try
            {
                //获取IP地址 
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        //st=mo["IpAddress"].ToString(); 
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        string GetDiskTypeName()
        {
            try
            {
                //获取硬盘类型名字
                String HDname = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDname = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return HDname;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //获取硬盘序列号
        string GetHardDiskID()
        {
            try
            {
                
                
                //创建ManagementObjectSearcher对象
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                String strHardDiskID = ""; //存储磁盘序列号
                //调用ManagementObjectSearcher类的Get方法取得硬盘序列号
                foreach (ManagementObject mo in searcher.Get())
                {
                    strHardDiskID = mo["SerialNumber"].ToString().Trim();//记录获得的磁盘序列号
                    break;
                }
                return strHardDiskID;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //获取硬盘序列号2
        string GetHardDiskSerialNumber()
        {
            try
            {
                ManagementClass managementClass = new ManagementClass("Win32_PhysicalMedia");
                ManagementObjectCollection managementObjColl = managementClass.GetInstances();
                PropertyDataCollection properties = managementClass.Properties;
                foreach (PropertyData property in properties)
                {
                    if (property.Name == "SerialNumber")
                    {
                        foreach (var managementBaseObject in managementObjColl)
                        {
                            var managementObject = (ManagementObject)managementBaseObject;
                            return managementObject.Properties[property.Name].Value.ToString().Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        public string GetHardDiskSerialNumber3()
        {
            var driveQuery = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject d in driveQuery.Get())
            {
                //var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", d.Path.RelativePath);
                //var partitionQuery = new ManagementObjectSearcher(partitionQueryText);

                //foreach (ManagementObject p in partitionQuery.Get())
                //{
                //    var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition",
                //        p.Path.RelativePath);
                //    var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
               // string strComputer = ".";
                //ManagementScope namespaceScope = new ManagementScope("\\\\" + strComputer + "\\ROOT\\CIMV2");
                //ObjectQuery diskQuery = new ObjectQuery("SELECT * FROM Win32_LogicalDisk ");
                //ManagementObjectSearcher logicalDriveQuery = new ManagementObjectSearcher(namespaceScope, diskQuery);

               // foreach (ManagementObject ld in logicalDriveQuery.Get())
               // {
                //    GetDiskInfo(ld);
                    //PrintLocalDiskSpecs();
                 //   Console.ReadKey();
               // }
                //}
               // return GetHardDriveInfo(d);
                //PrintMainSpecs();
               // Console.ReadLine();
                string SerialNumber = Convert.ToString(d.Properties["SerialNumber"].Value).Trim(); // bool
                return SerialNumber;
            }

            return "未测到";
        }
        string GetHardDriveInfo(ManagementObject d)
        {
            //PhysicalName = Convert.ToString(d.Properties["Name"].Value);
            //DiskName = Convert.ToString(d.Properties["Caption"].Value);
            //DiskModel = Convert.ToString(d.Properties["Model"].Value);

           // HdTotalSpace = Convert.ToUInt64(d.Properties["Size"].Value); // in bytes
            //HdNonFreeSpace = HdTotalSpace - HdFreeSpace;
            //DiskInterface = Convert.ToString(d.Properties["InterfaceType"].Value);
            string SerialNumber = Convert.ToString(d.Properties["SerialNumber"].Value); // bool
            return SerialNumber;
            //Size = Convert.ToUInt64(d.Properties["Size"].Value); // Fixed hard disk media
            //MediaSignature = Convert.ToUInt32(d.Properties["Signature"].Value); // int32
           // MediaStatus = Convert.ToString(d.Properties["Status"].Value); // OK
        }



        /// <summary> 
        /// 操作系统的登录用户名 
        /// </summary> 
        /// <returns></returns> 
        string GetUserName()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    st = mo["UserName"].ToString();

                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }


        /// <summary> 
        /// 操作系统的安装时间
        /// </summary> 
        string GetInstallDate()
        {
            System.Management.ObjectQuery MyQuery = new System.Management.ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            System.Management.ManagementScope MyScope = new System.Management.ManagementScope();
            ManagementObjectSearcher MySearch = new ManagementObjectSearcher(MyScope, MyQuery);
            ManagementObjectCollection MyCollection = MySearch.Get();
            string StrInfo = "";
            foreach (ManagementObject MyObject in MyCollection)
            {
                StrInfo = MyObject.GetText(TextFormat.Mof);
            }
            string installDate = "";
            installDate = StrInfo.Substring(StrInfo.LastIndexOf("InstallDate") + 15, 14);
            installDate = installDate.Substring(0, 8);
            return installDate;
        }

             
        /// <summary> 
        /// 操作系统版本
        /// </summary> 
        /// <returns></returns> 
        string GetOperatingSystem()
        {

            try
            {
                string st = "";
        
            switch (System.Environment.OSVersion.Version.Major + "." + System.Environment.OSVersion.Version.Minor)
            {
                case Windows2000:
                     st ="Windows2000";
                    break;
                case WindowsXP:
                     st ="XP";
                    break;
                case Windows2003:
                     st ="Windows2003";
                    break;
                case Windows2008:
                     st ="Windows2008";
                    break;
                case Windows7:
                     st ="Windows7";
                    break;
                case Windows8OrWindows81:
                     st ="Windows8.OrWindows8.1";
                    break;
                case Windows10:
                     st ="Windows10";
                    break;
            }
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        /// <summary> 
        /// PC类型 
        /// </summary> 
        /// <returns></returns> 
        string GetSystemType()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    st = mo["SystemType"].ToString();

                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        /// <summary> 
        /// 物理内存 
        /// </summary> 
        /// <returns></returns> 
        string GetTotalPhysicalMemory()
        {
            try
            {

                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    st = mo["TotalPhysicalMemory"].ToString();

                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }
        /// <summary> 
        ///  获取计算机名称
        /// </summary> 
        /// <returns></returns> 
        string GetComputerName()
        {
            try
            {
                return System.Environment.GetEnvironmentVariable("ComputerName");
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }


        /// <summary>
        /// 获得pc序列号
        /// </summary>
        public   string GetPcsnString()
        {
            string pcsn = "";
             
            try
            {
                var search = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                var mobos = search.Get();
                foreach (var temp in mobos)
                {
                    object serial = temp["SerialNumber"]; // ProcessorID if you use Win32_CPU
                    pcsn = serial.ToString();
                 //   Console.WriteLine(pcsn);

                    if
                    (
                        !string.IsNullOrEmpty(pcsn)
                        && pcsn != "To be filled by O.E.M" //没有找到
                        && !pcsn.Contains("O.E.M")
                        && !pcsn.Contains("OEM")
                        && !pcsn.Contains("Default")
                    )
                    {
                        break;
                    }
                    else
                    {
                        pcsn = "未检测到 \"" + pcsn + "\"";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // 无法处理
                pcsn = "未检测到";
                return pcsn;
            }

            return pcsn;
        }
    }


     /// <summary>
     /// 使用ＷＭＩ方式获取硬盘信息
     /// </summary>
     public class WMI_HD
     {
         /// <summary>
         /// 获取硬盘唯一序列号（不是卷标号），可能需要以管理员身份运行程序
         /// </summary>
         /// <returns></returns>
         public static string GetHdId()
         {
             ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher();
             /*
              * PNPDeviceID   的数据是由四部分组成的：   
   1、接口，通常有   IDE，ATA，SCSI；   
   2、型号   
   3、（可能）驱动版本号   
   4、（可能）硬盘的出厂序列号   
              * 
              * 
              */


             //signature 需要程序以管理员身份运行（经过测试，2003系统上非管理员身份也可以运行，查相关资料说，可能在2000系统上获取的值为空）

             wmiSearcher.Query = new SelectQuery(
             "Win32_DiskDrive",
             "",
             new string[] { "PNPDeviceID", "signature" }
             );
             ManagementObjectCollection myCollection = wmiSearcher.Get();
             ManagementObjectCollection.ManagementObjectEnumerator em =
             myCollection.GetEnumerator();
             em.MoveNext();
             ManagementBaseObject mo = em.Current;
             //string id = mo.Properties["PNPDeviceID"].Value.ToString().Trim();
             string id = "";
             try
             {
                 //首先使用signature，ＳＣＳＩ硬盘可能没有该属性
                 if (mo.Properties["signature"] != null && mo.Properties["signature"].Value != null)
                     id = mo.Properties["signature"].Value.ToString();
                 else if (mo.Properties["PNPDeviceID"] != null && mo.Properties["PNPDeviceID"].Value != null)//防止意外
                     id = mo.Properties["PNPDeviceID"].Value.ToString();
             }
             catch
             {
                 if (mo.Properties["PNPDeviceID"] != null && mo.Properties["PNPDeviceID"].Value != null)//防止意外
                     id = mo.Properties["PNPDeviceID"].Value.ToString();
             }

             return id;
         }
     }

     public class HardDiskSN
     {
         static string _serialNumber = string.Empty;
         /// <summary>
         /// 获取硬盘号
         /// </summary>
         public static string SerialNumber
         {
             get
             {
                 if (_serialNumber == string.Empty)
                 {
                     try
                     {
                         HardDiskInfo hdd = AtapiDevice.GetHddInfo(0); // 第一个硬盘
                         _serialNumber = hdd.SerialNumber;
                     }
                     catch
                     {
                         try
                         {
                             _serialNumber = "WMI" + WMI_HD.GetHdId();
                         }
                         catch
                         {
                             _serialNumber = "未测到";
                         }

                     }

                 }
                 return _serialNumber;
             }
         }
     }



 
    [Serializable]
    public struct HardDiskInfo
    {
        /// <summary>
        /// 型号
        /// </summary>
        public string ModuleNumber;
        /// <summary>
        /// 固件版本
        /// </summary>
        public string Firmware;
        /// <summary>
        /// 序列号
        /// </summary>
        public string SerialNumber;
        /// <summary>
        /// 容量，以M为单位
        /// </summary>
        public uint Capacity;
    }
    #region Internal Structs
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct GetVersionOutParams
    {
        public byte bVersion;
        public byte bRevision;
        public byte bReserved;
        public byte bIDEDeviceMap;
        public uint fCapabilities;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] dwReserved; // For future use.
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct IdeRegs
    {
        public byte bFeaturesReg;
        public byte bSectorCountReg;
        public byte bSectorNumberReg;
        public byte bCylLowReg;
        public byte bCylHighReg;
        public byte bDriveHeadReg;
        public byte bCommandReg;
        public byte bReserved;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SendCmdInParams
    {
        public uint cBufferSize;
        public IdeRegs irDriveRegs;
        public byte bDriveNumber;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] bReserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] dwReserved;
        public byte bBuffer;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct DriverStatus
    {
        public byte bDriverError;
        public byte bIDEStatus;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] bReserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] dwReserved;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SendCmdOutParams
    {
        public uint cBufferSize;
        public DriverStatus DriverStatus;
        public IdSector bBuffer;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 512)]
    internal struct IdSector
    {
        public ushort wGenConfig;
        public ushort wNumCyls;
        public ushort wReserved;
        public ushort wNumHeads;
        public ushort wBytesPerTrack;
        public ushort wBytesPerSector;
        public ushort wSectorsPerTrack;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public ushort[] wVendorUnique;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] sSerialNumber;
        public ushort wBufferType;
        public ushort wBufferSize;
        public ushort wECCSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] sFirmwareRev;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] sModelNumber;
        public ushort wMoreVendorUnique;
        public ushort wDoubleWordIO;
        public ushort wCapabilities;
        public ushort wReserved1;
        public ushort wPIOTiming;
        public ushort wDMATiming;
        public ushort wBS;
        public ushort wNumCurrentCyls;
        public ushort wNumCurrentHeads;
        public ushort wNumCurrentSectorsPerTrack;
        public uint ulCurrentSectorCapacity;
        public ushort wMultSectorStuff;
        public uint ulTotalAddressableSectors;
        public ushort wSingleWordDMA;
        public ushort wMultiWordDMA;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] bReserved;
    }
    #endregion
    /// <summary>
    /// ATAPI驱动器相关
    /// </summary>
    public class AtapiDevice
    {
        #region DllImport
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);
        [DllImport("kernel32.dll")]
        static extern int DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            ref GetVersionOutParams lpOutBuffer,
            uint nOutBufferSize,
            ref uint lpBytesReturned,
            [Out] IntPtr lpOverlapped);
        [DllImport("kernel32.dll")]
        static extern int DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            ref SendCmdInParams lpInBuffer,
            uint nInBufferSize,
            ref SendCmdOutParams lpOutBuffer,
            uint nOutBufferSize,
            ref uint lpBytesReturned,
            [Out] IntPtr lpOverlapped);
        const uint DFP_GET_VERSION = 0x00074080;
        const uint DFP_SEND_DRIVE_COMMAND = 0x0007c084;
        const uint DFP_RECEIVE_DRIVE_DATA = 0x0007c088;
        const uint GENERIC_READ = 0x80000000;
        const uint GENERIC_WRITE = 0x40000000;
        const uint FILE_SHARE_READ = 0x00000001;
        const uint FILE_SHARE_WRITE = 0x00000002;
        const uint CREATE_NEW = 1;
        const uint OPEN_EXISTING = 3;
        #endregion
        #region GetHddInfo
        /// <summary>
        /// 获得硬盘信息
        /// </summary>
        /// <param name="driveIndex">硬盘序号</param>
        /// <returns>硬盘信息</returns>
        /// <remarks>
        /// 参考lu0的文章：http://lu0s1.3322.org/App/2k1103.html
        /// by sunmast for everyone
        /// thanks lu0 for his great works
        /// 在Windows 98/ME中，S.M.A.R.T并不缺省安装，请将SMARTVSD.VXD拷贝到%SYSTEM%/IOSUBSYS目录下。
        /// 在Windows 2000/2003下，需要Administrators组的权限。
        /// </remarks>
        /// <example>
        /// AtapiDevice.GetHddInfo()
        /// </example>
        public static HardDiskInfo GetHddInfo(byte driveIndex)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32Windows:
                    return GetHddInfo9x(driveIndex);
                case PlatformID.Win32NT:
                    //throw new NotSupportedException("Win32NT");
                    return GetHddInfoNT(driveIndex);
                    
                case PlatformID.Win32S:
                    throw new NotSupportedException("Win32s is not supported.");
                case PlatformID.WinCE:
                    throw new NotSupportedException("WinCE is not supported.");
                default:
                    throw new NotSupportedException("Unknown Platform.");
            }
        }
        #region GetHddInfo9x
        private static HardDiskInfo GetHddInfo9x(byte driveIndex)
        {
            GetVersionOutParams vers = new GetVersionOutParams();
            SendCmdInParams inParam = new SendCmdInParams();
            SendCmdOutParams outParam = new SendCmdOutParams();
            uint bytesReturned = 0;
            IntPtr hDevice = CreateFile(
                @"//./Smartvsd",
                0,
                0,
                IntPtr.Zero,
                CREATE_NEW,
                0,
                IntPtr.Zero);
            if (hDevice == IntPtr.Zero)
            {
                throw new Exception("Open smartvsd.vxd failed.");
            }
            if (0 == DeviceIoControl(
                hDevice,
                DFP_GET_VERSION,
                IntPtr.Zero,
                0,
                ref vers,
                (uint)Marshal.SizeOf(vers),
                ref bytesReturned,
                IntPtr.Zero))
            {
                CloseHandle(hDevice);
                throw new Exception("DeviceIoControl failed:DFP_GET_VERSION");
            }
            // If IDE identify command not supported, fails
            if (0 == (vers.fCapabilities & 1))
            {
                CloseHandle(hDevice);
                throw new Exception("Error: IDE identify command not supported.");
            }
            if (0 != (driveIndex & 1))
            {
                inParam.irDriveRegs.bDriveHeadReg = 0xb0;
            }
            else
            {
                inParam.irDriveRegs.bDriveHeadReg = 0xa0;
            }
            if (0 != (vers.fCapabilities & (16 >> driveIndex)))
            {
                // We don't detect a ATAPI device.
                CloseHandle(hDevice);
                throw new Exception(string.Format("Drive {0} is a ATAPI device, we don't detect it", driveIndex));
            }
            else
            {
                inParam.irDriveRegs.bCommandReg = 0xec;
            }
            inParam.bDriveNumber = driveIndex;
            inParam.irDriveRegs.bSectorCountReg = 1;
            inParam.irDriveRegs.bSectorNumberReg = 1;
            inParam.cBufferSize = 512;
            if (0 == DeviceIoControl(
                hDevice,
                DFP_RECEIVE_DRIVE_DATA,
                ref inParam,
                (uint)Marshal.SizeOf(inParam),
                ref outParam,
                (uint)Marshal.SizeOf(outParam),
                ref bytesReturned,
                IntPtr.Zero))
            {
                CloseHandle(hDevice);
                throw new Exception("DeviceIoControl failed: DFP_RECEIVE_DRIVE_DATA");
            }
            CloseHandle(hDevice);
            return GetHardDiskInfo(outParam.bBuffer);
        }
        #endregion
        #region GetHddInfoNT
        private static HardDiskInfo GetHddInfoNT(byte driveIndex)
        {
            GetVersionOutParams vers = new GetVersionOutParams();
            SendCmdInParams inParam = new SendCmdInParams();
            SendCmdOutParams outParam = new SendCmdOutParams();
            uint bytesReturned = 0;
            // We start in NT/Win2000
            IntPtr hDevice = CreateFile(
                string.Format(@"//./PhysicalDrive{0}", driveIndex),
                GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                IntPtr.Zero,
                OPEN_EXISTING,
                0,
                IntPtr.Zero);
            if (hDevice == IntPtr.Zero)
            {
                throw new Exception("CreateFile faild.");
            }
            if (0 == DeviceIoControl(
                hDevice,
                DFP_GET_VERSION,
                IntPtr.Zero,
                0,
                ref vers,
                (uint)Marshal.SizeOf(vers),
                ref bytesReturned,
                IntPtr.Zero))
            {
                CloseHandle(hDevice);
                throw new Exception(string.Format("Win32NT Drive {0} may not exists.", driveIndex));
            }
            // If IDE identify command not supported, fails
            if (0 == (vers.fCapabilities & 1))
            {
                CloseHandle(hDevice);
                throw new Exception("Error: IDE identify command not supported.");
            }
            // Identify the IDE drives
            if (0 != (driveIndex & 1))
            {
                inParam.irDriveRegs.bDriveHeadReg = 0xb0;
            }
            else
            {
                inParam.irDriveRegs.bDriveHeadReg = 0xa0;
            }
            if (0 != (vers.fCapabilities & (16 >> driveIndex)))
            {
                // We don't detect a ATAPI device.
                CloseHandle(hDevice);
                throw new Exception(string.Format("Drive {0} is a ATAPI device, we don't detect it.", driveIndex + 1));
            }
            else
            {
                inParam.irDriveRegs.bCommandReg = 0xec;
            }
            inParam.bDriveNumber = driveIndex;
            inParam.irDriveRegs.bSectorCountReg = 1;
            inParam.irDriveRegs.bSectorNumberReg = 1;
            inParam.cBufferSize = 512;
            if (0 == DeviceIoControl(
                hDevice,
                DFP_RECEIVE_DRIVE_DATA,
                ref inParam,
                (uint)Marshal.SizeOf(inParam),
                ref outParam,
                (uint)Marshal.SizeOf(outParam),
                ref bytesReturned,
                IntPtr.Zero))
            {
                CloseHandle(hDevice);
                throw new Exception("DeviceIoControl failed: DFP_RECEIVE_DRIVE_DATA");
            }
            CloseHandle(hDevice);
            return GetHardDiskInfo(outParam.bBuffer);
        }
        #endregion
        private static HardDiskInfo GetHardDiskInfo(IdSector phdinfo)
        {
            HardDiskInfo hddInfo = new HardDiskInfo();
            ChangeByteOrder(phdinfo.sModelNumber);
            hddInfo.ModuleNumber = Encoding.ASCII.GetString(phdinfo.sModelNumber).Trim();
            ChangeByteOrder(phdinfo.sFirmwareRev);
            hddInfo.Firmware = Encoding.ASCII.GetString(phdinfo.sFirmwareRev).Trim();
            ChangeByteOrder(phdinfo.sSerialNumber);
            hddInfo.SerialNumber = Encoding.ASCII.GetString(phdinfo.sSerialNumber).Trim();
            hddInfo.Capacity = phdinfo.ulTotalAddressableSectors / 2 / 1024;
            return hddInfo;
        }
        private static void ChangeByteOrder(byte[] charArray)
        {
            byte temp;
            for (int i = 0; i < charArray.Length; i += 2)
            {
                temp = charArray[i];
                charArray[i] = charArray[i + 1];
                charArray[i + 1] = temp;
            }
        }
        #endregion
    }



    public class HardDisk
    {
        //public class PartitionHardDisk
        //{
        //    public string Caption;
        //    public string Size;
        //    public string Type;
        //}

        public string SerialNumber;
        public string Caption;
        public string Size;
        public string PartitionValue;
        //public PartitionHardDisk[] Partition;

        public HardDisk[] GetDevice()
        {
            List<HardDisk> arrayHardDisk = new List<HardDisk>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                HardDisk tempHardDisk = new HardDisk();
                tempHardDisk.SerialNumber = queryObj["SerialNumber"] != null ? queryObj["SerialNumber"].ToString().Trim() : "未测到";
                tempHardDisk.Caption = queryObj["Caption"] != null ? queryObj["Caption"].ToString() : "Nodata";
                tempHardDisk.Size = queryObj["Size"] != null ? (Convert.ToDouble(queryObj["Size"]) / 1073741824).ToString() : "Nodata";
                tempHardDisk.PartitionValue = queryObj["Partitions"] != null ? queryObj["Partitions"].ToString() : "Nodata";

                /*
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskPartition");
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        Console.WriteLine("Caption: {0}", queryObj["Caption"]);
                        Console.WriteLine("Size     : {0}", (long.Parse(queryObj["Size"].ToString()) / 1073741824).ToString());
                        Console.WriteLine("Type: {0}", queryObj["Type"]);
                    }
                }
                catch (ManagementException e)
                {
                    ;
                }
                 */

                arrayHardDisk.Add(tempHardDisk);
            }
            return arrayHardDisk.ToArray();
        }
    }

    
}
