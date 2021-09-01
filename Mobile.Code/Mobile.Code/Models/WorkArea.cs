using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Mobile.Code.Models;
using Xamarin.Forms;

namespace Mobile.Code.Models
{
    public class WorkArea
    {
        public ObservableCollection<ItemImage> WorkAreaImages { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }

    }
}