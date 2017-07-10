/*************************************************************************
 *  Copyright (C), 2017-2018, Mogoson tech. Co., Ltd.
 *  FileName: MeshExtension.cs
 *  Author: Mogoson   Version: 1.0   Date: 5/24/2017
 *  Version Description:
 *    Internal develop version,mainly to achieve its function.
 *  File Description:
 *    Ignore.
 *  Class List:
 *    <ID>           <name>             <description>
 *     1.         MeshExtension            Ignore.
 *  Function List:
 *    <class ID>     <name>             <description>
 *     1.
 *  History:
 *    <ID>    <author>      <time>      <version>      <description>
 *     1.     Mogoson     5/24/2017       1.0        Build this file.
 *************************************************************************/

namespace Developer.Mesh
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class DMesh
    {
        #region Public Method
        /// <summary>
        /// Combine the children meshes of meshesRoot to meshSave.
        /// </summary>
        /// <param name="meshesRoot">Root of multi meshes.</param>
        /// <param name="meshSave">Save combine mesh.</param>
        public static void MultiCombine(GameObject meshesRoot, GameObject meshSave)
        {
            //Combine meshes that in meshesRoot's children to a new mesh.
            var meshFilters = meshesRoot.GetComponentsInChildren<MeshFilter>();
            var combines = new CombineInstance[meshFilters.Length];
            var materialList = new List<Material>();
            for (int i = 0; i < meshFilters.Length; i++)
            {
                combines[i].mesh = meshFilters[i].sharedMesh;
                combines[i].transform = Matrix4x4.TRS(meshFilters[i].transform.position - meshesRoot.transform.position,
                    meshFilters[i].transform.rotation, meshFilters[i].transform.lossyScale);
                var materials = meshFilters[i].GetComponent<MeshRenderer>().sharedMaterials;
                foreach (var material in materials)
                {
                    materialList.Add(material);
                }//foreach()_end
            }//for()_end
            var newMesh = new Mesh();
            newMesh.CombineMeshes(combines, false);
            newMesh.Optimize();

            //Add the new mesh to the meshSave.
            meshSave.AddComponent<MeshFilter>().sharedMesh = newMesh;
            meshSave.AddComponent<MeshCollider>().sharedMesh = newMesh;
            meshSave.AddComponent<MeshRenderer>().sharedMaterials = materialList.ToArray();
        }//MultiCombine()_end
        #endregion
    } 
} 
