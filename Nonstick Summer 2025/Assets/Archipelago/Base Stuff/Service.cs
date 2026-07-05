using System.Threading.Tasks;
using UnityEngine;

public abstract class Service : MonoBehaviour
{
    protected abstract void InitializeSingleton();

    public async Task Initialize()
    {
        if (destroyCancellationToken.IsCancellationRequested)
            return;

        InitializeSingleton();

        await ThisInitialize();

        return;
    }

    protected async virtual Task ThisInitialize()
    {
        await Task.CompletedTask;
    }

    public async virtual Task DeInitialize() {
        await Task.CompletedTask;
    }
}
