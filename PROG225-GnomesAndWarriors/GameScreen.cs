using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.Numerics;
using System.Threading;

namespace PROG225_GnomesAndWarriors
{
    public partial class frmGameScreen : Form
    {
        public int GameLevel = 1;
        public Player Player1;
        private int gameTime = 0;
        private int pickupCount = 100;
        public int LevelUpCount = 100;
        private Point mouseLocation;
        private int mouseHeldCounter = 0;
        private PaintEventArgs heartbeatArgs;
        private int numberOfEnemiesToSpawn = 0;
        private List<Enemy> enemies = new List<Enemy>();
        private List<PowerVial> powerVials = new List<PowerVial>();
        private List<HealthVial> healthVials = new List<HealthVial>();

        public int Score = 0;
        public bool UpPressed, DownPressed, LeftPressed, RightPressed;
        public enum MouseHeld { Held, NotHeld, NoSpell }

        private MouseHeld mouseHeld = MouseHeld.NoSpell;

        public static string GameDirectory = Environment.CurrentDirectory;
        public static frmGameScreen GameScreen;
        public static Screen MyScreen = Screen.PrimaryScreen;

        public delegate void TimerEvent();
        public delegate void PaintTimerEvent(PaintEventArgs e);
        public event TimerEvent Heartbeat;
        public event PaintTimerEvent HeartbeatPaintEvent;

        private Label lblScore;
        private Label lblPowerUp;
        public Label lblLevelUp;

        public frmGameScreen()
        {
            GameScreen = this;
            UpPressed = false;
            DownPressed = false;
            LeftPressed = false;
            RightPressed = false;

            InitializeComponent();
            tmrHeartbeat.Interval = 25;

            Player1 = new Player();
            Heartbeat += Player1.Move;

            lblScore = new Label()
            {
                Size = new Size(350, 40),
                ForeColor = Color.Red,
                Font = new Font(FontFamily.GenericMonospace, 30),
                Text = $"Score: {Score}",
                BackColor = Color.Transparent
            };

            lblPowerUp = new Label()
            {
                Size = new Size(1200, 60),
                ForeColor = Color.Red,
                Font = new Font(FontFamily.GenericMonospace, 50),
                BackColor = Color.Transparent,
                Location = new Point((GameScreen.Width / 2) - (lblScore.Width / 2), 500)
            };

            lblLevelUp = new Label()
            {
                Size = new Size(1200, 60),
                ForeColor = Color.Red,
                Font = new Font(FontFamily.GenericMonospace, 50),
                BackColor = Color.Transparent,
                Location = new Point((GameScreen.Width / 2) - (lblScore.Width / 2), 500)
            };

            Controls.Add(lblScore);
            Controls.Add(Player1.PlayerPicture);
            Controls.Add(lblPowerUp);
            Controls.Add(lblLevelUp);
            lblPowerUp.Visible = false;
            lblLevelUp.Visible = false;

            NewLevel();
        }

        private void NewLevel()
        {
            numberOfEnemiesToSpawn = gameTime/100;

            for (int i = 0; i < numberOfEnemiesToSpawn; i++)
            {
                Dino newDino = new Dino(Enemy.Spawn());
                Controls.Add(newDino.EnemyPicture);
                enemies.Add(newDino);
            }
        }

        private void HeartbeatEventHandler()
        {
            if (Heartbeat != null)
            {
                Heartbeat.Invoke();
                Invalidate();

                if (mouseHeld == MouseHeld.Held) mouseHeldCounter++;
                else mouseHeldCounter = 0;
            }
        }

        private void HeartbeatPaintEventHandler(PaintEventArgs e)
        {
            if (HeartbeatPaintEvent != null)
            {
                HeartbeatPaintEvent.Invoke(e);

                if (mouseHeld == MouseHeld.Held) mouseHeldCounter++;
                else mouseHeldCounter = 0;
            }
        }

