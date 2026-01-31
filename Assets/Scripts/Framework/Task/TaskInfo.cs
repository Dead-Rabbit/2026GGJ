using System.Collections.Generic;
using Config;

namespace Framework.Task
{
    public class TaskInfo
    {
        public int DialogicId = 1;
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
        
        public bool GetDialogicData(out DialogicData dialogicData)
        {
            return DialogicConfig.DialogicCfg.TryGetValue(DialogicId, out dialogicData);
        }

        public void SetSuccess()
        {
            // TODO 加分
            
            Leave();
        }
        
        public void SetFailed()
        {
            // TODO 扣分

            Leave();
        }
        
        public void Leave()
        {
            IsTaskActive = false;
            
        }

        public virtual void OnLeave()
        {
            
        }
    }
}