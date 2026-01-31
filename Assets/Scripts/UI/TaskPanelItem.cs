using System;
using Framework.Task;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanelItem : MonoBehaviour
{
    public TMP_Text Title;
    public Slider Process;

    private TaskInfo _taskInfo;
    
    public void SetTask(TaskInfo taskInfo)
    {
        _taskInfo = taskInfo;

        if (_taskInfo.GetDialogicData(out var dialogicData))
        {
            Title.SetText(dialogicData.ShortContent);
        }
    }

    public void Update()
    {
        var percent = 0.0f;
        if (_taskInfo.DuringTime > 0)
        {
            percent = 1 - _taskInfo.RemainTime / _taskInfo.DuringTime;
        }

        percent = Mathf.Clamp01(percent);
        Process.value = percent;
    }
}