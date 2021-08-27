using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float verticalJumpForce;
    [SerializeField] private float horizontalJumpForce;

    [SerializeField] private Collider2D groundCollider;
    [SerializeField] private Collider2D wallCollider;
    [SerializeField] private Animator UIAnimator;
    [SerializeField] private TextMeshProUGUI resultTest;

    public LayerMask ground;
    public LayerMask wall;
    public LayerMask thorns;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private MovingPlatform platform;

    private bool isDead;

    void Start()
    {
        isDead = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        float hDir = Input.GetAxis("Horizontal");

        if(hDir < 0f)
        {
            rb.velocity = new Vector2(playerSpeed * -1f, rb.velocity.y);
            spriteRend.flipX = true;
            anim.SetBool("isRunning", true);
        }

        else if(hDir > 0f)
        {
            rb.velocity = new Vector2(playerSpeed, rb.velocity.y);
            spriteRend.flipX = false;
            anim.SetBool("isRunning", true);
        }

        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            anim.SetBool("isRunning", false);
        }

        if(Input.GetKeyDown(KeyCode.Space) && groundCollider.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, verticalJumpForce);
        }

        if(Input.GetKeyDown(KeyCode.Space) && wallCollider.IsTouchingLayers(wall) && !groundCollider.IsTouchingLayers(ground))
        {
            Debug.Log("wall " + Mathf.Sign(hDir));
            rb.AddForce(new Vector2(horizontalJumpForce * Mathf.Sign(hDir) * -1f, verticalJumpForce * 25f), ForceMode2D.Force);
        }

        if(groundCollider.IsTouchingLayers(thorns) && !isDead)
        {
            isDead = true;
            anim.SetBool("isDead", true);
            Destroy(this.gameObject, .5f);
            resultTest.text = "Game Over";
            UIAnimator.SetTrigger("Open");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Bonus"))
        {
            Destroy(other.gameObject);
            resultTest.text = "Win!";
            UIAnimator.SetTrigger("Open");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent<MovingPlatform>(out var collidedPlatform))
        {
            platform = collidedPlatform;
            this.transform.SetParent(collidedPlatform.transform);
        }
            
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent<MovingPlatform>(out var collidedPlatform))
        {
            platform = null;
            this.transform.SetParent(null);
        }
    }
}