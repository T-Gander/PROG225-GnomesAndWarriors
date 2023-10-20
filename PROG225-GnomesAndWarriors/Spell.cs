using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG225_GnomesAndWarriors
{
    public class Spell
    {
        public enum ChargeLevel { Level1, Level2, Level3, Level4, Level5 };
        public static List<Spell> ActiveSpells = new List<Spell>();
        public Rectangle Bounds;

        public int Damage { get; set; }

        private ChargeLevel chargeLevel;

        private Point location { get; set; }
        private Point finalLocation { get; set; }

        private decimal decay { get; set; }

        private decimal speedRatioX;
        private decimal speedRatioY;


        public Spell(ChargeLevel charge, Point mouseLocation, Point currentLocation)
        {
            chargeLevel = charge;
            SetSpellParameters(Damage);

            finalLocation = mouseLocation;
            location = currentLocation;

            decimal diffX = location.X - finalLocation.X;

            switch (diffX)
            {
                case >= 0 and <100:
                    speedRatioX = 0;
                    break;

                case >= 101:
                    speedRatioX = -speedRatioX;
                    break;

                case < 0 and > -100:
                    speedRatioX = 0;
                    break;

                case < -100:
                    break;
            }

            decimal diffY = location.Y - finalLocation.Y;

            switch (diffY)
            {
                case >= 0 and < 100:
                    speedRatioY = 0;
                    break;

                case >= 101:
                    speedRatioY = -speedRatioY;
                    break;

                case < 0 and > -100:
                    speedRatioY = 0;
                    break;

                case < -100:
                    break;
            }

            ActiveSpells.Add(this);
        }

        private void SetSpellParameters(int damage)
        {
            switch (chargeLevel)
            {
                case ChargeLevel.Level1:
                    speedRatioY = 5;
                    speedRatioX = 5;
                    decay = 10;
                    Damage = damage / 5;
                    break;

                case ChargeLevel.Level2:
                    speedRatioY = 6;
                    speedRatioX = 6;
                    decay = 20;
                    Damage = damage / 3;
                    break;

                case ChargeLevel.Level3:
                    speedRatioY = 8;
                    speedRatioX = 8;
                    decay = 40;
                    Damage = damage / 2;
                    break;

                case ChargeLevel.Level4:
                    speedRatioY = 10;
                    speedRatioX = 10;
                    decay = 60;
                    Damage = damage;
                    break;

                case ChargeLevel.Level5:
                    speedRatioY = 14;
                    speedRatioX = 14;
                    decay = 80;
                    Damage = (int)(damage*1.5m);
                    break;
            }
        }

        public void Move(PaintEventArgs e)
        {
            Pen myPen = new Pen(Color.Blue, 1);

            if (decay > 0)
            {
                Point newLocation = new Point();
                newLocation.X = (int)(location.X + speedRatioX);
                newLocation.Y = (int)(location.Y + speedRatioY);

                location = newLocation;

                try
                {
                    switch (chargeLevel)
                    {
                        case ChargeLevel.Level1:
                            e.Graphics.DrawEllipse(myPen, new Rectangle(location.X, location.Y, 2, 2));
                            Bounds = new Rectangle(newLocation, new Size(2, 2));
                            break;

                        case ChargeLevel.Level2:
                            e.Graphics.DrawEllipse(myPen, new Rectangle(location.X - 3, location.Y - 3, 5, 5));
                            Bounds = new Rectangle(newLocation, new Size(5, 5));
                            break;

                        case ChargeLevel.Level3:
                            e.Graphics.DrawEllipse(myPen, new Rectangle(location.X - 6, location.Y - 6, 8, 8));
                            Bounds = new Rectangle(newLocation, new Size(8, 8));
                            break;

                        case ChargeLevel.Level4:
                            e.Graphics.DrawEllipse(myPen, new Rectangle(location.X - 13, location.Y - 13, 15, 15));
                            Bounds = new Rectangle(newLocation, new Size(15, 15));
                            break;

                        case ChargeLevel.Level5:
                            e.Graphics.DrawEllipse(myPen, new Rectangle(location.X - 16, location.Y - 16, 18, 18));
                            Bounds = new Rectangle(newLocation, new Size(18, 18));
                            break;
                    }
                    decay -= 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DissolveSpell();
            }
        }

        public void DissolveSpell()
        {
            frmGameScreen.GameScreen.HeartbeatPaintEvent -= Move;
            ActiveSpells.Remove(this);
        }
    }
}
