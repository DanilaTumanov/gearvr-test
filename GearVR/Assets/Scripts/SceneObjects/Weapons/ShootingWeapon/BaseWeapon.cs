using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour {
    
    public float shotLightDelay = 0.05f;

    protected Transform _barrel;
    protected Light _shotLight;
    protected AudioSource _audio;


    protected virtual void Start()
    {
        _barrel = transform.Find("Barrel");
        _shotLight = _barrel.GetComponent<Light>();
        _audio = GetComponent<AudioSource>();
    }

	
    public virtual void Shoot()
    {
        if(_shotLight != null)
        {
            StartCoroutine(ProcessShotLight());
        }

        if(_audio != null)
        {
            _audio.Play();
        }
    }


    private IEnumerator ProcessShotLight()
    {
        _shotLight.enabled = true;
        yield return new WaitForSeconds(shotLightDelay);
        _shotLight.enabled = false;
    }


}
