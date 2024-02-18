using Newtonsoft.Json;
using Shopnear.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shopnear.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AgentCard
	{
		public AgentCard()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			AgentImg.Source = Application.Current.Properties["shop_agent_card"].ToString();
        }
    }
}