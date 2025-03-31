using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public int HP = 100;
    public Animator animator;
    public NavMeshAgent agent;

    public ZombieAttack zombieAttack;
    public int zombieDamage;

    public bool isDead;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        zombieDamage = zombieAttack.handAttackDamage;
    }
    public void TakeDamege(int damage)
    {
        HP -= damage;

        if(HP <= 0)
        {
            animator.SetTrigger("isDie");
            isDead = true;
            SoundManager.Instance.zombieChanel2.PlayOneShot(SoundManager.Instance.zombieDeath);
        }
        else
        {
            animator.SetTrigger("isDamage");
            SoundManager.Instance.zombieChanel2.PlayOneShot(SoundManager.Instance.zombieHurt);
        }
    }
}
