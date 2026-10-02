using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;  // Speed of enemy movement
    public Transform player;      // Reference to the player's position
    private bool isChasing = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isChasing && player != null)
        {
            // Move enemy toward the player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            // Stop moving if not chasing
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))  // Ensure player has the "Player" tag
        {
            isChasing = true;
            player = collision.transform;  // Set target to player
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = false; // Stop chasing when player leaves
            player = null;
        }
    }
}
