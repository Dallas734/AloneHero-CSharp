using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Xml;
using System.Globalization;
using System.Drawing.Text;

namespace AloneHero_CSharp
{
    class Level
    {
        private Player player;
        private List<Enemy> enemies;
        private List<SupportItem> supportItems;
        private int width, height, tileWidth, tileHeight;
        private int firstTileID;
        private string fileNameTMX;
        private FloatRect drawingBounds;
        private Texture tilesetImage;
        private List<ObjectLvl> objects;
        private List<Layer> layers;
        private Font font;

        private bool LoadFromFile(string fileName)
        {
            XmlDocument levelFile = new XmlDocument();

            levelFile.Load(fileName);
            if (levelFile == null)
            {
                Console.WriteLine("Loading level failed."); //выдаем ошибку
                return false;
            }

            // Получаем корневой элемент
            XmlElement map = levelFile.DocumentElement;

            // Извлекам из карты свойства
            XmlNode widthX = map.Attributes.GetNamedItem("width");
            width = int.Parse(widthX.Value);
            XmlNode heightX = map.Attributes.GetNamedItem("height");
            height = int.Parse(heightX.Value);
            XmlNode tileWidthX = map.Attributes.GetNamedItem("tilewidth");
            tileWidth = int.Parse(tileWidthX.Value);
            XmlNode tileHeightX = map.Attributes.GetNamedItem("tileheight");
            tileHeight = int.Parse(tileHeightX.Value);

            // Берем описание тайлсета и идентификатор первого тайла
            XmlNode tilesetElement = map.FirstChild;
            firstTileID = int.Parse(tilesetElement.Attributes.GetNamedItem("firstgid").Value);

            // source - путь до картинки в контейнере image
            string imagepath = tilesetElement.Attributes.GetNamedItem("source").Value;

            Image img = new Image(imagepath);

            if (img == null)
            {
                Console.WriteLine("Failed to load tile sheet.");//если не удалось загрузить тайлсет-выводим ошибку в консоль
                return false;
            }

            img.CreateMaskFromColor(new Color(255, 255, 255));
            tilesetImage = new Texture(img);
            tilesetImage.Smooth = false;

            // получаем количество столбцов и строк тайлсета
            int columns = (int)tilesetImage.Size.X / tileWidth;
            int rows = (int)tilesetImage.Size.Y / tileHeight;

            // Список из прямоугольников изображений
            List<IntRect> subRects = new List<IntRect>();

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    IntRect rect;

                    rect.Top = y * tileHeight;
                    rect.Height = tileHeight;
                    rect.Left = x * tileWidth;
                    rect.Width = tileWidth;

                    subRects.Add(rect);
                }
            }

            // Работа со слоями
            XmlNodeList layerElements;
            layerElements = map.GetElementsByTagName("layer");
            foreach (XmlElement layerElement in layerElements)
            {
                Layer layer = new Layer();

                //string layer1 = layerElement.GetAttribute("opacity");

                if (layerElement.GetAttribute("opacity") != null && layerElement.GetAttribute("opacity") != "")
                {
                    float opacity = float.Parse(layerElement.GetAttribute("opacity"));
                    layer.opacity = 255 * (int)opacity;
                }
                else
                {
                    layer.opacity = 255;
                }

                // Контейнер data
                XmlNodeList layerDataElements = layerElement.GetElementsByTagName("data");

                if (layerDataElements == null)
                {
                    Console.WriteLine("Bad map. No layer information found.");
                    return false;
                }

                // контейнер layer
                foreach (XmlElement layerDataElement in layerDataElements)
                {
                    XmlNodeList tileElements = layerDataElement.GetElementsByTagName("tile");
                    if (tileElements == null)
                    {
                        Console.WriteLine("Bad map. No tile information found.");
                        return false;
                    }

                    int x = 0;
                    int y = 0;

                    //int number = 0;

                    // контейнер tile
                    foreach (XmlElement tileElement in tileElements)
                    {
                        int tileGID = int.Parse(tileElement.GetAttribute("gid"));
                        int subRectToUse = tileGID - firstTileID;

                        // Устанавливаем TextureRect каждого тайла
                        if (subRectToUse >= 0)
                        {
                            Sprite sprite = new Sprite(tilesetImage);
                            sprite.TextureRect = new IntRect(subRects[subRectToUse].Left, subRects[subRectToUse].Top, subRects[subRectToUse].Width, subRects[subRectToUse].Height);
                            sprite.Position = new Vector2f(x * tileWidth, y * tileHeight);
                            //sprite.setColor(sf::Color(255, 255, 255, layer.opacity));

                            layer.tiles.Add(sprite);//закидываем в слой спрайты тайлов
                        }

                        x++;
                        if(x >= width)
                        {
                            x = 0;
                            y++;
                            if (y >= height) y = 0; 
                        }
                    }

                    layers.Add(layer);
                }
            
            }

