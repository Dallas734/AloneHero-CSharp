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
        private Texture heartTexture;
        private Sprite heartSprite;
        private int healthUnits;
        private int speedUnits;

        public GameInterface()
        {
            font = new Font("timesnewromanpsmt.ttf");
            heartImage = new Image("Images\\Interface\\Heart.png");
            heartTexture = new Texture(heartImage);
            heartSprite = new Sprite(heartTexture);
        }

        public void GetMessage(Message message)
        {
            if (message.code == Codes.HEALTH_UNITS)
            {
                healthUnits = message.units;
            }
            else if (message.code == Codes.SPEED_UNITS)
            {
                speedUnits = message.units;
            }
        }
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
        }
    }
}
