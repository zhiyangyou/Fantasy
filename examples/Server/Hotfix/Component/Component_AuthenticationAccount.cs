using Fantasy.Entitas;
using Hotfix.Model;

namespace Fantasy.Model;

/// <summary>
/// 鉴权组件
/// </summary>
public class Component_AuthenticationAccount : Entity {
    /// <summary>
    /// key 是 account_name
    /// </summary>
    public Dictionary<string, Model_Account> _dicAllAccountCache = new() ;
}