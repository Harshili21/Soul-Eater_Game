using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int damage = 10; // Damage per touch
    public Vector2 knockback = new Vector2(0, 5f); // Optional knockback
    public float damageCooldown = 1.5f; // Time before dealing damage again

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            bool gotHit = damageable.Hit(damage, knockback);
            if (gotHit)
            {
                Debug.Log(collision.name + " stepped on spikes! Took " + damage + " damage.");
                StartCoroutine(ApplyContinuousDamage(damageable));
            }
        }
    }

    private System.Collections.IEnumerator ApplyContinuousDamage(Damageable damageable)
    {
        while (damageable != null && damageable.Health > 0 && damageable.gameObject.CompareTag("Player"))
        {
            yield return new WaitForSeconds(damageCooldown);
            damageable.Hit(damage, knockback);
        }
    }
}
