using System;
using System.Windows;

namespace planirovshik_v0._1
{
    public partial class TaskEditWindow : Window
    {
        public TaskItem Task { get; }

        public TaskEditWindow(TaskItem task)
        {
            InitializeComponent();

            Task = task ?? throw new ArgumentNullException(nameof(task));

            // заполняем поля из задачи
            TitleTextBox.Text = Task.Title;
            DescriptionTextBox.Text = Task.Description;
            DueDatePicker.SelectedDate = Task.DueDate;

            switch (Task.Priority)
            {
                case TaskPriority.Low:
                    PriorityComboBox.SelectedIndex = 0;
                    break;
                case TaskPriority.Medium:
                    PriorityComboBox.SelectedIndex = 1;
                    break;
                case TaskPriority.High:
                    PriorityComboBox.SelectedIndex = 2;
                    break;
            }

            switch (Task.Status)
            {
                case TaskStatus.Planned:
                    StatusComboBox.SelectedIndex = 0;
                    break;
                case TaskStatus.InProgress:
                    StatusComboBox.SelectedIndex = 1;
                    break;
                case TaskStatus.Done:
                    StatusComboBox.SelectedIndex = 2;
                    break;
                case TaskStatus.Postponed:
                    StatusComboBox.SelectedIndex = 3;
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Введите заголовок задачи.");
                return;
            }

            Task.Title = TitleTextBox.Text;
            Task.Description = DescriptionTextBox.Text;
            Task.DueDate = DueDatePicker.SelectedDate ?? DateTime.Today;

            // приоритет
            switch ((PriorityComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString())
            {
                case "Low":
                    Task.Priority = TaskPriority.Low;
                    break;
                case "High":
                    Task.Priority = TaskPriority.High;
                    break;
                default:
                    Task.Priority = TaskPriority.Medium;
                    break;
            }

            // статус
            switch ((StatusComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString())
            {
                case "InProgress":
                    Task.Status = TaskStatus.InProgress;
                    break;
                case "Done":
                    Task.Status = TaskStatus.Done;
                    break;
                case "Postponed":
                    Task.Status = TaskStatus.Postponed;
                    break;
                default:
                    Task.Status = TaskStatus.Planned;
                    break;
            }

            DialogResult = true; 
        }
    }
}
