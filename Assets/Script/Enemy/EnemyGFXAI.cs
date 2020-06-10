using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFXAI : MonoBehaviour
{
    [SerializeField] AIPath ai;

    // Update is called once per frame
    void Update()
    {
        if(ai.desiredVelocity.x >= 0.01f)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if(ai.desiredVelocity.x <= -0.01f)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
