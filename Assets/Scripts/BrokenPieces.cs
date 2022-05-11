using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{

    public float spreadSpeed = 3f;
    private Vector3 moveDirection;

    public float piecesSlowing = 5f;
    public float lifetime = 3f;

    public SpriteRenderer sr;
    public float fadeoutSpeed = 2.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        moveDirection.x = Random.Range(-spreadSpeed, spreadSpeed);
        moveDirection.y = Random.Range(-spreadSpeed, spreadSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * Time.deltaTime;

        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, piecesSlowing * Time.deltaTime);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.MoveTowards(sr.color.a, 0f, fadeoutSpeed*Time.deltaTime));

            if (sr.color.a == 0f)
            {
                Destroy(gameObject);
            }
        }
    }

   
}
