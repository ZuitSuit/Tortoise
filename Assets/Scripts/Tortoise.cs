using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tortoise : MonoBehaviour
{
    public Animator animator;

    public bool isWalking, isHiding;
    bool wasHiding = false;
    bool isGrounded;
    float groundedTime;

    public Rigidbody2D rb;
    private Vector2 yeetDirection, target;
    float yeetForce, yeetJuice, yeetJuiceMax, yeetJuiceRecharge;

    float speed, maxSpeed;

    bool draggingTortoise = false;
    public SpriteRenderer dragPoint;
    public Transform groundCheck;
    public ParticleSystem dustTrail, dustExplosion;
    ParticleSystem.EmissionModule trailEmission;

    public Image juiceIndicator, speedIndicator;

    public Color maxGrassColor, noGrassColor, maxSpeedColor, noSpeedColor;

    
    LayerMask groundLayer;
    int groundLayerInt;
    RaycastHit2D hit;

    public AudioSource music;
    public bool active { get; set; }

    private void Awake()
    {
        active = false;
        if (rb == null) rb = GetComponentInChildren<Rigidbody2D>();
        yeetForce = 350f;
        yeetJuice = speed = 0f;
        yeetJuiceMax = maxSpeed = 100f;
        yeetJuiceRecharge = 6f; // per second
        groundLayer = LayerMask.GetMask("Ground");
        groundLayerInt = LayerMask.NameToLayer("Ground");

        groundedTime = 0f;
        trailEmission = dustTrail.emission;
    }

    void Update()
    {
        music.volume = Mathf.Lerp(.1f, .5f, Mathf.Clamp01(rb.velocity.x / 10f));
        //spam left right to move
        /*        axis = Input.GetButtonDown("Horizontal") ? Mathf.Sign(Input.GetAxis("Horizontal")) : 0;
                if (!isHiding && (axis != 0) && (axis != lastAxis))
                {
                    lastAxis = axis;
                    speed = Mathf.Clamp(speed + 5f, 0f, 100f);
                    Debug.Log(speed);
                }*/

        speed = Mathf.Clamp(speed - Time.deltaTime * (4f + 20f * speed/maxSpeed), 0f, maxSpeed);

        //spam space to move
        if (!isHiding)
        {
            if (Input.GetKeyDown(KeyCode.Space) && active)
            {
                speed = Mathf.Clamp(speed + 5f, 0f, maxSpeed);
            }
        }

        hit = Physics2D.Raycast(groundCheck.position, -transform.up, 0.6f, groundLayer);
        isGrounded = (hit.collider != null) && (hit.collider.gameObject.layer == groundLayerInt);

        groundedTime = (isGrounded ? groundedTime + Time.deltaTime: 0f);

        isHiding = !(isGrounded && (groundedTime > 1f));
        isWalking = speed > 0;

        if (isHiding != wasHiding)
        {
            //reset speed when hiding/unhiding
            speed = 0f;
            trailEmission.rateOverDistance = isHiding ? 0f : 20f;
        }

        animator.SetFloat("speed", Mathf.Lerp(0f, 5f, (speed / maxSpeed))); //animation speed based on speed
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isHiding", isHiding);
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            hit = Physics2D.Raycast(target, Vector2.zero);
            if (hit.collider != null && hit.collider.attachedRigidbody == rb)
            {
                ToggleDragging(true);
                dragPoint.transform.position = hit.point;
            }
        }

        if (yeetJuice < 1f || Input.GetMouseButtonUp(0))
        {
            ToggleDragging(false);
        }

        wasHiding = isHiding;

        juiceIndicator.fillAmount = (yeetJuice / yeetJuiceMax);
        juiceIndicator.color = Color.Lerp(noGrassColor, maxGrassColor, (yeetJuice / yeetJuiceMax));
        speedIndicator.fillAmount = (speed / maxSpeed);
        speedIndicator.color = Color.Lerp(noSpeedColor, maxSpeedColor, (speed / maxSpeed));

    }

    private void FixedUpdate()
    {
        if (draggingTortoise)
        {
            yeetJuice = Mathf.Clamp(yeetJuice - Time.deltaTime * 100f, 0f, yeetJuiceMax); //drain juice

            yeetDirection = (target - (Vector2)transform.position).normalized;
            rb.AddForceAtPosition(yeetDirection * Time.deltaTime * yeetForce * Mathf.Clamp(1f,4f,Vector2.Distance(transform.position, target)), dragPoint.transform.position);
            //rb.AddForce(yeetDirection * Time.deltaTime * yeetForce * Mathf.Clamp(0f, 4f, Vector2.Distance(transform.position, target)));

        }
        else
        {
            yeetJuice = Mathf.Clamp(yeetJuice += Time.deltaTime * yeetJuiceRecharge, 0f, yeetJuiceMax);
        }

        if (!isHiding)
        {
            rb.AddForceAtPosition(groundCheck.right * speed/5f, groundCheck.position);
        }
    }

    public void ToggleDragging(bool toggle)
    {
        draggingTortoise = toggle;
        dragPoint.gameObject.SetActive(toggle);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Plant plant = collision.gameObject.GetComponent<Plant>();
        if (plant != null)
        {
            plant.Eat();
            yeetJuice = Mathf.Clamp((yeetJuice + plant.yeetJuice), 0f, yeetJuiceMax);
        }
    }

    public void SetSpeed(float sp)
    {
        speed = Mathf.Clamp(sp, 0f, maxSpeed);
    }

    public void SetYeetJuice(float yj)
    {
        yeetJuice = Mathf.Clamp(yj, 0f, yeetJuiceMax);
    }
}
