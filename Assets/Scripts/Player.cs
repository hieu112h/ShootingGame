using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreen;
    public GameObject camera;
    public bool isDead;
    public void TakeDamage(int damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            print("die");
            GetComponent<MouseScripts>().enabled = false;
            GetComponent<PlayerMoveScripts>().enabled = false;
            camera.GetComponent<Animator>().enabled = true;
            GetComponent<ScreenBack>().StartFade();
            StartCoroutine(ShowGameOverUI());
            isDead = true;
            SoundManager.Instance.playerChanel.PlayOneShot(SoundManager.Instance.playerDeath);
        }
        else
        {
            StartCoroutine(BloodyScreenEffect());
            SoundManager.Instance.playerChanel.PlayOneShot(SoundManager.Instance.playerPain);
        }
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(2.0f);
        HUDManager.Instance.gameOver.SetActive(true);
    }

    private IEnumerator BloodyScreenEffect()
    {
        if(bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            yield return null; ; // Wait for the next frame.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ZombieHand"))
        {
            if(isDead == false)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieAttack>().handAttackDamage);
                HUDManager.Instance.health.text = $"HP: {HP.ToString()}";
            }
            
        }
    }


}
