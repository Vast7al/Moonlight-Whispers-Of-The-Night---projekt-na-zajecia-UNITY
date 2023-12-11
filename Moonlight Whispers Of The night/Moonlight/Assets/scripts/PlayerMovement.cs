using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private new Animator animation;
    private SpriteRenderer flipCharacter;
    private BoxCollider2D collision;
    private float directionX = 0f;
    private float moveSpeed = 8f;
    private float jumpForce = 20f;
    public LayerMask jumpableGround;
    private float resetPositionX = 2f;
    private float resetPositionY = 2f;

    private enum MovementState { idle, running, jumping, falling }
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource winSoundEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flipCharacter = GetComponent<SpriteRenderer>();
        collision = GetComponent<BoxCollider2D>();
        animation = GetComponent<Animator>();
    }

    private void Update()
    {
        directionX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(directionX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationUpdate();
    }

    private void UpdateAnimationUpdate()
    {
        MovementState state;

        if (directionX > 0f)
        {
            state = MovementState.running;
            flipCharacter.flipX = false;
        }
        else if (directionX < 0f)
        {
            state = MovementState.running;
            flipCharacter.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        animation.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(collision.bounds.center, collision.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EndTrigger"))
        {
            winSoundEffect.Play();
            SceneManager.LoadScene("End screen"); 
        }
        else if (other.CompareTag("OutOfBounds"))
        {
            ResetPlayerPosition();
        }
    }

    private void ResetPlayerPosition()
    {

        transform.position = new Vector2(resetPositionX, resetPositionY);
    }
}