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

    public static async FTask<(uint, Model_Account?)> LoginAccount(this Component_AuthenticationAccount self, string account, string password) {
        if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password)) {
            return (1005, null); // 账号或是密码为空
        }
        var dataBase = self.Scene.World.DataBase;
        var accountHashCode = account.GetHashCode();
        Model_Account accountModel = null;
        using (await self.Scene.CoroutineLockComponent.Wait(accountHashCode, accountHashCode, "Login Account")) {
            if (!self._dicAllAccountCache.TryGetValue(account, out accountModel)) {
                accountModel = await dataBase.First<Model_Account>((d) => d.account == account);
                if (accountModel == null) {
                    return (1006, null); // 账号不存在
                }
            }

            var componentRsa = self.Scene.GetComponent<Component_RSAEncrypt>();
            var isRight = componentRsa.VerifyPassword(password);
            if (!isRight) {
                return (1007, null);
            }
            accountModel.loginTime = TimeHelper.Now;

            self._dicAllAccountCache.Add(accountModel.account, accountModel);
            // 回写数据库
            await dataBase.Save<Model_Account>(accountModel);
            return (0, accountModel); // 登录成功
        }
    }
}