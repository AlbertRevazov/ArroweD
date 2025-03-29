using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start() { }
    void Update()
    {
        ChooseRegime();
    }

    void ChooseRegime()
    {
        int infinityRegime = SceneManager.sceneCountInBuildSettings;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(infinityRegime - 1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(1);
        }
    }
}
