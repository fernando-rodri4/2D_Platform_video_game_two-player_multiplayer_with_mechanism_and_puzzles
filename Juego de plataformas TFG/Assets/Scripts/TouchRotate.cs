using UnityEngine;

public class TouchRotate : MonoBehaviour
{

    /// <summary>
    /// Rotate the image to the right
    /// </summary>
    public void RotateRight()
    {
        if (PuzzleTutorialController.Instance != null && !PuzzleTutorialController.Instance.GetIsCorrect())
        {
            transform.Rotate(0f, 0f, -90f);
        }
        else if (PuzzleController.Instance != null && !PuzzleController.Instance.GetIsCorrect())
        {
            transform.Rotate(0f, 0f, -90f);
        }
    }

    /// <summary>
    /// Rotate the image to the left
    /// </summary>
    public void RotateLeft()
    {
        if (PuzzleTutorialController.Instance != null && !PuzzleTutorialController.Instance.GetIsCorrect())
        {
            transform.Rotate(0f, 0f, 90f);
        }
        else if (PuzzleController.Instance != null && !PuzzleController.Instance.GetIsCorrect())
        {
            transform.Rotate(0f, 0f, -90f);
        }
    }
}
