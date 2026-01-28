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
            var newObject = Object.Instantiate(GlobalConfig.Instance.MainPlayer);
            var mainPlayer = newObject.GetComponent<MainPlayer>();
            if (mainPlayer)
            {
                GlobalGame.Instance.mainPlayer = mainPlayer;
            }
        }

        #endregion
    }
}