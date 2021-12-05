using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Blast : MonoBehaviour
{

    public float speed;
    //public AudioClip lazer;
    private Transform player;
    private Vector2 target;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        Debug.Log(player);
        
        target = new Vector2(player.position.x, player.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed* Time.deltaTime);

        if(transform.position.x == target.x && transform.position.y == target.y){
            DestroyProjectile();
            
        }


    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            DestroyProjectile();
        }
        {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
    }
    void DestroyProjectile(){
      
        Destroy(gameObject);
       // controller.PlaySound(lazer);
    }

}
