using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusAtSpawnTime : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Collider2D _collider;
    
    private IActivable _activable;

    // Start is called before the first frame update
    private void Awake()
    {
        _rb.bodyType = RigidbodyType2D.Static;
        _collider.enabled = false;
        
        _activable = GetComponentInChildren<IActivable>();
        _activable.Deactivate();
    }

    public void OnSpawnFinished()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _collider.enabled = true;
        _activable.Activate();

        transform.parent = null;
    }
}
