using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace AloneHero_CSharp
{
    class Game
    {
        private bool endGame;

        public Game()
        {
            endGame = false;
        }

        public bool StartGame()
        {
            RenderWindow window = new RenderWindow(new VideoMode(1200, 800), "Alone Hero");
            Clock clock = new Clock();

            // Инициализация уровней (на будущее)
            List<Level> levels = new List<Level>();
            levels.Add(new Level("map_XML_2.tmx"));

            // Карта
            Level lvl = new Level("map_XML_2.tmx");

            window.KeyPressed += Window_KeyPressed;

            // Интерфейс
            Vector2f center = window.GetView().Center;
            Vector2f size = window.GetView().Size;
            //Image heartImage = new Image("Images\\Interface\\Heart.png");
            //Texture heartTexture = new Texture(heartImage);
            //Sprite heartSprite = new Sprite(heartTexture);
            //heartSprite.TextureRect = new IntRect(22, 18, 22, 19);
            //heartSprite.Position = new Vector2f(center.X - size.X / 2, center.Y - size.Y / 2);
            //window.Draw(heartSprite);
            //window.Display();

            while (window.IsOpen)
            {
                // Время для анимации
                float time = clock.ElapsedTime.AsMicroseconds();
                clock.Restart();
                time = time / 800;

                window.DispatchEvents();

                if (Keyboard.IsKeyPressed(Keyboard.Key.Tab) || endGame)
                {
                    window.Close();
                    return true;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) return false;

                levels[0].Draw(window, time, this);
            }

            return false;
        }

        public void GameRunning()
        {
            if (StartGame())
            {
                endGame = false;
                GameRunning();
            }
        }

        public void SetEndGame(bool endGame)
        {
            this.endGame = endGame;
        }

        private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                window.Close();
            }
        }

    }
}
