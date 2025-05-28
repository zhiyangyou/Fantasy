using Fantasy;
using Fantasy.Async;
using Fantasy.Entitas;
using ServerShareToClient;

namespace Hotfix.Model.Battle;

/// <summary>
/// 表达: 一场帧同步战斗
/// </summary>
public class Model_Battle : Entity {
    #region 属性和字段

    public long battleID { get; private set; }

    /// <summary>
    /// key: account_ID
    /// </summary>
    private Dictionary<long, Model_BattlePlayer> _dicAllPlayers = new();

    public BattleStateEnum BattleState { get; private set; } = BattleStateEnum.None;

    private long _logicFrameTimerID = -1;

    #endregion


    #region public

    public void Init(long battleID, List<Model_Role> players) {
        BattleState = BattleStateEnum.None;
        this.battleID = battleID;
        foreach (var modelRole in players) {
            var modelBattlePlayer = Entity.Create<Model_BattlePlayer>(Scene, true, false);
            modelBattlePlayer.Init(modelRole);
            _dicAllPlayers.Add(modelRole.account_id, modelBattlePlayer);
        }
    }

    public void BattleStart() {
        this.BattleState = BattleStateEnum.Start;
        _logicFrameTimerID = FTask.RepeatedTimer(Scene, GameConstConfig.LogicFrameIntervalMS, OnLogicFrameUpdate);
    }

    #endregion

    #region private

    private async void OnLogicFrameUpdate() {
        try { }
        catch (Exception e) {
            Log.Error($"逻辑帧更新失败: {e.Message}");
        }
    }

    #endregion
}