using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SaperLab2WPF
{
    class ApplyViewModelMini: INotifyPropertyChanged
    {
        private string labeltext;
        public ApplyViewModelMini(string text)
        {
            labeltext = text;
        }
        public string LabelText
        {
            get
            {
                return labeltext;
            }
            set
            {
                labeltext = value;
                OnPropertyChanged("LabelText");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
