﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeDateCalculatorDll
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutHelp : ContentPage
	{
		public AboutHelp()
		{
			InitializeComponent();
		}

		private async void OKButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}
	}
}