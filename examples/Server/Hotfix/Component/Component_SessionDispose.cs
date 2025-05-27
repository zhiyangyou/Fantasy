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
        Process_HallPlayerManager();
        Process_RoleManager();
        Process_TeamManager();
        base.Dispose();
    }



    private void Process_TeamManager() {
        var roleManager = this.Scene.GetComponent<Component_TeamManager>();
        roleManager.Process_PlayerDisconnect(account_id);
    }
    
    private void Process_RoleManager() {
        var roleManager = this.Scene.GetComponent<Component_RoleManager>();
        roleManager.Process_PlayerDisconnect(account_id);
    }

    private void Process_HallPlayerManager() {
        var hallPlayerManager = this.Scene.GetComponent<Component_HallPlayerManager>();
        hallPlayerManager.Process_PlayerDisconnect(account_id);
    }
}