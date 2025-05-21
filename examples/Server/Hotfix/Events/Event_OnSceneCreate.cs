using Fantasy;
using Fantasy.Async;
using Fantasy.Event;
using Fantasy.Model;

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
                Log.Info("鉴权服务添加组件: Component_AuthenticationAccount");
            }
        }
        return FTask.Create();
    }
}