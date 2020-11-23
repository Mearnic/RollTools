using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RollTools
{
    class Poll
    {
        long id;
        long template_id;
        string name;
        string is_visibility;

        bool _isVisibility;

        public long Id { get => id; set => id = value; }
        public long Template_id { get => template_id; set => template_id = value; }
        public string Name { get => name; set => name = value; }
        public string Is_visibility { get => is_visibility; set => is_visibility = value; }
        public bool IsVisibility { get => is_visibility == "1"; set => is_visibility = value ? "1" : "0"; }
    }

}
