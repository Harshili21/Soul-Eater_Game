using System;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flightspeed = 2f;
    private float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public Collider2D deathCollider;
    public List<Transform> waypoints;
    

    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

    Transform nextWaypoint; 
    int waypointNum = 0;

    public bool _hasTarget = false;
    public bool HasTarget { get{return _hasTarget;} 
        private set{
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        } 
    }

    public bool CanMove{
        get{
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake(){
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
    }

    private void Start(){
        nextWaypoint = waypoints[waypointNum];
    }

 

    // Update is called once per frame
    private void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
        
    }

    private void FixedUpdate(){
        if(damageable.IsAlive){
            if(CanMove){
                Flight();
            }else{
                rb.linearVelocity = Vector3.zero;
            }
        }
    }

    private void Flight()
{
    // Ensure there are waypoints to follow
    if (waypoints == null || waypoints.Count == 0)
    {
        Debug.LogWarning("No waypoints assigned.");
        return;
    }

    // Get the current waypoint
    nextWaypoint = waypoints[waypointNum];

    // Calculate direction to the next waypoint
    Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

    // Calculate distance to the next waypoint
    float distance = Vector2.Distance(nextWaypoint.position, transform.position);

    // Move towards the next waypoint
    rb.linearVelocity = directionToWaypoint * flightspeed;
    UpdateDirection();

    // Check if the waypoint is reached
    if (distance <= waypointReachedDistance)
    {
        // Move to the next waypoint
        waypointNum++;

        // Loop back to the first waypoint if we've reached the end
        if (waypointNum >= waypoints.Count)
        {
            waypointNum = 0;
        }

        // Update the next waypoint
        nextWaypoint = waypoints[waypointNum];
    }
}

    private void UpdateDirection()
    {   
        Vector3 locScale = transform.localScale;
        if(transform.localScale.x > 0){
            //facing right
            if(rb.linearVelocityX < 0){
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }else{
            //facing left
            if(rb.linearVelocityX > 0){
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }

    void OnDeath(){
        //Dead flyer falls to ground
        rb.gravityScale = 1.2f;
        rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        deathCollider.enabled = true; 
    }
}
