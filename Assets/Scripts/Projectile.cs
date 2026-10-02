using UnityEngine;

public class Projectile : MonoBehaviour
{   
    public Vector2 moveSpeed = new Vector2(3f,0);
    public int damage = 10;
    public Vector2 knockback = new Vector2(0,0);
    public float lifetime = 3f;
    
    Rigidbody2D rb;


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DestroyAfterLifetime());
        rb.linearVelocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);

    }

    public void OnTriggerEnter2D(Collider2D collision){
        Damageable damageable = collision.GetComponent<Damageable>();
        if(damageable != null){
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x,knockback.y);
            bool gotHit = damageable.Hit(damage, deliveredKnockback);
            if(gotHit){
                Debug.Log(collision.name + " hit for " + damage);
                Destroy(gameObject);
            }
        }
    }

    private System.Collections.IEnumerator DestroyAfterLifetime()
    {
        // Wait for the specified lifetime
        yield return new WaitForSeconds(lifetime);

        // Destroy the projectile if it still exists
        if (gameObject != null)
        {
            Destroy(gameObject);
            Debug.Log("Projectile destroyed after lifetime expired.");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
