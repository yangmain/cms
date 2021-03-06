﻿using System;
using Datory;
using Datory.Annotations;
using Newtonsoft.Json;

namespace SSCMS.Abstractions
{
    [DataTable("siteserver_User")]
    public class User : Entity
    {
        [DataColumn]
        public string UserName { get; set; }

        [DataColumn]
        [JsonIgnore]
        public string Password { get; set; }

        [DataColumn]
        [JsonIgnore]
        public PasswordFormat PasswordFormat { get; set; }

        [DataColumn]
        [JsonIgnore]
        public string PasswordSalt { get; set; }

        [DataColumn]
        public DateTime? CreateDate { get; set; }

        [DataColumn]
        public DateTime? LastResetPasswordDate { get; set; }

        [DataColumn]
        public DateTime? LastActivityDate { get; set; }

        [DataColumn]
        public int GroupId { get; set; }

        [DataColumn]
        public int CountOfLogin { get; set; }

        [DataColumn]
        public int CountOfFailedLogin { get; set; }

        [DataColumn]
        public bool Checked { get; set; }

        [DataColumn]
        public bool Locked { get; set; }

        [DataColumn]
        public string DisplayName { get; set; }

        [DataColumn]
        public string Mobile { get; set; }

        [DataColumn]
        public string Email { get; set; }

        [DataColumn]
        public string AvatarUrl { get; set; }

        [DataColumn]
        public string Gender { get; set; }

        [DataColumn]
        public string Birthday { get; set; }

        [DataColumn]
        public string WeiXin { get; set; }

        [DataColumn]
        public string Qq { get; set; }

        [DataColumn]
        public string WeiBo { get; set; }

        [DataColumn(Text = true)]
        public string Bio { get; set; }
    }
}