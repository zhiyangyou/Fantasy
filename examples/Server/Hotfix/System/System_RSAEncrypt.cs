using Fantasy.Model;

namespace Hotfix.System;

/// <summary>
/// 加密解密逻辑类
/// </summary>
public static class System_RSAEncrypt {
    public static string EncryptPassword(this Component_RSAEncrypt self, string password) {
        return RSAEncryptHelper.RSAEncrypt(self.PublicKey, password);
    }

    public static bool VerifyPassword(this Component_RSAEncrypt self, string password) {
        var ret = RSAEncryptHelper.RSADecrypt(self.PrivateKey, password);
        return string.Equals(ret, password);
    }
}