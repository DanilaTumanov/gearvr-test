using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grenade : ThrowingWeapon
{
    public float damage = 300;
    public float damageRadius = 5f;
    
    public float explodeTime = 3f;
    
    public GameObject explosionEffect;

    public LayerMask castLayers;

    
    public override void Throw()
    {
        StartCoroutine(ExplodeTimer());
    }

    private IEnumerator ExplodeTimer()
    {
        yield return new WaitForSeconds(explodeTime);

        Explode();
    }

    private void Explode()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius, castLayers);

        foreach(var hit in hitColliders)
        {
            var damagable = hit.GetComponent<IDamagable>();

            if(damagable != null)
            {
                float distMod = (damageRadius - Vector3.Distance(transform.position, hit.transform.position)) / damageRadius;

                damagable.ApplyDamage(damage * distMod);
            }
        }


        var explosion = Instantiate(explosionEffect, transform.position, transform.rotation, null);
        explosion.GetComponent<AudioSource>().Play();

        Destroy(gameObject);
    }
}
