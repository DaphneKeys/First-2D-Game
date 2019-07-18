using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;
    public ParticleSystem smokeEffect; 

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;

    bool broken = true;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();

    }

    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        //Destroy(smokeEffect.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            //Debug.Log(timer + "timer current when < 0");
            direction = -direction;
            timer = changeTime;
            //Debug.Log(timer + "timer after timer set to value of changeTime which is 3.0f");
            //Debug.Log(changeTime + "value of changeTime");
        }


        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            animator.SetFloat("Move.X", 0);
            animator.SetFloat("Move.Y", direction);
            position.y = position.y + Time.deltaTime * speed * direction;
            
        }
        else
        {
            animator.SetFloat("Move.X", direction);
            animator.SetFloat("Move.Y", 0);
            position.x = position.x + Time.deltaTime * speed * direction;
            
        }

        rigidbody2D.MovePosition(position);


       
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }

    }


}
