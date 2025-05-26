using System.Diagnostics;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.Component;
using ServerShareToClient;

class Hanlder_StateSync : MessageRPC<Send_StateSync, Rcv_StateSync> {
    protected override async FTask Run(Session session, Send_StateSync request, Rcv_StateSync response, Action reply) {
        var hallPlayerManager = session.Scene.GetComponent<Component_HallPlayerManager>();

        var (moveResult, syncData) = hallPlayerManager.DoPlayerMove(request.role_sync_data);

        response.ErrorCode = moveResult;
        if (moveResult != ErrorCode.Success) {
            return;
        }
        else {
            response.state_pack_id = request.state_pack_id;
            response.role_sync_data = syncData;
        }
        SendEnterMapToOtherPlayer(syncData,
            hallPlayerManager,
            request.role_sync_data.map_type,
            request.role_sync_data.player_id
        );

        await FTask.CompletedTask;
    }

    /// <summary>
    /// 把当前玩家的状态广播给其他这个地图上的其他玩家 
    /// </summary>
    /// <param name="syncData"></param>
    /// <param name="hallPlayerManager"></param>
    private void SendEnterMapToOtherPlayer(
        StateSyncData syncData,
        Component_HallPlayerManager hallPlayerManager,
        int mapType,
        long account_id
    ) {
        var message = new Msg_OtherPlayerStateSync();
        message.role_data = syncData;
        message.role_data.player_map_status = (int)PlayerMapStatus.InMap;
        var listPlayerInMap = hallPlayerManager.GetHallPlayersInMap(mapType, account_id);
        if (listPlayerInMap != null && listPlayerInMap.Count > 0) {
            foreach (var playerInMap in listPlayerInMap) {
                if (playerInMap == null) {
                    continue;
                }

                playerInMap.session.Send(message);
            }
        }
    }
}