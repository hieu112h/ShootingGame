using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;

    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //Burst
    public int bulletsPerBurst = 3;
    public int burstBulletLeft;

    //Spread
    public float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public Animator animator;


    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReload;

    public bool isADS;


    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public enum WeaponModel
    {
        M1911,
        M4
    }

    public WeaponModel thisWeaponModel;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    public GameObject muzzleEffect;

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        spreadIntensity = hipSpreadIntensity;
    }

    void Update()
    {
        if (isActiveWeapon)
        {
            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }
            
            if(Input.GetMouseButtonUp(1))
            {
                ExitsADS();
            }

            GetComponent<Outline>().enabled = false;
            if (currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletLeft = bulletsPerBurst;
                FireWeapon();
            }
            if (bulletsLeft <= 0 && isShooting)
            {
                SoundManager.Instance.emptySound.Play();
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReload == false && WeaponManager.Instance.CheckAmmoLeft(thisWeaponModel) > 0)
            {
                ReLoad();
            }

            //if(readyToShoot == true && isShooting == false && isReload == false && bulletsLeft <= 0)
            //{
            //    ReLoad();
            //}

            //AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft / bulletsPerBurst}/{magazineSize / bulletsPerBurst}"; 
        }
        
    }

    private void ExitsADS()
    {
        animator.SetTrigger("exitsADS");
        isADS = false;
        HUDManager.Instance.middleDot.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
    }

    private void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        HUDManager.Instance.middleDot.SetActive(false);
        spreadIntensity = adsSpreadIntensity;
    }

    private void FireWeapon()
    {
        bulletsLeft--;
        if (isADS)
        {
            animator.SetTrigger("isADSshoot");
        }
        else
        {
            animator.SetTrigger("Shoot");
        }

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke("ResetShoot", shootingDelay);
            allowReset = false;
        }

        if(currentShootingMode == ShootingMode.Burst && burstBulletLeft > 1)
        {
            burstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

    }
    private void ReLoad()
    {
        isReload = true;
        Invoke("ReLoadCompelete", reloadTime);
        SoundManager.Instance.PlayReLoadSound(thisWeaponModel);
        animator.SetTrigger("Reload");
    }

    private void ReLoadCompelete()
    {
        int bulletNeed = magazineSize - bulletsLeft;
        
        if (WeaponManager.Instance.CheckAmmoLeft(thisWeaponModel) > bulletNeed)
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletNeed, thisWeaponModel);
            
        }
        else
        {
            bulletsLeft += WeaponManager.Instance.CheckAmmoLeft(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(WeaponManager.Instance.CheckAmmoLeft(thisWeaponModel), thisWeaponModel);
        }
        isReload = false;
    }

    private void ResetShoot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        //Shooting from the middle of the screen to check where are we point
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;

        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(0, y, z); 
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletPrefabLifeTime)
    {
        yield return new WaitForSeconds(bulletPrefabLifeTime);
        Destroy(bullet);
    }
}
