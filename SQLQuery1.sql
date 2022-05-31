
-- Необходимо сделать SQL выборку, соответствующую условиям ниже.
--С целью поиска аномалий необходимо найти цены активов на дату (PriceDate, 
--PriceAssetId, ClosePrice), которые отличаются от цены актива за предыдущую дату, на 
--которую есть цена по этому активу, более чем на 30% в любую сторону.
--Необходимо вывести Дату(DateT), ИД Актива (AssetId), Цену закрытия текущего дня
--(PriceT), цену закрытия предыдущего дня (PriceT1), величину отличия в % (Divergence). В 
--случае, если текущая цена меньше предыдущей, то Divergence < 0, в противном случае 
--Divergence > 0
select distinct tblNew.PriceDate as DateT , tblNew.PriceAssetId as AssetId, tblNew.ClosePrice as PriceT, tblOld.ClosePrice, tblOld.PriceDate
from tblClosePrice tblNew
right join tblClosePrice tblOld on
tblOld.ClosePrice<tblNew.ClosePrice
where tblNew.PriceAssetId=tblOld.PriceAssetId 
--having DATEDIFF(DAY,tblOld.PriceDate,tblNew.PriceDate)=min(DATEDIFF(DAY,tblOld.PriceDate,tblNew.PriceDate))
order by tblNew.PriceAssetId asc, tblNew.PriceDate asc
;


--2) Создать необходимые для выборки из п. 1 индексы (написать скрипт создания 
--индексов)

