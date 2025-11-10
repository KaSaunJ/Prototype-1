using UnityEngine;

public class AsteroidVisual : MonoBehaviour
{
    public Sprite[] asteroidSprites; // assign in inspector

    void Start()
    {
        // Random rotation
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        // Optional: random sprite variation
        var sr = GetComponent<SpriteRenderer>();
        if (asteroidSprites != null && asteroidSprites.Length > 0)
        {
            sr.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];
        }
    }
}
