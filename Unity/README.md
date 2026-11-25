# Unity projects

<p>
  <img src="logos/unity.png" alt="Unity" width="72">
  &nbsp;
  <img src="logos/vscode.png" alt="Visual Studio Code" width="72">
</p>

One set-up, one try-out and six beginner projects for the course, built with **Unity 6** and **AR Foundation** (AR) and the **XR Interaction Toolkit** (VR).

**First time?** Do [Getting started with Unity](GETTING-STARTED.md) first. It covers your Unity account, the download, VS Code and getting a phone ready.

**Testing.** Every project is tested **in the Editor on the computer** with XR Simulation and the XR Device Simulator, so no phone or headset is needed. Every guide also has a short **Android** section and an **iPhone** section for building to a real device, written to match each other step for step.

**Tools.** Only tools from Unity itself are used: Unity 6, the Unity packages (AR Foundation, XR Interaction Toolkit and so on) and Unity's own sample assets. The 3D models and pictures are the same ones used in the Lens Studio course, copied to [`assets/`](assets/), so you can compare what each tool does well.

The rule for these projects: **as little code as possible.** Most of the work is done with Unity's ready-made components, set up in the Inspector. Where a tiny script saves a lot of time, it is short, copied from the guide and explained line by line.

> **Status: first full draft.** All lessons 000 to 007 now have a guide and a finished scene in the `xr-course` project. The scenes were checked in the Editor (no missing scripts or references, no console errors in Play mode, spawning and buttons tested by script). **Nothing has been tried by touch on a phone or in a headset yet**, and the guides still need a read-through by a person following them from the Starter package.

## Where the ideas come from

| Deck | What it gave us |
|------|-----------------|
| A003 AR Fundamentals | The AR types: markerless (surface), image tracking, face tracking |
| A004 AR Applications & Educational Experiences | The subjects: anatomy, chemistry, geometry, engineering, heritage, retail |
| A006 Creating AR with Unity 3D | The Unity skills: Hub, Editor, GameObjects, Transform, materials, AR Foundation, XR Simulation |
| A007 Virtual, Mixed & Extended Reality | The VR skills: 6DoF, controllers, teleport movement and comfort first |

## The set-up, the try-out and the projects

| # | Name | AR / VR type | What a beginner learns | Code |
|---|------|--------------|------------------------|------|
| 000 | **Set up** | AR | Create the AR project and test it in the Editor | None |
| 001 | **Try the XR Foundation** (not a project) | AR, markerless (surface) | Unity's built-in AR demo in the Mobile AR template: plane detection, tap to place a shape, move, rotate and scale it. Nothing to build, so it is a try-out only | None |
| 002 | **Place the Heart** | AR, markerless (surface) | Bring your own model into the scene, make a prefab, set the spawner to place it, touch to move, rotate and scale | None |
| 003 | **Textbook Pop-up** | AR, image tracking | A printed picture makes a 3D model appear, the reference image library | None |
| 004 | **Build a Molecule** | AR, markerless | Choosing and placing several different models, a simple menu with buttons | One 5-line script |
| 005 | **Label the Parts** | AR, markerless | World-space labels, a toggle button, labels that face the camera | Two short scripts |
| 006 | **Solar System on the Table** | AR, markerless | Parent and child objects, rotation and animation, scale for real-world size | One 5-line script |
| 007 | **Virtual Lab Visit** (VR demo) | VR, 6DoF | Teleport movement, grabbing objects, a world-space menu, comfort settings | None |

**001 is a try-out, not a project.** Unity does it all with built-in components, so there is nothing to build. From **002** the learner brings their own assets into the scene and sets them up for their own needs. **007** is the VR demo.

002 is the "hello world" project. Each project builds on a skill from the one before it, so they are best taken in order.

## Folder plan

```
Unity/
  README.md              this file
  GETTING-STARTED.md     account, download, VS Code and phone set-up (no screenshots)
  xr-course/             THE Unity project = the FINAL version (all lessons finished)
  packages/              XR-Course-Starter.unitypackage and XR-Course-Final.unitypackage
    Assets/Scenes/       001-Try-the-XR-Foundation, 002-Place-the-Heart, 003-... one scene per lesson
    Assets/Shared/       models, textures and prefabs used by more than one lesson
  000-setup/             guide: create the project and test it in the Editor
  001-try-the-xr-foundation/   guide (README + guide-images/)
  002-place-the-heart/   guide
  003-textbook-popup/    guide
  004-build-a-molecule/
  005-label-the-parts/
  006-solar-system-on-the-table/
  007-virtual-lab-visit/
  assets/                source models and pictures (copied from the Lens Studio course)
  logos/                 the app icons shown at the top of the guides
```

**Start and Final.** The **Final** version is the `xr-course` project: every lesson finished, to compare against. The **Starter** is one file, `packages/XR-Course-Starter.unitypackage`. A learner makes a new Mobile AR project (lesson 000), drags the Starter file into the Project window and clicks Import. They get all the models, textures and a clean `Start` scene, then build each lesson themselves and save it as a new numbered scene. `XR-Course-Final.unitypackage` holds the finished scenes for anyone who does not want the whole project.

**One project, many scenes.** Learners open `xr-course` once (one slow import) and then move through the lessons by opening the next numbered scene. Each lesson folder here holds only the guide and its screenshots. At the end of the course the project is also packaged as a single `.unitypackage` file.

Each guide follows the Lens Studio guides: what you will build, assets, steps with screenshots, what each part does, make it your own, troubleshooting and credits. The extra part is **Test on a phone**, with an **Android** and an **iPhone** version side by side.

## Ideas held back

- **AR face filter.** Face tracking in Unity needs an iPhone with a TrueDepth camera and a Mac for building, so it is not a good fit for a mixed-device class. It stays a Lens Studio topic (lenses 2, 6 and 7).
- **AR portal.** It needs custom shaders, which is too much for a first project. Lens 8 covers it in Lens Studio.
