using UnityEngine;

public class Normals : MonoBehaviour
{
    [SerializeField] private Vector2 _e;      // направление Ђземлиї (например, (0, 1) Ц вверх)
    [SerializeField] private float _a;        // порог косинуса (1 Ц строго вертикально)

    private Rigidbody2D _rigidbody2D;
    private bool _isGroundedByNormals = false;

    // ѕубличное свойство дл€ доступа из скрипта Player
    public bool IsGroundedByNormals => _isGroundedByNormals;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _isGroundedByNormals = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            float cos = Vector2.Dot(_e, contact.normal) / (_e.magnitude * contact.normal.magnitude);

            Color color = cos >= _a ? Color.blue : Color.red;
            Debug.DrawLine(contact.point, contact.point + contact.normal, color);

            // ≈сли нормаль достаточно близка к заданному направлению, считаем, что персонаж на земле
            if (cos >= _a)
            {
                _isGroundedByNormals = true;
            }
        }
    }
}