using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class GamePlay : MonoBehaviour
    {
        [LabelText("怀疑度进度条")] public Slider DoubtSlider;
        [LabelText("怀疑度")] public float CurrentDoubtValue = 0;

        public void Awake()
        {
        }

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
            
            // 怀疑度掉落
            CurrentDoubtValue -= dt * GlobalConfig.Instance.DoubtRevertSpeed;
            CurrentDoubtValue = Mathf.Clamp(CurrentDoubtValue, 0, GlobalConfig.Instance.MaxDoubtValue);
            
            // 同步显示怀疑度
            DoubtSlider.value = CurrentDoubtValue;
        }

        #endregion
    }
}