using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.Threading;

namespace PROG225_GnomesAndWarriors
{
    public partial class frmGameScreen : Form
    {
        public int GameLevel = 1;
        public Player Player1;
        private int gameTime = 0;
        private Point mouseLocation;
        private int mouseHeldCounter = 0;
        private PaintEventArgs heartbeatArgs;
        private int numberOfEnemiesToSpawn = 0;
        private List<Enemy> enemies = new List<Enemy>();
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

            //Need a health bar and charge level.
            //Also need a player level indicator.
            //Need powerups.
            //

            lblScore = new Label()
            {
                Size = new Size(350, 40),
                ForeColor = Color.Red,
                Font = new Font(FontFamily.GenericMonospace, 30),
                Text = $"Score: {Score}",
                BackColor = Color.Transparent
            };

            Controls.Add(lblScore);
            Controls.Add(Player1.PlayerPicture);

            NewLevel();
        }

        private void NewLevel()
        {
            numberOfEnemiesToSpawn = gameTime;

            for(int i = 0; i < 10; i++)
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
            gameTime++;

            if (gameTime % 200 == 0)
            {
                NewLevel();
                GameLevel++;
            }

            if(lblScore != null)
            {
                lblScore.Text = $"Score: {Score}";
                lblScore.Location = new Point((GameScreen.Width/2) - (lblScore.Width/2) , 100);
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