using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections;

namespace BomberBro
{
    public class Player : Grabbable
    {
        public byte id;

        public Character character;

        public bool vulnerable = true;
        public bool dead = false;
        // public int playerID;
        public Skull.Curses? cursedState = null;
        public int skullTimer = 0;
        public float skullBackUpData = 0f;
        public Player gonnaSwapPositionsWith = null;
        private bool isTrue = false;
        protected bool noMoreSkullCollisionEventsForNow = false;

        // Constants

        // public Direction direction;

        /* private const byte DIR_RIGHT = 3;
        private const byte DIR_UP = 1;
        private const byte DIR_LEFT = 2;
        private const byte DIR_DOWN = 0; */
        public List<PowerUp.Kinds> powerUpContainer = new List<PowerUp.Kinds>();
        private const int INFINITY = 9999;
        public Bomb.Kinds bombKind = Bomb.Kinds.NormalBomb;
        public int firepower = 1;
        public int maxBombCount = 1;
        private int detonateTimeLeft = -1;

        private List<Bomb> bombs = new List<Bomb>();

        private const int pxStandingStill = 0;
        private const int pxLeft = 1;
        private const int pxRight = 2;
        private int px = pxStandingStill;
        private bool isRight = false;

        // protected AdvancedKeyboardState keyboardState = null;

        private float cont = 8;
        private const float speedSprite = 8;
        // private byte direction;
        public KeyboardMapping input;

        public bool lineBomb = false;
        public bool bombKick = false;
        public bool powerGlove = false;

        // private Vector2 lastPosition = Vector2.Zero;
        
        // protected Point frameSize;
        // private int collisionOffset; // DEPRECATED
        // protected Point currentFrame;
        // private Point sheetSize;
        public float speed;
        
        public void CurseResetter(bool saveSkull)
        {
            if (cursedState != null)
            {
                switch (cursedState)
                {
                    case Skull.Curses.bomberrhea:
                        maxBombCount = (int)skullBackUpData;
                        break;
                    case Skull.Curses.bomblessness:
                        maxBombCount = (int)skullBackUpData;
                        break;
                    case Skull.Curses.caffeine:
                        speed = skullBackUpData;
                        break;
                    case Skull.Curses.mirrorMovement:
                        Keys up = input.getUp();
                        input.setUp(input.getDown());
                        input.setDown(up);

                        Keys left = input.getLeft();
                        input.setLeft(input.getRight());
                        input.setRight(left);
                        break;
                    case Skull.Curses.tooSlow:
                        speed = skullBackUpData;
                        break;
                }

                if (saveSkull)
                {
                    List<Point> possiblePowerUpPositions = new List<Point>();
                    for (int y = 4; y <= 14; y++)
                    {
                        for (int x = 4; x <= 20; x++)
                        {
                            if (room.instanceMeetingCollisionMaskPlacedAt<Wall>(new Vector2(x * DEFAULT_GRID, y * DEFAULT_GRID), defaultCollisionMask) == null &&
                                room.instanceMeetingCollisionMaskPlacedAt<Player>(new Vector2(x * DEFAULT_GRID, y * DEFAULT_GRID), defaultCollisionMask) == null &&
                                room.instanceMeetingCollisionMaskPlacedAt<PowerUp>(new Vector2(x * DEFAULT_GRID, y * DEFAULT_GRID), defaultCollisionMask) == null &&
                                room.instanceMeetingCollisionMaskPlacedAt<Bomb>(new Vector2(x * DEFAULT_GRID, y * DEFAULT_GRID), defaultCollisionMask) == null)
                            {
                                possiblePowerUpPositions.Add(new Point(x, y));
                            }
                        }
                    }

                    if (possiblePowerUpPositions.Count > 0)
                    {
                        Point powerUpPoint = possiblePowerUpPositions[new Random().Next(possiblePowerUpPositions.Count)];
                        Vector2 powerUpPosition = new Vector2(powerUpPoint.X * DEFAULT_GRID, powerUpPoint.Y * DEFAULT_GRID);
                        possiblePowerUpPositions.Remove(powerUpPoint);
                        new Skull(this.room, powerUpPosition, (Skull.Curses)cursedState);
                    }
                    else
                    {
                        Console.WriteLine("O NOES A SKULL HAS BEEN LOST!");
                    }
                }
                cursedState = null;
            }
        }

