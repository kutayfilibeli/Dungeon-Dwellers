using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float movementSpeed;  

    private Vector2 moveInput;

    public Rigidbody2D RB;

    public Transform gunArm;

    private Camera cam;

    public Animator animator;

    public GameObject fireBullet;

    public Transform shootingPoint;

    public SpriteRenderer bodySR;
   
    public float timeBetweenShots;

    private float shotCounter;

    private float activeMoveSpeed;

    public float dashSpeed = 8f, dashLength = .5f, dashCooldown = 1f, dashInvincibility = .5f;
    [HideInInspector]
    public float dashCounter;
    private float dashCooldownCounter;

    [HideInInspector]
    public bool canMove = true;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        activeMoveSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {


            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            //Duvara çarparken karakter titriyor.
            //transform.position += new Vector3 (moveInput.x * Time.deltaTime * movementSpeed, moveInput.y * Time.deltaTime * movementSpeed, 0f);

            RB.velocity = moveInput * activeMoveSpeed;

            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = cam.WorldToScreenPoint(transform.localPosition);

            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                gunArm.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                gunArm.localScale = Vector3.one;
            }

            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(0, 0, angle);



            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(fireBullet, shootingPoint.position, shootingPoint.rotation);

                shotCounter = timeBetweenShots;

                AudioManager.instance.playSFX(11);
            }

            if (Input.GetMouseButton(0))
            {
                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0)
                {
                    Instantiate(fireBullet, shootingPoint.position, shootingPoint.rotation);

                    shotCounter = timeBetweenShots;
                    AudioManager.instance.playSFX(11);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dashCooldownCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    animator.SetTrigger("Dash");

                    PlayerHealthController.instance.invincibleWhileDashing(dashInvincibility);

                    AudioManager.instance.playSFX(8);
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = movementSpeed;
                    dashCooldownCounter = dashCooldown;
                }
            }

            if (dashCooldownCounter > 0)
            {
                dashCooldownCounter -= Time.deltaTime;
            }







            if (moveInput != Vector2.zero)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
        else
        {
            RB.velocity = Vector2.zero;
            animator.SetBool("isMoving", false);
        }
        }
}
