using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPanel : MonoBehaviour
{
    public void EnterRestart()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
