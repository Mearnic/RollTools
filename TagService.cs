using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace RollTools
{
    class TagService : BasicService
    {
        public List<Tag> queryList(long id, bool isRepeact)
        {
            List<Tag> tables = null;
            string querySql = "";
            if (isRepeact)
            {
                querySql = "select id, poll_id, name, is_use, is_rolled from tag where poll_id = " + id + " and is_use = '1'  and is_rolled = '0'";

            }
            else {
                querySql = "select id, poll_id, name, is_use, is_rolled from tag where poll_id = " + id + " and is_use = '1'";
            }
            dBManager.Open();
            SQLiteDataReader reader = dBManager.ExecuteQuery(querySql);
            tables = new List<Tag>();
            while (reader.Read())
            {
                Tag tag = new Tag();
                tag.Id = reader.GetInt64(0);
                tag.Poll_id = reader.GetInt64(1);
                tag.Name = reader.GetString(2);
                tag.Is_use = reader.GetString(3);
                tag.Is_rolled = reader.GetString(4);
                tables.Add(tag);
            }
            reader.Close();
            return tables;

        }
        public List<Tag> queryAllList(long id)
        {
            List<Tag> tables = null;
            string querySql = "select id, poll_id, name, is_use, is_rolled from tag where poll_id = " + id;
            dBManager.Open();
            SQLiteDataReader reader = dBManager.ExecuteQuery(querySql);
            tables = new List<Tag>();
            while (reader.Read())
            {
                Tag tag = new Tag();
                tag.Id = reader.GetInt64(0);
                tag.Poll_id = reader.GetInt64(1);
                tag.Name = reader.GetString(2);
                tag.Is_use = reader.GetString(3);
                tag.Is_rolled = reader.GetString(4);
                tables.Add(tag);
            }
            reader.Close();
            return tables;

        }


        public void insert(Tag tag)
        {
            string querySql = "insert into tag values ( " + tag.Id + "," + tag.Poll_id + ",'" + tag.Name + "','" + tag.Is_use+ "','" + tag.Is_rolled + "')";
            dBManager.Open();
            dBManager.Execute(querySql);
            dBManager.Close();
        }

        public void update(Tag tag)
        {
            string querySql = "update tag set name = '" + tag.Name + "', is_use = '" + tag.Is_use + "', is_rolled = '" + tag.Is_rolled+ "' where id=" + tag.Id;
            dBManager.Open();
            dBManager.Execute(querySql);
            dBManager.Commit();
            dBManager.Close();
        }

        public void updateAllRolled(long poll_id)
        {
            string querySql = "update tag set is_rolled = '0' where poll_id=" + poll_id;
            dBManager.Open();
            dBManager.Execute(querySql);
            dBManager.Commit();
            dBManager.Close();
        }

        public void deleteAll(long poll_id)
        {
            string querySql = "delete from tag where poll_id=" + poll_id;
            dBManager.Open();
            dBManager.Execute(querySql);
            dBManager.Commit();
            dBManager.Close();
        }

        public void delete(long id)
        {
            string querySql = "delete from tag where id=" + id;
            dBManager.Open();
            dBManager.Execute(querySql);
            dBManager.Commit();
            dBManager.Close();
        }
    }


}
