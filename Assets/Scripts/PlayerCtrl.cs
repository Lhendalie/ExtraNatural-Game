using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    public Animator animator;
    public Animator cageAnim;

    public float speed;
    public float jumpForce;
    public float checkRadius;

    private float moveInput;

    public int enemiesKilled = 0;
    public int extraJumpsValue;

    private int extraJumps;

    public bool facingRight = true;
    public bool gameWon = false;

    private bool isGrounded = true;

    public GameObject bullet;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel4;
    public GameObject panel5;
    public GameObject panel6;

    private GameControl gc;
    private EnemyCtrl ec;
    private GhostMovement gm;

    AudioSource winAudio;

    public Transform groundCheck;

    public LayerMask whatIsGround;

    private Rigidbody2D rb;

    void Start() {
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();

        gc = GameObject.FindGameObjectWithTag("GameControl").GetComponent<GameControl>();
        winAudio = GetComponent<AudioSource>();
    }

    void FixedUpdate() {

        //This line exists in the begining of all methods so that the game wont continue if the player is dead or if the game is won
        if (gc.playerDead || gameWon)
        {
            return;
        }

        //Kills the character if the health is zero
        if (HealthbarCtrl.health == 0)
        {
            gc.Die();
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        //Player movement code
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
	}

    void Update()
    {
        if (gc.playerDead || gameWon)
        {
            return;
        }

        animator.SetBool("IsJumping", !isGrounded);

        //Player jump control
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {            
            if (isGrounded)
            {
                extraJumps = extraJumpsValue;
                rb.velocity = Vector2.up * jumpForce;
            }
            else if (extraJumps > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                extraJumps--;
            }
        }

        //Player attack control
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullet, transform.position + (transform.forward * 2), transform.rotation);            
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (gc.playerDead || gameWon)
        {
            return;
        }

        // On collision with coins
        if (col.CompareTag("Collectable"))
        {
            GameObject.FindGameObjectWithTag("collectAudio").GetComponent<AudioSource>().Play();

            Destroy(col.gameObject);

            gc.points = gc.points + 1;
        }

        //On collision with a spike or an acid pool
        if (col.CompareTag("Enemy"))
        {
            if (!gc.playerDead)
            {
                gc.Die();
                return;
            }
        }        

        //On collison with a ghost enemy
        if (col.CompareTag("Ghost"))
        {
            GetComponent<AudioSource>().Play();
        }

        //On collision with a health potion
        if (col.CompareTag("potion"))
        {
            GameObject.FindGameObjectWithTag("collectAudio").GetComponent<AudioSource>().Play();

            if (HealthbarCtrl.health < 10)
            {
                HealthbarCtrl.health += 2;
            }
            Destroy(col.gameObject);
        }

        //On collision with life potions
        if (col.CompareTag("lifePot"))
        {
            GameObject.FindGameObjectWithTag("collectAudio").GetComponent<AudioSource>().Play();

            if (gc.lives < 3)
            {
                gc.lives += 1;
            }
            Destroy(col.gameObject);
        }
        
        //Storrytelling panels control
        if (col.CompareTag("Panel1"))
        {
            if (panel2.activeInHierarchy)
            {
                panel2.SetActive(false);
            }

            panel1.SetActive(true);
            StartCoroutine(LateCall(panel1));
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Panel2"))
        {
            if(panel1.activeInHierarchy)
            {
                panel1.SetActive(false);
            }
            panel2.SetActive(true);
            StartCoroutine(LateCall(panel2));
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Panel3"))
        {
            panel3.SetActive(true);
            StartCoroutine(LateCall(panel3));
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Panel4"))
        {
            if (panel3.activeInHierarchy)
            {
                panel3.SetActive(false);
            }
            panel4.SetActive(true);
            StartCoroutine(LateCall(panel4));
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Panel5"))
        {
            panel5.SetActive(true);
            StartCoroutine(LateCall(panel5));
            Destroy(col.gameObject);
        }

        if (col.CompareTag("Panel6"))
        {
            panel6.SetActive(true);
            StartCoroutine(LateCall(panel6));
            Destroy(col.gameObject);
        }

        //Setting a checkpoint for respawn
        if (col.CompareTag("checkPoint"))
        {
            gc.lastCheckPoint = col.gameObject;
            Animator anim = col.GetComponent<Animator>();
            anim.SetBool("IsChecked", true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (gc.playerDead || gameWon)
        {
            return;
        }

        if (col.CompareTag("Ghost"))
        {
            ec = col.GetComponent<EnemyCtrl>();
            ec.anim.SetBool("IsAttacking", false);
        }

        if (col.CompareTag("checkPoint"))
        {
            Animator anim = col.GetComponent<Animator>();
            anim.SetBool("IsChecked", false);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(gc.playerDead || gameWon)
        {
            return;
        }

        // Sets the ghost to attack the player on collision and takes life from the player
        if (col.CompareTag("Ghost"))
        {
            ec = col.GetComponent<EnemyCtrl>();
            gm = col.GetComponent<GhostMovement>();            

            ec.anim.SetBool("IsAttacking", true);

            if (HealthbarCtrl.health <= 0)
            {                
                HealthbarCtrl.health = 0;
            }
            else
            {
                if (gm.isAttacking)
                {
                    HealthbarCtrl.health = HealthbarCtrl.health - 1;                    
                }
            }           
        }

        //Checks if Sean Binchester can be unlocked and does it if that is so
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (col.CompareTag("Panel8"))
            {
                gameWon = true;
                cageAnim.SetBool("cageOpen", true);
                GameObject.FindGameObjectWithTag("cage").GetComponent<AudioSource>().Play();
                winAudio.Play();
                StartCoroutine(LateCall());
            }
        }
    }

    IEnumerator LateCall(GameObject gobject)
    {
        yield return new WaitForSeconds(7);

        gobject.SetActive(false);
    }

    IEnumerator LateCall()
    {
        yield return new WaitForSeconds(4);
        gc.EndGame();
    }
}
