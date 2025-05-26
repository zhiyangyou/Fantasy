using Fantasy;
using Fantasy.Async;
using Fantasy.Event;
using Fantasy.Model;
using Hotfix.Component;
using Hotfix.Share;

namespace Hotfix;

public class Event_OnSceneCreate : IAsyncEvent {
    public Type EventType() {
        return typeof(OnCreateScene);
    }

    public FTask InvokeAsync(object self) {
        OnCreateScene onCreateScene = (OnCreateScene)self;
        Scene scene = onCreateScene.Scene;
        if (scene == null) {
            Log.Error("self as Scene 是空值");
        }
        else {
            if (scene.SceneType == (int)SceneType.Authentication) {
                scene.AddComponent<Component_AuthenticationAccount>();
                scene.AddComponent<Component_RSAEncrypt>();
                scene.AddComponent<Component_SceneConfig>();
                scene.AddComponent<Component_RoleManager>();
                
            } 
            if (scene.SceneType == (int)SceneType.Gate) {
                scene.AddComponent<Component_RSAEncrypt>();
                scene.AddComponent<Component_SceneConfig>();
                scene.AddComponent<Component_RoleManager>();
                scene.AddComponent<Component_HallPlayerManager>();
            } 
        }
        return FTask.Create();
    }
}