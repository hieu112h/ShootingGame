using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon weaponSelect;

    public AmmoBox ammoBoxSelect;

    public Throwable throwableSelect;

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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRayCast = hit.transform.gameObject;
            //Weapon
            if (objectHitByRayCast.GetComponent<Weapon>() && objectHitByRayCast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                weaponSelect = objectHitByRayCast.GetComponent<Weapon>();
                weaponSelect.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickUpWeapon(objectHitByRayCast.gameObject);
                    HUDManager.Instance.GetAmmoSprite(weaponSelect.GetComponent<Weapon>().thisWeaponModel);
                    HUDManager.Instance.GetWeaponSprite(weaponSelect.GetComponent<Weapon>().thisWeaponModel);
                }
            
            }
            else
            {

                if (weaponSelect)
                {
                    weaponSelect.GetComponent<Outline>().enabled = false; 
                }
            }

            //AmmoBox
            if (objectHitByRayCast.GetComponent<AmmoBox>())
            {
                ammoBoxSelect = objectHitByRayCast.GetComponent<AmmoBox>();
                ammoBoxSelect.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickUpAmmoBox(ammoBoxSelect);
                    Destroy(ammoBoxSelect.gameObject);
                }

            }
            else
            {

                if (ammoBoxSelect)
                {
                    ammoBoxSelect.GetComponent<Outline>().enabled = false;
                }
            }

            //Throwable
            if (objectHitByRayCast.GetComponent<Throwable>())
            {
                throwableSelect = objectHitByRayCast.GetComponent<Throwable>();
                throwableSelect.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    ThrowableManager.Instance.PickUpThrowable(throwableSelect);
                    Destroy(throwableSelect.gameObject);
                }

            }
            else
            {

                if (throwableSelect)
                {
                    throwableSelect.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
