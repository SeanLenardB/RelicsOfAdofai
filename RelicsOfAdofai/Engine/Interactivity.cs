using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Raylib_cs;
using RelicsOfAdofai.Game;

namespace RelicsOfAdofai.Engine
{
    public class Interactivity
    {
        public double BackspaceCooldown = 0;
        public void HandleInput(GuiContext guiContext, GameContext gameContext)
        {
            var isMouseLeftDown = Raylib.IsMouseButtonDown(MouseButton.Left);
            var isMouseMiddleDown = Raylib.IsMouseButtonDown(MouseButton.Middle);
            var mousePosition = Raylib.GetMousePosition();

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

                var collide = Raylib.CheckCollisionPointRec(mousePosition, inputBox.CollisionBox);
                // Hover
                if (collide) { inputBox.IsHover = true; anyInputBoxHovered = true; }
                else if (inputBox.IsHover) inputBox.IsHover = false;

                // Active
                if (isMouseLeftDown && collide) inputBox.IsActive = true;
                else if (isMouseLeftDown && !collide) inputBox.IsActive = false;
            }
            if (anyInputBoxHovered) Raylib.SetMouseCursor(MouseCursor.IBeam);
            else Raylib.SetMouseCursor(MouseCursor.Default);

            // Button focus by mouse interaction
            foreach (var button in guiContext.Buttons.Values)
            {
                if (button.BelongingState != guiContext.GuiState) continue;

                var collide = Raylib.CheckCollisionPointRec(mousePosition, button.CollisionBox);
                // Hover
                if (collide) button.IsHover = true;
                else if (button.IsHover) button.IsHover = false;

                // Active
                if (isMouseLeftDown && collide) button.IsPressed = true;
                else if (!isMouseLeftDown && !collide) button.IsPressed = false;
                else if (!isMouseLeftDown && collide && button.IsPressed)
                {
                    button.IsPressed = false;
                    button.PressAction();
                }
            }

            if (guiContext.GuiState == GuiState.Game)
            {
                // Panning hex grid
                if (isMouseMiddleDown) { gameContext.CurrentChart.HexOrigin += Raylib.GetMouseDelta(); Raylib.SetMouseCursor(MouseCursor.PointingHand); }
                else Raylib.SetMouseCursor(MouseCursor.Default);

                // Hovering hex grid
                foreach (var cell in gameContext.CurrentChart.Cells)
                {
                    var collide = false;
                    if (mousePosition.Y > Style.HeaderHeight && mousePosition.Y + Style.HandHeight < Style.WindowHeight)
                        collide = Raylib.CheckCollisionPointPoly(
                            mousePosition - ((cell.Coords.Cartesian() * Style.HexCellSpaceRadius) + gameContext.CurrentChart.HexOrigin),
                            cell.Coords.BoundingBox);  // @hack: the list of points is static and precalculated. Therefore we "move" the mouse.

                    if (collide) cell.IsHover = true;
                    else if (cell.IsHover) cell.IsHover = false;
                }
            }
        }
    }
}
