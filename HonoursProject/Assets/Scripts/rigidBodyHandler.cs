using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;
public class rigidBodyHandler : MonoBehaviour
{
    public FixedJoystick MoveJoystick;
    public FixedJoystick LookJoystick;

    public FixedTouchField TouchField;
    public GameObject mainCanvas;
    public GameObject pauseCanvas;
    public GameObject chairs;
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
    
    public void OnTriggerEnter(Collider other) //when the player enters a trigger
    {
        if (other.gameObject.tag == "chair") //if the player enters an objective
        {
            if (!done)                                             //if the player hasn't already completed the objective
            {
                done = true;                                       //set done to true so that the player can't complete the objective again
                SceneManager.LoadScene("PointersLessonUI"); //load the next scene
            }
        }
    }
    
    void Update()
    {
        var fps = GetComponent<RigidbodyFirstPersonController>(); //get the rigidbody controller
        fps.RunAxis = MoveJoystick.Direction;                     //set the joystick direction to the run axis
        fps.mouseLook.LookAxis = LookJoystick.Direction;          //set the look joystick direction to the look axis
        UnityEngine.Vector3 pos = transform.position;             //get the position of the player
    }
}


