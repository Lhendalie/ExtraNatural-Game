using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameCtrl : MonoBehaviour {

    public int points;
    public int lives;

    public Text pointsText;
    public Text livesText;
    public Text healthText;
    public Text keyText;
    public Text winScore;

    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject pauseMenu;
    public GameObject player;
    public GameObject lastCheckPoint;

    public Animator playerAnim;

    public PlayerCtrl pc;

    public bool playerDead = false;
    public bool hasKey = false;
    public bool gameOver = false;
    public bool isPaused;

    public void Start()
    {
        StartTime();
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }

    public void Update()
    {
        //Connects the UI elements (score, health, lives)
        livesText.text = ("" + lives);
        pointsText.text = ("Score: " + points);
        healthText.text = ("Health: " + HealthbarCtrl.health);
        int keyAquired = Convert.ToInt32(hasKey);
        keyText.text = ("Key: " + keyAquired + "/1");
        winScore.text = ("" + points);

        //Pauses the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (winScreen.activeInHierarchy || loseScreen.activeInHierarchy)
            {
                return;
            }

            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                isPaused = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
            }
        }             
    }

    //Unpauses the game
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        StartTime();
    }

    //Respawns the player on the last check point
    public void RespawnPlayer()
    {
        HealthbarCtrl.health = 10;
        lives = lives - 1;
        playerAnim.SetBool("IsDead", false);
        player.transform.position = lastCheckPoint.transform.position;
        playerDead = false;
    }

    //Kills the player
    public void Die()
    {
        playerAnim.SetBool("IsJumping", false);
        playerAnim.SetFloat("Speed", 0);

        playerDead = true;

        if (lives <= 1)
        {
            gameOver = true;
            loseScreen.SetActive(true);
            Time.timeScale = 0f;
            GetComponent<AudioSource>().Stop();
        }
        else
        {
            playerAnim.SetBool("IsDead", true);

            if (lives > 1)
            {
                StartCoroutine(LateCall());
            }
        }
    }

    //This happens when the level is considered completed
    public void EndGame()
    {
        winScreen.SetActive(true);
        this.GetComponent<AudioSource>().Stop();
        Time.timeScale = 0f;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void StartTime()
    {
        Time.timeScale = 1f;
    }

    IEnumerator LateCall()
    {
        yield return new WaitForSeconds(4);

        RespawnPlayer();
    }

   
}
