using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour {

    [SerializeField]
    private float _damage = 50;

    public float shotLightDelay = 0.05f;

    [SerializeField]
    private GameObject _sparkPrefab;

    private Transform _barrel;
    private Light _shotLight;
    private AudioSource _audio;


    protected virtual void Start()
    {
        _barrel = transform.Find("Barrel");
        _shotLight = _barrel.GetComponent<Light>();
        _audio = GetComponent<AudioSource>();
    }

	
    public void Shoot()
    {
        if(_shotLight != null)
        {
            StartCoroutine(ProcessShotLight());
        }

        if(_audio != null)
        {
            _audio.Play();
        }

        RaycastHit RCinfo;

        if(Physics.Raycast(_barrel.position, _barrel.forward, out RCinfo, 1000))
        {
            Instantiate(_sparkPrefab, RCinfo.point, Quaternion.LookRotation(RCinfo.normal));

            IDamagable damagable = RCinfo.collider.gameObject.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.ApplyDamage(_damage);
            }
        }
    }


    private IEnumerator ProcessShotLight()
    {
        _shotLight.enabled = true;
        yield return new WaitForSeconds(shotLightDelay);
        _shotLight.enabled = false;
    }


}
