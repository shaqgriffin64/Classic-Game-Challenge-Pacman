using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement2 : MonoBehaviour
{
    public float speed;
    public Text scoreText;
    public Text livesText;
    public Text winText;
    public Text loseText;
    public float maxSpeed = 20f;
    public Camera mainMenuCamera;
    public Camera levelOneCamera;
    public Camera levelTwoCamera;
    public AudioClip backgroundMusic;
    public AudioClip victoryClip;
    public AudioClip defeatClip;
    public AudioClip pickUpclip;
    public AudioClip enemyClip;

    private Rigidbody2D rb2d;
    private int score;
    private int lives;
    private AudioSource audioSource;

    void Start()
    {
        //setting the audiosource, I'm aware that the naming conventions here seem redundant
        audioSource = GetComponent<AudioSource>();

        //fetching data for the rigidbody2d component on the "Player" object
        rb2d = GetComponent<Rigidbody2D>();
        
        //setting beginning values for score and lives
        score = 0;
        lives = 3;

        //initializing score text
        SetScoreText();

        //initializing lives text
        SetLivesText();

        //Setting main menu camera as the active camera
        MainMenuCamera();

        //initialization of win and lose text to be blank
        winText.text = "";
        loseText.text = "";
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        rb2d.AddForce(movement * speed);

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown("space"))
        {
            LevelOneCamera();
        }

        if (GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * maxSpeed;
        }
    }

    void Update()
    {
    }

    //initializes the score text on the UI
    void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();


        if (score == 1460)
        {
            transform.position = new Vector3(163, 12, 0);

            LevelTwoCamera();
        }
        if (score == 2720)
        {
            winText.text = "You Win!!!";

            audioSource.volume = 0.3f;
            audioSource.clip = victoryClip;
            audioSource.loop = false;
            audioSource.Play();

            speed = 0;
        }
    }
    
    //initializes the lives text on the UI
    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();

        if (lives == 0)
        {
            loseText.text = "You lost :(";

            audioSource.volume = 0.3f;
            audioSource.clip = defeatClip;
            audioSource.loop = false;
            audioSource.Play();

            gameObject.SetActive(false);

            speed = 0;
        }
    }

    //sets adjusts the score when the player collides with a pickup
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);

            audioSource.volume = 0.2f;
            audioSource.clip = pickUpclip;
            audioSource.loop = false;
            audioSource.Play();

            score = score + 10;

            SetScoreText();
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);

            audioSource.volume = 0.2f;
            audioSource.clip = enemyClip;
            audioSource.loop = false;
            audioSource.Play();

            lives = lives - 1;

            SetLivesText();
        }
    }

    //Sets main menu camera to be active
    public void MainMenuCamera()
    {
        mainMenuCamera.enabled = true;
        levelOneCamera.enabled = false;
        levelTwoCamera.enabled = false;
    }

    //Sets first camera to be active
    public void LevelOneCamera()
    {
        mainMenuCamera.enabled = false;
        levelOneCamera.enabled = true;
        levelTwoCamera.enabled = false;
    }

    //sets second camera to be active
    public void LevelTwoCamera()
    {
        mainMenuCamera.enabled = false;
        levelOneCamera.enabled = false;
        levelTwoCamera.enabled = true;
    }
}
