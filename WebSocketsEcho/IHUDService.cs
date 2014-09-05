using System;

namespace WebSocketsEcho
{
	public interface IHUDService
	{
		void Show(); //shows the spinner
		void Show(string message); //show spinner + text
		void Dismiss();
	}
}

