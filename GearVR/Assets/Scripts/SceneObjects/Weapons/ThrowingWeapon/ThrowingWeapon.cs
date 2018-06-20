using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThrowingWeapon : MonoBehaviour, IThrowable {

    private Rigidbody _rb;

    public Rigidbody RB
    {
        get
        {
            return _rb;
        }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public abstract void Throw();

}
