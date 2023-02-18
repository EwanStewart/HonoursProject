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
        //toggle the canvases on and off
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
    
    public void loadScene(string scene) //load a scene
    {
        SceneManager.LoadScene(scene);
    }
    
    void Update()
    {
        var fps = GetComponent<RigidbodyFirstPersonController>(); //get the rigidbody controller
        fps.RunAxis = MoveJoystick.Direction;                     //set the joystick direction to the run axis
        fps.JumpAxis = JumpButton.Pressed;                        //set the jump button to the jump axis
        fps.mouseLook.LookAxis = TouchField.TouchDist;            //set the touch field to the look axis

        UnityEngine.Vector3 pos = transform.position;             //get the position of the player
        
        if (SceneManager.GetActiveScene().name == "Pointers" & (pos.z < 5 & pos.z > 3) ) //if the player is in the pointers scene and is in the right position
        {
            if (!done)                                             //if the player hasn't already completed the objective
            {
                ObjectivesRef.nextLine();                          //go to the next line in the objectives
                done = true;                                       //set done to true so that the player can't complete the objective again
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //load the next scene
            }
        }
    }
}


