namespace Server.Sock.Handlers
{
    public abstract class Handler
    {
        public string Name
        {
            get
            {
                return this.getName();
            }
        }

        public bool MethodExists(string methodName)
        {
            return this.GetType().GetMethod(methodName) != null;
        }

        private string getName()
        {
            string[] parts = this.GetType().Name.Split('.');
            return parts[parts.Length - 1];
        }
    }
}
