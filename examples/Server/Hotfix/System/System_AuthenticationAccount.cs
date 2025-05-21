using Fantasy;
using Fantasy.Async;
using Fantasy.DataBase;
using Fantasy.Entitas;
using Fantasy.Helper;
using Fantasy.Model;
using Hotfix.Model;

namespace Hotfix.System;

/// <summary>
/// 账号先关的逻辑类
/// </summary>
public static class System_AuthenticationAccount {
    public static async FTask<uint> RegisterAccount(this Component_AuthenticationAccount self, string account, string password) {
        if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password)) {
            return 1000; // 账号或是密码为空
        }
        var accountHashCode = account.GetHashCode();
        using (await self.Scene.CoroutineLockComponent.Wait(accountHashCode, accountHashCode, "Regitser Account")) {
            
            var dataBase = self.Scene.World.DataBase;
            
            if (self._dicAllAccountCache.ContainsKey(account)) {
                return 1001; // 账号已经存在
            }
            var hasExist = await dataBase.Exist<Model_Account>(modelAccount => modelAccount.account == account);
            if (hasExist) {
                return 1001;
            }

            Model_Account accountModel = Entity.Create<Model_Account>(self.Scene, true, false);
            accountModel.account = account;
            accountModel.password = self.Scene.GetComponent<Component_RSAEncrypt>().EncryptPassword(password);
            accountModel.createTime = TimeHelper.Now;

            if (!self._dicAllAccountCache.TryAdd(accountModel.account, accountModel)) {
                return 1001;
            }

            // 回写数据库
            await dataBase.Save<Model_Account>(accountModel);
            return 0; // 注册成功
        }
    }
}