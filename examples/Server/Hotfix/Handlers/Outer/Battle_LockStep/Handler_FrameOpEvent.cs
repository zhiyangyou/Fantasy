using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.Component.Battle;

namespace Hotfix.Handlers.Outer.Battle_LockStep;

public class Handler_FrameOpEvent : Message<Msg_C2S_FrameOpEvent> {
    protected override async FTask Run(Session session, Msg_C2S_FrameOpEvent message) {
        var battleMgr = session.Scene.GetComponent<Component_BattleManager>();
        battleMgr.OnPlayerOperateFrameInput(message.battle_id, message.frame_operate_datas);
        await FTask.CompletedTask;
    }
}