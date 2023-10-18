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

        public static List<Enemy> EnemyList { get; set; } = new List<Enemy>();

        public static void RefreshEnemyHeartbeats()
        {
            EnemyList.ForEach(enemy =>
            {
                frmGameScreen.GameScreen.Heartbeat -= enemy.Move;   //removes all heartbeats except the ones that don't exist.
                frmGameScreen.GameScreen.Heartbeat += enemy.Move;   //adds all of the existing enemys heartbeats.
            });
        }

        protected int pictureIndex = 0;
        protected Image[] leftImageArray, rightImageArray, currentImageArray;
    }
}
