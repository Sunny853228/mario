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

    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    public Animator animator;

    public int score;
    public TextMeshProUGUI scoreText;
    public GameObject MarioSprite;
    public int CanMove;

    public Transform spawnPoint;

    private Normals normals;

    public void Start()
    {
        scoreText.text = "Score:" + score.ToString();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = MarioSprite.GetComponent<SpriteRenderer>();
        // animator = GetComponent<Animator>(); // раскомментируйте, если аниматор не назначен в инспекторе
        score = PlayerPrefs.GetInt("Score");
        CanMove = 1;

        normals = GetComponent<Normals>();
        if (normals == null)
        {
            Debug.LogError("На объекте Player отсутствует компонент Normals! Прыжки не будут работать.");
        }
    }

    public void FixedUpdate()
    {
        if (CanMove == 1)
        {
            bool grounded = normals != null ? normals.IsGroundedByNormals : false;

            //прыжок
            if (Input.GetKey(KeyCode.Space) && grounded)
            {
                Jump();
            }

            //движение
            Vector3 position = transform.position;
            position.x += Input.GetAxis("Horizontal") * speed;
            transform.position = position;

            //поворот и анимация бега
            if (Input.GetAxis("Horizontal") != 0)
            {
                if (Input.GetAxis("Horizontal") < 0)
                    spriteRenderer.flipX = true;
                else if (Input.GetAxis("Horizontal") > 0)
                    spriteRenderer.flipX = false;

                if (grounded)
                    animator.SetInteger("Idle", 1);
            }
            else
            {
                if (grounded)
                    animator.SetInteger("Idle", 0);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);

        if (Input.GetKeyDown(KeyCode.R))
            Respawn();
    }

    private void Jump()
    {
        rigidbody2d.velocity = transform.up * jumpFoerce;
        animator.SetInteger("Idle", 2);
    }

    private void Respawn()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            rigidbody2d.velocity = Vector2.zero;
            animator.SetInteger("Idle", 0);
        }
        else
        {
            Debug.LogWarning("Точка спавна не назначена!");
        }
    }

    public void AddCoin(int count)
    {
        score += count;
        scoreText.text = "Score:" + score.ToString();
        PlayerPrefs.SetInt("Score", score);
        if (PlayerPrefs.GetInt("MaxScore") < score)
            PlayerPrefs.SetInt("MaxScore", score);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            AddCoin(1);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "DeathZone")
        {
            if (RespawnManager.Instance != null)
                RespawnManager.Instance.Respawn();
            else
                Debug.LogError("RespawnManager не найден! Добавьте объект RespawnManager на сцену.");
        }
    }

    public void NextRandomMap()
    {
        SceneManager.LoadScene(Random.Range(1, 3));
    }
}
