using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject startScreen, endScreen, gameUI;
    public List<GameObject> words;
    public GameObject anyKeyPrompt;
    public TextMeshProUGUI timerText, endTimerText, bestTimerText;

    bool timerActive, anyKeyFlag;
    float timer;

    public Tortoise player;


    public AudioSource audioSource;
    public List<AudioClip> titleWords;

    public GameObject highScore;


    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timer = 0;
        startScreen.SetActive(true);
        endScreen.SetActive(false);
        gameUI.SetActive(false);
        StartCoroutine(StartupAnimation());


    }

    void Update()
    {
        if (timerActive)
        {
            timer += Time.deltaTime;
            timerText.text = ((int)timer /3600).ToString("d2") + ":" + (((int)timer % 3600) / 60).ToString("d2") + ":" + (((int)timer % 3600) % 60).ToString("d2");

        }
    }

    public void EndGame()
    {
        StartCoroutine(GameEnd());
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    IEnumerator StartupAnimation()
    {
        foreach (GameObject word in words)
        {
            word.SetActive(false);
        }
        yield return new WaitForSeconds(.7f);
        int wordIterator = 0;
        foreach (GameObject word in words)
        {
            word.SetActive(true);
            audioSource.PlayOneShot(titleWords[wordIterator]);
            wordIterator++;
            yield return new WaitForSeconds(.7f);
        }

        anyKeyFlag = true;
        StartCoroutine(StartGame());
        anyKeyPrompt.SetActive(true);


        yield return null;
    }

    IEnumerator StartGame()
    {
        

        while (!anyKeyFlag || !Input.anyKey)
        {
            yield return null;
        }

        player.music.Play();
        player.SetYeetJuice(20f);
        player.active = true;
        startScreen.SetActive(false);
        gameUI.SetActive(true);
        timerActive = true;
        
    }
    IEnumerator GameEnd()
    {
        
        if (!PlayerPrefs.HasKey("bestTime"))
        {
            PlayerPrefs.SetFloat("bestTime", timer);
        }

        float oldRecord = PlayerPrefs.GetFloat("bestTime");
        
        if (timer < oldRecord)
        {
            PlayerPrefs.SetFloat("bestTime", timer);
            
        }

        highScore.SetActive(timer <= oldRecord);

        PlayerPrefs.Save();

        gameUI.SetActive(false);
        timerActive = false;
        yield return new WaitForSeconds(2f);

        endScreen.SetActive(true);
        bestTimerText.text = ((int)oldRecord / 3600).ToString("d2") + ":" + (((int)oldRecord % 3600) / 60).ToString("d2") + ":" + (((int)oldRecord % 3600) % 60).ToString("d2");
        endTimerText.text = ((int)timer / 3600).ToString("d2") + ":" + (((int)timer % 3600) / 60).ToString("d2") + ":" + (((int)timer % 3600) % 60).ToString("d2");

        yield return null;
    }
}
