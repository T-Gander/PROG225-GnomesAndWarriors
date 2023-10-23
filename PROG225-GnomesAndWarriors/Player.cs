using System;
using PROG225_GnomesAndWarriors.Properties;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PROG225_GnomesAndWarriors
{
    public class Player : CharacterSheet
    {
        public string Name { get; set; }
        private int pictureIndex = 0;
        private int currentWaitFrame = 0;
        private const int WAITFRAMES = 6;
        private int spellLocationX;
        private int spellLocationY;
        private const int SPELLOFFSETRIGHT = 68;
        private const int SPELLOFFSETLEFT = 0;
        private int experienceCap = 10;
        private Spell.ChargeLevel currentCharge;
        private int invulnerableCounter = 100;

        public ProgressBar HealthBar = new ProgressBar();
        public Rectangle Bounds { get; set; }
        public int Level;
        public bool Invulnerable = false;
        public bool PickedUpVial = false;
        public bool LevelingUp = false;

        public PictureBox PlayerPicture { get; set; }
        private Image[] rightGifArray, leftGifArray, currentGifArray;

        public Player()
        {
            rightGifArray = GetFramesFromAnimatedGIF(Resources.Mage);
            currentGifArray = GetFramesFromAnimatedGIF(Resources.Mage);
            leftGifArray = GetFramesFromAnimatedGIF(Resources.MageFlipped);

            Experience = 0;

            Level = 1;
            MaxHealth = 100;
            Health = MaxHealth;
            Damage = 1;
            Agility = 7;
            
            Location = new Point(frmGameScreen.MyScreen.Bounds.Width / 2, frmGameScreen.MyScreen.Bounds.Height / 2);

            PlayerPicture = new PictureBox
            {
                Size = new Size(60, 80),
                Image = Resources.Mage,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = Location
            };

            HealthBar = new ProgressBar()
            {
                Maximum = MaxHealth,
                Value = Health,
                Location = new Point(PlayerPicture.Location.X, PlayerPicture.Location.Y + 90),
                Width = PlayerPicture.Width,
                Height = 10
            };

            spellLocationX = Location.X + SPELLOFFSETRIGHT;
            spellLocationY = Location.Y + SPELLOFFSETRIGHT;

            Bounds = new Rectangle(Location, new Size(45,60));
        }

        public void CheckLevelUp()
        {
            if(Experience >= experienceCap)
            {
                LevelingUp = true;
                Level++;
                MaxHealth = (int)(MaxHealth * 1.2);
                Health = MaxHealth;
                HealthBar.Maximum = MaxHealth;
                HealthBar.Value = Health;
                Damage *= 2;

                Experience = 0;
                experienceCap = (int)(experienceCap * 2);
            }
        }

        public static Image[] GetFramesFromAnimatedGIF(Image IMG)       //Borrowed from stack overflow
        {
            List<Image> IMGs = new List<Image>();
            int Length = IMG.GetFrameCount(FrameDimension.Time);

            for (int i = 0; i < Length; i++)
            {
                IMG.SelectActiveFrame(FrameDimension.Time, i);
                IMGs.Add(new Bitmap(IMG));
            }

            return IMGs.ToArray();
        }

        public void UpdatePlayerPicture()
        {
            if (pictureIndex == 6) pictureIndex = 0;

            if (frmGameScreen.GameScreen.LeftPressed)
            {
                currentGifArray = leftGifArray;
                spellLocationX = Location.X + SPELLOFFSETLEFT;
                spellLocationY = Location.Y + SPELLOFFSETRIGHT;
            }
            else if (frmGameScreen.GameScreen.RightPressed)
            {
                currentGifArray = rightGifArray;
                spellLocationX = Location.X + SPELLOFFSETRIGHT;
                spellLocationY = Location.Y + SPELLOFFSETRIGHT;
            }

            if (frmGameScreen.GameScreen.UpPressed) spellLocationY = Location.Y + SPELLOFFSETRIGHT;
            else if (frmGameScreen.GameScreen.DownPressed) spellLocationY = Location.Y + SPELLOFFSETRIGHT;
            
            PlayerPicture.Image = currentGifArray[pictureIndex];

            if(currentWaitFrame == WAITFRAMES)
            {
                pictureIndex++;
                currentWaitFrame = 0;
            }
            else currentWaitFrame++;

            PlayerPicture.Invalidate();
        }

        public override void Move()
        {
            if (frmGameScreen.GameScreen.UpPressed) Location = new Point(Location.X, Location.Y - Agility);
            if (frmGameScreen.GameScreen.DownPressed) Location = new Point(Location.X, Location.Y + Agility);

            if (frmGameScreen.GameScreen.LeftPressed)
            {
                Location = new Point(Location.X - Agility, Location.Y);
                PlayerPicture.Image = Resources.MageFlipped;
            }
            if (frmGameScreen.GameScreen.RightPressed)
            {
                Location = new Point(Location.X + Agility, Location.Y);
                PlayerPicture.Image = Resources.Mage;
            }

            if (Invulnerable)
            {
                if (invulnerableCounter != 0) invulnerableCounter--;
                else
                {
                    invulnerableCounter = 100;
                    Invulnerable = false;
                }
            }

            UpdatePlayerPicture();
            CheckLevelUp();

            PlayerPicture.Location = Location;
            Bounds = new Rectangle(Location, new Size(45, 60));
        }

        public void ChargeSpell(PaintEventArgs e, int mouseHeldCounter)
        {
            Brush myBrush = Brushes.LightBlue;

            switch (mouseHeldCounter)
            {
                case <5:
                    e.Graphics.FillEllipse(myBrush, new Rectangle(spellLocationX, spellLocationY, 2, 2));
                    currentCharge = Spell.ChargeLevel.Level1;
                    break;

                case <10:
                    e.Graphics.FillEllipse(myBrush, new Rectangle(spellLocationX - 3, spellLocationY - 3, 5, 5));
                    currentCharge = Spell.ChargeLevel.Level2;
                    break;

                case <15:
                    e.Graphics.FillEllipse(myBrush, new Rectangle(spellLocationX - 6, spellLocationY - 6, 8, 8));
                    currentCharge = Spell.ChargeLevel.Level3;
                    break;

                case <20:
                    e.Graphics.FillEllipse(myBrush, new Rectangle(spellLocationX - 13, spellLocationY - 13, 15, 15));
                    currentCharge = Spell.ChargeLevel.Level4;
                    break;

                case >25:
                    e.Graphics.FillEllipse(myBrush, new Rectangle(spellLocationX - 13, spellLocationY - 13, 15, 15));
                    currentCharge = Spell.ChargeLevel.Level5;
                    break;
            }
        }

        public void ReleaseSpell()
        {
            Spell newSpell = new Spell(currentCharge, frmGameScreen.GetMouseLocation(), new Point(spellLocationX, spellLocationY));
            frmGameScreen.GameScreen.HeartbeatPaintEvent += newSpell.Move;
        }
    }
}
