using System.Collections.Concurrent;
using Fantasy.Entitas;
using Hotfix.Model.Battle;

namespace Hotfix.Component.Battle;

public class Component_BattleManager : Entity {
    private long battleID = 10000;

    private long NextBattleID {
        get {
            var ret = Interlocked.Increment(ref battleID);
            return ret;
        }
    }

    /// <summary>
    /// 所有帧同步战斗对象
    /// key: 战斗ID
    /// value: 战斗对象
    /// </summary>
    public ConcurrentDictionary<long, Model_Battle> _dicAllBattles = new();


    public void StartBattle(List<Model_Role> players) {
        var modelBattle = Entity.Create<Model_Battle>(Scene, true, false);
        var battleID = NextBattleID;
        _dicAllBattles.TryAdd(battleID, modelBattle);
    }
}