using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public GameObject buyText;

    private bool inBuyZone;

    public bool isHealthRestore, isHealthUpgrade, isWeapon;

    public int itemCost;

    public int healthUpgradeAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inBuyZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(LevelManager.instance.currentCoins >= itemCost)
                {
                    LevelManager.instance.SpendCoins(itemCost);

                    if (isHealthRestore)
                    {
                        PlayerHealthController.instance.HealPlayer(PlayerHealthController.instance.maxHealth);                       
                    }

                    if (isHealthUpgrade)
                    {
                        PlayerHealthController.instance.IncreaseMaxHealth(healthUpgradeAmount);                      
                    }                    
                    gameObject.SetActive(false);
                    inBuyZone = false;
                    AudioManager.instance.playSFX(17);
                }
                else
                {
                    AudioManager.instance.playSFX(18);
                }
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            buyText.SetActive(true);

            inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buyText.SetActive(false);

            inBuyZone = false;
        }
    }

}
