using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatchGame
{
    public partial class Form1 : Form
    {

        private Timer gameTimer = new Timer();
        private Timer spawnTimer = new Timer();
        private Timer countdownTimer = new Timer();
        private Random rand = new Random();

        Rectangle player = new Rectangle(190, 190, 20, 20);
        List<Rectangle> objects = new List<Rectangle>();

        int score = 0;
        int timeLeft = 60;
        int spawnInterval = 1000;

        bool moveUp, moveDown, moveLeft, moveRight;

        public Form1()
        {
            InitializeComponent();

            this.KeyPreview = true;

            gameTimer.Interval = 20;
            gameTimer.Tick += GameTick;

            spawnTimer.Interval = spawnInterval;
            spawnTimer.Tick += SpawnObject;

            countdownTimer.Interval = 1000;
            countdownTimer.Tick += UpdateTimer;

            gamePanel.Paint += GamePanel_Paint;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            score = 0;
            timeLeft = 60;
            spawnInterval = 1000;
            player.X = 190;
            player.Y = 190;
            objects.Clear();

            lblScore.Text = "Счёт: 0";
            lblTime.Text = "Время: 60";

            gameTimer.Start();
            spawnTimer.Start();
            countdownTimer.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) moveUp = true;
            if (e.KeyCode == Keys.Down) moveDown = true;
            if (e.KeyCode == Keys.Left) moveLeft = true;
            if (e.KeyCode == Keys.Right) moveRight = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) moveUp = false;
            if (e.KeyCode == Keys.Down) moveDown = false;
            if (e.KeyCode == Keys.Left) moveLeft = false;
            if (e.KeyCode == Keys.Right) moveRight = false;
        }

        private void GameTick(object sender, EventArgs e)
        {
            int speed = 5;

            if (moveUp && player.Y > 0) player.Y -= speed;
            if (moveDown && player.Y < gamePanel.Height - player.Height) player.Y += speed;
            if (moveLeft && player.X > 0) player.X -= speed;
            if (moveRight && player.X < gamePanel.Width - player.Width) player.X += speed;

            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (player.IntersectsWith(objects[i]))
                {
                    objects.RemoveAt(i);
                    score++;
                    lblScore.Text = $"Счёт: {score}";
                }
            }

            gamePanel.Invalidate();
        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            btnStart.Click += btnStart_Click;
        }

        private void SpawnObject(object sender, EventArgs e)
        {
            int x = rand.Next(gamePanel.Width - 15);
            int y = rand.Next(gamePanel.Height - 15);
            objects.Add(new Rectangle(x, y, 15, 15));
        }

        private void UpdateTimer(object sender, EventArgs e)
        {
            timeLeft--;
            lblTime.Text = $"Время: {timeLeft}";

            if (timeLeft <= 0)
            {
                gameTimer.Stop();
                spawnTimer.Stop();
                countdownTimer.Stop();
                MessageBox.Show($"Игра окончена! Ваш счёт: {score}");
            }
            else if (spawnInterval > 200)
            {
                spawnInterval -= 50;
                spawnTimer.Interval = spawnInterval;
            }
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Blue, player);
            foreach (var obj in objects)
            {
                e.Graphics.FillEllipse(Brushes.Red, obj);
            }
        }
    }
}
