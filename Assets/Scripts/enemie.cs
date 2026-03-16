using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    [Header("Edge Detection")]
    [SerializeField] private float edgeCheckDistance = 1f;
    [SerializeField] private Vector2 groundCheckOffset = new Vector2(0.5f, -0.5f);

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private int direction = 1; // 1 – вправо, -1 – влево

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (spriteRenderer.flipX)
            direction = -1;
    }

    void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(speed * direction, rigidBody.velocity.y);

        // Проверка края
        Vector2 rayOrigin = (Vector2)transform.position + new Vector2(direction * groundCheckOffset.x, groundCheckOffset.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, edgeCheckDistance, groundLayer);
        Debug.DrawRay(rayOrigin, Vector2.down * edgeCheckDistance, hit.collider == null ? Color.red : Color.green);

        if (hit.collider == null) // впереди обрыв
        {
            FlipDirection();
            return;
        }

        // Проверка стены впереди
        Vector2 forwardRayOrigin = (Vector2)transform.position + new Vector2(direction * 0.5f, 0);
        float rayLength = 0.1f;
        RaycastHit2D wallHit = Physics2D.Raycast(forwardRayOrigin, Vector2.right * direction, rayLength, groundLayer);
        Debug.DrawRay(forwardRayOrigin, Vector2.right * direction * rayLength, wallHit.collider != null ? Color.red : Color.green);

        if (wallHit.collider != null)
        {
            FlipDirection();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Столкновение со стеной (земля)
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                float dot = Vector2.Dot(contact.normal, Vector2.right * direction);
                if (dot < -0.7f) // встречная стена
                {
                    FlipDirection();
                    break;
                }
            }
        }

        // Столкновение с игроком
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Debug.Log($"Нормаль: {contact.normal}, y = {contact.normal.y}");
                Debug.DrawLine(contact.point, contact.point + contact.normal * 2, Color.magenta, 2f); // рисуем нормаль
                if (contact.normal.y < 0.7f) // игрок сверху
                {
                    Destroy(gameObject);
                    return;
                }
            }
            // Иначе – игрок получает урон
            if (RespawnManager.Instance != null)
                RespawnManager.Instance.Respawn();
            else
                Debug.LogError("RespawnManager не найден!");
        }
    }

    void FlipDirection()
    {
        direction *= -1;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}