namespace The_PackingHub___algorithm
{
    internal class BestFitPacker
    {
        public static Container Pack(Container container, List<Cargo> cargos)
        {
            //Разделение грузов на "особые" и "обычные"
            List<Cargo> specialCargos = cargos.Where(c => c.Signs.Chemically_active || c.Signs.Flammable).ToList();
            List<Cargo> regularCargos = cargos.Where(c => !c.Signs.Chemically_active && !c.Signs.Flammable).ToList();

            // Попытка укладки с учетом особых грузов
            Container containerWithSpecialHandling = PackWithSpecialHandling(container, specialCargos, regularCargos);

            //Если укладка с учетом особых грузов не удалась, используем обычную укладку
            if (containerWithSpecialHandling == null || containerWithSpecialHandling.GetPackedCargos.Count != cargos.Count)
            {
                return PackWithoutSpecialHandling(container, cargos);
            }

            return containerWithSpecialHandling;
        }

        private static Container PackWithSpecialHandling(Container container, List<Cargo> specialCargos, List<Cargo> regularCargos)
        {
            Container tempContainer = new Container(container.Length, container.Width, container.Height, container.WallThickness, container.Weight);
            List<Cargo> packedCargos = new List<Cargo>();

            //Укладка "особых" грузов с разделением "обычными"
            foreach (var specialCargo in specialCargos)
            {
                bool placed = false;
                foreach (var (length, width, height) in specialCargo.GetOrientations())
                {
                    var orientedCargo = new Cargo(length, width, height, specialCargo.Weight, specialCargo.Signs) { Name = specialCargo.Name };

                    Vector3 bestPosition = FindBestPositionWithSpecialHandling(tempContainer, orientedCargo, packedCargos);
                    if (bestPosition != null)
                    {
                        tempContainer.AddCargo(orientedCargo, bestPosition);
                        packedCargos.Add(orientedCargo);
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    //Если не удалось разместить "особый" груз, возвращаем null
                    return null;
                }

                //Попытка разместить "обычный" груз после "особого"
                if (regularCargos.Count > 0)
                {
                    var regularCargo = regularCargos[0];
                    placed = false;
                    foreach (var (length, width, height) in regularCargo.GetOrientations())
                    {
                        var orientedCargo = new Cargo(length, width, height, regularCargo.Weight, regularCargo.Signs) { Name = regularCargo.Name };

                        Vector3 bestPosition = FindBestPosition(tempContainer, orientedCargo);
                        if (bestPosition != null)
                        {
                            tempContainer.AddCargo(orientedCargo, bestPosition);
                            packedCargos.Add(orientedCargo);
                            regularCargos.RemoveAt(0);
                            placed = true;
                            break;
                        }
                    }
                    //Если не удалось разместить "обычный" груз, продолжаем без него
                }
            }

            //Укладка оставшихся "обычных" грузов
            foreach (var regularCargo in regularCargos)
            {
                bool placed = false;
                foreach (var (length, width, height) in regularCargo.GetOrientations())
                {
                    var orientedCargo = new Cargo(length, width, height, regularCargo.Weight, regularCargo.Signs) { Name = regularCargo.Name };

                    Vector3 bestPosition = FindBestPosition(tempContainer, orientedCargo);
                    if (bestPosition != null)
                    {
                        tempContainer.AddCargo(orientedCargo, bestPosition);
                        packedCargos.Add(orientedCargo);
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    //Если не удалось разместить "обычный" груз, возвращаем null
                    return null;
                }
            }

            return tempContainer;
        }

        private static Container PackWithoutSpecialHandling(Container container, List<Cargo> cargos)
        {
            var sortedCargos = SortCargos(cargos);

            foreach (var cargo in sortedCargos)
            {
                bool placed = false;
                foreach (var (length, width, height) in cargo.GetOrientations())
                {
                    var orientedCargo = new Cargo(length, width, height, cargo.Weight, cargo.Signs) { Name = cargo.Name };

                    Vector3 bestPosition = FindBestPosition(container, orientedCargo);
                    if (bestPosition != null)
                    {
                        container.AddCargo(orientedCargo, bestPosition);
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    Console.WriteLine($"Cargo {cargo.Length}x{cargo.Width}x{cargo.Height} could not be placed.");
                }
            }

            return container;
        }

        private static Vector3 FindBestPositionWithSpecialHandling(Container container, Cargo cargo, List<Cargo> packedCargos)
        {
            Vector3 bestPosition = null;
            float minRemainingVolume = float.MaxValue;
            float step = 0.01f;

            for (float z = 0; z < container.InnerHeight; z = MathF.Round(z + step, 2))
            {
                for (float y = 0; y < container.InnerWidth; y = MathF.Round(y + step, 2))
                {
                    for (float x = 0; x < container.InnerLength; x = MathF.Round(x + step, 2))
                    {
                        Vector3 position = new Vector3(x, y, z);
                        if (container.AddCargo(cargo, position))
                        {
                            //Проверка на соседство с "особыми" грузами
                            bool hasSpecialNeighbor = false;
                            foreach (var packed in packedCargos)
                            {
                                if ((packed.Signs.Chemically_active || packed.Signs.Flammable) &&
                                    container.BoxesOverlap(cargo, position, packed, FindPosition(container, packed)))
                                {
                                    hasSpecialNeighbor = true;
                                    break;
                                }
                            }

                            if (!hasSpecialNeighbor)
                            {
                                float remainingVolume = CalculateRemainingVolume(container);
                                container.GetPackedCargos.RemoveAt(container.GetPackedCargos.Count - 1);

                                if (remainingVolume < minRemainingVolume)
                                {
                                    minRemainingVolume = remainingVolume;
                                    bestPosition = position;
                                }
                            }
                            else
                            {
                                container.GetPackedCargos.RemoveAt(container.GetPackedCargos.Count - 1);
                            }
                        }
                    }
                }
            }

            return bestPosition;
        }

        private static Vector3 FindPosition(Container container, Cargo cargo)
        {
            foreach (var packedCargo in container.GetPackedCargos)
            {
                if (packedCargo.Item1 == cargo)
                {
                    return packedCargo.Item2;
                }
            }
            return null;
        }

        private static List<Cargo> SortCargos(List<Cargo> cargos)
        {
            return cargos
                .OrderByDescending(c => c.Length * c.Width * c.Height) //Сначала объёмные
                .ThenByDescending(c => c.Weight) //Затем тяжёлые
                .ThenByDescending(c => !c.Signs.Fragile) //Затем хрупкие (false идет раньше true)
                .ToList();
        }

        private static Vector3 FindBestPosition(Container container, Cargo cargo)
        {
            Vector3 bestPosition = null;
            float minRemainingVolume = float.MaxValue;
            float step = 0.01f;

            for (float z = 0; z < container.InnerHeight; z = MathF.Round(z + step, 2))
            {
                for (float y = 0; y < container.InnerWidth; y = MathF.Round(y + step, 2))
                {
                    for (float x = 0; x < container.InnerLength; x = MathF.Round(x + step, 2))
                    {
                        Vector3 position = new Vector3(x, y, z);
                        if (container.AddCargo(cargo, position))
                        {
                            float remainingVolume = CalculateRemainingVolume(container);
                            container.GetPackedCargos.RemoveAt(container.GetPackedCargos.Count - 1);

                            if (remainingVolume < minRemainingVolume)
                            {
                                minRemainingVolume = remainingVolume;
                                bestPosition = position;
                            }
                        }
                    }
                }
            }

            return bestPosition;
        }

        private static float CalculateRemainingVolume(Container container)
        {
            float usedVolume = 0;
            foreach (var packedCargo in container.GetPackedCargos)
            {
                usedVolume += packedCargo.Item1.Length * packedCargo.Item1.Width * packedCargo.Item1.Height;
            }
            return container.InnerLength * container.InnerWidth * container.InnerHeight - usedVolume;
        }
    }
}