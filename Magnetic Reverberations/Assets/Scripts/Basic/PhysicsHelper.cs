using UnityEngine;
using System.Collections.Generic;

public static class PhysicsHelper
{
    private const int MAX_RESULTS = 8;

    public static List<ItemPickupHandler> GetNearbyInteractables(
        Vector3 origin,
        float radius,
        LayerMask layerMask)
    {
        Collider[] colliders = new Collider[MAX_RESULTS];
        int count = Physics.OverlapSphereNonAlloc(
            origin,
            radius,
            colliders,
            layerMask,
            QueryTriggerInteraction.Collide
        );

        List<ItemPickupHandler> results = new List<ItemPickupHandler>(count);
        for (int i = 0; i < count; i++)
        {
            if (colliders[i].TryGetComponent(out ItemPickupHandler item))
                results.Add(item);
        }

        results.Sort((a, b) =>
            Vector3.Distance(a.transform.position, origin)
            .CompareTo(Vector3.Distance(b.transform.position, origin)));

        return results;
    }
}