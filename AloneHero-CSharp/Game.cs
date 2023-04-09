using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Xml.Linq;
using System.Xml;

namespace AloneHero_CSharp
{
    class Game
    {
        private bool endGame;
        private bool windowOpen;
        public delegate void OrderEventHandler(object sender, OrderEventArgs args);
        public event OrderEventHandler LoadGame;
        public event OrderEventHandler StatsMove;

        private Level level;
        private List<Level> levels;
        private int curLevel;

        private RenderWindow MENU;
        private Menu mainMenu;

        public Game()
        {
            endGame = false;
            curLevel = 0;
            InitializeLevels();
            
        }

        public bool StartGame()
        {
            MENU = new RenderWindow(new VideoMode(1200, 800), "Alone Hero");
            mainMenu = new Menu(MENU.Size.X, MENU.Size.Y);
            
            // Подписка на событие!
            MENU.KeyPressed += Window_KeyPressed;

            while (MENU.IsOpen)
            {
                MENU.DispatchEvents();
                mainMenu.Draw(MENU);
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
                windowOpen = false;
                endGame = true;
            }
            if (args.Code == Codes.NEXT_LEVEL && level.LevelEnd)
            {
                Player player = level.GetPlayer();
                level = levels[++curLevel];
                StatsMove += level.GetMessageEventHandler;
                StatsMove?.Invoke(this, new OrderEventArgs(Codes.STATS_MOVE_NEXT_LVL, player, player));
                StatsMove -= level.GetMessageEventHandler;
            }
        }

        private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                windowOpen = false;
                if (sender == MENU)
                {
                    window.Close();
                }
                else 
                {
                    window.Close();
                    MENU.Display();
                    return;
                }
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
            
            if (Keyboard.IsKeyPressed(Keyboard.Key.Enter) && windowOpen == false)
            {
               
                int x = mainMenu.MenuNum;

                if (x == 0)
                {
                    windowOpen = true;
                    RenderWindow PLAY = new RenderWindow(new VideoMode(1200, 800), "Alone Hero");
                    // window.Close();

                    Clock clock = new Clock();

                    InitializeLevels();
                    level = levels[0];

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
                        MENU.Close();
                    }
                } 

                if (x == 1)
                {
                    windowOpen = true;
                    RenderWindow PLAY = new RenderWindow(new VideoMode(1200, 800), "Alone Hero");
                    Clock clock = new Clock();
                    LoadGameSaves();
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
                        MENU.Close();
                    }
                }

                if (x == 2)
                {
                    windowOpen = true;
                    RenderWindow ABOUT = new RenderWindow(new VideoMode(1200, 800), "About");

                    ABOUT.KeyPressed += Window_KeyPressed;
                    while(ABOUT.IsOpen)
                    {
                        ABOUT.DispatchEvents();
                        // Тут надписи
                        Font font = new Font("timesnewromanpsmt.ttf");
                        Text text = new Text("Игра платформер о рыцаре.\nЦель игры пройти все уровни\nДвижение: WASD\nПрыжок: Space\nУдар: стрелки\nВернидуб Марк, 2-41\nИГЭУ, Иваново", font, 40);
                        ABOUT.Draw(text);
                        ABOUT.Display();
                    }
                }

