using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Fantasy;
using Fantasy.Async;
using Fantasy.Entitas;
using Hotfix.Model;

namespace Hotfix.Component;

public class Component_RoleManager : Entity {
    #region 属性和字段

    /// <summary>
    /// key是角色昵称, value 是角色实体
    /// </summary>
    private ConcurrentDictionary<string, Model_Role> _dicRoles = new();


    /// <summary>
    /// 记录玩家当前选择的角色
    /// key: account_id
    /// value: 选择的角色
    /// </summary>
    private ConcurrentDictionary<long, Model_Role> _dicAccountSelectRole = new();

    #endregion


    #region public

    public void UpdateSelectRole(long account_id, Model_Role modelRole) {
        if (modelRole == null) {
            Log.Error("UpdateSelectRole 输入参数modelRole 是null");
            return;
        }
        _dicAccountSelectRole.AddOrUpdate(account_id, (_) => modelRole, (_, __) => modelRole);
    }


    /// <summary>
    /// 获取account_id当前选择的角色, 结果可空
    /// </summary>
    /// <param name="account_id"></param>
    /// <returns></returns>
    public Model_Role? GetCurSelectRole(long account_id) {
        _dicAccountSelectRole.TryGetValue(account_id, out var modelRole);
        return modelRole;
    }

    public async FTask<bool> RoleExists(long role_uid) {
        if (role_uid <= 0) {
            return false;
        }
        var db = this.Scene.World.DataBase;
        var exist = await db.Exist<Model_Role>(role => role.Id == role_uid);

        return exist;
    }

    public async FTask<Model_Role?> GetRole(long role_uid) {
        if (role_uid <= 0) {
            return null;
        }
        var db = this.Scene.World.DataBase;
        var ret = await db.First<Model_Role>(role => role.Id == role_uid);
        return ret;
    }

    public async FTask<List<RoleData>> GetRoleDatas(long account_id) {
        var db = this.Scene.World.DataBase;
        var models = await db.Query<Model_Role>(data => data.account_id == account_id);
        return models.Select(role => role.ToRoleData()).ToList();
    }

    public async FTask<(uint errorCode, Model_Role? role)> CreateRole(int role_id, long account_id, string role_name) {
        if (role_id == 0 || account_id <= 0 || string.IsNullOrEmpty(role_name)) {
            return (1200, null);
        }

        var nameHashCode = role_name.GetHashCode();
        using (await this.Scene.CoroutineLockComponent.Wait(nameHashCode, nameHashCode, "Create Role")) {
            // 同名检查
            if (_dicRoles.ContainsKey(role_name)) {
                return (1201, null); // 同名了
            }

            var db = this.Scene.World.DataBase;
            var exist = await db.Exist<Model_Role>(role => string.Equals(role.role_name, role_name));

            if (exist) {
                return (1201, null); // 同名   
            }

            // 账户是否存在
            bool accountValid = await db.Exist<Model_Account>(account => account_id.Equals(account_id));
            if (!accountValid) {
                return (1202, null); // 账号不合法
            }

            // 角色数量上限
            List<Model_Role> roles = await db.Query<Model_Role>(role => role.account_id == account_id);
            if (roles.Count >= 10) {
                return (1203, null); // 角色上限
            }

            // TODO role_id 合法检查
            bool roleIDValid = true;
            if (!roleIDValid) {
                return (1204, null); // role_id 不合法
            }

            Model_Role modelRole = Entity.Create<Model_Role>(this.Scene, true, false);
            modelRole.role_name = role_name;
            modelRole.role_id = role_id;
            modelRole.account_id = account_id;
            modelRole.level = 1;

            _dicRoles.TryAdd(modelRole.role_name, modelRole);

            await db.Save(modelRole);
            return (0, modelRole);
        }
    }


    /// <summary>
    /// 获取用户当前选择的Role
    /// </summary>
    /// <param name="account_id"></param>
    /// <returns></returns>
    public async FTask<List<RoleData>> GetUserSelectRole(long account_id) {
        var db = this.Scene.World.DataBase;
        var models = await db.Query<Model_Role>(data => data.account_id == account_id);
        return models.Select(role => role.ToRoleData()).ToList();
    }

    #endregion

    public void Process_PlayerDisconnect(long accountId) { }
}