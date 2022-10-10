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

    private IEnumerator Explosion()
    {
        Bomb.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        boomVfx.Play();
        Bomb.SetActive(false) ;

        foreach (var gate in gates)
        {
            gate.AddExplosionForce(power, explosionPos.position, radius, 1.0F, ForceMode.Impulse);
        }
        
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < collidersAfterBuild.Length; i++)
        {
            gates[i].DOMoveY(-5, 1f);
        }


        yield return new WaitForSeconds(2f);
        base.AfterBuildAction();

    }
    protected override void AfterBuildAction()
    {
        Explosion();
    }    

}