                if (x == 3)
                {
                    endGame = true;
                }

            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.F5))
            {
                SaveGame();
            }
        }

        private void SaveGame()
        {
            XDocument xdoc = new XDocument();

            XElement levelEl = new XElement("level");
            XAttribute numLevel = new XAttribute("curLevel", curLevel.ToString());
            levelEl.Add(numLevel);

            XElement player = new XElement("player");
            XAttribute xPlayer = new XAttribute("x", level.GetPlayer().X.ToString());
            XAttribute yPlayer = new XAttribute("y", level.GetPlayer().Y.ToString());
            XAttribute healthPlayer = new XAttribute("Health", level.GetPlayer().Health.ToString());
            XAttribute speedPlayer = new XAttribute("Speed", level.GetPlayer().Speed.ToString());
            XAttribute coinsPlayer = new XAttribute("Coins", level.GetPlayer().Coins.ToString());
            XAttribute strengthPlayer = new XAttribute("Strenght", level.GetPlayer().Strength.ToString());
            player.Add(xPlayer);
            player.Add(yPlayer);
            player.Add(healthPlayer);
            player.Add(speedPlayer);
            player.Add(coinsPlayer);
            player.Add(strengthPlayer);

            // Добавляем игрока в корневой элемент 
            levelEl.Add(player);

            XElement enemyRoot = new XElement("enemies");
            int i = 0;
            foreach (Enemy enemy in level.GetEnemies())
            {                
                XElement enemyEl;
                if (enemy == null)
                {
                    enemyEl = new XElement("enemy");
                    XAttribute state = new XAttribute("State", "Death");
                    XAttribute iterNum = new XAttribute("iterNum", i);
                    enemyEl.Add(state);
                    enemyEl.Add(iterNum);
                }
                else
                {
                    enemyEl = new XElement("enemy");
                    XAttribute xEnemy = new XAttribute("x", enemy.X.ToString());
                    XAttribute yEnemy = new XAttribute("y", enemy.Y.ToString());
                    XAttribute healthEnemy = new XAttribute("Health", enemy.Health.ToString());
                    XAttribute speedEnemy = new XAttribute("Speed", enemy.Speed.ToString());
                    XAttribute strengthEnemy = new XAttribute("Strength", enemy.Strength.ToString());
                    XAttribute xDefaultEnemy = new XAttribute("xDefault", enemy.DefaultX.ToString());
                    XAttribute yDefaultEnemy = new XAttribute("yDefault", enemy.DefaultY.ToString());
                    XAttribute state = new XAttribute("State", "Alive");
                    enemyEl.Add(xEnemy);
                    enemyEl.Add(yEnemy);
                    enemyEl.Add(healthEnemy);
                    enemyEl.Add(speedEnemy);
                    enemyEl.Add(strengthEnemy);
                    enemyEl.Add(state);
                    enemyEl.Add(xDefaultEnemy);
                    enemyEl.Add(yDefaultEnemy);
                }

                enemyRoot.Add(enemyEl);
                i++;
            }
            levelEl.Add(enemyRoot);

            // Предметы поддержки 
            i = 0;
            XElement supportItemsRoot = new XElement("supportItems");
            foreach (SupportItem supportItem in level.GetSupportItems())
            {
                XElement supportItemEl;
                if (supportItem == null)
                {
                    supportItemEl = new XElement("supportItem");
                    XAttribute used = new XAttribute("Used", "true");
                    supportItemEl.Add(used);
                }
                else
                {
                    supportItemEl = new XElement("supportItem");
                    XAttribute used = new XAttribute("Used", "false");
                    supportItemEl.Add(used);
                    XAttribute xDefaultSupIt = new XAttribute("xDefault", supportItem.DefaultX.ToString());
                    XAttribute yDefaultSupIt = new XAttribute("yDefault", supportItem.DeafaultY.ToString());
                    supportItemEl.Add(xDefaultSupIt);
                    supportItemEl.Add(yDefaultSupIt);
                }
                supportItemsRoot.Add(supportItemEl);

            }
            levelEl.Add(supportItemsRoot);

            // Записываем корневой элемент 
            xdoc.Add(levelEl);

            xdoc.Save("save-file.xml");
        }

        private void LoadGameSaves()
        {
            XmlDocument saveFile = new XmlDocument();
            saveFile.Load("save-file.xml");

            // Получаем корневой элемент
            XmlElement levelEl = saveFile.DocumentElement;
            XmlNode cuLevelAttr = levelEl.Attributes.GetNamedItem("curLevel");
            curLevel = int.Parse(cuLevelAttr.Value);
            level = levels[curLevel];

            // Получаем игрока 
            XmlNode playerEl = levelEl.FirstChild;
            double x = double.Parse(playerEl.Attributes.GetNamedItem("x").Value);
            double y = double.Parse(playerEl.Attributes.GetNamedItem("y").Value);
            double speed = double.Parse(playerEl.Attributes.GetNamedItem("Speed").Value);
            int health = int.Parse(playerEl.Attributes.GetNamedItem("Health").Value);
            int strenght = int.Parse(playerEl.Attributes.GetNamedItem("Strenght").Value);
            int coins = int.Parse(playerEl.Attributes.GetNamedItem("Coins").Value);
            Player player = new Player(x, y, speed, health, strenght, coins, level);
            StatsMove += level.GetMessageEventHandler;
            StatsMove?.Invoke(this, new OrderEventArgs(Codes.STATS_MOVE_LOAD, player, level.GetPlayer()));
            StatsMove -= level.GetMessageEventHandler;

            // Получаем врагов 
            XmlNodeList enemies = levelEl.GetElementsByTagName("enemy");
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].Attributes.GetNamedItem("State").Value == "Death" && level.GetEnemies()[i] != null)
                {
                    StatsMove += level.GetMessageEventHandler;
                    Skeleton enemy = new Skeleton(x, y, speed, 0, strenght, level);
                    int iter = int.Parse(enemies[i].Attributes.GetNamedItem("iterNum").Value);
                    StatsMove?.Invoke(this, new OrderEventArgs(Codes.STATS_MOVE_LOAD_ENEMY, enemy, enemy, iter));
                    StatsMove -= level.GetMessageEventHandler;
                }
                else if (level.GetEnemies()[i] != null)
                {
                    StatsMove += level.GetMessageEventHandler;
                    x = double.Parse(enemies[i].Attributes.GetNamedItem("x").Value);
                    y = double.Parse(enemies[i].Attributes.GetNamedItem("y").Value);
                    health = int.Parse(enemies[i].Attributes.GetNamedItem("Health").Value);
                    speed = double.Parse(enemies[i].Attributes.GetNamedItem("Speed").Value);
                    strenght = int.Parse(enemies[i].Attributes.GetNamedItem("Strength").Value);
                    Skeleton enemy = new Skeleton(x, y, speed, health, strenght, level);
                    enemy.DefaultX = double.Parse(enemies[i].Attributes.GetNamedItem("xDefault").Value);
                    enemy.DefaultY = double.Parse(enemies[i].Attributes.GetNamedItem("yDefault").Value);
                    StatsMove?.Invoke(this, new OrderEventArgs(Codes.STATS_MOVE_LOAD_ENEMY, enemy, enemy));
                    StatsMove -= level.GetMessageEventHandler;
                }
            }

            // Получаем предмет поддержки
            XmlNodeList supportItems = levelEl.GetElementsByTagName("supportItem");
            for (int i = 0; i < supportItems.Count; i++)
            {
                if (supportItems[i].Attributes.GetNamedItem("Used").Value == "true" && level.GetSupportItems()[i] != null)
                {
                    StatsMove += level.GetMessageEventHandler;
                    StatsMove?.Invoke(this, new OrderEventArgs(Codes.STATS_MOVE_LOAD, true, level.GetSupportItems()[i]));
                    StatsMove -= level.GetMessageEventHandler;
                }           
            }
            
            //foreach (Enemy enemyLvl in level.GetEnemies())
            //{
            //    level.ChangeParamEvent += enemyLvl.GetMessageEventHandler;
            //    foreach (XmlElement enemy in enemies)
            //    {
            //        x = double.Parse(enemy.Attributes.GetNamedItem("x").Value);
            //        y = double.Parse(enemy.Attributes.GetNamedItem("y").Value);
            //        health = int.Parse(enemy.Attributes.GetNamedItem("Health").Value);
            //    }
            //}

        }

        private void InitializeLevels()
        {
            // Инициализация уровней (на будущее)
            levels = new List<Level>();
            //levels.Add(new Level("map_XML_2.tmx", this));
            levels.Add(new Level("Level_1.tmx", this));
            levels.Add(new Level("map_XML_1.tmx", this));
            // Подписка на событие
            foreach (Level level1 in levels)
            {
                level1.EndGame += GetMessageEventHandler;
                level1.NextLevel += GetMessageEventHandler;
            }
        }
    }
}
