using PROG225_GnomesAndWarriors.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROG225_GnomesAndWarriors
{
    public class Dino : Enemy
    {
        protected Image[] enemyImageRight = { Resources.DinoBase };
        protected Image[] enemyImageLeft = { Resources.DinoBaseFlipped };

        public Dino(Point SpawnLocation)
        {
            Health = (int)(frmGameScreen.GameScreen.GameLevel * 1.2);
            Damage = (int)(frmGameScreen.GameScreen.GameLevel * 10);
            Experience = (int)(frmGameScreen.GameScreen.GameLevel * 1);
            Location = SpawnLocation;

            switch (Experience)
            {
                case < 5:
                    EnemyPicture = new PictureBox
                    {
                        Size = new Size(60, 80),
                        Image = Resources.DinoBase,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Location = Location
                    };
                    break;

                case < 10:
                    EnemyPicture = new PictureBox
                    {
                        Size = new Size(72, 96),
                        Image = Resources.DinoBase,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Location = Location
                    };
                    enemyImageRight = new Image[] { Resources.DinoDevil };
                    enemyImageLeft = new Image[] { Resources.DinoDevilFlipped };
                    break;

                default:
                    EnemyPicture = new PictureBox
                    {
                        Size = new Size(100, 120),
                        Image = Resources.DinoBase,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Location = Location
                    };
                    enemyImageRight = new Image[] { Resources.DinoGod };
                    enemyImageLeft = new Image[] { Resources.DinoGodFlipped };
                    break;
            }
            
            currentImageArray = enemyImageRight;
            leftImageArray = enemyImageLeft;
            rightImageArray = enemyImageRight;
            
            Bounds = new Rectangle(EnemyPicture.Location, new Size(40, 60));
            frmGameScreen.GameScreen.Heartbeat += Move;
        }
    }
}
