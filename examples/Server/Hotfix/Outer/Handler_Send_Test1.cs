using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
namespace Hotfix {
    public class Handler_Send_Test1 : MessageRPC<Send_Test1, Rcv_Test1> {
        protected override async FTask Run(Session session, Send_Test1 request, Rcv_Test1 response, Action reply) {
            response.ErrorCode = 12345;
            response.success = true;
            response.error_msg = "test error code ";
            Log.Debug("test handler .. ");
            await FTask.CompletedTask;
        }
    }
}