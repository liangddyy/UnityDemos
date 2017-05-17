using UnityEngine;
using UnityEditor;

public class UtilityEditor : ScriptableObject
{
    [MenuItem("Tools/快捷工具/DeleteAllData")]
    static void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
    }
}