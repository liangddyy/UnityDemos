using UnityEngine;
using System.Collections;
using UnityEditor;
/// <summary>
/// 梁sir
/// </summary>
public class ToolsMenu : Editor
{
    [MenuItem("打包工具/测试打包")]
    public static void BuildConfigData()
    {
        Caching.CleanCache();
        //通过ScriptableObject创建一个对象的实例  
        GameDataScriptable gs = ScriptableObject.CreateInstance<GameDataScriptable>();
        //对对象的属性进行赋值  
        for (int i = 1; i < 3; i++)
        {
            SkillItem item = new SkillItem();
            item.SkillId = i;
            item.SkillName = "技能" + i;
            gs.SkillDictionary.Add(item);
        }
        //创建资源文件（可以在编辑器下直接编辑数据）  
        AssetDatabase.CreateAsset(gs, "Assets/GameDataConfig.asset");
        //将资源反序列化为一个对象  
        Object o = AssetDatabase.LoadAssetAtPath("Assets/GameDataConfig.asset", typeof(GameDataScriptable));
        string targetPath = "Assets/GameData.assetbundle";

        //assetbundle文件只能在运行时加载，并且只能通过WWW加载  
        BuildPipeline.BuildAssetBundle(o, null, targetPath);

       
        
    }
}
