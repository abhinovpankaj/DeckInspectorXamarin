﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Code.Models
{
    public class CheakBoxListReturntModel
    {
        public CheakBoxListReturntModel()
        {
            selectedList = new List<string>();
        }
        public int Count { get; set; }
        public List<string> selectedList { get; set; }
    }
}
