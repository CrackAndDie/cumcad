using cumcad.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumcad.Models.Other.MyEventArgs
{
    internal class MoveDirectionEventArgs : EventArgs
    {
        private readonly MoveDirection direction;

        public MoveDirectionEventArgs(MoveDirection direction)
        {
            this.direction = direction;
        }

        public MoveDirection Parameter
        {
            get { return direction; }
        }
    }
}
