using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG225_GnomesAndWarriors
{
    public class Enemy : CharacterSheet
    {
        public PictureBox EnemyPicture { get; set; }

        protected static int leftSpawnArea = 200;
        protected static int rightSpawnArea = frmGameScreen.GameScreen.Width-200;
        protected static int topSpawnArea = 200;
        protected static int bottomSpawnArea = frmGameScreen.GameScreen.Height-200;

        public Rectangle Bounds { get; set; }

        private int xSpeed = 5;
        private int ySpeed = 5;

        protected int pictureIndex = 0;
        protected Image[] leftImageArray, rightImageArray, currentImageArray;

        public void UpdateEnemyPicture()
        {
            Point newLocation = new Point(Location.X + xSpeed, Location.Y + ySpeed);
            EnemyPicture.Image = currentImageArray[pictureIndex];
            Location = newLocation;
            EnemyPicture.Location = newLocation;
            EnemyPicture.Invalidate();
            Bounds = new Rectangle(newLocation, EnemyPicture.Size);
        }

        private void CheckHealth()
        {
            if (Health <= 0)
            {
                Die(this);
            }
        }

        public override void Move()
        {
            CheckForCollisionWithGameBounds();
            UpdateEnemyPicture();

            for(int i = 0; i < Spell.ActiveSpells.Count; i++)
            {
                Spell spell = Spell.ActiveSpells[i];

                if (Bounds.IntersectsWith(spell.Bounds))
                {
                    Health -= spell.Damage;
                    if(spell.Decay == 0 || spell.Damage == 0)
                    {
                        spell.DissolveSpell();
                        i = -1;
                    }
                    else
                    {
                        spell.Damage -= 1;
                    }
                }
            }
            CheckHealth();
        }

        private void CheckForCollisionWithGameBounds()
        {
            if (Location.X + EnemyPicture.Width >= frmGameScreen.GameScreen.Width)
            {
                Point newPoint = new Point(Location.X - 5, Location.Y);
                Location = newPoint;
                EnemyPicture.Location = newPoint;
                xSpeed *= -1;
                currentImageArray = leftImageArray;
            }
            if (Location.X <= 0)
            {
                Point newPoint = new Point(Location.X + 5, Location.Y);
                Location = newPoint;
                EnemyPicture.Location = newPoint;
                xSpeed *= -1;
                currentImageArray = rightImageArray;
            }
            if (Location.Y <= 0)
            {
                Point newPoint = new Point(Location.X, Location.Y+5);
                Location = newPoint;
                EnemyPicture.Location = newPoint;
                ySpeed *= -1;
            }
            if (Location.Y + EnemyPicture.Height >= frmGameScreen.GameScreen.Height)
            {
                Point newPoint = new Point(Location.X, Location.Y - 5);
                Location = newPoint;
                EnemyPicture.Location = newPoint;
                ySpeed *= -1;
            }
        }

        public static Point Spawn()
        {
            Random coinflip = new Random();

            rightSpawnArea = frmGameScreen.GameScreen.Width - 200;
            bottomSpawnArea = frmGameScreen.GameScreen.Height - 200;

            int xLocation = 0;
            int yLocation = 0;

            bool left = false;
            bool right = false;
            bool up = false;
            bool down = false;

            for (int i = 0; i < 4;  i++)
            {
                bool flipResult = false;
                int result = coinflip.Next(2);

                if(result > 0)
                {
                    flipResult = true;
                }

                switch (i)
                {
                    case 0:
                        left = flipResult; break;

                    case 1:
                        right = flipResult; break;

                    case 2:
                        up = flipResult; break;

                    case 3: 
                        down = flipResult; break;
                }
            }

            Random randLocation = new Random();

            if (left)
            {
                xLocation = randLocation.Next(1,leftSpawnArea);
            }
            if(right)
            {
                xLocation = randLocation.Next(rightSpawnArea);
            }
            if(up)
            {
                yLocation = randLocation.Next(1,topSpawnArea);
            }
            if(down)
            {
                yLocation = randLocation.Next(bottomSpawnArea);
            }

            return new Point(xLocation,yLocation);
        }
    }
}
