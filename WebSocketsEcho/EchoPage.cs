using System;
using Xamarin.Forms;

namespace WebSocketsEcho
{
	public class EchoPage : ContentPage
	{
		private EchoViewModel viewModel;

		public EchoPage ()
		{
			viewModel = new EchoViewModel (this);

			BindingContext = viewModel;

			Title = "WebSocket Echo";
			Padding = new Thickness(10);

			BoxView topBox = new BoxView {
				BackgroundColor = Xamarin.Forms.Color.Transparent,
				HeightRequest = 20
			};

			Label locationLabel = new Label {
				Text = "Location:",
				Font = Font.SystemFontOfSize (18),
				TextColor = Xamarin.Forms.Color.Black,
				WidthRequest = 250,
				HeightRequest = 30,
			};

			Entry location = new Entry {
				WidthRequest = 280
			};
			location.SetBinding<EchoViewModel> (Entry.TextProperty, m => m.Location);

			Button connectButton = new Button {
				Text = "Connect",
				Font = Font.SystemFontOfSize (18),
			};
			connectButton.SetBinding<EchoViewModel>(View.IsEnabledProperty, m => m.EnableConnectButton);
			connectButton.SetBinding<EchoViewModel> (Button.CommandProperty, m => m.ConnectCommand);

			Button disconnectButton = new Button {
				Text = "Disconnect",
				Font = Font.SystemFontOfSize (18),
			};
			disconnectButton.SetBinding<EchoViewModel>(View.IsEnabledProperty, m => m.EnableDisconnectButton);
			disconnectButton.SetBinding<EchoViewModel> (Button.CommandProperty, m => m.DisconnectCommand);

			StackLayout buttons = new StackLayout () {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Start,
				Spacing = 40,
				Padding = new Thickness (20, 0, 0, 0),
				Children = {
					connectButton,
					disconnectButton
				}
			};

			Label messageLabel = new Label {
				Text = "Message:",
				Font = Font.SystemFontOfSize (18),
				TextColor = Xamarin.Forms.Color.Black,
				WidthRequest = 250,
				HeightRequest = 30,
			};

			Entry message = new Entry {
				WidthRequest = 280
			};
			message.SetBinding<EchoViewModel> (Entry.TextProperty, m => m.Message);

			Button sendButton = new Button {
				Text = "Send",
				Font = Font.SystemFontOfSize (18),
			};
			sendButton.SetBinding<EchoViewModel>(View.IsEnabledProperty, m => m.EnableSendButton);
			sendButton.SetBinding<EchoViewModel> (Button.CommandProperty, m => m.SendCommand);

			StackLayout sendButtonContainer = new StackLayout () {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Start,
				Spacing = 40,
				Padding = new Thickness (20, 0, 0, 0),
				Children = {
					sendButton
				}
			};

			Label logLabel = new Label {
				Text = "Log:",
				Font = Font.SystemFontOfSize (18),
				TextColor = Xamarin.Forms.Color.Black,
				WidthRequest = 40,
				HeightRequest = 30,
			};

			Button clearLogButton = new Button {
				Text = "Clear Log",
				Font = Font.SystemFontOfSize (18),
				HeightRequest = 24,
			};
			clearLogButton.SetBinding<EchoViewModel> (Button.CommandProperty, m => m.ClearLogCommand);

			StackLayout logStuffContainer = new StackLayout () {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center,
				//Spacing = 40,
				Padding = new Thickness (20, 0, 0, 0),
				Children = {
					logLabel,
					clearLogButton
				}
			};

			ListView log = new ListView () {
				WidthRequest = 250,
				HeightRequest = 100,
			};
			log.SetBinding<EchoViewModel>(ListView.ItemsSourceProperty, m => m.LogItems, BindingMode.TwoWay);

			StackLayout logContainer = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				Children =
				{
					log
				},
				BackgroundColor = Color.Black,
				Padding = 5,
			};

			Content = new ScrollView() { 
				Content = new StackLayout {
					Spacing = 5,
					VerticalOptions = LayoutOptions.FillAndExpand,
					//HorizontalOptions = LayoutOptions.Center,
					Children = { 
						//topBox,
						locationLabel,
						location,
						buttons,
						messageLabel,
						message,
						sendButtonContainer,
						logStuffContainer,
						logContainer
					}
				}
			};

		}
	}
}

