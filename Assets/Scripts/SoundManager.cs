using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource shootingSoundChanel;
    public AudioClip shootingSoundM4;
    public AudioClip shootingSoundM1911;

    public AudioSource reloadSoundM4;
    public AudioSource reloadSoundM1911;
    public AudioSource emptySound;

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

    public void PlayShootingSound(Weapon.WeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case Weapon.WeaponModel.M4:
                shootingSoundChanel.PlayOneShot(shootingSoundM4);
                break;
            case Weapon.WeaponModel.M1911:
                shootingSoundChanel.PlayOneShot(shootingSoundM1911);
                break;
        }
    }

    public void PlayReLoadSound(Weapon.WeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case Weapon.WeaponModel.M4:
                reloadSoundM4.Play();
                break;
            case Weapon.WeaponModel.M1911:
                reloadSoundM1911.Play();
                break;
        }
    }
}
