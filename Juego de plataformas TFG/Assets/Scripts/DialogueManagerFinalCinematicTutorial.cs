using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManagerFinalCinematicTutorial : DialogueManager
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

    [SerializeField] GameObject gameTitle = null;

    List <GameObject> players;

    new void Start()
    {
        base.Start();

        Debug.Log("cambiar ontrigger");


        if (image == null || text == null || gameTitle == null || images[0] == null || images[1] == null )
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

        if (players.Count == 2 && Input.GetButtonDown("Enter") && isDialogueStart)
        {
            DisplayNextSentence();
        }
        else if (players.Count == 2 && !isDialogueStart)
        {
            foreach (var player in players)
            {
                player.GetComponent<PlayerMovement>().canMove = false;
            }

            image.SetActive(false);
            text.SetActive(false);
            
            dialoguePanel.SetActive(true);
            displayText.fontSize = textSize;
            StartDialogue();
            isDialogueStart = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer /*&& !players.Contains(collision.gameObject) && Input.GetButtonDown("Enter")*/)
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

            ActivateCamera.Instance.EnableCamera(1);

            StartCoroutine(AppearTitle());

            return;
        }

        DisplayNextSentenceFuncionality();
    }

    IEnumerator AppearTitle()
    {
        yield return new WaitForSeconds(7);

        gameTitle.SetActive(true);

        TextMeshProUGUI title = gameTitle.GetComponent<TextMeshProUGUI>();

        for(float i = 0; i < 1; i += 0.1f)
        {
            title.color = new Vector4(1, 1, 1, i);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2);

        gameTitle.SetActive(false);

        LevelManager.Instance.PlayerWin();
    }
}