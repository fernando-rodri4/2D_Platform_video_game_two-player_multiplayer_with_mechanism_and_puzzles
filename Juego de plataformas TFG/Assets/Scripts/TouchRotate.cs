using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    void OnMouseDown()
    {
        if(!PuzzleTutorialController.Instance.GetIsCorrect())
        {
            transform.Rotate(0f, 0f, 90f);
        }
    }
}
