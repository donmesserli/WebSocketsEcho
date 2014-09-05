using System;
using Xamarin.Forms;

namespace WebSocketsEcho
{
	public class App
	{
		public static Page GetMainPage ()
		{	
			return new NavigationPage (new EchoPage ());
		}
	}
}

