using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanWeapon : BaseWeapon {

    [SerializeField]
    protected float _damage = 50;

    [SerializeField]
    protected GameObject _sparkPrefab;


    public override void Shoot()
    {
        base.Shoot();

        RaycastHit RCinfo;

        if (Physics.Raycast(_barrel.position, _barrel.forward, out RCinfo, 1000))
        {
            Instantiate(_sparkPrefab, RCinfo.point, Quaternion.LookRotation(RCinfo.normal));

            IDamagable damagable = RCinfo.collider.gameObject.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.ApplyDamage(_damage);
            }
        }
    }

}
