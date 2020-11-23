using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RollTools
{
    class Template
    {
        long id;
        string name;
        string is_used;

        bool _isSelected;
        bool _isLasted;

        public long Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Is_used { get => is_used; set => is_used = value; }
        public bool IsSelected { get => is_used == "1"; set => is_used = value?"1":"0"; }
        public bool IsLasted { get => _isLasted; set => _isLasted = value; }
    }
}
