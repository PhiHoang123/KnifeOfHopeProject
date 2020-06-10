using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameTransition : MonoBehaviour
{
    [SerializeField] GameObject firstFrame;
    [SerializeField] GameObject secondFrame;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (firstFrame.active == true)
        {
            firstFrame.SetActive(false);
            secondFrame.SetActive(true);
        }else if(firstFrame.active == false)
        {
            firstFrame.SetActive(true);
            secondFrame.SetActive(false);
        }
    }
}
