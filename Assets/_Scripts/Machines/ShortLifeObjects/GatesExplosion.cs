using DG.Tweening;
using System.Collections;
using UnityEngine;

public sealed class GatesExplosion : BuilderFromTiles
{
    [SerializeField] private float radius=5f;
    [SerializeField] private float power=12f;
    [SerializeField] private Transform explosionPos;
    [SerializeField] private Rigidbody[] gates;
    [SerializeField] private ParticleSystem boomVfx;
    [SerializeField] private GameObject Bomb;

    private void Start()
    {
        InitDictionary();

        for (int i = 0; i < _requiredTypesCount; i++)
        {
            minCountForCheck += productRequierments[i].Amount;
        }
    }


    protected override void AfterBuildAction()
    {
        StopCollect();
        base.AfterBuildAction();
        StartCoroutine(Explosion());
    }
    private IEnumerator Explosion()
    {
        Bomb.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        boomVfx.Play();
        Bomb.SetActive(false);

        foreach (var gate in gates)
        {
            gate.AddExplosionForce(power, explosionPos.position, radius, 1.0F, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < gates.Length; i++)
        {
            gates[i].isKinematic = true;
            gates[i].DOMoveY(-5, 1f);
            Destroy(gates[i], 1.5f);
        }


        yield return new WaitForSeconds(2f);

    }
}
