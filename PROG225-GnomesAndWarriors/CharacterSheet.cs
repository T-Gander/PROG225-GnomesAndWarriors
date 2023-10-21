using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG225_GnomesAndWarriors
{
    public class CharacterSheet //Should probly be an interface.
    {
        public int Health { get; set; }
        public int Damage { get; set; }
        public int Agility { get; set; }
        public Point Location { get; set; }
        public int Experience { get; set; }

        public virtual void Move()
        {

        }

        public virtual void Attack()
        {

        }

        public virtual void Die(CharacterSheet cs)
        {
            if(cs.GetType() == typeof(Dino))                    //instead of if, I should override the die method in each class?.
            {
                Dino dino = (Dino)cs;
                frmGameScreen.GameScreen.Player1.Experience += dino.Experience;
                frmGameScreen.GameScreen.Score += 1;
                frmGameScreen.GameScreen.Heartbeat -= dino.Move;
                frmGameScreen.GameScreen.Controls.Remove(dino.EnemyPicture);
            }
            else if(cs.GetType() == typeof(DinoDevil))
            {
                DinoDevil dino = (DinoDevil)cs;
                frmGameScreen.GameScreen.Player1.Experience += dino.Experience;
                frmGameScreen.GameScreen.Score += 5;
                frmGameScreen.GameScreen.Heartbeat -= dino.Move;
                frmGameScreen.GameScreen.Controls.Remove(dino.EnemyPicture);
            }
            else if(cs.GetType() == typeof(Player))
            {
                frmGameScreen.GameScreen.Heartbeat -= cs.Move;
            }
        }
    }
}
