using System.Collections.Generic;

namespace Framework.Task
{
    public class TaskManager
    {
        public List<TaskInfo> TaskList = new();

        public void Init()
        {
            // TaskList.Add(new CandleTaskInfo());
            TaskList.Add(new AnswerTaskInfo());
            TaskList.Add(new WaiterTaskInfo());
            // TaskList.Add(new EatTaskInfo());
        }

        public void OnUpdate(float dt)
        {
            foreach (var taskInfo in TaskList)
            {
                if (!taskInfo.IsTaskActive && taskInfo.CanEnter())
                {
                    taskInfo.Enter();
                }
                
                taskInfo.OnUpdate(dt);
            }
        }
    }
}