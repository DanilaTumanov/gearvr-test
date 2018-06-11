using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class AIMoveToController : BaseObjectScene
{
    [Tooltip("Расстояние до точки назначения при котором цель считается достигнутой")]
    [Range(0, 5)]
    [SerializeField]
    private float _targetReachDistance;

    [Tooltip("Использовать собственный обработчик поворота")]
    [SerializeField]
    private bool _useCustomRotation = false;

    [Tooltip("Использовать собственный обработчик передвижения")]
    [SerializeField]
    private bool _useCustomMovement = false;

    private NavMeshAgent _agent;
    private Vector3 _target;
    private BaseCharacterController _character;

    public NavMeshAgent Agent
    {
        get
        {
            return _agent;
        }
    }

    // Use this for initialization
    void Start()
    {            
        _target = transform.position;
        _character = GetComponent<BaseCharacterController>();
        _agent = GetComponent<NavMeshAgent>();

        _agent.updateRotation = !_useCustomRotation;
        _agent.updatePosition = !_useCustomMovement;
    }

        
    void Update()
    {
        if(_useCustomRotation)
            _character.Move(_agent.velocity);
    }


    public void SetTarget(Vector3 target)
    {
        if (!_agent.enabled)
            return;

        _target = target;
        _agent.SetDestination(_target);
    }


    public void StartMovement()
    {
        if (!_agent.enabled)
            return;

        _agent.isStopped = false;
    }


    public void StopMovement()
    {
        if (!_agent.enabled)
            return;

        _agent.isStopped = true;
    }

    public void StopImmediately()
    {
        _agent.velocity = Vector3.zero;
        StopMovement();
    }

    public void NavDisable()
    {
        _agent.enabled = false;
    }

    public void NavEnable()
    {
        _agent.enabled = true;
    }

    public bool TargetReached()
    {
        //print(_agent.pathPending + " --remainingDistance- " + _agent.remainingDistance + " --stoppingDistance- " + _agent.stoppingDistance + " --hasPath- " + _agent.hasPath + " --sqrMagnitude- " + _agent.velocity.sqrMagnitude);

        return !_agent.pathPending
        && (_agent.remainingDistance <= _targetReachDistance);
    }
}
