using UnityEngine;
using UnityEngine.SceneManagement;
public class Credits : MonoBehaviour
{
    public void OnReturn()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
