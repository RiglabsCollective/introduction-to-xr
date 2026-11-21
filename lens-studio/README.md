# Lens Studio lenses

Build each lens yourself by following its guide. Every file the guides ask for is in [`assets/`](assets/).

| # | Lens | What you build | XR concept | Guide | Status |
|---|------|----------------|------------|-------|--------|
| [1](001-3d-axis-explorer/) | 3D Axis Explorer | A world lens that places a cube in the room. Drag to move it, pinch to scale it and swipe with one finger to spin it. | World tracking, 3D coordinates and transforms (Deck 1 spatial concepts) | [Guide](001-3d-axis-explorer/README.md) | Draft guide |
| [2](002-face-filter-name-badge/) | Face-Filter Name Badge | A head-tracked lens that shows the faculty member's name, department and a college logo. | Face tracking | [Guide](002-face-filter-name-badge/README.md) | Planned |
| [3](003-solar-system-on-the-table/) | Solar System on the Table | Surface tracking puts planets in orbit on a table, and tapping a planet shows a fact card. | Surface tracking, tap interaction | [Guide](003-solar-system-on-the-table/README.md) | Planned |
| [4](004-anatomy-overlay/) | Anatomy Overlay | Body or face tracking shows the skull, muscles or organs over the user. | Body and face tracking (Deck 4 healthcare examples) | [Guide](004-anatomy-overlay/README.md) | Planned |
| [5](005-marker-based-textbook/) | Marker-Based Textbook | Point at a printed diagram (a heart or an engine) and an animated 3D model pops up off the page. | Image marker tracking | [Guide](005-marker-based-textbook/README.md) | Planned |
| [6](006-ar-quiz-lens/) | AR Quiz Lens | Questions float above the user's head and they answer by tilting their head left or right. | Face tracking, head-gesture input | [Guide](006-ar-quiz-lens/README.md) | Planned |
| [7](007-virtual-try-on/) | Virtual Try-On | Glasses, a lab coat or a safety helmet on the user. | Face and body tracking (retail and industry use cases) | [Guide](007-virtual-try-on/README.md) | Planned |
| [8](008-heritage-site-portal/) | Portal to a Heritage Site | A doorway on the floor that you walk through into a 360° monument or museum scene. | World tracking, portals and 360° environments | [Guide](008-heritage-site-portal/README.md) | Planned |
| [9](009-chemistry-molecule-builder/) | Chemistry Molecule Builder | Tap to add atoms on a surface and watch bonds form, such as H₂O or CO₂. | Surface tracking, tap interaction, scripting | [Guide](009-chemistry-molecule-builder/README.md) | Planned |
| [10](010-campus-wayfinding-sign/) | Campus Wayfinding Sign | Point at a building marker to get floating arrows and info about labs and departments. | Image marker tracking, spatial UI | [Guide](010-campus-wayfinding-sign/README.md) | Planned |

## Folder layout

```
lens-studio/
  assets/              shared files for all lenses (download these first)
    models/            3D models (.fbx, .glb, .obj)
    textures/          images, HDRIs, markers
    audio/
    scripts/
  001-3d-axis-explorer/
    README.md           step-by-step guide
    guide-images/      screenshots used in the guide
    project/           finished Lens Studio project, for reference
  002-face-filter-name-badge/
  ...
  _lens-template/      copy this to start a new lens
```

## Before you start

- Install **Lens Studio 5.25** or later (Windows or macOS). Every lens here is built and tested on 5.25.
- Install the Snapchat app on your phone to test lenses.

## Windows and macOS

Every project in this folder opens on both Windows and macOS. To keep it that way:

- **Windows users cloning with Git:** run `git config --global core.longpaths true` once, so long asset paths don't fail.
- **Open the project folder, not a copy of the Cache:** Lens Studio rebuilds `Cache/` and `Workspaces/` on each machine, so they are not in the repo.
- **File names:** use lowercase letters, numbers and hyphens only. No spaces, and nothing that differs only by capital letters (`Cube.png` and `cube.png` are the same file on Windows and macOS).
- **Large files:** keep each file under 50 MB. GitHub refuses files over 100 MB, and the repo does not use Git LFS so that **Code → Download ZIP** always contains the real files.
- **Finished lens:** each lens folder may include an exported `.lns` file, which works the same on both systems.
