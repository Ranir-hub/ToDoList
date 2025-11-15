using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToDoList
{
    public class ToDo : INotifyPropertyChanged
    {
        private bool complete = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        public string Task { get; set; }
        public ToDo(string task, bool complete) { Task = task; Complete = complete; }

        public bool Complete
        {
            get => complete;
            set
            {
                if (complete != value)
                {
                    complete = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TextDecorations));
                }
            }
        }

        public TextDecorations TextDecorations => Complete ? TextDecorations.Strikethrough : TextDecorations.None;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class ToDoViewModel : INotifyPropertyChanged
    {
        private string task = "";
        private bool complete = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand AddTask {  get; set; }
        public ICommand RemoveTask { get; set; }
        public ObservableCollection<ToDo> ToDoList { get; } = new();

        public ToDoViewModel()
        {
            AddTask = new Command<string>((task) =>
            {
                complete = false;
                if (!string.IsNullOrWhiteSpace(Task))
                {
                    ToDoList.Add(new ToDo(Task, complete));
                    Task = "";
                }
            },
            (task) => !string.IsNullOrWhiteSpace(task));

            RemoveTask = new Command<ToDo>((ToDo todo) =>
            {
                ToDoList.Remove(todo);
            });
        }

        public string Task
        {
            get => task;
            set
            {
                if (task != value)
                {
                    task = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Complete
        {
            get => complete;
            set
            {
                if (complete != value)
                {
                    complete = value;
                    OnPropertyChanged();
                }
            }
        }
        public TextDecorations TextDecorations => Complete ? TextDecorations.Strikethrough : TextDecorations.None;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            ((Command)AddTask).ChangeCanExecute();
        }
    }
}
