using Android.Content;
using Android.Graphics;
using ImageEditor.Controls;
using Mobile.Code.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EditorControl), typeof(EditorControlRenderer))]
namespace Mobile.Code.Droid.Renderers
{
    public class EditorControlRenderer : EditorRenderer
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public EditorControlRenderer()
        {
        }
#pragma warning restore CS0618 // Type or member is obsolete

        public EditorControlRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            Control.Background.SetColorFilter(Android.Graphics.Color.Transparent, PorterDuff.Mode.SrcIn);
        }
    }
}