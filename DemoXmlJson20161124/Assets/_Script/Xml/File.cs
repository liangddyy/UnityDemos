#if UNITY_WEBPLAYER

using System.IO;
using System.Text;
using System;

public static class File
{
    public static void AppendAllText(string path, string contents)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.AppendAllText(path, contents);
#endif
    }

    public static void AppendAllText(string path, string contents, Encoding encoding)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.AppendAllText(path, contents, encoding);
#endif
    }

    public static StreamWriter AppendText(string path)
    {
#if !UNITY_WEBPLAYER
        return System.IO.File.AppendText(path);
#else
        return null;
#endif
    }

    public static void Copy(string sourceFileName, string destFileName)
    {
        System.IO.File.Copy(sourceFileName, destFileName);
    }

    public static void Copy(string sourceFileName, string destFileName, bool overwrite)
    {
        System.IO.File.Copy(sourceFileName, destFileName, overwrite);
    }

    public static FileStream Create(string path)
    {
        return System.IO.File.Create(path);
    }

    public static FileStream Create(string path, int bufferSize)
    {
        return System.IO.File.Create(path, bufferSize);
    }

    public static StreamWriter CreateText(string path)
    {
#if !UNITY_WEBPLAYER
        return System.IO.File.CreateText(path);
#else
        return null;
#endif
    }

    //[MonoLimitationAttribute("System.IO.File encryption isn't supported (even on NTFS).")]
    public static void Decrypt(string path)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.Decrypt(path);
#endif
    }

    public static void Delete(string path)
    {
        System.IO.File.Delete(path);
    }

    //[MonoLimitationAttribute("System.IO.File encryption isn't supported (even on NTFS).")]
    public static void Encrypt(string path)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.Encrypt(path);
#endif
    }

    public static bool Exists(string path)
    {
        return System.IO.File.Exists(path);
    }

    public static FileAttributes GetAttributes(string path)
    {
        return System.IO.File.GetAttributes(path);
    }

    public static DateTime GetCreationTime(string path)
    {
#if !UNITY_WEBPLAYER
        return System.IO.File.GetCreationTime(path);
#else
        return DateTime.Now;
#endif
    }

    public static DateTime GetCreationTimeUtc(string path)
    {
#if !UNITY_WEBPLAYER
        return System.IO.File.GetCreationTimeUtc(path);
#else
        return DateTime.Now;
#endif
    }

    public static DateTime GetLastAccessTime(string path)
    {
#if !UNITY_WEBPLAYER
        return System.IO.File.GetLastAccessTime(path);
#else
        return DateTime.Now;
#endif
    }

    public static DateTime GetLastAccessTimeUtc(string path)
    {
#if !UNITY_WEBPLAYER
        return System.IO.File.GetLastAccessTimeUtc(path);
#else
        return DateTime.Now;
#endif
    }

    public static DateTime GetLastWriteTime(string path)
    {
#if !UNITY_WEBPLAYER
        return System.IO.File.GetLastWriteTime(path);
#else
        return DateTime.Now;
#endif
    }

    public static DateTime GetLastWriteTimeUtc(string path)
    {
#if !UNITY_WEBPLAYER
        return System.IO.File.GetLastWriteTimeUtc(path);
#else
        return DateTime.Now;
#endif
    }

    public static void Move(string sourceFileName, string destFileName)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.Move(sourceFileName, destFileName);
#endif
    }

    public static FileStream Open(string path, FileMode mode)
    {
        return System.IO.File.Open(path, mode);
    }

    public static FileStream Open(string path, FileMode mode, FileAccess access)
    {
#if !UNITY_WEBPLAYER
        return System.IO.File.Open(path, mode, access);
#else
        return null;
#endif
    }

    public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
    {
        return System.IO.File.Open(path, mode, access, share);
    }

    public static FileStream OpenRead(string path)
    {
        return System.IO.File.OpenRead(path);
    }

    public static StreamReader OpenText(string path)
    {
        return System.IO.File.OpenText(path);
    }

    public static FileStream OpenWrite(string path)
    {
        return System.IO.File.OpenWrite(path);
    }

    public static byte[] ReadAllBytes(string path)
    {
#if !UNITY_WEBPLAYER
        return System.IO.File.ReadAllBytes(path);
#else
        return null;
#endif
    }

    public static string[] ReadAllLines(string path)
    {
        return System.IO.File.ReadAllLines(path);
    }

    public static string[] ReadAllLines(string path, Encoding encoding)
    {
#if !UNITY_WEBPLAYER
        return System.IO.File.ReadAllLines(path, encoding);
#else
        return null;
#endif
    }

    public static string ReadAllText(string path)
    {
#if !UNITY_WEBPLAYER
        return System.IO.File.ReadAllText(path);
#else
        return null;
#endif
    }

    public static string ReadAllText(string path, Encoding encoding)
    {
#if !UNITY_WEBPLAYER
        return System.IO.File.ReadAllText(path, encoding);
#else
        return null;
#endif
    }

    public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.Replace(sourceFileName, destinationFileName, destinationBackupFileName);
#endif
    }

    public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.Replace(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
#endif
    }

    public static void SetAttributes(string path, FileAttributes fileAttributes)
    {
        System.IO.File.SetAttributes(path, fileAttributes);
    }

    public static void SetCreationTime(string path, DateTime creationTime)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.SetCreationTime(path, creationTime);
#endif
    }

    public static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.SetCreationTimeUtc(path, creationTimeUtc);
#endif
    }

    public static void SetLastAccessTime(string path, DateTime lastAccessTime)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.SetLastAccessTime(path, lastAccessTime);
#endif
    }

    public static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
#endif
    }

    public static void SetLastWriteTime(string path, DateTime lastWriteTime)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.SetLastWriteTime(path, lastWriteTime);
#endif
    }

    public static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
#endif
    }

    public static void WriteAllBytes(string path, byte[] bytes)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.WriteAllBytes(path, bytes);
#endif
    }

    public static void WriteAllLines(string path, string[] contents)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.WriteAllLines(path, contents);
#endif
    }

    public static void WriteAllLines(string path, string[] contents, Encoding encoding)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.WriteAllLines(path, contents, encoding);
#endif
    }

    public static void WriteAllText(string path, string contents)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.WriteAllText(path, contents);
#endif
    }

    public static void WriteAllText(string path, string contents, Encoding encoding)
    {
#if !UNITY_WEBPLAYER
        System.IO.File.WriteAllText(path, contents, encoding);
#endif
    }
}

#endif