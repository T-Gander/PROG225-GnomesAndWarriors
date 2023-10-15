using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG225_GnomesAndWarriors
{
    public class CharacterSheet
    {
        public int Health { get; set; }
        public int Damage { get; set; }
        public int Agility { get; set; }
        public Point Location { get; set; }

        public virtual void Move()
        {

        }

        public virtual void Attack()
        {

        }

        public virtual void Die()
        {

        }
    }
}
