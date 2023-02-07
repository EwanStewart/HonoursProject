using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI usernameText;
    public void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);

    }

    void Start()
    {
        if (PlayerPrefs.HasKey("username")) //check if user is logged in
        {
            usernameText.text = "Welcome, " + PlayerPrefs.GetString("username");
        } else //if not logged in, go to sign in page
        {
            SceneManager.LoadScene("sign-login");
        }
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
