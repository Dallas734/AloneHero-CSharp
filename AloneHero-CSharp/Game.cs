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

        private RenderWindow MENU;
        private Menu mainMenu;

        public Game()
        {
            endGame = false;
            curLevel = 0;
            
        }

        public bool StartGame()
        {
            MENU = new RenderWindow(new VideoMode(1200, 800), "Alone Hero");
            mainMenu = new Menu(MENU.Size.X, MENU.Size.Y);
            //Clock clock = new Clock();

            //// Инициализация уровней (на будущее)
            //levels = new List<Level>();
            //levels.Add(new Level("map_XML_2.tmx", this));
            //levels.Add(new Level("map_XML_1.tmx", this));

            //level = levels[curLevel];

            //// Подписка на событие
            //foreach (Level level1 in levels)
            //{
            //    level1.EndGame += GetMessageEventHandler;
            //    level1.NextLevel += GetMessageEventHandler;
            //}

            //    // Интерфейс
            //GameInterface gameInterface = new GameInterface(this);

            MENU.KeyPressed += Window_KeyPressed;

            while (MENU.IsOpen)
            {
                // Время для анимации
                //float time = clock.ElapsedTime.AsMicroseconds();
                //clock.Restart();
                //time = time / 800;
                

                MENU.DispatchEvents();
                mainMenu.Draw(MENU);

                //if (Keyboard.IsKeyPressed(Keyboard.Key.Tab) || endGame)
                //{
                //    window.Close();
                //    return true;
                //}
                //if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) return false;


                //if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                //{
                //    mainMenu.MoveUp();
                //}
                //if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                //{
                //    mainMenu.MoveDown();
                //}
                //if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
                //{
                //    RenderWindow PLAY = new RenderWindow(new VideoMode(1200, 800), "Alone Hero");

                //}


                // Здесь вставить отрисовку меню 

                //level.Draw(window, time);
                //LoadGame?.Invoke(this, new OrderEventArgs(Codes.HEALTH_UNITS, level.GetPlayer().Health, gameInterface));
                //LoadGame?.Invoke(this, new OrderEventArgs(Codes.SPEED_UNITS, level.GetPlayer().Speed, gameInterface));
                //LoadGame?.Invoke(this, new OrderEventArgs(Codes.COIN_UNITS, level.GetPlayer().Coins, gameInterface));
                ////Message healthUnits = new Message(Codes.HEALTH_UNITS, levels[0].GetPlayer().Health, null);
                ////Message speedUnits = new Message(Codes.SPEED_UNITS, levels[0].GetPlayer().Speed, null);
                //if (level.GetPlayer().Speed > 0.1)
                //{
                //    int a = 0;
                //}
                ////gameInterface.GetMessage(healthUnits);
                ////gameInterface.GetMessage(speedUnits);
                //gameInterface.Draw(window);
                // Отображение нарисованного
                MENU.Display();
            }
            
            return false;

        }

        public void GameRunning()
        {
            if (/*StartGame()*/ endGame == false)
            {
                StartGame();
                //endGame = false;
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
            var window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
            if (e.Code == Keyboard.Key.Tab || endGame)
            {
                window.Close();
                endGame = false;
                
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
            {
                mainMenu.MoveUp();
                return;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
            {
                mainMenu.MoveDown();
                return;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) endGame = true;
            
            if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
            {
               
                int x = mainMenu.MenuNum;

                if (x == 0)
                {
                    RenderWindow PLAY = new RenderWindow(new VideoMode(1200, 800), "Alone Hero");
                    //window.Close();

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

                    PLAY.KeyPressed += Window_KeyPressed;

                    while (PLAY.IsOpen)
                    {
                        // Время для анимации
                        float time = clock.ElapsedTime.AsMicroseconds();
                        clock.Restart();
                        time = time / 800;

                        PLAY.DispatchEvents();

                        level.Draw(PLAY, time);
                        LoadGame?.Invoke(this, new OrderEventArgs(Codes.HEALTH_UNITS, level.GetPlayer().Health, gameInterface));
                        LoadGame?.Invoke(this, new OrderEventArgs(Codes.SPEED_UNITS, level.GetPlayer().Speed, gameInterface));
                        LoadGame?.Invoke(this, new OrderEventArgs(Codes.COIN_UNITS, level.GetPlayer().Coins, gameInterface));
                        //Message healthUnits = new Message(Codes.HEALTH_UNITS, levels[0].GetPlayer().Health, null);
                        //Message speedUnits = new Message(Codes.SPEED_UNITS, levels[0].GetPlayer().Speed, null);
                        if (level.GetPlayer().Speed > 0.1)
                        {
                            int a = 0;
                        }
                        //gameInterface.GetMessage(healthUnits);
                        //gameInterface.GetMessage(speedUnits);
                        gameInterface.Draw(PLAY);
                        // Отображение нарисованного
                        PLAY.Display();
                    }
                } 

                if (x == 1)
                {
                    RenderWindow ABOUT = new RenderWindow(new VideoMode(1200, 800), "About");

                    ABOUT.KeyPressed += Window_KeyPressed;
                    while(ABOUT.IsOpen)
                    {
                        ABOUT.DispatchEvents();
                        ABOUT.Display();
                    }
                }

                if (x == 2)
                {
                    window.Close();
                }

            }
        }

    }
}
