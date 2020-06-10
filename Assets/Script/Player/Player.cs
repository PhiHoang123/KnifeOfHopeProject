using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //about character
    [Header("Player")]
    private float moveInput;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] int attackDamage;
    [SerializeField] float fallSpeed;
    [SerializeField] int health;
    private float extraJumps; //for double jump
    [SerializeField] float extraJumpsValue;
    [SerializeField] float dashSpeed;
    [SerializeField] float startDashTime;
    private float dashTime;
    private int direction;
    [Space(5)]

    //component
    private Animator anim;
    private Rigidbody2D rb;

    //check ground
    [Header("GroundCheck")]
    public bool isGrounded;
    public Transform feetpos;
    public float checkRadius;
    public LayerMask whatisGround;
    private float fJumpPressedRemember = 0;
    private float fJumpPressedRememberTime = 0.2f;
    private float fGroundedRemember = 0;
    private float fGroundedRememberTime = 0.2f;
    [Space(5)]

    //check Attack Range
    [Header("Attacking")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    [Space(5)]

    //knockback if collide with enemy
    [Header("Knockback")]
    public float knockback;
    public float knockbackCount; // count time of knockback
    public float knockbackLength;// the length time of knockback
    public bool knockbackFromRight;
    public bool isKnockBack; // check if player is knockback. 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dashTime = startDashTime;
    }

    // Update is called once per frame
    void Update()
    {
        //check if the character is grounded.
        isGrounded = Physics2D.OverlapCircle(feetpos.position, checkRadius, whatisGround);

        //get Input key to move and set animation. 
        getInputKey();

        //jump
        Jump();

        //attack
        if (timeBtwAttack <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                timeBtwAttack = startTimeBtwAttack;
                Attack();
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }

        //knockback
        KnockBack();

        //Dash
       


    }

    private void FixedUpdate()
    {
        if (isKnockBack == false)
        {
            moveCharacter();
        }

        Dash();



    }

    public void moveCharacter()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    //get Input Key
    void getInputKey()
    {
        moveInput = Input.GetAxis("Horizontal");
        if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            anim.SetBool("run", true);
        }
        else if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            anim.SetBool("run", true);
        }

        if (moveInput == 0)
        {
            anim.SetBool("run", false);
        }
    }

    void Dash()
    {
        if(direction == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if(moveInput > 0)
                {
                    direction = 1;
                }else if(moveInput < 0)
                {
                    direction = 2;
                }
            }
        }
        else
        {
            if(dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;
                if(direction == 1)
                {
                    rb.velocity = Vector2.right * dashSpeed;
                }else if(direction == 2)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                }
            }
        }
    }

    //Player Jump
    void Jump()
    {
        //if player is grounded , fGroundRemember will be subtracted to fix if the player leave the ground , he can't jump.
        fGroundedRemember -= Time.deltaTime;
        if(isGrounded == true)
        {
            fGroundedRemember = fGroundedRememberTime;
            extraJumps = extraJumpsValue;
        }

        fJumpPressedRemember -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fJumpPressedRemember = fJumpPressedRememberTime;

        }

        if((fJumpPressedRemember > 0) && (fGroundedRemember > 0) && extraJumps == 0)
        {
            fJumpPressedRemember = 0;
            fGroundedRemember = 0;
            anim.SetBool("isJumping", true);
            rb.velocity = Vector2.up * jumpForce;
        }

        //double jump
        if (fJumpPressedRemember > 0 && extraJumps > 0)
        {
            fJumpPressedRemember = 0;
            extraJumps--;
            rb.velocity = Vector2.up * jumpForce;
        }


        if (rb.velocity.y == 0)
        {
            anim.SetBool("isJumping", false);
        }

        //limit player fall
        if (rb.velocity.y < -Mathf.Abs(fallSpeed))
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -Mathf.Abs(fallSpeed), Mathf.Infinity));
        }
    }

    //Attack 
    void Attack()
    {
        anim.SetTrigger("attack");
        Collider2D [] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        //return an array which contains all of the enemy hit.
        for(int i = 0; i < hitEnemies.Length; i++)
        {
            //attack Scorpio
            if(hitEnemies[i].GetComponent<Scorpio>() != null)
            {
                hitEnemies[i].GetComponent<Scorpio>().TakeDamage(attackDamage);
            }

            //attack Mummy
            if (hitEnemies[i].GetComponent<Enemy>() != null)
            {
                hitEnemies[i].GetComponent<Enemy>().TakeDamage(attackDamage);
            }

        }
    }

    //knockback when collide with enemy.
    public void KnockBack()
    {
        if(knockbackCount <= 0)
        {
            isKnockBack = false; // when knockback is false , player will be allowed to move.
        }
        else
        {
            if (knockbackFromRight)
            {
                rb.velocity = new Vector2(-knockback, 4f); // if knockback from right is true, it will knock back left.
                isKnockBack = true; // check isKnockBack to knockback X axis, because the movement of player will effect the knockback.
            }
            if (!knockbackFromRight)
            {
                rb.velocity = new Vector2(knockback, 4f); // if knockback from right is false, it will knock back right.
                isKnockBack = true;
            }
            knockbackCount -= Time.deltaTime;
        }
    }

    //draw circle for we to see in scene view
    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    //take damage if enemy attack. just example, will update later :)))
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("hitBox"))
        {
            health--;
            Debug.Log("Take Damage");

            if(health < 0)
            {
                Debug.Log("Die");
                health = 0;
            }
        }
    }
}
