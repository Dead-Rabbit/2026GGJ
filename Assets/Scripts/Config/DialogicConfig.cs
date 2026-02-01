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
                    DialogicContent = "有点饿了上菜吧",
                    ShortContent = "摇铃",
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
                    DialogicContent = "你是怎么过来的？",
                    ShortContent = "回答问题",
                    Options = new()
                    {
                        "我不吃牛肉", "雨下的的很大", "开车来的", "我很好"
                    },
                    RightIndex = new List<int>()
                    {
                        2
                    }
                }
            },
            {
                5, new()
                {
                    DialogicContent = "你吃牛肉吗",
                    ShortContent = "回答问题",
                    Options = new()
                    {
                        "我不吃牛肉", "屁股在树上", "我喜欢你", "雨下的很大"
                    },
                    RightIndex = new List<int>()
                    {
                        0
                    }
                }
            },
            {
                6, new()
                {
                    DialogicContent = "你看起来不太好",
                    ShortContent = "回答问题",
                    Options = new()
                    {
                        "开车来的", "我很好", "对不起", "再见"
                    },
                    RightIndex = new List<int>()
                    {
                        1
                    }
                }
            },
            {
                7, new()
                {
                    DialogicContent = "今天天气真好啊",
                    ShortContent = "回答问题",
                    Options = new()
                    {
                        "我这有伞", "窝要验牌", "我同意", "牌妹尤问题"
                    },
                    RightIndex = new List<int>()
                    {
                        2
                    }
                }
            },
        };
    }
}