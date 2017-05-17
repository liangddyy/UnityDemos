using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

/// <summary>
/// BabyBus.Framework
/// 文件操作
/// </summary>

namespace Babybus.Framework.Extension
{
    public class FileUtility
    {
        public static string LoadTextFile(string path)
        {
            string content = "";
            if (File.Exists(path))
            {
                content = File.ReadAllText(path);
            }
            return content;
        }

        public static string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public static byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static void CreateDirectory(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName != "" && !Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
        }

        public static void WriteAllText(string path, string content)
        {
            if (string.IsNullOrEmpty(path))
                return;

            CreateDirectory(path);

            File.WriteAllText(path, content);

            SetNoBackupFlag(path);
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            if (string.IsNullOrEmpty(path))
                return;

            CreateDirectory(path);

            File.WriteAllBytes(path, bytes);

            SetNoBackupFlag(path);
        }

//        
//        public static void WriteAllBytesAsync(string path, byte[] bytes, Action action)
//        {
//            if (string.IsNullOrEmpty(path))
//            {
//                action();
//                return;
//            }
//
//            ThreadHelper.QueueOnThreadPool((state) =>
//            {
//                WriteAllBytes(path, bytes);
//                ThreadHelper.QueueOnMainThread(action);
//            });
//        }
//        

        public static void SetNoBackupFlag(string path)
        {
            if (Application.platform != RuntimePlatform.IPhonePlayer)
                return;

#if UNITY_IOS
            ThreadHelper.QueueOnMainThread(delegate ()
            {
                Device.SetNoBackupFlag(path);
            });
#endif
        }

        public static void Move(string sourceFileName, string destFileName)
        {
            if (sourceFileName == destFileName)
                return;

            if (!File.Exists(sourceFileName))
                return;

            //DebugBuild.Log(sourceFileName + " => " + destFileName);

            CreateDirectory(destFileName);

            DeleteFile(destFileName);

            File.Move(sourceFileName, destFileName);

            SetNoBackupFlag(destFileName);
        }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            if (sourceFileName == destFileName)
                return;

            if (!IsFileExists(sourceFileName))
            {
                Debug.LogError("!IsFileExists(sourceFileName)");
                return;
            }

            //DebugBuild.Log(sourceFileName + " => " + destFileName);

            CreateDirectory(destFileName);

            File.Copy(sourceFileName, destFileName, overwrite);

            SetNoBackupFlag(destFileName);
        }

        public static void CopyDirectory(string srcPath, string dstPath, string[] includeFileExtensions)
        {
            CopyDirectory(srcPath, dstPath,
                path => includeFileExtensions != null && includeFileExtensions.Contains(Path.GetExtension(path)));
        }

        public static void CopyDirectory(string srcPath, string dstPath, bool overwrite = true,
            string excludeExtension = ".meta")
        {
            CopyDirectory(srcPath, dstPath, new string[] {excludeExtension}, new string[] {excludeExtension}, overwrite);
        }

        public static void CopyDirectory(string srcPath, string dstPath, string[] excludeFileExtensions,
            string[] excludeDirectoryExtensions, bool overwrite = true)
        {
            CopyDirectory(srcPath, dstPath,
                path => excludeFileExtensions == null || !excludeFileExtensions.Contains(Path.GetExtension(path)),
                excludeDirectoryExtensions, overwrite);
        }

        public static void CopyDirectory(string srcPath, string dstPath, string[] excludeFileExtensions,
            Func<string, bool> filterDirectory, bool overwrite = true)
        {
            CopyDirectory(srcPath, dstPath,
                path => excludeFileExtensions == null || !excludeFileExtensions.Contains(Path.GetExtension(path)),
                filterDirectory, overwrite);
        }

        public static void CopyDirectory(string srcPath, string dstPath, Func<string, bool> filterFile,
            string[] excludeDirectoryExtensions, bool overwrite = true)
        {
            CopyDirectory(srcPath, dstPath, filterFile,
                path =>
                    excludeDirectoryExtensions == null ||
                    !excludeDirectoryExtensions.Contains(Path.GetExtension(path)), overwrite);
        }

