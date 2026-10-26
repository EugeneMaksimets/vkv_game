using UnityEngine;

/// <summary>
/// Находит IDamageable на объекте столкновения или его родителях.
/// </summary>
public static class DamageableResolver
{
    public static IDamageable Find(Transform target)
    {
        if (target == null)
            return null;

        MonoBehaviour[] components = target.GetComponentsInParent<MonoBehaviour>();

        foreach (MonoBehaviour component in components)
        {
            if (component is IDamageable damageable)
                return damageable;
        }

        return null;
    }

    public static bool HasTagInParents(Transform target, string tag)
    {
        Transform current = target;

        while (current != null)
        {
            if (current.CompareTag(tag))
                return true;

            current = current.parent;
        }

        return false;
    }
}
