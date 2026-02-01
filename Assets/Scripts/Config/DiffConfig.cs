using System.Collections.Generic;

namespace Config
{
    public class DiffData
    {
        public float EyeDropMin = 2;
        public float EyeDropMax = 5;
        public float MouthDropMin = 2;
        public float MouthDropMax = 5;

        public float DangerAreaScore = 30;
        public float KillAreaScore = 50;
    }

    public class DiffConfig
    {
        public static Dictionary<int, DiffData> Config = new()
        {
            {
                0, new()
                {
                    EyeDropMin = 5,
                    EyeDropMax = 8,
                    
                    MouthDropMin = 3,
                    MouthDropMax = 5f,
                    
                    DangerAreaScore = 30,
                    KillAreaScore = 50
                }
            },
            {
                1, new()
                {
                    EyeDropMin = 12,
                    EyeDropMax = 18,
                    
                    MouthDropMin = 10,
                    MouthDropMax = 15,
                    
                    DangerAreaScore = 20,
                    KillAreaScore = 30
                }
            },
            {
                2, new ()
                {
                    EyeDropMin = 12,
                    EyeDropMax = 18,
                    
                    MouthDropMin = 11,
                    MouthDropMax = 15,
                    
                    DangerAreaScore = 30,
                    KillAreaScore = 50
                }
            }
        };
    }
}