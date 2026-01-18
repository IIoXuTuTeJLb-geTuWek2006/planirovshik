using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace planirovshik_v0._1
{
    public interface ITaskFilter
    {
        IEnumerable<TaskItem> Apply(IEnumerable<TaskItem> tasks);
    }
}