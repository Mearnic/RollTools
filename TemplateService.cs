using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace RollTools
{
    class TemplateService : BasicService
    {

        PollService pollService;

        public TemplateService() {
            pollService = new PollService();
        }

        public Template getDefaultTemplate()
        {
            string querySql = "select id, name, is_used from template where is_used = '1'";
            dBManager.Open();
            SQLiteDataReader reader = dBManager.ExecuteQuery(querySql);
            Template template = null;
            while (reader.Read())
            {
                template = new Template();
                template.Id = reader.GetInt64(0);
                template.Name = reader.GetString(1);
                template.Is_used = reader.GetString(2);
            }
            reader.Close();
            dBManager.Close();
            return template;

        }

        public List<Template> queryList()
        {
            List<Template> tables = new List<Template>();
            string querySql = "select id, name, is_used from template order by id desc";
            dBManager.Open();
            SQLiteDataReader reader = dBManager.ExecuteQuery(querySql);
            while (reader.Read())
            {
                Template template = new Template();
                template.Id = reader.GetInt64(0);
                template.Name = reader.GetString(1);
                template.Is_used = reader.GetString(2);
                tables.Add(template);
            }
            reader.Close();
            dBManager.Open();
            return tables;

        }

        public void insert(Template template)
        {
            Update_All_Not_Used();
            string querySql = "insert into template values ( " + template.Id + ",'" + template.Name + "'," + template.Is_used + ")";
            dBManager.Open();
            dBManager.Execute(querySql);
            dBManager.Commit();
            dBManager.Close();
            for (int i = 0; i < 4; i++)
            {
                Poll poll = new Poll();
                poll.Id = GenerateId();
                poll.Name = "待设置";
                poll.Template_id = template.Id;
                poll.Is_visibility = "1";
                poll.Is_repeat = "0";
                pollService.insert(poll);
            }
        }

        public void updateUsed(Template template)
        {
            Update_All_Not_Used();
            string querySql = "update template set is_used = '1' where id=" + template.Id;
            dBManager.Open();
            dBManager.Execute(querySql);
            dBManager.Commit();
            dBManager.Close();
        }

        public void update(Template template)
        {
            Update_All_Not_Used();
            string querySql = "update template set is_used = '" + template.Is_used+"', name = '" + template.Name+ "' where id=" + template.Id;
            dBManager.Open();
            dBManager.Execute(querySql);
            dBManager.Commit();
            dBManager.Close();
        }

        public void Update_All_Not_Used()
        {
            string querySql = "update template set is_used = 0 where 1=1";
            dBManager.Open();
            dBManager.Execute(querySql);
            dBManager.Commit();
            dBManager.Close();
        }
        public void delete(long id)
        {
            string querySql = "delete from template where id=" + id;
            dBManager.Open();
            dBManager.Execute(querySql);
            dBManager.Commit();
            dBManager.Close();
        }
    }
}
