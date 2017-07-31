using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainRM21WPFapp.ViewModels
{
    public class NotifyPropertyChangedVMbase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(String str) { OnPropertyChanged(str); }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if(this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
