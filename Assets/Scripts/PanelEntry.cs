using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelEntry : MonoBehaviour
{
    public void Start()
    {
        var gameGlobal = GlobalGame.Instance;
    }

    /// <summary>
    /// 进入正式游戏场景
    /// </summary>
    public void EnterGameScene()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
