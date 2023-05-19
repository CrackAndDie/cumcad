using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumcad.ViewModels.Base
{
    internal interface ISaveable
    {
        object GetSaveableObject();
        void SetSaveableObject(object obj);
    }
}
