using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.Component;

namespace Hotfix.Handlers.Outer.Game;

public class Handler_LoadDungeonProgress : Message<Msg_LoadDungeonProgress> {
    protected override async FTask Run(Session session, Msg_LoadDungeonProgress message) {
        var teamComponent = session.Scene.GetComponent<Component_TeamManager>();

        List<Model_Role> listMember = teamComponent.GetTeamRoleListByTeamID(message.team_id);
        Msg_LoadDungeonProgress broadcastMsg = new();
        broadcastMsg.team_id = message.team_id;
        broadcastMsg.account_id = message.account_id;
        broadcastMsg.progress = message.progress;

        if (listMember != null) {
            foreach (Model_Role member in listMember) {
                member.session.Send(broadcastMsg);
            }
        }
        else {
            Log.Error("推送进入地下城加载进度消息, 队伍不存在");
        }

        await FTask.CompletedTask;
    }
}