using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastEnemyController : MonoBehaviour
{
    public float speed;
    public float timeToChange;
    public bool horizontal;
 
    public ParticleSystem smoke;
 
    
    Rigidbody2D rigidbody2d;
    float remainingTimeToChange;
    Vector2 direction = Vector2.right;
    bool repaired = false;
    
    Animator animator;

    AudioSource audioSource;
    public AudioClip fixedAudio;
    
    void Start ()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        remainingTimeToChange = timeToChange;
 
        direction = horizontal ? Vector2.right : Vector2.down;
 
        animator = GetComponent<Animator>();
 
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if(repaired)
            return;
        
        remainingTimeToChange -= Time.deltaTime;
 
        if (remainingTimeToChange <= 0)
        {
            remainingTimeToChange += timeToChange;
            direction *= -1;
        }
 
        animator.SetFloat("Move X", direction.x);
        animator.SetFloat("Move Y", direction.y);
    }
 
    void FixedUpdate()
    {
        rigidbody2d.MovePosition(rigidbody2d.position + direction * speed * Time.deltaTime);
    }
 
    void OnCollisionStay2D(Collision2D other)
    {
        if(repaired)
        return;
           
        
        RubyController controller = other.collider.GetComponent<RubyController>();
        
        if(controller != null)
        {
            controller.ChangeScore();
        }
    }
 
    public void Fix()
    {
        animator.SetTrigger("Fixed");
        repaired = true;
        
        
        smoke.Stop(); 
        
        rigidbody2d.simulated = false;
        
        audioSource.Stop();
        audioSource.PlayOneShot(fixedAudio);

    }
}
