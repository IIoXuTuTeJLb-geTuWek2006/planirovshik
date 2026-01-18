using System.Collections.Generic;
using System.Linq;

namespace planirovshik_v0._1
{
    public class TaskController
    {
        private readonly List<TaskItem> _tasks = new();

        public IReadOnlyList<TaskItem> Tasks => _tasks;

        public event EventHandler? TasksChanged;

        private void OnTasksChanged()
            => TasksChanged?.Invoke(this, EventArgs.Empty);

        public void Add(TaskItem task)
        {
            _tasks.Add(task);
            OnTasksChanged();
        }

        public void Remove(TaskItem task)
        {
            _tasks.Remove(task);
            OnTasksChanged();
        }
        public void SetStatus(TaskItem task, TaskStatus status)
        {
            if (!_tasks.Contains(task)) return;
            task.Status = status;
            OnTasksChanged();
        }
        public IEnumerable<TaskItem> GetFiltered(TaskStatus? statusFilter)
        {
            IEnumerable<TaskItem> q = _tasks;
            if (statusFilter != null)
                q = q.Where(t => t.Status == statusFilter);
            return q;
        }
    }
}
