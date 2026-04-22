using System;
using System.Linq;
using _Gameplay._Models;
using _Gameplay._Views;
using UnityEngine;

namespace _Data {
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Data/GameConfig")]
    public class GameConfig : ScriptableObject {
        public PlayerView PlayerViewPrefab;
        public BulletView BulletViewPrefab;
        public AsteroidView AsteroidViewPrefab;

        public AsteroidInfo[] AsteroidInfos;

        public AsteroidInfo GetAsteroidInfo(AsteroidType asteroidType) {
            var info = AsteroidInfos.FirstOrDefault(i => i.AsteroidType == asteroidType);
            if (info != null) {
                return info;
            }

            return new AsteroidInfo();
        }
    }
}

[Serializable]
public class AsteroidInfo {
    public AsteroidType AsteroidType = AsteroidType.None;
    public Color AsteroidColor = Color.white;
}