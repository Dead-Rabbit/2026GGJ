using System.Collections.Generic;
using Config;

namespace Framework.Task
{
    public class TaskInfo
    {
        public virtual int DialogicId => 1;
        public virtual float DuringTime => 10;
        public virtual float FailedScore => 0;
        public virtual float SuccessScore => 0;

        public virtual bool ShowInPanel => false;

        public bool IsTaskActive;
        public float RemainTime;

        public virtual bool CanEnter()
        {
            return false;
        }

        public void Enter()
        {
            IsTaskActive = true;
            RemainTime = DuringTime;
            
            OnEnter();
        }
        
        public virtual void OnEnter()
        {
        }

        protected virtual bool CheckSuccessCondition()
        {
            return false;
        }
        
        public virtual void OnUpdate(float dt)
        {
            if (!IsTaskActive)
                return;

            if (CheckSuccessCondition())
            {
                SetSuccess();
                return;
            }

            RemainTime -= dt;
            if (RemainTime <= 0)
            {
                SetFailed();
                return;
            }
        }
        
        public DialogicData GetDialogicData()
        {
            return DialogicConfig.DialogicCfg.GetValueOrDefault(DialogicId);
        }

        public void SetSuccess()
        {
            GamePlay.Instance.CurrentDoubtValue -= SuccessScore;
            Leave();
        }
        
        public void SetFailed()
        {
            GamePlay.Instance.CurrentDoubtValue += FailedScore;
            Leave();
        }
        
        public void Leave()
        {
            IsTaskActive = false;

            OnLeave();
        }

        public virtual void OnLeave()
        {
            
        }
    }
}