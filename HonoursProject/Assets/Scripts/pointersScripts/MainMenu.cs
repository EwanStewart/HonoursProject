using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI usernameText;
    public GameObject badgePanel;

    public void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);

    }

    private void ClearFeedback()
    {
        badgePanel.SetActive(false);
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
        
        if (!PlayerPrefs.HasKey("badge00Unlocked"))
        {
            PlayerPrefs.SetInt("badge00Unlocked", 99);
        }
    }

    void Update()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            if (PlayerPrefs.GetInt("badge00Unlocked") == 99) {
                FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").Child("badge00").SetValueAsync(true);
                PlayerPrefs.SetInt("badge00Unlocked", 0);
                badgePanel.SetActive(true);
                Invoke(nameof(ClearFeedback), 5);
            }
        }
    }
    
    public void quitGame()
    {
        Application.Quit();
    }
    
    public void wipeGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("sign-login");
    }
}
