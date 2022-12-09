using System;
using System.Web;
using Microsoft.AspNetCore.SignalR;

namespace XHTDHP_API.Hubs
{
    public class ScaleHub : Hub
    {
        public void Send9511ScaleInfo(DateTime time, string value)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync(HubMethod.Send9511ScaleInfo, time, value);
        }

        public void Send9512ScaleInfo(DateTime time, string value)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync(HubMethod.Send9512ScaleInfo, time, value);
        }

        public void SendClinkerScaleInfo(DateTime time, string value)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync(HubMethod.SendClinkerScaleInfo, time, value);
        }
    }
}
