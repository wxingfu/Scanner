using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace Scanner.Toolkit
{
    internal static class ScriptUtil
    {

        public static void DisableFips()
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Lsa", true);
                if (registryKey == null)
                    return;
                registryKey.SetValue("fipsalgorithmpolicy", "0", RegistryValueKind.DWord);
                registryKey.Close();
                Console.WriteLine("禁用FIPS组策略验证成功(XP)！ ");
            }
            else
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Lsa\\FipsAlgorithmPolicy", true);
                if (registryKey == null)
                    return;
                registryKey.SetValue("Enabled", "0");
                registryKey.Close();
                Console.WriteLine("禁用FIPS组策略验证成功！ ");
            }
        }

        public static bool KillApp(string appname, string title = "", bool isforce = true)
        {
            Process[] processesByName = Process.GetProcessesByName(appname);
            if (processesByName.Length == 0)
                return true;
            for (int index = 0; index < processesByName.Length; ++index)
            {
                Process process = processesByName[index];
                if (string.IsNullOrEmpty(title) || process.MainWindowTitle.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    if (isforce)
                        process.Kill();
                    else
                        process.CloseMainWindow();
                }
            }
            return true;
        }

        public static void StartServer(string appname)
        {
            OSHelper.ExecProcess(FilePath.PathCombine(FilePath.GetAppPath(), appname + ".exe"), "start", ProcessWindowStyle.Normal, false, false);
        }

        public static bool RunApp(string appname, string args, bool IsWaitExit = true)
        {
            return OSHelper.ExecProcess(appname, args, ProcessWindowStyle.Hidden, false, IsWaitExit);
        }

        public static void RegUrlProtocol()
        {
            try
            {
                string str = FilePath.PathCombine(FilePath.GetAppPath(), "Scanner.exe");
                RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("lisscanner", true) ?? Registry.ClassesRoot.CreateSubKey("lisscanner");
                registryKey.SetValue("URL Protocol", str);
                registryKey.CreateSubKey("DefaultIcon").SetValue("", string.Format("{0},0", str), RegistryValueKind.ExpandString);
                registryKey.CreateSubKey("shell\\open\\command").SetValue("", string.Format("\"{0}\" \"%1\"", str), RegistryValueKind.ExpandString);
                registryKey.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("写键值失败！ 错误原因：" + ex.Message);
            }
        }

        public static void UnRegUrlProtocol()
        {
            try
            {
                if (Registry.ClassesRoot.OpenSubKey("lisscanner", false) == null)
                    return;
                Registry.ClassesRoot.DeleteSubKeyTree("lisscanner");
            }
            catch (Exception ex)
            {
                Console.WriteLine("删除键值失败， 错误原因：" + ex.Message);
            }
        }
    }
}
