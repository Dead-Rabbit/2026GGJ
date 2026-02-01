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

        public float CandleBurnMinTime = 10;
        public float CandleBurnMaxTime = 15;
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
                    KillAreaScore = 50,
                    
                    CandleBurnMinTime = 10,
                    CandleBurnMaxTime = 15,
                }
            },
            {
                1, new()
                {
                    EyeDropMin = 7,
                    EyeDropMax = 10,
                    
                    MouthDropMin = 6,
                    MouthDropMax = 9,
                    
                    DangerAreaScore = 20,
                    KillAreaScore = 30,
                    
                    CandleBurnMinTime = 8,
                    CandleBurnMaxTime = 14,
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
                    KillAreaScore = 50,
                    
                    CandleBurnMinTime = 6,
                    CandleBurnMaxTime = 15,
                }
            },
            {
                3, new ()
                {
                    EyeDropMin = 0,
                    EyeDropMax = 0,
                    
                    MouthDropMin = 0,
                    MouthDropMax = 0,
                    
                    DangerAreaScore = 0,
                    KillAreaScore = 0,
                }
            }
        };
    }
}