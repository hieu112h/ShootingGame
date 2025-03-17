using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public int HP = 100;

    public void TakeDamege(int damage)
    {
        HP -= damage;

        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
