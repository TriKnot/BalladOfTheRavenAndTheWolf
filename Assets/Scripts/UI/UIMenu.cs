using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIMenu : MonoBehaviour
{
    [SerializeField] Button optionsButton;
    [SerializeField] Button backButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button creditsBackButton;

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Options()
    {
        optionsButton.Select();
    }
    public void Back()
    {
        backButton.Select();
    }

    public void Credits()
    {
        creditsBackButton.Select();
    }

    public void CreditsBack()
    {
        backButton.Select();
    }
}
