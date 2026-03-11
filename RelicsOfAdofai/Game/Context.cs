using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Raylib_cs;
using RelicsOfAdofai.Game.Gui;

namespace RelicsOfAdofai.Game
{
    public class Context
    {
        public static GuiState GuiState = GuiState.Splashscreen;
        
        public static Dictionary<string, GuiInputBox> StateInputBoxes = [];

        public static int Seed = 114514;

        public static void GuiInit()
        {
            StateInputBoxes["rngseed"] = new() { LegalText = text => int.TryParse(text, out var _) };
        }
        public static void RecalculateUIPosition()
        {
            StateInputBoxes["rngseed"].CollisionBox = 
                Layout.LeftBottom().Hpx(Style.Font.BaseSize / 4).YVh(95).DYpx(-180).Wpx(480).Xvw(50).DXpx(48).Rect();
        }
        public static void HandleInput()
        {
            // Input char
            int code = Raylib.GetCharPressed();
            while (code != 0 && code >= 32 && code <= 125)  // @note: might change to wider/narrower ranges
            {
                char ch = (char)code;
                foreach (var inputBox in StateInputBoxes.Values)
                {
                    if (inputBox.BelongingState != GuiState) continue;
                    var modifiedString = inputBox.Text + ch;
                    if (inputBox.IsActive && inputBox.LegalText(modifiedString)) inputBox.Text = modifiedString;
                }

                if (Raylib.GetKeyPressed() == 0) break;
                code = Raylib.GetKeyPressed();  // @hack: for unknown reasons, we will get double duplicate inputs.
            }
            if (Raylib.IsKeyDown(KeyboardKey.Backspace))
            {
                foreach (var inputBox in StateInputBoxes.Values)
                {
                    if (inputBox.BelongingState != GuiState) continue;
                    if (!inputBox.IsActive && inputBox.TimeAfterPreviousDelete >= 0.05f) continue;

                    if (inputBox.IsActive && inputBox.Text.Length > 0) inputBox.TimeAfterPreviousDelete -= Raylib.GetFrameTime();
                    if (inputBox.TimeAfterPreviousDelete < 0)
                    {
                        inputBox.TimeAfterPreviousDelete = 0.05f;
                        inputBox.Text = inputBox.Text[..^1];
                    }
                }
            }



            // Input focus by mouse interaction
            foreach (var inputBox in StateInputBoxes.Values)
            {
                if (inputBox.BelongingState != GuiState) continue;

                var collide = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), inputBox.CollisionBox);
                // Hover
                if (collide) inputBox.IsHover = true;
                if (inputBox.IsHover && !collide) inputBox.IsHover = false;

                // Active
                var isMouseDown = Raylib.IsMouseButtonDown(MouseButton.Left);
                if (isMouseDown && collide) inputBox.IsActive = true;
                if (isMouseDown && !collide) inputBox.IsActive = false;
            }
        }
    }

    public enum GuiState
    {
        Splashscreen
    }
}
