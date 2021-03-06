﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RollTools
{
    class Tag
    {
        long id;
        long poll_id;
        string name;
        string is_use;
        string is_rolled;
        bool _isChecked;
        bool _isUse;
        bool _isDelete;
        bool _isRolled;



        public long Id { get => id; set => id = value; }
        public long Poll_id { get => poll_id; set => poll_id = value; }
        public string Name { get => name; set => name = value; }
        public string Is_use { get => is_use; set => is_use = value; }
        public bool IsChecked { get => _isChecked; set => _isChecked = value; }
        public bool IsUse { get => is_use == "1"; set => is_use = value ? "1" : "0"; }
        public bool IsDelete { get => _isDelete; set => _isDelete = value; }
        public string Is_rolled { get => is_rolled; set => is_rolled = value; }
        public bool IsRolled { get => is_rolled == "1"; set => is_rolled = value ? "1" : "0"; }
    }
}
