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
        string is_repeat;

        bool _isVisibility;
        bool _isRepeat;

        public long Id { get => id; set => id = value; }
        public long Template_id { get => template_id; set => template_id = value; }
        public string Name { get => name; set => name = value; }
        public string Is_visibility { get => is_visibility; set => is_visibility = value; }
        public bool IsVisibility { get => is_visibility == "1"; set => is_visibility = value ? "1" : "0"; }
        public string Is_repeat { get => is_repeat; set => is_repeat = value; }
        public bool IsRepeat { get => is_repeat == "1"; set => is_repeat = value ? "1" : "0"; }
    }

}