        public Player(DrawableRoom room, Character character, Vector2 position, Point frameSize,
        Point currentFrame, Point sheetSize, float speed, KeyboardMapping input) : base(room)
        {
            this.character = character;
            this.textureImage = character.spritesheet;
            this.position = position; // setPosition(position);
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.input = input;
            this.scale = 1.3f;
            this.origin = new Vector2(3, 25);
            this.collisionMask = new Rectangle(7, 7, 18, 18);
        }

        public void AddBombToList(Bomb bomb)
        {
            this.bombs.Add(bomb);
        }

        public void RemoveBombFromList(Bomb bomb)
        {
            this.bombs.Remove(bomb);
            // Drawable.drawable.Remove(bomb);
        }

        public Bomb PlaceBomb()
        {
            return PlaceBomb(this.position);
        }

        public Bomb PlaceBomb(Vector2 position)
        {
            switch (bombKind)
            {
                case Bomb.Kinds.NormalBomb:
                    return (new NormalBomb(this.room, this, firepower, position));

                case Bomb.Kinds.PassThroughBomb:
                    return (new PassThroughBomb(this.room, this, firepower, position));

                case Bomb.Kinds.RemoteBomb:
                    return (new RemoteBomb(this.room, this, firepower, position));

                case Bomb.Kinds.DangerousBomb:
                    foreach (Bomb bomb in bombs)
                    {
                        if (bomb.GetType() == typeof(DangerousBomb) || bomb.GetType().IsSubclassOf(typeof(DangerousBomb)))
                        {
                            return (new NormalBomb(this.room, this, firepower, position));
                        }
                    }
                    return (new DangerousBomb(this.room, this, firepower, position));

                default:
                    return null;
            }
        }

