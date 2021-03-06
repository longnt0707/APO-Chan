﻿using Apo_Chan.Items;
using Apo_Chan.Managers;
using Apo_Chan.Models;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;

namespace Apo_Chan.ViewModels
{
    public class DetailGroupViewModel : BaseGroupViewModel, INavigatedAware
    {
        #region Variable and Property
        
        public DelegateCommand DeleteGroupCommand { get; private set; }
        #endregion

        #region Constructor
        public DetailGroupViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            DeleteGroupCommand = new DelegateCommand(deleteGroup);
        }
        #endregion

        #region Function

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            ;
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            this.GroupUserItems.Clear();
            var allGroupUserItems = new ObservableCollection<GroupUserItem>();
            if (parameters.ContainsKey("Id"))
            {
                IsBusy = true;
                try
                {
                    // get group and groupusers
                    //var item = await CustomFunction.Get<GroupAndGroupUsersItem>($"api/values/groupusers/{(string)parameters["Id"]}");
                    var item = await GroupUserManager.DefaultManager.GetGroupAndGroupUsers((string)parameters["Id"]);
                    if (item != null)
                    {
                        foreach (var user in item.GroupUsers)
                        {
                            if(user != null)
                            {
                                allGroupUserItems.Add(user);
                                // if userself item, set adminflg
                                if(user.RefUserId == GlobalAttributes.User.Id)
                                {
                                    item.Group.IsUserAdmin = user.AdminFlg;
                                }
                                // Add group user image
                                await Service.ImageService.SetImageSource(user.RefUser);
                            }
                        }
                        this.Group = item.Group;
                        await Service.ImageService.SetImageSource(this.Group);
                    }
                }
                catch (Exception e)
                {

                    DebugUtil.WriteLine("DetailGroupViewModel > " + e.Message);
                }
                this.GroupUserItems = allGroupUserItems;
                IsBusy = false;
            }
            else
            {
                await dialogService.DisplayAlertAsync("Error", "Failed to load the detail page!", "OK");
                await navigationService.GoBackAsync();
            }
        }

        
        
        private async void deleteGroup()
        {
            var accepted = await dialogService.DisplayAlertAsync
                (
                    "Confirmation",
                    "Warning! You are about to delete a group. Do you want to continue?",
                    "Confirm",
                    "Cancel"
                );
            if (accepted)
            {
                if (!GlobalAttributes.IsConnectedInternet)
                {
                    await dialogService.DisplayAlertAsync("Error", "This feature only available with network access!", "OK");
                    return;
                }
                IsBusy = true;
                try
                {
                    //### delete relational records of GroupUser & ReportGroup then delete the group
                    await ReportGroupManager.DefaultManager.DeleteGroupAsync(Group);
                    await GroupUserManager.DefaultManager.DeleteGroupAsync(Group);
                    await GroupManager.DefaultManager.DeleteAsync(Group);

                    //### sync with server immediately
                    await Service.OfflineSync.PerformAlInOneSync();
                }
                catch (Exception e)
                {
                    DebugUtil.WriteLine("DetailGroupViewModel > " + e.Message);
                }
                await navigationService.GoBackAsync();
                IsBusy = false;
            }
        }

        protected override void imageSelect()
        {
            if (this.Group.IsUserNotAdmin)
            {
                return;
            }
            base.imageSelect();
        }

        #endregion
    }
}
