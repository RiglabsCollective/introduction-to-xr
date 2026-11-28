using UnityEngine;
using UnityEngine.InputSystem;

namespace XRCourse.Completed
{
    /// <summary>
    /// Closes the application on Android when the user performs the system back swipe gesture or presses Back.
    /// </summary>
    public class AndroidBackToQuit : MonoBehaviour
    {
        InputAction m_BackAction;

        void OnEnable()
        {
            if (Application.platform != RuntimePlatform.Android)
                return;

            m_BackAction = new InputAction("AndroidBack", InputActionType.Button, "<Keyboard>/escape");
            m_BackAction.performed += OnBackPerformed;
            m_BackAction.Enable();
        }

        void OnDisable()
        {
            if (m_BackAction == null)
                return;

            m_BackAction.performed -= OnBackPerformed;
            m_BackAction.Disable();
            m_BackAction.Dispose();
            m_BackAction = null;
        }

        static void OnBackPerformed(InputAction.CallbackContext _)
        {
            Application.Quit();
        }
    }
}
