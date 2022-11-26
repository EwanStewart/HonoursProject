using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;
public class MyScript : MonoBehaviour
{
    public FixedJoystick MoveJoystick;
    public FixedButton JumpButton;
    public FixedTouchField TouchField;

    [SerializeField] private Objectives ObjectivesRef;

    private bool done;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {


        var fps = GetComponent<RigidbodyFirstPersonController>();
        fps.RunAxis = MoveJoystick.Direction;
        fps.JumpAxis = JumpButton.Pressed;
        fps.mouseLook.LookAxis = TouchField.TouchDist;

        UnityEngine.Vector3 pos = transform.position;
        //Debug.Log(pos.x + " " + pos.y + " " + pos.z);
        //check if in scence
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


