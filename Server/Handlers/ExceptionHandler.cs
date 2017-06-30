using SocketServer.Attributes;

namespace SocketServer.Handlers
{
    public abstract class ExceptionHandler
    {
        public virtual string HandledNamespace
        {
            get
            {
                return getHandledNamespace();
            }
        }

        private string getHandledNamespace()
        {
            object[] attrs = GetType().GetCustomAttributes(typeof(HandledExceptionNamespaceAttribute), true);
            HandledExceptionNamespaceAttribute attr = (HandledExceptionNamespaceAttribute)attrs[0];
            return attr?.Namespace;
        }
    }
}
