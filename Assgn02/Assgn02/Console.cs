using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assgn02;

namespace Assgn01
{

    class Console
    {
        string m_text;
        Boolean activated;
        Texture2D box;
        TextObject text;


        public Console(GraphicsDevice graphicsDevice, int w, int h)
        {
            box = new Texture2D(graphicsDevice, w, (h/3));
            text = new TextObject(m_text, Game1.font, Color.Black, false);
            Color[] data = new Color[w * (h/3)];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White * 0.2f;
            box.SetData(data);
            text.Position = Vector2.Zero + new Vector2(0, 120);

            activated = false;

            m_text = "";
        }

        public void Activate()
        {
            activated = true;
        }

        public void Deactivate()
        {
            m_text = "";
            activated = false;
        }

        public Boolean IsActivated()
        {
            return activated;
        }

        public void BackSpace()
        {
            if(m_text.Length != 0)
                m_text = m_text.Substring(0, m_text.Length - 1);
        }

        public void Draw(SpriteBatch spritebatch)
        {

            spritebatch.Draw(box, Vector2.Zero + new Vector2(0, 120), Color.Black);

            text.SetText(m_text);
            text.Draw(spritebatch);
            
        }

        public void TypeInto(Keys[] keys)
        {
            m_text += Convert(keys);


            if(keys.Contains(Keys.Enter))
            {
                if (m_text.Length > 13 && m_text.Substring(0, 13) == "set bg color ")
                    CheckIfColor();
                if(m_text.Length > 13 && m_text.Substring(0, 13) == "set ball spd ")
                    CheckIfSpeed();
                if(m_text.Length >= 9 && m_text.Substring(0, 9) == "stop ball")
                    Game1.SetBallSpeed(0f);
            }
        
        
        }

        public string Convert(Keys[] keys)
        {
            string output = "";
            bool usesShift = (keys.Contains(Keys.LeftShift) || keys.Contains(Keys.RightShift));

            foreach (Keys key in keys)  
            {
                if (key >= Keys.A && key <= Keys.Z)
                    output += key.ToString();
                else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                    output += ((int)(key - Keys.NumPad0)).ToString();
                else if (key >= Keys.D0 && key <= Keys.D9)
                {
                    string num = ((int)(key - Keys.D0)).ToString();
                    output += num;
                }
                else if(key == Keys.Space)
                {
                    output += " ";
                }
                else if(key == Keys.OemPeriod)
                {
                    output += ".";
                }
 
                if (!usesShift) output = output.ToLower();
            }
            return output;
        }

        void CheckIfColor()
        {
            String nums = m_text.Substring(13);
            
            switch(nums)
            {
                case "red":
                    Game1.SetBGColor(255, 0, 0);
                    break;
                case "green":
                    Game1.SetBGColor(0, 255, 0);
                    break;
                case "blue":
                    Game1.SetBGColor(0, 0, 255);
                    break;

                default:
                    break;
            }
        }

        void CheckIfSpeed()
        {
            String spd = m_text.Substring(13);

            try
            {
                float number = Single.Parse(spd);

                Game1.SetBallSpeed(number);
            }
            catch (FormatException)
            {
                System.Console.WriteLine("'{0}' is not in a valid format.", spd);
            }
            catch (OverflowException)
            {
                System.Console.WriteLine("{0} is outside the range of a Single.", spd);
            }
        }
        
    }

    
}
