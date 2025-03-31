using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    [Header("Sound Weapon")]
    public AudioSource shootingSoundChanel;
    public AudioClip shootingSoundM4;
    public AudioClip shootingSoundM1911;

    [Header("Sound Throwable")]
    public AudioSource throwableSoundChanel;
    public AudioClip grenadeSound;

    [Header("Sound Reload")]
    public AudioSource reloadSoundM4;
    public AudioSource reloadSoundM1911;

    [Header("Sound Empty Magazine")]
    public AudioSource emptySound;


    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieDeath;
    public AudioClip zombieHurt;

    public AudioSource zombieChanel;
    public AudioSource zombieChanel2;

    public AudioClip playerPain;
    public AudioClip playerDeath;
    public AudioSource playerChanel;

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

    public void PlayThrowableSound(Throwable.ThrowableType type)
    {
        switch (type)
        {
            case Throwable.ThrowableType.Grenade:
                throwableSoundChanel.PlayOneShot(grenadeSound);
                break;
        }
    }
}
