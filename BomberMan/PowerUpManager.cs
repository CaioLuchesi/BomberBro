using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberBro
{
    public class PowerUpManager
    {
        public List<PowerUp.Kinds> items = new List<PowerUp.Kinds>();

        public PowerUpManager(int fireUp, int bombUp, int speedUp, int passThroughBomb, int bombKick,
                              int skull, int lineBomb, int fullFire, int powerGlove, int dangerousBomb, int remoteBomb)
        {
            for (int i = 0; i < fireUp; i++)
            {
                items.Add(PowerUp.Kinds.fireUp);
            }

            for (int i = 0; i < bombUp; i++)
            {
                items.Add(PowerUp.Kinds.bombUp);
            }

            for (int i = 0; i < speedUp; i++)
            {
                items.Add(PowerUp.Kinds.speedUp);
            }

            for (int i = 0; i < passThroughBomb; i++)
            {
                items.Add(PowerUp.Kinds.passThroughBomb);
            }

            for (int i = 0; i < bombKick; i++)
            {
                items.Add(PowerUp.Kinds.bombKick);
            }

            for (int i = 0; i < skull; i++)
            {
                items.Add(PowerUp.Kinds.skull);
            }

            for (int i = 0; i < lineBomb; i++)
            {
                items.Add(PowerUp.Kinds.lineBomb);
            }

            for (int i = 0; i < fullFire; i++)
            {
                items.Add(PowerUp.Kinds.fullFire);
            }

            for (int i = 0; i < powerGlove; i++)
            {
                items.Add(PowerUp.Kinds.powerGlove);
            }

            for (int i = 0; i < dangerousBomb; i++)
            {
                items.Add(PowerUp.Kinds.dangerousBomb);
            }

            for (int i = 0; i < remoteBomb; i++)
            {
                items.Add(PowerUp.Kinds.remoteBomb);
            }
        }
    }
}
