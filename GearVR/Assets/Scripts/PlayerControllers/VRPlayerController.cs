using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerController : MonoBehaviour {

    [SerializeField]
    private float _movementSpeed = 12;
    [SerializeField]
    private float _strafeSpeed = 9;

    [SerializeField]
    private Vector3 _rightRotation = new Vector3(0, 90, 0);
    [SerializeField]
    private Vector3 _leftRotation = new Vector3(0, -90, 0);

    [SerializeField]
    private ThrowingWeapon _throwingWeaponPrefab;


    private BaseWeapon _weapon;
    private ThrowController _throwController;

    private HandleMode _handleMode = HandleMode.PrimaryWeapon;

    // Use this for initialization
    void Start () {
        //TODO: тут разумеется надо не нулевой брать, а через менеджер все проводить, но это ведь просто тренировка :)
        _weapon = GameObject.FindGameObjectWithTag("Weapons").transform.GetChild(0).GetComponent<BaseWeapon>();
        _throwController = GetComponent<ThrowController>();
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

                if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
                {
                    Vector2 touchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
                    ProcessControllerClickDownAtPosition(touchPosition);
                }

                if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
                {
                    ProcessControllerClickUp();
                }
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) &&
                _handleMode == HandleMode.PrimaryWeapon)
            {
                _weapon.Shoot();
            }
        }
        else if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) // finger on HMD pad?
        {
            // not using controller, same behavior as before.
            Vector2 touchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            //ProcessHMDClickAtPosition(touchPosition);
        }


#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) &&
            _handleMode == HandleMode.PrimaryWeapon)
        {
            _weapon.Shoot();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 touchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            ProcessControllerClickDownAtPosition(touchPosition);
        }

        if (Input.GetMouseButtonUp(1))
        {
            ProcessControllerClickUp();
        }
#endif

    }

    private void ProcessControllerClickAtPosition(Vector2 touchPosition)
    {
        var pos = transform.position;

        if (touchPosition.y > 0.3)
        {
            pos += transform.forward * _movementSpeed * Time.deltaTime;
        }
        else if (touchPosition.y < -0.3)
        {
            pos -= transform.forward * _movementSpeed * Time.deltaTime;
        }

        //if(touchPosition.x > 0.3)
        //{
        //    //transform.Rotate(_rightRotation * Time.deltaTime);
        //    pos += transform.right * _strafeSpeed * Time.deltaTime;
        //}
        

        transform.position = pos;
    }


    private void ProcessControllerClickDownAtPosition(Vector3 touchPosition)
    {
        if (touchPosition.x < -0.3 ||
            Input.GetMouseButton(1))
        {
            SetHandleMode(HandleMode.SecondaryWeapon);
        }
    }


    private void ProcessControllerClickUp()
    {
        SetHandleMode(HandleMode.PrimaryWeapon);
    }



    private void SetHandleMode(HandleMode mode)
    {
        _handleMode = mode;

        switch (mode)
        {
            case HandleMode.PrimaryWeapon:
                SwitchToPrimaryWeapon();
                break;

            case HandleMode.SecondaryWeapon:
                SwitchToSecondaryWeapon();
                break;
        }
    }


    private void SwitchToPrimaryWeapon()
    {
        _weapon.gameObject.SetActive(true);
        _throwController.Disable();
    }


    private void SwitchToSecondaryWeapon()
    {
        _weapon.gameObject.SetActive(false);
        _throwController.Enable(_throwingWeaponPrefab);
    }

}



public enum HandleMode
{
    PrimaryWeapon,
    SecondaryWeapon
}