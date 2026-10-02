using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public int healthRestore = 30;
    public Vector3 spinRotationSpeed = new Vector3(0,30,0);



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision){
        Damageable damageable = collision.GetComponent<Damageable>();

        if(damageable){
            bool wasHealed = damageable.Heal(healthRestore);
            if(wasHealed){
                Destroy(gameObject);
            }
        }
    }

    void Update(){
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}
