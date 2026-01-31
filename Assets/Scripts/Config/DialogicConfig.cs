using System.Collections.Generic;

namespace Config
{
    public static class DialogicConfig
    {
        public static List<DialogicData> DialogicCfg = new()
        {
            new ()
            {
                Name = "对面角色",
                Content = "今天天气还不错啊",
                Options = new()
                {
                    "是啊", "#￥%#@", "味道不错"
                },
                RightIndex = 1,
            }
        };

        #region Define

        public class DialogicData
        {
            public string PeopleIcon = "";
            public string Name = "A";
            public string Content = "";
            public List<string> Options;
            public int RightIndex;
            
            // TODO 期望表情
        }

        #endregion
    }
}