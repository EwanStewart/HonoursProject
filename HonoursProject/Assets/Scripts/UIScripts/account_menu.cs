using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIScripts
{
    public class account_menu : MonoBehaviour
    {
        public void loadScene(string scene) //load scene on parameter value
        {
            SceneManager.LoadScene(scene);
        }
    }
}
