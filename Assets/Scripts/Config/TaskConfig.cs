using System.Collections.Generic;
using Config;

public static class TaskConfig
{
    public static Dictionary<string, TaskCfgData> TaskCfg = new()
    {
        // 点燃蜡烛
        {
            "Burn1", new()
            {
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