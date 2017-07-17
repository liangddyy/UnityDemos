#if ENABLE_IOS_ON_DEMAND_RESOURCES || ENABLE_IOS_APP_SLICING
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS; 
using System.Collections;
using System.IO;

/// <summary>
/// 对资源进行标识赋值的编辑器脚本
/// </summary>
public class BuildResources
{
    [InitializeOnLoadMethod]
    static void SetupResourcesBuild()
    {
        UnityEditor.iOS.BuildPipeline.collectResources += CollectResources;
    }

    static string GetPath(string relativePath)
    {
        string root = Path.Combine(AssetBundles.Utility.AssetBundlesOutputPath, 
                                   AssetBundles.Utility.GetPlatformName());
        return Path.Combine(root, relativePath);
    }
 
    static UnityEditor.iOS.Resource[] CollectResources()
    {
        string manifest = AssetBundles.Utility.GetPlatformName();
        return new Resource[]
        {
            new Resource(manifest, GetPath(manifest)).AddOnDemandResourceTags(manifest),
//            new Resource("scene-bundle", GetPath("scene-bundle")).AddOnDemandResourceTags("scene-bundle"),
//            new Resource("cube-bundle", GetPath("cube-bundle")).AddOnDemandResourceTags("cube-bundle"),
//            new Resource("material-bundle", GetPath("material-bundle")).AddOnDemandResourceTags("material-bundle"),
//            
//            new Resource("variants/variant-scene", GetPath("variants/variant-scene")).AddOnDemandResourceTags("variants>variant-scene"),
//            // 资源分割
//            new Resource("variants/myassets").BindVariant(GetPath("variants/myassets.hd"), "hd")
//                                             .BindVariant(GetPath("variants/myassets.sd"), "sd")
//                                             .AddOnDemandResourceTags("variants>myassets"),

            new Resource("v/mycube").BindVariant(GetPath("v/mycube.hd"), "hd")
                .BindVariant(GetPath("v/mycube.sd"), "sd")
                .AddOnDemandResourceTags("v>mycube")


//            new Resource("banner", GetPath("banner.english")).AddOnDemandResourceTags("banner"),
//            new Resource("tanks-scene-bundle", GetPath("tanks-scene-bundle")).AddOnDemandResourceTags("tanks-scene-bundle"),
//            new Resource("tanks-albedo", GetPath("tanks-albedo.normal-sd")).AddOnDemandResourceTags("tanks-albedo")
        };
    }
}
#endif