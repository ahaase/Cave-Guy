using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BBBA
{
    class Weapon
    {
        float bulletSpeed;
        int weaponID;
        int cooldown;
        int charge;
        int ammo;

        float damage;
        float bulletGravity;

        Texture2D bulletSprite;
        public Weapon(float damage, float bulletSpeed, float bulletGravity, int cooldown, int weaponID, Texture2D bulletSprite)
        {
            this.damage = damage;
            this.bulletSpeed = bulletSpeed;
            this.bulletGravity = bulletGravity;
            this.cooldown = cooldown;
            charge = 0;
            this.weaponID = weaponID;

            this.bulletSprite = bulletSprite;
        }
        public void Update(GameTime gameTime, bool fire, Vector2 position, Map map, Vector2 direction, Character source)
        {
            if (charge < cooldown)
            {
                charge += gameTime.ElapsedGameTime.Milliseconds;
            }
            if(fire && charge >= cooldown)
            {
                charge -= cooldown;
                map.Projectiles.Add(Fire(position, map, direction, source));
            }
        }
        private Projectile Fire(Vector2 position, Map map, Vector2 direction, Character source)
        {
            return new BBBA.Projectile(position, bulletSprite, map, bulletSprite, direction, bulletSpeed, source, damage, bulletGravity);
        }
    }
}
