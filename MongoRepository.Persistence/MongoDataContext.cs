using MongoDB.Driver;

namespace MongoRepository.Persistence
{
    public class MongoDataContext
    {
        private const string LOCAL_HOST = "localhost", PORT = "27017", DATABASE_NAME = "database";

        public IMongoDatabase MongoDatabase { get; set; }

        private string Username { get; set; }
        private string Password { get; set; }
        private string Address { get; set; }
        private string Port { get; set; }
        public string DbName { get; set; }
        private string privateConnectionString { get; set; }
        private bool isLocal { get; set; }
        private bool isPrivateConnectionString { get; set; }

        public MongoDataContext() : this(string.Empty, string.Empty, LOCAL_HOST, PORT, DATABASE_NAME, true) { }
        public MongoDataContext(string Username, string Password, string Address, string Port, string DbName, bool isLocal)
        {
            this.isLocal = isLocal;
            this.Username = Username;
            this.Password = Password;
            this.Address = Address;
            this.Port = Port;
            this.DbName = DbName;
            this.privateConnectionString = string.Empty;
            this.isPrivateConnectionString = false;

            this.Connection();
        }
        public MongoDataContext(string connectionString, string DbName)
        {
            this.isLocal = false;
            this.Username = this.Password = this.Address = this.Port = string.Empty;
            this.DbName = DbName;

            this.privateConnectionString = connectionString;
            this.isPrivateConnectionString = true;

            this.Connection();
        }
        private void Connection()
        {
            IMongoClient MongoClient;

            if (this.isPrivateConnectionString)
                MongoClient = new MongoClient(this.privateConnectionString);
            else
            {
                if (this.isLocal)
                    MongoClient = new MongoClient($"mongodb://{this.Address}:{this.Port}/{this.DbName}");
                else
                    MongoClient = new MongoClient($"mongodb://{this.Username}:{this.Password}@{this.Address}:{this.Port}/{this.DbName}");
            }

            MongoDatabase = MongoClient.GetDatabase(this.DbName);
        }
    }
}
