using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace RelicsOfAdofai.Engine
{
    public class Interactivity
    {
        public double BackspaceCooldown = 0;
        public void HandleInput(GuiContext guiContext)
        {
            // Input char
            int code = Raylib.GetCharPressed();
            while (code != 0 && code >= 32 && code <= 125)  // @note: might change to wider/narrower ranges
            {
                char ch = (char)code;
                foreach (var inputBox in guiContext.InputBoxes.Values)
                {
                    if (inputBox.BelongingState != guiContext.GuiState) continue;
                    var modifiedString = inputBox.Text + ch;
                    if (inputBox.IsActive && inputBox.LegalText(modifiedString)) inputBox.Text = modifiedString;
                }

                if (Raylib.GetKeyPressed() == 0) break;
                code = Raylib.GetKeyPressed();  // @hack: for unknown reasons, we will get double duplicate inputs.
            }
            // @cleanup: The current implementation of deleting is very shit. Make it better.
            // Another problem is that the "time after previous delete" should be global instead of belonging to a specific input box.
            if (Raylib.IsKeyDown(KeyboardKey.Backspace))
            {
                this.BackspaceCooldown -= Raylib.GetFrameTime();
                foreach (var inputBox in guiContext.InputBoxes.Values)
                {
                    if (inputBox.BelongingState != guiContext.GuiState) continue;
                    if (!inputBox.IsActive && this.BackspaceCooldown > 0) continue;

                    if (inputBox.IsActive && this.BackspaceCooldown <= 0 && inputBox.Text.Length > 0)
                    {
                        inputBox.Text = inputBox.Text[..^1];
                        this.BackspaceCooldown = 0.1;
                    }
                }
            }
            else this.BackspaceCooldown = 0;



            // Input focus by mouse interaction
            bool anyInputBoxHovered = false;
            foreach (var inputBox in guiContext.InputBoxes.Values)
            {
                if (inputBox.BelongingState != guiContext.GuiState) continue;

                var collide = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), inputBox.CollisionBox);
                // Hover
                if (collide) { inputBox.IsHover = true; anyInputBoxHovered = true; }
                if (inputBox.IsHover && !collide) inputBox.IsHover = false;

                // Active
                var isMouseDown = Raylib.IsMouseButtonDown(MouseButton.Left);
                if (isMouseDown && collide) inputBox.IsActive = true;
                else if (isMouseDown && !collide) inputBox.IsActive = false;
            }
            if (anyInputBoxHovered) Raylib.SetMouseCursor(MouseCursor.IBeam);
            else Raylib.SetMouseCursor(MouseCursor.Default);

            // Button focus by mouse interaction
            foreach (var button in guiContext.Buttons.Values)
            {
                if (button.BelongingState != guiContext.GuiState) continue;

                var collide = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), button.CollisionBox);
                // Hover
                if (collide) button.IsHover = true;
                if (button.IsHover && !collide) button.IsHover = false;

                // Active
                var isMouseDown = Raylib.IsMouseButtonDown(MouseButton.Left);
                if (isMouseDown && collide) button.IsPressed = true;
                else if (!isMouseDown && !collide) button.IsPressed = false;
                else if (!isMouseDown && collide && button.IsPressed)
                {
                    button.IsPressed = false;
                    button.PressAction();
                }
            }
        }
    }
}
