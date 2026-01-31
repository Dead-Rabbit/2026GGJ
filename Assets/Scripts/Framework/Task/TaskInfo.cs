namespace Framework.Task
{
    public class TaskInstance
    {
        public TaskConfig.TaskCfgData TaskData;

        public TaskInstance(TaskConfig.TaskCfgData taskData)
        {
            TaskData = taskData;
        }
    }
}