using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int points = 10; // очки за разрушение

    void OnCollisionEnter2D(Collision2D collision)
    {
        // —толкновение с игроком
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Debug.Log($"Ќормаль: {contact.normal}, y = {contact.normal.y}");
                Debug.DrawLine(contact.point, contact.point + contact.normal * 2, Color.magenta, 2f); // рисуем нормаль
                if (contact.normal.y > 0.8f) //игрок снизу
                {
                    BreakBlock(collision.gameObject);
                    return;
                }
            }
        }
    }

    void BreakBlock(GameObject player)
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
            playerScript.AddCoin(points);

        Destroy(gameObject);
    }
}