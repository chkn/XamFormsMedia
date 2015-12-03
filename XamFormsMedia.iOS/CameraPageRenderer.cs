using System;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;

using UIKit;
using Foundation;
using ObjCRuntime;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


namespace XamFormsMedia.iOS {

	public class CameraPageRenderer : PageRenderer
	{
		UIImagePickerController picker;

		public new CameraPage Element {
			get { return (CameraPage)base.Element; }
		}

		public CameraPageRenderer ()
		{
			picker = new UIImagePickerController {
				SourceType = UIImagePickerControllerSourceType.Camera,
				CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo
			};
			picker.Canceled += delegate {
				Element.OnCompleted ();
			};
			picker.FinishedPickingImage += (_, e) => {
				var dir = Runtime.GetNSObject<NSString> (NSTemporaryDirectory ()).ToString ();
				var path = Path.Combine (dir, "fartbum.png");
				e.Image.AsPNG ().Save (path);
				Element.Image = path;
				Element.OnCompleted ();
			};
		}

		protected override void OnElementChanged (VisualElementChangedEventArgs e)
		{
			base.OnElementChanged (e);
			if (e.OldElement != null) {
				e.OldElement.PropertyChanged -= HandlePropertyChanged;
			}
			if (e.NewElement != null) {
				e.NewElement.PropertyChanged += HandlePropertyChanged;
				UpdateBindings ();
			}
		}

		void HandlePropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			UpdateBindings ();
		}

		void UpdateBindings ()
		{
			switch (Element.PreferredCamera) {

			case CameraPreference.Front:
				picker.CameraDevice = UIImagePickerControllerCameraDevice.Front;
				break;

			case CameraPreference.Rear:
				picker.CameraDevice = UIImagePickerControllerCameraDevice.Rear;
				break;
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			AddChildViewController (picker);
			picker.View.Frame = View.Bounds;
			View.AddSubview (picker.View);
			picker.DidMoveToParentViewController (this);
		}

		[DllImport (Constants.FoundationLibrary)]
		static extern IntPtr NSTemporaryDirectory ();
	}
}

