using Fantasy.Entitas;
using ServerShareToClient;

namespace Hotfix.Model.Battle;

/// <summary>
/// 表达: 一场帧同步战斗
/// </summary>
public class Model_Battle : Entity {
    public long battleID { get; private set; }

    /// <summary>
    /// key: account_ID
    /// </summary>
    private Dictionary<long, Model_BattlePlayer> _dicAllPlayers = new();

    public BattleStateEnum BattleState;

    public void Init(long battleID, List<Model_Role> players) {
        this.battleID = battleID;
        foreach (var modelRole in players) {
            var modelBattlePlayer = Entity.Create<Model_BattlePlayer>(Scene, true, false);
            modelBattlePlayer.Init(modelRole);
            _dicAllPlayers.Add(modelRole.account_id, modelBattlePlayer);
        }
    }
}