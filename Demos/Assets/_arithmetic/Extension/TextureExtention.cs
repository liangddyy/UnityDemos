/***************************************************************************************
 *  Copyright (C), 2015-2016, Mogoson tech. Co., Ltd.
 *  FileName: TextureExtention.cs
 *  Author: Mogoson   Version: 1.0   Date: 10/24/2015
 *  Version Description:
 *    Internal develop version,mainly to achieve its function.
 *  File Description:
 *    Ignore description.
 *  Class List:
 *    <ID>           <name>             <description>
 *     1.      Texture2DExtention    Extend Texture2D's functions.
 *  Function List:
 *    <class ID>     <name>             <description>
 *     1.
 *  History:
 *    <ID>    <author>      <time>      <version>      <description>
 *     1.     Mogoson     10/24/2015       1.0        Build this file.
 ***************************************************************************************/

namespace Developer.Texture2D
{
    using Converter;
    using UnityEngine;

    /// <summary>
    /// Extend Texture2D's functions.
    /// </summary>
    public static class Texture2DExtention
	{
		#region Public Static Method
		/// <summary>
		/// Update Texture2D's Pixels.
		/// </summary>
		public static void UpdatePixels(this Texture2D texture2D, Color[] colorArray, int mipLevel = 0, bool updateMipmaps = false, bool makeNointerReadable = false)
		{
			if (colorArray == null || colorArray.Length != texture2D.width * texture2D.height)
				return;
			texture2D.SetPixels(colorArray, mipLevel);
			texture2D.Apply(updateMipmaps, makeNointerReadable);
		}//UpdateP...()_end
		
		/// <summary>
		/// Update Texture2D's Pixels.
		/// </summary>
		public static void UpdatePixels(this Texture2D texture2D, Color[,] colorArray, int mipLevel = 0, bool updateMipmaps = false, bool makeNointerReadable = false)
		{
			if (colorArray == null || colorArray.Length != texture2D.width * texture2D.height)
				return;
			Color[] cArray = ArrayConverter.ToOneDimentionArray(colorArray);
			UpdatePixels(texture2D, cArray, mipLevel, updateMipmaps, makeNointerReadable);
		}//UpdateP...()_end
		
		/// <summary>
		/// Update Texture2D's Pixels.
		/// </summary>
		public static void UpdatePixels(this Texture2D texture2D, Color32[] colorArray, int mipLevel = 0, bool updateMipmaps = false, bool makeNointerReadable = false)
		{
			if (colorArray == null || colorArray.Length != texture2D.width * texture2D.height)
				return;
			texture2D.SetPixels32(colorArray, mipLevel);
			texture2D.Apply(updateMipmaps, makeNointerReadable);
		}//UpdateP...()_end
		
		/// <summary>
		/// Update Texture2D's Pixels.
		/// </summary>
		public static void UpdatePixels(this Texture2D texture2D, Color32[,] colorArray, int mipLevel = 0, bool updateMipmaps = false, bool makeNointerReadable = false)
		{
			if (colorArray == null || colorArray.Length != texture2D.width * texture2D.height)
				return;
			Color32[] cArray = ArrayConverter.ToOneDimentionArray(colorArray);
			UpdatePixels(texture2D, cArray, mipLevel, updateMipmaps, makeNointerReadable);
		}//UpdateP...()_end
		#endregion
	} 
} 
