using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemies : MonoBehaviour
{
    public int count;
    public float speed;
    public Vector3[] positions;
    public GameObject poz1, poz2;

    private int currentTarget;
    public void Start()
    {
        positions[0] = new Vector3(poz1.transform.position.x, poz1.transform.position.y);
        positions[1] = new Vector3(poz2.transform.position.x, poz2.transform.position.y);
    }

    public void FixedUpdate()
    {
        poz1.transform.position = positions[0];
        poz2.transform.position = positions[1];
        transform.position = Vector3.MoveTowards(transform.position, positions[currentTarget], speed);

        if (transform.position == positions[currentTarget])
        {
            if (currentTarget < positions.Length -1)
            {
                currentTarget++;
            } else 
            {
                currentTarget = 0;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(0);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            Destroy(gameObject);
            collision.GetComponent<Player>().AddCoin(count);
        }
    }
}