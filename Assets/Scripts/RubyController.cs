using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public static int level = 1;
    public float speed = 3.0f;
    
    public int maxHealth = 5;
    
    public GameObject projectilePrefab;
    
    public bool end = false;
    
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip backroundMusic;
    public AudioClip coin;
    
    public int health { get { return currentHealth; }}
    private int score = 0;
    private int coins = 0;
    private int currentScore; 
    public Text scoreText;
    public Text endText;
    public Text cogText;
    public Text coinText;
    int currentHealth;
    int cogs = 4;
    
    
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    
    AudioSource audioSource;
    public GameObject DamageParticlePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;
        cogText.text = " X " + cogs.ToString ();
        scoreText.text = "Number of Robots Fixed: " +  score.ToString ();
        audioSource = GetComponent<AudioSource>();
        endText.text = "  ";
        
        
        audioSource.clip = backroundMusic;
        audioSource.Play();
        audioSource.loop = true;
        coinText.text = " Collect 4 coins to access the secret level! ";

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(cogs > 0)
            {
                Launch();
                cogs = cogs - 1;
                cogText.text = " X " + cogs.ToString ();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if(end == true)
                {
                    SceneManager.LoadScene("Level 2");
                    level = 2;

                }
                else if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
        scoreText.text = "Number of Robots Fixed: " +  score.ToString ();

        if (Input.GetKey(KeyCode.R))
        {
            if (end == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if (coins == 4)
            coinText.text = " Follow the path to the upper right to access the secret level! "; 
        
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ammo"))
        {
            other.gameObject.SetActive(false);
            cogs = cogs + 1;
            cogText.text = " X " + cogs.ToString ();
        }
        if (other.gameObject.CompareTag("End"))
        {
            endText.text ="You Won The Secret Level! Game created by Andrew Sisk! Press R to Restart!";
            end = true;
            audioSource.clip = winSound;
            audioSource.Play();
            audioSource.loop = false;
        }
        if (other.gameObject.CompareTag("Level3"))
        {
            if(coins == 4)
            {
            SceneManager.LoadScene("Level 3");
            level = 3;
            }
        }
        
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            coins = coins + 1;
            Debug.Log(coins);
            PlaySound(coin);
        }
        
    }
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            GameObject DamageParticleObject = Instantiate(DamageParticlePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            PlaySound(hitSound);
        }
        if (currentHealth == 0)
        {
            endText.text = "You lose! Press R to restart!"; 
            audioSource.clip = loseSound;
            audioSource.Play();
            end = true;
            audioSource.loop = false;
            speed = 0;
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
       
    }
    public void ChangeScore(int CurrentScore)
    {
        score +=1;
        scoreText.text = "Number of Robots Fixed: " +  score.ToString ();
        if(score == 4&&level == 2)
        {
            endText.text ="You Win! Game created by Andrew Sisk! Press R to Restart!";
            end = true;
            audioSource.clip = winSound;
            audioSource.Play();
            audioSource.loop = false;

        }
        /*if(score == 4&&level == 3)
        {
            endText.text ="You Won The Secret Level! Game created by Andrew Sisk! Press R to Restart!";
            end = true;
            audioSource.clip = winSound;
            audioSource.Play();
            audioSource.loop = false;

        }*/
        else if(score == 4)
        {
            endText.text ="Talk To Jambi to visit stage two!";
            end = true;
        }
    }
    
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        
        PlaySound(throwSound);
    } 
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
   
}
