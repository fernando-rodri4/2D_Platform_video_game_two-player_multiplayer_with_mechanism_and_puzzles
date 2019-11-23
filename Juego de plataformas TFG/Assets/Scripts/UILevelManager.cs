// This script is a Manager that controls the UI HUD (thieves captured, time) for the project.

using UnityEngine;
using TMPro;
using System.Collections;

public class UILevelManager : MonoBehaviour
{
    UILevelManager() { }

    /// <summary>
    /// This class holds a static reference to itself to ensure that there will only be
    /// one in existence. This is often referred to as a "singleton" design pattern. Other
    /// scripts access this one through this instance.
    /// </summary>
    public static UILevelManager Instance = null;

    /// <summary>
    /// Text element showing number of thieves captured.
    /// </summary>
    [SerializeField] TextMeshProUGUI thiefText = null;

    /// <summary>
    /// Thief text container.
    /// </summary>
    [SerializeField] RectTransform thiefCount = null;

    bool isThiefTextActive = false;

    void Awake()
    {
        //If an UILevelManager exists and it is not this...
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Error with UILevelManager script components, 2 instances " + this);
            //...destroy this and exit. There can be only one UILevelManager.
            Destroy(gameObject);
            return;
        }

        //This is the Instance UILevelManager.
        Instance = this;
    }

    void Start()
    {
        if (thiefText == null || thiefCount == null)
        {
            Debug.LogError("Error with UILevelManager script component " + this);
            Destroy(this);
            return;
        }
    }

    void FixedUpdate()
    {
        if (!isThiefTextActive && thiefCount.position.x == -127)
        {
            return;
        }

        if (isThiefTextActive && thiefCount.position.x < 127)
        {
            thiefCount.position = new Vector3(thiefCount.position.x + 2.54f, thiefCount.position.y, thiefCount.position.z); //1.984375f
        }
        else if (!isThiefTextActive && thiefCount.position.x > -127)
        {
            thiefCount.position = new Vector3(thiefCount.position.x - 2.54f, thiefCount.position.y, thiefCount.position.z);
        }
    }

    /// <summary>
    /// Update thieves number
    /// </summary>
    /// <param name="numThieves">Thieves number captured</param>
    /// <returns></returns>
    public IEnumerator UpdateThiefUI(int numThieves)
    {
        //If there is no Instance UILevelManager, exit
        if (Instance == null)
            yield break;

        isThiefTextActive = true;

        yield return new WaitForSeconds(2.6f);

        //Update the text orb element
        thiefText.text = "x " + numThieves.ToString();

        StartCoroutine(FlickeringText(thiefText, Color.white, Color.red));

        yield return new WaitForSeconds(2.6f);

        isThiefTextActive = false;
    }

    /// <summary>
    /// Flicker effect in the text with 2 color
    /// </summary>
    /// <param name="text"></param>
    /// <param name="color1">Original color</param>
    /// <param name="color2">Second color</param>
    /// <returns></returns>
    IEnumerator FlickeringText(TextMeshProUGUI text, Color color1, Color color2)
    {
        float time = 0;
        float waitSecond = 0.15f;

        while (time < 0.75)
        {
            //Cambiamos el color
            text.color = color1;

            //Mostramos el texto por waitSecond segundos
            yield return new WaitForSeconds(waitSecond);

            time += waitSecond;

            //Cambiamos el color
            text.color = color2;

            //mostramos el texto por waitSecond segundos
            yield return new WaitForSeconds(waitSecond);

            time += waitSecond;
        }

        //Establecemos nuestro texto en blanco
        text.color = color1;
    }
}
