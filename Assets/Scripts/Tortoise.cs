using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tortoise : MonoBehaviour
{
    public Animator animator;

    public float speed;
    public bool isWalking, isHiding;
    bool isGrounded;
    float groundedTime;

    public Rigidbody2D rb;
    private Vector2 yeetDirection, target;
    float yeetForce, yeetJuice, yeetJuiceMax, yeetJuiceRecharge;

    bool draggingTortoise = false;
    public SpriteRenderer dragPoint;
    public Transform groundCheck;
    public Image juiceIndicator;


    
    LayerMask groundLayer, tortoiseLayer;
    int groundLayerInt;
    RaycastHit2D hit;

    private void Awake()
    {
        if(rb == null) rb = GetComponentInChildren<Rigidbody2D>();
        yeetForce = 3000f;
        yeetJuice = 0f;
        yeetJuiceMax = 100f;
        yeetJuiceRecharge = 20f; // per second
        groundLayer = LayerMask.GetMask("Ground");
        groundLayerInt = LayerMask.NameToLayer("Ground");
        tortoiseLayer = gameObject.layer;

        groundedTime = 0f;

    }

    void Update()
    {
        hit = Physics2D.Raycast(groundCheck.position, -transform.up, 0.3f, groundLayer);

        //Debug.DrawRay(groundCheck.position, -transform.up, Color.red, 0.1f);

        isGrounded = (hit.collider != null) && (hit.collider.gameObject.layer == groundLayerInt);

        groundedTime = (isGrounded ? groundedTime + Time.deltaTime: 0f);
        isHiding = !(isGrounded && (groundedTime > 2f));

        animator.SetFloat("speed", speed);
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

        if (draggingTortoise)
        {
            yeetJuice = Mathf.Clamp(yeetJuice - Time.deltaTime * 200f, 0f, 100f); //drain juice

            yeetDirection = (target - (Vector2)transform.position).normalized;
            rb.AddForceAtPosition(yeetDirection * Time.deltaTime * yeetForce, dragPoint.transform.position);

        }
        else
        {
            yeetJuice = Mathf.Clamp(yeetJuice += Time.deltaTime * yeetJuiceRecharge, 0f, 100f);
        }

        juiceIndicator.fillAmount = yeetJuice / yeetJuiceMax;
    }

    public void ToggleDragging(bool toggle)
    {
        draggingTortoise = toggle;
        dragPoint.gameObject.SetActive(toggle);
    }


}
