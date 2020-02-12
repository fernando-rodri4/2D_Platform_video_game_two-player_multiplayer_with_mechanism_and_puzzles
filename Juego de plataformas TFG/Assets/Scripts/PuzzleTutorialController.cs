﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PuzzleTutorialController : MonoBehaviour
{
    PuzzleTutorialController() { }

    /// <summary>
    /// This class holds a static reference to itself to ensure that there will only be
    /// one in existence. This is often referred to as a "singleton" design pattern.
    /// </summary>
    public static PuzzleTutorialController Instance = null;

    /// <summary>
    /// Puzzle pictures
    /// </summary>
    [SerializeField] Transform[] picturesBackground = null;

    /// <summary>
    /// Puzzle pictures
    /// </summary>
    [SerializeField] Transform[] pictures = null;

    /// <summary>
    /// Box that active final ladder when puzzle is correct
    /// </summary>
    [SerializeField] GameObject activeLadder = null;

    /// <summary>
    /// The layer the player game object is on
    /// </summary>
    int playerLayer;

    List<GameObject> players;

    bool isCorrect = false;

    int activeForPlayer1 = 0, activeForPlayer2 = 1;

    void Awake()
    {
        //If an PuzzleTutorialController exists and it is not this...
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Error with PuzzleTutorialController script components, 2 instances " + this);
            //...destroy this and exit. There can be only one PuzzleTutorialController
            Destroy(gameObject);
            return;
        }

        //This is the Instance PuzzleTutorialController and it should persist
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (picturesBackground == null || pictures == null || activeLadder == null)
        {
            Debug.LogError("Error with PuzzleTutorialController script component " + this);
            Destroy(this);
            return;
        }

        //Get the integer representation of the "Player" layer
        playerLayer = LayerMask.NameToLayer("Player");

        players = new List<GameObject>();

        //Active the first image for each player
        if (ClientScene.localPlayers != null)
        {
            var players = ClientScene.localPlayers;

            foreach (var player in players)
            {
                if (player.gameObject.GetComponent<NetworkIdentity>().hasAuthority)
                {
                    if (player.gameObject.GetComponent<PlayerMovement>().GetId() == 0)
                    {
                        pictures[activeForPlayer1].localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        pictures[activeForPlayer2].localScale = new Vector3(1, 1, 1);
                    }
                    return;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCorrect || players.Count == 0)
        {
            return;
        }

        NextPicture();

        Rotate();

        if (players.Count == 2)
        {
            ActivateCamera.Instance.EnableCamera(0);

            foreach (var player in players)
            {
                player.GetComponent<PlayerMovement>().canMove = false;
            }
        }

        if (pictures[0].rotation.z == 0 && pictures[1].rotation.z == 0 && pictures[2].rotation.z == 0 && pictures[3].rotation.z == 0 &&
            pictures[4].rotation.z == 0 && pictures[5].rotation.z == 0 && pictures[6].rotation.z == 0 && pictures[7].rotation.z == 0)
        {
            StartCoroutine(CompletePuzzle());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer && !players.Contains(collision.gameObject) /*&& Input.GetButtonDown("Enter")*/) //Todo: Intentar poner confirmación de jugador
        {
            players.Add(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        players.Remove(collision.gameObject);
    }

    /// <summary>
    /// Return the variable 'isCorrect'
    /// </summary>
    /// <returns>Variable that says if the puzzle is solved</returns>
    public bool GetIsCorrect()
    {
        return isCorrect;
    }

    /// <summary>
    /// Rotate picture left or right when player press the correct button
    /// </summary>
    void Rotate()
    {
        if ((Input.GetButtonDown("RotateRight") && players[0].GetComponent<PlayerMovement>().GetId() == 0) || Input.GetButtonDown("Enter"))
        {
            pictures[activeForPlayer1].GetComponent<TouchRotate>().RotateRight();
        }
        else if ((Input.GetButtonDown("RotateRight") && players[0].GetComponent<PlayerMovement>().GetId() == 1) || Input.GetButtonDown("Enter")) //Todo:cambiar a enter2
        {
            pictures[activeForPlayer2].GetComponent<TouchRotate>().RotateRight();
        }

        if ((Input.GetButtonDown("RotateLeft") && players[0].GetComponent<PlayerMovement>().GetId() == 0) || Input.GetButtonDown("Enter"))
        {
            pictures[activeForPlayer1].GetComponent<TouchRotate>().RotateLeft();
        }
        else if ((Input.GetButtonDown("RotateLeft") && players[0].GetComponent<PlayerMovement>().GetId() == 1) || Input.GetButtonDown("Enter")) //Todo:cambiar a enter2
        {
            pictures[activeForPlayer2].GetComponent<TouchRotate>().RotateLeft();
        }
    }

    /// <summary>
    /// Select the next picture
    /// </summary>
    void NextPicture()
    {
        if ((Input.GetButtonDown("Enter") && players[0].GetComponent<PlayerMovement>().GetId() == 0) || Input.GetButtonDown("Enter"))
        {
            pictures[activeForPlayer1].localScale = new Vector3(0.96f, 0.96f, 1);

            if (activeForPlayer1 == 0 || activeForPlayer1 == 5)
            {
                activeForPlayer1 = (activeForPlayer1 + 2) % 8;
            }
            else if (activeForPlayer1 == 7)
            {
                activeForPlayer1 = (activeForPlayer1 + 1) % 8;
            }
            else
            {
                activeForPlayer1 = (activeForPlayer1 + 3) % 8;
            }

            pictures[activeForPlayer1].localScale = new Vector3(1, 1, 1);
        }
        else if ((Input.GetButtonDown("Enter") && players[0].GetComponent<PlayerMovement>().GetId() == 1) || Input.GetButtonDown("Enter")) //Todo:cambiar a enter2
        {
            pictures[activeForPlayer2].localScale = new Vector3(0.96f, 0.96f, 1);

            if (activeForPlayer2 == 1 || activeForPlayer2 == 4)
            {
                activeForPlayer2 = (activeForPlayer2 + 2) % 8;
            }
            else if (activeForPlayer2 == 3)
            {
                activeForPlayer2 = (activeForPlayer2 + 1) % 8;
            }
            else
            {
                activeForPlayer2 = (activeForPlayer2 + 3) % 8;
            }

            pictures[activeForPlayer2].localScale = new Vector3(1, 1, 1);
        }
    }

    IEnumerator CompletePuzzle()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (var picture in pictures)
        {
            picture.localScale = new Vector3(1, 1, 1);

            yield return new WaitForSeconds(0.5f);
        }

        isCorrect = true;

        foreach (var picture in picturesBackground)
        {
            picture.localScale = new Vector3(1, 1, 1);
            picture.rotation = Quaternion.Euler(0, 0, 0);
        }

        ActivateCamera.Instance.DisableCamera(0);

        foreach (var player in players)
        {
            player.GetComponent<PlayerMovement>().canMove = true;
        }

        players.Clear();

        // Active final ladder
        yield return new WaitForSeconds(1f);
        activeLadder.SetActive(true);
    }
}