        protected bool IsItPossibleToGrab(Grabbable grabbable)
        {
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            // KeyboardState keyboardState = Keyboard.GetState();
            // keyboardState = new AdvancedKeyboardState(Keyboard.GetState(), keyboardState);

            if (room.keyboard.CheckPressed(input.getLinebomb()))
            {
                if (this.lineBomb && (grabbingState == GrabbingState.nothing || grabbingState == GrabbingState.grabbing))
                {
                    bool collision = false;
                    Vector2 bombPosition =
                        new Vector2(
                                    (float)Math.Round(this.position.X / DEFAULT_GRID) * DEFAULT_GRID,
                                    (float)Math.Round(this.position.Y / DEFAULT_GRID) * DEFAULT_GRID
                                   );
                    while (bombs.Count < maxBombCount && collision == false)
                    {
                        if (room.instanceMeetingCollisionMaskPlacedAt<Wall>(bombPosition, defaultCollisionMask) != null ||
                            room.instanceMeetingCollisionMaskPlacedAt<PowerUp>(bombPosition, defaultCollisionMask) != null)
                        {
                            collision = true;
                        }
                        /* Bomb newBomb = new PassThroughBomb(this.Game, this, firepower, bombPosition);
                        if ((newBomb.instanceMeeting<Wall>() != null) || (newBomb.instanceMeeting<PowerUp>() != null) || (newBomb.instanceMeeting<Bomb>() != null))
                        {
                            this.RemoveBombFromList(newBomb);
                            collision = true;
                        } */
                        else
                        {
                            Bomb other = (Bomb)room.instanceMeetingCollisionMaskPlacedAt<Bomb>(bombPosition, defaultCollisionMask);
                            if (other != null)
                            {
                                if (other.grabbingState == GrabbingState.nothing || other.grabbingState == GrabbingState.landing)
                                {
                                    collision = true;
                                }
                            }

                            if (!collision)
                            {
                                Bomb newBomb = PlaceBomb(bombPosition);
                                // Bomb newBomb = new NormalBomb(this.Game, this, firepower, bombPosition);
                                switch (direction)
                                {
                                    case Direction.down:
                                        bombPosition.Y += DEFAULT_GRID;
                                        break;
                                    case Direction.left:
                                        bombPosition.X -= DEFAULT_GRID;
                                        break;
                                    case Direction.right:
                                        bombPosition.X += DEFAULT_GRID;
                                        break;
                                    case Direction.up:
                                        bombPosition.Y -= DEFAULT_GRID;
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            Vector2 nextLastPosition = new Vector2(this.position.X, this.position.Y);

            if (grabbingState == GrabbingState.nothing || grabbingState == GrabbingState.grabbing)
            {
                if (room.keyboard.Check(input.getUp()))
                {
                    if (Math.Round(position.X / DEFAULT_GRID) % 2 == 0)
                    {
                        StepTowards(new Vector2((float)(Math.Round(this.position.X / DEFAULT_GRID) * DEFAULT_GRID), this.position.Y - speed), speed);
                        direction = Direction.up;
                        currentFrame = new Point(Animation(), 2);
                    }
                    else
                    {
                        if (cursedState != Skull.Curses.caffeine)
                        {
                            direction = Direction.up;
                            currentFrame = new Point(Animation(), 2);
                        }
                        else
                        {
                            CantStopNeedMoreCaffeine();
                        }
                    }
                }
                else
                {
                    if (room.keyboard.Check(input.getDown()))
                    {
                        if (Math.Round(position.X / DEFAULT_GRID) % 2 == 0)
                        {
                            StepTowards(new Vector2((float)(Math.Round(this.position.X / DEFAULT_GRID) * DEFAULT_GRID), this.position.Y + speed), speed);
                            direction = Direction.down;
                            currentFrame = new Point(Animation(), 0);
                        }
                        else
                        {
                            if (cursedState != Skull.Curses.caffeine)
                            {
                                direction = Direction.down;
                                currentFrame = new Point(Animation(), 0);
                            }
                            else
                            {
                                CantStopNeedMoreCaffeine();
                            }
                        }
                    }
                    else
                    {
                        if (room.keyboard.Check(input.getLeft()))
                        {
                            if (Math.Round(position.Y / DEFAULT_GRID) % 2 == 0)
                            {
                                StepTowards(new Vector2(this.position.X - speed, (float)(Math.Round(this.position.Y / DEFAULT_GRID) * DEFAULT_GRID)), speed);
                                direction = Direction.left;
                                currentFrame = new Point(Animation(), 1);
                            }
                            else
                            {
                                if (cursedState != Skull.Curses.caffeine)
                                {
                                    direction = Direction.left;
                                    currentFrame = new Point(Animation(), 1);
                                }
                                else
                                {
                                    CantStopNeedMoreCaffeine();
                                }
                            }
                        }
                        else
                        {
                            if (room.keyboard.Check(input.getRight()))
                            {
                                if (Math.Round(position.Y / DEFAULT_GRID) % 2 == 0)
                                {
                                    StepTowards(new Vector2(this.position.X + speed, (float)(Math.Round(this.position.Y / DEFAULT_GRID) * DEFAULT_GRID)), speed);
                                    direction = Direction.right;
                                    currentFrame = new Point(Animation(), 3);
                                }
                                else
                                {
                                    if (cursedState != Skull.Curses.caffeine)
                                    {
                                        direction = Direction.right;
                                        currentFrame = new Point(Animation(), 3);
                                    }
                                    else
                                    {
                                        CantStopNeedMoreCaffeine();
                                    }
                                }
                            }
                            else
                            {
                                if (cursedState == Skull.Curses.caffeine)
                                {
                                    CantStopNeedMoreCaffeine();
                                }
                                else
                                {
                                    switch (direction)
                                    {
                                        case Direction.right: currentFrame = new Point(0, 3); break;
                                        case Direction.up: currentFrame = new Point(0, 2); break;
                                        case Direction.left: currentFrame = new Point(0, 1); break;
                                        case Direction.down: currentFrame = new Point(0, 0); break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (room.keyboard.CheckPressed(input.getBomb()))
            {
                if (powerGlove)
                {
                    Drawable other = instanceMeeting<Grabbable>();
                    if (other != null)
                    {
                        if (grabbingState == GrabbingState.nothing)
                        {
                            Grabbable victim = (Grabbable)other;
                            if (victim.grabbingState == GrabbingState.nothing || victim.grabbingState == GrabbingState.grabbing)
                            {
                                if (victim.GetType() == typeof(RemoteBomb) || victim.GetType().IsSubclassOf(typeof(RemoteBomb)))
                                {
                                    RemoteBomb victimAsRemoteBomb = (RemoteBomb)victim;
                                    if (victimAsRemoteBomb.canBeGrabbed)
                                    {
                                        Grab(victim);
                                    }
                                }
                                else
                                {
                                    Grab(victim);
                                }
                            }
                        }
                    }
                }
            }

            if (room.keyboard.Check(input.getBomb()) || cursedState == Skull.Curses.bomberrhea)
            {
                if (grabbingState == GrabbingState.nothing)
                {
                    if (instanceMeetingPlacedAt<Bomb>(getAlignedPosition(nextLastPosition)) == null)
                    {
                        if (bombs.Count < maxBombCount)
                        {
                            PlaceBomb(nextLastPosition);
                            // new NormalBomb(this.Game, this, firepower);
                        }
                    }
                }
                /* else
                {
                    if (grabbingState == GrabbingState.grabbing)
                    {
                        KeepGrabbing();
                    }
                } */
            }

            if (room.keyboard.CheckReleased(input.getBomb()))
            {
                if (grabbingState == GrabbingState.grabbing)
                {
                    ThrowAway();
                }
            }

            if (room.keyboard.CheckPressed(input.getDetonate()))
            {
                bool foundRemoteBomb = false;
                foreach (Bomb bomb in bombs)
                {
                    Type bombType = bomb.GetType();
                    if (bombType == typeof(RemoteBomb) || bombType.IsSubclassOf(typeof(RemoteBomb)))
                    {
                        if (((RemoteBomb)bomb).timer < 0 && bomb.grabbingState == GrabbingState.nothing)
                        {
                            foundRemoteBomb = true;
                            break;
                        }
                    }
                }

                if (foundRemoteBomb)
                {
                    try
                    {
                        AudioLibrary.beep.Play(0.3f, 0f, 0f);
                    }
                    catch (NullReferenceException)
                    {
                        Console.WriteLine("OH NOES");
                    }

                    // play neat sound effect
                    detonateTimeLeft = 30;
                }
            }

            if (room.keyboard.Check(input.getDetonate()))
            {
                if (detonateTimeLeft >= 0)
                {
                    detonateTimeLeft--;
                }

                if (detonateTimeLeft == 0)
                {
                    // Detonate EVERY remote bomb in the list
                    List<RemoteBomb> targetBombs = new List<RemoteBomb>();
                    foreach (Bomb bomb in bombs)
                    {
                        Type bombType = bomb.GetType();
                        if (bombType == typeof(RemoteBomb) || bombType.IsSubclassOf(typeof(RemoteBomb)))
                        {
                            targetBombs.Add((RemoteBomb)bomb);
                        }
                    }

                    foreach (RemoteBomb remoteBomb in targetBombs)
                    {
                        if (remoteBomb.grabbingState == GrabbingState.nothing)
                        {
                            remoteBomb.Explode();
                        }
                    }
                }
            }

            if (room.keyboard.CheckPressed(input.getStop()))
            {
                // stop kicked bomb
            }

            if (room.keyboard.CheckReleased(input.getDetonate()))
            {
                if (detonateTimeLeft > 0)
                {
                    // Detonate the first remote bomb in the list
                    RemoteBomb targetBomb = null;
                    foreach (Bomb bomb in bombs)
                    {
                        Type bombType = bomb.GetType();
                        if (bombType == typeof(RemoteBomb) || bombType.IsSubclassOf(typeof(RemoteBomb)))
                        {
                            RemoteBomb targetBombAsRemote = (RemoteBomb)bomb;
                            if (targetBombAsRemote.timer < 0)
                            {
                                targetBomb = targetBombAsRemote;
                                break;
                            }
                        }
                    }

                    if (targetBomb != null)
                    {
                        if (targetBomb.grabbingState == GrabbingState.nothing)
                        {
                            targetBomb.timer = detonateTimeLeft;
                        }
                    }
                }
            }
            if (cursedState != null)
            {
                if (blendColor == Color.White)
                {
                    if (isTrue)
                    {
                        blendColor = Color.Black;
                    }
                    else
                    {
                        isTrue = true;
                    }
                }
                else
                {
                    if (isTrue)
                    {
                        blendColor = Color.White;
                    }
                    else
                    {
                        isTrue = true;
                    }
                }
            }
            else
            {
                if (vulnerable == false)
                {
                    if (blendColor == Color.White)
                    {
                        if (isTrue)
                        {
                            blendColor = Color.TransparentBlack;
                        }
                        else
                        {
                            isTrue = true;
                        }
                    }
                    else
                    {
                        if (isTrue)
                        {
                            blendColor = Color.White;
                        }
                        else
                        {
                            isTrue = true;
                        }
                    }
                }
                else
                {
                    blendColor = Color.White;
                }
            }
            if (gonnaSwapPositionsWith != null)
            {
                SwapPositionsWith(gonnaSwapPositionsWith);
            }
            base.Update(gameTime);
        }

        public void SwapPositionsWith(Player victim)
        {
            Vector2 auxPosition = this.position;
            this.position = victim.position;
            victim.position = auxPosition;

            Vector2 auxLastPosition = this.previousPosition;
            this.previousPosition = victim.previousPosition;
            victim.previousPosition = auxLastPosition;

            float auxZ = this.z;
            this.z = victim.z;
            victim.z = auxZ;

            Grabbable.GrabbingState auxGrabbingState = this.grabbingState;
            this.grabbingState = victim.grabbingState;
            victim.grabbingState = auxGrabbingState;

            // FIX THIS
            if (victim != this.other)
            {
                Grabbable auxOther = this.other;
                if (this.other != null)
                    if (this.other.GetType() == typeof(Grabbable) || this.other.GetType().IsSubclassOf(typeof(Grabbable)))
                        this.other.other = victim;
                if (victim.other != null)
                    if (victim.other.GetType() == typeof(Grabbable) || victim.other.GetType().IsSubclassOf(typeof(Grabbable)))
                        victim.other.other = this;
                this.other = victim.other;
                victim.other = auxOther;
            }

            Direction auxDirection = this.direction;
            this.direction = victim.direction;
            victim.direction = auxDirection;

            float auxHorizontalAirSpeed = this.horizontalAirSpeed;
            this.horizontalAirSpeed = victim.horizontalAirSpeed;
            victim.horizontalAirSpeed = auxHorizontalAirSpeed;

            float auxVerticalAirSpeed = this.verticalAirSpeed;
            this.verticalAirSpeed = victim.verticalAirSpeed;
            victim.verticalAirSpeed = auxVerticalAirSpeed;

            float auxVerticalAirAcceleration = this.verticalAirAcceleration;
            this.verticalAirAcceleration = victim.verticalAirAcceleration;
            victim.verticalAirAcceleration = auxVerticalAirAcceleration;

            Vector2 auxTargetLandingPosition = this.targetLandingPosition;
            this.targetLandingPosition = victim.targetLandingPosition;
            victim.targetLandingPosition = auxTargetLandingPosition;

            gonnaSwapPositionsWith = null;
        }

        public void CantStopNeedMoreCaffeine()
        {
            switch (direction)
            {
                case Direction.right: StepTowards(new Vector2(this.position.X + speed, (float)(Math.Round(this.position.Y / DEFAULT_GRID) * DEFAULT_GRID)), speed); currentFrame = new Point(Animation(), 3); break;
                case Direction.up: StepTowards(new Vector2((float)(Math.Round(this.position.X / DEFAULT_GRID) * DEFAULT_GRID), this.position.Y - speed), speed); currentFrame = new Point(Animation(), 2); break;
                case Direction.left: StepTowards(new Vector2(this.position.X - speed, (float)(Math.Round(this.position.Y / DEFAULT_GRID) * DEFAULT_GRID)), speed); currentFrame = new Point(Animation(), 1); break;
                case Direction.down: StepTowards(new Vector2((float)(Math.Round(this.position.X / DEFAULT_GRID) * DEFAULT_GRID), this.position.Y + speed), speed); currentFrame = new Point(Animation(), 0); break;
            }
        }

        public override void CollisionEvent(Drawable other)
        {
            // Console.WriteLine(other.GetType().Name);
            if (this.grabbingState == GrabbingState.nothing || this.grabbingState == GrabbingState.grabbing)
            {
                Type type = other.GetType();
                if (type == typeof(Wall) || type.IsSubclassOf(typeof(Wall)))
                {
                    this.StepBack(); // setPosition(getLastPosition());
                }
                else
                {
                    if (type.IsSubclassOf(typeof(Bomb)))
                    {
                        if (!instanceMeetingPlacedAtPreviousPosition(other) && (((Bomb)other).grabbingState == GrabbingState.nothing || ((Bomb)other).grabbingState == GrabbingState.landing))
                        {
                            this.StepBack();
                            if (bombKick)
                            {
                                Bomb bomb = (Bomb)other;
                                bomb.direction = this.direction;
                                bomb.moving = true;
                                bomb.position = new Vector2((float)Math.Round(bomb.position.X / DEFAULT_GRID) * DEFAULT_GRID, (float)Math.Round(bomb.position.Y / DEFAULT_GRID) * DEFAULT_GRID);
                                bomb.kicker = this;
                            }
                        }
                    }
                    else
                    {
                        if (type == typeof(Fire) || type.IsSubclassOf(typeof(Fire)))
                        {
                            if (vulnerable)
                            {
                                this.Destroy();
                            }
                        }
                        else
                        {
                            if (type == typeof(Player) || type.IsSubclassOf(typeof(Player)))
                            {
                                if (this.cursedState != null)
                                {
                                    if (!instanceMeetingPlacedAtPreviousPosition(other))
                                    {
                                        Player otherPlayer = (Player)other;
                                        if (otherPlayer.grabbingState == GrabbingState.nothing || otherPlayer.grabbingState == GrabbingState.grabbing)
                                        {
                                            if (noMoreSkullCollisionEventsForNow)
                                            {
                                                noMoreSkullCollisionEventsForNow = false;
                                            }
                                            else
                                            {
                                                Skull.Curses? othersCursedState = otherPlayer.cursedState;

                                                otherPlayer.CurseResetter(false);
                                                Skull.CurseThisPlayer(otherPlayer, (Skull.Curses)cursedState);

                                                this.CurseResetter(false);
                                                if (othersCursedState != null)
                                                {
                                                    Skull.CurseThisPlayer(this, (Skull.Curses)othersCursedState);
                                                }
                                                otherPlayer.noMoreSkullCollisionEventsForNow = true;
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

        public void ReleasePowerUps()
        {
            // MUST FIX THIS
            
            List<Point> possiblePowerUpPositions = new List<Point>();
            for (int y = 4; y <= 14; y++)
            {
                for (int x = 4; x <= 20; x++)
                {
                    if (room.instanceMeetingCollisionMaskPlacedAt<Wall>(new Vector2(x * DEFAULT_GRID, y * DEFAULT_GRID), defaultCollisionMask) == null &&
                        room.instanceMeetingCollisionMaskPlacedAt<Player>(new Vector2(x * DEFAULT_GRID, y * DEFAULT_GRID), defaultCollisionMask) == null &&
                        room.instanceMeetingCollisionMaskPlacedAt<PowerUp>(new Vector2(x * DEFAULT_GRID, y * DEFAULT_GRID), defaultCollisionMask) == null &&
                        room.instanceMeetingCollisionMaskPlacedAt<Bomb>(new Vector2(x * DEFAULT_GRID, y * DEFAULT_GRID), defaultCollisionMask) == null)
                    {
                        possiblePowerUpPositions.Add(new Point(x, y));
                    }
                }
            }

            foreach (PowerUp.Kinds powerUp in powerUpContainer)
            {
                if (possiblePowerUpPositions.Count > 0)
                {
                    Point powerUpPoint = possiblePowerUpPositions[new Random().Next(possiblePowerUpPositions.Count)];
                    Vector2 powerUpPosition = new Vector2(powerUpPoint.X * DEFAULT_GRID, powerUpPoint.Y * DEFAULT_GRID);
                    possiblePowerUpPositions.Remove(powerUpPoint);
                    switch (powerUp)
                    {
                        case PowerUp.Kinds.bombKick:
                            new BombKick(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.bombUp:
                            new BombUp(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.dangerousBomb:
                            new DangerousBombPowerUp(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.fireUp:
                            new FireUp(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.fullFire:
                            new FullFire(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.lineBomb:
                            new LineBomb(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.passThroughBomb:
                            new PassThroughBombPowerUp(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.powerGlove:
                            new PowerGlove(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.remoteBomb:
                            new RemoteBombPowerUp(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.skull:
                            // WHAT THE HECK?
                            // new Skull(this.Game, powerUpPosition);
                            break;
                        case PowerUp.Kinds.speedUp:
                            new SpeedUp(this.room, powerUpPosition);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("O NOES A POWER-UP HAS BEEN LOST!");
                }
            }
        }

        public override void Destroy()
        {
            if (!dead)
            {
                dead = true;

                if (((Match)room).root.revengeSettings != BattleManager.RevengeSettings.off)
                {
                    AvengeMyself();
                }

                this.ThrowAway();

                this.ReleasePowerUps();

                this.CurseResetter(false);

                PlayerDeathAnimation playerDA = new PlayerDeathAnimation(this.room, textureImage, frameSize, scale, origin, position);//, winAnimationSequences[0]);
            }
            base.Destroy();
        }

        public void AvengeMyself()
        {
            float ytop;
            float xright;
            float ybottom;
            float xleft;
            float linearPosition;

            Match m = (Match)room;

            ytop = this.position.Y - m.arenaTopmostRow * DEFAULT_GRID;
            xright = (m.arenaRightmostColumn + 1) * DEFAULT_GRID - this.position.X;
            ybottom = (m.arenaBottommostRow + 1) * DEFAULT_GRID - this.position.Y;
            xleft = this.position.X - m.arenaLeftmostColumn * DEFAULT_GRID;

            // which one is the smallest?

            if (ytop < xright && ytop < ybottom && ytop < xleft) // ytop < all?
            {
                linearPosition = xleft;
            }
            else
            {
                if (xright < ybottom && xright < xleft)          // xright < all?
                {
                    linearPosition = m.arenaColumnCount * DEFAULT_GRID + ytop;
                }
                else
                {
                    if (ybottom < xleft)                         // ybottom < all?
                    {
                        linearPosition = (m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID + xright;
                    }
                    else                                         // xleft < all?
                    {
                        linearPosition = (2 * m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID + ybottom;
                    }
                }
            }

            new Revenger(room, linearPosition, id, input);
        }

        private int Animation()
        {
            if (cont >= speedSprite)
            {
                if (px == pxStandingStill && !isRight)
                {
                    px = pxRight;
                    isRight = true;
                }
                else
                    if (px == pxStandingStill && isRight)
                    {
                        px = pxLeft;
                        isRight = false;
                    }
                    else
                        if (px == pxRight)
                            px = 0;
                        else
                            if (px == pxLeft)
                                px = 0;

                cont = 0;
            }
            cont++;
            return px;
        }

        /* public void setCollisionOffset(int collision)
        {
            this.collisionOffset = collision;
        }

        public int getCollisionOffset()
        {
            return this.collisionOffset;
        } */

        /* public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                (int)position.X + (collisionOffset+2),
                (int)position.Y + collisionOffset,
                frameSize.X - (collisionOffset+2),
                frameSize.Y - (collisionOffset * 5));
            }
        } */

        /* public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.Draw(gameTime, spriteBatch, new Rectangle(currentFrame.X * frameSize.X,
            currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y));
            spriteBatch.Draw(textureImage,
            position,
            new Rectangle(currentFrame.X * frameSize.X,
            currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
            Color.White, 0, origin,
            1.3f, SpriteEffects.None, position.Y / 2000);

        } */
        public override void Draw(GameTime gameTime)
        {
            if (cursedState != Skull.Curses.invisibility)
            {
                base.Draw(gameTime);
            }
        }

        public void OhNoNotInTheHead()
        {
            LoseRandomPowerUp();
            Console.WriteLine("O NOES NOT IN DA HEAD");
        }

        public void LoseRandomPowerUp()
        {
            List<Point> possiblePowerUpPositions = new List<Point>();
            for (int y = 4; y <= 14; y++)
            {
                for (int x = 4; x <= 20; x++)
                {
                    if (room.instanceMeetingCollisionMaskPlacedAt<Wall>(new Vector2(x * DEFAULT_GRID, y * DEFAULT_GRID), defaultCollisionMask) == null &&
                        room.instanceMeetingCollisionMaskPlacedAt<Player>(new Vector2(x * DEFAULT_GRID, y * DEFAULT_GRID), defaultCollisionMask) == null &&
                        room.instanceMeetingCollisionMaskPlacedAt<PowerUp>(new Vector2(x * DEFAULT_GRID, y * DEFAULT_GRID), defaultCollisionMask) == null &&
                        room.instanceMeetingCollisionMaskPlacedAt<Bomb>(new Vector2(x * DEFAULT_GRID, y * DEFAULT_GRID), defaultCollisionMask) == null)
                    {
                        possiblePowerUpPositions.Add(new Point(x, y));
                    }
                }
            }

            if (possiblePowerUpPositions.Count > 0)
            {
                Point powerUpPoint = possiblePowerUpPositions[new Random().Next(possiblePowerUpPositions.Count)];
                Vector2 powerUpPosition = new Vector2(powerUpPoint.X * DEFAULT_GRID, powerUpPoint.Y * DEFAULT_GRID);
                possiblePowerUpPositions.Remove(powerUpPoint);
                if (powerUpContainer.Count > 0)
                {
                    PowerUp.Kinds randomPowerUp = powerUpContainer[new Random().Next(powerUpContainer.Count)];
                    powerUpContainer.Remove(randomPowerUp);
                    switch (randomPowerUp)
                    {
                        case PowerUp.Kinds.bombKick:
                            this.bombKick = false;
                            foreach (PowerUp.Kinds kind in powerUpContainer)
                            {
                                if (kind == PowerUp.Kinds.bombKick)
                                {
                                    this.bombKick = true;
                                    break;
                                }
                            }
                            new BombKick(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.bombUp:
                            if (maxBombCount > 1)
                            {
                                this.maxBombCount -= 1;
                            }
                            new BombUp(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.dangerousBomb:
                            this.bombKind = Bomb.Kinds.NormalBomb;
                            foreach (PowerUp.Kinds kind in powerUpContainer)
                            {
                                switch (kind)
                                {
                                    case PowerUp.Kinds.dangerousBomb:
                                        this.bombKind = Bomb.Kinds.DangerousBomb;
                                        break;
                                    case PowerUp.Kinds.passThroughBomb:
                                        this.bombKind = Bomb.Kinds.PassThroughBomb;
                                        break;
                                    case PowerUp.Kinds.remoteBomb:
                                        this.bombKind = Bomb.Kinds.RemoteBomb;
                                        break;
                                }
                            }
                            new DangerousBombPowerUp(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.fireUp:
                            this.firepower = 1;
                            foreach (PowerUp.Kinds kind in powerUpContainer)
                            {
                                bool fullFireFound = false;
                                switch (kind)
                                {
                                    case PowerUp.Kinds.fireUp:
                                        firepower++;
                                        break;
                                    case PowerUp.Kinds.fullFire:
                                        firepower = INFINITY;
                                        fullFireFound = true;
                                        break;
                                }
                                if (fullFireFound)
                                {
                                    break;
                                }
                            }
                            new FireUp(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.fullFire:
                            this.firepower = 1;
                            foreach (PowerUp.Kinds kind in powerUpContainer)
                            {
                                bool fullFireFound = false;
                                switch (kind)
                                {
                                    case PowerUp.Kinds.fireUp:
                                        firepower++;
                                        break;
                                    case PowerUp.Kinds.fullFire:
                                        firepower = INFINITY;
                                        fullFireFound = true;
                                        break;
                                }
                                if (fullFireFound)
                                {
                                    break;
                                }
                            }
                            new FullFire(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.lineBomb:
                            this.lineBomb = false;
                            foreach (PowerUp.Kinds kind in powerUpContainer)
                            {
                                if (kind == PowerUp.Kinds.lineBomb)
                                {
                                    this.lineBomb = true;
                                    break;
                                }
                            }
                            new LineBomb(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.passThroughBomb:
                            this.bombKind = Bomb.Kinds.NormalBomb;
                            foreach (PowerUp.Kinds kind in powerUpContainer)
                            {
                                switch (kind)
                                {
                                    case PowerUp.Kinds.dangerousBomb:
                                        this.bombKind = Bomb.Kinds.DangerousBomb;
                                        break;
                                    case PowerUp.Kinds.passThroughBomb:
                                        this.bombKind = Bomb.Kinds.PassThroughBomb;
                                        break;
                                    case PowerUp.Kinds.remoteBomb:
                                        this.bombKind = Bomb.Kinds.RemoteBomb;
                                        break;
                                }
                            }
                            new PassThroughBombPowerUp(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.powerGlove:
                            this.powerGlove = false;
                            foreach (PowerUp.Kinds kind in powerUpContainer)
                            {
                                if (kind == PowerUp.Kinds.powerGlove)
                                {
                                    this.powerGlove = true;
                                    break;
                                }
                            }
                            new PowerGlove(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.remoteBomb:
                            this.bombKind = Bomb.Kinds.NormalBomb;
                            foreach (PowerUp.Kinds kind in powerUpContainer)
                            {
                                switch (kind)
                                {
                                    case PowerUp.Kinds.dangerousBomb:
                                        this.bombKind = Bomb.Kinds.DangerousBomb;
                                        break;
                                    case PowerUp.Kinds.passThroughBomb:
                                        this.bombKind = Bomb.Kinds.PassThroughBomb;
                                        break;
                                    case PowerUp.Kinds.remoteBomb:
                                        this.bombKind = Bomb.Kinds.RemoteBomb;
                                        break;
                                }
                            }
                            new RemoteBombPowerUp(this.room, powerUpPosition);
                            break;
                        case PowerUp.Kinds.skull:
                            // WHAT THE HECK?
                            // new Skull(this.Game, powerUpPosition);
                            break;
                        case PowerUp.Kinds.speedUp:
                            if (this.speed > SpeedUp.SPEED_INCREASE_AMOUNT)
                            {
                                this.speed -= SpeedUp.SPEED_INCREASE_AMOUNT;
                            }
                            new SpeedUp(this.room, powerUpPosition);
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("O NOES A POWER-UP HAS BEEN LOST!");
            }
        }
    }
}
