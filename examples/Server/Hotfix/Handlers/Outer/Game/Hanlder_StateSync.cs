using System.Diagnostics;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.Component;
using Hotfix.ShareToClient;

class Hanlder_StateSync : MessageRPC<Send_StateSync, Rcv_StateSync> {
    protected override async FTask Run(Session session, Send_StateSync request, Rcv_StateSync response, Action reply) {
        var hallPlayerManager = session.Scene.GetComponent<Component_HallPlayerManager>();

        var (moveResult, syncData) = hallPlayerManager.DoPlayerMove(request.role_sync_data);

        response.ErrorCode = moveResult;
        if (moveResult != ErrorCode.Success) {
            return;
        }
        else { 
            // TODO 
            response.state_pack_id = request.state_pack_id;
            response.role_sync_data = syncData;
            Log.Info($"Player:{request.role_sync_data.player_id} pos:{syncData.position}");
        }
        await FTask.CompletedTask;
    }
}