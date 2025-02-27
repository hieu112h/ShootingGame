using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit");
            CreatBulletImpactEffect(collision);
            Destroy(gameObject);
        }
    }

    public void CreatBulletImpactEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject hole = Instantiate(
            GlobalReference.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );

        hole.transform.SetParent(collision.gameObject.transform);
    }
}
