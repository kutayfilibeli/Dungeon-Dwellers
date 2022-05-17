using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float bulletSpeed = 7.5f;

    public Rigidbody2D rb;

    public GameObject impactEffect;

    public int PistolDamage = 50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
     if (other.tag != "Enemy")
        {
            Instantiate(impactEffect, transform.position, transform.rotation); 
        }
        
        Destroy(gameObject);

        AudioManager.instance.playSFX(4);

        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(PistolDamage);
        }
        
        
    }

    private void OnBecameInvisible()
    {   
        Destroy(gameObject);
    }
}
