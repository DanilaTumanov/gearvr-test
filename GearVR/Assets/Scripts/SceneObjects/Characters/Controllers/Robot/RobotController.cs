using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(AIMoveToController))]
[RequireComponent(typeof(AIPatrolController))]
public class RobotController : BaseCharacterController
{

    public float maxTargetDistance = 1000f;

    [SerializeField]
    private AIVisionController _vision;


    private AIPatrolController _patrol;
    private AIMoveToController _move;
    private AIStateMachine _stateMachine;

    private int _visionLayerMask;

    private void Start()
    {
        _patrol = GetComponent<AIPatrolController>();
        _move = GetComponent<AIMoveToController>();
        //_vision = new AIVisionController(transform);
        _stateMachine = new AIStateMachine(this);


        SetVisionLayerMask();
        InitSMStates();
    }

        

    private void InitSMStates()
    {
        //_stateMachine.AddState("patrol", ProcessPatrol);
        _stateMachine.AddState("pursuit", ProcessPursuit);


        _stateMachine.Start("pursuit");
    }



    private IEnumerator ProcessPatrol()
    {
        WaitForSeconds patrolCheckPeriod = new WaitForSeconds(0.5f);

        string nextState = String.Empty;

        _move.StartMovement();

        while (true)
        {

            if (SearchEnemies())
            {
                nextState = "pursuit";

                // TODO: тут разумеется нужно убрать жесткую привязку к игроку и определять конкретного врага через SearchEnemies
                _move.SetTarget(Main.Instance.Player.transform.position);

                break;
            }
            else if (_move.TargetReached())
            {
                _move.SetTarget(_patrol.NextPoint());
                _move.Agent.stoppingDistance = 0;
            }

            yield return patrolCheckPeriod;
        }

        yield return nextState;
    }


    private IEnumerator ProcessPursuit()
    {
        WaitForSeconds pursuitCheckPeriod = new WaitForSeconds(0.2f);

        string nextState = String.Empty;

        _move.StartMovement();

        while (true)
        {
            //if (SearchEnemies())
            //{
                // TODO: И тут тоже надо убрать жесткую привязку
                _move.SetTarget(Main.Instance.Player.transform.position);
            //}
            //else if(_move.TargetReached()) {
            //    nextState = "patrol";
            //    break;
            //}                

            yield return pursuitCheckPeriod;
        }

        yield return nextState;
    }



    private void SetVisionLayerMask()
    {

        // TODO: Нужно отдельно задавать слои. Тут это делать плохо.
        int includeLayers = int.MaxValue;

        int excludeLayers = 1 << LayerMask.NameToLayer("Player") |
                            1 << LayerMask.NameToLayer("Enemies");
            
        _visionLayerMask = includeLayers ^ excludeLayers;
    }


    private bool SearchEnemies()
    {
        return _vision.HasInVisionRange(Main.Instance.Player.transform, _visionLayerMask);
    }

}