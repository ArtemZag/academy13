using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace BinaryStudio.PhotoGallery.Domain.Services.Tasks
{
    internal class UsersMonitorTask : IUsersMonitorTask
    {
        private const int MAX_INACTIVITY_PERIOD = 15;

        /// <summary>
        ///     Represents key-value pair, where key - userEmail, value - period of inactivity in minutes.
        /// </summary>
        private readonly ConcurrentDictionary<string, int> users = new ConcurrentDictionary<string, int>();

        private int period;

        public UsersMonitorTask()
        {
            period = 2;
        }

        public int Period
        {
            get { return period; }
            set
            {
                if (value > 0)
                {
                    period = value;
                }
            }
        }

        public void Execute()
        {
            AppendPeriod();

            UpdateUsers();
        }

        public void SetOnline(string userEmail)
        {
            if (users.ContainsKey(userEmail))
            {
                users[userEmail] = 0;
            }
            else
            {
                users.AddOrUpdate(userEmail, 0, (s, i) => 0);
            }
        }

        public void SetOffline(string userEmail)
        {
            int value;

            users.TryRemove(userEmail, out value);
        }

        public bool IsOnline(string userEmail)
        {
            return users.ContainsKey(userEmail);
        }

        private void AppendPeriod()
        {
            foreach (string key in users.Keys)
            {
                users[key] += period;
            }
        }

        private void UpdateUsers()
        {
            var usersToRemove = new Collection<string>();

            foreach (string key in users.Keys)
            {
                if (users[key] >= MAX_INACTIVITY_PERIOD)
                {
                    usersToRemove.Add(key);
                }
            }

            foreach (string user in usersToRemove)
            {
                int value;

                users.TryRemove(user, out value);
            }
        }
    }
}