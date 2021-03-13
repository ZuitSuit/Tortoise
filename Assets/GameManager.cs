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
    public TextMeshProUGUI timerText, endTimerText;

    bool timerActive, anyKeyFlag;
    float timer;

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

        foreach (GameObject word in words)
        {
            word.SetActive(true);
            yield return new WaitForSeconds(1f);
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

        startScreen.SetActive(false);
        gameUI.SetActive(true);
        timerActive = true;
    }
    IEnumerator GameEnd()
    {
        gameUI.SetActive(false);
        timerActive = false;
        yield return new WaitForSeconds(2f);

        endScreen.SetActive(true);
        endTimerText.text = ((int)timer / 3600).ToString("d2") + ":" + (((int)timer % 3600) / 60).ToString("d2") + ":" + (((int)timer % 3600) % 60).ToString("d2");

        yield return null;
    }
}
