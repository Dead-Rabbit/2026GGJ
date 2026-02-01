using System;
using System.Collections.Generic;
using Character.Face;
using Config;
using Framework.Task;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Framework
{
    public class GamePlay : MonoBehaviour
    {
        public TaskManager TaskManager;

        [LabelText("背景音乐")] public AudioClip BackgroundAudio;
        [LabelText("背景音源（空则用本物体上的 AudioSource）")]
        [SerializeField] private AudioSource _backgroundAudioSource;

        [LabelText("怀疑列表")] public List<DoubtAreaDetector> DoubtDetectors = new();
        [LabelText("怀疑度进度条")] public Slider DoubtSlider;
        [LabelText("怀疑度")] public float CurrentDoubtValue = 0;

        [LabelText("对话框")] public DialogicPanel DialogicPanel;

        [LabelText("蜡烛列表")] public List<CandleInstance> CandleInstanceList = new();

        [LabelText("服务员")] public WaiterInstance WaiterInstance;

        [LabelText("剩余时间Text")] public TMP_Text Txt_RemainTime;

        [HideInInspector] public float CurrentGameTime;

        /// <summary>
        /// 难度配置
        /// </summary>
        public DiffData DiffData;
        
        public Dictionary<int, float> DoubtIncreaseConfig = new();
        
        public static GamePlay Instance;

        public bool IsActive = false;

        public int TaskFailCombo = 0;

        public void Awake()
        {
            Instance = this;

            DiffData = DiffConfig.Config.GetValueOrDefault(GlobalGame.Instance.CurrentDiffIndex);
            
            // 读入权重
            DoubtIncreaseConfig.Add(0, 0);
            DoubtIncreaseConfig.Add(1, DiffData.DangerAreaScore);
            DoubtIncreaseConfig.Add(-1, DiffData.KillAreaScore);
        }

        public void Start()
        {
            Cursor.visible = DiffData.bShowCursor;
            
            IsActive = true;
            
            // 初始化怀疑度
            if (DoubtSlider != null)
            {
                DoubtSlider.maxValue = GlobalConfig.Instance.MaxDoubtValue;
            }

            CurrentGameTime = 0;
            
            TaskManager = new TaskManager();
            TaskManager.Init();

            PlayBackgroundAudio();
        }

        public void Update()
        {
            if (!IsActive)
                return;
            
            UpdateDoubt();
        }

        public void OnDestroy()
        {
            Cursor.visible = true;
            StopBackgroundAudio();
        }

        /// <summary>
        /// 播放背景音乐（若已配置 BackgroundAudio）。
        /// </summary>
        public void PlayBackgroundAudio()
        {
            if (BackgroundAudio == null) return;

            var source = _backgroundAudioSource != null ? _backgroundAudioSource : GetComponent<AudioSource>();
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
                _backgroundAudioSource = source;
            }

            source.clip = BackgroundAudio;
            source.loop = true;
            source.Play();
        }

        /// <summary>
        /// 停止背景音乐。
        /// </summary>
        public void StopBackgroundAudio()
        {
            var source = _backgroundAudioSource != null ? _backgroundAudioSource : GetComponent<AudioSource>();
            if (source != null && source.isPlaying)
                source.Stop();
        }

        public void FixedUpdate()
        {
            var dt = Time.fixedDeltaTime;
            CurrentGameTime += dt;

            if (CurrentGameTime >= GlobalConfig.Instance.GameDuringTime)
            {
                // 游戏结束
                IsActive = false;
                GlobalEvent.Instance.OnSuccess?.Invoke();
                
                SceneManager.LoadSceneAsync(2);

                return;
            }
            
            Txt_RemainTime?.SetText($"剩余时间: {(GlobalConfig.Instance.GameDuringTime - CurrentGameTime).ToString("F0") }s");
            
            TaskManager.OnUpdate(dt);
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

            if (CurrentDoubtValue >= GlobalConfig.Instance.MaxDoubtValue)
            {
                // 游戏结束
                IsActive = false;
                GlobalEvent.Instance.OnFail?.Invoke();
                
                SceneManager.LoadSceneAsync(3);
            }
        }

        #endregion
    }
}