using OWOrganizerDLL.Objects;
using System.Collections.Generic;
using System.ComponentModel;

namespace OWOrganizerApp.Models
{
    public class AccountModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Account account;
        public Account Account
        {
            get { return account; }
            set { account = value; NotifyPropertyChanged("Account"); NotifyPropertyChanged("RankIcon"); }
        }
        
        public string RankIcon
        {
            get
            {
                if (Account.SeasonRating > 4000)
                    return GetImage("Grandmaster");
                if (Account.SeasonRating > 3500)
                    return GetImage("Master");
                if (Account.SeasonRating > 3000)
                    return GetImage("Diamond");
                if (Account.SeasonRating > 2500)
                    return GetImage("Platinum");
                if (Account.SeasonRating > 2000)
                    return GetImage("Gold");
                if (Account.SeasonRating > 1500)
                    return GetImage("Silver");
                return GetImage("Bronze");
            }
        }
        private string GetImage(string filename)
        {
            if (!Account.Updated && (!Account.SeasonRating.HasValue || Account.SeasonRating.Value == 0))
                return $"/Assets/Images/Ranks/NR_dotted.png";
            if (!Account.Updated)
                filename += "_dotted";
            return $"/Assets/Images/Ranks/{filename}.png";
        }

        public AccountModel() => Account = new Account();
        public AccountModel(Account acc) => Account = acc;

        public bool EqualsValues(AccountModel acc)
        {
            return Account.Updated == acc.Account.Updated &&
                   Account.SeasonRating == acc.Account.SeasonRating &&
                   Account.Password == acc.Account.Password &&
                   EqualsBattleNet(acc.Account.BattleNet);
        }

        private bool EqualsBattleNet(BattleAccount b)
        {
            if (Account.BattleNet != null && b != null)
            {
                return Account.BattleNet.BattleName == b.BattleName &&
                       Account.BattleNet.BattleTag.Value == b.BattleTag.Value;
            }
            return (Account.BattleNet == null && b == null);
        }

        private void NotifyPropertyChanged(string nameProp) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameProp));
    }
}
