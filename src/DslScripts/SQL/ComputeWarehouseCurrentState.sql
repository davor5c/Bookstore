SELECT
    ID = NEWID(),
    WarehouseItemID,
    WarehouseID,
    Quantity = SUM(Quantity)
FROM
    Warehouse1.WarehouseEvent
GROUP BY
    WarehouseItemID,
    WarehouseID
