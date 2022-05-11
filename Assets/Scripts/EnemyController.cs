using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Rigidbody2D enemyRB;
    public float movementSpeed;

    public float playerChaseRange;
    private Vector3 movementDirection;

    public Animator animator;

    public int health = 150;

    public GameObject[] SplatterEffects;

    public GameObject HitEffect;

    public bool canShoot;

    public GameObject bullet;
    public Transform firingPoint;
    public float rateofFire;
    private float fireCounter;
    public float shootrange;

    public SpriteRenderer body;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(body.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < playerChaseRange)
            {
                movementDirection = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                movementDirection = Vector3.zero;
            }

            movementDirection.Normalize();
            enemyRB.velocity = movementDirection * movementSpeed;



            if (canShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootrange)
            {
                fireCounter -= Time.deltaTime;
                if (fireCounter <= 0)
                {
                    fireCounter = rateofFire;
                    Instantiate(bullet, firingPoint.position, firingPoint.rotation);
                }
            }


        }
        else
        {
            enemyRB.velocity = Vector2.zero;
        }
        
        if (movementDirection != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        
        Instantiate(HitEffect, transform.position, transform.rotation);
        
        if (health <= 0)
        {
            Destroy(gameObject);

            int chosenSplatter = Random.Range(0, SplatterEffects.Length);

            int splatterRotation = Random.Range(0,4);

            Instantiate(SplatterEffects[chosenSplatter], transform.position, Quaternion.Euler(0f,0f, splatterRotation*90f));
        }
    }
}
