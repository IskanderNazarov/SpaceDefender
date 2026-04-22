using System;
using System.Collections.Generic;
using System.Reflection;

namespace _Services._DI {
    public class DIContainer {
        private static DIContainer _instance;

        private Dictionary<Type, object> _services;

        public static void Install() {
            _instance = new DIContainer();
            _instance._services = new Dictionary<Type, object>();
        }

        public static void Initialize() {
            foreach (var kv in _instance._services) {
                if (kv.Value is IInitializable initializable) {
                    initializable.Initialize();
                }
            }
        }

        public static void Register<T>(T instance)  {
            var type = typeof(T);
            _instance._services.TryAdd(type, instance);
        }

        public static void Inject(object target) {
            var targetType = target.GetType();

            var fields = targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields) {
                if (Attribute.IsDefined(field, typeof(InjectAttribute))) {
                    var dependencyType = field.FieldType;

                    // Если такой сервис зарегистрирован, вставляем его в поле
                    if (_instance._services.ContainsKey(dependencyType)) {
                        field.SetValue(target, _instance._services[dependencyType]);
                    } else {
                        UnityEngine.Debug.LogError($"[DI] Зависимость типа {dependencyType.Name} не найдена для {targetType.Name}!");
                    }
                }
            }
        }
    }
}