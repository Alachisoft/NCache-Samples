using Alachisoft.NCache.Client;

namespace NHibernate.Caches.NCache
{
    public class NCacheCredentials
    {
        internal readonly Credentials credentials;

        public NCacheCredentials(
            string userId,
            string password)
        {
            credentials = new Credentials(userId, password);
        }

        public string UserID
        {
            get
            {
                return credentials.UserID;
            }
        }

        public string Password
        {
            get
            {
                return credentials.Password;
            }
        }
    }
}
