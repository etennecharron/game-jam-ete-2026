using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class volcano : MonoBehaviour
{

    public GameObject[] projectileArr;
    public Transform player;
    
    public List<Projectile> projectiles;
    
    public class Projectile
    {
        public GameObject projectile;
        public bool activated;

    }

    public void shootProjectile()
    {

    }

}
