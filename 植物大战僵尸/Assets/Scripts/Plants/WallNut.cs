using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantSpace
{
    public class WallNut : Plants
    {
        
       public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame

        private void Update()
        {
            if (currentHealth < maxHealth / 3 * 2)
            {
                anim.SetBool("WallNut1", true);
            }
            if (currentHealth < maxHealth / 3)
            {
                anim.SetBool("WallNut2", true);
            }
        }
        public override void Init()
        {
            base.Init();
            anim.SetBool("WallNut1", false);
            anim.SetBool("WallNut2", false);
        }
        public override void TakeNormal()
        {
            base.TakeNormal();
        }
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);

        }
        public override IEnumerator Plant()
        {
            return base.Plant();
        }
        public override IEnumerator Blink()
        {
            return base.Blink();
        }
        public override void StartBlink()
        {
            base.StartBlink();
        }
        public override IEnumerator PlaySoundAndBlink()
        {
            return base.PlaySoundAndBlink();
        }
    }
}
