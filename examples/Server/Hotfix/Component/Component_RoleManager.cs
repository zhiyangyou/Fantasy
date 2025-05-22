using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
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

    #endregion


    #region public

    public async FTask<List<RoleData>> GetRoleDatas( long account_id) {
        var db =  this.Scene.World.DataBase;
        var models =  await db.Query<Model_Role>(data => data.account_id == account_id);
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

    #endregion
}