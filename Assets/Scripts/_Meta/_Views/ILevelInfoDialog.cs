using System;
using _Gameplay._Models;

namespace _Meta._Views {
    public interface ILevelInfoDialog {
        event Action OnPlayRequested;
        event Action OnCloseRequested;
        void Show(LevelData data, bool isLocked);
        void Hide();
    }
}