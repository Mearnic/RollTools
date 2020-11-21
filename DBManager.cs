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

        StreamWriter streamWriterLog = null;
        private SQLiteConnection _SQLiteConn = null;
        private SQLiteTransaction _SQLiteTrans = null;
        private bool _IsRunTrans = false;


        private string _SQLiteConnString = null;
        private bool _AutoCommit = false;

        public string _dbName { get; private set; }
        public string SQLiteConnString { get => _SQLiteConnString; set => _SQLiteConnString = value; }
        public StreamWriter StreamWriterLog { get => streamWriterLog; set => streamWriterLog = value; }

        public DBManager(string dbPath)
        {
            this._dbName = dbPath;
            StreamWriterLog = new StreamWriter(this._dbName + ".log", true, Encoding.Default);
            this._SQLiteConnString = "Data Source=" + this._dbName;
            if (!File.Exists(_dbName))
            {
                NewDbFile();
            }
        }

        public Boolean NewDbFile()
        {
            try
            {
                SQLiteConnection.CreateFile(_dbName);
                StreamWriterLog.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "" + "-->" + _dbName + "创建成功！");
                StreamWriterLog.Flush();
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
            StreamWriterLog.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "" + "-->" + sql + "执行成功！");
            StreamWriterLog.Flush();
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
