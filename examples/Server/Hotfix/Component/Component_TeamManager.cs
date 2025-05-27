using System.Collections.Concurrent;
using Fantasy.Entitas;
using ServerShareToClient;

namespace Hotfix.Component;

public class Component_TeamManager : Entity {
    #region 属性和字段

    /// <summary>
    /// key: teamID
    /// value: 队伍成员
    /// </summary>
    private ConcurrentDictionary<int, List<Model_Role>> _dicTeamInfos = new();

    /// <summary>
    /// key: account_id
    /// value: 该用户创建的队伍
    /// </summary>
    private ConcurrentDictionary<long, int> _dicAlreadyCreatedTeams = new();

    private int teamID = 10000;

    private int NextTeamID {
        get {
            var ret = Interlocked.Increment(ref teamID);
            return ret;
        }
    }

    #endregion


    #region public

    public async Task<(uint errorCode, int teamID, Model_Role? modelRole)> CreateTeam(long account_id) {
        if (_dicAlreadyCreatedTeams.ContainsKey(account_id)) {
            return (ErrorCode.CreateTeam_TeamExist, -1, null);
        }

        var hallPlayerComponent = this.Scene.GetComponent<Component_HallPlayerManager>();

        var hallPlayer = hallPlayerComponent.FindHallPlayer(account_id);
        if (hallPlayer == null) {
            return (ErrorCode.CreateTeam_PlayerUnvalid, -1, null);
        }

        var modelRole = hallPlayer.role;
        int newTeamID = NextTeamID;

        var newTeamList = new List<Model_Role>() { modelRole };
        _dicTeamInfos.AddOrUpdate(newTeamID, id => newTeamList, (i, oldList) => newTeamList);
        _dicAlreadyCreatedTeams.AddOrUpdate(account_id, id => newTeamID, (oldAccount, oldTeamID) => newTeamID);
        return (ErrorCode.Success, newTeamID, modelRole);
    }


    public void Process_PlayerDisconnect(long accountId) {
        DisposeTeam(accountId);
    }

    #endregion

    #region private

    /// <summary>
    /// 解散队伍
    /// </summary>
    public void DisposeTeam(long accountId) {
        if (_dicAlreadyCreatedTeams.TryRemove(accountId, out var removeTeamID)) {
            _dicTeamInfos.TryRemove(removeTeamID, out var listTeamMember);
            foreach (var modelRole in listTeamMember) {
                // TODO 通知队伍中的玩家
            }
        }
    }

    #endregion
}