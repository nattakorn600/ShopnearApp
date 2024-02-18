using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingPop
	{
		public LoadingPop()
		{
			InitializeComponent();
			Retext();
		}
		private async void Retext()
		{
			int count = 0;

			while (true)
			{
				if (count > 3)
				{
					count = 0;
				}
				else
				{
					if (count == 0)
					{
						Loading.Text = "Loading.   ";
					}
					if (count == 1)
					{
						Loading.Text = "Loading..  ";
					}
					if (count == 2)
					{
						Loading.Text = "Loading... ";
					}
					if (count == 3)
					{
						Loading.Text = "Loading....";
					}
					count++;
					await Task.Delay(500);
				}
			}
		}
	}
}