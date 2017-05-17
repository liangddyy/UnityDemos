using System;
using UnityEngine;
using System.Collections;

public class GameStatus
{
    public string gameName;
    public string version;
    public bool isStereo;
    public bool isUseHardWare;
    public refencenes[] statusList;

    public String ToString()
    {
        return "名字：" + gameName + "版本:" + version;
    }
}

[Serializable]
public class refencenes
{
    public string name;
    public int id;
}
