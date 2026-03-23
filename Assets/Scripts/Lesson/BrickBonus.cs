using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBonus : MonoBehaviour
{
    [SerializeField] private Animator _brickAnimator;
    [SerializeField] private Animator _bonusAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Столкновение с игроком
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Debug.Log($"Нормаль: {contact.normal}, y = {contact.normal.y}");
                Debug.DrawLine(contact.point, contact.point + contact.normal * 2, Color.magenta, 2f); // рисуем нормаль
                if (contact.normal.y > 0.8f) //игрок снизу
                {
                    OnHit();
                    return;
                }
            }
        }
    }

    private void OnHit()
    {
        _brickAnimator.SetTrigger("Hit");
        _bonusAnimator.SetTrigger("Hit");
    }
}
