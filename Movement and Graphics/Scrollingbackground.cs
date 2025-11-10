using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeedX = 0.02f;
    public float scrollSpeedY = 0.01f;

    private Material mat;
    private Transform cam;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // Duplicate the material so only this object scrolls
        mat = Instantiate(sr.material);
        sr.material = mat;

        // Get reference to main camera
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Keep background aligned with camera position (so it follows view)
        transform.position = new Vector3(cam.position.x, cam.position.y, cam.position.z + 10);

        // Scroll texture over time for drifting effect
        Vector2 offset = mat.mainTextureOffset;
        offset.x += scrollSpeedX * Time.deltaTime;
        offset.y += scrollSpeedY * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}
