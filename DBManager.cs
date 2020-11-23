using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace RollTools
{
    class DBManager : IDisposable
    {
        private SQLiteConnection _SQLiteConn = null;
        private SQLiteTransaction _SQLiteTrans = null;
        private bool _IsRunTrans = false;


        private string _SQLiteConnString = null;
        private bool _AutoCommit = false;

        public string _dbName { get; private set; }
        public string SQLiteConnString { get => _SQLiteConnString; set => _SQLiteConnString = value; }

        public DBManager(string dbPath)
        {
            this._dbName = dbPath;
            this._SQLiteConnString = "Data Source=" + this._dbName;
        }

        public Boolean NewDbFile()
        {
            try
            {
                SQLiteConnection.CreateFile(_dbName);
            }
            catch (Exception ex)
            {
                throw new Exception("新建数据库文件" + _dbName + "失败：" + ex.Message);
            }
            return true;

        }

        public static Boolean NewDbFile(string dbPath)
        {
            try
            {
                SQLiteConnection.CreateFile(dbPath);
            }
            catch (Exception ex)
            {
                throw new Exception("新建数据库文件" + dbPath + "失败：" + ex.Message);
            }
            return true;
        }

        public void Execute(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = _SQLiteConn;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="queryString">SQL命令字符串</param>
        public SQLiteDataReader ExecuteQuery(string sql)
        {
            SQLiteDataReader dataReader;
            try
            {
                SQLiteCommand cmd = _SQLiteConn.CreateCommand();
                cmd.CommandText = sql;
                dataReader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("执行：" + _dbName + "的查询失败：" + ex.Message);
            }
            return dataReader;
        }

        public Boolean Open()
        {
            try
            {
                this._SQLiteConn = new SQLiteConnection(this._SQLiteConnString);
                this._SQLiteConn.Open();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("打开数据库：" + _dbName + "的连接失败：" + ex.Message);
            }
        }

        public void Close()
        {
            if (this._SQLiteConn != null && this._SQLiteConn.State != ConnectionState.Closed)
            {
                if (this._IsRunTrans && this._AutoCommit)
                {
                    this.Commit();
                }
                this._SQLiteConn.Close();
                this._SQLiteConn = null;
            }
        }

        public void BeginTransaction()
        {
            this._SQLiteConn.BeginTransaction();
            this._IsRunTrans = true;
        }

        public void BeginTransaction(IsolationLevel isoLevel)
        {
            this._SQLiteConn.BeginTransaction(isoLevel);
            this._IsRunTrans = true;
        }

        public void Commit()
        {
            if (this._IsRunTrans)
            {
                this._SQLiteTrans.Commit();
                this._IsRunTrans = false;
            }
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
