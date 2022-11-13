using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SaperLab2WPF
{
    public class Cell : INotifyPropertyChanged
    {
        private int? minescloseby;
        private bool ismine;
        private bool isopened;
        private bool ischecked;
        private bool isflagged;

        public Cell(int? minesclose, bool isthismine)
        {
            this.minescloseby = minesclose;
            this.ismine = isthismine;
            this.isopened = false;
            this.ischecked = false;
            this.isflagged = false;
        }

        public int? MinesCloseBy
        {
            get
            {
                if (this.isopened)
                    return minescloseby;
                else
                    return null;
            }
        }

        public bool IsMine
        {
            get
            {
                return ismine;
            }
        }

        public bool IsOpened
        {
            get
            {
                return this.isopened;
            }
            set
            {
                this.isopened = value;
                if (isflagged)
                    IsFlagged = false;
                OnPropertyChanged("IsOpened");
                OnPropertyChanged("Content");
                OnPropertyChanged("IsFreeToCheck");
                OnPropertyChanged("IsFlagged");
                if (this.ismine)
                    GameManager.singleton.Lose();
            }
        }

        public bool IsFreeToCheck
        {
            get
            {
                return !this.isopened;
            }
        }

        public string Content
        {
            get
            {
                if (this.isopened)
                {
                    if (this.ismine)
                        return "Bomb";
                    else
                        return minescloseby.ToString();
                }
                else
                    return null;
            }
        }

        public bool IsFlagged
        {
            get
            {
                return this.isflagged;
            }
            set
            {
                if (!GameManager.singleton.CanPlaceFlag && !this.isflagged)
                {
                    this.isflagged = false;
                    return;
                }
                this.isflagged = !this.isflagged;
                int cell = 0;
                int mine = 0;
                if(this.isflagged)
                {
                    cell = 1;
                    if(ismine)
                    {
                        mine = 1;
                    }
                }
                else
                {
                    if (ismine)
                    {
                        mine = -1;
                    }
                    cell = -1;
                }
                GameManager.singleton.MinesFlagged += mine;
                GameManager.singleton.CellsFlagged += cell;
                OnPropertyChanged("CurrentImage");
            }
        }

        public ImageSource CurrentImage
        {
            get
            {
                if (isflagged)
                    return new BitmapImage(new Uri(@"C:\flag.png"));
                else
                    return null;
            }
        }


        RelayCommand? flagCommand;
        public RelayCommand FlagCommand
        {
            get
            {
                return flagCommand ??
                  (flagCommand = new RelayCommand(obj =>
                  {
                      IsFlagged = !IsFlagged;
                  }));
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
