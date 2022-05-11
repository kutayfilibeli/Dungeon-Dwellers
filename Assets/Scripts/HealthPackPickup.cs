using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackPickup : MonoBehaviour
{
    public int healingAmount = 1;

    public float timeBeforePickup = .5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBeforePickup > 0)
        {
            timeBeforePickup -= Time.deltaTime;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
     if(other.tag == "Player" && timeBeforePickup <= 0)
        {
            PlayerHealthController.instance.HealPlayer(healingAmount);

            Destroy(gameObject);
        }   
    }
}
