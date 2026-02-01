using System.Collections.Generic;

namespace Config
{
    public class DialogicData
    {
        public string DialogicContent = "";
        public string ShortContent = "";
            
        // 有选项的
        public List<string> Options;
        public List<int> RightIndex;
    }

    public static class DialogicConfig
    {
        public static Dictionary<int, DialogicData> DialogicCfg = new()
        {
            {
                1, new()
                {
                    ShortContent = "点燃蜡烛吧",
                }
            },
            {
                2, new()
                {
                    DialogicContent = "我饿了，想吃煎饼",
                    ShortContent = "摇铃叫服务生来吧",
                }
            },
            {
                3, new()
                {
                    DialogicContent = "这不是我想吃的，给你吃了",
                    ShortContent = "摇铃叫服务生来吧",
                }
            },
            {
                4, new()
                {
                    DialogicContent = "问题1问题1问题1问题1问题1问题1",
                    ShortContent = "回答问题",
                    Options = new()
                    {
                        "答案1", "答案2", "答案3", "答案4"
                    },
                    RightIndex = new List<int>()
                    {
                        0
                    }
                }
            },
            {
                5, new()
                {
                    DialogicContent = "问题2问题2问题2问题2问题2问题2",
                    ShortContent = "回答问题2",
                    Options = new()
                    {
                        "答案1", "答案2", "答案3", "答案4"
                    },
                    RightIndex = new List<int>()
                    {
                        0
                    }
                }
            },
        };
    }
}