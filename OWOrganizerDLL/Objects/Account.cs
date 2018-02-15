using System;
using System.Collections.Generic;
using System.Text;

namespace OWOrganizerDLL.Objects
{
    public class Account
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public BattleAccount BattleNet { get; set; }
        public int? SeasonRating { get; set; }
        public bool Updated { get; set; }

        public string LongSeasonRating
        {
            get
            {
                if (Updated)
                    return SeasonRating.Value.ToString();
                if (SeasonRating.HasValue && SeasonRating.Value != 0)
                    return $"NR ({SeasonRating.Value})";
                return $"NR";
            }
        }

        public Account()
        {
            Updated = false;
            SeasonRating = 0;
            BattleNet = new BattleAccount();
        }
        public Account(string email, string pwd)
        {
            Updated = false;
            SeasonRating = 0;
            Email = email;
            Password = pwd;
            BattleNet = new BattleAccount();
        }
        public Account(string email, string pwd, string bName, int bTag)
        {
            Updated = false;
            SeasonRating = 0;
            Email = email;
            Password = pwd;
            BattleNet = new BattleAccount(bName, bTag);
        }
    }

    public class BattleAccount
    {
        public string BattleName { get; set; }
        public int? BattleTag { get; set; }

        public BattleAccount()
        {
            BattleName = string.Empty;
            BattleTag = null;
        }

        public BattleAccount(string bnet, int btag)
        {
            BattleName = bnet;
            BattleTag = btag;
        }

        public string LongName
        {
            get
            {
                bool isNoBtag = (BattleTag == null || BattleTag == 0);
                if (string.IsNullOrEmpty(BattleName) && isNoBtag)
                    return "";
                if (!string.IsNullOrEmpty(BattleName) && isNoBtag)
                    return BattleName;
                return $"{BattleName}#{BattleTag}"; ;
            }
        }
        public string Path() => $"https://playoverwatch.com/en-us/career/pc/{BattleName}-{BattleTag}";
    }
}
