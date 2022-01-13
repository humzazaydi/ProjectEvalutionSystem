using System;

namespace Skeddle.Business.Exceptions.UIExceptions
{
    public class UIException : SkeddleException
    {
        public string RedirecToAction { get; set; }
        public string RedirectController { get; set; }
        public object ActionModel { get; set; }

        public UIException()
        {
        }

        public UIException(string message) : base(message)
        {
        }

        public UIException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UIException(string message, string redirectAction, string redirectController, object actionModel = null) : base(message)
        {
            RedirecToAction = redirectAction;
            RedirectController = redirectController;
            ActionModel = actionModel;
        }

        public UIException(string message, Exception innerException, string redirectAction, string redirectController, object actionModel = null) : base(message, innerException)
        {
            RedirecToAction = redirectAction;
            RedirectController = redirectController;
            ActionModel = actionModel;
        }
    }
}
