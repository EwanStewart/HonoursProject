using System.Collections.Generic;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIScripts
{
    public class BadgeGallery : MonoBehaviour
    {
        public TextMeshProUGUI usernameText; //text object to store username
        private readonly Dictionary<string, object> _data = new(); //dictionary to store badges from firebase
        private bool _flag;   //bool flag to control when badges have been loaded from firebase

        private static void SetBadgeData(string badgeName)
        {
            BadgeInfo.CrossSceneInformation = badgeName;
            SceneManager.LoadScene("IndividualBadgeView");
        }
        
        private void GetBadges() 
        {
            //get badges from firebase using username stored in playerprefs
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted) //if there is an error, return
                {
                    Debug.Log("Error");
                    return;
                }

                if (!task.IsCompleted) return; //if task is completed, add badges to dictionary
                var snapshot = task.Result;
                foreach (var badge in snapshot.Children)
                {
                    _data.Add(badge.Key, badge.Value);
                }
                _flag = true; //set flag to denote that badges have been loaded
            });
        }

        public void LoadScene(string scene) //function to load from parameter value
        {
            SceneManager.LoadScene(scene);
        }

        public void ReturnToMainMenu() //function to return to main menu
        {
            SceneManager.LoadScene("MainMenu");
        }
        void Start()
        {
            if (PlayerPrefs.HasKey("username")) //check if username is stored in playerprefs, else load sign-in scene
            {
                usernameText.text = PlayerPrefs.GetString("username") + "'s Badges";
            } else {
                SceneManager.LoadScene("sign-login");
            }
            GetBadges(); //call getBadges function to load badges from firebase
        }

        void Update()
        {
            if (!_flag) return; //if badges have been loaded, change colour of badge to red if false, else white
            foreach (var badge in _data) //loop through badge dictionary
            {
                try {
                    if (badge.Value.ToString() != "True") continue;
                    var obj = GameObject.Find(badge.Key);
                    var child = obj.transform.GetChild(obj.transform.childCount - 1);
                    Destroy(child.gameObject);
                    var button = obj.GetComponent<Button>();
                    button.onClick.AddListener(() => SetBadgeData(badge.Key));
                    _flag = false;
                } catch {
                    Debug.Log("Error");
                }

            }
        }
    }
}
