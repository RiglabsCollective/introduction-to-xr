/**
 * One-finger drag to spin (trackball style).
 *
 * Put this on the same object as an Interaction component (and, optionally,
 * a Manipulate component for pinch-to-zoom / two-finger twist).
 *
 * Dragging one finger across the object rotates it as if you were rolling a
 * ball under your finger, relative to the camera view:
 *   - left/right drag spins it around the camera's up direction
 *   - up/down drag tumbles it around the camera's right direction
 *   - diagonal drags do both, so the object can reach any orientation
 * The two-finger twist from the Manipulate component covers the third
 * (roll) direction.
 *
 * As soon as a second finger touches, spinning pauses so pinch and twist
 * from the Manipulate component work without fighting this script.
 */
@component
export class OneFingerSpin extends BaseScriptComponent {
    @input
    @hint("The camera that renders this object (usually Camera Object).")
    camera: Camera;

    @input
    @hint("Degrees of spin for a drag across the full width (or height) of the screen.")
    degreesPerScreenWidth: number = 360;

    @input
    @hint("On: drag in any direction to tumble freely. Off: left/right only (turntable).")
    freeRotation: boolean = true;

    private interaction: InteractionComponent;
    private activeTouches: { [id: number]: boolean } = {};
    private touchCount = 0;
    private spinTouchId = -1;
    private lastPos = new vec2(0, 0);

    onAwake() {
        this.interaction = this.sceneObject.getComponent(
            "Component.InteractionComponent"
        );
        if (!this.interaction) {
            print("OneFingerSpin: no Interaction component on " + this.sceneObject.name);
            return;
        }

        this.interaction.onTouchStart.add((e) => {
            if (!this.activeTouches[e.touchId]) {
                this.activeTouches[e.touchId] = true;
                this.touchCount++;
            }
            if (this.touchCount === 1) {
                this.spinTouchId = e.touchId;
                this.lastPos = e.position;
            } else {
                this.spinTouchId = -1;
            }
        });

        this.interaction.onTouchMove.add((e) => {
            if (this.touchCount !== 1) {
                return;
            }
            if (this.spinTouchId !== e.touchId) {
                // Coming back from two fingers to one: start fresh from here
                // so the object doesn't jump.
                this.spinTouchId = e.touchId;
                this.lastPos = e.position;
                return;
            }
            const dx = e.position.x - this.lastPos.x;
            const dy = e.position.y - this.lastPos.y;
            this.lastPos = e.position;
            if (dx !== 0 || dy !== 0) {
                this.spin(dx, this.freeRotation ? dy : 0);
            }
        });

        this.interaction.onTouchEnd.add((e) => {
            if (this.activeTouches[e.touchId]) {
                delete this.activeTouches[e.touchId];
                this.touchCount = Math.max(0, this.touchCount - 1);
            }
            this.spinTouchId = -1;
        });
    }

    private spin(dx: number, dy: number) {
        const k = this.degreesPerScreenWidth * MathUtils.DegToRad;
        let up = vec3.up();
        let right = vec3.right();
        if (this.camera) {
            const camT = this.camera.getTransform();
            up = camT.up;
            right = camT.right;
        }
        // Screen y grows downward, so dragging down tips the top toward you.
        const yaw = quat.angleAxis(dx * k, up);
        const pitch = quat.angleAxis(dy * k, right);
        const t = this.getTransform();
        t.setWorldRotation(pitch.multiply(yaw).multiply(t.getWorldRotation()));
    }
}
