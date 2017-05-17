using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
/// <summary>  
/// 继承ScriptableObject
/// </summary>  
[CreateAssetMenu(fileName = "GameDataScriptable", menuName = "创建Scriptable示例/GameDataScriptable", order = 100)]
public class GameDataScriptable : ScriptableObject
{
    /// <summary>  
    /// 
    /// </summary>  
    public List<SkillItem> _SkillDictionary = new List<SkillItem>();
    public List<SkillItem> SkillDictionary
    {
        get { return _SkillDictionary; }
        set { _SkillDictionary = value; }
    }
}
