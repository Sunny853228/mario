using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{

    public float speed;
    public float jumpFoerce;

    public bool isGrounded;

    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    public Animator animator;

    public int score;
    public TextMeshProUGUI scoreText;
    public GameObject PressS;
    public GameObject MarioSprite;
    public int CanMove;

    public void Start()
    {
        scoreText.text = "Score:" + score.ToString();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = MarioSprite.GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();
        score = PlayerPrefs.GetInt("Score");
        //scoreText.text = score.ToString();
        CanMove = 1;
    }

    public void FixedUpdate()
    {
        if (CanMove == 1)
        {
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
            Vector3 position = transform.position;

            position.x += Input.GetAxis("Horizontal") * speed;
            //position.y += Input.GetAxis("Vertical");

            transform.position = position;

            if (Input.GetAxis("Horizontal") != 0)
            {
                if (Input.GetAxis("Horizontal") < 0)
                {
                    spriteRenderer.flipX = true;
                }
                else if (Input.GetAxis("Horizontal") > 0)
                {
                    spriteRenderer.flipX = false;
                }
                if (isGrounded)
                    animator.SetInteger("Idle", 1);
            }
            else
            {
                if (isGrounded)
                    animator.SetInteger("Idle", 0);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            //rigidbody2d.AddForce(transform.up * jumpFoerce, ForceMode2D.Impulse);
            rigidbody2d.velocity = transform.up * jumpFoerce;
            animator.SetInteger("Idle", 2);
        }
    }

    public void AddCoin(int count)
    {
        score += count;
        scoreText.text = "Score:" + score.ToString();
        PlayerPrefs.SetInt("Score", score);
        if (PlayerPrefs.GetInt("MaxScore") < score)
        {
            PlayerPrefs.SetInt("MaxScore", score);
        }
    }



    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            AddCoin(1);
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Teleport")
        {
            PressS.SetActive(true);

            if (Input.GetKey(KeyCode.S))
            {
                CanMove = 0;
                transform.position = collision.transform.position;
                animator.SetInteger("Idle", 3);
                NextRandomMap();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Teleport")
        {
            PressS.SetActive(false);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }

    }


    public void NextRandomMap()
    {
        SceneManager.LoadScene(Random.Range(1, 3));
    }




}
