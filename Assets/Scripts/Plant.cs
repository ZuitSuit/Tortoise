using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public ParticleSystem grassExplosion;
    public AudioSource audioSource;
    public AudioClip clip;
    public BoxCollider2D boxCollider;
    public float yeetJuice;

    public void Eat()
    {
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
        audioSource.PlayOneShot(clip);
        grassExplosion.Play();
    }
}
