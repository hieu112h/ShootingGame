using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static Weapon;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weapons;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;
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

    void Start()
    {
        activeWeaponSlot = weapons[0];
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject weapon in weapons)
        {
            if(weapon == activeWeaponSlot)
            {
                weapon.SetActive(true);
            }
            else
            {
                weapon.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }
    }

    public void PickUpWeapon(GameObject pickedUpWeapon)
    {
        AddWeaponIntoActiveSlot(pickedUpWeapon);
    }

    public void AddWeaponIntoActiveSlot(GameObject pickedUpWeapon)
    {
        DropCurrentWeapon(pickedUpWeapon);
        pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickedUpWeapon.GetComponent<Weapon>();

        pickedUpWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    public void DropCurrentWeapon(GameObject pickedUpWeapon)
    {
        if(activeWeaponSlot.transform.childCount > 0)
        {
            var weapon = activeWeaponSlot.transform.GetChild(0).gameObject;
            weapon.GetComponent<Weapon>().isActiveWeapon = false;
            weapon.GetComponent<Weapon>().animator.enabled = false;
            weapon.transform.SetParent(pickedUpWeapon.transform.parent, false);
            weapon.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weapon.transform.localRotation = pickedUpWeapon.transform.localRotation;
        }
    }

    public void SwitchWeapon(int slotNumber)
    {
        if(activeWeaponSlot.transform.childCount > 0)
        {
            var weapon = activeWeaponSlot.transform.GetChild(0).gameObject;
            weapon.GetComponent<Weapon>().isActiveWeapon = false;
        }
        activeWeaponSlot = weapons[slotNumber];
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weapon = activeWeaponSlot.transform.GetChild(0).gameObject;
            weapon.GetComponent<Weapon>().isActiveWeapon = true;
        }
    }

    internal void PickUpAmmoBox(AmmoBox ammoBox)
    {
        switch (ammoBox.ammoType)
        {
            case AmmoBox.AmmoType.RifleAmmo:
                totalRifleAmmo += ammoBox.ammoAmount;
                break;
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammoBox.ammoAmount;
                break;
        }
    }

    internal void DecreaseTotalAmmo(int bulletsLeft, Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.M1911:
                totalPistolAmmo -= bulletsLeft;
                break;
            case Weapon.WeaponModel.M4:
                totalRifleAmmo -= bulletsLeft;
                break;
            default: break;
        }
    }

    public int CheckAmmoLeft(WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case WeaponModel.M1911:
                return WeaponManager.Instance.totalPistolAmmo;
            case WeaponModel.M4:
                return WeaponManager.Instance.totalRifleAmmo;
            default: return 0;
        }
    }
}
