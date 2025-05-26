using System.Collections.Concurrent;
using System.Runtime.InteropServices.JavaScript;
using Fantasy;
using Fantasy.Entitas;
using Fantasy.Network;
using Hotfix.Model.Hall;
using Hotfix.ShareToClient;

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

    /// <summary>
    /// 验证大厅角色进入地图
    /// </summary>
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

        if (!dicMapPlayers.ContainsKey(account_id)) {
            Model_HallPlayer hallPlayer = Entity.Create<Model_HallPlayer>(this.Scene, true, false);
            hallPlayer.player_id = account_id;
            hallPlayer.session = session;
            hallPlayer.role = roleInfo;
            hallPlayer.cur_map_type = gotoMapType;
            hallPlayer.position = MapConfigConter.Instance.GetMapConfig((MapType)gotoMapType).GetRoleInitPos((MapType)gotoMapType).ToVector3();
            dicMapPlayers.TryAdd(account_id, hallPlayer);
            return hallPlayer;
        }
        else {
            return dicMapPlayers[account_id];
        }
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