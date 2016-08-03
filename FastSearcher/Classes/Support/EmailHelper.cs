using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastSearcher.Classes.Support
{
    class EmailHelper
    {
        static EmailHelper instance;
        public static EmailHelper Instance { get { if (instance == null) instance = new EmailHelper(); return instance; } }

        private EmailHelper() { }

        public void ComposeEmailToPublisher()
        {
            EmailComposeTask mailTask = new EmailComposeTask();
            mailTask.To = "lmsoft@outlook.it";
            mailTask.Subject = "Fast Searcher App";
            mailTask.Show();
        }
    }
}
