using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace RollTools
{
    class PollService : BasicService
    {
        public List<Poll> queryList(long id)
        {
            List<Poll> tables = null;
            string querySql = "select id, name, is_visibility from poll where template_id = " + id;
            dBManager.Open();
            SQLiteDataReader reader = dBManager.ExecuteQuery(querySql);
            tables = new List<Poll>();
            while (reader.Read())
            {
                Poll poll = new Poll();
                poll.Id = reader.GetInt64(0);
                poll.Name = reader.GetString(1);
                poll.Is_visibility = reader.GetString(2);
                tables.Add(poll);
            }
            reader.Close();
            dBManager.Close();
            return tables;

        }

        public void insert(Poll poll)
        {
            string querySql = "insert into poll values ( " + poll.Id + "," + poll.Template_id + ",'"  +  poll.Name +  "','" + poll.Is_visibility + "')";
            dBManager.Open();
            dBManager.Execute(querySql);
            dBManager.Close();
        }


        public void update(Poll poll)
        {
            string querySql = "update poll set is_visibility = '" + poll.Is_visibility + "', name = '" + poll.Name + "' where id=" + poll.Id;
            dBManager.Open();
            dBManager.Execute(querySql);
            dBManager.Commit();
            dBManager.Close();
        }


        public Poll getPoll(long pollId)
        {
            string querySql = "select id, name, is_visibility from poll where id = " + pollId;
            dBManager.Open();
            SQLiteDataReader reader = dBManager.ExecuteQuery(querySql);
            Poll poll = null;
            while (reader.Read())
            {
                poll = new Poll();
                poll.Id = reader.GetInt64(0);
                poll.Name = reader.GetString(1);
                poll.Is_visibility = reader.GetString(2);
            }
            reader.Close();
            dBManager.Close();
            return poll;

        }
    }
}
