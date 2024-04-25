using System.ComponentModel;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class ZenjectNetCodeFactory : INetworkPrefabInstanceHandler
{
    private readonly GameObject _prefab;
    private readonly DiContainer _container;

    public ZenjectNetCodeFactory(GameObject prefab, DiContainer container)
    {
        _prefab = prefab;
        _container = container;
    }

    public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
    {
        var gameObject = Object.Instantiate(_prefab);
        _container.InjectGameObject(gameObject);
        return gameObject.GetComponent<NetworkObject>();
    }

    public void Destroy(NetworkObject networkObject)
    {
        Object.Destroy(networkObject.gameObject);
    }
}
