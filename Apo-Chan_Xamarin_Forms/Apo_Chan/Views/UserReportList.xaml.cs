﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Apo_Chan.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserReportList : ContentPage
    {
        public UserReportList()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            reportList.SelectedItem = null;
        }
    }
}