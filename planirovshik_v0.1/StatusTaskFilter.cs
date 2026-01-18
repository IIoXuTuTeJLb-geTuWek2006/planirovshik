using System.Collections.Generic;
using System.Linq;

namespace planirovshik_v0._1
{
    public class StatusTaskFilter : ITaskFilter
    {
        private readonly TaskStatus? _status;

        public StatusTaskFilter(TaskStatus? status)
        {
            _status = status;
        }

        public IEnumerable<TaskItem> Apply(IEnumerable<TaskItem> tasks)
        {
            if (_status == null)
                return tasks;

            return tasks.Where(t => t.Status == _status);
        }
    }
}