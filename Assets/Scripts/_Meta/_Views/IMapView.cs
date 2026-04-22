using System;

namespace _Meta._Views {
    public interface IMapView {
        event Action<int> OnLevelSelected;
        void UpdateMapState(int maxUnlockedLevel);
        void Show();
        void Hide();
    }
}