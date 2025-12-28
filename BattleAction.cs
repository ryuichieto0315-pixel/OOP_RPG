using System;
using System.Collections.Generic;
using System.Text;

namespace OOP_RPG
{
    internal abstract class BattleAction

    {
        public string Action { get;  set; } = "";
        public string Target { get;  set; } = "";
        public string Kind { get;  set; } = "";
        public uint TakeHp { get;  set; }
        public uint UseMp { get;  set; }


    }
}
