using SignaturePad.Forms;
using Xamarin.Forms;

namespace ImageEditor.Behaviors
{
    internal class ScratchView : Behavior<SignaturePadView>
    {
        protected override void OnAttachedTo(SignaturePadView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.SignatureLine.IsVisible = false;
            bindable.CaptionLabel.IsVisible = false;
            bindable.SignaturePrompt.IsVisible = false;
            bindable.ClearLabel.IsEnabled = false;
            bindable.ClearLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
        }
        protected override void OnDetachingFrom(SignaturePadView bindable)
        {
            base.OnDetachingFrom(bindable);
        }
    }
}
