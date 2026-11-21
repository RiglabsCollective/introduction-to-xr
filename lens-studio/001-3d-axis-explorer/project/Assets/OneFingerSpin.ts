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
 * Set "Max Tilt Degrees" (for example 25) to stop the object from tipping
 * over: turning left and right stays free, but up and down tilt is limited
 * to that many degrees either way from upright.
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

    @input
    @hint("How far the object may tilt away from upright, in degrees, so it can't flip over. 180 means no limit. 25 keeps it within 25 degrees.")
    maxTiltDegrees: number = 180;

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
        let next = pitch.multiply(yaw).multiply(t.getWorldRotation());

        // Keep the object from tilting too far: measure how far its up direction
        // has leaned away from the world's up, and swing it back if it is past the limit.
        if (this.maxTiltDegrees < 180) {
            const objectUp = next.multiplyVec3(vec3.up());
            const cosTilt = Math.min(1, Math.max(-1, objectUp.dot(vec3.up())));
            const tilt = Math.acos(cosTilt) * MathUtils.RadToDeg;
            if (tilt > this.maxTiltDegrees) {
                const axis = objectUp.cross(vec3.up());
                if (axis.length > 0.0001) {
                    const back = quat.angleAxis(
                        (tilt - this.maxTiltDegrees) * MathUtils.DegToRad,
                        axis.normalize()
                    );
                    next = back.multiply(next);
                }
            }
        }
        t.setWorldRotation(next);
    }
}
