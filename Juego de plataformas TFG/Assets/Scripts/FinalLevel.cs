using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalLevel : DialogueManager
{
    /// <summary>
    /// People shape
    /// </summary>
    [SerializeField] GameObject image = null;

    /// <summary>
    /// Final text
    /// </summary>
    [SerializeField] GameObject text = null;

    /// <summary>
    /// Sprites of final shapes
    /// </summary>
    [SerializeField] Sprite[] images = null;

    List <GameObject> players;

    new void Start()
    {
        base.Start();

        if (image == null || text == null || images[0] == null || images[1] == null)
        {
            Debug.LogError("Error with DialogueManagerFinalCinematicTutorial script component " + this);
            Destroy(this);
            return;
        }

        players = new List<GameObject>();
    }


    // Update is called once per frame
    void Update()
    {
        if (players.Count == 0)
        {
            return;
        }
        else if (players.Count == 2 && Input.GetButtonDown("Enter") && isDialogueStart)
        {
            DisplayNextSentence();
        }
        else if (players.Count == 2 && !isDialogueStart)
        {
            foreach (var player in players)
            {
                player.GetComponent<PlayerMovement>().canMove = false;
            }

            LevelManager.Instance.SetGameOverTrue();

            image.SetActive(false);
            text.SetActive(false);
            
            dialoguePanel.SetActive(true);
            displayText.fontSize = textSize;
            displayText.text = "";
            StartDialogue();
            isDialogueStart = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer && !players.Contains(collision.gameObject)/* && Input.GetButtonDown("Enter")*/)   //Todo: Intentar poner confirmación de jugador
        {
            players.Add(collision.gameObject);

            image.GetComponent<SpriteRenderer>().sprite = images[1];
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        players.Remove(collision.gameObject);

        image.GetComponent<SpriteRenderer>().sprite = images[0];
    }

    new void DisplayNextSentence()
    {
        if (sentences.Count <= 0 && displayText.text == activeSentence)
        {
            dialoguePanel.SetActive(false);
            LevelManager.Instance.PlayerWin();
            return;
        }

        DisplayNextSentenceFuncionality();
    }

    new void DisplayNextSentenceFuncionality()
    {
        if (displayText.text == activeSentence || displayText.text == "")
        {
            button.SetActive(false);

            activeSentence = sentences.Dequeue();

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(TypeTheSentence(activeSentence));
        }
    }
}