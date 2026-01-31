using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Face
{
    public class DoubtAreaDetector : MonoBehaviour
    {
        private AreaDetector _areaDetector;

        [LabelText("检测列表")] public List<Transform> TargetList = new();

        [LabelText("怀疑度追加速度")] public float CurrentDoubtSpeed;

        public void Awake()
        {
            _areaDetector = GetComponent<AreaDetector>();
        }

        public void Update()
        {
            CurrentDoubtSpeed = 0;
            
            if (!_areaDetector)
                return;

            foreach (Transform targetTransform in TargetList)
            {
                var index = _areaDetector.GetCurrentAreaIndex(targetTransform.position);
                var speed = GlobalConfig.Instance.DoubtIncreaseConfig.GetValueOrDefault(index, 0.0f);

                CurrentDoubtSpeed += speed;
            }
        }
    }
}