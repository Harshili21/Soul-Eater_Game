using UnityEngine;

public class Gem : MonoBehaviour
{
    public int GemValue = 1; // Points per coin

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Detect Player Collision
        {
            ScoreManager.instance.AddScore(GemValue); // Add score
            Destroy(gameObject); // Remove coin
        }
    }
}
