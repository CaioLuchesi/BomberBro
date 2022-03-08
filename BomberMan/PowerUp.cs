using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberBro
{
    public class PowerUp : Drawable
    {
        public enum Kinds
        {
            fireUp = 0,
            bombUp = 1,
            speedUp = 2,
            passThroughBomb = 3,
            bombKick = 4,
            skull = 5,
            lineBomb = 6,
            fullFire = 7,
            powerGlove = 8,
            dangerousBomb = 9,
            remoteBomb = 10
        }
        // Define Origin

        public PowerUp(DrawableRoom room, Vector2 position)
            : base(room) {
            this.position = position;
        }

        public override void CollisionEvent(Drawable other)
        {
            Type type = other.GetType();
            if (type == typeof(Player) || type.IsSubclassOf(typeof(Player)))
            {
                if (((Player)other).grabbingState == Grabbable.GrabbingState.nothing || ((Player)other).grabbingState == Grabbable.GrabbingState.landing || ((Player)other).grabbingState == Grabbable.GrabbingState.grabbing)
                {
                    AudioLibrary.itemPick.Play(0.6f, 0f, 0f);
                    ((Player)other).CurseResetter(true);
                    Activate((Player)other);
                    this.Destroy();
                    Type thisType = this.GetType();
                    if (thisType == typeof(BombKick))
                    {
                        ((Player)other).powerUpContainer.Add(PowerUp.Kinds.bombKick);
                    }
                    else
                    {
                        if (thisType == typeof(BombUp))
                        {
                            ((Player)other).powerUpContainer.Add(PowerUp.Kinds.bombUp);
                        }
                        else
                        {
                            if (thisType == typeof(DangerousBombPowerUp))
                            {
                                ((Player)other).powerUpContainer.Add(PowerUp.Kinds.dangerousBomb);
                            }
                            else
                            {
                                if (thisType == typeof(FireUp))
                                {
                                    ((Player)other).powerUpContainer.Add(PowerUp.Kinds.fireUp);
                                }
                                else
                                {
                                    if (thisType == typeof(FullFire))
                                    {
                                        ((Player)other).powerUpContainer.Add(PowerUp.Kinds.fullFire);
                                    }
                                    else
                                    {
                                        if (thisType == typeof(LineBomb))
                                        {
                                            ((Player)other).powerUpContainer.Add(PowerUp.Kinds.lineBomb);
                                        }
                                        else
                                        {
                                            if (thisType == typeof(PassThroughBombPowerUp))
                                            {
                                                ((Player)other).powerUpContainer.Add(PowerUp.Kinds.passThroughBomb);
                                            }
                                            else
                                            {
                                                if (thisType == typeof(PowerGlove))
                                                {
                                                    ((Player)other).powerUpContainer.Add(PowerUp.Kinds.powerGlove);
                                                }
                                                else
                                                {
                                                    if (thisType == typeof(RemoteBombPowerUp))
                                                    {
                                                        ((Player)other).powerUpContainer.Add(PowerUp.Kinds.remoteBomb);
                                                    }
                                                    else
                                                    {
                                                        if (thisType == typeof(Skull))
                                                        {
                                                            // ((Player)other).powerUpContainer.Add(PowerUp.Kinds.skull);
                                                        }
                                                        else
                                                        {
                                                            if (thisType == typeof(SpeedUp))
                                                            {
                                                                ((Player)other).powerUpContainer.Add(PowerUp.Kinds.speedUp);
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("WHAT?!?");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (type == typeof(Fire) || type.IsSubclassOf(typeof(Fire)))
                {
                    
                    //PowerUpDestroyAnimation pUpDA = new PowerUpDestroyAnimation(this.room,GraphicsLibrary.);
                    // this.Destroy();
                }
                else
                {
                    if (type == typeof(Bomb) || type.IsSubclassOf(typeof(Bomb)))
                    {
                        if (((Bomb)other).grabbingState == Grabbable.GrabbingState.nothing)
                        {
                            this.Destroy();
                        }
                    }
                }
            }
            base.CollisionEvent(other);
        }

        public virtual void Activate(Player other) { }
    }
}
