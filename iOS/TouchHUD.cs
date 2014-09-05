using System;
using BigTed;
using Xamarin.Forms;
using WebSocketsEcho;

[assembly: Dependency (typeof (TouchHUD))]

namespace WebSocketsEcho
{
	public class TouchHUD : IHUDService
	{
		public TouchHUD ()
		{
			BTProgressHUD.ForceiOS6LookAndFeel = true;
		}

		public void Show()
		{
			BTProgressHUD.Show ();
		}

		public void Show(string message)
		{
			BTProgressHUD.Show (message);
		}

		public void Dismiss()
		{
			BTProgressHUD.Dismiss ();
		}

	}
}

