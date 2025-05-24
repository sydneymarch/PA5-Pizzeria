namespace mis_221_pa_5_sydneymarch
{
    public class User
    {
        private string username;
        private string password;
        private string role;

        public User(string username, string password, string role)
        {
            this.username = username;
            this.password = password;
            this.role = role;
        }

        public string GetUsername() => username;
        public string GetPassword() => password;
        public string GetRole() => role;
    }
}
