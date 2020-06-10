using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    [Header("Crab")]
    private float moveSpeed;
    private bool facingRight = true;
    [SerializeField] float runSpeed;
    [SerializeField] float currentMoveSpeed; 

    [Header("GroundCheck")]
    private bool isGround;
    public LayerMask layerMask;
    [SerializeField] float groundRadius;
    [SerializeField] Transform groundPos;

    [Header("Vision")]
    [SerializeField] float distance;

    //Component
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
    }

    private void FixedUpdate()
    {
        Patrol();
    }

    //enemy patrol.
    void Patrol()
    {
        rb.velocity = new Vector2(moveSpeed * Time.deltaTime, rb.velocity.y);
        
        isGround = Physics2D.OverlapCircle(groundPos.position, groundRadius, layerMask);
        if (!isGround && facingRight)
        {
            Flip();
        }else if(!isGround && !facingRight)
        {
            Flip();
        }       
    }

    //when see player, enemy will chase player.
    void ChasePlayer()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, -transform.right, distance, 1 << LayerMask.NameToLayer("Player"));
        if(raycast.collider != null)
        {
            Debug.DrawLine(transform.position, raycast.point, Color.red);
            if (facingRight)
            {               
                moveSpeed = runSpeed;
            }
            else if(!facingRight)
            {
                moveSpeed = -runSpeed;
            }
           
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position - transform.right * distance, Color.green);
            if (facingRight)
            {
                moveSpeed = currentMoveSpeed; //reset moveSpeed when enemy doesn't see player
            }
            else
            {
                moveSpeed = -currentMoveSpeed; //reset moveSpeed when enemy doesn't see player
            }
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
        Gizmos.DrawWireSphere(groundPos.position, groundRadius);
    }


}
