using System;
using System.Collections.Generic;
using Framework.Task;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogicPanel : MonoBehaviour
{
    [LabelText("文本节点")] public TMP_Text TextContent;
    [LabelText("关闭按钮")] public Button CloseBtn;

    [LabelText("回答选项")] public List<DialogicOption> OptionsViewList = new();

    private TaskInfo currentTaskInfo = null;
    private string currentShowContent = "";
    private Dictionary<int, bool> secContentMap = new();

    public void Awake()
    {
        if (CloseBtn)
        {
            CloseBtn.onClick.AddListener(OnCloseBtnClick);
        }

        for (int i = 0; i < OptionsViewList.Count; i++)
        {
            var option = OptionsViewList[i];
            int index = i; // 必须复制到局部变量，否则 lambda 会捕获到循环结束后的值
            var btn = option.GetComponent<Button>();
            if (btn)
                btn.onClick.AddListener(() => OnClickOption(index));
        }
    }

    /// <summary>
    /// 设置文本内容
    /// </summary>
    /// <param name="taskInfo"></param>
    public void SetTaskInfo(TaskInfo taskInfo)
    {
        SetHide(false);

        currentTaskInfo = taskInfo;

        var dialogicData = taskInfo?.GetDialogicData();
        if (dialogicData == null)
        {
            currentShowContent = "";
            secContentMap.Clear();
            SetShowContent();
            return;
        }

        currentShowContent = dialogicData.DialogicContent ?? "";
        secContentMap.Clear();

        List<string> options = dialogicData.Options ?? new List<string>();
        if (taskInfo is AnswerTaskInfo)
        {
            for (var i = 0; i < currentShowContent.Length; i++)
            {
                secContentMap[i] = Random.Range(0, 100) > 90;
            }
        }

        for (int i = 0; i < OptionsViewList.Count; i++)
        {
            var optionItem = OptionsViewList[i];
            var bActive = i < options.Count;
            optionItem.gameObject.SetActive(bActive);
            if (bActive)
            {
                optionItem.SetContent(options[i]);
            }
        }

        SetShowContent();
    }

    private void SetShowContent()
    {
        var showContent = currentShowContent;
        
        // 如果是加密
        if (secContentMap.Count > 0)
        {
            for (var i = 0; i < showContent.Length; i++)
            {
                if (!secContentMap.GetValueOrDefault(i))
                {
                    // 替换字符为加密的 * 样式
                    showContent = showContent.Substring(0, i) + "*" + showContent.Substring(i + 1);
                }
            }
        }
        
        // 标题内容
        TextContent?.SetText(showContent);
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
        if (currentTaskInfo is AnswerTaskInfo)
        {
            // 解密：遍历副本避免修改集合时异常
            var keys = new List<int>(secContentMap.Keys);
            foreach (var key in keys)
            {
                if (!secContentMap.GetValueOrDefault(key, true))
                    secContentMap[key] = Random.Range(0, 100) > 20;
            }
            SetShowContent();
        }
        else
        {
            SetHide(true);
        }
    }

    private void OnClickOption(int index)
    {
        if (currentTaskInfo == null)
        {
            SetHide(true);
            return;
        }

        var dialogicData = currentTaskInfo.GetDialogicData();
        if (dialogicData == null)
        {
            SetHide(true);
            return;
        }

        var rightIndex = dialogicData.RightIndex;
        bool isCorrect = rightIndex != null && rightIndex.Contains(index);

        if (isCorrect)
            currentTaskInfo.SetSuccess();
        else
            currentTaskInfo.SetFailed();

        SetHide(true);
    }
}