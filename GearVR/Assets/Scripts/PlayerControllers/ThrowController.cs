using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour {

    public float _throwImpulse = 10f;

    public float _curveTimeStep = 0.1f;


    private bool _enable = false;
    private Transform _throwPoint;
    private LineRenderer _curveRenderer;
    private IThrowable _throwableObject;

    private Vector3 _prevPosition;
    private Vector3 _throwVector = Vector3.zero;

    private void Start()
    {
        _throwPoint = GameObject.FindGameObjectWithTag("ThrowPoint").transform;
        //_curveRenderer = _throwPoint.GetComponent<LineRenderer>();
    }


    private void Update()
    {
        if (!_enable)
            return;

        if (_throwableObject != null)
        {
            CalculateThrowVector();
            ProcessThrow();
        }
            
    }


    public void Enable(IThrowable _throwableObject)
    {
        _enable = true;

        if(this._throwableObject != _throwableObject)
        {
            if(this._throwableObject != null)
            {
                Destroy((this._throwableObject as MonoBehaviour).gameObject);
            }

            this._throwableObject = (IThrowable) Instantiate((MonoBehaviour)_throwableObject, _throwPoint.position, _throwPoint.rotation, _throwPoint);
            this._throwableObject.RB.isKinematic = true;
        }
        else if(this._throwableObject != null)
        {
            (this._throwableObject as MonoBehaviour).gameObject.SetActive(true);
        }

        _prevPosition = _throwPoint.position;
    }

    public void Disable()
    {
        _enable = false;
        if(_throwableObject != null)
        {
            (_throwableObject as MonoBehaviour).gameObject.SetActive(false);
        }
    }



    private void ProcessThrow()
    {
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) ||
            Input.GetMouseButtonDown(0))
        {
            (_throwableObject as MonoBehaviour).transform.parent = null;
            _throwableObject.RB.isKinematic = false;

            // Возможно тут понадобится корректировка. При броске объектов разной массы результат должен быть разный, но сила броска персонажа должна влиять на это.. Надо доделать
            var throwImpulse = _throwVector;

            _throwableObject.RB.AddForce(throwImpulse, ForceMode.Impulse);

            _throwableObject.Throw();

            _throwableObject = null;
        }
    }



    private void CalculateThrowVector()
    {
        _throwVector = (_throwableObject.RB.position - _prevPosition) / Time.deltaTime;
        _prevPosition = _throwableObject.RB.position;
    }

}
