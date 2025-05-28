using System.Collections.Concurrent;
using Fantasy;
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
    /// 记录哪个用户在哪个队伍中
    /// key: account_id
    /// value: 该account_id所在的队伍ID
    /// </summary>
    private ConcurrentDictionary<long, int> _dicAccountIDWithTeamID = new();

    private int teamID = 10000;

    private const int TeamLockKey = 10000;

    private int NextTeamID {
        get {
            var ret = Interlocked.Increment(ref teamID);
            return ret;
        }
    }

    #endregion


    #region public

    public async Task<(uint errorCode, int teamID, List<Model_Role>? modelRoles, Model_Role curModelRole)> JoinTeam(long account_id, int teamID) {
        // 队伍存在
        if (!_dicTeamInfos.TryGetValue(teamID, out var teamMembers)) {
            return (ErrorCode.JoinTeam_TeamNotExist, -1, null, null);
        }


        var hallPlayerManager = this.Scene.GetComponent<Component_HallPlayerManager>();
        var thisPlayer = hallPlayerManager.FindHallPlayer(account_id);
        if (thisPlayer == null) {
            return (ErrorCode.JoinTeam_PlayerNotExist, -1, null, null);
        }
        using (await Scene.CoroutineLockComponent.Wait(LockKeys.LockKey_TeamOp, LockKeys.LockKey_TeamOp, "JoinTeam")) {
            if (teamMembers != null && teamMembers.Count >= GameConstConfig.MaxSyncStateCount) {
                return (ErrorCode.JoinTeam_TeamFullMember, -1, null, null);
            }
            if (_dicAccountIDWithTeamID.ContainsKey(account_id)) {
                return (ErrorCode.JoinTeam_PlayerHasTeam, -1, null, null);
            }

            teamMembers.Add(thisPlayer.role);
            _dicAccountIDWithTeamID.TryAdd(account_id, teamID);
            return (ErrorCode.Success, teamID, teamMembers, thisPlayer.role);
        }
    }

    public async Task<(uint errorCode, int teamID, Model_Role? modelRole)> CreateTeam(long account_id) {
        if (_dicAccountIDWithTeamID.ContainsKey(account_id)) {
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
        _dicAccountIDWithTeamID.AddOrUpdate(account_id, id => newTeamID, (oldAccount, oldTeamID) => newTeamID);
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
        if (_dicAccountIDWithTeamID.TryRemove(accountId, out var removeTeamID)) {
            _dicTeamInfos.TryRemove(removeTeamID, out var listTeamMember);
            foreach (var modelRole in listTeamMember) {
                // TODO 通知队伍中的玩家
            }
        }
    }

    #endregion
}