using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Rigidbody2D enemyRB;
    public float movementSpeed;

    [Header("Chasing")]
    public bool shouldChasePlayer;
    public float playerChaseRange;
    private Vector3 movementDirection;
    [Header("Wander")]
    public bool shouldWander;
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;
    [Header("Patrolling")]
    public bool shouldPatrol;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;
    [Header("Running Away")]
    public bool shouldRunAway;
    public float runAwayRange;

    
    
    [Header("Shooting")]
    public bool canShoot;
    public GameObject bullet;
    public Transform firingPoint;
    public float rateofFire;
    private float fireCounter;
    public float shootrange;

    [Header("Variables")]
    public SpriteRenderer body;
    public Animator animator;

    public int health = 150;

    public GameObject[] SplatterEffects;
    public GameObject HitEffect;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropRate;



    // Start is called before the first frame update
    void Start()
    {
        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(body.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            movementDirection = Vector3.zero;

            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < playerChaseRange && shouldChasePlayer)
            {
                movementDirection = PlayerController.instance.transform.position - transform.position;
            }
            else
            {

                if (shouldWander)
                {
                    if (wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        movementDirection = wanderDirection;

                        if (wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                        }

                    }

                        if(pauseCounter > 0)
                        {
                            pauseCounter -= Time.deltaTime;

                            if(pauseCounter <= 0)
                            {
                                wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                                wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                            }
                        }
                    
                }
                if (shouldPatrol)
                {
                    movementDirection = patrolPoints[currentPatrolPoint].position - transform.position;
                    if(Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < .2f)
                    {
                        currentPatrolPoint++;
                        if(currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0;
                        }
                    }
                }
            }
            if(shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runAwayRange)
            {
                movementDirection = transform.position - PlayerController.instance.transform.position;
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
                    AudioManager.instance.playSFX(12);
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

        AudioManager.instance.playSFX(2);

        Instantiate(HitEffect, transform.position, transform.rotation);
        
        if (health <= 0)
        {
            Destroy(gameObject);

            AudioManager.instance.playSFX(1);

            int chosenSplatter = Random.Range(0, SplatterEffects.Length);

            int splatterRotation = Random.Range(0,4);

            Instantiate(SplatterEffects[chosenSplatter], transform.position, Quaternion.Euler(0f,0f, splatterRotation*90f));

            if (shouldDropItem)
            {
                float dropRate = Random.Range(0f, 100f);

                if (dropRate < itemDropRate)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);

                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }
        }
    }
}
