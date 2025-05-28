using System.Runtime.CompilerServices;
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
        teamComponent.TryUpdateTeamMemberLoadProgress(message.team_id, message.account_id, message.progress);
        if (listMember != null) {
            foreach (Model_Role member in listMember) {
                member.session.Send(broadcastMsg);
            }
        }
        else {
            Log.Error("推送进入地下城加载进度消息, 队伍不存在");
        }

        bool isAllComplete = teamComponent.IsAllTeamMemberLoadComplete(message.team_id);

        if (isAllComplete) {
            teamComponent.ResetLoadProgress(message.team_id);
            Msg_StartDungeonBattle msgStartDungeonBattle = new();
            var listAllMembers = teamComponent.GetTeamRoleListByTeamID(message.team_id);
            if (listAllMembers != null) {
                foreach (var member in listAllMembers) {
                    member.session.Send(msgStartDungeonBattle);
                    Log.Info($"队伍:{message.team_id} 通知玩家 {member.role_name} 开始战斗");
                }
            }
        }
        await FTask.CompletedTask;
    }
}