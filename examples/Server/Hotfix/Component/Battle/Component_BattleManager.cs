using System.Collections.Concurrent;
using Fantasy;
using Fantasy.Entitas;
using Hotfix.Model.Battle;

namespace Hotfix.Component.Battle;

public class Component_BattleManager : Entity {
    #region 属性和字段

    /// <summary>
    /// 所有帧同步战斗对象
    /// key: 战斗ID
    /// value: 战斗对象
    /// </summary>
    public ConcurrentDictionary<long, Model_Battle> _dicAllBattles = new();

    #endregion


    #region public

    public void StartBattle(List<Model_Role> players) {
        var modelBattle = Entity.Create<Model_Battle>(Scene, true, false);
        modelBattle.Init(players);
        _dicAllBattles.TryAdd(modelBattle.battleID, modelBattle);
        modelBattle.BattleStart();
    }

    public void OnPlayerOperateFrameInput(long battleID, List<FrameOperateData> listFrameOpDatas) {
        if (_dicAllBattles.TryGetValue(battleID, out Model_Battle battleMgr)) {
            if (listFrameOpDatas != null && listFrameOpDatas.Count > 0) {
                battleMgr.OnPlayerOperateFrameInput(listFrameOpDatas);
            }
        }
        else {
            Log.Error($"战斗对象不存在 battleID:{battleID}");
        }
    }

    #endregion
}