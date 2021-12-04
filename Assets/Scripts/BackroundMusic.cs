using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackroundMusic : MonoBehaviour
{

    private RubyController rubyController;
    public AudioClip WinClip;
    public AudioClip LoseClip;

    public AudioClip BackroundClip;

    AudioSource audioSource;

    public bool endValue;
    

    
    

    // Update is called once per frame
    void Update()
    {
        GameObject rubyControllerObject = GameObject.FindWithTag("rubyController");

        if(rubyController)
        {
            if(rubyController.end == true)
            {
                audioSource.PlayOneShot(LoseClip, 0.5f);
            }
        }
       /* if (rubyControllerObject != null)
        {
            rubyController = rubyControllerObject.GetComponent<RubyController>();
        }
        endValue = rubyController.GetComponent<end>();

        
            if (controller.endValue == true )
            {
               
                controller.PlaySound(LoseClip);
            }
        */
    }
    
}
