using Fantasy;
using Fantasy.Entitas;
using Fantasy.Network;

namespace Hotfix.Model.Hall;

public class Model_HallPlayer : Entity {
    /// <summary>
    /// account_id
    /// </summary>
    public long player_id;

    public Model_Role role;

    public Session session;

    /// <summary>
    /// 当前所在的地图id
    /// </summary>
    public int cur_map_type;

    /// <summary>
    /// 当前玩家所处的一个坐标
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// 继承了Entity, 就可以被对象池管理
    /// </summary>
    public override void Dispose() {
        base.Dispose();
        player_id = 0;
        role = null;
        session = null;
        cur_map_type = 0;
        position = null;
    }
}