using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Utils.Variables;
public class FloatingText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float DestroyTime = 3f;
    [SerializeField] Vector3 Offset = new Vector3 (0, 3, 0);
    [SerializeField] Vector3 RandomizeIntensity = new Vector3(0.7f,0,0);
    [SerializeField] private ScriptableGameObject lookat;

    private TextMesh text;
    private ScriptableGameObjectPool _pool;
    void Awake()
    {
        text = GetComponent<TextMesh>();
    }

    public void Innit(int damage, ScriptableGameObjectPool pool)
    {
        _pool = pool;
        text.text = damage.ToString();
        Invoke("ReturnToPool", DestroyTime);
        transform.LookAt(transform.position + lookat.Value.transform.forward);
        transform.localPosition += Offset;
        transform.localPosition += new Vector3(Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
        Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
        Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));
    }

    private void ReturnToPool()
    {
        _pool.ReturnToPool(gameObject);
    }

}
