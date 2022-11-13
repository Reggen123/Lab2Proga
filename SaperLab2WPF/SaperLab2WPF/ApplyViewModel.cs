using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace SaperLab2WPF
{
    public class ApplyViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<Cell> cells;
        private Cell[,] cellsforgame;

        private int minefieldrows = 0;
        private int minefieldcols = 0;
        private bool isGameStarted = false;
        private int cellsflagged = 0;
        private GameManager gameManager;
        public ApplyViewModel()
        {
           
        }


        public ObservableCollection<Cell> Cells
        {
            get
            {
                return this.cells;
            }
        }

        public string LabelRows
        {
            get
            {
                return $"Ширина:{this.minefieldrows}";
            }
        }
        public string LabelCols
        {
            get
            {
                return $"Высота:{this.minefieldcols}";
            }
        }

        public int MineFieldRows
        {
            get
            {
                return this.minefieldrows;
            }
            set
            {
                this.minefieldrows = value;
                OnPropertyChanged("MineFieldRows");
                OnPropertyChanged("LabelRows");
            }
        }

        public int MineFieldCols
        {
            get
            {
                return this.minefieldcols;
            }
            set
            {
                this.minefieldcols = value;
                OnPropertyChanged("MineFieldCols");
                OnPropertyChanged("LabelCols");
            }
        }

        public bool IsGameStarted
        {
            get
            {
                return this.isGameStarted;
            }
            set
            {
                this.isGameStarted = value;
                if (isGameStarted)
                {
                    gameManager = new GameManager(minefieldrows - 1);
                    gameManager.NotifyCFlagged += (int x) =>
                    {
                        cellsflagged = x;
                        OnPropertyChanged("CellsFlagged");
                    };
                    gameManager.NotifyLose += Lose;
                    gameManager.NotifyWin += Win;
                    cellsforgame = GameManager.GetGameField(minefieldrows, minefieldcols);
                    cells = new ObservableCollection<Cell>();
                    for (int i = 0; i < minefieldrows; i++)
                    {
                        for (int j = 0; j < minefieldcols; j++)
                        {
                            cells.Add(cellsforgame[i, j]);
                        }
                    }
                }
                else
                {
                    gameManager.NotifyCFlagged -= (int x) =>
                    {
                        cellsflagged = x;
                        OnPropertyChanged("CellsFlagged");
                    };
                    gameManager.NotifyLose -= Lose;
                    gameManager.NotifyWin -= Win;
                    gameManager = null;
                    cellsflagged = 0;
                    cells = new ObservableCollection<Cell>();
                    OnPropertyChanged("IsGameStarted");
                }
                OnPropertyChanged("CellsFlagged");
                OnPropertyChanged("Cells");
                OnPropertyChanged("StartGameButtonText");
            }
        }

        public string StartGameButtonText
        {
            get
            {
                if (isGameStarted)
                    return $"Стереть поле";
                else
                    return $"Создать поле";
            }
        }
        public string CellsFlagged
        {
            get
            {
                if (gameManager != null)
                    return $"Помеченных клеток:\n{cellsflagged}/{this.minefieldrows - 1}";
                else
                    return $"";
            }
        }

        public void Lose()
        {
            IsGameStarted = false;
            WindowNotMain window = new WindowNotMain("Саперы ошибаются только однажды");
            window.Show();
        }

        public void Win()
        {
            IsGameStarted = false;
            WindowNotMain window = new WindowNotMain("Ура, Вы победили!!!!");
            window.Show();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        

        //<ToggleButton Content="{Binding Content}" IsChecked="{Binding IsOpened}" IsEnabled="{Binding IsFreeToCheck}">
    }

    public class RelayCommand : ICommand
    {
        Action<object?> execute;
        Func<object?, bool>? canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }
        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }
}
