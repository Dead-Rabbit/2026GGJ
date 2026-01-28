using UnityEngine;

namespace Framework
{
    public class GamePlay : MonoBehaviour
    {
        public void Awake()
        {
        }

        public void Start()
        {
            GenerateMainPlayer();

            GlobalEvent.Instance.OnStart?.Invoke();
        }

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