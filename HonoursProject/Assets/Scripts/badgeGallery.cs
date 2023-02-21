using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;
using TMPro;
public class badgeGallery : MonoBehaviour
{
	public TextMeshProUGUI usernameText; //text object to store username
    private Dictionary<string, object> data = new Dictionary<string, object>(); //dictionary to store badges from firebase
    public bool flag = false;   //bool flag to control when badges have been loaded from firebase
    public void getBadges() 
    {
        //get badges from firebase using username stored in playprefs
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted) //if there is an error, return
            {
				Debug.Log("Error");
                return;
            }
            else if (task.IsCompleted) //if task is completed, add badges to dictionary
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot badge in snapshot.Children)
                {
                    data.Add(badge.Key, badge.Value);
                }
                flag = true; //set flag to denote that badges have been loaded
            }
        });
    }

    public void loadScene(string scene) //function to load from parameter value
    {
       SceneManager.LoadScene(scene);
    }

	public void returnToMainMenu() //function to return to main menu
    {
	   SceneManager.LoadScene("MainMenu");
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("username")) //check if username is stored in playprefs, else load sign-in scene
        {
            usernameText.text = PlayerPrefs.GetString("username") + "'s Badges";
        } else {
            SceneManager.LoadScene("sign-login");
        }
        getBadges(); //call getBadges function to load badges from firebase
    }

    void Update() 
    {
        if (flag) //if badges have been loaded, change colour of badge to red if false, else white
        {
            foreach (KeyValuePair<string, object> badge in data) //loop through badge dictionary
            {
                try { //try catch to anticipate badges being incorrectly set in firebase
                    if (badge.Value.ToString() == "False")
                    {
                        GameObject.Find(badge.Key).GetComponent<Image>().color = Color.red; //change colour of badge to red
                        flag = false;
                    } else {
                        GameObject.Find(badge.Key).GetComponent<Image>().color = Color.white; //change colour of badge to white
                        flag = false;
                    }
                } catch {
                    Debug.Log("Error");
                }

            }
        }
    }
}
