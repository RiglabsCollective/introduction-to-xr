# Shared assets

Files used across the lens guides. Each guide lists exactly which files it needs.

## Models and textures

| File | Used in | Source / licence |
|------|---------|------------------|
| `models/axis-cube.glb` | Not used by a lens now | Made for this course, free to reuse |
| `models/solar-system.fbx` | 003 Solar System on the Table | "Solar System" by NANDA KUMAR ([@Nandakumar000](https://sketchfab.com/Nandakumar000)) on Sketchfab, licensed under [CC Attribution 4.0](http://creativecommons.org/licenses/by/4.0/). [Original model](https://sketchfab.com/3d-models/solar-system-b6b69a95a6f0426bb8bbc2e8cb7ff46a) |
| `models/heart.fbx` | 004 Anatomy Overlay | "Realistic Human Heart" by neshallads ([@neshallads](https://sketchfab.com/neshallads)) on Sketchfab, licensed under [CC Attribution 4.0](http://creativecommons.org/licenses/by/4.0/). The picture textures are inside the model file. [Original model](https://sketchfab.com/3d-models/realistic-human-heart-3f8072336ce94d18b3d0d055a1ece089) |
| `textures/solar-system/` | 003 Solar System on the Table | Planet and Moon images that come with the "Solar System" model above (same author and licence) |
| `textures/department-of-xr.png` | 002 Face-Filter Name Badge | Department of XR logo, made for this course |

## Scripts

Reusable scripts. Each works on any object in any lens. Drag the file into the Asset Browser of your project, then add it to an object as a **Script** component.

| File | What it does | Used in |
|------|--------------|---------|
| `scripts/OneFingerSpin.ts` | Spins an object when you swipe one finger across it | 001, 004 |
| `scripts/TouchBlocking.ts` | Stops Snapchat from taking over swipes (Chat, Stories) | 001 |
| `scripts/TapToShowText.ts` | Shows a message in a text box when an object is tapped | Not used by a lens yet |
| `scripts/PlanetProperties.ts` | Makes a set of planets orbit at real speed ratios and floats a name label above each one. One script, one list with a slot per planet | 003 |
| `scripts/Annotations.ts` | Floating name labels with a dot, from one list: a target, the words and where the label floats. Labels face the camera | 004 |
