using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cumcad.Models.Other.MyEventArgs
{
    internal class WindowStateChangedEventArgs : EventArgs
    {
        private readonly WindowState windowState;

        public WindowStateChangedEventArgs(WindowState windowState)
        {
            this.windowState = windowState;
        }

        public WindowState Parameter
        {
            get { return windowState; }
        }
    }
}
