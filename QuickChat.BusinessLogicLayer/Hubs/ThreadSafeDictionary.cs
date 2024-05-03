namespace QuickChat.BusinessLogicLayer.Hubs
{

    public class ThreadSafeDictionary
    {
        public static Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
        public List<string> GetClientIds(string username)
        {
            var clientIds = new List<string>();
            if (dictionary.ContainsKey(username))
            {
                clientIds.AddRange(dictionary[username]);
            }
            return clientIds;
        }
        public List<string> GetClientIds(List<string> usernames)
        {
            var clientIds = new List<string>();
            foreach (var username in usernames)
            {
                if (dictionary.ContainsKey(username))
                {
                    clientIds.AddRange(dictionary[username]);
                }
            }
            return clientIds;
        }
        public List<string> Get(string key)
        {
            return dictionary[key];
        }

        public async Task<string?> AddConnectionId(string connectionId, string userName)
        {
            string? result = null;
            if (dictionary.ContainsKey(userName))
            {
                var newValues = new List<string>(dictionary[userName]) { connectionId };
                dictionary[userName] = newValues;
            }
            else
            {
                result = userName;
                var newValues = new List<string>() { connectionId };
                dictionary.Add(userName, newValues);
            }
            return result;
        }
        public async Task<string?> RemoveConnectionId(string connectionId)
        {
            //Console.WriteLine("Removing the UserConnectionId in Concurrent Dictionary");
            KeyValuePair<string, List<string>>? keyToRemove = null;
            foreach (var kvp in dictionary)
            {
                if (kvp.Value.Contains(connectionId))
                {
                    keyToRemove = kvp;
                    break;
                }
            }
            if (keyToRemove != null)
            {
                if (keyToRemove.Value.Value.Count > 1)
                {
                    var newConnections = new List<string>(keyToRemove.Value.Value);
                    newConnections.Remove(connectionId);
                    //Console.WriteLine("Updating Keys");
                    dictionary[keyToRemove.Value.Key] = newConnections;
                    var removedInFuture = await EnsureUserNameRemoval(keyToRemove.Value.Key);
                    if (removedInFuture)
                    {
                        return keyToRemove.Value.Key;
                    }
                    return null;
                }
                else
                {
                    dictionary.Remove(keyToRemove.Value.Key);
                    //Console.WriteLine("Removing Keys");
                    return keyToRemove.Value.Key;
                }
            }
            return null;

        }
        public async Task<bool> EnsureUserNameRemoval(string username)
        {
            await Task.Delay(4000);
            if (!IsUserActive(username))
            {
                dictionary.Remove(username, out List<string>? removedValue);
                return true;
            }
            return false;
        }
        public List<string> GetActiveusers()
        {
            //Console.WriteLine("Active Users ------ {0}", string.Join(',', dictionary.Select(item => item.Key.ToLower() + "-" + item.Value + ";").ToList()));
            var result = dictionary.Where((kvp) => kvp.Value.Count() > 0).Select((item) => item.Key).ToList();
            //Console.WriteLine("Result" + string.Join(',', result));
            return result;
        }
        public void PrintDictionary()
        {
            foreach (var kvp in dictionary)
            {
                Console.WriteLine(kvp.Key, string.Join(",", dictionary[kvp.Key]));
            }
        }
        public bool IsUserActive(string userName)
        {
            var result = dictionary.ContainsKey(userName) && dictionary[userName].Count() > 0;
            return result;
        }
        // Other methods for manipulation with appropriate locking mechanisms
    }
}
