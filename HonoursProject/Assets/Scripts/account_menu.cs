using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class account_menu : MonoBehaviour
{
    public void loadScene(string scene) //load scene on parameter value
    {
        SceneManager.LoadScene(scene);
    }
}
