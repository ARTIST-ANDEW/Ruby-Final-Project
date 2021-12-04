using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class HealthCollectible : MonoBehaviour
{
    public AudioClip collectedClip;
    public GameObject HealthParticlePrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.health < controller.maxHealth)
            {
                controller.ChangeHealth(1);
                GameObject HealthParticleObject = Instantiate(HealthParticlePrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
                
            
                controller.PlaySound(collectedClip);
            }
        }

    }
}