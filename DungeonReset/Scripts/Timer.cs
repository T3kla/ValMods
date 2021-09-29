using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonReset
{
    public class Timer
    {
        public static readonly List<Timer> Each = new();

        public DungeonGenerator dungeon;
        public float delay;
        public float interval;
        private Coroutine task = null;

        public Timer(DungeonGenerator dungeon, float delay, float interval)
        {
            this.dungeon = dungeon;
            this.delay = delay;
            this.interval = interval;

            Each.Find(ctx => ctx.dungeon == this.dungeon)?.Destroy();
            task = Main.Instance.StartCoroutine(Cycle());
            Each.Add(this);
        }

        public void Destroy()
        {
            Main.Instance.StopCoroutine(task);
            task = null;
            Each.Remove(this);
        }

        private IEnumerator Cycle()
        {
            yield return null;
            yield return new WaitForSeconds(delay);
            while (true)
            {
                if (Dungeons.Validate(this))
                    Dungeons.Reset(dungeon);
                yield return null;
                yield return new WaitForSeconds(interval);
            }
        }
    }
}
