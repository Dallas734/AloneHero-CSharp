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
        private bool isMenu;
        public int MenuNum { get; private set; }

        public Menu(float width, float height)
        {
            MenuNum = 0;
            font = new Font("timesnewromanpsmt.ttf");
            mainMenu = new List<Text>();
            mainMenu.Add(new Text("Play", font, 70));
            mainMenu[0].Position = new Vector2f(400, 200);

            mainMenu.Add(new Text("About", font, 70));
            mainMenu[1].Position = new Vector2f(400, 300);

            mainMenu.Add(new Text("Exit", font, 70));
            mainMenu[2].Position = new Vector2f(400, 400);
        }

        public void Draw(RenderWindow window)
        {
            
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
                    MenuNum = 2;
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
