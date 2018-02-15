using OWOrganizerDLL.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OWOrganizerDLL
{
    public static class AccountAction
    {
        public static int UPDATED;
        public static DateTime? LAST_UPDATE;
        private static string FILENAME = "accounts.xml";

        public static async Task<List<Account>> Update(List<Account> accs)
        {
            UPDATED = 0;

            for (int i = 0; i < accs.Count; i++)
            {
                var test = await Helpers.Basics.GetPage(accs[i].BattleNet.Path());
                int? sr = Helpers.Basics.GetSRFromDocument(test);
                accs[i] = Helpers.Basics.UpdateSRAccount(accs[i], sr);
                UPDATED++;
            }
            return accs;
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


    }
}
