﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTrigger : MonoBehaviour
{
    Turret turret;
    // Start is called before the first frame update
    void Start()
    {
        turret = GameObject.FindGameObjectWithTag("turret").GetComponent<Turret>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            turret.startShooting = true;
        }
    }
}
