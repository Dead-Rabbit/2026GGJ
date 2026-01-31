using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogicPanel : MonoBehaviour
{
    [LabelText("文本节点")] public TMP_Text TextContent;
    [LabelText("关闭按钮")] public Button CloseBtn;

    public void Awake()
    {
        if (CloseBtn)
        {
            CloseBtn.onClick.AddListener(OnCloseBtnClick);
        }
    }

    /// <summary>
    /// 设置文本内容
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(string content)
    {
        SetHide(true);
        
        TextContent?.SetText(content);
    }

    /// <summary>
    /// 设置显示状态
    /// </summary>
    /// <param name="isHide"></param>
    public void SetHide(bool isHide)
    {
        gameObject.SetActive(!isHide);
    }

    private void OnCloseBtnClick()
    {
        SetHide(true);
    }
}