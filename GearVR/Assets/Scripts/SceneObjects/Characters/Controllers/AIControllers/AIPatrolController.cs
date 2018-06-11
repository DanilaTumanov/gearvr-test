using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIPatrolController : BaseObjectScene
{

    public Transform patrolRoute;

    private List<Transform> _patrolPoints = new List<Transform>();
    private int _currentPointIndex = -1;

    public int CurrentPointIndex
    {
        get
        {
            return _currentPointIndex;
        }
    }

        

    private void Start()
    {
        if (patrolRoute != null)
            foreach (Transform point in patrolRoute)
            {
                _patrolPoints.Add(point);
            }
    }


    public Vector3 GetPoint(int index)
    {
        return _patrolPoints[index].position;
    }

    public Vector3 NextPoint()
    {
        return GetPoint(SetNextIndex());
    }

    public Vector3 CurrentPoint()
    {
        return _currentPointIndex < 0 ? NextPoint() : GetPoint(_currentPointIndex);
    }


    private int SetNextIndex()
    {
        _currentPointIndex = ++_currentPointIndex % _patrolPoints.Count;
        return _currentPointIndex;
    }
}