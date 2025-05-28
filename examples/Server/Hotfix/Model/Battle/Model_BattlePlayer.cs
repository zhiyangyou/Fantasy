using Fantasy.Entitas;
using Fantasy.Network;

namespace Hotfix.Model.Battle;

/// <summary>
/// 帧同步中一个用户对象
/// </summary>
public class Model_BattlePlayer : Entity {
    public Model_Role roleInfo { get; private set; }
    public long account_id => roleInfo.account_id;
    public Session session => roleInfo.session;
    public bool battleIsEnd = false;

    public void Init(Model_Role modelRole) {
        this.roleInfo = modelRole;
        battleIsEnd = false;
    }


    public override void Dispose() {
        roleInfo = null;
        battleIsEnd = false;
        base.Dispose();
    }
}