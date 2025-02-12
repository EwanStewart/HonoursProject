using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace pointersScripts
{
    public class handlerBadge : MonoBehaviour
    {
        public GameObject canvas;


        // Update is called once per frame
        void Update()
        {
            TextMeshProUGUI countText = canvas.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
            if (countText.text == "3/3")
            {
                unlockBadge();
            }
        }
    
        void unlockBadge() 
        {
            TextMeshProUGUI countText = canvas.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

            canvas.SetActive(true);
            countText.gameObject.SetActive(false);
        
            if (PlayerPrefs.HasKey("username"))
            {
                FirebaseDatabase.DefaultInstance.GetReference("users").Child(PlayerPrefs.GetString("username")).Child("badges").Child("badge01").SetValueAsync(true);
            } else
            {
                SceneManager.LoadScene("sign-login");
            }
        
            Invoke("loadPointers", 3);
        }
    
        void loadPointers()
        {
            SceneManager.LoadScene("Pointers");
        }
    }
}
