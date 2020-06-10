using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueMana : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI npcNameText;
    [SerializeField] GameObject dialogueBox;

    //create Queue. What is queue ? read document.
    private Queue<string> sentences;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //set name of NPC
        npcNameText.text = dialogue.npcName;
        //Clear previous sentences.
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // delete first elements. Example : S1 is first , S2 is second. After S1 display complete,we delete this to display S2.
        string sentence = sentences.Dequeue(); 
        dialogueText.text = sentence;
        //Stop coroutines to wait for sentences display complete.
        StopAllCoroutines();
        StartCoroutine(Type(sentence));
    }

    IEnumerator Type(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
    }

}
