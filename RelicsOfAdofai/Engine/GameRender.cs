using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using Raylib_cs;
using RelicsOfAdofai.Engine.Gui;
using RelicsOfAdofai.Game;

namespace RelicsOfAdofai.Engine
{
    public class GameRender
    {
        /* ----------- SPLASH ----------- */
        public void SplashScreen()
        {
            var bg = Style.Textures["bg"];

            var widthMultiplier = 1.0 * Style.WindowWidth / bg.Width;
            var heightMultiplier = 1.0 * Style.WindowHeight / bg.Height;
            var finalMultiplier = widthMultiplier > heightMultiplier ? widthMultiplier : heightMultiplier;

            finalMultiplier *= 1.15;  // Leaving room for the cursor effect
            var cursorPosition = Raylib.GetMousePosition();
            var xOffsetProportion = -(cursorPosition.X - (Style.WindowWidth / 2.0)) / Style.WindowWidth / 2.0;
            var yOffsetProportion = (cursorPosition.Y - (Style.WindowHeight / 2.0)) / Style.WindowHeight / 2.0;

            var scaledWidth = (int)(bg.Width * finalMultiplier);
            var scaledHeight = (int)(bg.Height * finalMultiplier);

            Raylib.DrawTexturePro(
                bg,
                new(0, 0, bg.Width, bg.Height),
                new(
                    (Style.WindowWidth / 2) + (int)(xOffsetProportion * bg.Width * 0.1),
                    (Style.WindowHeight / 2) + (int)(yOffsetProportion * bg.Height * 0.1),
                    scaledWidth, scaledHeight),
                new(scaledWidth / 2, scaledHeight / 2), 0,
                Color.Gray);



            var titleExtent = Raylib.MeasureTextEx(Style.FontStylistic, "Relics of Adofai", Style.SizeTitle, 0);
            Raylib.DrawRectangleRounded(
                Layout.CenterTop().Hpx(titleExtent.Y * 3).YVh(15).Wvw(60).Wmax(1600).Wmin(titleExtent.X).Xvw(50).Rect(),
                0.1f,
                8,
                Style.ColorBgDark);
            Raylib.DrawTextEx(
                Style.FontStylistic,
                "Relics of Adofai",
                Layout.CenterTop().Hpx(titleExtent.Y).YVh(15).Wpx(titleExtent.X).Xvw(50).DYpx(titleExtent.Y).Vect(),
                Style.SizeTitle,
                0,
                Style.ColorTextGeneral);

            Raylib.DrawRectangleRounded(
                Layout.CenterBottom().Hvh(45).YVh(95).Wvw(60).Wmax(1600).Wmin(720).Xvw(50).Rect(),
                0.1f,
                8,
                Style.ColorBgMedium);

            var rngExtent = Raylib.MeasureTextEx(Style.FontGeneral, "随机数种子", Style.SizeNormal, 0);
            Raylib.DrawTextEx(
                Style.FontGeneral,
                "随机数种子",
                Layout.RightCenter().Hpx(rngExtent.Y).YVh(95).DYpx(-360).Wpx(rngExtent.X).Xvw(50).DXpx(-24).Vect(),
                Style.SizeNormal,
                0,
                Style.ColorTextGeneral);
        }


        
        /* ----------- GAME ----------- */
        public void Game(GameContext gameContext, Interactivity interactivity)
        {
            /* ----- BACKGROUND ----- */
            // @copypasta: from SplashScreen()
            Debug.Assert(Style.Textures.ContainsKey("bg"), "Cannot find the background image bg!");
            var bg = Style.Textures["bg"];

            var widthMultiplier = 1.0 * Style.WindowWidth / bg.Width;
            var heightMultiplier = 1.0 * Style.WindowHeight / bg.Height;
            var finalMultiplier = widthMultiplier > heightMultiplier ? widthMultiplier : heightMultiplier;

            finalMultiplier *= 1.15;  // Leaving room for the cursor effect
            var cursorPosition = Raylib.GetMousePosition();
            var xOffsetProportion = -(cursorPosition.X - (Style.WindowWidth / 2.0)) / Style.WindowWidth / 2.0;
            var yOffsetProportion = (cursorPosition.Y - (Style.WindowHeight / 2.0)) / Style.WindowHeight / 2.0;

            var scaledWidth = (int)(bg.Width * finalMultiplier);
            var scaledHeight = (int)(bg.Height * finalMultiplier);

            Raylib.DrawTexturePro(
                bg,
                new(0, 0, bg.Width, bg.Height),
                new(
                    (Style.WindowWidth / 2) + (int)(xOffsetProportion * bg.Width * 0.1),
                    (Style.WindowHeight / 2) + (int)(yOffsetProportion * bg.Height * 0.1),
                    scaledWidth, scaledHeight),
                new(scaledWidth / 2, scaledHeight / 2), 0,
                Color.Gray);



            /* ----- GRID ----- */
            this.DrawChartGrid(gameContext, interactivity);



            /* ----- HEADER ----- */
            var headerRect = Layout.CenterTop().Hpx(Style.HeaderHeight).Ypx(0).Wvw(100).Xvw(50).Rect();
            var headerRectLeftHalf = headerRect;
            headerRectLeftHalf.Width /= 2;
            var headerRectRightHalf = headerRectLeftHalf;
            headerRectRightHalf.X += headerRectRightHalf.Width;
            Raylib.DrawRectangle(
                (int)headerRect.X, (int)headerRect.Y, 
                (int)headerRect.Width, (int)headerRect.Height,
                Style.ColorBgLight);
            Raylib.DrawRectangleGradientEx(
                headerRectLeftHalf,
                Color.Blank, Color.Blank, Style.ColorBgInputGradientInactive, Color.Blank);
            Raylib.DrawRectangleGradientEx(
                headerRectRightHalf,
                Color.Blank, Style.ColorBgInputGradientInactive, Color.Blank, Color.Blank);
            Raylib.DrawRectangleGradientH(
                0, (int)headerRect.Height,
                (int)(headerRect.Width / 2), Style.NormalThickness, Style.ColorBorderBlack, Style.ColorBorderLight);
            Raylib.DrawRectangleGradientH(
                (int)(headerRect.Width / 2), (int)headerRect.Height,
                (int)(headerRect.Width / 2), Style.NormalThickness, Style.ColorBorderLight, Style.ColorBorderBlack);

            var headerString = gameContext.DebugMode ? "Debug Mode" : "Relics of Adofai";
            var titleExtent = Raylib.MeasureTextEx(Style.FontStylistic, headerString, Style.SizeNormal, 0);
            var padding = (128 - titleExtent.Y) / 2;
            Raylib.DrawTextEx(
                Style.FontStylistic,
                headerString,
                new(padding, padding),
                Style.SizeNormal,
                0,
                Style.ColorTextGeneral);

            /*
             * [  ]----[  ]----[  ]
             *    < 96 >
             * <40>
             */
            var chartListLineLength = 136 * (gameContext.Charts.Count - 1);
            Raylib.DrawLineEx(
                new((Style.WindowWidth - chartListLineLength) / 2, 64),
                new((Style.WindowWidth + chartListLineLength) / 2, 64),
                4.0f,
                Style.ColorBorderLight);
            var firstIconCenterX = (Style.WindowWidth - chartListLineLength ) / 2;
            for (int i = 0; i < gameContext.Charts.Count; i++)
            {
                var iconRect = Layout.CenterCenter().Hpx(40).Ypx(64).Wpx(40).Xpx(firstIconCenterX + (i * 136)).Rect();
                Raylib.DrawRectangle(
                    (int)iconRect.X, (int)iconRect.Y, (int)iconRect.Width, (int)iconRect.Height,
                    gameContext.Charts[i].IconColor);
                Raylib.DrawRectangleRoundedLinesEx(
                    iconRect,
                    0.1f,
                    8,
                    4f,
                    Style.ColorBorderLight);
            }



            /* ----- HAND ----- */
            var handRect = Layout.CenterBottom().Hpx(Style.HandHeight).YVh(100).Wvw(100).Xvw(50).Rect();
            Raylib.DrawRectangleGradientV(
                (int)handRect.X, (int)handRect.Y, (int)handRect.Width, (int)handRect.Height,
                Style.ColorBgInputGradientActive, Style.ColorBgInputGradientInactive);
            Raylib.DrawLineEx(
                new(handRect.X, handRect.Y),
                new(handRect.X + handRect.Width, handRect.Y),
                Style.NormalThickness,
                Style.ColorBorderLight);

            this.DrawHand(gameContext);
        }

