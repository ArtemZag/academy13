using System.Collections.Concurrent;
using System.Linq;

namespace BinaryStudio.PhotoGallery.Domain.Services.Tasks
{
    internal class UsersMonitorTask : IUsersMonitorTask
    {
        private const int MAX_INACTIVITY_PERIOD = 15;

        /// <summary>
        ///     Represents key-value pair, where key - userId, value - period of inactivity in minutes.
        /// </summary>
        private readonly ConcurrentDictionary<int, int> _users = new ConcurrentDictionary<int, int>();

        private int _period;

        public UsersMonitorTask()
        {
            _period = 2;
        }

        public int Period
        {
            get { return _period; }
            set
            {
                if (value > 0)
                {
                    _period = value;
                }
            }
        }

        public void Execute()
        {
            AppendPeriod();
            UpdateUsers();
        }

        public void SetOnline(int userId)
        {
            if (_users.ContainsKey(userId))
            {
                _users[userId] = 0;
            }
            else
            {
                _users.AddOrUpdate(userId, 0, (s, i) => 0);
            }
        }

        public void SetOffline(int userId)
        {
            int value;
            _users.TryRemove(userId, out value);
        }

        public bool IsOnline(int userId)
        {
            return _users.ContainsKey(userId);
        }

        private void AppendPeriod()
        {
            foreach (var key in _users.Keys)
            {
                _users[key] += _period;
            }
        }

        private void UpdateUsers()
        {
            foreach (var user in _users.Where(user => user.Value > MAX_INACTIVITY_PERIOD))
            {
                int value;
                _users.TryRemove(user.Key, out value);
            }
        }
    }
}