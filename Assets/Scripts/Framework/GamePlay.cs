using UnityEngine;

namespace Framework
{
    public class GamePlay : MonoBehaviour
    {
        public void Awake()
        {
            GlobalEvent.Instance.OnStart += OnGameStart;
        }

        public void Start()
        {
            GlobalEvent.Instance.OnStart?.Invoke();
        }

        #region 生命周期

        private void OnGameStart()
        {
            GenerateMainPlayer();
        }

        #endregion


        #region Player

        private void GenerateMainPlayer()
        {
            var newObject = new GameObject("MainPlayer");
            var mainPlayer = newObject.AddComponent<MainPlayer>();
            GlobalGame.Instance.mainPlayer = mainPlayer;
        }

        #endregion
    }
}