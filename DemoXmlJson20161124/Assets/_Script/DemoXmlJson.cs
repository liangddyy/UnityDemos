using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Xml;

public class DemoXmlJson : MonoBehaviour
{

    
    // Use this for initialization
    
    IEnumerator Start()
    {

        // 资源读取
        WWW www = new WWW("file://" + Application.dataPath + "/GameData.assetbundle");
        yield return www;
        //转换资源为Test，这个test对象将拥有原来在编辑器中设置的数据。  
        GameDataScriptable test = www.assetBundle.mainAsset as GameDataScriptable;
        Debug.Log("技能1的Id:" + test.SkillDictionary[0].SkillId);

        
    }

    // Update is called once per frame
    void Update()
    { 
    }
    

    public void MyClick()
    {
        GameStatus gameStatus = ResolveJson();
        if (gameStatus != null)
        {
            Debug.Log(gameStatus.ToString());
        }
    }


    public void ResolveXml()
    {
        // 本地文件
        //string url = Application.dataPath + "/Resources/Test.xml";
        // 
        //String url = "http://539go.com/index.php/feed/";

        //        WWW www = new WWW("http://539go.com/index.php/feed/");
        //
        //
        //        XmlDocument XmlDoc = new XmlDocument();
        //        XmlDoc.Load(url);
        //
        //        XmlNodeList nodeList = XmlDoc.SelectSingleNode("Xml").ChildNodes;
        //
        //        if (nodeList == null)
        //        {
        //            Debug.Log("空");
        //            return;
        //        }
        //        foreach (XmlElement xe in nodeList)
        //        {
        //            foreach (XmlElement xxe in xe.ChildNodes)
        //            {
        //                if (xxe.Name == "name")
        //                {
        //                    Debug.Log("名字:" + xxe.InnerText);
        //                }
        //            }
        //        }

        string url = Application.dataPath + "/Resources/Test1.xml";
        AssetInfo assetInfo = new AssetInfo();
        assetInfo.name = "liang";
        assetInfo.sex = "男";

        XmlUtility.SerializeXmlToFile(url,assetInfo);

//        var xmlData = XmlUtility.DeserializeXmlFromFile<List<AssetInfo>>(url);
//
//        if (xmlData != null)
//        {
//            foreach (AssetInfo value in xmlData)
//            {
//                Debug.Log(value.name);
//
//            }
//
//        }

    }

    public GameStatus ResolveJson()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (!File.Exists(Application.dataPath + "/Resources/Test.json"))
        {
            return null;
        }

        ArrayList arrayList = new ArrayList();


        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/Test.json");

        if (sr == null)
        {
            return null;
        }
        string json = sr.ReadToEnd();

        if (json.Length > 0)
        {
            return JsonUtility.FromJson<GameStatus>(json);
        }

        return null;
    }
}