using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace WebSocketsEcho
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
		protected Page page;

		public BaseViewModel(Page pg)
        {
			page = pg;
			page.Appearing += PageIsAppearing;
			this.IsBusyChanged += HandleIsBusyChanged;
        }

		protected virtual async void PageIsAppearing (object o, System.EventArgs e)
		{

		}					

        private string title = string.Empty;

        /// <summary>
        /// Gets or sets the "Title" property
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return title; }
			set { SetProperty (ref title, value, () => Title); }
        }

        private string subTitle = string.Empty;
        /// <summary>
        /// Gets or sets the "Subtitle" property
        /// </summary>
        public string Subtitle
        {
            get { return subTitle; }
			set { SetProperty (ref subTitle, value, () => Subtitle); }
        }

        private string icon = null;
        /// <summary>
        /// Gets or sets the "Icon" of the viewmodel
        /// </summary>
        public string Icon
        {
            get { return icon; }
			set { SetProperty (ref icon, value, () => Icon); }
        }

		protected void SetProperty<U>(ref U backingStore, U value, Expression<Func<U>> property)
		{
			var memberExpression = property.Body as MemberExpression;

			string propertyName = memberExpression.Member.Name;

			SetProperty (ref backingStore, value, propertyName);
		}

        protected void SetProperty<U>(
            ref U backingStore, U value,
            string propertyName,
            Action onChanged = null,
            Action<U> onChanging = null)
        {
            if (EqualityComparer<U>.Default.Equals(backingStore, value))
                return;

            if (onChanging != null)
                onChanging(value);

            OnPropertyChanging(propertyName);

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }

        #region INotifyPropertyChanging implementation
        public event Xamarin.Forms.PropertyChangingEventHandler PropertyChanging ;
        #endregion

        public void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging == null)
                return;

            PropertyChanging(this, new Xamarin.Forms.PropertyChangingEventArgs(propertyName));
        }


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

		public event BusyEventDelegate IsBusyChanged;
		public delegate void BusyEventDelegate(object sender, BusyEventArgs _args);

		private string whatYaDoing;
		public string WhatYaDoing {
			get {
				return whatYaDoing;
			}
			protected set {
				whatYaDoing = value;
			}
		}

		bool isBusy = false;

		public bool IsBusy
		{
			get { return isBusy; }
			set
			{
				if (isBusy != value) {
					isBusy = value;

					//if (isBusy == false)
						//WhatYaDoing = null;
						//RaisePropertyChanged(() => IsBusy);
					//OnPropertyChanged ("IsBusy");
						OnIsBusyChanged (isBusy);
				}
			}
		}

		protected virtual void OnIsBusyChanged (bool busy)
		{
			var ev = IsBusyChanged;
			if (ev != null) {
				ev (this, new BusyEventArgs (busy, WhatYaDoing));
					//EventArgs.Empty);
			}
		}

		public class BusyEventArgs : EventArgs
		{
			private bool busy;
			private string message;

			public BusyEventArgs(bool busy, string msg)
			{
				this.busy = busy;
				message = msg;
			}

			public bool Busy {
				get { return busy; }
			}

			public string Message {
				get { return message; }
			}
		}
		void HandleIsBusyChanged (object sender, BaseViewModel.BusyEventArgs e)
		{
			IsBusy = e.Busy;

			if (IsBusy) {
				DependencyService.Get<IHUDService> ().Show (e.Message);
			} else {
				DependencyService.Get<IHUDService> ().Dismiss ();
			}
		}
    }
}
