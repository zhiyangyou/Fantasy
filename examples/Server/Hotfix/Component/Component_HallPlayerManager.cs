using System.Collections.Concurrent;
using Fantasy;
using Fantasy.Entitas;
using Fantasy.Network;
using Hotfix.Model.Hall;
using ServerShareToClient;

namespace Hotfix.Component;

public class Component_HallPlayerManager : Entity {
    /// <summary>
    /// key : 地图类型
    /// value: <account_id, 玩家数据>
    /// </summary>
    public ConcurrentDictionary<int, ConcurrentDictionary<long, Model_HallPlayer>> _dicPlayer = new();


    public Component_HallPlayerManager() {
        Log.Info("Component_HallPlayerManager ctor");
        var mapTypes = Enum.GetValues(typeof(MapType));
        foreach (MapType mapType in mapTypes) {
            if (mapType != MapType.None) {
                _dicPlayer.TryAdd((int)mapType, new ConcurrentDictionary<long, Model_HallPlayer>());
            }
        }
    }


    public Model_HallPlayer GetHallPlayer(int mapTypeID, long account_id) {
        var exist = _dicPlayer.TryGetValue(mapTypeID, out ConcurrentDictionary<long, Model_HallPlayer> dicPlayer);
        if (!exist) {
            return null;
        }
        return dicPlayer.TryGetValue(account_id, out Model_HallPlayer player) ? player : null;
    }

    /// <summary>
    /// 验证大厅角色进入地图
    /// </summary>
    public (uint errorCode, StateSyncData serverSyncData) DoPlayerMove(StateSyncData syncData) {
        long account_id = syncData.player_id;
        var hallPlayer = GetHallPlayer(syncData.map_type, account_id);
        if (hallPlayer == null) {
            return (ErrorCode.StateSync_PlayerNotExist, null);
        }

        hallPlayer.position.x += syncData.input_dir.x * GameConstConfig.FixedDeltaTime * GameConstConfig.HallPlayerMoveSpeed * GameConstConfig.MaxSyncStateCount;
        hallPlayer.position.y += syncData.input_dir.y * GameConstConfig.FixedDeltaTime * GameConstConfig.HallPlayerMoveSpeed * GameConstConfig.MaxSyncStateCount;
        hallPlayer.position.z += syncData.input_dir.z * GameConstConfig.FixedDeltaTime * GameConstConfig.HallPlayerMoveSpeed * GameConstConfig.MaxSyncStateCount;


        syncData.position = hallPlayer.position.ToCSVector3();
        return (ErrorCode.Success, syncData);
    }


    public uint VerifyHallRoleEnterMap(int gotoMapType) {
        var mapConfig = MapConfigConter.Instance.GetMapConfig((MapType)gotoMapType);
        if (mapConfig == null) {
            return ErrorCode.EnterMap_MapConfigNotExist;
        }
        var roleInitPos = mapConfig.GetRoleInitPos((MapType)gotoMapType);
        if (roleInitPos == null) {
            return ErrorCode.EnterMap_DoorConfigNotExist;
        }
        return ErrorCode.Success;
    }


    /// <summary>
    /// 向地图中添加玩家
    /// </summary>
    /// <param name="account_id"></param>
    /// <param name="session"></param>
    /// <param name="gotoMapType"></param>
    /// <param name="roleInfo"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Model_HallPlayer AddHallPlayerToMap(long account_id, Session session, int gotoMapType, Model_Role roleInfo) {
        ConcurrentDictionary<long, Model_HallPlayer> dicMapPlayers = null;
        if (!_dicPlayer.TryGetValue(gotoMapType, out dicMapPlayers)) {
            throw new Exception($"不存在地图:{gotoMapType} 检查MapType枚举是否更新");
        }

        Model_HallPlayer hallPlayer = null;
        if (!dicMapPlayers.ContainsKey(account_id)) {
            hallPlayer = Entity.Create<Model_HallPlayer>(this.Scene, true, false);
            hallPlayer.player_id = account_id;
            hallPlayer.session = session;
            hallPlayer.role = roleInfo;
            hallPlayer.position = MapConfigConter.Instance.GetMapConfig((MapType)gotoMapType).GetRoleInitPos((MapType)gotoMapType).ToVector3();
            dicMapPlayers.TryAdd(account_id, hallPlayer);
        }
        else {
            hallPlayer = dicMapPlayers[account_id];
        }
        hallPlayer.cur_map_type = gotoMapType;
        // 重新进入地图, 初始化角色位置
        hallPlayer.position = MapConfigConter.Instance.GetMapConfig((MapType)gotoMapType).GetRoleInitPos((MapType)gotoMapType).ToVector3();
        return hallPlayer;
    }

    /// <summary>
    /// 从某张地图中中移除玩家
    /// </summary>
    /// <param name="account_id"></param>
    /// <param name="curMapTypeID"></param>
    /// <returns></returns>
    public bool RemoveHallPlayerFromMap(long account_id, int curMapTypeID) {
        if (this._dicPlayer.TryGetValue(curMapTypeID, out var dicMapPlayers)) {
            dicMapPlayers.TryRemove(account_id, out var hallPlayer);
            hallPlayer.Dispose();
            return true;
        }
        return false;
    }
}