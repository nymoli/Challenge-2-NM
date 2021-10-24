using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;
    
    public Text score;
    public Text lives;
    public Text winText;
    public Text loseText;
    
    public GameObject winTextObject;
    public GameObject loseTextObject;

    private int scoreValue = 0;
    private int livesValue = 3;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    private Animator anim;

    private bool isRunning;
    private bool isJumping;

    private bool facingRight = true;

    private SpriteRenderer _renderer;

    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();

        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();

        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;

        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

        anim = GetComponent<Animator>();

        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null)
        {
            Debug.LogError("Player Sprite is missing a renderer");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if(scoreValue == 4)
            {
                livesValue = 3;
                lives.text = "Lives :" + livesValue.ToString();
                transform.position = new Vector3(61.0f, 0.0f, 0.0f);
            }

            if(scoreValue == 8)
            {
                winTextObject.SetActive(true);

                musicSource.Pause();

                musicSource.clip = musicClipTwo;
                musicSource.Play();
            }
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);

            if (livesValue == 0)
            {
                loseTextObject.SetActive(true);
                Destroy(gameObject);
            }

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }

        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    void Update()
    {
        //animation stuff
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            anim.SetBool("isRunning", true); //while running
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("isRunning", false); //stop running
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetBool("isJumping", true); //while jumping
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetBool("isJumping", false); //stop jumping
        }

        //flip character
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            _renderer.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            _renderer.flipX = true;
        }


    }

}
