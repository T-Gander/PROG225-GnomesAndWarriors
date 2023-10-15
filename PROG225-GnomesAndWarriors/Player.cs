using System;
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
        private int level;
        public PictureBox PlayerPicture { get; set; }
        private Image[] rightGifArray, leftGifArray, currentGifArray;

        public Player()
        {
            rightGifArray = GetFramesFromAnimatedGIF(Properties.Resources.Mage);
            currentGifArray = GetFramesFromAnimatedGIF(Properties.Resources.Mage);
            leftGifArray = GetFramesFromAnimatedGIF(Properties.Resources.MageFlipped);

            level = 1;
            Health = 100;
            Damage = 5;
            Agility = 5;
            
            Location = new Point(frmGameScreen.MyScreen.Bounds.Width / 2, frmGameScreen.MyScreen.Bounds.Height / 2);

            PlayerPicture = new PictureBox
            {
                Size = new Size(60, 80),
                Image = Properties.Resources.Mage,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = Location
            };
        }

        public static void LevelUp()
        {

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
                
            }
            else if (frmGameScreen.GameScreen.RightPressed)
            {
                currentGifArray = rightGifArray;
            }
            
            PlayerPicture.Image = currentGifArray[pictureIndex];

            if(currentWaitFrame == WAITFRAMES)
            {
                pictureIndex++;
                currentWaitFrame = 0;
            }
            else
            {
                currentWaitFrame++;
            }

            PlayerPicture.Invalidate();
        }

        public override void Move()
        {
            if (frmGameScreen.GameScreen.UpPressed)
            {
                Location = new Point(Location.X, Location.Y - Agility);
            }
            if (frmGameScreen.GameScreen.DownPressed)
            {
                Location = new Point(Location.X, Location.Y + Agility);
            }
            if (frmGameScreen.GameScreen.LeftPressed)
            {
                Location = new Point(Location.X - Agility, Location.Y);
                PlayerPicture.Image = Properties.Resources.MageFlipped;
            }
            if (frmGameScreen.GameScreen.RightPressed)
            {
                Location = new Point(Location.X + Agility, Location.Y);
                PlayerPicture.Image = Properties.Resources.Mage;
            }

            PlayerPicture.Location = Location;
        }
    }
}
