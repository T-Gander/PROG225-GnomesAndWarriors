using PROG225_GnomesAndWarriors.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG225_GnomesAndWarriors
{
    public class DinoDevil : Dino
    {
        public DinoDevil(Dino dino) : base(dino.Location) //Need to see if this works to inherit a constructor
        {
            enemyImageLeft = new Image[] {Resources.}
        }
    }
}
