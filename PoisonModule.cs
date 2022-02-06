using System;
using System.Collections;
using UnityEngine;
using ThunderRoad;

namespace Mordeth
{
    public class PoisonModule : ItemModule
    {
        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<Poison>();
        }
    }
}
