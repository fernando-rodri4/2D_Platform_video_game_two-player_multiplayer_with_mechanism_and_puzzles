// This script is a Manager that controls the UI HUD (shadows captured, time) for the project.

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
    /// Text element that shows number of shadows captured.
    /// </summary>
    [SerializeField] TextMeshProUGUI shadowText = null;

    /// <summary>
    /// Shadow text container.
    /// </summary>
    [SerializeField] RectTransform shadowCount = null;

    /// <summary>
    /// The text element that shows the time required to complete the level.
    /// </summary>
    [SerializeField] TextMeshProUGUI totalTime = null;

    /// <summary>
    /// Text element that shows the number of puzzles completed.
    /// </summary>
    [SerializeField] TextMeshProUGUI puzzlesCompleted = null;

    /// <summary>
    /// Text element that shows number of shadows captured.
    /// </summary>
    [SerializeField] TextMeshProUGUI shadowsCaptured = null;

    /// <summary>
    /// Final panel with stats
    /// </summary>
    [SerializeField] GameObject finalPanel = null;

    bool isShadowTextActive = false;

    float initialShadowCountPosition = -1086.8f;
    float finalShadowCountPosition = -830.8f;

    float shadowCountSpeed = 2.54f;

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
        if (shadowText == null || shadowCount == null || totalTime == null || puzzlesCompleted == null || shadowsCaptured == null || finalPanel == null)
        {
            Debug.LogError("Error with UILevelManager script component " + this);
            Destroy(this);
            return;
        }
    }

    void FixedUpdate()
    {
        //Debug.Log(shadowCount.localPosition);
        //-1086.8 -  -830.8
        
        if (!isShadowTextActive && shadowCount.localPosition.x == initialShadowCountPosition)
        {
            return;
        }

        if (isShadowTextActive && shadowCount.localPosition.x < finalShadowCountPosition)
        {
            shadowCount.localPosition = new Vector3(shadowCount.localPosition.x + shadowCountSpeed, shadowCount.localPosition.y, shadowCount.localPosition.z);
        }
        else if (!isShadowTextActive && shadowCount.localPosition.x > initialShadowCountPosition)
        {
            shadowCount.localPosition = new Vector3(shadowCount.localPosition.x - shadowCountSpeed, shadowCount.localPosition.y, shadowCount.localPosition.z);
        }
    }

    /// <summary>
    /// Update shadows number
    /// </summary>
    /// <param name="numShadows">Shadows number captured</param>
    /// <returns></returns>
    public IEnumerator UpdateShadowUI(int numShadows)
    {
        //If there is no Instance UILevelManager, exit
        if (Instance == null)
            yield break;

        isShadowTextActive = true;

        yield return new WaitForSeconds(2.6f);

        //Update the text orb element
        shadowText.text = "x " + numShadows.ToString();

        StartCoroutine(FlickeringText(shadowText, Color.white, Color.red));

        AudioLevelManager.Instance.PlayCollectionAudio();

        yield return new WaitForSeconds(2.6f);

        isShadowTextActive = false;
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

    public void SetStatistics(string time, string puzzles, string shadows)
    {
        totalTime.text = time;
        puzzlesCompleted.text = puzzles;
        shadowsCaptured.text = shadows;

        finalPanel.SetActive(true);
    }
}
