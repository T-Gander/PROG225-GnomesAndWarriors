using PROG225_GnomesAndWarriors.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG225_GnomesAndWarriors
{
    internal class HealthVial : IVial
    {
        PictureBox IVial.VialPicture { get; set; }
        public Rectangle Bounds { get; set; }
        public Point Location { get; set; }

        public HealthVial() 
        {
            Random rnd = new Random();

            Location = new Point(rnd.Next(frmGameScreen.GameScreen.Width), rnd.Next(frmGameScreen.GameScreen.Height));

            ((IVial)this).VialPicture = new PictureBox()
            {
                Size = new Size(17, 25),
                Image = Resources.HealthPotionSmall,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = Location
            };

            Bounds = new Rectangle(Location, new Size(17, 25));
        }
    }
}
