using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;

namespace Hotfix;

public class Handler_C2G_Test2 : Message<C2G_Test2> {
    protected override async FTask Run(Session session, C2G_Test2 message) {
        Log.Debug($"c2g : {message.frameOpCode} {message.msg_content}");
        session.Send(new G2C_Test2() {
            frameOpCode = message.frameOpCode,
            msg_content = message.msg_content,
        });
        await FTask.CompletedTask;
    }
}