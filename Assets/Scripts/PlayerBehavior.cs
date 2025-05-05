using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    Rigidbody2D rb;

    public float moveSpeed = 8f;

    public Animator animator;
    private bool isInDialogue = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        // Horizontal movement
        float inputX = Input.GetAxisRaw("Horizontal");

        if (inputX != 0 && !isInDialogue)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(inputX);
            transform.localScale = scale;
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (!isInDialogue)rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
    }


    public void setInDialogue()
    {
        isInDialogue = !isInDialogue;
    }
}