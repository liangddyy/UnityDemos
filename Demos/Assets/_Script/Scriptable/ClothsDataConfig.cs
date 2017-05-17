using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// ScriptableObject使用示例
/// 便捷的创建 .asset
/// </summary>
[CreateAssetMenu(fileName = "ClothsData", menuName = "创建Scriptable/ClothsData", order = 100)]
public class ClothsDataConfig : ScriptableObject
{
    public ClothsData[] data;
}

[Serializable]
public class ClothsData
{
    public ClothsType clothsType;
    public string path;
}
public enum ClothsType
{
    BeiLeiMao,//贝雷帽
    ChuanXingMao,//船形帽
    BangQiuMao,//棒球帽
    JingXiao,//警哨
    ShouKao,//手铐
    JingFuTong,//警务通
    DuiJiangJi,//对讲机
    FangGeShouTao,//防割手套
    MinJingFu,//民警服
    XingJingFu,//刑警服
    JiaoJingFu
}
