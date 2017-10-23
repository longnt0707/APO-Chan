﻿using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;
using System.Runtime.Serialization;
using Xamarin.Auth;
using Xamarin.Forms;
using Apo_Chan.Models;
using Apo_Chan.Managers;
using Newtonsoft.Json.Linq;

namespace Apo_Chan.Items
{
    [DataContract(Name = "group")]
    public class GroupItem : BaseItem
    {

        private string groupKey;
        private string groupName;
        private string createdUserId;
        private int userCount;
        private string userAuth;
        private CustomImageSource groupImage;

        [JsonProperty(PropertyName = "groupKey")]
        public string GroupKey { get
            {
                return this.groupKey;
            }
            set
            {
                SetProperty(ref this.groupKey, value);
            }
        }

        [JsonProperty(PropertyName = "groupName")]
        public string GroupName
        {
            get
            {
                return this.groupName;
            }
            set
            {
                SetProperty(ref this.groupName, value);
            }
        }

        [JsonProperty(PropertyName = "createdUserId")]
        public string CreatedUserId
        {
            get
            {
                return this.createdUserId;
            }
            set
            {
                SetProperty(ref this.createdUserId, value);
            }
        }

        public CustomImageSource GroupImage
        {
            get
            {
                if (this.groupImage == null)
                {
                    return CustomImageSource.FromFile(Constants.IconGropName);
                }
                return this.groupImage;
            }
            set
            {
                SetProperty(ref this.groupImage, value);
            }
        }

        public bool HasImage
        {
            get
            {
                return this.groupImage != null;
            }
        }

        /// <summary>
        /// Count users Joined to group
        /// </summary>
        public int UserCount
        {
            get
            {
                return this.userCount;
            }
            set
            {
                SetProperty(ref this.userCount, value);
            }
        }

        public string UserAuth
        {
            get
            {
                return this.userAuth;
            }
            set
            {
                SetProperty(ref this.userAuth, value);
            }
        }
    }

    /// <summary>
    /// for grouplist view. has group, count, auth(target user).
    /// </summary>
    public class GroupAndUserCountItem
    {
        [JsonProperty(PropertyName = "group")]
        public GroupItem Group { get; set; }

        [JsonProperty(PropertyName = "usercount")]
        public int UserCount { get; set; }

        [JsonProperty(PropertyName = "adminflg")]
        public bool AdminFlg { get; set; }
    }
    /// <summary>
    /// for detailgroup view. has group, List(Tuple(User, Groupuser)).
    /// </summary>
    public class GroupAndGroupUsersItem
    {
        [JsonProperty(PropertyName = "group")]
        public GroupItem Group { get; set; }

        [JsonProperty(PropertyName = "groupusers")]
        public List<GroupUserItem> GroupUsers { get; set; }
    }
}
