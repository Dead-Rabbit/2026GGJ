using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelEntry : MonoBehaviour
{
    public void Start()
    {
    }

    /// <summary>
    /// 进入正式游戏场景
    /// </summary>
    public void EnterGameScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
