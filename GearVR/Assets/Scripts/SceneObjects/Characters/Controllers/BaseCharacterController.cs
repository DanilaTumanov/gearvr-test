using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseCharacterController : BaseObjectScene, IDamagable
{
    [SerializeField]
    private float _hp = 200;

    [SerializeField]
    float _turnSpeed = 10;

    [SerializeField]
    float _GroundCheckDistance = 0.2f;

    [SerializeField]
    bool _turnToNormal = false;

    private Vector3 _GroundNormal;
    bool _IsGrounded;
    float _TurnAmount;
    float _ForwardAmount;



    private void Start()
    {
        //Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }


    public void Move(Vector3 move)
    {
        CheckGroundStatus();
        ApplyExtraTurnRotation(move);
    }



    void ApplyExtraTurnRotation(Vector3 move)
    {
        Vector3 forwardVector = Vector3.ProjectOnPlane(move.magnitude > 1 ? move : transform.forward, _turnToNormal ? _GroundNormal : Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forwardVector, _GroundNormal), _turnSpeed * Time.deltaTime);

#if UNITY_EDITOR
        //Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + forwardVector);
#endif
            
        Quaternion nextRotation = Quaternion.LookRotation(forwardVector, _GroundNormal);

        // Отдельно формируем поворот по y, т.к. по остальным осям поворот должен происходить мгновенно, а по y дискретизировано
        nextRotation.y = Mathf.Lerp(transform.rotation.y, nextRotation.y, _turnSpeed * Time.deltaTime);

        //print(nextRotation + " --- " + transform.rotation);

        transform.rotation = nextRotation;
    }


    void CheckGroundStatus()
    {
        RaycastHit hitInfo;

        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, _GroundCheckDistance))
        {
            _GroundNormal = hitInfo.normal;
            _IsGrounded = true;
        }
        else
        {
            _IsGrounded = false;
            _GroundNormal = Vector3.up;
        }
    }


    public void ApplyDamage(float damage)
    {
        ReduceHP(damage);
    }

    private void ReduceHP(float count)
    {
        _hp -= count;
        if (_hp < 0)
        {
            _hp = 0;
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}


