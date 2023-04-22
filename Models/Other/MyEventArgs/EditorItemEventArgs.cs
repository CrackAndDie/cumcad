using cumcad.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cumcad.Models.Other.MyEventArgs
{
    internal class EditorItemEventArgs : EventArgs
    {
        private readonly EditorItem editorItem;

        public EditorItemEventArgs(EditorItem editorItem)
        {
            this.editorItem = editorItem;
        }

        public EditorItem Parameter
        {
            get { return editorItem; }
        }
    }
}