            // Работа с объектами
            XmlNodeList objectGroupElements;

            if (map.GetElementsByTagName("objectgroup") != null)
            {
                objectGroupElements = map.GetElementsByTagName("objectgroup");
                foreach(XmlElement objectGroupElement in objectGroupElements)
                {
                    XmlNodeList objectElements = objectGroupElement.GetElementsByTagName("object");
                    foreach (XmlElement objectElement in objectElements)
                    {
                        string objectType = null;
                        if (objectElement.GetAttribute("type") != null)
                        {
                            objectType = objectElement.GetAttribute("type");
                        }
                        string objectName = null;
                        if (objectElement.GetAttribute("name") != null)
                        {
                            objectName = objectElement.GetAttribute("name");
                        }
                        
                        int x = (int)Single.Parse(objectElement.GetAttribute("x"), CultureInfo.InvariantCulture);
                        int y = (int)Single.Parse(objectElement.GetAttribute("y"), CultureInfo.InvariantCulture); ;
                        //int.TryParse(objectElement.GetAttribute("x"), out x);
                        //int.TryParse(objectElement.GetAttribute("y"), out y);

                        int width;
                        int height;

                        Sprite sprite = new Sprite(tilesetImage);
                        sprite.TextureRect = new IntRect(0, 0, 0, 0);
                        sprite.Position = new Vector2f(x, y);

                        if (objectElement.GetAttribute("width") != null)
                        {
                            string atr = objectElement.GetAttribute("width");
                            width = (int)Single.Parse(objectElement.GetAttribute("width"), CultureInfo.InvariantCulture);
                            height = (int)Single.Parse(objectElement.GetAttribute("height"), CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            width = subRects[int.Parse(objectElement.GetAttribute("gid")) - firstTileID].Width;
                            height = subRects[int.Parse(objectElement.GetAttribute("gid")) - firstTileID].Height;
                            sprite.TextureRect = subRects[int.Parse(objectElement.GetAttribute("gid")) - firstTileID];
                        }

                        // Экземпляр объекта
                        ObjectLvl objectlvl = new ObjectLvl();
                        objectlvl.Name = objectName;
                        objectlvl.Type = objectType;
                        objectlvl.Sprite = sprite;

                        FloatRect objectRect = new FloatRect(x, y, width, height);
                        objectlvl.Rect = objectRect;

                        objects.Add(objectlvl);

                    }
                }
            }
            else
            {
                Console.WriteLine("No object layers found...");
            }

            //Ищем игрока
            ObjectLvl playerObject = GetObject("Player");
            if (player == null)
            {
                player = new Player(playerObject.Rect.Left, playerObject.Rect.Top);
                view.Reset(new FloatRect(0, 0, 1200, 800));
            }

            FillEnemy("Skeleton");
            FillEnemy("Goblin");
            FillEnemy("Mushroom");

            FillSupportItem("GreenPotion");
            FillSupportItem("RedPotion");

            return true;

        }

        private bool collisionWithPlayer;
        private View view;
        private Vector2i sizeOfView;

        public Level(string fileNameTMX)
        {
            layers = new List<Layer>();
            objects = new List<ObjectLvl>();
            enemies = new List<Enemy>();
            supportItems = new List<SupportItem>();
            view = new View();
            this.fileNameTMX = "Levels\\" + fileNameTMX;
            LoadFromFile(this.fileNameTMX);
            collisionWithPlayer = false;
            sizeOfView.X = 600;
            sizeOfView.Y = 400;
            font = new Font("timesnewromanpsmt.ttf");
        }

        public ObjectLvl GetObject(string name)
        {
            for (int i = 0; i < objects.Count; i++)
                if (objects[i].Name == name)
                    return objects[i];

            return new ObjectLvl();
        }

        public List<ObjectLvl> GetObjects(string name)
        {
            List<ObjectLvl> thisObjects = new List<ObjectLvl>();
            for (int i = 0; i < objects.Count; i++)
                if (objects[i].Name == name)
                    thisObjects.Add(objects[i]);

            return thisObjects;
        }

        public List<ObjectLvl> GetAllObjects()
        {
            return objects;
        }

