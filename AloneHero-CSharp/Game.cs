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

            // Интерфейс
            GameInterface gameInterface = new GameInterface();

            window.KeyPressed += Window_KeyPressed;

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
                Message healthUnits = new Message(Codes.HEALTH_UNITS, levels[0].GetPlayer().Health, null);
                Message speedUnits = new Message(Codes.SPEED_UNITS, levels[0].GetPlayer().Speed, null);
                if (levels[0].GetPlayer().Speed > 0.1)
                {
                    int a = 0;
                }
                gameInterface.GetMessage(healthUnits);
                gameInterface.GetMessage(speedUnits);
                gameInterface.Draw(window);
                // Отображение нарисованного
                window.Display();
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
