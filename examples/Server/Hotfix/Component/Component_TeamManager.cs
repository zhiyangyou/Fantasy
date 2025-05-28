using System.Collections.Concurrent;
using Fantasy;
using Fantasy.Entitas;
using ServerShareToClient;

namespace Hotfix.Component;

public class TeamMember {
    public Model_Role ModelRole;
    public float LoadProgress;

    public bool IsLoadComplete => LoadProgress >= 1f;

    public TeamMember(Model_Role modelRole) {
        ModelRole = modelRole;
        LoadProgress = 0f;
    }

    private TeamMember() { }
}

public class TeamInfo {
    public List<TeamMember> Members { get; private set; } = new();

    public bool IsFull => Members.Count >= GameConstConfig.MaxSyncStateCount;

    public void ResetTeamMemberLoadProgress() {
        foreach (var member in Members) {
            member.LoadProgress = 0f;
        }
    }

    public bool AddMember(Model_Role member) {
        if (member == null) {
            return false;
        }
        else {
            var index = Members.FindIndex(role => role.ModelRole.account_id == member.account_id);
            if (index < 0) {
                Members.Add(new TeamMember(member));
                return true;
            }
            else {
                return false;
            }
        }
    }

    public bool RemoveMember(long account_id) {
        var index = Members.FindIndex(role => role.ModelRole.account_id == account_id);
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
            return Members[0].ModelRole.account_id == account_id;
        }
    }

    public void UpdateLoadProgress(long accountId, float progress) {
        for (int i = 0; i < Members.Count; i++) {
            if (Members[i].ModelRole.account_id == accountId) {
                Members[i].LoadProgress = progress;
            }
        }
    }

    public bool AllLoadComplete() {
        return Members.All(member => member.IsLoadComplete);
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
        return teamInfo.Members.Select(member => member.ModelRole).ToList();
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
            return (ErrorCode.Success, teamID, teamInfo.Members.Select(member => member.ModelRole).ToList(), thisPlayer.role);
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
        newTeamList.Members.Add(new TeamMember(modelRole));
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
                    _dicAccountIDWithTeamID.TryRemove(member.ModelRole.account_id, out _);
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

    /// <summary>
    /// 计算全部成员是否加载完成
    /// </summary>
    /// <returns></returns>
    public void TryUpdateTeamMemberLoadProgress(int teamID, long account_id, float progress) {
        if (!_dicTeamInfos.TryGetValue(teamID, out var teamInfo)) {
            Log.Error($"TryUpdateTeamMemberLoadProgress 传入了错误的队伍ID teamID:{teamID}");
            return;
        }
        else {
            teamInfo.UpdateLoadProgress(account_id, progress);
        }
    }

    public bool IsAllTeamMemberLoadComplete(int teamID) {
        if (!_dicTeamInfos.TryGetValue(teamID, out var teamInfo)) {
            Log.Error($"IsAllTeamMemberLoadComplete 传入了错误的队伍ID teamID:{teamID}");
            return false;
        }
        else {
            return teamInfo.AllLoadComplete();
        }
    }

    #endregion

    #region private

    private void BroadcastMsgToOthoerPlyers(TeamInfo teamInfo, Msg_TeamStateChanged msg, long accountId) {
        Model_Role modelRoleWhoChanged = teamInfo.Members.First(role => role.ModelRole.account_id == accountId).ModelRole;
        foreach (var modelRole in teamInfo.Members) {
            if (modelRole.ModelRole.account_id != modelRoleWhoChanged.account_id) {
                msg.role_data = modelRoleWhoChanged.ToRoleData();
                modelRole.ModelRole.session.Send(msg);
            }
        }
    }

    #endregion

    public async void ResetLoadProgress(int teamID) {
        if (_dicTeamInfos.TryGetValue(teamID, out var teamInfo)) {
            using (await Scene.CoroutineLockComponent.Wait(LockKeys.LockKey_ResetProgress, teamID, "Team Reset Load Progress")) {
                teamInfo.ResetTeamMemberLoadProgress();
            }
        }
    }
}