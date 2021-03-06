using System.Linq;
using CommandCore.Library.Attributes;
using CommandCore.Library.PublicBase;
using TaskCore.App.Options;
using TaskCore.App.Views;
using TaskCore.Dal.Interfaces;

namespace TaskCore.App.Verbs
{
    [VerbName("c", Description = "Marks a given task as complete.")]
    public class CompleteTask : VerbBase<CompleteTaskOptions>
    {
        private readonly ITodoTaskRepository _todoTaskRepository;

        public CompleteTask(ITodoTaskRepository todoTaskRepository)
        {
            _todoTaskRepository = todoTaskRepository;
        }

        public override VerbViewBase Run()
        {
            // TODO By decreasing the number of query calls, the performance can be improved here, but heck yeah for now.
            // I will consider performance tuning in the next iteration.
            var tasks = _todoTaskRepository.GetActiveTasksOrderedByAddedDate();
            var activeTasks = tasks.Where((t, i) => Options.TaskIds.Contains(i)).ToList();
            foreach (var activeTask in activeTasks)
            {
                _todoTaskRepository.MarkComplete(activeTask);
            }

            return new CompleteTaskView(activeTasks);
        }
    }
}