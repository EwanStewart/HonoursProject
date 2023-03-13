using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using System.Security.Cryptography;
using TMPro;
using UnityEngine.SceneManagement;
public class FirebaseManager : MonoBehaviour
{
    //get objects from scene
    public TMP_InputField passwordInputField;
    public TMP_InputField usernameInputField;
    public TMP_InputField confirmPasswordInputField;
    public Button submitButton;
    
    public bool nextSceneFlag = false;
    public bool usernameTakenFlag = false;
    
    public bool incorrectDetailsFlag = false;
    public TextMeshProUGUI errorText;
    
    public void backToLoginMenu() //load login scene
    {
        SceneManager.LoadScene("sign-login");
    }
    static string encrypt(string password) //encrypt password using sha256
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
    
    public class User //user class to structure data for firebase
    {
        public string username;
        public string password;
        public User(string username, string password)
        {
            this.username = username;
            this.password = encrypt(password);
        }
    }

    void Update()
    {
        if (nextSceneFlag) 
        {
            PlayerPrefs.SetString("username", usernameInputField.text); //save username to playerprefs
            SceneManager.LoadScene("MainMenu");                         //load main menu
        }

        if (usernameTakenFlag)
        {
            errorText.text = "Username already taken";
        }

        if (incorrectDetailsFlag)
        {
            errorText.text = "Incorrect details";
        }
    }
    
    void addUserToFirebase() 
    {
        User user = new User(usernameInputField.text, passwordInputField.text);                                           //create user
        string json = JsonUtility.ToJson(user);                                                                           //convert user to json
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(usernameInputField.text).SetRawJsonValueAsync(json); //add user to firebase
        
		Dictionary<string, object> badgeData = new Dictionary<string, object>();    //create dictionary for badges
		for (int i = 0; i < 12; i++) {                                             //add badges to user
            if (i < 10) {
                badgeData.Add("badge0" + i, false);
            } else {
                badgeData.Add("badge" + i, false);
            }
		}
		FirebaseDatabase.DefaultInstance.GetReference("users").Child(usernameInputField.text).Child("badges").UpdateChildrenAsync(badgeData); //add badges to user
        //add a child to ever
        
        nextSceneFlag = true;
    }
    void checkIfUsernameTaken()
    {
        
        FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task => { //get all users
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result; 
                foreach (DataSnapshot user in snapshot.Children)                            //loop through all users
                {
                    if (user.Child("username").Value.ToString() == usernameInputField.text) //check if username is taken
                    {
                        Debug.Log("Username already taken");
                        usernameTakenFlag = true;
                        return;
                    }
                }
                addUserToFirebase();                                                       //add user to firebase
            }
        });
    }
    
    void checkLoginDetails()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task => { //get all users
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot user in snapshot.Children)                            //loop through all users
                {
                    if (user.Child("username").Value.ToString() == usernameInputField.text) //check if username is taken
                    {
                        if (user.Child("password").Value.ToString() == encrypt(passwordInputField.text)) //check if password is correct
                        {
                            nextSceneFlag = true;
                            return;
                        }
                        else
                        {
                            incorrectDetailsFlag = true;
                            return;
                        }
                    }
                }
                incorrectDetailsFlag = true;
            }
        });
    }
    
    void RegisterAccount()
    {
        if (usernameInputField.text.Length >= 1 && passwordInputField.text.Length >= 1)   //check if inputs aren't empty
        {
            if (passwordInputField.text != confirmPasswordInputField.text)
            {
                errorText.text = "Passwords do not match";
                return;
            }

            checkIfUsernameTaken();                                                     //check if username is taken

        } else {    //set error text
            errorText.text = "Please enter a username and password";
        }

    }

    

    void LoginAccount()
    {
        if (usernameInputField.text.Length >= 1 && passwordInputField.text.Length >= 1)   //check if inputs aren't empty
        {
            if (usernameInputField.text.Length > 15)
            {
                errorText.text = "Username must be no longer than 15 characters";
                return;
            }
            checkLoginDetails();
        } else {   
            errorText.text = "Please enter a username and password";
        }

    }
    
    void Start() { 
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
