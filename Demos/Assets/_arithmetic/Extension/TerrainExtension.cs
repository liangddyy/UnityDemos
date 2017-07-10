/*************************************************************************
 *  Copyright (C), 2016-2017, Mogoson tech. Co., Ltd.
 *  FileName: TerrainExtension.cs
 *  Author: Mogoson   Version: 1.0   Date: 4/29/2016
 *  Version Description:
 *    Internal develop version,mainly to achieve its function.
 *  File Description:
 *    Ignore.
 *  Class List:
 *    <ID>           <name>             <description>
 *     1.         TerrainExtension             Ignore.
 *  Function List:
 *    <class ID>     <name>             <description>
 *     1.
 *  History:
 *    <ID>    <author>      <time>      <version>      <description>
 *     1.     Mogoson     4/29/2016       1.0        Build this file.
 *************************************************************************/

namespace Developer.Terrain
{
    using UnityEngine;

    /// <summary>
    /// Extension of terrain.
    /// </summary>
    public static class TerrainExtension
    {
        #region Public Method
        /// <summary>
        /// Normalize position relative to terrain.
        /// </summary>
        /// <param name="terrain">Base terrain.</param>
        /// <param name="woldPos">Position in wold space.</param>
        /// <returns>Normalize position.</returns>
        public static Vector3 NormalizeRelativePosition(this Terrain terrain, Vector3 woldPos)
        {
            var coord = woldPos - terrain.transform.position;
            return new Vector3(coord.x / terrain.terrainData.size.x, coord.y / terrain.terrainData.size.y, coord.z / terrain.terrainData.size.z);
        }//NormalizeR...()_end

        /// <summary>
        /// Position relative to terrain map.
        /// </summary>
        /// <param name="terrain">Base terrain.</param>
        /// <param name="mapSize">Map size(x is width, z is height).</param>
        /// <param name="normalizePos">Normalize position relative to terrain.</param>
        /// <returns>Relative position.</returns>
        public static Vector3 MapRelativePosition(this Terrain terrain, Vector3 mapSize, Vector3 normalizePos)
        {
            return new Vector3(normalizePos.x * mapSize.x, 0, normalizePos.z * mapSize.z);
        }//MapR...()_end
        #endregion
    } 
} 
