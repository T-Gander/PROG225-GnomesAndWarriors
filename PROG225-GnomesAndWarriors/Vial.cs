using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG225_GnomesAndWarriors
{
    public interface IVial
    {
        public PictureBox VialPicture { get; set; }
        public Rectangle Bounds { get; set; }
        public Point Location { get; set; }
    }
}
