using System;
using System.Collections.Generic;
using Framework.Task;
using UnityEngine;

public class TaskPanel : MonoBehaviour
{
    public List<TaskPanelItem> TaskItemList = new();

    public void Update()
    {
        var taskList = GlobalGame.Instance.TaskManager.TaskList;
        List<TaskInfo> activeTaskList = new();
        foreach (var taskInfo in taskList)
        {
            if (taskInfo.IsTaskActive && taskInfo.ShowInPanel)
            {
                activeTaskList.Add(taskInfo);
            }
        }

        for (int i = 0; i < TaskItemList.Count; i++)
        {
            var bActive = i < activeTaskList.Count;

            var taskItem = TaskItemList[i];
            taskItem.gameObject.SetActive(bActive);

            if (bActive)
            {
                taskItem.SetTask(activeTaskList[i]);
            }
        }
    }
}