        private void tmrHeartbeat_Tick(object sender, EventArgs e)
        {
            HeartbeatEventHandler();    //Everything that lives needs to perform an action.
            CheckForPlayerEnemyCollision();
            CheckForPlayerVialCollision();
            gameTime++;

            Debug.WriteLine(Player1.Bounds.ToString());

            if (gameTime % 200 == 0)
            {
                NewLevel();
                GameLevel++;
            }

            if (gameTime % 400 == 0)
            {
                PowerVial pv = new PowerVial();
                IVial ivial = pv;
                powerVials.Add(pv);
                Controls.Add(ivial.VialPicture);
            }

            if (gameTime % 800 == 0)
            {
                HealthVial hv = new HealthVial();
                IVial ivial = hv;
                healthVials.Add(hv);
                Controls.Add(ivial.VialPicture);
            }

            if (lblScore != null)
            {
                lblScore.Text = $"Score: {Score}";
                lblScore.Location = new Point((GameScreen.Width / 2) - (lblScore.Width / 2), 100);
            }

            Controls.Remove(Player1.HealthBar);
            Player1.HealthBar.Location = new Point(Player1.PlayerPicture.Location.X, Player1.PlayerPicture.Location.Y + 90);
            Controls.Add(Player1.HealthBar);

            if (Player1.PickedUpVial)
            {
                lblPowerUp.Text = $"Vial! Damage is now: {Player1.Damage}!";
                lblPowerUp.Show();
                pickupCount--;

                if(pickupCount == 0)
                {
                    lblPowerUp.Hide();
                    pickupCount = 200;
                    Player1.PickedUpVial = false;
                }
            }

            if (Player1.LevelingUp && !Player1.PickedUpVial)
            {
                lblLevelUp.Text = $"Level Up! Level: {Player1.Level}!";
                lblLevelUp.Show();
                LevelUpCount--;

                if (LevelUpCount == 0)
                {
                    lblLevelUp.Hide();
                    LevelUpCount = 100;
                    Player1.LevelingUp = false;
                }
            }
        }

        public static Point GetMouseLocation()
        {
            return new Point(Cursor.Position.X, Cursor.Position.Y);
        }

        private void frmGameScreen_Paint(object sender, PaintEventArgs e)   //Borrowed from stack overflow, apparently allows images to have transparent backgrounds?
        {
            DoubleBuffered = true;
            for (int i = 0; i < Controls.Count; i++)
                if (Controls[i].GetType() == typeof(PictureBox))
                {
                    var p = Controls[i] as PictureBox;
                    p.Visible = false;
                    e.Graphics.DrawImage(p.Image, p.Left, p.Top, p.Width, p.Height);
                }

            if (mouseHeld == MouseHeld.Held) Player1.ChargeSpell(e, mouseHeldCounter);
            else if (mouseHeld == MouseHeld.NotHeld)
            {
                Player1.ReleaseSpell();
                mouseHeld = MouseHeld.NoSpell;
            }
            HeartbeatPaintEventHandler(e);
        }

        private void CheckForPlayerEnemyCollision()
        {
            enemies.ForEach(enemy =>
            {
                if (Player1.Bounds.IntersectsWith(enemy.Bounds))
                {
                    if (!Player1.Invulnerable)
                    {
                        Player1.Health -= enemy.Damage;
                        if (Player1.Health < 0) Player1.Health = 0;
                        Player1.Invulnerable = true;
                        Player1.HealthBar.Value = Player1.Health;
                    }
                }
            });

            if (Player1.Health == 0) Player1.Die(Player1);
        }

        private void CheckForPlayerVialCollision()
        {
            for (int i = 0; i < powerVials.Count; i++)
            {
                if (Player1.Bounds.IntersectsWith(powerVials[i].Bounds))
                {
                    Player1.Damage = (int)(Player1.Damage * 2);
                    Player1.PickedUpVial = true;
                    IVial ivail = powerVials[i];
                    Controls.Remove(ivail.VialPicture);
                    powerVials.Remove(powerVials[i]);
                }
            }

            for (int i = 0; i < healthVials.Count; i++)
            {
                if (Player1.Bounds.IntersectsWith(healthVials[i].Bounds))
                {
                    Player1.Health = Player1.MaxHealth;
                    Player1.HealthBar.Value = Player1.MaxHealth;
                    IVial ivail = healthVials[i];
                    Controls.Remove(ivail.VialPicture);
                    healthVials.Remove(healthVials[i]);
                }
            }
        }

        private void CheckKeyPress(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    UpPressed = true;
                    break;

                case Keys.S:
                    DownPressed = true;
                    break;

                case Keys.A:
                    LeftPressed = true;
                    break;

                case Keys.D:
                    RightPressed = true;
                    break;
            }
        }

        private void CheckKeyRelease(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    UpPressed = false;
                    break;

                case Keys.S:
                    DownPressed = false;
                    break;

                case Keys.A:
                    LeftPressed = false;
                    break;

                case Keys.D:
                    RightPressed = false;
                    break;
            }
        }

        private void frmGameScreen_KeyDown(object sender, KeyEventArgs e)
        {
            CheckKeyPress(e);
        }

        private void frmGameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            CheckKeyRelease(e);
        }

        private void frmGameScreen_MouseDown(object sender, MouseEventArgs e)
        {
            mouseHeld = MouseHeld.Held;
        }

        private void frmGameScreen_MouseUp(object sender, MouseEventArgs e)
        {
            mouseHeld = MouseHeld.NotHeld;
        }
    }
}