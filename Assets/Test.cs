using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    SpriteRenderer sprite;
    SpriteMask spriteMask;
    public bool isLight = false;
    public bool isGround = false;
    public Sprite box;
    Rigidbody2D _rb;
    

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!isLight)
        {
            sprite.GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            sprite.sprite = box;
            sprite.GetComponent<Collider2D>().isTrigger = false;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, sprite.size.y + 0.1f, LayerMask.GetMask("Ground"));
        isGround = hit;
        if (!isGround)
        {
            _rb.gravityScale = 1;
        }
        else
        {
            _rb.gravityScale = 0;
            _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Light"))
        {
            isLight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Light"))
        {
            isLight = false;
        }
    }
}
