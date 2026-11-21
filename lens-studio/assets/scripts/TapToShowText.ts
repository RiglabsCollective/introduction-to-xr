/**
 * Show a message in a Text component when this object is tapped.
 *
 * Put this on an object that also has an Interaction component (the
 * Interaction decides which meshes can be tapped). Point "Text" at the Text
 * component that should show the message. Use one copy per object, each with
 * its own title and message.
 *
 * Works in any lens: planets, buttons, labelled parts of a model.
 */
@component
export class TapToShowText extends BaseScriptComponent {
    @input
    @hint("The Text component that shows the message (for example a Screen Text).")
    textComponent: Text;

    @input
    @hint("First line of the message, usually the name of the object.")
    title: string = "";

    @input
    @widget(new TextAreaWidget())
    @hint("The text shown under the title.")
    message: string = "";

    onAwake() {
        const interaction = this.sceneObject.getComponent(
            "Component.InteractionComponent"
        );
        if (!interaction) {
            print("TapToShowText: no Interaction component on " + this.sceneObject.name);
            return;
        }
        interaction.onTap.add(() => {
            if (!this.textComponent) {
                return;
            }
            this.textComponent.text = this.title + "\n" + this.message;
        });
    }
}
