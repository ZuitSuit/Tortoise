using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tortoise : MonoBehaviour
{
    public Animator animator;

    public float speed;
    public bool isWalking, isHiding;

    public Rigidbody2D rb;
    private Vector2 yeetDirection, target;
    float yeetForce, yeetJuice, yeetJuiceMax, yeetJuiceRecharge;

    bool draggingTortoise = false;
    public SpriteRenderer dragPoint;

    public Image juiceIndicator;
    

    private void Awake()
    {
        if(rb == null) rb = GetComponentInChildren<Rigidbody2D>();
        yeetForce = 3000f;
        yeetJuice = 0f;
        yeetJuiceMax = 100f;
        yeetJuiceRecharge = 20f; // per second
    }

    void Update()
    {

        animator.SetFloat("speed", speed);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isHiding", isHiding);
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(target, Vector2.zero);
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
