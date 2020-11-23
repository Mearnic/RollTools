using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RollTools
{
    class BasicService
    {
        public DBManager dBManager = null;
        public BasicService()
        {
            string dbPath = Environment.CurrentDirectory + "/data" + "/SqliteModel.db";
            dBManager = new DBManager(dbPath);
        }

        public long GenerateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