        public void DrawChartGrid(GameContext gameContext, Interactivity interactivity)
        {
            Debug.Assert(gameContext.CurrentChart is not null, "Cannot render a null chart!");
            ChartCell? hoveredCell = null;


            /*----- Cells -----*/
            foreach (var cell in gameContext.CurrentChart.Cells)
            {
                var cellCenter = (cell.Coords.Cartesian() * Style.HexCellSpaceRadius) + gameContext.CurrentChart.HexOrigin;
                var polyDrawCenter = cellCenter + new Vector2(0, Style.NormalThickness / 2);
                Raylib.DrawPolyLinesEx(
                    polyDrawCenter,
                    6,
                    Style.HexCellDrawRadius,
                    30,
                    Style.NormalThickness,
                    Style.ColorBorderMedium);

                var drawRect = Layout.CenterCenter()
                        .Hpx(Style.NodeTextureSize).Ypx(cellCenter.Y)
                        .Wpx(Style.NodeTextureSize).Xpx(cellCenter.X).RectCenter();

                if (cell.FilledNode is not null)
                {
                    var filledNodeTexture = Style.Textures[cell.FilledNode.ResourceKey()];
                    Raylib.DrawTexturePro(  // Technically Ex works here but we want versatile drawing if animation is needed.
                        filledNodeTexture,
                        cell.FilledNode.IsFlipped ? 
                            new(Style.NodeTextureSize, 0, -Style.NodeTextureSize, Style.NodeTextureSize) :
                            new(0, 0, Style.NodeTextureSize, Style.NodeTextureSize),
                        drawRect,
                        new(Style.NodeTextureSize / 2, Style.NodeTextureSize / 2),
                        -cell.FilledNode.Rotation,
                        Style.HintUnselectedNode);
                }
                else Raylib.DrawPoly(polyDrawCenter, 6, Style.HexCellDrawRadius, 30, Style.ColorBgDark);

                if (cell.IsHover) hoveredCell = cell;

                var imageLocation = cellCenter;
                imageLocation.X -= 64; imageLocation.Y -= 64;  // The image is 256x256. We draw 0.5x.
                if (cell.Type == ChartCell.CellType.Source)
                {
                    var startEnergyText = cell.SourceEnergy.ToString("0.0");
                    var startEnergyTextExtent = Raylib.MeasureTextEx(Style.FontStylistic, startEnergyText, Style.SizeSmall, 0);
                    Raylib.DrawTextureEx(
                        Style.Textures["nodeStart"],
                        imageLocation,
                        0,
                        0.5f,
                        new(255, 255, 255, 128));
                    Raylib.DrawTextEx(
                        Style.FontStylistic,
                        startEnergyText,
                        Layout.CenterTop()
                            .Wpx(startEnergyTextExtent.X).Xpx(cellCenter.X)
                            .Hpx(startEnergyTextExtent.Y).Ypx(cellCenter.Y).DYpx(Style.HexCellDrawRadius / 3).Vect(),
                        Style.SizeSmall,
                        0,
                        Style.ColorTextGeneral);
                }
                else if (cell.Type == ChartCell.CellType.End)
                {
                    Raylib.DrawTextureEx(
                        Style.Textures["nodeEnd"],
                        imageLocation,
                        0,
                        0.5f,
                        new(255, 255, 255, 128));
                }
            }


            /*----- Right-top energy text -----*/
            var targetEnergyText = gameContext.CurrentChart.OptimalEnergy.ToString("0.0");
            var targetEnergyTextExtent = Raylib.MeasureTextEx(Style.FontStylistic, targetEnergyText, Style.SizeNormal, 0);
            Raylib.DrawTextEx(
                Style.FontStylistic,
                targetEnergyText,
                Layout.RightTop()
                    .Hpx(targetEnergyTextExtent.Y).Ypx(Style.HeaderHeight + (targetEnergyTextExtent.Y / 2))
                    .Wpx(targetEnergyTextExtent.X).Xpx(Style.WindowWidth - (targetEnergyTextExtent.Y / 2))
                    .Vect(),
                Style.SizeNormal,
                0,
                Style.ColorTextGeneral);

            string receivedEnergyText;
            if (gameContext.CurrentChart.FinalEnergy <= 0.0) receivedEnergyText = "未连通";
            else receivedEnergyText = gameContext.CurrentChart.FinalEnergy.ToString("接收总量0.0");

            var receivedEnergyTextExtent = Raylib.MeasureTextEx(Style.FontGeneral, receivedEnergyText, Style.SizeSmall, 0);
            Raylib.DrawTextEx(
                Style.FontGeneral,
                receivedEnergyText,
                Layout.RightTop()
                    .Hpx(receivedEnergyTextExtent.Y).Ypx(Style.HeaderHeight + (receivedEnergyTextExtent.Y / 2)).DYpx(targetEnergyTextExtent.Y)
                    .Wpx(receivedEnergyTextExtent.X).Xpx(Style.WindowWidth - (receivedEnergyTextExtent.Y / 2))
                    .Vect(),
                Style.SizeSmall,
                0,
                Style.ColorTextGeneral);



            /*----- Hover-snap cell drawing -----*/
            // @hack: the hover hint should render after everything has been drawn. Otherwise it might get overriden.
            if (hoveredCell is null) return;
            var hoveredCellCenter = (hoveredCell.Coords.Cartesian() * Style.HexCellSpaceRadius) + gameContext.CurrentChart.HexOrigin;
            var hoveredCellDrawRect = Layout.CenterCenter()
                    .Hpx(Style.NodeTextureSize).Ypx(hoveredCellCenter.Y)
                    .Wpx(Style.NodeTextureSize).Xpx(hoveredCellCenter.X).RectCenter();
            Raylib.DrawPoly(hoveredCellCenter, 6, Style.HexCellDrawRadius, 30, Style.ColorBgMedium);
            if (gameContext.CurrentSelectedNode is not null)
            {
                var selectedNodeTexture = Style.Textures[gameContext.CurrentSelectedNode.ResourceKey()];
                Raylib.DrawTexturePro(  // Technically Ex works here but we want versatile drawing if animation is needed.
                    selectedNodeTexture,
                    gameContext.CurrentSelectedNode.IsFlipped ?
                        new(Style.NodeTextureSize, 0, -Style.NodeTextureSize, Style.NodeTextureSize) :
                        new(0, 0, Style.NodeTextureSize, Style.NodeTextureSize),
                    hoveredCellDrawRect,
                    new(Style.NodeTextureSize / 2, Style.NodeTextureSize / 2),
                    -gameContext.CurrentSelectedNode.Rotation,
                    Style.HintSelectedNode);
                this.DrawFluxHint(gameContext, hoveredCell, gameContext.CurrentSelectedNode);
            }
            

            if (hoveredCell.FilledNode is not null && interactivity.MouseStayDuration > Style.MouseStayDurationThreshold)
            {
                var mousePosition = Raylib.GetMousePosition();
                var inText = hoveredCell.FluxIn.ToString("流入0.00");
                var outText = hoveredCell.FluxOut.ToString("流出0.00");
                var inTextExtent = Raylib.MeasureTextEx(Style.FontGeneral, inText, Style.SizeSmall, 0);
                var outTextExtent = Raylib.MeasureTextEx(Style.FontGeneral, outText, Style.SizeSmall, 0);

                var boxRect = Layout.CenterBottom()
                    .Wpx((3 * inTextExtent.Y) + (inTextExtent.X + outTextExtent.X)).Xpx(mousePosition.X)
                    .Hpx(inTextExtent.Y * 2).Ypx(mousePosition.Y).Rect();
                var inTextVect = Layout.RightCenter()
                    .Wpx(inTextExtent.X).Xpx(mousePosition.X - (inTextExtent.Y * 0.5))
                    .Hpx(inTextExtent.Y).Ypx(mousePosition.Y - inTextExtent.Y).Vect();
                var outTextVect = Layout.LeftCenter()
                    .Wpx(outTextExtent.X).Xpx(mousePosition.X + (inTextExtent.Y * 0.5))
                    .Hpx(outTextExtent.Y).Ypx(mousePosition.Y - outTextExtent.Y).Vect();

                Raylib.DrawRectangleRounded(boxRect, 0.1f, 8, Style.ColorBgDark);
                Raylib.DrawRectangleRoundedLinesEx(boxRect, 0.1f, 8, Style.ThinThickness, Style.ColorBorderLight);
                Raylib.DrawTextEx(Style.FontGeneral, inText, inTextVect, Style.SizeSmall, 0, Style.ColorBorderFluxIn);
                Raylib.DrawTextEx(Style.FontGeneral, outText, outTextVect, Style.SizeSmall, 0, Style.ColorBorderFluxOut);

                this.DrawFluxHint(gameContext, hoveredCell, hoveredCell.FilledNode);
            }
        }

