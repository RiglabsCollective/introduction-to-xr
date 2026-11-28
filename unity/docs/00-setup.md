# Unity 00 – Set up

**What you will build:** An empty AR project ready to run on your computer.
**Time:** about 15 minutes

Not installed yet? Do [Getting started with Unity](../GETTING-STARTED.md) first.

## Option A – Open the course project (recommended)

1. Open **Unity Hub** and go to **Projects**.
2. Click **Add > Add project from disk**.
3. Select the `unity/xr-course` folder and open it with **Unity 6000.6.11f1**.
4. In the **Project** window:
   - `Assets/Scenes/` holds the starter template scenes (`01-World-AR` and `02-Image-Marker`) with XR already configured.
   - `Assets/Shared/` holds the raw models and textures (`Models/` and `Textures/`).
   - `Assets/Completed/` holds the finished reference scenes, prefabs, materials, and image library.

## Option B – Create a new Unity AR project from scratch

If you want to create a brand-new project in Unity Hub instead:

### Step 1 – Create the project

1. Open **Unity Hub** and click **New project**.
2. Choose the **AR Mobile** template (it may be listed as **Mobile AR**). Download it if asked.
3. Name the project, choose a folder and click **Create project**. The first open takes a few minutes. Close any welcome, tutorial or material upgrade popups.

### Step 2 – Turn on XR Simulation

1. Choose **Edit > Project Settings > XR Plug-in Management**.
2. Click the computer tab (**Windows, Mac, Linux**).
3. Tick **XR Simulation**.

XR Simulation pretends to be a phone in a room, so you can test AR on the computer.

## What is in the scene

Open `Assets/Scenes/01-World-AR` (or `Assets/Starter/Start`). In the **Hierarchy** find:

| Object | What it does |
|--------|--------------|
| **AR Session** | Runs the AR tracking |
| **XR Origin (AR Rig)** | Holds the camera |
| **Object Spawner** | Places an object where you tap |

Next: [01 – World AR](01-world-ar.md).
