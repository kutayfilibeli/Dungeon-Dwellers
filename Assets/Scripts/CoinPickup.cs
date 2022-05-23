using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;

    public float timeBeforePickup = .4f;
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
        if (other.tag == "Player" && timeBeforePickup <= 0)
        {
            LevelManager.instance.GetCoins(coinValue);

            Destroy(gameObject);

            AudioManager.instance.playSFX(5);
        }
    }
}

