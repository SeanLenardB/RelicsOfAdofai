using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var isMouseRightDown = Raylib.IsMouseButtonDown(MouseButton.Right);
            var mousePosition = Raylib.GetMousePosition();

            /* ---------- GENERIC GUI ---------- */
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



            /* ---------- SCENE SPECIFIC ---------- */
            if (guiContext.GuiState == GuiState.Game)
            {
                Debug.Assert(gameContext.CurrentChart is not null, "Cannot interact with a null chart!");
                // @cleanup: This is fairly inefficient because the algo needs to go through all hexagons to find which one is highlighted.
                // But since the transformation to hex -> cart is a matrix multiplication,
                // it's possible to invert the matrix and directly get the hex coords from the cart pixel location.
                //
                // But that will include some crazy offseting and other things. We will do this for now.
                // If the performance is bad, we will change it.

                var mouseInGrid = mousePosition.Y > Style.HeaderHeight && mousePosition.Y + Style.HandHeight < Style.WindowHeight;
                // Panning hex grid
                if (mouseInGrid && isMouseMiddleDown) { gameContext.CurrentChart.HexOrigin += Raylib.GetMouseDelta(); Raylib.SetMouseCursor(MouseCursor.PointingHand); }
                else Raylib.SetMouseCursor(MouseCursor.Default);

                // Hovering hex grid
                foreach (var cell in gameContext.CurrentChart.Cells)
                {
                    var collide = false;
                    if (mouseInGrid)
                        collide = Raylib.CheckCollisionPointPoly(
                            mousePosition - ((cell.Coords.Cartesian() * Style.HexCellSpaceRadius) + gameContext.CurrentChart.HexOrigin),
                            cell.Coords.BoundingBox);  // @hack: the list of points is static and precalculated. Therefore we "move" the mouse.

                    if (collide) cell.IsHover = true;
                    else if (cell.IsHover) cell.IsHover = false;

                    if (collide && isMouseLeftDown && gameContext.CurrentSelectedNode is not null)
                    {
                        cell.FilledNode?.IsUsed = false;
                        cell.FilledNode?.Rotation = 0;
                        Debug.Assert(!gameContext.CurrentSelectedNode.IsUsed, "Cannot use a used node!");
                        cell.FilledNode = gameContext.CurrentSelectedNode;
                        cell.FilledNode.IsUsed = true;
                        gameContext.RecalculateCurrentChart();
                    }

                    if (collide && isMouseRightDown && cell.FilledNode is not null)
                    {
                        cell.FilledNode.IsUsed = false;
                        cell.FilledNode.Rotation = 0;
                        cell.FilledNode = null;
                        gameContext.RecalculateCurrentChart();
                    }
                }

                // Hand hovering & selection
                var currentHandNodeCenter =
                    Layout.CenterCenter().Hpx(Style.NodeInHandRadius).YVh(100).DYpx(-Style.HandHeight / 2)
                        .Wpx(Style.NodeInHandRadius).Xpx(Style.HandHeight / 2).Vect();
                currentHandNodeCenter += new Vector2(Style.NodeInHandRadius / 2, Style.NodeInHandRadius / 2);
                gameContext.HandNodes.ForEach(n => n.IsHover = false);
                // Currently a selection will only get changed when the left click is down. Therefore it's safe to do this.
                // When we allow keyboard controls we need to refactor this.
                if (isMouseLeftDown) gameContext.CurrentSelectedNode = null;

                foreach (var node in gameContext.HandNodes)
                {
                    if (node.IsUsed) continue;

                    if (Raylib.CheckCollisionPointPoly(mousePosition - currentHandNodeCenter, node.BoundingBox))
                    {
                        node.IsHover = true;
                        if (isMouseLeftDown)
                        {
                            gameContext.CurrentSelectedNode?.Rotation = 0;
                            if (node != gameContext.CurrentSelectedNode) gameContext.CurrentSelectedNode = node;
                            break;
                        }
                    }

                    currentHandNodeCenter.X += Style.NodeInHandSpacing;
                }

                // Candidate node rotation
                if (mouseInGrid && Raylib.IsKeyPressed(KeyboardKey.Q)) gameContext.CurrentSelectedNode?.Rotation += 60;
                if (mouseInGrid && Raylib.IsKeyPressed(KeyboardKey.E)) gameContext.CurrentSelectedNode?.Rotation -= 60;
            }
        }
    }
}
