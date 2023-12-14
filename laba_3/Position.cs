using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba_3
{
    [Serializable]
    public class Position
    {
        public int X { set; get; } 
        public int Y { get; set; }


        public override bool Equals(object obj)
        {
            if(obj is Position position) 
            {
                return this.X == position.X && this.Y == position.Y;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return (X << 2) ^ Y;
        }
    }
}
