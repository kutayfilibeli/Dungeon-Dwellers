using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Guns gun; 
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
        if (other.tag == "Player" && timeBeforePickup <= 0)
        {
            bool ownsGun = false;

            foreach(Guns gunToCheck in PlayerController.instance.usableGuns)
            {
                if(gun.weaponName == gunToCheck.weaponName)
                {
                    ownsGun = true;
                }
            }

            if (!ownsGun)
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

            Destroy(gameObject);

            AudioManager.instance.playSFX(7);
        }
    }
}
