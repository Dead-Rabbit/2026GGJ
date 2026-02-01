using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelEntry : MonoBehaviour
{
    public TMP_Dropdown diffDropdown;
    
    public void Start()
    {
    }

    /// <summary>
    /// 进入正式游戏场景
    /// </summary>
    public void EnterGameScene()
    {
        GlobalGame.Instance.CurrentDiffIndex = diffDropdown.value;
        SceneManager.LoadSceneAsync(1);
    }
}
