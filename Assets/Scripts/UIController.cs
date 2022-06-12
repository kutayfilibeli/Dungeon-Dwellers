using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthSlider;
    public Text healthText, coinText;

    public GameObject DeathScreen;

    public Image fadeOutScreen;
    public float fadeOutSpeed;
    private bool fadeToBlack, fadeOutBlack;

    public string newGameScene, mainMenuScene;

    public GameObject pauseMenu, mapDisplay, bigMapText;

    public Image currentGun;
    public Text gunNameText;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        fadeOutBlack = true;
        fadeToBlack = false;

        currentGun.sprite = PlayerController.instance.usableGuns[PlayerController.instance.currentGun].gunUI;
        gunNameText.text = PlayerController.instance.usableGuns[PlayerController.instance.currentGun].weaponName;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOutBlack)
        {
            fadeOutScreen.color = new Color(fadeOutScreen.color.r, fadeOutScreen.color.g, fadeOutScreen.color.b, Mathf.MoveTowards(fadeOutScreen.color.a, 0f, fadeOutSpeed * Time.deltaTime));
            if(fadeOutScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }

        if (fadeToBlack)
        { 
            fadeOutScreen.color = new Color(fadeOutScreen.color.r, fadeOutScreen.color.g, fadeOutScreen.color.b, Mathf.MoveTowards(fadeOutScreen.color.a, 1f, fadeOutSpeed * Time.deltaTime));
            if (fadeOutScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public void NewGame()
    {
        Time.timeScale = 2f;
        SceneManager.LoadScene(newGameScene);

        Destroy(PlayerController.instance.gameObject);
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);

        Destroy(PlayerController.instance.gameObject);
    }

    public void Resume()
    {
        LevelManager.instance.PauseUnpause();
    }
}
