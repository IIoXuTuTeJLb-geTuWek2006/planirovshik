using System.Collections.Generic;
using System.Linq;

namespace planirovshik_v0._1
{
    public class TaskController
    {
        private readonly List<TaskItem> _tasks = new();

        public IReadOnlyList<TaskItem> Tasks => _tasks;

        public void Add(TaskItem task) => _tasks.Add(task);
        public void Remove(TaskItem task) => _tasks.Remove(task);

        public IEnumerable<TaskItem> GetFiltered(TaskStatus? statusFilter)
        {
            IEnumerable<TaskItem> q = _tasks;
            if (statusFilter != null)
                q = q.Where(t => t.Status == statusFilter);
            return q;
        }
    }
}
