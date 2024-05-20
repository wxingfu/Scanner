using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;

namespace Scanner.Toolkit
{
    public static class OSHelper
    {

        public static bool ExecProcess(string appName, string arguments, ProcessWindowStyle style, bool waitidle = false, bool waitExit = false)
        {
            Process process = new Process();
            process.StartInfo.FileName = appName;
            process.StartInfo.WindowStyle = style;
            process.StartInfo.Arguments = arguments;
            try
            {
                process.Start();
                if (waitidle)
                    process.WaitForInputIdle();
                if (waitExit)
                {
                    process.WaitForExit();
                    process.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static bool ExecProcess(string appName, string arguments)
        {
            return ExecProcess(appName, arguments, ProcessWindowStyle.Normal, false, false);
        }

        public static bool ExecProcess(string appName, ProcessWindowStyle style)
        {
            return ExecProcess(appName, null, style, false, false);
        }

        public static bool ExecProcess(string appName)
        {
            return ExecProcess(appName, ProcessWindowStyle.Normal);
        }

        public static void KillProcess(string processName, bool waitidle = false, bool waitExit = false)
        {
            Process[] processes = Process.GetProcesses();
            List<Process> processList = new List<Process>();
            for (int index = 0; index < processes.Length; ++index)
            {
                Process process = processes[index];
                if (process.ProcessName == processName || process.ProcessName == processName + ".vshost")
                {
                    process.Kill();
                    if (waitidle)
                        process.WaitForInputIdle();
                    if (waitExit)
                        process.WaitForExit();
                }
            }
        }

        public static string ExecCommand(string[] commandTexts, bool isWaitResult = false)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            string str = (string)null;
            try
            {
                process.Start();
                foreach (string commandText in commandTexts)
                    process.StandardInput.WriteLine(commandText);
                if (isWaitResult)
                {
                    str = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    process.Close();
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public static string ExecCommand(string commandText, bool isWaitResult = false)
        {
            return ExecCommand(new string[1]
            {
        commandText
            }, isWaitResult);
        }

        public static bool RegSvr(string localFile, bool install = true)
        {
            try
            {
                string str1 = FilePath.GetSystemPath() + "\\regsvr32.exe";
                string str2 = string.Format("/s \"{0}\"", (object)localFile);
                if (!install)
                    str2 = string.Format("/s /u \"{0}\"", (object)localFile);
                string str3 = str2;
                return ExecCommand(str1 + str3, false) != "";
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool RegAsm(string localFile, bool install = true)
        {
            try
            {
                string str1 = RuntimeEnvironment.GetRuntimeDirectory() + "\\RegAsm.exe";
                string str2 = string.Format(" /codebase \"{0}\"", (object)localFile);
                if (!install)
                    str2 = string.Format(" /u \"{0}\"", (object)localFile);
                string str3 = str2;
                return ExecCommand(str1 + str3, false) != "";
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<Process> TakeProcess(string fileName)
        {
            List<Process> process1 = new List<Process>();
            try
            {
                Process process2 = new Process();
                process2.StartInfo.FileName = FilePath.PathCombine(FilePath.GetSupportPath(), "handle.exe");
                process2.StartInfo.Arguments = fileName + " /accepteula";
                process2.StartInfo.UseShellExecute = false;
                process2.StartInfo.RedirectStandardOutput = true;
                process2.StartInfo.RedirectStandardInput = false;
                process2.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                process2.Start();
                process2.WaitForExit();
                string end = process2.StandardOutput.ReadToEnd();
                process2.Close();
                string pattern = "(?<=\\s+pid:\\s+)\\b(\\d+)\\b(?=\\s+)";
                foreach (Capture match in Regex.Matches(end, pattern))
                {
                    Process processById = Process.GetProcessById(int.Parse(match.Value));
                    if (processById.Id != Process.GetCurrentProcess().Id)
                        process1.Add(processById);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return process1;
        }

        public static bool RegKeyExist(RegistryKey rootkey, string subkey)
        {
            try
            {
                RegistryKey registryKey = rootkey.OpenSubKey(subkey, false);
                if (registryKey != null)
                {
                    registryKey.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("读取键值失败：" + subkey + " 错误原因：" + ex.Message);
            }
            return false;
        }

        public static bool Is64BitOperatingSystem()
        {
            bool flag = false;
            try
            {
                RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("Wow6432Node", false);
                if (registryKey != null)
                {
                    flag = true;
                    registryKey.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("打开键值失败：" + ex.Message);
            }
            return flag;
        }

        public static void AddSecurityControll2Folder(string dirPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
            DirectorySecurity accessControl = directoryInfo.GetAccessControl(AccessControlSections.All);
            FileSystemAccessRule rule1 = new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
            FileSystemAccessRule rule2 = new FileSystemAccessRule("Users", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
            bool modified = false;
            accessControl.ModifyAccessRule(AccessControlModification.Add, (AccessRule)rule1, out modified);
            accessControl.ModifyAccessRule(AccessControlModification.Add, (AccessRule)rule2, out modified);
            directoryInfo.SetAccessControl(accessControl);
        }

        public static void AddSecurityControll2Folder()
        {
            try
            {
                Console.WriteLine("正在设置目录控制权限....");
                string dataPath = FilePath.GetDataPath();
                AddSecurityControll2Folder(dataPath);
                FilePath.GetAppPath();
                AddSecurityControll2Folder(dataPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("设置用户控制权限失败！错误原因： " + ex.Message);
            }
        }

        public static bool Ping(string address, int port)
        {
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(address), port);
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Connect(remoteEP);
                    socket.Close();
                }
                return true;
            }
            catch (SocketException ex)
            {
                //Debugger.WriteLine("Ping:" + ex.Message);
                return false;
            }
        }

        public static bool Ping(string address)
        {
            return new Ping().Send(address, 500).Status == IPStatus.Success;
        }

        public static bool PortInActive(int port)
        {
            bool flag = false;
            foreach (IPEndPoint activeTcpListener in IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners())
            {
                if (activeTcpListener.Port == port)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        public static bool PortInActive(string address, int port)
        {
            bool flag = false;
            foreach (IPEndPoint activeTcpListener in IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners())
            {
                if (activeTcpListener.Port == port)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
    }
}