        public void DrawFluxHint(GameContext gameContext, ChartCell cell, SkillNode node)
        {
            Debug.Assert(gameContext.CurrentChart is not null, "Trying to draw on a null chart!");
            var hoveredCellCenter = (cell.Coords.Cartesian() * Style.HexCellSpaceRadius) + gameContext.CurrentChart.HexOrigin;
            /// <see cref="GameContext.PropagateChartCell(Chart, ChartCell, GameContext.CellPropagationPacket)"/>
            switch (node.Type)  // @copypasta
            {
                case SkillNode.NodeType.Extractor_Single:
                    {
                        var outOffsetAngle = 0;
                        if (node.IsFlipped) outOffsetAngle = 180 - outOffsetAngle;
                        outOffsetAngle += node.Rotation;
                        var outCenter = hoveredCellCenter + (HexCoords.RotationUnit(outOffsetAngle).Cartesian() * Style.HexCellSpaceRadius);
                        Raylib.DrawPolyLinesEx(hoveredCellCenter, 6, Style.HexCellDrawRadius, 30, Style.ThinThickness, Style.ColorBorderFluxIn);
                        Raylib.DrawPolyLinesEx(outCenter, 6, Style.HexCellDrawRadius, 30, Style.ThinThickness, Style.ColorBorderFluxOut);
                        break;
                    }

                case SkillNode.NodeType.Connector_Opposite:
                    {
                        var inOffsetAngle = 180; var outOffsetAngle = 0;
                        if (node.IsFlipped) { inOffsetAngle = 180 - inOffsetAngle; outOffsetAngle = 180 - outOffsetAngle; }
                        inOffsetAngle += node.Rotation; outOffsetAngle += node.Rotation;
                        var inCenter = hoveredCellCenter + (HexCoords.RotationUnit(inOffsetAngle).Cartesian() * Style.HexCellSpaceRadius);
                        var outCenter = hoveredCellCenter + (HexCoords.RotationUnit(outOffsetAngle).Cartesian() * Style.HexCellSpaceRadius);
                        Raylib.DrawPolyLinesEx(inCenter, 6, Style.HexCellDrawRadius, 30, Style.ThinThickness, Style.ColorBorderFluxIn);
                        Raylib.DrawPolyLinesEx(outCenter, 6, Style.HexCellDrawRadius, 30, Style.ThinThickness, Style.ColorBorderFluxOut);
                        break;
                    }

                case SkillNode.NodeType.Connector_Interval:
                    {
                        var inOffsetAngle = 180; var outOffsetAngle = 60;
                        if (node.IsFlipped) { inOffsetAngle = 180 - inOffsetAngle; outOffsetAngle = 180 - outOffsetAngle; }
                        inOffsetAngle += node.Rotation; outOffsetAngle += node.Rotation;
                        var inCenter = hoveredCellCenter + (HexCoords.RotationUnit(inOffsetAngle).Cartesian() * Style.HexCellSpaceRadius);
                        var outCenter = hoveredCellCenter + (HexCoords.RotationUnit(outOffsetAngle).Cartesian() * Style.HexCellSpaceRadius);
                        Raylib.DrawPolyLinesEx(inCenter, 6, Style.HexCellDrawRadius, 30, Style.ThinThickness, Style.ColorBorderFluxIn);
                        Raylib.DrawPolyLinesEx(outCenter, 6, Style.HexCellDrawRadius, 30, Style.ThinThickness, Style.ColorBorderFluxOut);
                        break;
                    }

                case SkillNode.NodeType.Connector_Adjacent:
                    {
                        var inOffsetAngle = 180; var outOffsetAngle = 120;
                        if (node.IsFlipped) { inOffsetAngle = 180 - inOffsetAngle; outOffsetAngle = 180 - outOffsetAngle; }
                        inOffsetAngle += node.Rotation; outOffsetAngle += node.Rotation;
                        var inCenter = hoveredCellCenter + (HexCoords.RotationUnit(inOffsetAngle).Cartesian() * Style.HexCellSpaceRadius);
                        var outCenter = hoveredCellCenter + (HexCoords.RotationUnit(outOffsetAngle).Cartesian() * Style.HexCellSpaceRadius);
                        Raylib.DrawPolyLinesEx(inCenter, 6, Style.HexCellDrawRadius, 30, Style.ThinThickness, Style.ColorBorderFluxIn);
                        Raylib.DrawPolyLinesEx(outCenter, 6, Style.HexCellDrawRadius, 30, Style.ThinThickness, Style.ColorBorderFluxOut);
                        break;
                    }

                case SkillNode.NodeType.Receiver_Neighbor:
                    {
                        foreach (var direction in HexCoords.Directions)
                            Raylib.DrawPolyLinesEx(
                                hoveredCellCenter + (direction.Cartesian() * Style.HexCellSpaceRadius),
                                6,
                                Style.HexCellDrawRadius,
                                30,
                                Style.ThinThickness,
                                Style.ColorBorderFluxIn);
                        Raylib.DrawPolyLinesEx(hoveredCellCenter, 6, Style.HexCellDrawRadius, 30, Style.ThinThickness, Style.ColorBorderFluxOut);
                        break;
                    }

                default: Debug.Assert(false, "Discriminated union!"); return;
            }
        }

