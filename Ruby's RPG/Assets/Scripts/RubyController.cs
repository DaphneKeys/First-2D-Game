using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{

    public float speed = 3.0f;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f; 

    public int health { get { return currentHealth; } }
    int currentHealth;
    bool isInvincible; //= true;
    float invincibleTimer;
    public GameObject projectilePrefab;

   
    //public ParticleSystem hitEffect;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    private AudioSource audioSource; //Collectable
    private AudioSource audioSource2; //Ruby get hit sound
    private AudioSource audioSource3; //Ruby Throw Cog sound 
    public AudioClip collectedClip;
    public AudioClip CogClip;

    Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
        //Player hit sound
        audioSource2 = GetComponent<AudioSource>();
        //Cog throw sound
        audioSource3 = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        Vector2 position = rigidbody2d.position;

        position = position + move * speed * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        
        if (amount < 0)
        {

            if (isInvincible == true)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            //Instantiate(hitEffect, transform.position, Quaternion.identity);

            this.PlaySound(collectedClip); //play sound
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        //Debug.Log(currentHealth + "/" + maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);


    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        this.PlaySound(CogClip); //play sound of cog
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlaySound2(AudioClip clip)
    {
        audioSource2.PlayOneShot(clip);
    }

    public void PlaySound3(AudioClip clip)
   {
        audioSource3.PlayOneShot(clip);
    }


}
