using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    private Vector3 _rightRotation = new Vector3(0, 60, 0);
    private Vector3 _leftRotation = new Vector3(0, -60, 0);


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // is player using a controller?
        if (OVRInput.GetActiveController() == OVRInput.Controller.LTrackedRemote ||
            OVRInput.GetActiveController() == OVRInput.Controller.RTrackedRemote)
        {
            // yes, are they touching the touchpad?
            if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
            {
                // yes, let's require an actual click rather than just a touch.
                if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
                {
                    // button is depressed, handle the touch.
                    Vector2 touchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
                    ProcessControllerClickAtPosition(touchPosition);
                }
            }
        }
        else if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) // finger on HMD pad?
        {
            // not using controller, same behavior as before.
            Vector2 touchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            //ProcessHMDClickAtPosition(touchPosition);
        }

    }

    private void ProcessControllerClickAtPosition(Vector2 touchPosition)
    {
        if(touchPosition.y > 0.7)
        {
            var pos = transform.position;
            pos.z += 3 * Time.deltaTime;
            transform.position = pos;
        }
        else if (touchPosition.y < -0.7)
        {
            var pos = transform.position;
            pos.z -= 3 * Time.deltaTime;
            transform.position = pos;
        }

        if(touchPosition.x > 0.7)
        {
            transform.Rotate(_rightRotation * Time.deltaTime);
        }
        else if (touchPosition.x < -0.7)
        {
            transform.Rotate(_leftRotation * Time.deltaTime);
        }
    }
}
