using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControls : MonoBehaviour
{
    [Tooltip("Reference to the earth_x GameObject to rotate the earth's X axis")]
    [SerializeField] GameObject earth_x;

    [Tooltip("Reference to the earth_y GameObject to rotate the earth's Y axis")]
    [SerializeField] GameObject earth_y;

    [Tooltip("Max rotation speed for the earth depending on how far to the edge of the screen is the user touching")]
    [SerializeField] float maxRotationSpeed;

    /// <summary>
    /// Reference to the Cannon script in the scene
    /// </summary>
    Cannon cannon;

    /// <summary>
    /// Reference to the MainCamera in the scene
    /// </summary>
    Camera mainCamera;

    /// <summary>
    /// Track if a finger is on the screen or not
    /// </summary>
    bool fingerOnScreen;

    /// <summary>
    /// Track if the game is running or not in order to enable/disable input
    /// </summary>
    bool gameRunning;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        cannon = FindObjectOfType<Cannon>();
    }

    /// <summary>
    /// Check for user input
    /// </summary>
    void FixedUpdate()
    {
        if(gameRunning)
        {
            //If there is a finger on the screeen, rotate the earth, otherwise fire cannon if there was previously a finger on the screen
            if (Input.touchCount > 0)
            {
                fingerOnScreen = true;
                RotateEarth(Input.GetTouch(0));
            }
            else if (fingerOnScreen)
            {
                cannon.FireCannon();
                fingerOnScreen = false;
            }
        }
    }

    /// <summary>
    /// Rotate the earth based on finger location
    /// </summary>
    /// <param name="touch"></param>
    void RotateEarth(Touch touch)
    {
        //Get the finger position and x,y offset based on where the finger is located
        //(0.5, 0.5) is the value of the center of the screen in ViewPort, substracting the offSet by that in order to get (0, 0) as the "Center value" if the finger was on the center of the screen
        Vector2 fingerPos = mainCamera.ScreenToViewportPoint(touch.position);
        float xOffSet = fingerPos.x - 0.5f;
        float yOffSet = fingerPos.y - 0.5f;

        //Calculating the rotationSpeed for the x axis and y axis of the earth based on how far the finger is from the center of the screen
        //The closer it is to the screen the slower and the further it is the faster it gets with maxSpeedRotation as the cap
        float rotationSpeedX = xOffSet * maxRotationSpeed * Time.deltaTime;
        float rotationSpeedY = yOffSet * maxRotationSpeed * Time.deltaTime;

        //Rotate the earth X and earth Y gameObjects. Rotating separate objects in order to avoid gimbal lock
        earth_y.transform.Rotate(0, rotationSpeedY, 0, Space.World);
        earth_x.transform.Rotate(rotationSpeedX, 0, 0, Space.World);
    }

    /// <summary>
    /// Enables/Disabled user input and resets fingerOnScreen
    /// </summary>
    /// <param name="state"></param>
    public void SetControlsActive(bool state)
    {
        gameRunning = state;
        fingerOnScreen = false;
    }
}
