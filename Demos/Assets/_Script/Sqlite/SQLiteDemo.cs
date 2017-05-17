using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.IO;
using Liangddyy.Util;

/// <summary>
/// 测试用
/// </summary>
public class SQLiteDemo : MonoBehaviour
{
    //private SQLiteHelper sql;
    private DbUtil dbUtil; //自定义
    //private DbUtil dbUtil1;


    List<Pen> penList = new List<Pen>();
    List<Person> personList = new List<Person>();

    void Start()
    {
        dbUtil = DbUtil.getInstance();

        for (int i = 5; i < 100000; i++)
        {
            Pen pen = new Pen() {Id = i, name = "钢笔" + i, color = "red", size = 1.123};
            Person person = new Person() {Id = i, Name = "liang" + i, Email = "你好"};

            personList.Add(person);
            penList.Add(pen);
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 150, 80, 50), "建表"))
        {
            CreateTable();
        }
        if (GUI.Button(new Rect(200, 150, 80, 50), "插入One"))
        {
            InsertOne();
        }
        if (GUI.Button(new Rect(300, 150, 80, 50), "插入链表"))
        {
            StartCoroutine(InsertAll());
        }

        if (GUI.Button(new Rect(400, 150, 80, 50), "更新One"))
        {
            UpdateOne();
        }
    }

    private void CreateTable()
    {
        dbUtil.CreateTable(new Person());
        dbUtil.CreateTable(new Pen());
    }

    private void InsertOne()
    {
        dbUtil.Insert(new Pen() {Id = 1, name = "钢笔1", color = "red", size = 1.123}); //插入
        dbUtil.Insert(new Person() {Id = 1, Name = "liangddyy", Email = "hehe"});
    }

    IEnumerator InsertAll()
    {
        yield return new WaitForSeconds(1);

        dbUtil.InsertList(penList); //插入链表
        dbUtil.InsertList(personList);

        Debug.Log("完成");
    }

    private void UpdateOne()
    {
        Person person = new Person() {Id = 1, Name = "梁先生", Email = "这条记录被修改了哈哈"};

        dbUtil.Update(person);
    }

    void test()
    {
        /*
//创建名为sqlite4unity的数据库
        sql = new SQLiteHelper("data source=sqlite4unity.db");


//创建名为table1的数据表
        sql.CreateTable("table1", new string[] {"ID", "Name", "Age", "Email"},
            new string[] {"INTEGER", "TEXT", "INTEGER", "TEXT"});

//插入两条数据
        sql.InsertValues("table1", new string[] {"'1'", "'张三'", "'22'", "'Zhang3@163.com'"});
        sql.InsertValues("table1", new string[] {"'2'", "'李四'", "'25'", "'Li4@163.com'"});

//更新数据，将Name="张三"的记录中的Name改为"Zhang3"
        sql.UpdateValues("table1", new string[] {"Name"}, new string[] {"'Zhang3'"}, "Name", "=", "'张三'");

//插入3条数据
        sql.InsertValues("table1", new string[] {"3", "'王五'", "25", "'Wang5@163.com'"});
        sql.InsertValues("table1", new string[] {"4", "'王五'", "26", "'Wang5@163.com'"});
        sql.InsertValues("table1", new string[] {"5", "'王五'", "27", "'Wang5@163.com'"});

//删除Name="王五"且Age=26的记录,DeleteValuesOR方法类似
        sql.DeleteValuesAND("table1", new string[] {"Name", "Age"}, new string[] {"=", "="},
            new string[] {"'王五'", "'26'"});

//读取整张表
        SqliteDataReader reader = sql.ReadFullTable("table1");
        while (reader.Read())
        {
            //读取ID
            Debug.Log(reader.GetInt32(reader.GetOrdinal("ID")));
            //读取Name
            Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
            //读取Age
            Debug.Log(reader.GetInt32(reader.GetOrdinal("Age")));
            //读取Email
            Debug.Log(reader.GetString(reader.GetOrdinal("Email")));
        }

//读取数据表中Age>=25的所有记录的ID和Name
        reader = sql.ReadTable("table1", new string[] {"ID", "Name"}, new string[] {"Age"}, new string[] {">="},
            new string[] {"'25'"});
        while (reader.Read())
        {
            //读取ID
            Debug.Log(reader.GetInt32(reader.GetOrdinal("ID")));
            //读取Name
            Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
        }

//自定义SQL,删除数据表中所有Name="王五"的记录
        sql.ExecuteQuery("DELETE FROM table1 WHERE NAME='王五'");

//关闭数据库连接
        sql.CloseConnection();


        sql.InsertValues("table1", new string[] {"'1'", "'张三'", "'22'", "'Zhang3@163.com'"});*/
    }

    void OnDestroy()
    {
        dbUtil.CloseConnection();
    }
}

public class BaseEntity
{
    private int id;

    public int Id
    {
        get { return id; }

        set { id = value; }
    }
}

public class Pen : BaseEntity
{
    public string name;
    public string color;
    public double size;
}

public class Person : BaseEntity
{
    private string name;
    private string email;

    public string Name
    {
        get { return name; }

        set { name = value; }
    }

    public string Email
    {
        get { return email; }

        set { email = value; }
    }
}