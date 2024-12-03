namespace The_PackingHub___algorithm
{
    public static class BestFitPacker
    {
        public static float Step
        {
            get => step;
            set
            {
                if (value < 0f) throw new ArgumentOutOfRangeException("Значение step отрицательное!");
                step = value;
            }
        }
        private static float step = -1;

        public static Container Pack(Container container, List<Cargo> cargos, Route route)
        {
            // Проверка на соответствие адресов
            ValidateAddresses(cargos, route);

            // Сортируем грузы по маршруту доставки
            var sortedCargos = SortCargos(cargos, route);

            // Укладываем с учетом манипуляционных признаков
            Container packedContainer = PackWithSpecialHandling(container, sortedCargos);

            if (packedContainer == null || packedContainer.GetPackedCargos.Count != cargos.Count)
            {
                // Если не удалось, пробуем без учета специальных правил
                return PackWithoutSpecialHandling(container, sortedCargos);
            }

            return packedContainer;
        }

        private static void ValidateAddresses(List<Cargo> cargos, Route route)
        {
            var routeAddresses = route.Addresses.ToHashSet();
            var cargoAddresses = cargos.Select(c => c.DeliveryAddress).ToHashSet();

            // Проверяем, что все адреса грузов есть в маршруте
            foreach (var cargoAddress in cargoAddresses)
            {
                if (!routeAddresses.Contains(cargoAddress))
                {
                    throw new InvalidOperationException($"Адрес {cargoAddress.Name} из грузов отсутствует в маршруте!");
                }
            }

            // Проверяем, есть ли адреса в маршруте, по которым ничего не нужно доставлять
            foreach (var routeAddress in routeAddresses)
            {
                if (!cargoAddresses.Contains(routeAddress))
                {
                    Console.WriteLine($"Предупреждение: В маршруте указан адрес {routeAddress.Name}, по которому нет доставки грузов.");
                }
            }
        }

        private static Container PackWithSpecialHandling(Container container, List<Cargo> cargos)
        {
            Container tempContainer = new Container(container.Length, container.Width, container.Height, container.WallThickness, container.Weight);
            List<Cargo> packedCargos = new List<Cargo>();

            foreach (var cargo in cargos)
            {
                bool placed = false;

                foreach (var (length, width, height) in cargo.GetOrientations())
                {
                    var orientedCargo = new Cargo(length, width, height, cargo.Weight, cargo.Signs, cargo.DeliveryAddress) { Name = cargo.Name };

                    Vector3 bestPosition = cargo.Signs.Chemically_active || cargo.Signs.Flammable
                        ? FindBestPositionWithSpecialHandling(tempContainer, orientedCargo, packedCargos)
                        : FindBestPosition(tempContainer, orientedCargo);

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
                    return null;
                }
            }

            return tempContainer;
        }

        private static Container PackWithoutSpecialHandling(Container container, List<Cargo> cargos)
        {
            foreach (var cargo in cargos)
            {
                bool placed = false;

                foreach (var (length, width, height) in cargo.GetOrientations())
                {
                    var orientedCargo = new Cargo(length, width, height, cargo.Weight, cargo.Signs, cargo.DeliveryAddress) { Name = cargo.Name };

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
                    return null;
                }
            }

            return container;
        }

        public static Vector3 FindBestPositionWithSpecialHandling(Container container, Cargo cargo, List<Cargo> packedCargos)
        {
            if (step == -1) throw new InvalidOperationException("Переменной step не назначено значение!");
            Vector3 bestPosition = null;
            float minRemainingVolume = float.MaxValue;

            for (float z = 0; z < container.InnerHeight; z += step)
            {
                for (float y = 0; y < container.InnerWidth; y += step)
                {
                    for (float x = 0; x < container.InnerLength; x += step)
                    {
                        Vector3 position = new Vector3(x, y, z);

                        if (container.AddCargo(cargo, position))
                        {
                            bool hasSpecialNeighbor = packedCargos.Any(p =>
                                (p.Signs.Chemically_active || p.Signs.Flammable) &&
                                container.BoxesOverlap(cargo, position, p, FindPosition(container, p)));

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

        public static Vector3 FindBestPosition(Container container, Cargo cargo)
        {
            if (step == -1) throw new InvalidOperationException("Переменной step не назначено значение!");
            Vector3 bestPosition = null;
            float minRemainingVolume = float.MaxValue;

            for (float z = 0; z < container.InnerHeight; z += step)
            {
                for (float y = 0; y < container.InnerWidth; y += step)
                {
                    for (float x = 0; x < container.InnerLength; x += step)
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

        public static List<Cargo> SortCargos(List<Cargo> cargos, Route route)
        {
            var addressOrder = route.Addresses
                .Select((address, index) => new { address, index })
                .ToDictionary(a => a.address, a => a.index);

            return cargos
                .OrderBy(c => addressOrder.ContainsKey(c.DeliveryAddress) ? addressOrder[c.DeliveryAddress] : int.MaxValue)
                .ThenByDescending(c => c.Length * c.Width * c.Height)
                .ThenByDescending(c => c.Weight)
                .ThenByDescending(c => !c.Signs.Fragile)
                .ToList();
        }

        private static Vector3 FindPosition(Container container, Cargo cargo)
        {
            return container.GetPackedCargos.FirstOrDefault(p => p.Item1 == cargo)?.Item2;
        }

        private static float CalculateRemainingVolume(Container container)
        {
            float usedVolume = container.GetPackedCargos.Sum(p => p.Item1.Length * p.Item1.Width * p.Item1.Height);
            return container.InnerLength * container.InnerWidth * container.InnerHeight - usedVolume;
        }
    }
}
