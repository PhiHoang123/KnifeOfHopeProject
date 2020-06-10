using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorpio : MonoBehaviour
{
    [Header("Scorpio")]
    [SerializeField] float moveSpeed;
    private Rigidbody2D rb;
    [SerializeField] int maxHealth;
    private int currentHealth;

    //check if the grounded not collide with anything, it will turn.
    [Header("GroundCheck")]
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundPos;
    [SerializeField] float groundPosRadius;
    [SerializeField] LayerMask layerMarks;
    private bool faceRight = true;
    private Player player;
    [Space(5)]

    //knockbackForce
    [Header("Knockback")]
    public float knockbackForce;
    private bool isKnockback;
    private float knockbackCount;
    private float knockbackLength = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if(isKnockback == false)
        {
            Patrol();
        }

        if(isKnockback == true) // if take damage, start knockback.
        {
            KnockBack();
        }
      
    }

    void Patrol()
    {
        rb.velocity = new Vector2(moveSpeed * Time.deltaTime, rb.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundPos.position, groundPosRadius, layerMarks);
        if(!isGrounded && faceRight)
        {
            Flip();
        }else if(!isGrounded && !faceRight)
        {
            Flip();
        }  
    }


    void Flip()
    {
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
        moveSpeed = -moveSpeed;
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Take Damage!");
        //if(currentHealth <= 0)
        //{
        //currentHealth = 0;
        // Destroy(this.gameObject);
        // }

        //if enemy take damage, set isKnockBack == true and knockbackCountTime = knockbackLength;
        isKnockback = true;
        knockbackCount = knockbackLength;      
    }

    //Enemy knockback.
    void KnockBack()
    {
        if (knockbackCount <= 0) // if knockbackCount <= 0 , stop knockback and move again.
        {
            isKnockback = false;
        }
        else
        {
            if (player.transform.position.x < transform.position.x)
            {
                rb.AddForce(Vector2.right * knockbackForce);  // addforce. if facingright, knock left.
            }
            else if (player.transform.position.x > transform.position.x)
            {
                rb.AddForce(Vector2.right * -knockbackForce);  // addforce. if facingleft, knock right.
            }
        }
        // when take damage , knockbackCount will be minus to zero or less, it will make enemy stop knockback and move again.
        knockbackCount -= Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Timestop>().StopTime(0.5f);
            StartCoroutine(WaitForKnockBack());
        }
    }

    //waiting when time is resume, starting knockback
    IEnumerator WaitForKnockBack()
    {
        
        while (Time.timeScale != 1.0f)
        {
            yield return null;
        }
        player.knockbackCount = player.knockbackLength;


        if (player.transform.position.x < transform.position.x)
        {
            player.knockbackFromRight = true;
        }
        else
        {
            player.knockbackFromRight = false;
        }
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


}
