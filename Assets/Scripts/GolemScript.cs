using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemScript : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    public float stoppingDistance;
    public float retreatDistance;
    public Transform player;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public GameObject projectile;

    private RubyController rubyController;

    
    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;
    
    Animator animator;


   
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();

        timeBtwShots = startTimeBtwShots;

        GameObject rubyControllerObject = GameObject.FindWithTag("rubyController");

        if (rubyControllerObject != null)
        {
            rubyController = rubyControllerObject.GetComponent<RubyController>();
        }

 
    }

    void Update()
    {
       
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
        
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

         GameObject rubyControllerObject = GameObject.FindWithTag("RubyController"); //this line of code finds the RubyController script by looking for a "RubyController" tag on Ruby

        if (rubyControllerObject != null)

        {

            rubyController = rubyControllerObject.GetComponent<RubyController>(); //and this line of code finds the rubyController and then stores it in a variable

            print ("Found the RubyConroller Script!");

        }

        if (rubyController == null)

        {

            print ("Cannot find GameController Script!");

        }

        if(Vector2.Distance(transform.position, player.position) > stoppingDistance){
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        } else if(Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance){
           
            transform.position = this.transform.position;

        } else if(Vector2.Distance(transform.position, player.position) < retreatDistance){
           
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);

        }
        //transform.LookAt (player.position, Vector3.back);
     

        Vector2 dir= player.transform.position - transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
        animator.SetFloat("Move X", dir.x);
        animator.SetFloat("Move Y", dir.y);
        /*if (dir.x < 0)
        {
            animator.SetFloat("Move X", dir.x);
            animator.SetFloat("Move Y", 0);
        }    
        if (dir.x < 0)
        {
            animator.SetFloat("Move X", dir.x);
            animator.SetFloat("Move Y", 0);
        }*/
 
        Debug.Log(dir.x);

        if(timeBtwShots <= 0){

            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;

        } else {

            timeBtwShots -= Time.deltaTime;

        }

    }
    
    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
        
        Vector2 position = rigidbody2D.position;
        /*
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        
        rigidbody2D.MovePosition(position);
        */
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    
    
    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        
        broken = false;
        rigidbody2D.simulated = false;
        //optional if you added the fixed animation
        animator.SetTrigger("Fixed");

   
       
        
        if (rubyController != null)
        {
            rubyController = rubyController.GetComponent<RubyController>();
            rubyController.ChangeScore(1);

        }
        
    }
  
}