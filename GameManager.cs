using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }
    //keep track 
    private bool startGame = false;
    private PlayerController controller;
    public bool IsDead { set; get; }
	//UI field
	public Animator gameCanvasAnimator;
	public Animator startCanvasAnimator;
    public Text scoreText;
    public Text livesText;
    public Text modifierText;
    public float score;
    public int lives;
    public float modifier;
    private int lastScore;
    //end menu game over
	public Animator deathMenuAnim;
    public Text finalScoreText, finalLevelText;

	private void Awake()
    {
        Instance = this;
        modifier = 1;
        score = 0;
        lives = 3;
        modifier = 1;
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        scoreText.text = "SCORE: " + score.ToString("0");
        modifierText.text = "LEVEL SPEED: x" + modifier.ToString("0.0");
        livesText.text = "LIVES: " + lives.ToString();
		startCanvasAnimator.SetTrigger("ShowStart");
    }

    private void Update()
    {
        if(MobileInput.Instance.Tap && !startGame)
        {
            startGame = true;
            controller.TurnOn();
			FindObjectOfType<CameraController>().IsMoving = true;
			startCanvasAnimator.SetTrigger("HideStart");
			gameCanvasAnimator.SetTrigger("Show");

		}
        if (startGame && !IsDead)
        {
            if(lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = "SCORE: " + score.ToString("0");
            }
        }
    }

    public void PointScore()
    {
        score++;
        scoreText.text = "SCORE: " + score.ToString("0");
    }


    public void UpdateLives()
    {
        livesText.text = "LIVES: " + lives.ToString();
    }

    public void UpdateLevelSpeed(float levelUpAmount)
    {
        modifier = 1.0f + levelUpAmount;
        modifierText.text = "LEVEL SPEED: x" + modifier.ToString("0.0");
    }

	public void OnPlayButton()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
	}
    
    public void OnDeath()
	{
		IsDead = true;
	    finalScoreText.text = "FINAL SCORE: " + score.ToString("0");
		finalLevelText.text = "FINAL LEVEL SPEED: x" + modifier.ToString("0.0");
		deathMenuAnim.SetTrigger("Dead");
		gameCanvasAnimator.SetTrigger("Hide");
	}
}
