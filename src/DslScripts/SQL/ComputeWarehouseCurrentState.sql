SELECT WarehouseID, WarehouseItemID, SUM(Quantity) Quantity
FROM Warehouse1.WarehouseEvent
GROUP BY WarehouseID, WarehouseItemID;