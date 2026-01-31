using System.Collections.Generic;

namespace Config
{
    public class DialogicData
    {
        public string DialogicContent = "";
        public string ShortContent = "";
            
        // 有选项的
        public List<string> Options;
        public int RightIndex;
            
        // 
            
        // TODO 期望表情
    }

    public static class DialogicConfig
    {
        public static Dictionary<int, DialogicData> DialogicCfg = new()
        {
            {
                1, new()
                {
                    DialogicContent = "今天天气还不错啊，点燃蜡烛吧",
                    ShortContent = "点燃蜡烛吧",
                }
            }
        };

        #region Define
        
        

        #endregion
    }
}