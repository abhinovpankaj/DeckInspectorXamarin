
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CodeRepo.Mobile.Models
{
    

    public class ProjectTab : BindableBase
    {
        public string Id { get; set; }
        public bool Selected { get; set; }
        public string Title { get; set; }
        
        private List<Project> _items;
        public List<Project> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
    }
}
