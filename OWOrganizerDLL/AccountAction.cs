using OWOrganizerDLL.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OWOrganizerDLL
{
    public static class AccountAction
    {
        public static int UPDATED = 0;
        public static DateTime? LAST_UPDATE;
        private static string FILENAME = "accounts.xml";
        public static event EventHandler NewFileIsUpdated;

        public async static Task<List<Account>> Update(List<Account> accs, bool store = false)
        {
            UPDATED = 0;

            for (int i = 0; i < accs.Count; i++)
            {
                var test = await Helpers.Basics.GetPage(accs[i].BattleNet.Path());
                int? sr = Helpers.Basics.GetSRFromDocument(test);
                accs[i] = Helpers.Basics.UpdateSRAccount(accs[i], sr);
                UPDATED++;
                OnNewFileIsUpdated(EventArgs.Empty);
            }

            if (store) Store(accs);

            return accs;
        }

        public async static Task<int?> GetSR(Account acc)
        {
            var test = await Helpers.Basics.GetPage(acc.BattleNet.Path());
            return Helpers.Basics.GetSRFromDocument(test);
        }

        public static void Store(List<Account> accs)
        {
            string path = Helpers.Storage.UserDataFolder;
            Helpers.Storage.SerializeObject(new StorageObject(accs, DateTime.Now),
                Helpers.Storage.GetFullPathUserDataFolder(FILENAME));
        }

        public static List<Account> Get()
        {
            var stoObj = Helpers.Storage.DeSerializeObject<StorageObject>(
                Helpers.Storage.GetFullPathUserDataFolder(FILENAME));

            LAST_UPDATE = stoObj.LastUpdate;
            return stoObj.Accounts;
        }

        private static void OnNewFileIsUpdated(EventArgs e)
        {
            NewFileIsUpdated?.Invoke(null, e);
        }
    }
}
