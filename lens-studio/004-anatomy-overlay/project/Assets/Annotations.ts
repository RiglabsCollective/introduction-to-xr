/**
 * Floating name labels with a marker dot, from one script.
 *
 * Put this script on any one object (an empty "layer" object works well).
 * Add one slot to the "Annotations" list for every label you want:
 *   - Target:       the object the label points at (for example a point on a face)
 *   - Label Text:   the words on the label
 *   - Label Offset: where the label floats, as three numbers in centimetres:
 *                   X = the gap between the dot and the nearest edge of the text,
 *                       sideways and away from the centre of the object (the script
 *                       works out which way is "away" for each label; for a target
 *                       right on the centre line, a positive X goes right and a
 *                       negative X goes left)
 *                   Y = up
 *                   Z = towards the camera
 *
 * When the lens starts the script makes a 3D text label and a small dot for
 * every slot. Each frame the dot stays on its target, the label floats beside
 * it, and the label turns to face the camera so it can always be read.
 *
 * Because the labels are made by this script, switching the object off hides
 * all of its labels and dots at once. That makes it easy to build layers.
 */
@typedef
export class Annotation {
    @input
    @hint("The object the label points at.")
    target: SceneObject;

    @input
    @hint("The words shown on the label.")
    labelText: string = "";

    @input
    @hint("X: gap between the dot and the text, sideways. Y: up. Z: towards the camera. In centimetres.")
    labelOffset: vec3 = new vec3(4, 3, 2);
}

@component
export class Annotations extends BaseScriptComponent {
    @input
    @hint("The camera every label turns to face (usually Camera Object).")
    camera: Camera;

    @input
    @hint("The centre of the face or object. Labels fan out sideways away from it.")
    centerObject: SceneObject;

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
    labelScale: number = 0.4;

    @input
    @allowUndefined
    @hint("The mesh for the marker dots (a sphere). Leave it empty for no dots.")
    dotMesh: RenderMesh;

    @input
    @allowUndefined
    @hint("The material for the marker dots. Choose a colour that shows up.")
    dotMaterial: Material;

    @input
    @hint("Diameter of the marker dots, in centimetres.")
    dotSize: number = 0.8;

    @input
    @hint("One slot per label: the target, the words and where the label floats.")
    annotations: Annotation[];

    private entries: {
        target: SceneObject;
        label: SceneObject;
        text: Text3D;
        halfWidth: number;
        dot: SceneObject | null;
        offset: vec3;
    }[] = [];

    onAwake() {
        this.createEvent("OnStartEvent").bind(() => this.build());
        this.createEvent("UpdateEvent").bind(() => this.follow());
    }

    private build() {
        if (!this.annotations) {
            return;
        }
        for (const slot of this.annotations) {
            if (!slot.target) {
                continue;
            }
            const label = global.scene.createSceneObject("Label " + slot.labelText);
            label.setParent(this.sceneObject);
            const text = label.createComponent("Component.Text3D");
            text.text = slot.labelText;
            text.size = this.fontSize;
            if (this.font) {
                text.font = this.font;
            }
            if (this.labelMaterial) {
                text.mainMaterial = this.labelMaterial;
            }

            let dot: SceneObject | null = null;
            if (this.dotMesh && this.dotMaterial) {
                dot = global.scene.createSceneObject("Dot " + slot.labelText);
                dot.setParent(this.sceneObject);
                const visual = dot.createComponent("Component.RenderMeshVisual");
                visual.mesh = this.dotMesh;
                visual.mainMaterial = this.dotMaterial;
            }
            this.entries.push({
                target: slot.target,
                label: label,
                text: text,
                halfWidth: 0,
                dot: dot,
                offset: slot.labelOffset,
            });
        }
    }

    private follow() {
        if (!this.camera) {
            return;
        }
        const camT = this.camera.getTransform();
        const camPos = camT.getWorldPosition();
        const camRight = camT.right;
        const camUp = camT.up;
        const centrePos = this.centerObject
            ? this.centerObject.getTransform().getWorldPosition()
            : null;
        const labelSize = new vec3(this.labelScale, this.labelScale, this.labelScale);
        const dotSize = new vec3(this.dotSize, this.dotSize, this.dotSize);

        for (const entry of this.entries) {
            const targetPos = entry.target.getTransform().getWorldPosition();

            // Which way is "away from the centre" for this label? Use the camera's
            // sideways direction so it works however the face is turned.
            let side = 0;
            if (centrePos) {
                const across = targetPos.sub(centrePos).dot(camRight);
                side = across > 0.3 ? 1 : across < -0.3 ? -1 : 0;
            }
            const toCamera = camPos.sub(targetPos);
            const towards = toCamera.length > 0.001 ? toCamera.normalize() : vec3.zero();

            // The text is centred on the label, so to keep the same small gap
            // between the dot and the text edge for every label, move the label
            // out by half of its own width plus the gap. The width is measured
            // once the text has been laid out.
            if (entry.halfWidth === 0) {
                const box = entry.text.getBoundingBox();
                entry.halfWidth = ((box.right - box.left) / 2) * this.labelScale;
            }
            // A target on the centre line has no "away", so the sign of its X
            // offset decides: positive goes right, negative goes left.
            const direction = side !== 0 ? side : entry.offset.x < 0 ? -1 : 1;
            const sideways = direction * (Math.abs(entry.offset.x) + entry.halfWidth);

            const labelPos = targetPos
                .add(camRight.uniformScale(sideways))
                .add(camUp.uniformScale(entry.offset.y))
                .add(towards.uniformScale(entry.offset.z));

            const lt = entry.label.getTransform();
            lt.setWorldPosition(labelPos);
            lt.setWorldScale(labelSize);
            const toCam = camPos.sub(labelPos);
            if (toCam.length > 0.001) {
                lt.setWorldRotation(quat.lookAt(toCam.normalize(), vec3.up()));
            }

            if (entry.dot) {
                const dt = entry.dot.getTransform();
                dt.setWorldPosition(targetPos);
                dt.setWorldScale(dotSize);
            }
        }
    }
}
