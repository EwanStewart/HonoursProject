using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;
using TMPro;
public class badgeGallery : MonoBehaviour
{
	public TextMeshProUGUI usernameText;
    private Dictionary<string, object> data = new Dictionary<string, object>();
    public bool flag = false;
    public void getBadges()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
				Debug.Log("Error");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot badge in snapshot.Children)
                {
                    data.Add(badge.Key, badge.Value);
                    
                }
                flag = true;
            }
        });
        
        

    }

    public void loadScene(string scene)
    {
       SceneManager.LoadScene(scene);
    }

	public void returnToMainMenu()
    {
	   SceneManager.LoadScene("MainMenu");
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            usernameText.text = PlayerPrefs.GetString("username") + "'s Badges";
        } else
        {
            SceneManager.LoadScene("sign-login");
        }

        getBadges(); 
        
    }

    void Update()
    {
        if (flag)
        {
            foreach (KeyValuePair<string, object> badge in data)
            {
                try
                {
                    if (badge.Value.ToString() == "False")
                    {
                        GameObject.Find(badge.Key).GetComponent<Image>().color = Color.red;
                        flag = false;
                    } else
                    {
                        GameObject.Find(badge.Key).GetComponent<Image>().color = Color.white;
                        flag = false;
                    }
                } catch
                {
                    Debug.Log("Error");
                }

            }
        }
    }
}
