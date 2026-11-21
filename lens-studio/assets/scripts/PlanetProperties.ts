/**
 * Orbits and name labels for a whole solar system, from one script.
 *
 * Put this script on any one object (the Sun works well). Set the global
 * settings once, then add one slot to the "Planets" list for each object:
 *
 *   Planet Object          the planet (or moon) itself
 *   Planet Name            the words on its floating label
 *   Label Offset           how far above the planet the label floats, in cm
 *   Orbit Period In Years  how long one orbit takes, in Earth years
 *                          (leave it at 0 for an object that doesn't orbit, like the Sun)
 *
 * Orbits: put each planet inside the object it orbits (the Sun for a planet,
 * a planet for its moon) and move it sideways from the centre. The script then
 * swings the planet round its parent's centre, around the Y axis. The speeds
 * keep the real ratios between the planets: Mercury goes round about four
 * times for every Earth orbit and Neptune takes 165 years.
 *
 * Labels: the script makes a 3D text label for every slot when the lens
 * starts. Each frame the label moves above its planet and turns to face the
 * camera so the name can always be read.
 */
@typedef
export class PlanetSlot {
    @input
    @hint("The planet (or moon) itself. It orbits the centre of its parent object.")
    planetObject: SceneObject;

    @input
    @hint("The words shown on the label above the planet.")
    planetName: string = "";

    @input
    @hint("How far above the planet the label floats, in centimetres.")
    labelOffset: number = 3;

    @input
    @hint("How long one orbit takes, in Earth years. Mercury 0.24, Earth 1, Neptune 164.8. Use 0 for no orbit.")
    orbitPeriodInYears: number = 0;
}

@component
export class PlanetProperties extends BaseScriptComponent {
    @input
    @hint("How many seconds one Earth year lasts in the lens. Smaller is faster.")
    secondsPerEarthYear: number = 30;

    @input
    @hint("1 turns counter-clockwise seen from above, -1 turns clockwise.")
    direction: number = 1;

    @input
    @hint("The camera every label turns to face (usually Camera Object).")
    camera: Camera;

    @input
    @allowUndefined
    @hint("The font used for every label. Leave it empty to use the default font.")
    font: Font;

    @input
    @allowUndefined
    @hint("The material used for every label. Leave it empty for the default text material.")
    labelMaterial: Material;

    @input
    @hint("Font size of the label text.")
    fontSize: number = 48;

    @input
    @hint("Size of every label in the world. Raise it to make all the labels bigger.")
    labelScale: number = 3;

    @input
    @hint("One slot per planet: the object, its name, the label height and the orbit time.")
    planets: PlanetSlot[];

    private entries: {
        label: SceneObject;
        planet: SceneObject;
        offset: number;
        period: number;
    }[] = [];

    onAwake() {
        this.createEvent("OnStartEvent").bind(() => this.build());
        this.createEvent("UpdateEvent").bind(() => this.update());
    }

    private build() {
        if (!this.planets) {
            return;
        }
        for (const slot of this.planets) {
            if (!slot.planetObject) {
                continue;
            }
            const label = global.scene.createSceneObject("Label " + slot.planetName);
            label.setParent(this.sceneObject);
            const text = label.createComponent("Component.Text3D");
            text.text = slot.planetName;
            text.size = this.fontSize;
            if (this.font) {
                text.font = this.font;
            }
            if (this.labelMaterial) {
                text.mainMaterial = this.labelMaterial;
            }
            this.entries.push({
                label: label,
                planet: slot.planetObject,
                offset: slot.labelOffset,
                period: slot.orbitPeriodInYears,
            });
        }
    }

    private update() {
        const dt = getDeltaTime();
        const scale = new vec3(this.labelScale, this.labelScale, this.labelScale);
        const camPos = this.camera ? this.camera.getTransform().getWorldPosition() : null;

        for (const entry of this.entries) {
            // Orbit: swing the planet round its parent's centre, around the Y axis.
            const seconds = entry.period * this.secondsPerEarthYear;
            if (seconds > 0) {
                const radians = (2 * Math.PI * dt) / seconds;
                const pt = entry.planet.getTransform();
                const turn = quat.angleAxis(radians * this.direction, vec3.up());
                pt.setLocalPosition(turn.multiplyVec3(pt.getLocalPosition()));
            }

            // Label: float above the planet and face the camera.
            const t = entry.label.getTransform();
            const pos = entry.planet
                .getTransform()
                .getWorldPosition()
                .add(new vec3(0, entry.offset, 0));
            t.setWorldPosition(pos);
            t.setWorldScale(scale);
            if (camPos) {
                const toCamera = camPos.sub(pos);
                if (toCamera.length > 0.001) {
                    t.setWorldRotation(quat.lookAt(toCamera.normalize(), vec3.up()));
                }
            }
        }
    }
}
