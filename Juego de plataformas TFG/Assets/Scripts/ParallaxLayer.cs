using UnityEngine;

namespace Platformer.View
{
    /// <summary>
    /// Used to move a transform relative to the main camera position with a scale factor applied.
    /// This is used to implement parallax scrolling effects on different branches of gameobjects.
    /// </summary>
    public class ParallaxLayer : MonoBehaviour
    {
        /// <summary>
        /// Movement of the layer is scaled by this value.
        /// </summary>
        public Vector3 movementScale = Vector3.one;

        /// <summary>
        /// Reference to main camera.
        /// </summary>
        Transform cam;

        void Awake()
        {
            // Get reference to the main camera.
            cam = Camera.main.transform;
        }

        void LateUpdate()
        {
            // We update the position of the element with parallax, multiplying the position of the camera by the movementScale vector
            transform.position = Vector3.Scale(cam.position, movementScale);
        }

    }
}