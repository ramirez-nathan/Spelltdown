using UnityEngine;

public class Rune : MonoBehaviour
{
    [Header("Type")]
    public RuneType type;

    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;
    public Sprite circleSprite;
    public Sprite triangleSprite;
    public Sprite zigzagSprite;

    [Header("Behavior")]
    public float baseDrainRate = 0.2f;
    public float floatAmplitude = 0.1f;
    public float floatSpeed = 2f;

    [Header("Optional VFX")]
    public GameObject dispelVFX;

    private Vector3 startPos;
    private float floatTimer;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Float animation
        floatTimer += Time.deltaTime * floatSpeed;
        transform.position = startPos + new Vector3(0f, Mathf.Sin(floatTimer) * floatAmplitude, 0f);

        // Always face the camera (billboard)
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180f, 0); // flip to face properly
        }
    }

    // Called by RuneManager when spawning a rune
    public void SetRuneType(RuneType runeType)
    {
        type = runeType;

        switch (type)
        {
            case RuneType.Circle:
                spriteRenderer.sprite = circleSprite;
                break;
            case RuneType.Triangle:
                spriteRenderer.sprite = triangleSprite;
                break;
            case RuneType.ZigZag:
                spriteRenderer.sprite = zigzagSprite;
                break;
        }
    }
}
