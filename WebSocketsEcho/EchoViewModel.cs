using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using Coe.WebSocketWrapper;

namespace WebSocketsEcho
{
	public class EchoViewModel : BaseViewModel
	{
		private Boolean bConnected;
		private WebSocketWrapper webSocketWrapper;
		private UTF8Encoding encoder = new UTF8Encoding();
		private TaskCompletionSource<bool> taskCompletionSource;

		public EchoViewModel (Page pg) : base (pg)
		{
			logItems = new ObservableCollection<String> ();
		}

		private ObservableCollection<String> logItems;
		public ObservableCollection<String> LogItems {
			get { return logItems; }
			set { SetProperty (ref logItems, value, () => LogItems); }
		}

		private string location;
		public string Location {
			get { return location; }
			set { 
				SetProperty (ref location, value, () => Location);
				EnableConnectButton = (!string.IsNullOrEmpty (location));
			}
		}

		private Boolean enableConnectButton = true;
		public Boolean EnableConnectButton {
			get { return enableConnectButton; }
			set { SetProperty (ref enableConnectButton, value, () => EnableConnectButton); }
		}

		private Boolean enableDisconnectButton = false;
		public Boolean EnableDisconnectButton {
			get { return enableDisconnectButton; }
			set { SetProperty (ref enableDisconnectButton, value, () => EnableDisconnectButton); }
		}

		private string message = string.Empty;
		public string Message
		{
			get { return message; }
			set { 
				SetProperty (ref message, value, () => Message);
				EnableSendButton = (!string.IsNullOrEmpty (message) && bConnected);
			}
		}

		private Boolean enableSendButton = false;
		public Boolean EnableSendButton {
			get { return enableSendButton; }
			set { SetProperty (ref enableSendButton, value, () => EnableSendButton); }
		}

		private Command connectCommand;
		public Command ConnectCommand
		{
			get
			{
				return connectCommand ?? (connectCommand = new Command(async () => await ExecuteConnectCommand()));
			}
		}

		private void OnConnect(WebSocketWrapper wrapper)
		{
			taskCompletionSource.SetResult (true);
			logItems.Clear ();
		}

		private void OnMessage(string messsageReceived, WebSocketWrapper wrapper)
		{
			logItems.Add (messsageReceived);
		}

		protected async Task ExecuteConnectCommand ()
		{
			try {
				WhatYaDoing = "Connecting...";
				IsBusy = true;
				webSocketWrapper = WebSocketWrapper.Create(location);
				webSocketWrapper.OnConnect(OnConnect);
				taskCompletionSource = new TaskCompletionSource<bool>();
				webSocketWrapper.Connect();
				await taskCompletionSource.Task;

				webSocketWrapper.OnMessage(OnMessage);
				SetConnected (true);

			} finally {
				IsBusy = false;
			}
		}

		private void OnDisconnect(WebSocketWrapper wrapper)
		{
			taskCompletionSource.SetResult (true);
		}

		private Command disconnectCommand;
		public Command DisconnectCommand
		{
			get
			{
				return disconnectCommand ?? (disconnectCommand = new Command(async () => await ExecuteDisconnectCommand()));
			}
		}

		protected async Task ExecuteDisconnectCommand ()
		{
			try {
				WhatYaDoing = "Disconnecting...";
				IsBusy = true;
				webSocketWrapper.OnDisconnect(OnDisconnect);
				taskCompletionSource = new TaskCompletionSource<bool>();
				webSocketWrapper.Disconnect();
				await taskCompletionSource.Task;

				SetConnected (false);

			} finally {
				IsBusy = false;
			}
		}

		private Command sendCommand;
		public Command SendCommand
		{
			get
			{
				return sendCommand ?? (sendCommand = new Command(async () => await ExecuteSendCommand()));
			}
		}

		protected async Task ExecuteSendCommand ()
		{
			if (!string.IsNullOrEmpty (message)) {
				byte[] buffer = encoder.GetBytes (message);
				webSocketWrapper.SendMessage (message);
			}
		}

		private Command clearLogCommand;
		public Command ClearLogCommand
		{
			get
			{
				return clearLogCommand ?? (clearLogCommand = new Command(async () => await ExecuteClearLogCommand()));
			}
		}

		protected async Task ExecuteClearLogCommand ()
		{
			logItems.Clear ();
		}
			
		protected override async void PageIsAppearing (object o, System.EventArgs e)
		{
			logItems.Clear ();
			Location = "ws://echo.websocket.org";
			// OnPropertyChanged won't fire unless the value actually changes
			SetConnected (true);
			SetConnected (false);
			EnableSendButton = true;
			EnableSendButton = false;
		}		

		private void SetConnected (Boolean connected)
		{
			bConnected = connected;
			EnableConnectButton = !connected;
			EnableDisconnectButton = connected;
			EnableSendButton = (!string.IsNullOrEmpty (message) && bConnected);
		}
	}
}
