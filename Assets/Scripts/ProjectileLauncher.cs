using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;

    public void FireProjectile() {
    // Instantiate the projectile
    GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);

    // Get the original scale of the projectile
    Vector3 origScale = projectile.transform.localScale;

    // Determine the direction of the scale flip based on the parent's scale
    float xScale = transform.localScale.x > 0 ? 1 : -1;

    // Apply the scale flip while preserving the rotation
    projectile.transform.localScale = new Vector3(
        origScale.x * xScale,
        origScale.y,
        origScale.z
    );

    // Adjust the rotation if the projectile is flipped
    if (xScale < 0) {
        // Flip the rotation by 180 degrees around the Z-axis
        projectile.transform.Rotate(0, 0, 270);
    }
}
}
