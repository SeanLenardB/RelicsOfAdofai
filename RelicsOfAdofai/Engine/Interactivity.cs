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
        public double MouseStayDuration = 0;
        public void HandleInput(GuiContext guiContext, GameContext gameContext)
        {
            var isMouseLeftDown = Raylib.IsMouseButtonDown(MouseButton.Left);
            var isMouseMiddleDown = Raylib.IsMouseButtonDown(MouseButton.Middle);
            var isMouseRightDown = Raylib.IsMouseButtonDown(MouseButton.Right);
            var mousePosition = Raylib.GetMousePosition();

            if (Raylib.IsKeyPressed(KeyboardKey.Grave)) gameContext.DebugMode = !gameContext.DebugMode;

            /* ---------- GENERIC GUI ---------- */
            // Input char
            if (guiContext.InputBoxes.Values.Any(i => i.IsActive))
            {
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
                    this.BackspaceCooldown -= gameContext.DeltaTime;
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
            }

            var isMouseStay = Raylib.GetMouseDelta() == new Vector2(0, 0);
            if (isMouseStay && this.MouseStayDuration < 10.0) this.MouseStayDuration += gameContext.DeltaTime;
            else if (!isMouseStay) this.MouseStayDuration = 0.0;



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
                if (!button.Enabled) continue;
                else if (isMouseLeftDown && collide) button.IsPressed = true;
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
                gameContext.CurrentChart.Cells.ForEach(c => c.IsHover = false);
                HexCoords mouseHexCoords = HexCoords.FromCartesian((mousePosition - gameContext.CurrentChart.HexOrigin) / Style.HexCellSpaceRadius);
                var collidedCell = gameContext.CurrentChart.Cells.FirstOrDefault(c =>
                {
                    var coordsDiff = c.Coords - mouseHexCoords;
                    return
                        -Style.HexCellDrawHexCoord <= coordsDiff.Q && coordsDiff.Q <= Style.HexCellDrawHexCoord &&
                        -Style.HexCellDrawHexCoord <= coordsDiff.R && coordsDiff.R <= Style.HexCellDrawHexCoord &&
                        -Style.HexCellDrawHexCoord <= coordsDiff.S && coordsDiff.S <= Style.HexCellDrawHexCoord;
                });

                if (collidedCell is not null)
                {
                    if (!collidedCell.IsHover) collidedCell.IsHover = true;

                    if (isMouseLeftDown && gameContext.CurrentSelectedNode is not null)
                    {
                        collidedCell.FilledNode?.IsUsed = false;
                        collidedCell.FilledNode?.Rotation = 0;
                        collidedCell.FilledNode?.IsFlipped = false;
                        Debug.Assert(!gameContext.CurrentSelectedNode.IsUsed, "Cannot use a used node!");
                        collidedCell.FilledNode = gameContext.CurrentSelectedNode;
                        collidedCell.FilledNode.IsUsed = true;
                        gameContext.RecalculateCurrentChart(guiContext);
                    }

                    if (isMouseRightDown && collidedCell.FilledNode is not null)
                    {
                        collidedCell.FilledNode.IsUsed = false;
                        collidedCell.FilledNode.Rotation = 0;
                        collidedCell.FilledNode.IsFlipped = false;
                        collidedCell.FilledNode = null;
                        gameContext.RecalculateCurrentChart(guiContext);
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
                            gameContext.CurrentSelectedNode?.IsFlipped = false;
                            if (node != gameContext.CurrentSelectedNode) gameContext.CurrentSelectedNode = node;
                            break;
                        }
                    }

                    currentHandNodeCenter.X += Style.NodeInHandSpacing;
                }

                // Candidate node rotation
                if (mouseInGrid && Raylib.IsKeyPressed(KeyboardKey.E)) gameContext.CurrentSelectedNode?.Rotation -= 60;
                if (mouseInGrid && Raylib.IsKeyPressed(KeyboardKey.Q)) gameContext.CurrentSelectedNode?.Rotation += 60;
                if (mouseInGrid && Raylib.IsKeyPressed(KeyboardKey.F)) gameContext.CurrentSelectedNode?.IsFlipped = !gameContext.CurrentSelectedNode.IsFlipped;
            }
        }
    }
}
