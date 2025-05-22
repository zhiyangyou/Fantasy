using Fantasy.Model;

namespace Hotfix.System;

/// <summary>
/// 加密解密逻辑类
/// </summary>
public static class System_RSAEncrypt {
    public static string EncryptPassword(this Component_RSAEncrypt self, string password) {
        return RSAEncryptHelper.RSAEncrypt(self.PublicKey, password);
    }

    public static bool VerifyPassword(this Component_RSAEncrypt self, string password, string encryptedPassword) {
        var ret = RSAEncryptHelper.RSADecrypt(self.PrivateKey, encryptedPassword);
        return string.Equals(ret, password);
    }

    public static string GenerateToken(this Component_RSAEncrypt self, long accountID, uint sceneConfigID) {
        string encryStr = $"{accountID},{sceneConfigID}";
        return RSAEncryptHelper.RSAEncrypt(self.PublicKey, encryStr);
    }

    public static bool VerifyToken(this Component_RSAEncrypt self, string token, long accountID, uint sceneConfigID) {
        if (string.IsNullOrEmpty(token)) {
            return false;
        }
        if (accountID <= 0) {
            return false;
        }
        var descryptStr = RSAEncryptHelper.RSADecrypt(self.PrivateKey, token);
        string compareStr = $"{accountID},{sceneConfigID}";
        return string.Equals(descryptStr, compareStr);
    }
}