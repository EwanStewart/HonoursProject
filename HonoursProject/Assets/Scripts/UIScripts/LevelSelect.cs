using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIScripts
{
    public class LevelSelect : MonoBehaviour
    {
        public void loadScene(string scene)
        {
            SceneManager.LoadScene(scene);

        }

        public void quitGame()
        {
            Application.Quit();
        }
    }
}
