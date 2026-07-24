using UnityEngine;

namespace DevCore.Core {
    public static class Collider2DExtensions {
        public static bool TryGetComponentOnRoot<T>(this Collider2D col, out T comp) where T : Component {
            return GetRootObject(col).TryGetComponent<T>(out comp);
        }

        public static GameObject GetRootObject(this Collider2D col) {
            return col.attachedRigidbody != null ? col.attachedRigidbody.gameObject : col.gameObject;
        }
    }
}
