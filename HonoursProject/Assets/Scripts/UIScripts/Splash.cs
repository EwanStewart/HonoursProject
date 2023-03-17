using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace UIScripts
{

    public class Splash : MonoBehaviour
    {
        public Slider progressBar;

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                progressBar.value += asyncLoad.progress;
                yield return null;
            }
        }
        
        void Start()
        {
            var list = new List<string> {"Pointers", "sorting", "MainMenu", "FillGaps", "BadgeGallery"};
            progressBar.maxValue = list.Count;
            foreach (var s in list)
            {
                StartCoroutine(LoadSceneAsync(s));
            }
        }

        private void Wait()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        void Update() {
            if (progressBar.value == progressBar.maxValue) {
                Invoke(nameof(Wait), 2);
            }
        }
    }
}