# Unity projects

<p>
  <img src="docs/images/logos/unity.png" alt="Unity" width="72">
  &nbsp;
  <img src="docs/images/logos/vscode.png" alt="Visual Studio Code" width="72">
</p>

Two beginner AR projects in **Unity 6** with **AR Foundation**. No code. Both can be tested on the computer with XR Simulation, so no phone is needed.

**First time?** Do [Getting started with Unity](GETTING-STARTED.md) first.

| # | Guide | What it teaches |
|---|-------|-----------------|
| 00 | [Set up](docs/00-setup.md) | Open the `xr-course` project (or create a new AR project) and enable XR Simulation |
| 01 | [World AR](docs/01-world-ar.md) | Click a surface and a heart appears (markerless AR) |
| 02 | [Image marker AR](docs/02-image-marker.md) | A printed picture makes an engine appear (image tracking) |
| 03 | [Android back gesture](docs/03-android-back-exit.md) | Add a C# script so Android's back swipe closes the app |

## Scenes in `xr-course/`

Both lessons have a **starter template scene** (XR already set up, ready for you to build and assign prefabs/materials) and a **completed reference scene**:

| Lesson | Starter scene | Completed scene |
|--------|---------------|-----------------|
| World AR | `Assets/Scenes/01-World-AR` | `Assets/Completed/Scenes/01-World-AR-Completed` |
| Image marker AR | `Assets/Scenes/02-Image-Marker` | `Assets/Completed/Scenes/02-Image-Marker-Completed` |

## The pattern

Both projects follow the same four steps:

1. Open the starter scene (`01-World-AR` or `02-Image-Marker`).
2. Get a **prefab** (a saved object with materials and textures) ready.
3. Give the prefab to **one component**.
4. Press **Play**.

| | World AR | Image marker AR |
|---|---|---|
| Prefab | `Heart` (you make it) | `V8Engine` (ready-made or build your own) |
| Component | **Object Spawner** | **AR Tracked Image Manager** |
| When does it appear? | When you tap a surface | When the camera sees the picture |

## Folders

| Folder | What is in it |
|--------|---------------|
| `docs/` | All guides (`00-setup.md`, `01-world-ar.md`, `02-image-marker.md`) and guide images |
| `xr-course/` | The Unity project containing starter scenes, completed scenes, models, and textures (`Assets/Shared/`) |
