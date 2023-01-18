using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace AloneHero_CSharp
{
    class GameInterface
    {
        private Font font;
        private Image heartImage;
        private Image speedImage;
        private Texture heartTexture;
        private Texture speedTexture;
        private Sprite heartSprite;
        private Sprite speedSprite;
        private int healthUnits;
        private double speedUnits;

        public GameInterface(Game game)
        {
            font = new Font("timesnewromanpsmt.ttf");
            heartImage = new Image("Images\\Interface\\Heart.png");
            speedImage = new Image("Images\\Interface\\Energy.png");
            heartTexture = new Texture(heartImage);
            speedTexture = new Texture(speedImage);
            heartSprite = new Sprite(heartTexture);
            speedSprite = new Sprite(speedTexture);
            // Подписка
            game.LoadGame += GetMessageEventHandler;
        }


        public void GetMessageEventHandler(object sender, OrderEventArgs args)
        {
            if (args.Code == Codes.HEALTH_UNITS)
            {
                healthUnits = args.IntUnits;
            }
            else if (args.Code == Codes.SPEED_UNITS)
            {
                speedUnits = args.DoubleUnits;
            }
        }
        //public void GetMessage(Message message)
        //{
        //    if (message.code == Codes.HEALTH_UNITS)
        //    {
        //        healthUnits = message.intUnits;
        //    }
        //    else if (message.code == Codes.SPEED_UNITS)
        //    {
        //        speedUnits = message.doubleUnits;
        //    }
        //}
        public void Draw(RenderWindow window)
        {
            Text text = new Text(healthUnits.ToString(), font, 20);
            Vector2f center = window.GetView().Center;
            Vector2f size = window.GetView().Size;
            text.Position = new Vector2f(center.X - size.X / 2 + 25, center.Y - size.Y / 2 - 5);
            heartSprite.TextureRect = new IntRect(22, 18, 22, 19);
            heartSprite.Position = new Vector2f(center.X - size.X / 2, center.Y - size.Y / 2);
            window.Draw(heartSprite);
            window.Draw(text);

            text = new Text(speedUnits.ToString(), font, 20);
            text.Position = new Vector2f(center.X - size.X / 2 + 25, center.Y - size.Y / 2 + 15);
            speedSprite.TextureRect = new IntRect(0, 0, 16, 16);
            speedSprite.Position = new Vector2f(center.X - size.X / 2, center.Y - size.Y / 2 + 21);
            window.Draw(speedSprite);
            window.Draw(text);
        }
    }
}
