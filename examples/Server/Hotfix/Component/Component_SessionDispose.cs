using System.Diagnostics;
using Fantasy;
using Fantasy.Entitas;

namespace Hotfix.Component;

/// <summary>
/// 处理用户断开连接的事情
/// </summary>
public class Component_SessionDispose : Entity {
    public long account_id;

    public override void Dispose() {
        var roleManager = this.Scene.GetComponent<Component_RoleManager>();
        var hallPlayerManager = this.Scene.GetComponent<Component_HallPlayerManager>();
        Log.Info($"TODO 用户{account_id} 下线了 {roleManager != null} {hallPlayerManager != null}");
        hallPlayerManager.Process_PlayerDisconnect(account_id);
        roleManager.Process_PlayerDisconnect(account_id);
        base.Dispose();
    }
}