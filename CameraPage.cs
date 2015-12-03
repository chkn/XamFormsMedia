using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace XamFormsMedia {

	public enum CameraPreference {
		Rear,
		Front
	}

	public class CameraPage : Page {

		public static readonly BindableProperty PreferredCameraProperty = BindableProperty.Create (nameof (PreferredCamera), typeof (CameraPreference), typeof (CameraPage), default (CameraPreference));

		internal static readonly BindablePropertyKey ImagePropertyKey  = BindableProperty.CreateReadOnly (nameof (Image), typeof (FileImageSource), typeof (CameraPage), null);
		public static readonly BindableProperty ImageProperty = ImagePropertyKey.BindableProperty;

		public CameraPreference PreferredCamera {
			get { return (CameraPreference) GetValue (PreferredCameraProperty); }
			set { SetValue (PreferredCameraProperty, value); }
		}

		public FileImageSource Image {
			get { return (FileImageSource) GetValue (ImageProperty); }
			internal set { SetValue (ImagePropertyKey, value); }
		}

		public event EventHandler Completed;

		protected internal virtual void OnCompleted ()
		{
			var completed = Completed;
			if (completed != null)
				completed (this, EventArgs.Empty);
		}

	}
}

