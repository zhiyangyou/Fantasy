using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.Component;

namespace Hotfix.Handlers.Outer.Game;

public class Handler_EnterDungeon : Message<Msg_EnterDungeon> {
    protected override async FTask Run(Session session, Msg_EnterDungeon message) {
        var teamComponent = session.Scene.GetComponent<Component_TeamManager>();

        List<Model_Role> listMember = teamComponent.GetTeamRoleListByTeamID(message.team_id);

        Msg_EnterDungeon broadcastMsg = new Msg_EnterDungeon();
        broadcastMsg.team_id = message.team_id;
        broadcastMsg.dungeonCfgID = message.dungeonCfgID;
        if (listMember != null) {
            broadcastMsg.teamMembers = listMember.Select(role => role.ToRoleData()).ToList();
            foreach (Model_Role member in listMember) {
                member.session.Send(broadcastMsg);
            }
        }
        else {
            broadcastMsg.teamMembers = null;
            Log.Error($"队伍不存在, 进入地下城失败 teamID:{message.team_id}");
            session.Send(broadcastMsg);
        }

        await FTask.CompletedTask;
    }
}