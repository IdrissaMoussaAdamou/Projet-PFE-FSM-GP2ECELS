namespace Projet_PFE
{
    public static class Config
    {
        public static string ConnectionString { get; set; }
        public static string GetConnectionString()
        {
            return ConnectionString;
        }
        public static void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}