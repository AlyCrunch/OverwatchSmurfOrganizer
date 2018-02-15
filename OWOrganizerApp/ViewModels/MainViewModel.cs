using OWOrganizerApp.Helpers;
using OWOrganizerApp.Models;
using System.Collections.ObjectModel;
using OWOrganizerDLL;
using System;
using System.Collections.Generic;

namespace OWOrganizerApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<AccountModel> accounts = new ObservableCollection<AccountModel>();
        public ObservableCollection<AccountModel> Accounts
        {
            get { return accounts; }
            set { SetProperty(ref accounts, value); }
        }

        private DateTime lastUpdate;
        public DateTime MyProperty
        {
            get { return lastUpdate; }
            set { SetProperty(ref lastUpdate, value); }
        }

        private string headerRank;
        public string HeaderRank
        {
            get { return headerRank; }
            set { SetProperty(ref headerRank, value); }
        }

        private string headerBtag;
        public string HeaderBtag
        {
            get { return headerBtag; }
            set { SetProperty(ref headerBtag, value); }
        }

        public RelayCommand MyCommand
        {
            get;
            private set;
        }

        public MainViewModel()
        {
            Accounts = new ObservableCollection<AccountModel>(GetAccounts());
            HeaderRank = "Rank";
            HeaderBtag = "BattleTag";
            MyCommand = new RelayCommand(ExecuteMyCommand,() => _canExecuteMyCommand);
        }       
        public List<AccountModel> GetAccounts()
        {
            List<AccountModel> prout = new List<AccountModel>();
            var basedAccs = AccountAction.Get();
            
            foreach(var acc in basedAccs)
            {
                prout.Add(new AccountModel(acc));
            }

            return prout;
        }

        private void ExecuteMyCommand()
        {
            // Do something 
        }
    }
}