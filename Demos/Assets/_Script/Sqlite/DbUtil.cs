using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Liangddyy.Util
{
    /// <summary>
    /// Sqlite封装 dbUtil
    /// </summary>
    public class DbUtil
    {
#if UNITY_ANDROID
    private static string path = "URI=file:" + Application.dataPath;
#endif
#if UNITY_IPHONE
    private static string path = "data source=" + Application.persistentDataPath;
#endif
#if UNITY_STANDALONE_WIN
        private static readonly string path = "data source=" + Application.dataPath;
#endif
        private static readonly string dbName = "/test02.db";
        private static DbUtil dbUtil;

        private SqliteConnection dbConnection;
        private SqliteCommand dbCmd;
        private SqliteTransaction dbTransaction;
        private SqliteDataReader dbReader;


        public static DbUtil getInstance()
        {
            if (dbUtil == null)
                dbUtil = new DbUtil(path + dbName);
            return dbUtil;
        }

        private DbUtil(string connectionStr)
        {
            try
            {
                dbConnection = new SqliteConnection(connectionStr);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public SqliteDataReader ExecuteQuery(string queryString)
        {
            dbCmd = dbConnection.CreateCommand();
            dbCmd.CommandText = queryString;

            dbReader = dbCmd.ExecuteReader();

            return dbReader;
        }

        public int ExecuteSql(string sqlStr)
        {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();

            dbCmd = dbConnection.CreateCommand();
            dbCmd.CommandText = sqlStr;
            return dbCmd.ExecuteNonQuery();
        }

        private int ExecuteSql(SqliteCommand sqlCmd)
        {
            return dbCmd.ExecuteNonQuery();
        }

        public void CreateTable<T>(T arg)
        {
            this.ExecuteSql(GetCmdCreateTable(arg));
        }

        public void Insert<T>(T record)
        {
            dbCmd = dbConnection.CreateCommand();
            this.ExecuteSql(GetCmdInsert<T>(record, dbCmd));

            //Debug.Log(dbCmd.CommandText);
        }

        public void InsertList<T>(List<T> Records)
        {
            dbTransaction = dbConnection.BeginTransaction();
            try
            {
                for (var i = 0; i < Records.Count; i++)
                    Insert(Records[i]);
                dbTransaction.Commit();
            }
            catch (Exception)
            {
                dbTransaction.Rollback();
                throw;
            }
        }

        public void InsertArray<T>(T[] Records)
        {
            dbTransaction = dbConnection.BeginTransaction();
            try
            {
                for (var i = 0; i < Records.Length; i++)
                    Insert(Records[i]);
                dbTransaction.Commit();
            }
            catch (Exception)
            {
                dbTransaction.Rollback();
                throw;
            }
        }

        public void Update<T>(T arg)
        {
            dbCmd = dbConnection.CreateCommand();
            this.ExecuteSql(GetCmdUpdate(arg, dbCmd));
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
        /// 更新的sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg">The argument.</param>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        private SqliteCommand GetCmdUpdate<T>(T arg, SqliteCommand cmd)
        {
            Type type = arg.GetType();
            var TableName = type.Name;

            Column[] Columns = GetColumnsByType(arg);
            if ((Columns == null) || (Columns.Length < 1))
            {
                Debug.LogError("Type is Wrong");
                return null;
            }

            Column key = null;
            Column[] normal = null;
            Debug.Log("Now Insert:" + TableName);


            var RowLength = Columns.Length;

            normal = GetNormalColumn(Columns);
            cmd.CommandText = "UPDATE " + TableName + " SET " + normal[0].columnName + " = @" + normal[0].columnName;

            for (var i = 1; i < normal.Length; i++)
                cmd.CommandText += "," + normal[i].columnName + " = @" + normal[i].columnName;

            key = GetPrimaryColumn(Columns);
            cmd.CommandText += " WHERE " + key.columnName + " = @" + key.columnName + ";";

            for (var i = 0; i < RowLength; i++)
                cmd.Parameters.Add(new SqliteParameter("@" + Columns[i].columnName, Columns[i].ColumnValue));
            return cmd;
        }

        /// <summary>
        ///     获取主键
        /// </summary>
        /// <param name="Columns">The columns.</param>
        /// <returns></returns>
        private Column GetPrimaryColumn(Column[] Columns)
        {
            foreach (var item in Columns)
                if (item.IsPrimaryKey) return item;
            return null;
        }

        /// <summary>
        ///     获取普通字段
        /// </summary>
        /// <param name="Columns">The columns.</param>
        /// <returns></returns>
        private Column[] GetNormalColumn(Column[] Columns)
        {
            var tmp = new List<Column>();

            foreach (var item in Columns)
                if (!item.IsPrimaryKey)
                    tmp.Add(item);
            return tmp.ToArray();
        }

        /// <summary>
        ///     获取类的相关字段信息。
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns></returns>
        private Column[] GetColumnsByType<T>(T arg)
        {
            FieldInfo[] fieldsThis =
                arg.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] fieldsBase =
                arg.GetType().BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            FieldInfo[] fields = new FieldInfo[fieldsBase.Length + fieldsThis.Length];

            Array.Copy(fieldsBase, 0, fields, 0, fieldsBase.Length);
            Array.Copy(fieldsThis, 0, fields, fieldsBase.Length, fieldsThis.Length);

            int field_len = fields.Length;

            if ((fields == null) || (field_len < 1))
                return null;

            Column[] tableType = new Column[field_len];
            for (int i = 0; i < field_len; i++)
            {
                String Cur_Name = fields[i].Name;

                Type Cur_Type = fields[i].FieldType;

                var Value = fields[i].GetValue(arg);

                tableType[i] = new Column(Cur_Type, Cur_Name, Value);
            }
            return tableType;
        }

        /// <summary>
        /// 建表Sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg">The argument.</param>
        /// <returns></returns>
        private string GetCmdCreateTable<T>(T arg)
        {
            Type type = arg.GetType();
            string TableName = type.Name;

            Column[] columns = GetColumnsByType(arg);

            if ((columns == null) || (columns.Length < 1))
            {
                Debug.LogError("Type is Wrong");
                //todo 处理
                return null;
            }

            Debug.Log("NowCreate:" + TableName);

            string queryCreate = "CREATE TABLE IF NOT EXISTS " + TableName + "(" + columns[0].columnName + " " +
                                 columns[0].TableType;

            for (var i = 1; i < columns.Length; i++)
                queryCreate += ", " + columns[i].columnName + " " + columns[i].TableType;
            queryCreate += ");";

            return queryCreate;
        }

        /// <summary>
        /// 获取一条插入sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record">The record.</param>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        private SqliteCommand GetCmdInsert<T>(T record, SqliteCommand cmd)
        {
            Type type = record.GetType();
            string TableName = type.Name;

            Column[] columns = GetColumnsByType(record);
            if ((columns == null) || (columns.Length < 1))
            {
                Debug.LogError("Type is Wrong");
                return null;
            }

            int RowLength = columns.Length;

            cmd.CommandText = "INSERT INTO " + TableName + "(" + columns[0].columnName;
            for (var i = 1; i < RowLength; i++)
                cmd.CommandText += "," + columns[i].columnName;
            cmd.CommandText += ") VALUES (@" + columns[0].columnName;

            for (var i = 1; i < RowLength; i++)
                cmd.CommandText += ",@" + columns[i].columnName;
            cmd.CommandText += ");";

            for (var i = 0; i < RowLength; i++)
                cmd.Parameters.Add(new SqliteParameter("@" + columns[i].columnName, columns[i].ColumnValue));
            return cmd;
        }
    }
}