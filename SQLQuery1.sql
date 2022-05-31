
-- Необходимо сделать SQL выборку, соответствующую условиям ниже.
--С целью поиска аномалий необходимо найти цены активов на дату (PriceDate, 
--PriceAssetId, ClosePrice), которые отличаются от цены актива за предыдущую дату, на 
--которую есть цена по этому активу, более чем на 30% в любую сторону.
--Необходимо вывести Дату(DateT), ИД Актива (AssetId), Цену закрытия текущего дня
--(PriceT), цену закрытия предыдущего дня (PriceT1), величину отличия в % (Divergence). В 
--случае, если текущая цена меньше предыдущей, то Divergence < 0, в противном случае 
--Divergence > 0
--select PriceDate as DateT , PriceAssetId as AssetId, ClosePrice as PriceT, 
--(select PriceDate from tblClosePrice as tblClose 
--where DATEDIFF(day,tblClose.PriceDate,tbl1.PriceDate) = (-1)
--and tbl1.PriceAssetId=tblClose.PriceAssetId
--)
----, as PriceT1, as Divergence
--from tblClosePrice as tbl1

----where 
--order by PriceAssetId asc, PriceDate asc

--;

select tbl1.PriceDate as DateT, tbl1.PriceAssetId as AssetId, tbl1.ClosePrice as PriceT, tbl2.PriceDate,
tbl2.ClosePrice as PriceT1, CAST((tbl1.ClosePrice-tbl2.ClosePrice)/tbl2.ClosePrice*100 AS INT) as Divergence
from tblClosePrice as tbl1
join tblClosePrice tbl2
on (tbl1.PriceAssetId=tbl2.PriceAssetId)
where tbl1.PriceAssetId=tbl2.PriceAssetId
and DATEDIFF(day,tbl2.PriceDate,tbl1.PriceDate) = 1
and (tbl2.ClosePrice/tbl1.ClosePrice>1.3 or tbl1.ClosePrice/tbl2.ClosePrice>1.3)
;
--2) Создать необходимые для выборки из п. 1 индексы (написать скрипт создания 
--индексов)


CREATE INDEX [IX_tblClosePrice_PriceAssetId_PriceDate] ON [dbo].[tblClosePrice] ( PriceAssetId,PriceDate )