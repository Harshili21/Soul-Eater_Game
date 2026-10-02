using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionBox : MonoBehaviour
{
    public UnityEvent onPlayerDetected;
    public UnityEvent onPlayerLost;

    public List<Collider2D> detectedColliders = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            detectedColliders.Add(collision);
            onPlayerDetected.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            detectedColliders.Remove(collision);
            if (detectedColliders.Count == 0)
            {
                onPlayerLost.Invoke();
            }
        }
    }
}
