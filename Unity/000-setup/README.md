# Unity 000 – Set up the first AR project

**What you will build:** An empty AR project that runs in the Unity Editor with **XR Simulation**, so you can place cubes on a pretend floor with the mouse. Every later project starts from this one.
**XR concept:** AR Foundation, plane detection, the XR Simulation environment (Deck A006)
**Unity version:** Unity 6 (6000.x) on Windows or macOS
**Time:** about 20 minutes

> **Status: draft.** This lesson creates the project every later lesson uses. The finished course project is in [`../xr-course/`](../xr-course/). Written without screenshots; still needs a check in the Editor.

Not installed yet? Do [Getting started with Unity](../GETTING-STARTED.md) first.

## What participants learn

- An AR scene needs two special objects: an **AR Session** (runs the tracking) and an **XR Origin** (the camera and the tracked space).
- **AR Foundation** is one set of components that works on both Android (ARCore) and iPhone (ARKit).
- **Plane detection** finds flat surfaces such as the floor or a table. Objects are placed on those planes.
- **XR Simulation** pretends to be a phone in a room, so you can test AR on the computer.

## Steps

### Step 1 – Create the project

1. Open **Unity Hub** and click **New project**.
2. Choose the template **Mobile AR** (under the Universal 3D group). If it shows a download arrow, click it first.
3. Name the project `xr-course`, choose a folder and click **Create project**.

Unity opens with a scene called `SampleScene`. In this course you will keep ONE project and work scene by scene (`001 - …`, `002 - …`), so rename it to `001-Try-the-XR-Foundation` later. It already has an **AR Session**, an **XR Origin**, a **Directional Light**, an **Object Spawner** and a menu of shapes.

Prefer to skip this step? Open the finished course project from the repository instead: in Unity Hub choose **Add → Add project from disk** and pick `Unity/xr-course`. The first open takes several minutes while Unity imports everything.

### Step 2 – Look at the Hierarchy

In the **Hierarchy** (left) find these objects:

| Object | What it does |
|--------|--------------|
| **AR Session** | Starts and stops the phone's AR tracking |
| **XR Origin (Mobile AR)** | Holds the camera. The phone's movement moves this camera through the scene |
| **Object Spawner** | Puts the chosen shape on the detected plane when you tap |
| **UI** | The on-screen menu of shapes and the options button |

Why two objects? The **AR Session** only runs tracking. The **XR Origin** is where the tracked camera and the detected planes live.

### Step 3 – Turn on XR Simulation

1. Choose **Edit → Project Settings → XR Plug-in Management**.
2. Click the **PC** tab (the monitor icon).
3. Tick **XR Simulation**.
4. Choose **Window → XR → AR Foundation → XR Environment**, then click the **Simulation** option in the toolbar if it is not on already.

The Scene view can now show a small room. That room is the pretend world your AR phone looks at.

### Step 4 – Press Play

1. Make sure the **Game** tab is open and press **Play**.
2. Hold the **right mouse button** and move the mouse to look around. Use **W A S D** to walk.
3. When the floor appears as a pattern of dots, choose a shape in the menu and **click on the floor**.

A cube should now sit on the floor. Walk around it. It stays in place, which is the main idea of AR.

### Step 5 – Try one change

1. Stop Play mode.
2. In the Hierarchy select **Object Spawner**.
3. In the Inspector, change **Spawn Angle Range** or the object prefab list.
4. Press Play and see the difference.

Change one thing at a time, and watch what moves.

## Test on a phone

### Android
1. Turn on **Developer options** and **USB debugging** on the phone, and connect it with a cable.
2. In **File → Build Profiles** choose **Android**, click **Switch Platform**, then **Build and Run**.

### iPhone
1. You need a Mac with Xcode. Choose **iOS** in **Build Profiles** and click **Build**.
2. Open the generated Xcode project, choose your team under **Signing**, and run it on the phone.

## Make it your own

- Add a second light or change the colour of the shapes.
- Remove the shapes you do not need from the Object Spawner list.

## Troubleshooting

| Problem | Fix |
|---------|-----|
| The Mobile AR template is missing | Update to Unity 6 in Unity Hub, then try **New project** again |
| Play mode shows a black Game view | Check **XR Simulation** is ticked in **Project Settings → XR Plug-in Management → PC** |
| The floor never shows dots | Open the XR Environment window and check **Simulation** is on, then press Play again |
| Pink objects | The project is not using URP. Re-create it from the Mobile AR template |
| Errors about missing packages | Open **Window → Package Manager** and let it finish restoring |

## Credits

The project uses Unity's **Mobile AR** template and the **AR Starter Assets** sample from the XR Interaction Toolkit. Both are from Unity.
