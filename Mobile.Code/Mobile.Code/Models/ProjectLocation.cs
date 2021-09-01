﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Mobile.Code.Models
{
    public class ProjectLocation : BindingModel
    {





        private string _pimage;

        public string ImageUrl
        {
            get { return _pimage; }
            set { _pimage = value; OnPropertyChanged("ImageUrl"); }
        }


        public string Id { get; set; }


        public string Name { get; set; }

        public string Description { get; set; }


        public string ProjectId { get; set; }

        public string CreatedOn { get; set; }

        public bool IsOriginal { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public bool IsDelete { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public int SeqNo { get; set; }
        public string AssignTo { get; set; }
        public string Username { get; set; }
    }
}
