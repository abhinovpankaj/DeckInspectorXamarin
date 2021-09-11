using Android.Graphics.Drawables;
using Android.Text.Method;
using Android.Views;
using Mobile.Code.Controls;

using Mobile.Code.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;


[assembly: ExportRenderer(typeof(XEditor), typeof(XEditorRenderer))]
namespace Mobile.Code.Droid.Renderer
{
    public class XEditorRenderer : EditorRenderer
    {
        public XEditorRenderer() : base(Application.Context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            }
            if (e.OldElement == null)
            {
                var nativeEditText = (global::Android.Widget.EditText)Control;

                //While scrolling inside Editor stop scrolling parent view.
                nativeEditText.OverScrollMode = OverScrollMode.Always;
                nativeEditText.ScrollBarStyle = ScrollbarStyles.InsideInset;
                nativeEditText.SetOnTouchListener(new DroidTouchListener());

                //For Scrolling in Editor innner area
                Control.VerticalScrollBarEnabled = true;
                Control.MovementMethod = ScrollingMovementMethod.Instance;
                Control.ScrollBarStyle = Android.Views.ScrollbarStyles.InsideInset;
                //Force scrollbars to be displayed
                Android.Content.Res.TypedArray a = Control.Context.Theme.ObtainStyledAttributes(new int[0]);
                InitializeScrollbars(a);
                a.Recycle();
            }

        }
    }
}