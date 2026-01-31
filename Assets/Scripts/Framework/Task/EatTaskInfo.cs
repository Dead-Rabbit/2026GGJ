using Framework.Task;

public class EatTaskInfo : TaskInfo
{
    public override float DuringTime => 10;
    public override float FailedScore => 50;
    
    public override bool ShowInPanel => true;

    public override bool CanEnter()
    {
        return true;
    }
}
