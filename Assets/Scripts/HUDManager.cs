using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Grenade")]
    public TextMeshProUGUI grenadeUI;
    public Image grenadeImageUI;

    [Header("Smoke")]
    public TextMeshProUGUI smokeUI;
    public Image smokeImageUI;

    [Header("Health")]
    public TextMeshProUGUI health;


    public Sprite transparent;
    public GameObject middleDot;
    public GameObject gameOver;

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
    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        //Weapon unActiveWeapon = GetUnActiveWeponSlot().GetComponent<Weapon>();

        if (activeWeapon != null)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeft(activeWeapon.thisWeaponModel)}";


        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";
        }

        if(ThrowableManager.Instance.grenadeCount == 0)
        {
            grenadeImageUI.sprite = transparent;
            grenadeUI.text = "";
        }

        if (ThrowableManager.Instance.smokeCount == 0)
        {
            smokeImageUI.sprite = transparent;
            smokeUI.text = "";
        }
    }


    public Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.M1911:
                ammoTypeUI.sprite = Resources.Load<GameObject>("M1911_Ammo").GetComponent<SpriteRenderer>().sprite;
                return ammoTypeUI.sprite;
            case Weapon.WeaponModel.M4:
                ammoTypeUI.sprite = Resources.Load<GameObject>("M4_Ammo").GetComponent<SpriteRenderer>().sprite;
                return activeWeaponUI.sprite;
            default:
                return ammoTypeUI.sprite = transparent;
        }
        
    }

    public Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.M1911:
                activeWeaponUI.sprite = Resources.Load<GameObject>("M1911").GetComponent<SpriteRenderer>().sprite;
                return activeWeaponUI.sprite;
            case Weapon.WeaponModel.M4:
                activeWeaponUI.sprite = Resources.Load<GameObject>("M4").GetComponent<SpriteRenderer>().sprite;
                return activeWeaponUI.sprite;
            default:
                return null;
        }
    }

    public GameObject GetUnActiveWeponSlot()
    {
        foreach(GameObject weaponSlot in WeaponManager.Instance.weapons)
        {
            if(weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }

    public Sprite GetUnActiveWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.M1911:
                unActiveWeaponUI.sprite = Resources.Load<GameObject>("M1911").GetComponent<SpriteRenderer>().sprite;
                return activeWeaponUI.sprite;
            case Weapon.WeaponModel.M4:
                unActiveWeaponUI.sprite = Resources.Load<GameObject>("M4").GetComponent<SpriteRenderer>().sprite;
                return activeWeaponUI.sprite;
            default:
                return null;
        }
    }

    public void UpdateThrowable(Throwable.ThrowableType grenade)
    {
        switch (grenade)
        {
            case Throwable.ThrowableType.Grenade:
                grenadeUI.text=$"{ThrowableManager.Instance.grenadeCount}";
                grenadeImageUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite ;
                break;
            case Throwable.ThrowableType.Smoke:
                smokeUI.text = $"{ThrowableManager.Instance.smokeCount}";
                smokeImageUI.sprite = Resources.Load<GameObject>("Smoke").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}
