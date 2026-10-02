using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    //Starting position for the parallax game object
    Vector2 startingPosition;

    //Start Z value of the parallax game object
    float startingZ;
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float clippingPlane => Mathf.Max(cam.farClipPlane - cam.nearClipPlane, 0.1f);
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget)/clippingPlane;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cameraMovement = camMoveSinceStart; // Cache to avoid recalculating
        Vector2 newPosition = startingPosition + cameraMovement * parallaxFactor;
        
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }

}
