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
        protected Image[] enemyImageRight = { Resources.DinoBase };
        protected Image[] enemyImageLeft = { Resources.DinoBaseFlipped };

        public Dino(Point SpawnLocation)
        {
            currentImageArray = enemyImageRight;
            leftImageArray = enemyImageLeft;
            rightImageArray = enemyImageRight;

            EnemyPicture = new PictureBox
            {
                Size = new Size(60, 80),
                Image = Resources.DinoBase,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = Location
            };

            Bounds = new Rectangle(EnemyPicture.Location,EnemyPicture.Size);

            Health = (int)(frmGameScreen.GameScreen.GameLevel * 1);
            Damage = (int)(frmGameScreen.GameScreen.GameLevel * 1);
            Experience = (int)(frmGameScreen.GameScreen.GameLevel * 1);
            Location = SpawnLocation;

            frmGameScreen.GameScreen.Heartbeat += Move;
            frmGameScreen.GameScreen.Heartbeat += CheckDinoEvolution;

        }

        private DinoDevil Evolve()
        {
            return new DinoDevil(this);
        }

        private void CheckDinoEvolution()
        {
            if (Experience > 5)
            {
                frmGameScreen.GameScreen.Heartbeat -= CheckDinoEvolution;
                Evolve();
            }
        }



    }
}
