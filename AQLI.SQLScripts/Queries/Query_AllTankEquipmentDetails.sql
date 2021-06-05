DECLARE @TankID INT

SET @TankID = 1

SELECT EquipmentTypeCategory.PurchaseCategoryTypeName, 
	   EquipmentType.EquipmentTypeName,
	   TankPurchaseDetails.Description,	   
       PurchaseLocation.StoreName AS [PurchaseLocation],
	   TankPurchaseDetails.Date AS [PurchaseDate],
       EquipmentCategory.CategoryName,
       TankPurchaseDetails.Cost,
       TankPurchaseDetails.OwnerID,       
       TankPurchaseDetails.InvoiceID FROM dbo.Tank_Equipment_J AS jTankEquipment
JOIN dbo.Purchases AS TankPurchaseDetails
ON TankPurchaseDetails.EquipmentID = jTankEquipment.EquipmentID
JOIN dbo.TankEquipmentTypes AS EquipmentType
ON jTankEquipment.EquipmentID = EquipmentType.TankEquipmentID
JOIN dbo.Stores AS PurchaseLocation
ON PurchaseLocation.StoreID = TankPurchaseDetails.StoreID
JOIN dbo.PurchaseCategory AS EquipmentCategory
ON EquipmentCategory.PurchaseCategoryID = TankPurchaseDetails.PurchaseCategoryID
JOIN dbo.PurchaseCategoryTypes AS EquipmentTypeCategory
ON EquipmentTypeCategory.PurchaseCategoryTypeID = EquipmentCategory.PurchaseCategoryTypeID

WHERE jTankEquipment.TankID = @TankID