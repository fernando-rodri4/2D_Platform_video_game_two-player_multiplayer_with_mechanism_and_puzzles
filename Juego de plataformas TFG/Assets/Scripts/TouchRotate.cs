using UnityEngine;

public class TouchRotate : MonoBehaviour
{
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
