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
                return null;
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

    private GameObject GetUnActiveWeponSlot()
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
}