        public void DrawHand(GameContext gameContext)
        {
            // @note: also see code in Interactivity.
            var currentDrawRect =
                Layout.CenterCenter().Hpx(Style.NodeTextureSize).YVh(100).DYpx(-Style.HandHeight / 2)
                    .Wpx(Style.NodeTextureSize).Xpx(Style.HandHeight / 2).RectCenter();
            foreach (var node in gameContext.HandNodes)
            {
                if (node.IsUsed) continue;
                var texture = Style.Textures[node.ResourceKey()];

                var scale = node.IsHover ? 1.1f : 1;
                var scaledDrawRect = currentDrawRect;
                scaledDrawRect.Width *= scale;
                scaledDrawRect.Height *= scale;

                var colorHint = gameContext.CurrentSelectedNode == node ? Style.HintSelectedNode : Style.HintUnselectedNode;

                Raylib.DrawTexturePro(
                    texture,
                    new(0, 0, Style.NodeTextureSize, Style.NodeTextureSize),
                    scaledDrawRect,
                    new(scaledDrawRect.Width / 2, scaledDrawRect.Height / 2),
                    0,
                    colorHint);

                currentDrawRect.X += Style.NodeInHandSpacing;
            }
        }



        /* ----------- GENERIC GUI ----------- */
        public void RenderGui(GameContext gameContext, GuiContext guiContext, Interactivity interactivity)
        {
            foreach (var inputBox in guiContext.InputBoxes.Values)
            {
                if (inputBox.BelongingState != guiContext.GuiState) continue;

                Color underbarColor;
                if (inputBox.IsActive) underbarColor = Style.ColorBorderLight;
                else if (inputBox.IsHover) underbarColor = Style.ColorBorderMedium;
                else underbarColor = Style.ColorBorderDark;

                if (inputBox.IsActive)
                    Raylib.DrawRectangleGradientEx(
                        inputBox.CollisionBox,
                        Color.Blank, Style.ColorBgInputGradientActive, Style.ColorBgInputGradientActive, Color.Blank);
                else
                    Raylib.DrawRectangleGradientEx(
                        inputBox.CollisionBox,
                        Color.Blank, Style.ColorBgInputGradientInactive, Style.ColorBgInputGradientInactive, Color.Blank);

                Raylib.DrawLineEx(
                    new(inputBox.CollisionBox.X, inputBox.CollisionBox.Y + inputBox.CollisionBox.Height),
                    new(inputBox.CollisionBox.X + inputBox.CollisionBox.Width, inputBox.CollisionBox.Y + inputBox.CollisionBox.Height),
                    4.0f,
                    underbarColor);

                var textExtent = Raylib.MeasureTextEx(Style.FontGeneral, inputBox.Text, inputBox.TextSize, 0);

                var yPadding = (inputBox.CollisionBox.Height - textExtent.Y) / 2.0f;
                if (yPadding < 0) yPadding = 0;

                var xPadding = 16.0f;  // @note: this should be dependent on text?
                if (inputBox.Align == InputBox.TextAlign.Center) xPadding = (inputBox.CollisionBox.Width - textExtent.X) / 2.0f;
                else if (inputBox.Align == InputBox.TextAlign.Right) xPadding = inputBox.CollisionBox.Width - textExtent.X - xPadding;
                if (xPadding < 0) xPadding = 0;

                Raylib.DrawTextEx(
                    Style.FontGeneral,
                    inputBox.Text,
                    new(inputBox.CollisionBox.X + xPadding, inputBox.CollisionBox.Y + yPadding),
                    inputBox.TextSize,
                    0,
                    Style.ColorTextGeneral);
            }
            foreach (var button in guiContext.Buttons.Values)
            {
                if (button.BelongingState != guiContext.GuiState) continue;

                Color outlineColor;
                if (!button.Enabled) outlineColor = Style.ColorBorderBlack;
                else if (button.IsPressed) outlineColor = Style.ColorBorderDark;
                else if (button.IsHover) outlineColor = Style.ColorBorderLight;
                else outlineColor = Style.ColorBorderMedium;

                Raylib.DrawRectangleRoundedLinesEx(
                    button.CollisionBox,
                    0.1f,
                    8,
                    4.0f,
                    outlineColor);

                var textExtent = Raylib.MeasureTextEx(Style.FontGeneral, button.Text, button.TextSize, 0);

                var yPadding = (button.CollisionBox.Height - textExtent.Y) / 2.0f;
                if (yPadding < 0) yPadding = 0;

                var xPadding = 16.0f;  // @note: this should be dependent on text?
                if (button.Align == Button.TextAlign.Center) xPadding = (button.CollisionBox.Width - textExtent.X) / 2.0f;
                else if (button.Align == Button.TextAlign.Right) xPadding = button.CollisionBox.Width - textExtent.X - xPadding;
                if (xPadding < 0) xPadding = 0;

                Raylib.DrawTextEx(
                    Style.FontGeneral,
                    button.Text,
                    new(button.CollisionBox.X + xPadding, button.CollisionBox.Y + yPadding),
                    button.TextSize,
                    0,
                    Style.ColorTextGeneral);

                var shouldDrawDisabledHint = 
                    !button.Enabled && button.IsHover 
                    && interactivity.MouseStayDuration > Style.MouseStayDurationThreshold && !string.IsNullOrEmpty(button.DisabledHint);
                if (shouldDrawDisabledHint)
                {
                    var mousePosition = Raylib.GetMousePosition();
                    var inTextExtent = Raylib.MeasureTextEx(Style.FontGeneral, button.DisabledHint, Style.SizeSmall, 0);

                    var boxRect = Layout.CenterBottom()
                        .Wpx((3 * inTextExtent.Y) + inTextExtent.X).Xpx(mousePosition.X)
                        .Hpx(inTextExtent.Y * 2).Ypx(mousePosition.Y).Rect();
                    var inTextVect = Layout.CenterCenter()
                        .Wpx(inTextExtent.X).Xpx(mousePosition.X)
                        .Hpx(inTextExtent.Y).Ypx(mousePosition.Y - inTextExtent.Y).Vect();

                    Raylib.DrawRectangleRounded(boxRect, 0.1f, 8, Style.ColorBgDark);
                    Raylib.DrawRectangleRoundedLinesEx(boxRect, 0.1f, 8, Style.ThinThickness, Style.ColorBorderLight);
                    Raylib.DrawTextEx(Style.FontGeneral, button.DisabledHint, inTextVect, Style.SizeSmall, 0, Style.ColorBorderFluxIn);
                }
            }
            foreach (var message in guiContext.FloatingMessages)
            {
                message.RemainingTime -= gameContext.DeltaTime;
                message.Position += message.Velocity * (float)gameContext.DeltaTime;
                Raylib.DrawTextEx(Style.FontGeneral, message.Text, message.Position, Style.SizeSmall, 0, Style.ColorTextGeneral);
            }

            while (guiContext.FloatingMessages.Count != 0 && guiContext.FloatingMessages.Peek().RemainingTime <= 0)
                guiContext.FloatingMessages.Dequeue();
        }
    }
}
