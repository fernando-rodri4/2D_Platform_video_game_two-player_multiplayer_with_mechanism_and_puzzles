// This script is a Manager that controls the the flow and control of the level. It keeps
// track of player data (thieves count, total level time) and interfaces with
// the UILevelManager.
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    GameManager() { }

    /// <summary>
    /// This class holds a static reference to itself to ensure that there will only be
    /// one in existence. This is often referred to as a "singleton" design pattern. Other
    /// scripts access this one through this instance.
    /// </summary>
    public static GameManager Instance = null;


}
