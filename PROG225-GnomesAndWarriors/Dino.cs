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
        public Dino(Point SpawnLocation)
        {
            currentImageArray = new Image[] { Resources.DinoBase };
            leftImageArray = new Image[] { Resources.DinoBaseFlipped };
            rightImageArray = new Image[] { Resources.DinoBase };

            EnemyPicture = new PictureBox
            {
                Size = new Size(60, 80),
                Image = Resources.DinoBase,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = Location
            };

            Health = (int)(frmGameScreen.GameScreen.GameLevel * 1);
            Damage = (int)(frmGameScreen.GameScreen.GameLevel * 1);
            Experience = (int)(frmGameScreen.GameScreen.GameLevel * 1);
            Location = SpawnLocation;

            frmGameScreen.GameScreen.Heartbeat += Move;
        }

        private DinoDevil Evolve()
        {
            return new DinoDevil(this);
        }

        

    }
}
