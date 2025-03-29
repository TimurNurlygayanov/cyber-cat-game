﻿using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class CatController : MonoBehaviour
{
    public float jumpForce = 5.5f;
    public float moveSpeed = 5f;
    public float fallThreshold = -10f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    public AudioClip jumpSound;

    private bool isFrozen = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isFrozen) return;

        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Телепорт по краям экрана
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x < 0f)
            transform.position = new Vector3(Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x, transform.position.y, transform.position.z);
        else if (viewPos.x > 1f)
            transform.position = new Vector3(Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x, transform.position.y, transform.position.z);

        // Разворот
        if (moveInput > 0.01f)
            spriteRenderer.flipX = false;
        else if (moveInput < -0.01f)
            spriteRenderer.flipX = true;

        // Автопрыжок при касании платформ
        // Реализован в OnCollisionEnter2D

        // Перезапуск при падении
        if (transform.position.y < fallThreshold)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFrozen) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

                if (jumpSound != null && audioSource != null)
                    audioSource.PlayOneShot(jumpSound);

                break;
            }
        }
    }

    public void FreezeCat()
    {
        isFrozen = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
    }
}
