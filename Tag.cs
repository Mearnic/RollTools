using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RollTools
{
    class Tag
    {
        int id;
        int pollId;
        string name;

        public int Id { get => id; set => id = value; }
        public int PollId { get => pollId; set => pollId = value; }
        public string Name { get => name; set => name = value; }
    }
}
