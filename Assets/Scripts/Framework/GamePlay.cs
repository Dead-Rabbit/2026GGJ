using System.Collections.Generic;
using Character.Face;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class GamePlay : MonoBehaviour
    {
        [LabelText("怀疑度进度条")] public Slider DoubtSlider;
        [LabelText("怀疑度")] public float CurrentDoubtValue = 0;

        [LabelText("对话框")] public DialogicPanel DialogicPanel;
        
        [HideInInspector] public List<DoubtAreaDetector> DoubtDetectors = new();

        public void Start()
        {
            GlobalEvent.Instance.OnStart?.Invoke();

            // 初始化怀疑度
            if (DoubtSlider != null)
            {
                DoubtSlider.maxValue = GlobalConfig.Instance.MaxDoubtValue;
            }
        }

        public void Update()
        {
            UpdateDoubt();
        }

        #region 怀疑度

        private void UpdateDoubt()
        {
            if (!DoubtSlider)
                return;

            var dt = Time.deltaTime;

            var doubtSpeed = -GlobalConfig.Instance.DoubtRevertSpeed;
            
            // 各个器官的怀疑度
            foreach (var doubtAreaDetector in DoubtDetectors)
            {
                doubtSpeed += doubtAreaDetector.CurrentDoubtSpeed;
            }
            
            // 怀疑度掉落
            CurrentDoubtValue += dt * doubtSpeed;
            CurrentDoubtValue = Mathf.Clamp(CurrentDoubtValue, 0, GlobalConfig.Instance.MaxDoubtValue);
            
            // 同步显示怀疑度
            DoubtSlider.value = CurrentDoubtValue;
        }

        #endregion
    }
}