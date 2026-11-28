# Unity 00 – Set up

**What you will build:** An empty AR project that runs on your computer.
**Time:** about 20 minutes

Not installed yet? Do [Getting started with Unity](../GETTING-STARTED.md) first.

## Steps

### Step 1 – Create the project

1. Open **Unity Hub** and click **New project**.
2. Choose the **AR Mobile** template (it may be listed as **Mobile AR**). Download it if asked.
3. Name the project, choose a folder and click **Create project**. The first open takes a few minutes. Close any welcome, tutorial or material upgrade popups.

### Step 2 – Turn on XR Simulation

1. Choose **Edit > Project Settings > XR Plug-in Management**.
2. Click the computer tab (**Windows, Mac, Linux**).
3. Tick **XR Simulation**.

XR Simulation pretends to be a phone in a room, so you can test AR on the computer.

### Step 3 – Import the Starter package

1. Drag `unity/packages/XR-Course-Starter.unitypackage` into the **Project** window (or choose **Assets > Import Package > Custom Package**).
2. Unity may warn about a **Missing signature**. The package is this course's own file without a Unity signature, so choose to continue. Closing the warning cancels the import.
3. Click **Import** with everything ticked.

You now have the models and pictures in `Assets/Shared`, and a clean scene at `Assets/Starter/Start`.

### Step 4 – Look around

Open `Assets/Starter/Start`. In the **Hierarchy** find:

| Object | What it does |
|--------|--------------|
| **AR Session** | Runs the AR tracking |
| **XR Origin (AR Rig)** | Holds the camera |
| **Object Spawner** | Places an object where you tap |

Next: [World AR](../01-world-ar/README.md).
