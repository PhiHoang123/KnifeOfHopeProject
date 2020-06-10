using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 5;
    private float currentHealth;
    public Transform target;
    public float distance;
    public float moveSpeed;

    public Animator anim;
    private Rigidbody2D rb;
    public bool facingRight = true;
    public bool isGrounded;
    private bool chasePlayer;
    public Transform groundPos;
    

    [SerializeField] float distanceToPlayer;
    [SerializeField] float distanceToAttack = 2f;
    [SerializeField] bool isAttacking;
    [SerializeField] float groundPosRadius;
    [SerializeField] LayerMask whatIsGround;

    [SerializeField] float thurst;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SeePlayer();
        distanceToPlayer = Vector2.Distance(transform.position, target.position);
        Attack();
    }

    //take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        anim.SetTrigger("hurt");

        if(currentHealth == 0)
        {
            Died();
        }

        //if take damage, flip to chase player or do sth...
        if (facingRight)
        {
            Flip();
        }
        else
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        Patrol();
        Chase();
    }

    void Died()
    {
        Debug.Log("Enemy Die!");

        anim.SetBool("isDead", true);

        GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;

        Destroy(transform.gameObject, 3);
        Stop();
    }

    void SeePlayer()
    {
        //draw a raycast. 
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, -transform.right, distance, 1<<LayerMask.NameToLayer("Player"));
        if(raycast.collider != null)
        {
            Debug.DrawLine(transform.position, raycast.point, Color.red);
            chasePlayer = true;
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position - transform.right * distance, Color.green);
            chasePlayer = false;
        }

        //check attack. if distance from enemy to player less than distancetoAttack of enemy. Enemy will attack
        if (distanceToPlayer > distanceToAttack)
        {
            isAttacking = false;
        }
        else if (distanceToPlayer < distanceToAttack)
        {
            isAttacking = true;
        }

    }

    //enemy patrol
    void Patrol()
    {
        rb.velocity = Vector2.right * moveSpeed * Time.fixedDeltaTime;
        anim.SetBool("isWalking", true);

        isGrounded = Physics2D.OverlapCircle(groundPos.position, groundPosRadius, whatIsGround);
        if(!isGrounded && facingRight)
        {
            Flip();
        }
        else if(!isGrounded && !facingRight)
        {
            Flip();
        }     
    }

    //enemy chase player when they see player
    void Chase()
    {
        if(chasePlayer == true && facingRight)
        {
            moveSpeed = 150f;
        }else if(chasePlayer == true && !facingRight)
        {
            moveSpeed = -150f;
        }
        else if(chasePlayer == false && facingRight)
        {
            moveSpeed = 80f;
        }
        else if (chasePlayer == false && !facingRight)
        {
            moveSpeed = -80f;
        }
    }

    //enemy stop when die
    void Stop()
    {
        rb.velocity = Vector2.right * 0 * Time.fixedDeltaTime;
        anim.SetBool("isWalking", false);
    }

    //enemy attack
    void Attack()
    {     
       if(isAttacking == true)
        {
            anim.SetBool("isAttacking", true);

        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        moveSpeed = -moveSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (groundPos == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(groundPos.position, groundPosRadius);
    }

    //if collide with player then flip 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (facingRight && transform.position.x > target.position.x)
            {
                Flip();
            }
            else if(!facingRight && transform.position.x < target.position.x)
            {
                Flip();
            }
        }
    }

}
