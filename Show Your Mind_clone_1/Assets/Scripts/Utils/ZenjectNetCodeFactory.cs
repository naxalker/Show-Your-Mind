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
        GameObjectCreationParameters parameters = new()
        {
            Name = $"{_prefab.name} | Owner: {ownerClientId}",
            Position = position,
            Rotation = rotation
        };

        return _container.InstantiateNetworkPrefab(_prefab, parameters);
    }

    public void Destroy(NetworkObject networkObject)
    {
        Object.Destroy(networkObject.gameObject);
    }
}

public static class ContainerExtension
{
    public static NetworkObject InstantiateNetworkPrefab(this DiContainer container, GameObject prefab, 
        GameObjectCreationParameters creationParameters = null)
    {
        var state = prefab.activeSelf;
        prefab.SetActive(false);
        var gameObject = container.InstantiatePrefab(prefab, creationParameters ?? GameObjectCreationParameters.Default);
        prefab.SetActive(state);
        gameObject.SetActive(state);
        NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
        return networkObject;
    }
}
