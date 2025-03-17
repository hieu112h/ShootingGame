using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ThrowableManager : MonoBehaviour
{
    public static ThrowableManager Instance { get; set; }

    public float throwForce = 40f;
    public float forceMultiplier = 0;
    public float forceMultiplierLimit = 2f;
    public GameObject throwableSpawn;

    [Header("Infor Grenade")]
    public int grenadeCount = 0;
    public Throwable.ThrowableType grenadeType;
    public GameObject grenadePrefab;

    [Header("Infor Smoke")]
    public int smokeCount = 0;
    public Throwable.ThrowableType smokeType;
    public GameObject smokePrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        grenadeType = Throwable.ThrowableType.None;
        smokeType = Throwable.ThrowableType.None;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            forceMultiplier += Time.deltaTime;
            if(forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            ThrowLethal(grenadeType);
            forceMultiplier = 0;
        }

        if (Input.GetKey(KeyCode.T))
        {
            forceMultiplier += Time.deltaTime;
            if (forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            ThrowLethal(smokeType);
            forceMultiplier = 0;
        }


    }

    private void ThrowLethal(Throwable.ThrowableType type)
    {
        GameObject lethalPrefab = GetThrowablePrefab(type);
        GameObject throwable =  Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;
        CaculateThrowable(type);

        HUDManager.Instance.UpdateThrowable(throwable.GetComponent<Throwable>().type);

    }

    private void CaculateThrowable(Throwable.ThrowableType type)
    {
        switch (type)
        {
            case Throwable.ThrowableType.Grenade:
                grenadeType--;
                break;
            case Throwable.ThrowableType.Smoke:
                smokeCount--;
                break;
        }
    }

    private GameObject GetThrowablePrefab(Throwable.ThrowableType type)
    {
        switch (type)
        {
            case Throwable.ThrowableType.Grenade:
                return grenadePrefab;
            case Throwable.ThrowableType.Smoke:
                return smokePrefab;
            default:
                return null;
        }
    }

    public void PickUpThrowable(Throwable throwableSelect)
    {
        switch (throwableSelect.type)
        {
            case Throwable.ThrowableType.Grenade:
                PickUpThrowableAsLethal(Throwable.ThrowableType.Grenade);
                break;
            case Throwable.ThrowableType.Smoke:
                PickUpThrowableAsLethal(Throwable.ThrowableType.Smoke);
                break;
        }
    }

    private void PickUpThrowableAsLethal(Throwable.ThrowableType lethal)
    {
        switch (lethal)
        {
            case Throwable.ThrowableType.Grenade:
                if (grenadeType == lethal || grenadeType == Throwable.ThrowableType.None)
                {
                    grenadeType = lethal;
                    if (grenadeCount < 2)
                    {
                        grenadeCount++;
                    }
                }
                break;
            case Throwable.ThrowableType.Smoke:
                if (smokeType == lethal || smokeType == Throwable.ThrowableType.None)
                {
                    smokeType = lethal;
                    if (smokeCount < 2)
                    {
                        smokeCount++;
                    }
                }
                break;
        }
        //throwableType = lethal;
        HUDManager.Instance.UpdateThrowable(lethal);
    }
}
