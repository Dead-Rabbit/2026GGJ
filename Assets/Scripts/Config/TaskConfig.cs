using System.Collections.Generic;

public static class TaskConfig
{
    public static Dictionary<string, TaskCfgData> TaskCfg = new()
    {
        // 点燃蜡烛
        {
            "Burn1", new()
            {
                DialogicId = 1,
                DuringTime = 10,
            }
        }
    };

    #region Define

    public class TaskCfgData
    {
        public int DialogicId;
        public float DuringTime;
    }

    #endregion
}