using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace AloneHero_CSharp
{
    class Menu
    {
        private Font font;
        private List<Text> mainMenu;
        private Sprite background;
        private Text title;
        public int MenuNum { get; private set; }

        public Menu(float width, float height)
        {
            //Image image = new Image("Images\\Menu\\Background.png");
            Image imageBG = new Image("Images\\Menu\\123.png");
            background = new Sprite(new Texture(imageBG));
            background.Scale = new Vector2f(width / imageBG.Size.X, height / imageBG.Size.Y);

            MenuNum = 0;
            //font = new Font("timesnewromanpsmt.ttf");
            font = new Font(Fonts_r.timesnewromanpsmt);
            mainMenu = new List<Text>();

            title = new Text("ALONE HERO", font, 100);
            title.Position = new Vector2f(400, 100);
            title.FillColor = Color.Red;

            mainMenu.Add(new Text("New Game", font, 70));
            mainMenu[0].Position = new Vector2f(400, 300);

            mainMenu.Add(new Text("Load", font, 70));
            mainMenu[1].Position = new Vector2f(400, 400);

            mainMenu.Add(new Text("About", font, 70));
            mainMenu[2].Position = new Vector2f(400, 500);

            mainMenu.Add(new Text("Exit", font, 70));
            mainMenu[3].Position = new Vector2f(400, 600);

            mainMenu[MenuNum].FillColor = Color.Blue;
        }

        public void Draw(RenderWindow window)
        {

            window.Draw(background);
            window.Draw(title);   
            foreach (Text element in mainMenu)
            {
                window.Draw(element);
            }
        }

        public void MoveUp()
        {
            if (MenuNum >= 0)
            {
                mainMenu[MenuNum].FillColor = Color.White;

                MenuNum--;
                if (MenuNum == -1)
                {
                    MenuNum = 3;
                }
                mainMenu[MenuNum].FillColor = Color.Blue;
            }
        }

        public void MoveDown()
        {
            if (MenuNum <= mainMenu.Count)
            {
                mainMenu[MenuNum].FillColor = Color.White;

                MenuNum++;
                if (MenuNum == mainMenu.Count)
                {
                    MenuNum = 0;
                }

                mainMenu[MenuNum].FillColor = Color.Blue;
            }
        }

    }
}
