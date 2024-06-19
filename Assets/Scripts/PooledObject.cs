using System;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public Action<PooledObject> onDisableAction;

    private void OnDisable()
    {
        onDisableAction?.Invoke(this);
    }

}
