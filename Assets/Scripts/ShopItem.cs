using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopItem : MonoBehaviour
{
    public GameObject buyText;

    private bool inBuyZone;

    public bool isHealthRestore, isHealthUpgrade, isWeapon;

    public int itemCost;

    public int healthUpgradeAmount;

    public Guns[] potentialGuns;
    private Guns gun;
    public SpriteRenderer gunSprite;
    public Text infoText;
    // Start is called before the first frame update
    void Start()
    {
        if (isWeapon)
        {
            int selectedGun = Random.Range(0, potentialGuns.Length);
            gun = potentialGuns[selectedGun];

            gunSprite.sprite = gun.gunShopSprite;
            infoText.text = gun.weaponName + "\n - " + gun.itemCost + " Gold - ";
            itemCost = gun.itemCost;
        }
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
                    if (isWeapon)
                    {
                        Guns gunClone = Instantiate(gun);
                        gunClone.transform.parent = PlayerController.instance.gunArm;
                        gunClone.transform.position = PlayerController.instance.gunArm.position;
                        gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                        gunClone.transform.localScale = Vector3.one;

                        PlayerController.instance.usableGuns.Add(gunClone);
                        PlayerController.instance.currentGun = PlayerController.instance.usableGuns.Count - 1;
                        PlayerController.instance.GunSwitch();
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
