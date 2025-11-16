using System.Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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
    public float floatAmplitude = 0.01f;
    public float floatSpeed = 0.5f;

    [Header("Optional VFX")]
    public GameObject dispelVFX;

    private Vector3 startPos;
    private float floatTimer;

    public Rigidbody rb;
    public Vector3 velocity;
    bool isAtApex = false;
    public float upwardGravityReduction = 0.8f;
    public float gravityScale = 0.5f;
    private float spawnTime = 0.5f;
    private float spawnCountdown = 0f;
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearVelocity = new Vector3(0f, 3f, Random.Range(-3f, 3f));
    }

    void Update()
    {
        

        // Always face the camera (billboard)
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180f, 0); // flip to face properly
        }  
    }

    private void FixedUpdate()
    {
        spawnCountdown += Time.deltaTime;
        if (!isAtApex)
        {
            ApplyUpwardPhysics();

            // Only check for apex when velocity is very small AND moving downward or stationary
            if (spawnCountdown >= spawnTime)
            {
                HandleApexPhysics();
            }
        }
        // Float animation
        floatTimer += Time.deltaTime * floatSpeed;
        
        if (isAtApex) transform.position = transform.position + new Vector3(0f, Mathf.Sin(floatTimer) * 0.0025f, 0f);
    }

    private void ApplyUpwardPhysics()
    {
        Debug.Log("traveling up!");
        //float reductedGravity = gravityScale * upwardGravityReduction;
        //rb.AddForce(Physics.gravity * reductedGravity, ForceMode.Acceleration);
    }

    private void HandleApexPhysics()
    {
        Debug.Log("hit apex height, going to zero");
        rb.linearVelocity = Vector3.zero;
        isAtApex = true;
        //rb.useGravity = false;
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

    public void HandleDispell()
    {
        Debug.Log("Destroying myself !");
        Destroy(this.gameObject);
    }
}
