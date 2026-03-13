using UnityEngine;

public class Normals : MonoBehaviour
{
    [SerializeField] private Vector2 _e;
    [SerializeField] private float _a;

    private Rigidbody2D _rigidbody2D;
    private bool _isGroundedByNormals = false;

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
            if (cos >= _a)
            {
                _isGroundedByNormals = true;
            }
            Debug.DrawLine(contact.point, contact.point + contact.normal, cos >= _a ? Color.blue : Color.red);
        }
    }
}