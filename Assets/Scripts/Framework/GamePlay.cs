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
            GlobalEvent.Instance.OnStart?.Invoke();
        }
    }
}