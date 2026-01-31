namespace Framework.Task
{
    public class TaskManager
    {
        public TaskInstance CurrentTask = null;
        
        public void StartTask(string taskId)
        {
            if (CurrentTask != null)
            {
                return;
            }
            
            if (!TaskConfig.TaskCfg.TryGetValue(taskId, out var taskCfgData))
            {
                return;
            }
        }
    }
}