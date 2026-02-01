using System.Collections.Generic;
using Framework;
using Framework.Task;
using UnityEngine;

public class AnswerTaskInfo : TaskInfo
{
    public override int DialogicId => _dialogicId;

    private int _dialogicId = 0;

    public override float DuringTime => 10;
    public override float FailedScore => 180;
        
    public override bool ShowInPanel => true;

    private float NextRunRemainTime = 0;
    
    public override bool CanEnter()
    {
        var taskList = GamePlay.Instance.TaskManager.TaskList;
        foreach (var taskInfo in taskList)
        {
            // 如果当前有吃饭任务正在进行
            if (taskInfo.IsTaskActive && taskInfo is WaiterTaskInfo)
            {
                NextRunRemainTime = Random.Range(GlobalConfig.Instance.EventStartMinTime,
                    GlobalConfig.Instance.EventStartMaxTime);
                
                return false;
            }
        }

        // 倒计时
        if (NextRunRemainTime > 0)
        {
            NextRunRemainTime -= Time.deltaTime;
            return false;
        }

        return true;
    }
    
    public override void OnEnter()
    {
        var dialogicList = new List<int>() { 4, 5, 6, 7 };
        _dialogicId = dialogicList[Random.Range(0, dialogicList.Count)];
        
        GamePlay.Instance.DialogicPanel.SetTaskInfo(this);
    }

    public override void OnLeave()
    {
        NextRunRemainTime = Random.Range(GlobalConfig.Instance.EventStartMinTime,
            GlobalConfig.Instance.EventStartMaxTime);
                    
        GamePlay.Instance.DialogicPanel.SetHide(true);
    }
}