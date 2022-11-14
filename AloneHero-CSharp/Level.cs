using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Xml;
using System.Globalization;

namespace AloneHero_CSharp
{
    class Level
    {
        private Player player;
        //private List<Enemy> enemies;
        //private List<SupportItem> supportItems;
        private int width, height, tileWidth, tileHeight;
        private int firstTileID;
        private string fileNameTMX;
        private FloatRect drawingBounds;
        private Texture tilesetImage;
        private List<ObjectLvl> objects;
        private List<Layer> layers;

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
                            width = int.Parse(objectElement.GetAttribute("width"));
                            height = int.Parse(objectElement.GetAttribute("height"));
                        }
                        else
                        {
                            width = subRects[int.Parse(objectElement.GetAttribute("gid")) - firstTileID].Width;
                            height = subRects[int.Parse(objectElement.GetAttribute("gid")) - firstTileID].Height;
                            sprite.TextureRect = subRects[int.Parse(objectElement.GetAttribute("gid")) - firstTileID];
                        }

                        // Экземпляр объекта
                        ObjectLvl objectlvl = new ObjectLvl();
                        objectlvl.name = objectName;
                        objectlvl.type = objectType;
                        objectlvl.sprite = sprite;

                        FloatRect objectRect = new FloatRect(x, y, width, height);
                        objectlvl.rect = objectRect;

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
                player = new Player(playerObject.rect.Left, playerObject.rect.Top);
                view.Reset(new FloatRect(0, 0, 1200, 800));
            }

            //FillEnemy("Skeleton");
            //FillEnemy("Goblin");
            //FillEnemy("Mushroom");

            //FillSupportItem("GreenPotion");
            //FillSupportItem("RedPotion");

            return true;

        }

        private bool collisionWithPlayer;
        private View view;
        private Vector2i sizeOfView;

        public Level(string fileNameTMX)
        {
            layers = new List<Layer>();
            this.fileNameTMX = "Levels\\" + fileNameTMX;
            LoadFromFile(this.fileNameTMX);
            collisionWithPlayer = false;
            sizeOfView.X = 600;
            sizeOfView.Y = 400;
        }

        public ObjectLvl GetObject(string name)
        {
            for (int i = 0; i < objects.Count; i++)
                if (objects[i].name == name)
                    return objects[i];

            return new ObjectLvl();
        }

        public List<ObjectLvl> GetObjects(string name)
        {
            for (int i = 0; i < objects.Count; i++)
                if (objects[i].name == name)
                    objects.Add(objects[i]);

            return objects;
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

        //void Level::FillSupportItem(std::string nameOfSupportItem)
        //{
        //    std::vector<Object> supportObjects = GetObjects(nameOfSupportItem);
        //    for (int i = 0; i < supportObjects.size(); i++)
        //    {
        //        SupportItem* supportItem;
        //        if (nameOfSupportItem == "GreenPotion") supportItem = new GreenPotion(supportObjects[i].rect.left, supportObjects[i].rect.top, 0.1);
        //        else if (nameOfSupportItem == "RedPotion") supportItem = new RedPotion(supportObjects[i].rect.left, supportObjects[i].rect.top, 20);
        //        this->supportItems.push_back(supportItem);
        //    }
        //}

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
                    if (message.sender.GetRect().Intersects(obj[i].rect))
                    {
                        if (obj[i].name == "Solid")
                        {
                            if (message.dy > 0)
                            {
                                messageToSomeone = new Message(Codes.FALL_C, 0, null, 0, obj[i].rect.Top - message.sender.Height, 0, 0);
                                message.sender.GetMessage(messageToSomeone);
                            }
                            if (message.dy < 0)
                            {
                                messageToSomeone = new Message(Codes.JUMP_C, 0, null, 0, obj[i].rect.Top + obj[i].rect.Height, 0, 0);
                                message.sender.GetMessage(messageToSomeone);
                            }
                            if (message.dx > 0)
                            {
                                messageToSomeone = new Message(Codes.CHANGE_X, 0, null, obj[i].rect.Left - message.sender.Width, 0, 0, 0);
                                message.sender.GetMessage(messageToSomeone);
                            }
                            if (message.dx < 0)
                            {
                                messageToSomeone = new Message(Codes.CHANGE_X, 0, null, obj[i].rect.Left + obj[i].rect.Width, 0, 0, 0);
                                message.sender.GetMessage(messageToSomeone);
                            }
                        }

                        //if (obj[i].name == "enemyBarier" && typeid(*(message.sender)) == typeid(Enemy))
                        //{
                        //    messageToEnemy = new Message(ENEMY_BARIER, 0, nullptr);
                        //    message.sender->GetMessage(*messageToEnemy);
                        //}
                    }
                }
            }

            // Игрок
            //if (message.sender is Player)
            //{
            //    if (message.code == Codes.HIT_C)
            //    {
            //        for (int i = 0; i < enemies.size(); i++)
            //        {
            //            Enemy* enemy = &enemies[i];
            //            if (player->getHitRect().intersects(enemy->getRect()) && enemy->collisionWithPlayer == false && player->GetDY() == 0 && player->GetState() == HIT && enemy->GetState() != HIT && ((player->GetDirection() == LEFT && player->GetX() > enemy->GetX()) || (player->GetDirection() == RIGHT && player->GetX() < enemy->GetX())))
            //            {
            //                messageToEnemy = new Message(DAMAGE_C, player->GetStrength(), nullptr);
            //                enemy->GetMessage(*messageToEnemy);
            //                return;
            //            }
            //            else if (enemy->collisionWithPlayer && player->GetState() != HIT && player->GetDY() == 0 && enemy->GetState() == DAMAGE)
            //            {
            //                messageToEnemy = new Message(RUN_C, 0, nullptr);
            //                enemy->GetMessage(*messageToEnemy);
            //                return;
            //            }
            //        }
            //    }

            //    if (message.code == Codes.RUN_C)
            //    {
            //        for (int i = 0; i < supportItems.size(); i++)
            //        {
            //            SupportItem* supportItem = supportItems[i];
            //            if (player->getRect().intersects(supportItems[i]->getRect()))
            //            {
            //                messageToItem = new Message(IMPROVE_STATS, 0, player);
            //                supportItem->GetMessage(*messageToItem);
            //            }
            //        }
            //    }
            //}

            // Враг
            //if (typeid(*(message.sender)) == typeid(Enemy))
            //{
            //    Enemy* enemy = (Enemy*)message.sender;
            //    if (message.code == RUN_C)
            //    {
            //        if (enemy->getRect().intersects(player->getRect()))
            //        {
            //            int a = 0;
            //        }

            //        if (enemy->getRect().intersects(player->getRect()) && enemy->collisionWithPlayer == false && player->GetDY() == 0 && player->GetState() != HIT)
            //        {
            //            messageToEnemy = new Message(HIT_C, 0, nullptr);
            //            messageToPlayer = new Message(DAMAGE_C, enemy->GetStrength(), nullptr);
            //            enemy->GetMessage(*messageToEnemy);
            //            player->GetMessage(*messageToPlayer);
            //            return;
            //        }
            //        else if (enemy->collisionWithPlayer && enemy->GetState() == RUN && player->GetDY() == 0)
            //        {
            //            if (!enemy->getRect().intersects(player->getRect()))
            //            {
            //                messageToEnemy = new Message(RUN_C, 0, nullptr);
            //                enemy->GetMessage(*messageToEnemy);
            //                /*enemy->collisionWithPlayer = false;*/
            //            }
            //            if (enemy->collisionWithPlayer && player->GetState() != RUN && player->GetState() != HIT)
            //            {
            //                messageToPlayer = new Message(IDLE_C, 0, nullptr);
            //                player->GetMessage(*messageToPlayer);
            //                /*player->SetState(IDLE);*/
            //            }
            //            return;
            //        }

            //    }
            //}

            //void Level::FillEnemy(std::string nameOfEnemy)
            //{
            //    std::vector<Object> enemyObjects = GetObjects(nameOfEnemy);
            //    for (int i = 0; i < enemyObjects.size(); i++)
            //    {
            //        Enemy* enemy = nullptr;
            //        if (nameOfEnemy == "Mushroom") enemy = new Mushroom(enemyObjects[i].rect.left, enemyObjects[i].rect.top, 0.08, 100, 50);
            //        else if (nameOfEnemy == "Skeleton") enemy = new Skeleton(enemyObjects[i].rect.left, enemyObjects[i].rect.top, 0.08, 300, 50);
            //        else if (nameOfEnemy == "Goblin") enemy = new Goblin(enemyObjects[i].rect.left, enemyObjects[i].rect.top, 0.08, 200, 50);
            //        this->enemies.push_back(*enemy);
            //    }
            //}


        }

        public void Draw(RenderWindow window, float time, Game game)
        {
            player.Update(time, window, this);
            if (player.State == States.DEATH)
            {
                game.SetEndGame(true);
                return;
            }

            // Проходимся по всем врагам
            //for (std::vector<Enemy>::iterator it = enemies.begin(); it != enemies.end();)
            //{
            //    it->Update(time, window, this);
            //    if (it->GetState() == DEATH)
            //    {
            //        it = enemies.erase(it);
            //    }
            //    else it++;
            //    //window.draw(enemies[i].GetSprite(enemies[i].GetState()));
            //}

            // Проходимся по предметам поддержки
            //for (std::vector<SupportItem*>::iterator it = supportItems.begin(); it != supportItems.end();)
            //{
            //    (*it)->Update(time, window);
            //    if ((*it)->GetUsed())
            //    {
            //        it = supportItems.erase(it);
            //    }
            //    else it++;
            //}

            // Отрисовка
            window.SetView(view);

            window.Clear();

            // рисуем все тайлы (объекты не рисуем!)
            for (int layer = 0; layer < layers.Count; layer++)
                for (int tile = 0; tile < layers[layer].tiles.Count; tile++)
                    window.Draw(layers[layer].tiles[tile]);

            // Рисуем персонажа
            window.Draw(player.GetSprite(player.State));

            // Рисуем врагов
            //for (int i = 0; i < enemies.size(); i++)
            //{
            //    window.draw(enemies[i].GetSprite(enemies[i].GetState()));
            //}

            // Рисуем предметы поддержки
            //for (int i = 0; i < supportItems.size(); i++)
            //{
            //    window.draw(supportItems[i]->GetSprite());
            //}

            window.Display();
        }
    }
}
