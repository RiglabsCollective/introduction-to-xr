# Unity projects

<p>
  <img src="logos/unity.png" alt="Unity" width="72">
  &nbsp;
  <img src="logos/vscode.png" alt="Visual Studio Code" width="72">
</p>

Two beginner AR projects in **Unity 6** with **AR Foundation**. No code. Both can be tested on the computer with XR Simulation, so no phone is needed.

**First time?** Do [Getting started with Unity](GETTING-STARTED.md) first.

| # | Guide | What it teaches |
|---|-------|-----------------|
| 00 | [Set up](00-setup/README.md) | Create the project and import the Starter package |
| 01 | [World AR](01-world-ar/README.md) | Click a surface and a heart appears (markerless AR) |
| 02 | [Image marker AR](02-image-marker/README.md) | A printed picture makes an engine appear (image tracking) |

## The pattern

Both projects follow the same four steps:

1. Open the `Start` scene and save it under a new name.
2. Get a **prefab** (a saved object) ready.
3. Give the prefab to **one component**.
4. Press **Play**.

| | World AR | Image marker AR |
|---|---|---|
| Prefab | `Heart` (you make it) | `V8Engine` (ready-made) |
| Component | **Object Spawner** | **AR Tracked Image Manager** |
| When does it appear? | When you tap a surface | When the camera sees the picture |

## Folders

| Folder | What is in it |
|--------|---------------|
| `00-setup/`, `01-world-ar/`, `02-image-marker/` | The guides |
| `xr-course/` | The finished project, to compare with (scenes `01-World-AR` and `02-Image-Marker`) |
| `packages/XR-Course-Starter.unitypackage` | The models, pictures and `Start` scene for learners |
| `assets/` | Source models and pictures, with credits |
