using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{

    [Header("Infor Throwable")]
    public float delay = 3.0f;
    public float damageRadius = 20f;
    public float explosionForce = 1200f;

    float countDown;

    public bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum ThrowableType
    {
        Grenade,
        None,
        Smoke
    }

    public ThrowableType type;

    private void Start()
    {
        countDown = delay;
    }
    private void Update()
    {
        if (hasBeenThrown)
        {
            countDown -= Time.deltaTime;
            if(countDown < 0 && !hasExploded)
            {
                hasExploded = true;
                Explode();
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect();
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (type)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                SoundManager.Instance.PlayThrowableSound(ThrowableType.Grenade);
                break;
            case ThrowableType.Smoke:
                GrenadeEffectSmoke();
                SoundManager.Instance.PlayThrowableSound(ThrowableType.Smoke);
                break;
        }
    }

    private void GrenadeEffectSmoke()
    {
        GameObject explosionEffect = GlobalReference.Instance.smokeEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);
    }

    private void GrenadeEffect()
    {
        GameObject explosionEffect = GlobalReference.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position,transform.rotation);

       //SoundManager.Instance.throwableSoundChanel.PlayOneShot(SoundManager.Instance.grenadeSound);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            if (collider.gameObject.GetComponent<Zombie>())
            {
                collider.gameObject.GetComponent<Zombie>().TakeDamege(100);
            }
        }
        
    }
}
