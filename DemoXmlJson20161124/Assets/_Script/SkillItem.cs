using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
/// <summary>  
/// 数据
/// </summary>  
public class SkillItem
{
    public int _SkillId;
    public string _SkillName;
    public int SkillId
    {
        get { return _SkillId; }
        set { _SkillId = value; }
    }
    public string SkillName
    {
        get { return _SkillName; }
        set { _SkillName = value; }
    }
}