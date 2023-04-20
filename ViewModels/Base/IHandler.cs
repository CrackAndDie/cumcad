﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace cumcad.ViewModels.Base
{
    internal interface IHandler
    {
        List<Mat> GetResult(List<Mat> images);
        event EventHandler<EventArgs> PropertiesChanged;
    }
}
