using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

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
    public Animator animator;

    private bool isFrozen = false;
    private bool isInvulnerable = false;
    private bool isKnockbacked = false;

    private HashSet<Transform> scoredPlatforms = new HashSet<Transform>();

    public float knockbackForce = 5f;
    public AudioClip damageSound;
    public float invulnerabilityTime = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isFrozen || isKnockbacked) return;

        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x < 0f)
            transform.position = new Vector3(Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x, transform.position.y, transform.position.z);
        else if (viewPos.x > 1f)
            transform.position = new Vector3(Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x, transform.position.y, transform.position.z);

        if (moveInput > 0.01f)
            spriteRenderer.flipX = false;
        else if (moveInput < -0.01f)
            spriteRenderer.flipX = true;

        if (animator != null)
        {
            animator.SetFloat("HorizontalMove", Mathf.Abs(rb.linearVelocity.x));
            animator.SetBool("Jumping", rb.linearVelocity.y > 1f);
        }

        if (transform.position.y < fallThreshold)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFrozen) return;

        if (collision.collider.CompareTag("Damage") && !isInvulnerable)
        {
            StartCoroutine(HandleDamage(collision));
            return;
        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

                if (jumpSound && audioSource)
                    audioSource.PlayOneShot(jumpSound);

                Transform platform = contact.collider.transform;

                if (!scoredPlatforms.Contains(platform))
                {
                    scoredPlatforms.Add(platform);
                    GameManager.Instance.AddScore(1);
                }

                break;
            }
        }
    }

    private IEnumerator HandleDamage(Collision2D collision)
    {
        isInvulnerable = true;
        isKnockbacked = true;

        GameManager.Instance.LoseLife();

        if (damageSound && audioSource)
            audioSource.PlayOneShot(damageSound);

        Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
        rb.linearVelocity = new Vector2(knockbackDir.x * knockbackForce, knockbackForce);

        StartCoroutine(DamageFlash());

        yield return new WaitForSeconds(invulnerabilityTime);

        isInvulnerable = false;
        isKnockbacked = false;
    }

    private IEnumerator DamageFlash()
    {
        float flashDuration = 0.1f;
        int flashes = 5;

        for (int i = 0; i < flashes; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    public void FreezeCat()
    {
        if (isFrozen) return;

        isFrozen = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = false;

        foreach (var col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }

        StartCoroutine(SinkAndFadeOut());
    }

    private IEnumerator SinkAndFadeOut()
    {
        float duration = 1.5f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.down * 1.5f;
        Color startColor = spriteRenderer.color;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;

            transform.position = Vector3.Lerp(startPos, endPos, t);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);

            yield return null;
        }

        spriteRenderer.enabled = false;
    }

    public void DieGracefully()
    {
        if (isFrozen) return;

        isFrozen = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = false;

        var collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;

        spriteRenderer.color = Color.red;
        StartCoroutine(SinkAndFadeOut());
    }
}
