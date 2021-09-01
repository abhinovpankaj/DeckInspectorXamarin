﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CodeRepo.Mobile.Models;
using Mobile.Code.Models;
using Xamarin.Forms;

namespace Mobile.Code.Controls
{
    public partial class CarouselIndicatorView : Grid, INotifyPropertyChanged
    {
        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IEnumerable), typeof(CarouselIndicatorView), null);
        public static readonly BindableProperty CurrentItemProperty = BindableProperty.Create(nameof(CurrentItem), typeof(object), typeof(CarouselIndicatorView), null, BindingMode.TwoWay, propertyChanged: CurrentItemChange);
        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set 
            {
                SetValue(ItemsProperty, value);
                //OnPropertyChanged("Items");
            }
        }
        public object CurrentItem
        {
            get { return (object)GetValue(CurrentItemProperty); }
            set 
            { 
                SetValue(CurrentItemProperty, value);
                //OnPropertyChanged("CurrentItem");
            }
        }
        public CarouselIndicatorView()
        {
            InitializeComponent();
        }
        static void CurrentItemChange(object bindable, object oldValue, object newValue)
        {
            var x = (CarouselIndicatorView)bindable;
            var labelContainer = x.FindByName("myList") as FlexLayout;
            foreach (Label label in labelContainer.Children)
            {
                var tabGesture = label.GestureRecognizers[0] as TapGestureRecognizer;
                if (newValue == tabGesture.CommandParameter)
                {
                    x.MoveActiveIndicator(label);
                    return;
                }


            }

        }

        void MoveActiveIndicator(Label target)
        {
            var width = target.Width - activeIndicator.Width;
            activeIndicator.TranslateTo(target.X + (width / 2), 0, 100, Easing.Linear);
        }
        void ChangeTab(System.Object sender, System.EventArgs e)
        {
            List<Project> temp=new List<Project>();
            foreach (ProjectTab item in Items)
            {
                //ProjectTab tb = item as ProjectTab;
                if (item.Title =="All")
                {
                    temp=item.Items.Where(x => x.IsStarred).ToList();
                }
                if(item.Title=="Starred")
                {
                    item.Items = temp;
                }

                //}
            }

            foreach (var item in Items)
            {
                if (item == ((TappedEventArgs)e).Parameter)
                {
                    CurrentItem = item;
                    return;
                }
                    

            }
        }

        
    }
}
