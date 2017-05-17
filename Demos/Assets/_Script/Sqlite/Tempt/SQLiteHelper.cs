using System;
using Mono.Data.Sqlite;
using UnityEngine;
/// <summary>
/// Sqlite简单帮助类
/// 参考 http://blog.csdn.net/qinyuanpei/article/details/46812655#t1
/// </summary>
public class SQLiteHelper
{
    private SqliteDataReader dbReader;

    /// <summary>
    ///     SQL命令定义
    /// </summary>
    private SqliteCommand dbCmd;

    /// <summary>
    ///     数据库连接定义
    /// </summary>
    private SqliteConnection dbConnection;

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="connectionString">数据库连接字符串</param>
    public SQLiteHelper(string connectionString)
    {
        try
        {
            //构造数据库连接
            dbConnection = new SqliteConnection(connectionString);
            //打开数据库
            dbConnection.Open();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }


    /// <summary>
    ///     执行SQL命令
    /// </summary>
    /// <returns>The query.</returns>
    /// <param name="queryString">SQL命令字符串</param>
    public SqliteDataReader ExecuteQuery(string queryString)
    {
        dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = queryString;
        
        dbReader = dbCmd.ExecuteReader();

        return dbReader;
    }

    /// <summary>
    ///     关闭数据库连接 销毁连接
    /// </summary>
    public void CloseConnection()
    {
        //销毁Command
        if (dbCmd != null)
            dbCmd.Cancel();
        dbCmd = null;

        //销毁Reader
        if (dbReader != null)
            dbReader.Close();
        dbReader = null;

        //销毁Connection
        if (dbConnection != null)
            dbConnection.Close();
        dbConnection = null;
    }

    /// <summary>
    ///     读取整张数据表
    /// </summary>
    /// <returns>The full table.</returns>
    /// <param name="tableName">数据表名称</param>
    public SqliteDataReader ReadFullTable(string tableName)
    {
        var queryString = "SELECT * FROM " + tableName;
        return ExecuteQuery(queryString);
    }

    /// <summary>
    ///     向指定数据表中插入数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="values">插入的数值</param>
    public SqliteDataReader InsertValues(string tableName, string[] values)
    {
        
        // 获取数据表中字段数目
        var fieldCount = ReadFullTable(tableName).FieldCount;
        // 当插入的数据长度不等于字段数目时引发异常(字段数目不匹配)
        if (values.Length != fieldCount)
            throw new SqliteException("values.Length!=fieldCount");

        var queryString = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for (var i = 1; i < values.Length; i++)
            queryString += ", " + values[i];
        queryString += " )";
        return ExecuteQuery(queryString);
    }

    /// <summary>
    ///     更新指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    /// <param name="key">关键字</param>
    /// <param name="value">关键字对应的值</param>
    public SqliteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string key,
        string operation, string value)
    {
        //当字段名称和字段数值不对应时引发异常
        if (colNames.Length != colValues.Length)
            throw new SqliteException("colNames.Length!=colValues.Length");

        var queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + colValues[0];
        for (var i = 1; i < colValues.Length; i++)
            queryString += ", " + colNames[i] + "=" + colValues[i];
        queryString += " WHERE " + key + operation + value;
        return ExecuteQuery(queryString);
    }


    /// <summary>
    ///     删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    public SqliteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] operations, string[] colValues)
    {
        //当字段名称和字段数值不对应时引发异常
        if ((colNames.Length != colValues.Length) || (operations.Length != colNames.Length) ||
            (operations.Length != colValues.Length))
            throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");

        var queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
        for (var i = 1; i < colValues.Length; i++)
            queryString += "OR " + colNames[i] + operations[0] + colValues[i];
        return ExecuteQuery(queryString);
    }

    /// <summary>
    ///     删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    public SqliteDataReader DeleteValuesAND(string tableName, string[] colNames, string[] operations, string[] colValues)
    {
        //当字段名称和字段数值不对应时引发异常
        if ((colNames.Length != colValues.Length) || (operations.Length != colNames.Length) ||
            (operations.Length != colValues.Length))
            throw new SqliteException(
                "colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");

        var queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
        for (var i = 1; i < colValues.Length; i++)
            queryString += " AND " + colNames[i] + operations[i] + colValues[i];
        return ExecuteQuery(queryString);
    }

    /// <summary>
    ///     创建数据表
    /// </summary>
    /// +
    /// <returns>The table.</returns>
    /// <param name="tableName">数据表名</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colTypes">字段名类型</param>
    public SqliteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
    {
        
        var queryString = "create table if not exists " + tableName + "( " + colNames[0] + " " + colTypes[0];
        for (var i = 1; i < colNames.Length; i++)
            queryString += ", " + colNames[i] + " " + colTypes[i];
        queryString += "  ) ";
        return ExecuteQuery(queryString);
    }

    /// <summary>
    ///     Reads the table.
    /// </summary>
    /// <returns>The table.</returns>
    /// <param name="tableName">Table name.</param>
    /// <param name="items">Items.</param>
    /// <param name="colNames">Col names.</param>
    /// <param name="operations">Operations.</param>
    /// <param name="colValues">Col values.</param>
    public SqliteDataReader ReadTable(string tableName, string[] items, string[] colNames, string[] operations,
        string[] colValues)
    {
        var queryString = "SELECT " + items[0];
        for (var i = 1; i < items.Length; i++)
            queryString += ", " + items[i];
        queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
        for (var i = 0; i < colNames.Length; i++)
            queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
        return ExecuteQuery(queryString);
    }
}