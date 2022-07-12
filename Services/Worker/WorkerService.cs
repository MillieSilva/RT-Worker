// System Usings

// Application Usings

// External Usings
using Library.Network;
using Library.Network.Teller;

namespace Worker.Services;

public class WorkerService
{
    public TellerWorker Worker;

    public void Start()
    {
        Worker = new(new ConnectionInfo
        {
            IPv4 = "127.0.0.1",
            Port = Constants.DefaultWorkerServerPort
        });
    }

    internal void Authenticate()
    {
        Worker.Authenticate();
    }

    internal void Listen()
    {
        Worker.Listen();
    }

    internal void Deafen()
    {
        Worker.Deafen();
    }
}
