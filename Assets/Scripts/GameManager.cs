using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.VersionControl.Asset;

public class GameManager : MonoBehaviour
{

    public enum States
    {
        wait, play, levelup, dead
    }
    public static States state;

    int level;
    int score;
    int lives;

    Camera cam;
    float height, width;

    public Text levelTxt;
    public Text scoreTxt;
    public Text livesTxt;

    public Text messageTxt;

    public GameObject asteroid;
    public GameObject boom;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        InitGame();
        LoadLevel();
        UpdateTexts();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == States.play) EndOfLevel();
    }

    void InitGame()
    {
        messageTxt.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");

        cam = Camera.main;
        height = cam.orthographicSize;
        width = height * cam.aspect;

        state = States.wait;

        level = 1;
        score = 0;
        lives = 5;
    }

    void LoadLevel()
    {
        state = States.play;

        for(int i = 0; i < 2 + level; i++)
        {
            float x = Random.Range(-width, width);
            float y = Random.Range(-height, height);
            Instantiate(asteroid, new Vector2(x,y), Quaternion.identity);
        }
    }

    void UpdateTexts()
    {
        levelTxt.text = "level: " + level;
        scoreTxt.text = "score: " + score;
        livesTxt.text = "lives: " + lives;
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateTexts();
    }

    void EndOfLevel()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemys.Length == 0)
        {
            StartCoroutine(LevelUp());
        }
    }

    IEnumerator LevelUp()
    {
        state = States.levelup;
        // display message "level up"
        messageTxt.text = "level up";
        messageTxt.gameObject.SetActive(true);
        // pause few seconds
        yield return new WaitForSecondsRealtime(2f);
        // hide message
        messageTxt.gameObject.SetActive(false);
        level += 1;
        LoadLevel();
        UpdateTexts();
    }

    public void KillPlayer()
    {
        StartCoroutine(PlayerAgain());
    }

    public void GameOver()
    {
        state = States.wait;
        messageTxt.text = "game over";
        messageTxt.gameObject.SetActive(true);
    }

    IEnumerator PlayerAgain()
    {
        state = States.dead;
        // Show boom at the player position
        GameObject boomGO = Instantiate(boom, player.transform.position, Quaternion.identity);
        lives -= 1;
        player.SetActive(false);
        UpdateTexts();

        if (lives <= 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            Destroy(boomGO);
            GameOver();
        }
        else
        {
            yield return new WaitForSecondsRealtime(2.5f);
            Destroy(boomGO);
            player.SetActive(true);
            state = States.play;
        }
    }
}
