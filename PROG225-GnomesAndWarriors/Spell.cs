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

        private ChargeLevel chargeLevel;

        private Point location { get; set; }
        private Point finalLocation { get; set; }

        private decimal decay { get; set; }

        private decimal ratioX;
        private decimal ratioY;


        public Spell(ChargeLevel charge, Point mouseLocation, Point currentLocation)
        {
            chargeLevel = charge;
            SetSpeedRatios();

            finalLocation = mouseLocation;
            location = currentLocation;

            decimal diffX = location.X - finalLocation.X;

            switch (diffX)
            {
                case >= 0 and <100:
                    ratioX = 0;
                    break;

                case >= 101:
                    ratioX = -ratioX;
                    break;

                case < 0 and > -100:
                    ratioX = 0;
                    break;

                case < -100:
                    break;
            }

            decimal diffY = location.Y - finalLocation.Y;

            switch (diffY)
            {
                case >= 0 and < 100:
                    ratioY = 0;
                    break;

                case >= 101:
                    ratioY = -ratioY;
                    break;

                case < 0 and > -100:
                    ratioY = 0;
                    break;

                case < -100:
                    break;
            }

            decay = 1000;

        }

        private void SetSpeedRatios()
        {
            switch (chargeLevel)
            {
                case ChargeLevel.Level1:
                    ratioY = 5;
                    ratioX = 5;
                    break;

                case ChargeLevel.Level2:
                    ratioY = 6;
                    ratioX = 6;
                    break;

                case ChargeLevel.Level3:
                    ratioY = 8;
                    ratioX = 8;
                    break;

                case ChargeLevel.Level4:
                    ratioY = 10;
                    ratioX = 10;
                    break;

                case ChargeLevel.Level5:
                    ratioY = 14;
                    ratioX = 14;
                    break;
            }
        }

        public void Move(PaintEventArgs e)
        {
            Pen myPen = new Pen(Color.Blue, 1);

            if (decay != 0)
            {
                Point newLocation = new Point();
                newLocation.X = (int)(location.X + ratioX);
                newLocation.Y = (int)(location.Y + ratioY);

                location = newLocation;

                try
                {
                    switch (chargeLevel)
                    {
                        case ChargeLevel.Level1:
                            e.Graphics.DrawEllipse(myPen, new Rectangle(location.X, location.Y, 2, 2));
                            break;

                        case ChargeLevel.Level2:
                            e.Graphics.DrawEllipse(myPen, new Rectangle(location.X - 3, location.Y - 3, 5, 5));
                            break;

                        case ChargeLevel.Level3:
                            e.Graphics.DrawEllipse(myPen, new Rectangle(location.X - 6, location.Y - 6, 8, 8));
                            break;

                        case ChargeLevel.Level4:
                            e.Graphics.DrawEllipse(myPen, new Rectangle(location.X - 13, location.Y - 13, 15, 15));
                            break;

                        case ChargeLevel.Level5:
                            e.Graphics.DrawEllipse(myPen, new Rectangle(location.X - 13, location.Y - 13, 15, 15));
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
                frmGameScreen.GameScreen.HeartbeatPaintEvent -= this.Move;
            }
        }
    }
}
