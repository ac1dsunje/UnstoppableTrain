using UnityEngine;
using UnityEngine.Pool;

public abstract class ObjectPoolManager<T> : MonoBehaviour where T : Component
{
    
    [SerializeField] protected T prefab;
    [SerializeField] protected int defaultCapacity = 10;
    [SerializeField] protected int maxSize = 100;

    protected ObjectPool<T> Pool { get; private set; }
    public virtual void Init()
    {
        Pool = new ObjectPool<T>(
            createFunc: Create,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroy,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    protected abstract T Create();
    protected virtual void OnGet(T item) => item.gameObject.SetActive(true);
    protected virtual void OnRelease(T item) => item.gameObject.SetActive(false);
    protected virtual void OnDestroy(T item) => Destroy(item.gameObject);

    public T Get() => Pool.Get();
    public void Release(T item) => Pool.Release(item);

    protected virtual void Awake() => Init();
}