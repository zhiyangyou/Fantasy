using System.Collections.Concurrent;
using Fantasy;
using Fantasy.Entitas;
using ServerShareToClient;

namespace Hotfix.Component;

public class TeamInfo {
    public List<Model_Role> Members { get; private set; } = new();
    // private List<Model_Role> Members = new();

    public bool IsFull => Members.Count >= GameConstConfig.MaxSyncStateCount;

    public bool AddMember(Model_Role member) {
        if (member == null) {
            return false;
        }
        else {
            var index = Members.FindIndex(role => role.account_id == member.account_id);
            if (index < 0) {
                Members.Add(member);
                return true;
            }
            else {
                return false;
            }
        }
    }

    public bool RemoveMember(long account_id) {
        var index = Members.FindIndex(role => role.account_id == account_id);
        if (index < 0) {
            return false;
        }
        else {
            Members.RemoveAt(index);
            return true;
        }
    }


    public bool RemoveMember(Model_Role member) {
        if (member == null) {
            return false;
        }
        else {
            return RemoveMember(member.account_id);
        }
    }


    public bool IsLeader(long account_id) {
        if (Members.Count <= 0) {
            return false;
        }
        else {
            return Members[0].account_id == account_id;
        }
    }
}

public class Component_TeamManager : Entity {
    #region 属性和字段

    /// <summary>
    /// key: teamID
    /// value: 队伍成员
    /// </summary>
    private ConcurrentDictionary<int, TeamInfo> _dicTeamInfos = new();

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

    public List<Model_Role>? GetTeamRoleListByTeamID(int teamid) {
        if (!_dicTeamInfos.TryGetValue(teamid, out var teamInfo)) {
            return null;
        }
        return teamInfo.Members;
    }

    public async Task<(uint errorCode, int teamID, List<Model_Role>? modelRoles, Model_Role curModelRole)> JoinTeam(long account_id, int teamID) {
        // 队伍存在
        if (!_dicTeamInfos.TryGetValue(teamID, out var teamInfo)) {
            return (ErrorCode.JoinTeam_TeamNotExist, -1, null, null);
        }


        var hallPlayerManager = this.Scene.GetComponent<Component_HallPlayerManager>();
        var thisPlayer = hallPlayerManager.FindHallPlayer(account_id);
        if (thisPlayer == null) {
            return (ErrorCode.JoinTeam_PlayerNotExist, -1, null, null);
        }
        using (await Scene.CoroutineLockComponent.Wait(LockKeys.LockKey_TeamOp, LockKeys.LockKey_TeamOp, "JoinTeam")) {
            if (teamInfo.IsFull) {
                return (ErrorCode.JoinTeam_TeamFullMember, -1, null, null);
            }
            if (_dicAccountIDWithTeamID.ContainsKey(account_id)) {
                return (ErrorCode.JoinTeam_PlayerHasTeam, -1, null, null);
            }

            var addSuccess = teamInfo.AddMember(thisPlayer.role);
            if (!addSuccess) {
                return (ErrorCode.JoinTeam_TeamAddFailed, -1, null, null);
            }
            _dicAccountIDWithTeamID.TryAdd(account_id, teamID);
            return (ErrorCode.Success, teamID, teamInfo.Members, thisPlayer.role);
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

        var newTeamList = new TeamInfo();
        newTeamList.Members.Add(modelRole);
        _dicTeamInfos.AddOrUpdate(newTeamID, id => newTeamList, (i, oldList) => newTeamList);
        _dicAccountIDWithTeamID.AddOrUpdate(account_id, id => newTeamID, (oldAccount, oldTeamID) => newTeamID);
        return (ErrorCode.Success, newTeamID, modelRole);
    }


    public async void Process_PlayerDisconnect(long accountId) {
        // 是否在队伍中
        if (!_dicAccountIDWithTeamID.TryGetValue(accountId, out var inThisTeam)) {
            return;
        }
        using (await this.Scene.CoroutineLockComponent.Wait(LockKeys.LockKey_TeamOp, LockKeys.LockKey_TeamOp, "player dicconnect ,process Team")) {
            if (!_dicTeamInfos.TryGetValue(inThisTeam, out var teamInfo)) {
                _dicAccountIDWithTeamID.TryRemove(accountId, out _);
                return;
            }
            var msg = new Msg_TeamStateChanged();
            var isLeader = teamInfo.IsLeader(accountId);
            if (isLeader) {
                msg.team_state = (int)TeamOpStatus.TeamDispose;
                BroadcastMsgToOthoerPlyers(teamInfo, msg, accountId);
                foreach (var member in teamInfo.Members) {
                    _dicAccountIDWithTeamID.TryRemove(member.account_id, out _);
                }
                _dicTeamInfos.TryRemove(teamID, out _);
            }
            else {
                msg.team_state = (int)TeamOpStatus.MemberLeave;
                BroadcastMsgToOthoerPlyers(teamInfo, msg, accountId);
                _dicAccountIDWithTeamID.TryRemove(accountId, out _);
                teamInfo.RemoveMember(accountId);
            }
        }
    }

    #endregion

    #region private

    private void BroadcastMsgToOthoerPlyers(TeamInfo teamInfo, Msg_TeamStateChanged msg, long accountId) {
        Model_Role modelRoleWhoChanged = teamInfo.Members.First(role => role.account_id == accountId);
        foreach (var modelRole in teamInfo.Members) {
            if (modelRole.account_id != modelRoleWhoChanged.account_id) {
                msg.role_data = modelRoleWhoChanged.ToRoleData();
                modelRole.session.Send(msg);
            }
        }
    }

    #endregion
}