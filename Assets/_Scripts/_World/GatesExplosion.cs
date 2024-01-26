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
        if (forceToBuild)
        {
            StopCollect();
            BuildEffects(building);
            AfterBuildAction();
        }
        else 
        {
            InitLocalDictionary();

            for (int i = 0; i < _requiredTypesCount; i++)
            {
                var count = productRequierments[i].Amount;
                var type = productRequierments[i].Type;

                if (type == TileType.Gold)
                {
                    _counterView.InitCounerValues(type, _tilesCountByType[type], count);
                    goldCount = _tilesCountByType[type];
                }
                else 
                {
                    currentTilesCount += _tilesCountByType[type];
                    minCountForCheck += count;
                    _counterView.InitCounerValues(type, _tilesCountByType[type], count);
                }

           
            }
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
        gates[0].isKinematic = false;
        gates[1].isKinematic = false;
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
            gates[i].DOMoveY(-10, 1f);
            Destroy(gates[i].gameObject, 1.5f);
        }


    }
}
