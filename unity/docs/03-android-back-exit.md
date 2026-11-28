# Unity 03 – Android back gesture: close the app

**What you will build:** Support the Android edge back-swipe gesture (and Back button) so users can exit the AR app cleanly.
**Time:** about 10 minutes. **Code:** 1 short C# script

Do [01 – World AR](01-world-ar.md) or [02 – Image marker AR](02-image-marker.md) first.

> Need to compare with the finished version? Both `01-World-AR-Completed` and `02-Image-Marker-Completed` in `Assets/Completed/Scenes/` already have `Assets/Completed/Scripts/AndroidBackToQuit` attached to **AR Session**.

## Why we need this

On Android, a full-screen Unity AR app does not automatically close when the user swipes from the left or right edge (or taps the system Back button). Unless your scene listens for that back event and calls `Application.Quit()`, the gesture does nothing.

When **Predictive Back Support** is off (the default in **Edit > Project Settings > Player > Android > Other Settings**), Android delivers the back-swipe gesture as `KEYCODE_BACK`, which Unity's **Input System** maps to `<Keyboard>/escape`.

## Step 1 – Create the script

1. In the **Project** window, right-click `Assets` > **Create > Folder** and name it `Scripts` (skip if it already exists).
2. Open `Assets/Scripts`, right-click > **Create > Scripting > MonoBehaviour Script** (or **C# Script**).
3. Name it `AndroidBackToQuit` and press **Enter**.
4. Double-click `AndroidBackToQuit` to open it in **Visual Studio Code**.

## Step 2 – Add the back-gesture code

Replace the contents of `AndroidBackToQuit.cs` with:

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

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
```

Save the file (**Ctrl+S** / **Cmd+S**).

### How it works

| Part | What it does |
|------|--------------|
| `using UnityEngine.InputSystem;` | Uses Unity's New Input System (the active input backend in this project) |
| `Application.platform != RuntimePlatform.Android` | Only listens on Android so pressing **Esc** in Unity Editor XR Simulation still just unlocks your mouse cursor |
| `"<Keyboard>/escape"` | Receives Android's back-swipe gesture and Back button |
| `Application.Quit();` | Closes the app when the back gesture is performed |

## Step 3 – Attach it to your scene

Repeat this for each buildable scene (`Assets/Scenes/01-World-AR` and `Assets/Scenes/02-Image-Marker`):

1. Open the scene.
2. Select **AR Session** in the **Hierarchy**.
3. Click **Add Component**, search for `AndroidBackToQuit`, and add it (or drag `AndroidBackToQuit.cs` onto **AR Session**).
4. Press **Ctrl+S** (**Cmd+S** on Mac) to save the scene.

## Step 4 – Test on an Android phone

1. Build and run the scene on your Android phone (**File > Build Profiles > Android > Build and Run**).
2. Once the AR camera view opens, swipe inward from the left or right edge of the screen (or tap Back). The app closes cleanly.

## Troubleshooting

| Problem | Fix |
|---------|-----|
| Compiler error about `InputAction` | Make sure `using UnityEngine.InputSystem;` is at the top of the file |
| Pressing `Esc` in Editor Play mode does not quit | That is expected — the script checks `RuntimePlatform.Android` so `Esc` only unlocks the mouse in XR Simulation |
| Back swipe does nothing on Android | Check `AndroidBackToQuit` is attached to an active GameObject (such as **AR Session**) in the scene you built, and that **Predictive Back Support** is unticked in **Project Settings > Player > Android** |
