using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource level1Music, gameOverSound, winMusic;

    public AudioSource[] sfx;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playGameOver()
    {
        level1Music.Stop();
        gameOverSound.Play();
    }

    public void playVictory()
    {
        level1Music.Stop();
        winMusic.Play();
    }

    public void playSFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }
}
