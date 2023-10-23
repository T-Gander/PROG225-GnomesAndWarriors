using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace PROG225_GnomesAndWarriors
{
    public class CharacterSheet //Should probly be an interface.
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
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
                dino.Bounds = new Rectangle(new Point(-100, -100), new Size(0,0));
                frmGameScreen.GameScreen.Player1.Experience += dino.Experience;
                frmGameScreen.GameScreen.Score += dino.Experience;
                frmGameScreen.GameScreen.Heartbeat -= dino.Move;
                frmGameScreen.GameScreen.Controls.Remove(dino.EnemyPicture);
            }
            else if(cs.GetType() == typeof(Player))
            {
                frmGameScreen.GameScreen.tmrHeartbeat.Stop();

                Label gameOver = new Label()
                {
                    Size = new Size(500, 100),
                    ForeColor = Color.Red,
                    Font = new Font(FontFamily.GenericMonospace, 60),
                    Text = $"GAME OVER",
                    BackColor = Color.Transparent,
                    Location = new Point(frmGameScreen.GameScreen.Width/2 - 250, frmGameScreen.GameScreen.Height/2) 
                };

                frmGameScreen.GameScreen.Controls.Add(gameOver);
            }
        }
    }
}
