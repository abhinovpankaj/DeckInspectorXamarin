using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Code.Models
{
    public class ProjectTab 
    {
        public string Id { get; set; }
        public bool Selected { get; set; }
        public string Title { get; set; }

        public List<Project> Items { get; set; }
    }
}
