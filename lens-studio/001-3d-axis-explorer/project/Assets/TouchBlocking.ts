/**
 * Stop Snapchat from reacting to touches while this lens is running.
 *
 * Without this, swiping left or right opens Chat or Stories instead of
 * reaching the lens, so drag and swipe gestures on your objects appear
 * to do nothing.
 *
 * Put this on any object (an empty one named "Touch Blocking" works well).
 * It needs no inputs and works in any lens.
 */
@component
export class TouchBlocking extends BaseScriptComponent {
    onAwake() {
        this.createEvent("OnStartEvent").bind(() => {
            global.touchSystem.touchBlocking = true;
        });
    }
}
