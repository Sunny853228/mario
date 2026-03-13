using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 2f;               // скорость движения
    [SerializeField] private LayerMask _groundLayer;          // слой земли
    [SerializeField] private LayerMask _playerLayer;          // слой игрока

    [Header("Edge Detection")]
    [SerializeField] private float _edgeCheckDistance = 1f;   // длина луча для проверки края
    [SerializeField] private Vector2 _groundCheckOffset = new Vector2(0.5f, -0.5f); // смещение точки луча от центра (x – вперёд, y – вниз)

    private Rigidbody2D _rigidBody;
    private SpriteRenderer _spriteRenderer;
    private int _direction = 1; // 1 – вправо, -1 – влево

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Определяем начальное направление по спрайту (если спрайт развёрнут, то идём влево)
        if (_spriteRenderer.flipX)
            _direction = -1;
    }

    void FixedUpdate()
    {
        // Горизонтальное движение с постоянной скоростью (вертикальная скорость сохраняется от гравитации)
        _rigidBody.velocity = new Vector2(_speed * _direction, _rigidBody.velocity.y);

        // Проверка края (отсутствие земли впереди-снизу)
        Vector2 rayOrigin = (Vector2)transform.position + new Vector2(_direction * _groundCheckOffset.x, _groundCheckOffset.y);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, _edgeCheckDistance, _groundLayer);
        Debug.DrawRay(rayOrigin, Vector2.down * _edgeCheckDistance, hit.collider == null ? Color.red : Color.green);

        if (hit.collider == null) // впереди обрыв
        {
            FlipDirection();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Столкновение со стеной (земля)
        if (((1 << collision.gameObject.layer) & _groundLayer) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // contact.normal направлен от врага к объекту, с которым столкнулись
                // Проверяем, направлена ли нормаль примерно противоположно движению
                float dot = Vector2.Dot(contact.normal, Vector2.right * _direction);
                if (dot < -0.7f) // встречная стена
                {
                    FlipDirection();
                    break;
                }
            }
        }

        // Столкновение с игроком
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Если игрок сверху (нормаль от врага к игроку направлена вверх)
                if (contact.normal.y > 0.7f)
                {
                    // Враг умирает
                    Destroy(gameObject);
                    return;
                }
            }
            // Иначе игрок получает урон – перезагрузка уровня
            if (RespawnManager.Instance != null)
                RespawnManager.Instance.Respawn();
            else
                Debug.LogError("RespawnManager не найден! Добавьте объект RespawnManager на сцену.");
        }
    }

    void FlipDirection()
    {
        _direction *= -1;
        _spriteRenderer.flipX = !_spriteRenderer.flipX; // разворачиваем спрайт
    }
}