using OWOrganizerApp.Helpers;
using OWOrganizerApp.Models;
using System.Collections.ObjectModel;
using OWOrganizerDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OWOrganizerApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Const
        private const string up_char = "▴";
        private const string down_char = "▾";
        private const string headerNameRank = "Rank";
        private const string headerNameBTag = "BattleTag";
        private static AccountModel emptyAcc = new AccountModel();
        #endregion

        #region Setters/Getters
        private ObservableCollection<AccountModel> accounts = new ObservableCollection<AccountModel>();
        public ObservableCollection<AccountModel> Accounts
        {
            get { return accounts; }
            set { SetProperty(ref accounts, value); }
        }

        private AccountModel account;
        public AccountModel Account
        {
            get { return account; }
            set { SetProperty(ref account, value); }
        }

        public AccountModel SavedAccount { get; set; }

        private DateTime lastUpdate;
        public DateTime LastUpdate
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

        private string stateSort;
        public string StateSort
        {
            get { return stateSort; }
            set { stateSort = value; }
        }

        private string infos;
        public string Infos
        {
            get { return infos; }
            set { SetProperty(ref infos, value); }
        }

        private string triggerInfo;
        public string TriggerInfo
        {
            get { return triggerInfo; }
            set { SetProperty(ref triggerInfo, value); }
        }

        #region Visibility
        private bool updateButtonVisibilty;
        public bool UpdateButtonVisibilty
        {
            get { return updateButtonVisibilty; }
            set { SetProperty(ref updateButtonVisibilty, value); UpdatingProgressBarVisibility = !value; }
        }

        private bool updatingProgressBarVisibility;
        public bool UpdatingProgressBarVisibility
        {
            get { return updatingProgressBarVisibility; }
            set { SetProperty(ref updatingProgressBarVisibility, value); }
        }

        private bool dialogVisibility;
        public bool DialogVisibilty
        {
            get { return dialogVisibility; }
            set { SetProperty(ref dialogVisibility, value); }
        }

        private bool buttonAddAccountVisibilty;
        public bool ButtonAddAccountVisibilty
        {
            get { return buttonAddAccountVisibilty; }
            set { SetProperty(ref buttonAddAccountVisibilty, value); }
        }

        private bool buttonEditAccountVisibilty;
        public bool ButtonEditAccountVisibilty
        {
            get { return buttonEditAccountVisibilty; }
            set { SetProperty(ref buttonEditAccountVisibilty, value); }
        }
        #endregion

        #region RelayCommands
        private RelayCommand sortCommand;
        public RelayCommand SortCommand
        {
            get { return sortCommand; }
            set { SetProperty(ref sortCommand, value); }
        }

        private RelayCommand copyClipboardCommand;
        public RelayCommand CopyClipboardCommand
        {
            get { return copyClipboardCommand; }
            set { SetProperty(ref copyClipboardCommand, value); }
        }

        private RelayCommandAsync updateAccs;
        public RelayCommandAsync UpdateAccs
        {
            get { return updateAccs; }
            set { SetProperty(ref updateAccs, value); }
        }

        private RelayCommand accDialogCommand;
        public RelayCommand AccDialogCommand
        {
            get { return accDialogCommand; }
            set { SetProperty(ref accDialogCommand, value); }
        }

        private RelayCommand deleteAccountCommand;
        public RelayCommand DeleteAccountCommand
        {
            get { return deleteAccountCommand; }
            set { SetProperty(ref deleteAccountCommand, value); }
        }

        private RelayCommand addNewAccountCommand;
        public RelayCommand AddNewAccountCommand
        {
            get { return addNewAccountCommand; }
            set { SetProperty(ref addNewAccountCommand, value); }
        }        
        #endregion
        #endregion

        public MainViewModel()
        {
            Accounts = new ObservableCollection<AccountModel>(GetAccounts());
            Account = SavedAccount = new AccountModel();
            HeaderRank = headerNameRank;
            HeaderBtag = headerNameBTag;
            StateSort = string.Empty;
            UpdateButtonVisibilty = true;
            DialogVisibilty = false;

            InitCommands();
        }

        public void InitCommands()
        {
            SortCommand = new RelayCommand(o => SortHeader(o.ToString()), o => true);
            CopyClipboardCommand = new RelayCommand(o => CopyToClipboard(o), o => true);
            UpdateAccs = new RelayCommandAsync(o => UpdateAccounts(), o => true);
            AccDialogCommand = new RelayCommand(o => OpenDialog(o), o => true);
            AddNewAccountCommand = new RelayCommand(o => UpsertAccount(), o => true);
            DeleteAccountCommand = new RelayCommand(o => DeleteAccount(), o => true);

            AccountAction.NewFileIsUpdated += UpdateInfos;
        }

        public List<AccountModel> GetAccounts()
        {
            var basedAccs = AccountAction.Get();
            lastUpdate = AccountAction.LAST_UPDATE.Value;
            return FormatListAcc(basedAccs);
        }

        private List<AccountModel> FormatListAcc(List<OWOrganizerDLL.Objects.Account> basedAccs)
        {
            var accs = new List<AccountModel>();
            foreach (var acc in basedAccs)
                accs.Add(new AccountModel(acc));

            return accs;
        }

        private void UpdateInfos(Object sender, EventArgs e)
        {
            Infos = $"{AccountAction.UPDATED}/{Accounts.Count} updated";
        }

        private void ResetDatas()
        {
            Account = new AccountModel();
        }

        #region Commands Methods

        private void SortHeader(string header)
        {
            switch (header)
            {
                case headerNameBTag:
                    HeaderBtag = FormatSorting((a) => a.Account.BattleNet.LongName, headerNameBTag, stateSort != $"{header}_down"); break;
                case headerNameRank:
                    HeaderRank = FormatSorting((a) => a.Account.LongSeasonRating, headerNameRank, stateSort != $"{header}_down"); break;
            }

            string FormatSorting(Func<AccountModel, string> funcAcc, string headerName, bool down)
            {
                if (down)
                {
                    Accounts = new ObservableCollection<AccountModel>(Accounts.OrderByDescending(funcAcc));
                    stateSort = $"{headerName}_down";
                    return $"{headerName} {down_char}";
                }

                Accounts = new ObservableCollection<AccountModel>(Accounts.OrderBy(funcAcc));
                stateSort = $"{headerName}_up";
                return $"{headerName} {up_char}";
            }
        }

        private void CopyToClipboard(object o)
        {
            string text = o.ToString();
            System.Windows.Clipboard.SetText(text);
            Infos = $"Copied to clipboard !";
            TriggerInfo = "True";
            TriggerInfo = "";
        }

        public void OpenDialog(object value)
        {
            bool isOpen;
            if (value is bool)
            {
                isOpen = (bool)value;
                ButtonAddAccountVisibilty = true;
            }
            else
            {
                Tuple<bool, AccountModel> acc = value as Tuple<bool, AccountModel>;
                isOpen = acc.Item1;
                Account = SavedAccount = acc.Item2;
                ButtonAddAccountVisibilty = false;
            }

            ButtonEditAccountVisibilty = !ButtonAddAccountVisibilty;
            DialogVisibilty = isOpen;

            if (!isOpen) ResetDatas();
        }

        private async Task UpdateAccounts()
        {
            UpdateButtonVisibilty = false;
            Infos = $"{AccountAction.UPDATED}/{Accounts.Count} updated";
            var listDll = Accounts.Select(x => x.Account).ToList();

            var newAccs = await AccountAction.Update(listDll, true);
            Accounts = new ObservableCollection<AccountModel>(FormatListAcc(newAccs));
            UpdateButtonVisibilty = true;
            Infos = "Done!";
            TriggerInfo = "True";
            TriggerInfo = "";
        }

        private void UpsertAccount()
        {
            var acc = Account.Account;
            if (string.IsNullOrEmpty(acc.Password)
                && string.IsNullOrEmpty(acc.Email)
                && string.IsNullOrEmpty(acc.BattleNet.BattleName)
                && !acc.BattleNet.BattleTag.HasValue) return;

            var item = Accounts.FirstOrDefault(x => x.Account == SavedAccount.Account);

            if (SavedAccount.EqualsValues(emptyAcc))
            {
                Accounts.Add(Account);
                StoreAndClean();
                return;
            }

            if (item != null)
            {
                item.Account = acc;
                StoreAndClean();
                return;
            }
        }

        private void DeleteAccount()
        {
            var acc = Accounts.Where(x => x.Account == SavedAccount.Account).FirstOrDefault();
            if (acc is null) return;

            Accounts.Remove(acc);
            StoreAndClean();
        }

        private void StoreAndClean()
        {
            SavedAccount = emptyAcc;
            AccountAction.Store(Accounts.Select(x => x.Account).ToList());
            DialogVisibilty = false;
            ResetDatas();
        }
        #endregion
    }
}