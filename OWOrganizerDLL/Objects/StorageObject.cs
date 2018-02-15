using System;
using System.Collections.Generic;
using System.Text;

namespace OWOrganizerDLL.Objects
{
    public class StorageObject
    {
        public DateTime LastUpdate { get; set; }
        public List<Account> Accounts { get; set; }

        public StorageObject() { }
        public StorageObject(List<Account> accs, DateTime uTime)
        {
            LastUpdate = uTime;
            Accounts = accs;
        }
    }
}
