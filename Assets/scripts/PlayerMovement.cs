using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D _rb;

    float horizontal;
    float vertical;

    [Header("Движение")]
    [Range(0f, 10f)]
    [SerializeField] float boost;
    [SerializeField] float speedDefault = 8;
    [SerializeField] float speedWithItem = 2;
    float speedPlayer;
    bool right = true;
    [SerializeField] bool isStay = false;

    [Header("Руки")]
    public Collider2D Obj;
    public bool ItemInHand = false;
    public RaycastHit2D hit;
    public Transform hand;
    // Start is called before the first frame update
    void Start()
    {
        speedPlayer = speedDefault;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetObject();
        CheckingGround();
        Jump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        MovePlayer();
    }

    void MovePlayer()
    {
        isStay = horizontal == 0 ? true : false;

        if (!isStay)
        {
            transform.Translate(horizontal * speedPlayer/10 * boost * Time.fixedDeltaTime, 0, 0);
            if (boost < 10f)
            {
                boost += Time.fixedDeltaTime * speedPlayer;
            }
        }
        else
        {
            boost = 0;
        }
        if (!ItemInHand)
        {
            if (horizontal > 0 && !right || horizontal < 0 && right)
            {
                Flip();
            }
        }  
    }

    void Flip()
    {
        right = !right;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void GetObject()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!ItemInHand)
            {
                Physics2D.queriesStartInColliders = false;
                hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 2f);
                if (hit.collider != null && hit.collider.tag == "Item" && hit.collider.GetComponent<Item>().isLight.Count > 0)
                {
                    ItemInHand = true;
                    speedPlayer = speedWithItem;
                }
            }
            else
            {
                DropItem();
            }
        }

        if (ItemInHand)
        {
            hit.collider.transform.position = new Vector2(hand.position.x, hit.collider.transform.position.y);
        }
    }

    public void DropItem()
    {
        ItemInHand = false;
        if (hit.collider.transform.GetComponent<Rigidbody2D>() != null)
        {
            hit.collider.transform.position = hit.collider.transform.position;
        }
        speedPlayer = speedDefault;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * 2f);
    }

    public float jumpForce = 7f;

    public bool onGround;
    public Transform GroundCheck;
    public float checkRadius = 0.4f;
    public LayerMask Ground;
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Physics2D.IgnoreLayerCollision(7, 8, true);
            Invoke("IgnorelayerOff", 2f);
        }
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }
    }

    void IgnorelayerOff()
    {
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
    }
}
