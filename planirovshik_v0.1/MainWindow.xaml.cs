using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace planirovshik_v0._1
{
    public partial class MainWindow : Window
    {
        private readonly TaskController _controller = new();
        private readonly JsonTaskSaver _saver = new();
        private string _tasksPath;

        public MainWindow()
        {
            InitializeComponent();

            _tasksPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tasks.json");

            _controller.TasksChanged += (_, __) =>
            {
                RefreshTaskList();
                _saver.Save(_controller.Tasks.ToList(), _tasksPath);
            };

            LoadTasks();
            InitStatusFilter();
            InitSortCombo();
            RefreshTaskList();

            Closing += MainWindow_Closing;
        }


        private void LoadTasks()
        {
            List<TaskItem> loaded = _saver.Load(_tasksPath);
            foreach (var t in loaded)
                _controller.Add(t);
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _saver.Save(_controller.Tasks.ToList(), _tasksPath);
        }

        private void InitStatusFilter()
        {
            StatusFilterCombo.Items.Add("Все");
            foreach (TaskStatus st in Enum.GetValues(typeof(TaskStatus)))
                StatusFilterCombo.Items.Add(st);
            StatusFilterCombo.SelectedIndex = 0;
        }

        private void InitSortCombo()
        {

        }

        private TaskStatus? GetSelectedStatusFilter()
        {
            if (StatusFilterCombo.SelectedIndex <= 0)
                return null;
            return (TaskStatus)StatusFilterCombo.SelectedItem;
        }

        private string GetSortTag()
        {
            if (SortCombo.SelectedItem is not ComboBoxItem item || item.Tag == null)
                return "None";
            return item.Tag.ToString() ?? "None";
        }

        private void RefreshTaskList()
        {
            var status = GetSelectedStatusFilter();
            IEnumerable<TaskItem> view = _controller.GetFiltered(status);

            ITaskSorter? sorter = GetSortTag() switch
            {
                "Date" => new DateTaskSorter(),
                "Priority" => new PriorityTaskSorter(),
                _ => null
            };

            if (sorter != null)
                view = sorter.Sort(view);

            TaskList.ItemsSource = view.ToList();
        }

        private TaskItem? GetSelectedTask() => TaskList.SelectedItem as TaskItem;

        //фильтр и сортировка

        private void StatusFilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsInitialized) RefreshTaskList();
        }

        private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsInitialized) RefreshTaskList();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var task = new TaskItem
            {
                Title = "новая задача",
                Description = "",
                DueDate = DateTime.Today,
                Priority = TaskPriority.Medium,
                Status = TaskStatus.Planned
            };

            var dlg = new TaskEditWindow(task) { Owner = this };
            if (dlg.ShowDialog() == true)
            {
                _controller.Add(task);
            }
        }


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var task = GetSelectedTask();
            if (task == null)
            {
                MessageBox.Show("выберите задачу для редактирования.");
                return;
            }

            var dlg = new TaskEditWindow(task) { Owner = this };
            if (dlg.ShowDialog() == true)
            {
                RefreshTaskList();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var task = GetSelectedTask();
            if (task == null)
            {
                MessageBox.Show("выберите задачу для удаления.");
                return;
            }

            if (MessageBox.Show("удалить выбранную задачу?", "удаление",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _controller.Remove(task);
            }
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            var task = GetSelectedTask();
            if (task == null) return;
            _controller.SetStatus(task, TaskStatus.Done);
        }

        private void PostponeButton_Click(object sender, RoutedEventArgs e)
        {
            var task = GetSelectedTask();
            if (task == null) return;
            _controller.SetStatus(task, TaskStatus.Postponed);
        }
        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Выберите файл задач",
                Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*",
                FileName = Path.GetFileName(_tasksPath)
            };

            if (dialog.ShowDialog() == true)
            {
                _tasksPath = dialog.FileName;

                // очищаем текущие задачи и загружаем новые
                _controller.Clear();
                var loaded = _saver.Load(_tasksPath);
                foreach (var t in loaded)
                    _controller.Add(t);

                RefreshTaskList();
            }
        }
        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Сохранить задачи как",
                Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*",
                FileName = Path.GetFileName(_tasksPath)
            };

            if (dialog.ShowDialog() == true)
            {
                _tasksPath = dialog.FileName;
                _saver.Save(_controller.Tasks.ToList(), _tasksPath);
            }
        }
    }
}
