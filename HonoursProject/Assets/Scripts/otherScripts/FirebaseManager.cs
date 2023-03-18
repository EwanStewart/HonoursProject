using System.Collections.Generic;
using System.Security.Cryptography;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace otherScripts
{
    public class FirebaseManager : MonoBehaviour
    {
        //get objects from scene
        public TMP_InputField passwordInputField;
        public TMP_InputField usernameInputField;
        public TMP_InputField confirmPasswordInputField;
        public Button submitButton;
    
        private bool _nextSceneFlag = false;
        private bool _usernameTakenFlag = false;
    
        private bool _incorrectDetailsFlag = false;
        public TextMeshProUGUI errorText;
    
        public void BackToLoginMenu() //load login scene
        {
            SceneManager.LoadScene("sign-login");
        }
        static string Encrypt(string password) //encrypt password using sha256
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        private class User //user class to structure data for firebase
        {
            public string username;
            public string password;
            public User(string username, string password)
            {
                this.username = username;
                this.password = Encrypt(password);
            }
        }

        private void Update()
        {
            if (_nextSceneFlag) 
            {
                PlayerPrefs.SetString("username", usernameInputField.text); //save username to playerprefs
                SceneManager.LoadScene("MainMenu");                         //load main menu
            }

            if (_usernameTakenFlag)
            {
                errorText.text = "Username already taken";
            }

            if (_incorrectDetailsFlag)
            {
                errorText.text = "Incorrect details";
            }
        }

        private void AddUserToFirebase() 
        {
            User user = new User(usernameInputField.text, passwordInputField.text);                                           //create user
            string json = JsonUtility.ToJson(user);                                                                           //convert user to json
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(usernameInputField.text).SetRawJsonValueAsync(json); //add user to firebase
            print("here");
            Dictionary<string, object> badgeData = new Dictionary<string, object>();    //create dictionary for badges
            for (int i = 0; i < 12; i++) {                                             //add badges to user
                if (i < 10) {
                    badgeData.Add("badge0" + i, false);
                } else {
                    badgeData.Add("badge" + i, false);
                }
            }
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(usernameInputField.text).Child("badges").UpdateChildrenAsync(badgeData); //add badges to user
            _nextSceneFlag = true;
        }

        private void CheckIfUsernameTaken()
        {
            FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task => { //get all users
                if (task.IsFaulted)
                {
                    print("Error");
                } else if (task.IsCompleted) {
                    var snapshot = task.Result; 
                    foreach (var user in snapshot.Children) {
                        if (user.Child("username").Value.ToString() == usernameInputField.text) { //check if username exists
                            _usernameTakenFlag = true;
                            return;
                        }
                    }
                    AddUserToFirebase();                                                       
                }
            });
        }

        private void CheckLoginDetails()
        {
            FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    print("Error");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    foreach (DataSnapshot user in snapshot.Children)
                    {
                        if (user.Child("username").Value.ToString() == usernameInputField.text) //check if username exists
                        {
                            if (user.Child("password").Value.ToString() == Encrypt(passwordInputField.text)) //check if password is correct
                            {
                                _nextSceneFlag = true;
                                return;
                            }
                        }
                    }
                    _incorrectDetailsFlag = true;
                }
            });
        }

        private void RegisterAccount()
        {
            if (usernameInputField.text.Length >= 1 && passwordInputField.text.Length >= 1)   //check if inputs aren't empty
            {
                if (passwordInputField.text != confirmPasswordInputField.text)
                {
                    errorText.text = "Passwords do not match";
                    return;
                }

                CheckIfUsernameTaken();                                                     //check if username is taken

            } else {    //set error text
                errorText.text = "Please enter a username and password";
            }

        }


        private void LoginAccount()
        {
            if (usernameInputField.text.Length >= 1 && passwordInputField.text.Length >= 1)   //check if inputs aren't empty
            {
                if (usernameInputField.text.Length > 15)
                {
                    errorText.text = "Username must be no longer than 15 characters";
                    return;
                }
                CheckLoginDetails();
            } else {   
                errorText.text = "Please enter a username and password";
            }

        }

        private void Start() { 
            if (confirmPasswordInputField != null) //if register scene
            {
                submitButton.onClick.AddListener(RegisterAccount);
            }
            else                                    //if login scene
            {
                submitButton.onClick.AddListener(LoginAccount); 
            }
        }
    
    }
}
