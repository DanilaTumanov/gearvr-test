using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// TODO: Возможно тут все нужно переделать на MB, чтобы было удобно устанавливать значения
[Serializable]
public class AIVisionController
{

    [SerializeField]
    private Transform _sightPoint;
    [SerializeField]
    private float _visionAngle = 90;
    [SerializeField]
    private float _visionRange = 10;



    public bool HasInVisionRange(Transform target, LayerMask obstacleLayerMask)
    {
        Vector3 toTarget = target.position - _sightPoint.position;

        bool isInRange = (Vector3.Angle(_sightPoint.forward, toTarget) <= _visionAngle / 2) && (toTarget.magnitude <= _visionRange);

        bool hasObstacle = isInRange ? Physics.Linecast(_sightPoint.position, target.position, obstacleLayerMask) :  true;
            
        //Debug.Log(isInRange + " -- " + hasObstacle);

        //Debug.DrawRay(_sightPoint.position, toTarget, Color.red, 0.5f);
        //Debug.DrawRay(_sightPoint.position, _sightPoint.forward * 10, Color.green, 0.5f);

        return isInRange && !hasObstacle;
    }

}