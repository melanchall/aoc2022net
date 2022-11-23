using Aoc2022Net.Utilities;

namespace Aoc2022Net.Days
{
    internal sealed class Day20 : Day
    {
        private enum PixelType
        {
            [GridSymbol('#')]
            Light,
            [GridSymbol('.')]
            Dark
        }

        public override object SolvePart1() => Get(2);

        public override object SolvePart2() => Get(50);

        private int Get(int steps)
        {
            var linesGroups = InputData.GetInputLinesGroups();

            var imageEnhancementAlgorithm = linesGroups.First().First().Select(c => c.GetGridSymbolType<PixelType>()).ToArray();
            var (image, width, height) = InputData.GetGrid<PixelType>(linesGroups.Last());

            char GetBit(PixelType pixelType) =>
                pixelType == PixelType.Dark ? '0' : '1';

            PixelType GetImageEnhancementAlgorithmValue(string index) =>
                imageEnhancementAlgorithm[Convert.ToInt32(index, 2)];

            PixelType GetBackgroundNewState(PixelType afterType) =>
                GetImageEnhancementAlgorithmValue(new string(GetBit(afterType), 9));

            var backgroundPixelType = PixelType.Dark;

            for (var step = 0; step < steps; step++)
            {
                (image, width, height) = image.AddMargin(1, backgroundPixelType);

                var newImage = new PixelType[width, height];

                foreach (var (x, y) in newImage.GetAllCoordinates())
                {
                    var imageEnhancementAlgorithmIndexString = string.Empty;

                    foreach (var (windowY, windowX) in DataProvider.GetFullIndicesPairs(-1, 1))
                    {
                        var (gridY, gridX) = (y + windowY, x + windowX);
                        var neighborPixelType = gridX < 0 || gridX >= width || gridY < 0 || gridY >= height
                            ? backgroundPixelType
                            : image[gridX, gridY];
                        imageEnhancementAlgorithmIndexString += GetBit(neighborPixelType);
                    }

                    newImage[x, y] = GetImageEnhancementAlgorithmValue(imageEnhancementAlgorithmIndexString);
                }

                image = newImage;
                backgroundPixelType = GetBackgroundNewState(backgroundPixelType);
            }

            return image.CountValue(PixelType.Light);
        }
    }
}
