using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulture : MonoBehaviour
{
    [SerializeField] float moveSpeed, circleRadius;

    
    private Rigidbody2D rb;
    private bool roofTouch, rightTouch, groundTouch, facingRight = true;

    private Player player;

    //direction of Enemy
    [SerializeField] float dirX = 1, dirY = 0.25f;

    [Header("GroundCheck")]
    [SerializeField] Transform roofPos;
    [SerializeField] Transform rightPos;
    [SerializeField] Transform groundPos;
    [SerializeField] LayerMask layerMarks;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, dirY) * moveSpeed * Time.deltaTime;
        HitDetection();
    }

    void HitDetection()
    {
        roofTouch = Physics2D.OverlapCircle(roofPos.position, circleRadius, layerMarks);
        rightTouch = Physics2D.OverlapCircle(rightPos.position, circleRadius, layerMarks);
        groundTouch = Physics2D.OverlapCircle(groundPos.position, circleRadius, layerMarks);
        HitLogic();
    }

    void HitLogic()
    {
        if(rightTouch && facingRight)
        {
            Flip();
        }
        else if(rightTouch && !facingRight)
        {
            Flip();
        }

        if (roofTouch)
        {
            dirY = -0.25f;
        }

        if (groundTouch)
        {
            dirY = 0.25f;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        dirX = -dirX;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Timestop>().StopTime(0.5f);
        StartCoroutine(WaitForKnockBack());
    }

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
        Gizmos.DrawWireSphere(roofPos.position, circleRadius);
        Gizmos.DrawWireSphere(rightPos.position, circleRadius);
        Gizmos.DrawWireSphere(groundPos.position, circleRadius);
    }

}
