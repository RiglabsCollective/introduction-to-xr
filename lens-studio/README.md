# Lens Studio lenses

Build each lens yourself by following its guide. Every file the guides ask for is in [`assets/`](assets/).

| # | Lens | What you build | XR concept | Guide | Status |
|---|------|----------------|------------|-------|--------|
| [1](001-3d-axis-explorer/) | 3D Axis Explorer | A world lens that drops a cube with red X, green Y and blue Z arrows. Pinch to move, rotate and scale it. | World tracking, 3D coordinates and transforms (Deck 1 spatial concepts) | [GUIDE](001-3d-axis-explorer/GUIDE.md) | Planned |
| [2](002-face-filter-name-badge/) | Face-Filter Name Badge | A head-tracked lens that shows the faculty member's name, department and a college logo. | Face tracking | [GUIDE](002-face-filter-name-badge/GUIDE.md) | Planned |
| [3](003-solar-system-on-the-table/) | Solar System on the Table | Surface tracking puts planets in orbit on a table, and tapping a planet shows a fact card. | Surface tracking, tap interaction | [GUIDE](003-solar-system-on-the-table/GUIDE.md) | Planned |
| [4](004-anatomy-overlay/) | Anatomy Overlay | Body or face tracking shows the skull, muscles or organs over the user. | Body and face tracking (Deck 4 healthcare examples) | [GUIDE](004-anatomy-overlay/GUIDE.md) | Planned |
| [5](005-marker-based-textbook/) | Marker-Based Textbook | Point at a printed diagram (a heart or an engine) and an animated 3D model pops up off the page. | Image marker tracking | [GUIDE](005-marker-based-textbook/GUIDE.md) | Planned |
| [6](006-ar-quiz-lens/) | AR Quiz Lens | Questions float above the user's head and they answer by tilting their head left or right. | Face tracking, head-gesture input | [GUIDE](006-ar-quiz-lens/GUIDE.md) | Planned |
| [7](007-virtual-try-on/) | Virtual Try-On | Glasses, a lab coat or a safety helmet on the user. | Face and body tracking (retail and industry use cases) | [GUIDE](007-virtual-try-on/GUIDE.md) | Planned |
| [8](008-heritage-site-portal/) | Portal to a Heritage Site | A doorway on the floor that you walk through into a 360° monument or museum scene. | World tracking, portals and 360° environments | [GUIDE](008-heritage-site-portal/GUIDE.md) | Planned |
| [9](009-chemistry-molecule-builder/) | Chemistry Molecule Builder | Tap to add atoms on a surface and watch bonds form, such as H₂O or CO₂. | Surface tracking, tap interaction, scripting | [GUIDE](009-chemistry-molecule-builder/GUIDE.md) | Planned |
| [10](010-campus-wayfinding-sign/) | Campus Wayfinding Sign | Point at a building marker to get floating arrows and info about labs and departments. | Image marker tracking, spatial UI | [GUIDE](010-campus-wayfinding-sign/GUIDE.md) | Planned |

## Folder layout

```
lens-studio/
  assets/              shared files for all lenses (download these first)
    models/            3D models (.fbx, .glb, .obj)
    textures/          images, HDRIs, markers
    audio/
    scripts/
  001-3d-axis-explorer/
    GUIDE.md           step-by-step guide
    guide-images/      screenshots used in the guide
    project/           finished Lens Studio project, for reference
  002-face-filter-name-badge/
  ...
  _lens-template/      copy this to start a new lens
```

## Before you start

- Install Lens Studio (each guide states the version it was written with).
- Install the Snapchat app on your phone to test lenses.
- If you clone with Git, run `git lfs install` first so large models and images download properly.
