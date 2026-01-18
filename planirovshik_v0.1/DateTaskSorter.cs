using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Linq;

namespace planirovshik_v0._1
{
    // сортировка по сроку выполнения (раньше → позже)
    public class DateTaskSorter : ITaskSorter
    {
        public IEnumerable<TaskItem> Sort(IEnumerable<TaskItem> tasks)
            => tasks.OrderBy(t => t.DueDate);
    }
}
