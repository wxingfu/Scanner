using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Scanner.Toolkit
{
    public class FilePath
    {

        public static string GetAppPath()
        {
            try
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetSupportPath()
        {
            return PathCombine(GetAppPath(), "support");
        }

        public static string GetWindowsPath()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
            return folderPath.Substring(0, folderPath.IndexOf("\\system"));
        }

        public static string GetSystemPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.System);
        }

        public static string GetCommonFilesPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
        }

        public static string GetLogsPath()
        {
            return PathCombine(GetAppPath(), "\\logs");
        }

        public static string GetDataPath()
        {
            return PathCombine(GetAppPath(), "\\data");
        }

        public static string GetTempPath()
        {
            return PathCombine(GetAppPath(), "\\temp\\");
        }

        public static string GetUpdatePath()
        {
            return PathCombine(GetAppPath(), "\\update\\");
        }

        public static string GetRootPath(string subdir)
        {
            string fullName = new DirectoryInfo(GetAppPath()).Root.FullName;
            return string.IsNullOrEmpty(subdir) ? fullName : PathCombine(fullName, subdir);
        }

        public static string GetUserPath(string sPath)
        {
            string dataPath = GetDataPath();
            string path = dataPath;
            if (!string.IsNullOrEmpty(sPath))
            {
                if (sPath.StartsWith("..\\"))
                {
                    path = PathCombine(GetAppPath(), sPath.TrimStart("..\\".ToCharArray()));
                }
                else
                {
                    string str = StringHelper.UseRegexSubString("%[a-zA-Z]*%", sPath);
                    if (!string.IsNullOrEmpty(str))
                    {
                        string variable = str.Substring(1, str.Length - 2);
                        string environmentVariable = Environment.GetEnvironmentVariable(variable);
                        path = sPath.Replace("%" + variable + "%", environmentVariable);
                        if (!string.IsNullOrEmpty(path))
                        {
                            CreateDirectory(path);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(path))
            {
                path = dataPath;
            }

            return path;
        }

        public static string GetBasePath()
        {
            try
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                return Directory.GetParent(codeBase.Substring(8, codeBase.Length - 8)).FullName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string RelativePath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return fileName;
            }
            string fpath = fileName.Replace(GetAppPath(), "");
            return PathCombine(GetAppPath(), fpath);
        }

        public static string RelativePath(string absolutePath, string relativeTo)
        {
            string[] strArray1 = absolutePath.Split(new char[1] { '\\' });
            string[] strArray2 = relativeTo.Split(new char[1] { '\\' });
            int num1 = strArray1.Length < strArray2.Length ? strArray1.Length : strArray2.Length;
            int num2 = -1;
            for (int index = 0; index < num1 && strArray1[index] == strArray2[index]; ++index)
            {
                num2 = index;
            }
            if (num2 == -1)
            {
                throw new ArgumentException("Paths do not have a common base");
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = num2 + 1; index < strArray1.Length; ++index)
            {
                if (strArray1[index].Length > 0)
                {
                    stringBuilder.Append("..\\");
                }
            }
            for (int index = num2 + 1; index < strArray2.Length - 1; ++index)
            {
                stringBuilder.Append(strArray2[index] + "\\");
            }
            stringBuilder.Append(strArray2[strArray2.Length - 1]);
            return stringBuilder.ToString();
        }

        public static string GetFileFormat(string fileName)
        {
            string fileFormat = Path.GetExtension(fileName);
            if (!string.IsNullOrEmpty(fileFormat))
            {
                fileFormat = fileFormat.Replace(".", "");
            }
            return fileFormat;
        }

        public static bool CheckWriteAccess(string fileName)
        {
            int num = 0;
            while (num < 10)
            {
                try
                {
                    new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None).Close();
                    return true;
                }
                catch
                {
                    Thread.Sleep(200);
                }
                finally
                {
                    ++num;
                }
            }
            return false;
        }

        public static bool IsFileOpen(string filePath)
        {
            try
            {
                File.OpenWrite(filePath).Close();
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public static bool IsFileReadOnly(string path)
        {
            try
            {
                return (File.GetAttributes(path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteFile(string fileName)
        {
            bool flag = false;
            if (File.Exists(fileName))
            {
                if (IsFileReadOnly(fileName))
                {
                    throw new ApplicationException(string.Format("File '{0}' Is Readonly.", fileName));
                }
                File.Delete(fileName);
                flag = true;
            }
            return flag;
        }

        public static bool DeleteFiles(string filename)
        {
            string directoryName = Path.GetDirectoryName(filename);
            string withoutExtension = Path.GetFileNameWithoutExtension(filename);
            foreach (string file in Directory.GetFiles(directoryName))
            {
                if (Path.GetFileNameWithoutExtension(file).Equals(withoutExtension))
                {
                    DeleteFile(file);
                }
            }
            return true;
        }

        public static string FileCombine(string filename, string extension)
        {
            filename = RelativePath(filename);
            return Path.ChangeExtension(filename, extension);
        }

        public static bool RenameFile(string source, string target)
        {
            try
            {
                if (File.Exists(target))
                {
                    DeleteFile(target);
                }
                File.Move(source, target);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CopyFile(string source, string desc)
        {
            if (!File.Exists(source))
            {
                throw new FileNotFoundException(source + "File Not Found！");
            }
            File.Copy(source, desc, true);
            SetFileNormal(desc);
            return true;
        }

        public static bool MoveFile(string source, string desc)
        {
            if (!File.Exists(source))
            {
                throw new FileNotFoundException(source + "File Not Found！");
            }
            CreateDirectory(Path.GetDirectoryName(desc));
            if (File.Exists(desc))
            {
                File.Delete(desc);
            }
            File.Move(source, desc);
            SetFileNormal(desc);
            return true;
        }

        public static void SetFileNormal(string fileName)
        {
            try
            {
                int attributes = (int)File.GetAttributes(fileName);
                File.SetAttributes(fileName, FileAttributes.Normal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void MakeWritable(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }
            File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);
        }

        public static string GetExtension(string fileName)
        {
            string str = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(str))
            {
                str = "";
            }
            return str.ToLower();
        }

        public static bool DeleteDirectory(string path)
        {
            bool flag = false;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                flag = true;
            }
            return flag;
        }

        public static bool CreateDirectory(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("不能访问这个目录，错误原因：" + ex.Message);
            }
        }

        public static void DirectoryClear(string path)
        {
            foreach (string file in Directory.GetFiles(path))
            {
                File.Delete(file);
            }
        }

        public static string DirectoryCombine(string path1, string path2)
        {
            string path = Path.Combine(path1, path2);
            if (CreateDirectory(path))
            {
                return path;
            }
            throw new ArgumentException("Can not Create Directory");
        }

        public static void CopyDirectory(string srcDir, string destDir)
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(srcDir);
            DirectoryInfo directoryInfo2 = new DirectoryInfo(destDir);
            if (directoryInfo2.FullName.StartsWith(directoryInfo1.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!directoryInfo1.Exists)
            {
                return;
            }

            if (!directoryInfo2.Exists)
            {
                directoryInfo2.Create();
            }

            FileInfo[] files = directoryInfo1.GetFiles();
            for (int index = 0; index < files.Length; ++index)
            {
                CopyFile(files[index].FullName, directoryInfo2.FullName + "\\" + files[index].Name);
            }

            DirectoryInfo[] directories = directoryInfo1.GetDirectories();
            for (int index = 0; index < directories.Length; ++index)
            {
                CopyDirectory(directories[index].FullName, directoryInfo2.FullName + "\\" + directories[index].Name);
            }
        }

        public static void MoveDirectory(string srcDir, string destDir)
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(srcDir);
            DirectoryInfo directoryInfo2 = new DirectoryInfo(destDir);
            if (directoryInfo2.FullName.StartsWith(directoryInfo1.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!directoryInfo1.Exists)
            {
                return;
            }

            if (!directoryInfo2.Exists)
            {
                directoryInfo2.Create();
            }

            FileInfo[] files = directoryInfo1.GetFiles();
            for (int index = 0; index < files.Length; ++index)
            {
                MoveDirectory(files[index].FullName, directoryInfo2.FullName + "\\" + files[index].Name);
            }

            DirectoryInfo[] directories = directoryInfo1.GetDirectories();
            for (int index = 0; index < directories.Length; ++index)
            {
                MoveDirectory(directories[index].FullName, directoryInfo2.FullName + "\\" + directories[index].Name);
            }
        }

        /// <summary>
        /// 路径拼接
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="fpath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string PathCombine(string path1, string fpath)
        {
            try
            {
                if (!string.IsNullOrEmpty(fpath) && fpath.StartsWith("\\"))
                {
                    fpath = fpath.TrimStart(new char[1] { '\\' });
                }

                if (string.IsNullOrEmpty(path1))
                {
                    path1 = GetAppPath();
                }

                string extension = Path.GetExtension(fpath);
                string path2 = string.IsNullOrEmpty(extension) ? fpath : Path.GetDirectoryName(fpath);
                string path = Path.Combine(path1, path2);

                if (!CreateDirectory(path))
                {
                    throw new ArgumentException("Can not Create Directory");
                }

                if (!path.EndsWith("\\"))
                {
                    path += "\\";
                }

                return !string.IsNullOrEmpty(extension) ? path + Path.GetFileName(fpath) : path;
            }
            catch (Exception ex)
            {
                throw new Exception("PathCombine:" + ex?.ToString());
            }
        }

        public static List<string> GetFiles(string path, string[] exts)
        {
            List<string> files = new List<string>();
            try
            {
                if (exts != null)
                {
                    foreach (string ext in exts)
                    {
                        foreach (string file in Directory.GetFiles(path, "*" + ext))
                        {
                            files.Add(file);
                        }
                    }
                }
                else
                {
                    foreach (string file in Directory.GetFiles(path))
                    {
                        files.Add(file);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return files;
        }

        public static bool HasFileAttribute(string file, FileAttributes att)
        {
            FileAttributes attributes = File.GetAttributes(file);
            return (att & attributes) != 0;
        }

        public static List<string> GetFilesWithExtension(string curPath, string[] exts = null, bool isRecursive = false)
        {
            List<string> stringList = new List<string>();
            stringList.AddRange(GetFiles(curPath, exts));
            if (isRecursive)
            {
                foreach (string directory in Directory.GetDirectories(curPath))
                {
                    stringList.AddRange(GetFilesWithExtension(directory, exts, isRecursive));
                }
            }
            List<string> filesWithExtension = new List<string>();
            FileAttributes att = FileAttributes.Hidden | FileAttributes.System | FileAttributes.Offline;
            foreach (string file in stringList)
            {
                if (!HasFileAttribute(file, att))
                {
                    filesWithExtension.Add(file);
                }
            }
            return filesWithExtension;
        }

        public static List<string> GetFileList(string path, string exts = null, bool isRecursive = false)
        {
            if (string.IsNullOrEmpty(exts))
            {
                return GetFilesWithExtension(path, null, isRecursive);
            }

            return GetFilesWithExtension(path, new string[1] { exts }, (isRecursive ? 1 : 0) != 0);
        }

        public static List<string> GetPictureList(string path, bool isRecursive = false)
        {
            string[] exts = new string[] { ".jpg", ".bmp", ".png", ".tif", ".gif", ".tiff" };
            return GetFilesWithExtension(path, exts, isRecursive);
        }

        public static string GetImageFormatExtension(ImageFormat imageFormat)
        {
            string imageFormatExtension = ".null";
            if (imageFormat.Equals(ImageFormat.Bmp))
            {
                imageFormatExtension = ".bmp";
            }
            else if (imageFormat.Equals(ImageFormat.Emf))
            {
                imageFormatExtension = ".emf";
            }
            else if (imageFormat.Equals(ImageFormat.Exif))
            {
                imageFormatExtension = ".exif";
            }
            else if (imageFormat.Equals(ImageFormat.Gif))
            {
                imageFormatExtension = ".gif";
            }
            else if (imageFormat.Equals(ImageFormat.Icon))
            {
                imageFormatExtension = ".ico";
            }
            else if (imageFormat.Equals(ImageFormat.Jpeg))
            {
                imageFormatExtension = ".jpg";
            }
            else if (imageFormat.Equals(ImageFormat.Png))
            {
                imageFormatExtension = ".png";
            }
            else if (imageFormat.Equals(ImageFormat.Tiff))
            {
                imageFormatExtension = ".tiff";
            }
            else if (imageFormat.Equals(ImageFormat.Wmf))
            {
                imageFormatExtension = ".wmf";
            }
            return imageFormatExtension;
        }

        public static ImageFormat GetImageFormat(string extension)
        {
            ImageFormat jpeg = ImageFormat.Jpeg;
            ImageFormat imageFormat;
            switch (extension.ToLower())
            {
                case ".jpg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".bmp":
                    imageFormat = ImageFormat.Bmp;
                    break;
                case ".tif":
                    imageFormat = ImageFormat.Tiff;
                    break;
                case ".png":
                    imageFormat = ImageFormat.Png;
                    break;
                case ".tiff":
                    imageFormat = ImageFormat.Tiff;
                    break;
                case ".gif":
                    imageFormat = ImageFormat.Gif;
                    break;
                default:
                    return jpeg;
            }
            return imageFormat;
        }

        public static bool IsImageFormat(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }
            string extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
            {
                return false;
            }
            string lower = extension.ToLower();
            return lower == ".jpeg" || lower == ".jpg" || lower == ".png"
                || lower == ".bmp" || lower == ".tif" || lower == ".tiff"
                || lower == ".gif" || lower == ".emf" || lower == ".wmf";
        }

        public static string FormatFileSizeString(long lSize)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            return (lSize >= 1024L ? (lSize / 1024L).ToString("n", provider).Replace(".00", "") : (lSize != 0L ? "1" : "0")) + " KB";
        }

        public static long GetFileSize(string fileName)
        {
            return File.Exists(fileName) ? new FileInfo(fileName).Length : 0L;
        }
    }
}
