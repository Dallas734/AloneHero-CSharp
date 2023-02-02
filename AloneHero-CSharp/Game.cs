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
        public delegate void OrderEventHandler(object sender, OrderEventArgs args);
        public event OrderEventHandler LoadGame;
        public event OrderEventHandler PlayerStatsMove;

        private Level level;
        private List<Level> levels;
        private int curLevel;
        private int prevLevel;

        public Game()
        {
            endGame = false;
            curLevel = 0;
        }

        public bool StartGame()
        {
            RenderWindow window = new RenderWindow(new VideoMode(1200, 800), "Alone Hero");
            Clock clock = new Clock();

            // Инициализация уровней (на будущее)
            levels = new List<Level>();
            levels.Add(new Level("map_XML_2.tmx", this));
            levels.Add(new Level("map_XML_1.tmx", this));

            level = levels[curLevel];

            // Подписка на событие
            foreach (Level level1 in levels)
            {
                level1.EndGame += GetMessageEventHandler;
                level1.NextLevel += GetMessageEventHandler;
            }

                // Интерфейс
                GameInterface gameInterface = new GameInterface(this);

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

                    level.Draw(window, time);
                    LoadGame?.Invoke(this, new OrderEventArgs(Codes.HEALTH_UNITS, level.GetPlayer().Health, gameInterface));
                    LoadGame?.Invoke(this, new OrderEventArgs(Codes.SPEED_UNITS, level.GetPlayer().Speed, gameInterface));
                    LoadGame?.Invoke(this, new OrderEventArgs(Codes.COIN_UNITS, (int)level.GetPlayer().Coins, gameInterface));
                    //Message healthUnits = new Message(Codes.HEALTH_UNITS, levels[0].GetPlayer().Health, null);
                    //Message speedUnits = new Message(Codes.SPEED_UNITS, levels[0].GetPlayer().Speed, null);
                    if (level.GetPlayer().Speed > 0.1)
                    {
                        int a = 0;
                    }
                    //gameInterface.GetMessage(healthUnits);
                    //gameInterface.GetMessage(speedUnits);
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

        //public void SetEndGame(bool endGame)
        //{
        //    this.endGame = endGame;
        //}

        public void GetMessageEventHandler(object sender, OrderEventArgs args)
        {
            if (args.Code == Codes.END_GAME)
            {
                endGame = true;
            }
            if (args.Code == Codes.NEXT_LEVEL && level.LevelEnd)
            {
                Player player = level.GetPlayer();
                level = levels[++curLevel];
                PlayerStatsMove += level.GetMessageEventHandler;
                PlayerStatsMove?.Invoke(this, new OrderEventArgs(Codes.PLAYER_STATS_MOVE, player, player));
                PlayerStatsMove -= level.GetMessageEventHandler;
            }
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
