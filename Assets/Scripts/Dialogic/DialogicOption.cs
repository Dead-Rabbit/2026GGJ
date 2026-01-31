using TMPro;
using UnityEngine;

public class DialogicOption : MonoBehaviour
{
    public TMP_Text text;

    public void SetContent(string content)
    {
        text.text = content;
    }
}