        public static void CopyDirectory(string srcPath, string dstPath, Func<string, bool> filterFile,
            Func<string, bool> filterDirectory = null, bool overwrite = true)
        {
            if (!Directory.Exists(dstPath))
                Directory.CreateDirectory(dstPath);

            IEnumerable<string> files = Directory.GetFiles(srcPath, "*", SearchOption.TopDirectoryOnly);
            if (filterFile != null)
                files = files.Where(filterFile);

            foreach (var file in files)
            {
                File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)), overwrite);
            }

            IEnumerable<string> directories = Directory.GetDirectories(srcPath, "*", SearchOption.TopDirectoryOnly);
            if (filterDirectory != null)
                directories = directories.Where(filterDirectory);

            foreach (var srcDirectory in directories)
            {
                var srcDirectoryName = Path.GetFileName(srcDirectory);
                if (srcDirectory.Contains(".svn"))
                    continue;

                var dstDirectory = Path.Combine(dstPath, srcDirectoryName);
                CopyDirectory(srcDirectory, dstDirectory, filterFile, filterDirectory, overwrite);

                if (Directory.GetFiles(dstDirectory, "*", SearchOption.TopDirectoryOnly).Length == 0 &&
                    Directory.GetDirectories(dstDirectory, "*", SearchOption.TopDirectoryOnly).Length == 0)
                    Directory.Delete(dstDirectory);
            }
        }

        public static List<string> GetFilesName(string path, string[] includeExtensions)
        {
            return
                GetFiles(path, file => includeExtensions != null && includeExtensions.Contains(Path.GetExtension(file)))
                    .Select((item) => Path.GetFileName(item))
                    .ToList();
        }

        public static List<string> GetFilesName(string path, Func<string, bool> filter = null)
        {
            return GetFiles(path, filter).Select((item) => Path.GetFileName(item)).ToList();
        }

        public static List<string> GetFiles(string path, string[] includeExtensions)
        {
            return GetFiles(path,
                file => includeExtensions != null && includeExtensions.Contains(Path.GetExtension(file)));
        }

        public static List<string> GetFiles(string path, Func<string, bool> filter = null)
        {
            if (!Directory.Exists(path))
                return null;

            IEnumerable<string> files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);

            if (filter != null)
                files = files.Where(filter).ToList();

            return files.ToList();
        }

        public static List<string> GetDirectoriesName(string path, Func<string, bool> filter = null)
        {
            return GetDirectories(path, filter).Select((item) => Path.GetFileName(item)).ToList();
        }

        public static List<string> GetDirectories(string path, Func<string, bool> filter = null)
        {
            if (!Directory.Exists(path))
                return null;

            IEnumerable<string> directories = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);

            if (filter != null)
                directories = directories.Where(filter).ToList();

            return directories.ToList();
        }

        public static void DeleteFile(string path)
        {
            if (!IsFileExists(path))
                return;

            if (Application.isEditor)
                File.Delete(path);
            //FileTools.DeleteFileOrDirectory(path);
            else
                File.Delete(path);
        }

        public static void DeleteDirectory(string path, bool recursive = false)
        {
            if (!IsDirectoryExists(path))
                return;

            //if (recursive)
            //{
            //    var directories = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            //    foreach (var directory in directories)
            //    {
            //        DeleteDirectory(directory, true);
            //    }
            //}

            //var files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            //foreach (var file in files)
            //{
            //    DeleteFile(file);
            //}

            if (Application.isEditor)
                Directory.Delete(path, recursive);
            //FileTools.DeleteFileOrDirectory(path);
            else
                Directory.Delete(path, recursive);
        }

        public static bool IsFileExists(string path)
        {
            return File.Exists(path);
        }

        public static bool IsDirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public static string GetFullPathWithoutExtension(string path)
        {
            return Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path);
        }

        public static int GetFileSize(string path)
        {
            if (!IsFileExists(path))
                return 0;

            FileStream fs = new FileStream(path, FileMode.Open);
            long length = fs.Length;
            fs.Close();

            return (int) length;
        }

        public static byte[] GetBytes(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        public static string GetMD5HashFromFile(string path)
        {
            if (!File.Exists(path))
                return null;

            FileStream file = new FileStream(path, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] computeHash = md5.ComputeHash(file);

            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < computeHash.Length; i++)
            {
                sb.Append(computeHash[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static string GetMD5Hash(string text)
        {
            return GetMD5Hash(GetBytes(text));
        }

        public static string GetMD5Hash(byte[] buffer)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] computeHash = md5.ComputeHash(buffer);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < computeHash.Length; i++)
            {
                sb.Append(computeHash[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}