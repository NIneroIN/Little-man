using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    PlayerMovement player;

    SpriteRenderer sprite;
    SpriteMask spriteMask;
    public bool isLight = false;
    public bool isGround = false;
    public Sprite box;
    Rigidbody2D _rb;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        FindLight();
        FindGround();
    }

    void FindLight()
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
    }

    void FindGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, sprite.size.y + 0.1f, LayerMask.GetMask("Ground"));
        isGround = hit;
        if (!isGround)
        {
            transform.Translate(0, -Time.fixedDeltaTime, 0);
        }
        else
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
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
            player.ItemInHand = false;
        }
    }
}
