# Unity 02 – Image marker AR: textbook pop-up

**What you will build:** Point the camera at a printed page and a V8 engine appears on it.
**AR type:** Image tracking
**Time:** about 20 minutes. **Code:** none

Do [Set up](../00-setup/README.md) first.

## Step 1 – Make your scene

Open `Assets/Starter/Start`. Choose **File > Save As** and save it as `02-Image-Marker`. Keep the word **Marker** in the name: it tells the simulator to put the page on the table.

Select **Object Spawner** and untick the box next to its name, so clicking does nothing in this scene.

## Step 2 – Look at the prefab

In `Assets/Shared/Prefabs`, double-click `V8Engine`. It is ready to use: an engine 15 cm across. Click the arrow at the top of the Hierarchy to go back.

## Step 3 – Make the image library

This is the list of pictures the app looks for.

1. Right-click in the **Project** window > **Create > XR > Reference Image Library**. Name it `TextbookImages`.
2. Click **Add Image** and drag `engine-marker` (in `Assets/Shared/Textures`) into the slot.
3. Tick **Specify Size** and enter `0.2` by `0.2`. This is the printed size in metres.

![The image library](guide-images/step-03-image-library.png)

## Step 4 – Give the prefab to the AR Tracked Image Manager

1. Select **XR Origin (AR Rig)**, click **Add Component** and add **AR Tracked Image Manager**.
2. Drag `TextbookImages` into **Serialized Library**.
3. Set **Max Number Of Moving Images** to `1`.
4. Drag the `V8Engine` prefab into **Tracked Image Prefab**.

![The AR Tracked Image Manager](guide-images/step-04-tracked-image-manager.png)

## Step 5 – Press Play

Press **Play** and wait a few seconds. The engine appears on the page on the table. Hold the **right mouse button** to look around and walk with **W A S D**.

## Test on a phone (optional)

Print `unity/assets/textures/engine-marker.png` 20 cm wide, or show it on another screen. Then build as in [World AR](../01-world-ar/README.md), adding this scene.

## Make it your own

Use your own picture (one with lots of detail) and your own prefab. The steps are the same.

## Troubleshooting

| Problem | Fix |
|---------|-----|
| Nothing appears | Check the library and prefab are assigned, and the scene name contains `Marker` |
| The size is wrong | Check **Specify Size** matches the printed picture |
| You cannot see the page | Walk towards the table with **W A S D** |

## Credits

"V8 Engine" by little.bucket on Sketchfab, licensed under [CC Attribution-NonCommercial 4.0](http://creativecommons.org/licenses/by-nc/4.0/). Non-commercial use only.
