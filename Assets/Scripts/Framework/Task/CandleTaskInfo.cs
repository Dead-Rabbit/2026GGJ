namespace Framework.Task
{
    public class CandleTaskInfo : TaskInfo
    {
        public override float DuringTime => 10;
        public override float FailedScore => 50;
        
        public override bool ShowInPanel => true;
        
        public override bool CanEnter()
        {
            var candleList = GamePlay.Instance.CandleInstanceList;
            var canEnter = false;
            foreach (var candleInstance in candleList)
            {
                if (!candleInstance.IsBurning)
                {
                    canEnter = true;
                    break;
                }
            }
            
            return canEnter;
        }

        protected override bool CheckSuccessCondition()
        {
            var candleList = GamePlay.Instance.CandleInstanceList;
            foreach (var candleInstance in candleList)
            {
                if (!candleInstance.IsBurning)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}