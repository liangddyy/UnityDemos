/*************************************************************************
 *  Copyright (C), 2015-2016, Mogoson tech. Co., Ltd.
 *  FileName: BinaryFileOperater.cs
 *  Author: Mogoson   Version: 1.0   Date: 10/24/2015
 *  Version Description:
 *    Internal develop version,mainly to achieve its function.
 *  File Description:
 *    Ignore description.
 *  Class List:
 *    <ID>           <name>             <description>
 *     1.  BinaryFileOperater    Ignore description.
 *  Function List:
 *    <class ID>     <name>             <description>
 *     1.
 *  History:
 *    <ID>    <author>      <time>      <version>      <description>
 *     1.     Mogoson     10/24/2015       1.0        Build this file.
 *************************************************************************/

namespace Developer.File
{
    using System;
    using System.IO;

    /// <summary>
    /// Binary file operater.
    /// </summary>
    public static class BinaryFileOperater
    {
        #region Public Static Method
        /// <summary>
        /// Read the index page of file.
        /// </summary>
        /// <returns>Page byte array.</returns>
        public static byte[] ReadIndexPage(string fileName, int pageSize = 65536, int pageIndex = 0, int readPageCount = 1)
        {
            bool fileExists = File.Exists(fileName);
            if (!fileExists || pageSize <= 0 || pageIndex < 0 || readPageCount < 0)
                return null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                if (!fs.CanRead || !fs.CanSeek)
                    return null;
                int pageCount = fs.Length / pageSize + fs.Length % pageSize > 0 ? 1 : 0;
                if (pageIndex > pageCount)
                    return null;
                int arrayLength = pageSize * readPageCount;
                if (readPageCount > pageCount - pageIndex || readPageCount == 0)
                {
                    readPageCount = pageCount - pageIndex;
                    arrayLength = (int)(fs.Length - pageIndex * pageSize);
                }
                byte[] pageByteArray = new byte[arrayLength];
                int readCount = 0;
                do
                {
                    pageIndex += readCount;
                    long start = pageSize * pageIndex;
                    long end = 0;
                    if (pageIndex < pageCount - 1)
                        end = start + pageSize;
                    else
                        end = fs.Length;
                    int byteCount = (int)(end - start);
                    byte[] readByteArray = new byte[byteCount];
                    fs.Seek(start, SeekOrigin.Begin);
                    fs.Read(readByteArray, 0, byteCount);
                    Buffer.BlockCopy(readByteArray, 0, pageByteArray, readCount * byteCount, byteCount);
                    readCount++;
                }
                while (readCount < readPageCount);
                return pageByteArray;
            }
        }//ReadI...()_end
        #endregion
    } 
} 
