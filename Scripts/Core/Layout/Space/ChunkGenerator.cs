using System.Linq;
using UnityEngine;
using Kyzlyk.Helpers;
using Core.Building;
using System.Collections.Generic;

namespace Core.Layout.Space
{
    public struct ChunkGenerator
    {
        public static Vector2[] GenerateFromStart(byte[][] space, int width, int height, Builder builder, Vector2 offset)
        {
            List<Vector2> spacePositions = new(width * height);

            Vector2 currentPoint = offset;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (space[i][j] != 0)
                    {
                        builder.CreateGMaterial(currentPoint, new GMaterialTexture(), false);
                        spacePositions.Add(currentPoint);
                    }

                    currentPoint.x++;
                }

                currentPoint.x = offset.x;
                currentPoint.y--;
            }

            builder.Apply();

            return spacePositions.ToArray();
        }
        
        public static Vector2[] GenerateFromStart(byte[][] space, Chunk chunk, Vector2 offset)
        {
            return GenerateFromStart(space, chunk.Width, chunk.Height, chunk.Builder, offset);
        }

        public static Vector2[] GenerateFromEnd(byte[][] space, Builder builder, Vector2 offset)
        {
            List<Vector2> spacePositions = new(space.Length);

            Vector2 currentPoint = offset;

            for (int i = space.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < space[i].Length; j++)
                {
                    if (space[i][j] != 0)
                    {
                        builder.CreateGMaterial(currentPoint, new GMaterialTexture(), false);
                        spacePositions.Add(currentPoint);
                    }

                    currentPoint.x++;
                }

                currentPoint.x = offset.x;
                currentPoint.y++;
            }

            builder.Apply();

            return spacePositions.ToArray();
        }
        
        public static Vector2[] GenerateFromEnd(byte[][] space, int width, int height, Builder builder, Vector2 offset)
        {
            List<Vector2> spacePositions = new(width * height);

            Vector2 currentPoint = offset;

            for (int i = height - 1; i >= 0; i--)
            {
                for (int j = 0; j < width; j++)
                {
                    if (space[i][j] != 0)
                    {
                        builder.CreateGMaterial(currentPoint, new GMaterialTexture(), false);
                        spacePositions.Add(currentPoint);
                    }

                    currentPoint.x++;
                }

                currentPoint.x = offset.x;
                currentPoint.y++;
            }

            builder.Apply();

            return spacePositions.ToArray();
        }
        
        public static Vector2[] GenerateFromEnd(byte[][] space, Chunk chunk, Vector2 offset)
        {
            return GenerateFromEnd(space, chunk.Width, chunk.Height, chunk.Builder, offset);
        }

        public static Vector2[] GenerateWithPlatforms(int width, int height, Builder builder, int randomCoefficient, Vector2 offset)
        {
            if (randomCoefficient == 0)
                return System.Array.Empty<Vector2>();

            List<Vector2> space = new(width * height);
            Vector2 currentPosition = offset;

            if (randomCoefficient >= 100)
            {
                GenerateFromEnd(Enumerable.Repeat<byte[]>(Enumerable.Repeat<byte>(1, width).ToArray(), height).ToArray(), width, height, builder, offset);
            }

            Range rangeLengthOfPlatform = new(2, 5);

            int GetRandomPlatformLength()
                => Random.Range(rangeLengthOfPlatform.StartValue, rangeLengthOfPlatform.EndValue + 1);

            bool BuildNewPlatform()
                => Random.Range(0, 100) <= randomCoefficient;

            for (int i = 0; i < height; i++)
            {
                int maxPlatformLength = GetRandomPlatformLength();

                int platformCountOnRow = 0;
                int currentPlatformLength = 0;

                bool endOfBuildingCurrentPlatform = !BuildNewPlatform();

                for (int j = 0; j < width; j++)
                {
                    bool skip = false;

                    if (!skip && endOfBuildingCurrentPlatform)
                    {
                        if (BuildNewPlatform())
                            endOfBuildingCurrentPlatform = false;

                        skip = true;
                    }

                    if (!skip && currentPlatformLength == maxPlatformLength)
                    {
                        maxPlatformLength = GetRandomPlatformLength();
                        endOfBuildingCurrentPlatform = true;
                        currentPlatformLength = 0;
                        platformCountOnRow++;

                        skip = true;
                    }

                    if (!skip)
                    {
                        builder.CreateGMaterial(currentPosition, new GMaterialTexture(), false);
                        currentPlatformLength++;
                    }

                    space.Add(currentPosition);
                    currentPosition.x++;
                }

                currentPosition.x = offset.x;
                currentPosition.y++;
            }

            builder.Apply();
            return space.ToArray();
        }

        public static void GenerateRandom(int width, int height, Builder builder, int randomCoeficient, Vector2 offset)
        {
            Vector2 currentPosition = offset;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int random = Random.Range(0, 100);

                    if (random <= randomCoeficient)
                        builder.CreateGMaterial(currentPosition, new GMaterialTexture(), false);

                    currentPosition.x++;
                }

                currentPosition.x = offset.x;
                currentPosition.y--;
            }

            builder.Apply();
        }
        
        public static void GenerateRandom(Chunk chunk, int randomCoeficient, Vector2 offset)
        {
            GenerateRandom(chunk.Width, chunk.Height, chunk.Builder, randomCoeficient, offset);
        }

        public static Vector2[] GenerateWithPlatforms(Chunk chunk, int randomCoefficient, Vector2 offset)
        {
            return GenerateWithPlatforms(chunk.Width, chunk.Height, chunk.Builder, randomCoefficient, offset);
        }
    }
}