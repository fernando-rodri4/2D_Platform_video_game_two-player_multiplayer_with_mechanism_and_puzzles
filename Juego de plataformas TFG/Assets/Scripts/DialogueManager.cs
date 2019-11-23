using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// Sentences that person entering the collider says
    /// </summary>
    protected Queue<string> sentences;

    /// <summary>
    /// Dialogue panel
    /// </summary>
    [SerializeField] protected GameObject dialoguePanel = null;

    /// <summary>
    /// Button that appear when current sentence is visible
    /// </summary>
    [SerializeField] protected GameObject button = null;

    /// <summary>
    /// Text panel that contains the current sentence
    /// </summary>
    [SerializeField] protected TextMeshProUGUI displayText = null;

    /// <summary>
    /// Player that enters in the collider
    /// </summary>
    GameObject playerEnter;

    [SerializeField] protected float textSize = -1;

    /// <summary>
    /// Write speed
    /// </summary>
    [SerializeField] float typingSpeed = 0;

    /// <summary>
    /// Current sensence it's being written
    /// </summary>
    protected string activeSentence;

    /// <summary>
    /// The layer the player game object is on
    /// </summary>
    protected int playerLayer;

    protected bool isDialogueStart = false;
    
    Coroutine coroutine = null;

    public Dialogue dialogue;

    // Start is called before the first frame update
    protected void Start()
    {
        sentences = new Queue<string>();

        //Get the integer representation of the "Player" layer
        playerLayer = LayerMask.NameToLayer("Player");

        displayText.text = "";

        if (dialoguePanel == null || button == null || displayText == null)
        {
            Debug.LogError("Error with DialogueManager script component " + this);
            Destroy(this);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerEnter == null)
        {
            return;
        }

        if (playerEnter != null && Input.GetButtonDown("Enter") && isDialogueStart)
        {
            DisplayNextSentence();
        }
        else if (playerEnter != null && Input.GetButtonDown("Enter") && !isDialogueStart)
        {
            playerEnter.GetComponent<PlayerMovement>().canMove = false;

            dialoguePanel.SetActive(true);
            displayText.fontSize = textSize;
            StartDialogue();
            isDialogueStart = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            playerEnter = collision.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            playerEnter = null;
        }
    }

    protected void StartDialogue()
    {
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    protected void DisplayNextSentence()
    {
        if(sentences.Count <= 0 && displayText.text == activeSentence)
        {
            dialoguePanel.SetActive(false);
            isDialogueStart = false;

            playerEnter.GetComponent<PlayerMovement>().canMove = true;

            return;
        }

        DisplayNextSentenceFuncionality();
    }

    protected void DisplayNextSentenceFuncionality()
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
        else
        {
            displayText.text = activeSentence;
        }
    }

    IEnumerator TypeTheSentence(string sentence)
    {
        displayText.text = "";

        for (int i = 0; i < sentence.ToCharArray().Length && displayText.text != activeSentence; i++)
        {
            displayText.text += sentence.ToCharArray()[i];
            AudioLevelManager.Instance.PlayLetterAudio();
            yield return new WaitForSeconds(typingSpeed);
        }

        button.SetActive(true);
    }
}