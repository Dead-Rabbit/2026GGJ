using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PanelEntry : MonoBehaviour
{
    public TMP_Dropdown diffDropdown;

    public List<Transform> HideItems;
    /// <summary>
    /// 进入正式游戏场景
    /// </summary>
    public void EnterGameScene()
    {
        GlobalGame.Instance.CurrentDiffIndex = diffDropdown.value;
        StartCoroutine(GotoStart());
    }

    private IEnumerator GotoStart()
    {
        // 隐藏UI
        foreach (var hideItem in HideItems)
        {
            hideItem.gameObject.SetActive(false);
        }
        
        GlobalEvent.Instance.OnStart?.Invoke();

        yield return new WaitForSeconds(1.0f);
        
        SceneManager.LoadSceneAsync(1);
    }
}
