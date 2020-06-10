using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    [SerializeField] GameObject dialogueBubble;
    [SerializeField] GameObject dialogueBox;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueMana>().StartDialogue(dialogue);
        dialogueBox.SetActive(true);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dialogueBubble.SetActive(true);        
        }      
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueBubble.SetActive(false);
                TriggerDialogue();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dialogueBox.SetActive(false);
        dialogueBubble.SetActive(false);
    }

}
