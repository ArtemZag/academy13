using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class UsersMonitorTask : IUsersMonitorTask
    {
        private const int MAX_INACTIVITY_PERIOD = 15;

        /// <summary>
        /// Update period of users statuses. 
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// Represents key-value pair, where key - userEmail, value - period of inactivity in minutes. 
        /// todo: concurrent 
        /// </summary>
        private readonly Dictionary<string, int> users = new Dictionary<string, int>();
        
        public void Execute()
        {
            AppendPeriod();

            UpdateUsers();
        }

        private void AppendPeriod()
        {
            foreach (var key in users.Keys)
            {
                users[key] += Period;
            }
        }

        private void UpdateUsers()
        {
            var usersToRemove = new Collection<string>();

            foreach (var key in users.Keys)
            {
                if (users[key] >= MAX_INACTIVITY_PERIOD)
                {
                    usersToRemove.Add(key);
                }
            }

            foreach (var user in usersToRemove)
            {
                users.Remove(user);
            }
        }

        public void SetOnline(string userEmail)
        {
            if (users.ContainsKey(userEmail))
            {
                users[userEmail] = 0;
            }
            else
            {
                users.Add(userEmail, 0);                
            }
        }

        public void SetOffline(string userEmail)
        {
            users.Remove(userEmail);
        }

        public bool IsOnline(string userEmail)
        {
            return users.ContainsKey(userEmail);
        }
    }
}