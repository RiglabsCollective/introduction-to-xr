# Lens Studio lenses

Build each lens yourself by following its guide. Every file the guides ask for is in [`assets/`](assets/).

| # | Lens | What you build | XR concept | Guide |
|---|------|----------------|------------|-------|
| [1](001-3d-axis-explorer/) | 3D Axis Explorer | A world lens that places a cube in the room. Drag to move it, pinch to scale it and swipe with one finger to spin it. | World tracking, 3D coordinates and transforms (Deck 1 spatial concepts) | [Guide](001-3d-axis-explorer/README.md) |
| [2](002-face-filter-name-badge/) | Face-Filter Name Badge | A head-tracked lens that floats the user's name, a department and a logo above their head. | Face tracking | [Guide](002-face-filter-name-badge/README.md) |
| [3](003-solar-system-on-the-table/) | Solar System on the Table | Surface tracking puts the solar system on a table. The planets orbit at their real speed ratios and a name floats above each one. | Surface tracking, 3D text labels | [Guide](003-solar-system-on-the-table/README.md) |
| [4](004-anatomy-overlay/) | Anatomy Overlay | A large 3D human heart sits on the floor or a table. Spin it with one finger and read the labels that name each part. | Surface tracking, 3D anatomy models and annotation labels (Deck 4 healthcare examples) | [Guide](004-anatomy-overlay/README.md) |
| [5](005-marker-based-textbook/) | Marker-Based Textbook | Point at a printed textbook page and an animated 3D V8 engine pops up off it, flying apart and back together. | Image marker tracking | [Guide](005-marker-based-textbook/README.md) |
| [6](006-ar-quiz-lens/) | AR Quiz Lens | A quiz floats above your head. Raise your eyebrows or open your mouth to pick an answer. | Face tracking, facial-expression input, the Behavior script (no code) | [Guide](006-ar-quiz-lens/README.md) |
| [7](007-virtual-try-on/) | Virtual Try-On | Glasses and a safety helmet on your face. Tap the screen to try the next one. | Face tracking, 3D accessories, the Behavior script (retail and industry use cases) | [Guide](007-virtual-try-on/README.md) |
| [8](008-heritage-site-portal/) | Portal to a Heritage Site | A doorway on the floor that you walk through into a 360° monument or museum scene. | World tracking, portals and 360° environments | [Guide](008-heritage-site-portal/README.md) |
| [9](009-chemistry-molecule-builder/) | Chemistry Molecule Builder | Tap to add atoms on a surface and watch bonds form, such as H₂O or CO₂. | Surface tracking, tap interaction, scripting | [Guide](009-chemistry-molecule-builder/README.md) |
| [10](010-campus-wayfinding-sign/) | Campus Wayfinding Sign | Point at a building marker to get floating arrows and info about labs and departments. | Image marker tracking, spatial UI | [Guide](010-campus-wayfinding-sign/README.md) |

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
