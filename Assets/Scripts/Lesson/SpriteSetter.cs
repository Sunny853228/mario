using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSetter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Sprite _sprite;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SetSprite();
        }
    }

    public void SetSprite()
    {
        _sr.sprite = _sprite;
    }
}
