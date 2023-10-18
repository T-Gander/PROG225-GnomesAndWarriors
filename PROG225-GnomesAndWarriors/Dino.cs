using PROG225_GnomesAndWarriors.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG225_GnomesAndWarriors
{
    public class Dino : Enemy
    {
        public Dino()
        {
            leftImageArray = new Image[] { Resources.DinoBaseFlipped };
            rightImageArray = new Image[] { Resources.DinoBase };

            EnemyPicture = new PictureBox
            {
                Size = new Size(60, 80),
                Image = Resources.DinoBase,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = Location
            };

            Health = (int)(frmGameScreen.GameScreen.GameLevel * 0.1m);
            Damage = (int)(frmGameScreen.GameScreen.GameLevel * 0.1m);
            Experience = (int)(frmGameScreen.GameScreen.GameLevel * 0.1m);

            EnemyList.Add(this);
            RefreshEnemyHeartbeats();
        }

        public void UpdateEnemyPicture()
        {
            if (pictureIndex == 6) pictureIndex = 0;

            if (frmGameScreen.GameScreen.LeftPressed) currentImageArray = leftImageArray;
            else if (frmGameScreen.GameScreen.RightPressed) currentImageArray = rightImageArray;

            EnemyPicture.Image = currentImageArray[pictureIndex];
            EnemyPicture.Invalidate();
        }

        private DinoDevil Evolve()
        {
            return new DinoDevil(this);
        }
    }
}
