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
        
        //initialise time based badges
        if (!PlayerPrefs.HasKey("badge00Unlocked"))
        {
            PlayerPrefs.SetInt("badge00Unlocked", 99);
        }
        if (!PlayerPrefs.HasKey("badge08Unlocked"))
        {
            PlayerPrefs.SetInt("badge08Unlocked", 99);
        }
    }

    void Update()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            //if badge hasen't been unlocked yet, check if it should be unlocked
            if (PlayerPrefs.GetInt("badge00Unlocked") == 99) {
                PlayerPrefs.SetString("start", System.DateTime.Now.ToString());
                FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").Child("badge00").SetValueAsync(true);
                PlayerPrefs.SetInt("badge00Unlocked", 0);
                badgePanel.SetActive(true);
                Invoke(nameof(ClearFeedback), 5);
            }
            
            //if badge hasen't been unlocked yet, check if it should be unlocked
            if (PlayerPrefs.GetInt("badge08Unlocked") == 99) {
                if (PlayerPrefs.GetInt("PointersCompleted") == 1 && PlayerPrefs.GetInt("SortingCompleted") == 1 && PlayerPrefs.GetInt("LinkedListsCompleted") == 1) {
                    PlayerPrefs.SetString("end", System.DateTime.Now.ToString());
                    var diff = System.DateTime.Parse(PlayerPrefs.GetString("end")) - System.DateTime.Parse(PlayerPrefs.GetString("start"));
                    if (diff.TotalMinutes < 60) {
                        FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").Child("badge11").SetValueAsync(true);
                    } 
                    if (diff.TotalMinutes < 30) {
                        FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").Child("badge10").SetValueAsync(true);
                    } 
                    
                    FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username"))
                        .Child("badges").Child("badge09").SetValueAsync(true);
                    badgePanel.SetActive(true);
                    badgePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Completed it!";
                    PlayerPrefs.SetInt("badge08Unlocked", 0);
                    Invoke(nameof(ClearFeedback), 5);
                }
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
