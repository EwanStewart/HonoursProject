using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;
public class rigidBodyHandler : MonoBehaviour
{
    public FixedJoystick MoveJoystick;
    public FixedButton JumpButton;
    public FixedTouchField TouchField;
    public GameObject mainCanvas;
    public GameObject pauseCanvas;

    [SerializeField] private Objectives ObjectivesRef;

    private bool done;
    
    public void togglePause()
    {
        if (mainCanvas.activeSelf)
        {
            mainCanvas.SetActive(false);
            pauseCanvas.SetActive(true);
        }
        else
        {
            mainCanvas.SetActive(true);
            pauseCanvas.SetActive(false);
        }
    }
    
    public void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    
    void Update()
    {
        var fps = GetComponent<RigidbodyFirstPersonController>();
        fps.RunAxis = MoveJoystick.Direction;
        fps.JumpAxis = JumpButton.Pressed;
        fps.mouseLook.LookAxis = TouchField.TouchDist;

        UnityEngine.Vector3 pos = transform.position;
        string scene;
        
        scene = SceneManager.GetActiveScene().name;
        if (scene == "Pointers" & (pos.z < 5 & pos.z > 3) )
        {
            if (!done)
            {
                ObjectivesRef.nextLine();
                done = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}


