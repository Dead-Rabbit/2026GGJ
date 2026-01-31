using UnityEngine;

namespace Framework.Task
{
    public class WaiterTaskInfo : TaskInfo
    {
        public override int DialogicId => 2;
    
        public override float DuringTime => 10;
        public override float FailedScore => 50;
        
        public override bool ShowInPanel => true;

        private float NextRunRemainTime = 0;

        public WaiterTaskInfo()
        {
            GlobalEvent.Instance.OnRattleWorlk += () =>
            {
                if (IsTaskActive)
                {
                    SetSuccess();
                    
                    // 召唤服务员
                    GamePlay.Instance.DialogicPanel.SetHide(true);
                }
            };
        }
    
        public override bool CanEnter()
        {
            // 倒计时
            if (NextRunRemainTime > 0)
            {
                NextRunRemainTime -= Time.deltaTime;
                return false;
            }
        
            var taskList = GlobalGame.Instance.TaskManager.TaskList;
            foreach (var taskInfo in taskList)
            {
                // 如果当前有吃饭任务正在进行
                if (taskInfo.IsTaskActive && taskInfo is AnswerTaskInfo)
                {
                    NextRunRemainTime = Random.Range(GlobalConfig.Instance.EventStartMinTime,
                        GlobalConfig.Instance.EventStartMaxTime);
                
                    return false;
                }
            }

            return true;
        }
    
        public override void OnEnter()
        {
            GamePlay.Instance.DialogicPanel.SetTaskInfo(this);
        }
        
        public override void OnLeave()
        {
            NextRunRemainTime = Random.Range(GlobalConfig.Instance.EventStartMinTime,
                GlobalConfig.Instance.EventStartMaxTime);
        }
    }
}