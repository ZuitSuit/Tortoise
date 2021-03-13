using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextShaker : MonoBehaviour
{
    Vector3 originalPosition, moveTo;
    float timer = 0f;
    void Start()
    {
        originalPosition = transform.position;
        moveTo = new Vector3(transform.position.x + 2f, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * 50f;
        transform.position = Vector3.Lerp(originalPosition, moveTo, Mathf.Sin(timer));
    }
}
