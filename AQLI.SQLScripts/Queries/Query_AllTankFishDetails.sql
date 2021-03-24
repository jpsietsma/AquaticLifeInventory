USE AquaticLife

DECLARE @TankID INT

SET @TankID = 1

SELECT userCreatureType.CreatureTypeName AS [CreatureType],
	   userFishDetails.FriendlyName AS [FriendlyName],	   
       fishTypesVw.TypeName AS [Species],
	   fishTypesVw.FishFamilyName AS [Family],
	   fishTypesVw.TempormentName AS [Temporment],
       fishTypesVw.TerritorialLevel AS [Territorial],
       fishTypesVw.FeedingType AS [Feeding],
       fishTypesVw.BirthingType AS [Birthing],
       fishTypesVw.WaterTypeName AS [WaterType],
	   fishTypesVw.TankPopulationEncumbrance * userFishDetails.EncumbranceMultiplier AS [Encumbrance],
       fishTypesVw.FishTypeImagePath AS [ImagePath],       
       purchaseDetails.Date AS [PurchaseDate],
       purchaseLocationDetails.StoreName AS [Location],
       purchaseDetails.Cost,
       CONCAT(ownerDetails.FirstName, ' ', ownerDetails.LastName) AS [Owner],
       purchaseDetails.InvoiceFilePath FROM dbo.Tank_UserFish_J AS jAllFish
JOIN dbo.UserFish AS userFishDetails
ON userFishDetails.UserFishID = jAllFish.UserFishID
JOIN dbo.CreatureType AS userCreatureType
ON userCreatureType.CreatureTypeID = userFishDetails.CreatureTypeID
JOIN dbo.FishType_View AS fishTypesVw
ON fishTypesVw.CreatureTypeID = userFishDetails.FishTypeID
JOIN dbo.Purchases AS purchaseDetails 
ON userFishDetails.PurchaseID = purchaseDetails.PurchaseID
JOIN dbo.AspNetUsers AS userDetails
ON userDetails.UserId = jAllFish.AddedBy
JOIN dbo.Stores AS purchaseLocationDetails
ON purchaseLocationDetails.StoreID = purchaseDetails.StoreID
JOIN dbo.AspNetUsers AS ownerDetails
ON ownerDetails.UserId = purchaseDetails.OwnerID

WHERE jAllFish.TankID = @TankID

