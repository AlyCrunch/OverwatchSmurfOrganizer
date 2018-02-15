using OWOrganizerDLL.Objects;
using System.Collections.Generic;

namespace OWOrganizerApp.Models
{
    public class AccountModel : Account
    {
        public string RankIcon
        {
            get
            {
                if (SeasonRating > 4000)
                    return GetImage("Grandmaster");
                if (SeasonRating > 3500)
                    return GetImage("Master");
                if (SeasonRating > 3000)
                    return GetImage("Diamond");
                if (SeasonRating > 2500)
                    return GetImage("Platinum");
                if (SeasonRating > 2000)
                    return GetImage("Gold");
                if (SeasonRating > 1500)
                    return GetImage("Silver");
                return GetImage("Bronze");
            }
        }

        private string GetImage(string filename)
        {
            return $"/Assets/Images/Ranks/{filename}.png";
        }

        public AccountModel(){}
        public AccountModel(Account acc)
        {
            Email = acc.Email;
            Password = acc.Password;
            BattleNet = acc.BattleNet;
            SeasonRating = acc.SeasonRating;
            Updated = acc.Updated;
        }
    }
}