        public Vector2i GetTileSize()
        {
            return new Vector2i(tileWidth, tileHeight);
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public bool GetCollisionWithPLayer()
        {
            return collisionWithPlayer;
        }

        public void ViewOnPlayer(Player player)
        {
            double tempX = player.X;
            double tempY = player.Y;

            view.Size = new Vector2f(sizeOfView.X, sizeOfView.Y);

            // Проверка правой границы 
            if (tempX + sizeOfView.X / 2 >= width * tileWidth)
            {
                tempX = width * tileWidth - sizeOfView.X / 2;
            }

            // Проверка левой границы
            if (tempX - sizeOfView.X / 2 <= 0)
            {
                tempX = sizeOfView.X / 2;
            }

            // Проверка нижней границы
            if (tempY + sizeOfView.Y / 2 >= height * tileHeight)
            {
                tempY = height * tileHeight - sizeOfView.Y / 2;
            }

            // Проверка верхней границы
            if (tempY - sizeOfView.Y / 2 <= 0)
            {
                tempY = sizeOfView.Y / 2;
            }

            view.Center = new Vector2f((float)tempX, (float)tempY);
        }

        public void GetMessage(Message message)
        {
            Message messageToEnemy;
            Message messageToPlayer;
            Message messageToSomeone;
            Message messageToItem;
            List<ObjectLvl> obj = GetAllObjects();

            // Общая проверка столкновения при ходьбе
            if (message.code == Codes.RUN_C)
            {
                for (int i = 0; i < obj.Count; i++)
                {
                    if (message.sender.GetRect().Intersects(obj[i].Rect))
                    {
                        if (obj[i].Name == "Solid")
                        {
                            if (message.dy > 0)
                            {
                                messageToSomeone = new Message(Codes.FALL_C, 0, null, 0, obj[i].Rect.Top - message.sender.Height, 0, 0);
                                message.sender.GetMessage(messageToSomeone);
                            }
                            if (message.dy < 0)
                            {
                                messageToSomeone = new Message(Codes.JUMP_C, 0, null, 0, obj[i].Rect.Top + obj[i].Rect.Height, 0, 0);
                                message.sender.GetMessage(messageToSomeone);
                            }
                            if (message.dx > 0)
                            {
                                messageToSomeone = new Message(Codes.CHANGE_X, 0, null, obj[i].Rect.Left - message.sender.Width, 0, 0, 0);
                                message.sender.GetMessage(messageToSomeone);
                            }
                            if (message.dx < 0)
                            {
                                messageToSomeone = new Message(Codes.CHANGE_X, 0, null, obj[i].Rect.Left + obj[i].Rect.Width, 0, 0, 0);
                                message.sender.GetMessage(messageToSomeone);
                            }
                        }

                        if (obj[i].Name == "enemyBarier" && message.sender is Enemy)
                        {
                            messageToEnemy = new Message(Codes.ENEMY_BARIER, 0, null);
                            message.sender.GetMessage(messageToEnemy);
                        }
                    }
                }
            }

            // Игрок
            if (message.sender is Player)
            {
                if (message.code == Codes.HIT_C)
                {
                    foreach (Enemy enemy in enemies)
                    {
                        if (player.GetHitRect().Intersects(enemy.GetRect()) && enemy.CollisionWithPlayer == false && player.Dy == 0 && player.State == States.HIT && enemy.State != States.HIT && ((player.Direction == Directions.LEFT && player.X > enemy.X) || (player.Direction == Directions.RIGHT && player.X < enemy.X)) && enemy.State != States.DEATH)
                        {
                            messageToEnemy = new Message(Codes.DAMAGE_C, player.Strength, null);
                            enemy.GetMessage(messageToEnemy);
                            return;
                        }
                        
                    }
                }

                if (message.code == Codes.RUN_C)
                {
                    foreach (SupportItem supportItem in supportItems)
                    {
                        if (player.GetRect().Intersects(supportItem.GetRect()))
                        {
                            messageToItem = new Message(Codes.IMPROVE_STATS, 0, player);
                            supportItem.GetMessage(messageToItem);
                        }
                    }
                }
            }

            // Враг
            if (message.sender is Enemy)
            {
                Enemy enemy = (Enemy)message.sender;
                if (message.code == Codes.RUN_C)
                {
                    if (enemy.CollisionWithPlayer && player.State != States.HIT && enemy.State == States.DAMAGE)
                    {
                        messageToEnemy = new Message(Codes.RUN_C, 0, null);
                        enemy.GetMessage(messageToEnemy);
                        return;
                    }

                    if (enemy.GetRect().Intersects(player.GetRect()) && enemy.CollisionWithPlayer == false && player.Dy == 0 && player.State != States.HIT)
                    {
                        messageToEnemy = new Message(Codes.HIT_C, 0, null);
                        messageToPlayer = new Message(Codes.DAMAGE_C, enemy.Strength, null);
                        enemy.GetMessage(messageToEnemy);
                        player.GetMessage(messageToPlayer);
                        return;
                    }
                    else if (enemy.CollisionWithPlayer && enemy.State == States.RUN && player.Dy == 0)
                    {
                        if (!enemy.GetRect().Intersects(player.GetRect()))
                        {
                            messageToEnemy = new Message(Codes.RUN_C, 0, null);
                            enemy.GetMessage(messageToEnemy);
                            /*enemy->collisionWithPlayer = false;*/
                        }
                        if (enemy.CollisionWithPlayer && player.State != States.RUN && player.State != States.HIT)
                        {
                            messageToPlayer = new Message(Codes.IDLE_C, 0, null);
                            player.GetMessage(messageToPlayer);
                            /*player->SetState(IDLE);*/
                        }
                        return;
                    }

                }
            }

        }

        private void FillEnemy(string nameOfEnemy)
        {
            List<ObjectLvl> enemyObjects = GetObjects(nameOfEnemy);
            for (int i = 0; i < enemyObjects.Count; i++)
            {
                Enemy enemy = null;
                if (nameOfEnemy == "Mushroom") enemy = new Mushroom(enemyObjects[i].Rect.Left, enemyObjects[i].Rect.Top, 0.08, 300, 50);
                else if (nameOfEnemy == "Skeleton") enemy = new Skeleton(enemyObjects[i].Rect.Left, enemyObjects[i].Rect.Top, 0.08, 300, 50);
                else if (nameOfEnemy == "Goblin") enemy = new Goblin(enemyObjects[i].Rect.Left, enemyObjects[i].Rect.Top, 0.08, 300, 50);

                enemies.Add(enemy);
            }
        }

        public void FillSupportItem(string nameOfSupportItem)
        {
            List<ObjectLvl> supportObjects = GetObjects(nameOfSupportItem);
            foreach (ObjectLvl supportObject in supportObjects)
            {
                SupportItem supportItem = null;
                if (nameOfSupportItem == "GreenPotion") supportItem = new GreenPotion(supportObject.Rect.Left, supportObject.Rect.Top, 0.1);
                else if (nameOfSupportItem == "RedPotion") supportItem = new RedPotion(supportObject.Rect.Left, supportObject.Rect.Top, 20);
                supportItems.Add(supportItem);
            }
        }

        public void Draw(RenderWindow window, float time, Game game)
        {
            player.Update(time, window, this);
            if (player.State == States.DEATH)
            {
                game.SetEndGame(true);
                return;
            }

            Text text = new Text(player.Health.ToString(), font, 20);
            Vector2f center = window.GetView().Center;
            Vector2f size = window.GetView().Size;
            text.Position = new Vector2f(center.X - size.X / 2 + 25, center.Y - size.Y / 2 - 5);
            Image heartImage = new Image("Images\\Interface\\Heart.png");
            Texture heartTexture = new Texture(heartImage);
            Sprite heartSprite = new Sprite(heartTexture);
            heartSprite.TextureRect = new IntRect(22, 18, 22, 19);
            heartSprite.Position = new Vector2f(center.X - size.X / 2, center.Y - size.Y / 2);
            //window.Draw(text);
            //window.Display();


            // Проходимся по всем врагам
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(time, window, this);
                
                //window.Draw(enemy.GetSprite(enemy.State));
            }
            foreach (Enemy enemy in enemies)
            {
                if (enemy.State == States.DEATH && enemy.Counter > 1)
                {
                    enemies.Remove(enemy);
                    return;
                }
            }

            // Проходимся по предметам поддержки
            foreach (SupportItem supportItem in supportItems)
            {
                supportItem.Update(time, window);
                if (supportItem.Used)
                {
                    supportItems.Remove(supportItem);
                    return;
                }
            }

            // Отрисовка
            window.SetView(view);
            window.Clear();

            // рисуем все тайлы (объекты не рисуем!)
            for (int layer = 0; layer < layers.Count; layer++)
                for (int tile = 0; tile < layers[layer].tiles.Count; tile++)
                    window.Draw(layers[layer].tiles[tile]);

            // Рисуем здоровье
            window.Draw(heartSprite);
            window.Draw(text);

            // Рисуем персонажа
            window.Draw(player.GetSprite(player.State));

            // Рисуем врагов
            foreach (Enemy enemy in enemies)
            {
                window.Draw(enemy.GetSprite(enemy.State));
            }
            //foreach (Enemy enemy in enemies)
            //{
            //    if (enemy.State == States.DEATH)
            //    {
            //        enemies.Remove(enemy);
            //        return;
            //    }
            //}

            // Рисуем предметы поддержки
            foreach (SupportItem supportItem in supportItems)
            {
                window.Draw(supportItem.Sprite);
            }

            window.Display();
        }
    }
}
