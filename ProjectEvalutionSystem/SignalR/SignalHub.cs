using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ProjectEvalutionSystem.SignalR
{
    public class SignalHub : Hub
    {
        public void Notify<T>(List<T> dataset) where T : class
        {
            Clients.All.DataSet(dataset);
        }
    }
}