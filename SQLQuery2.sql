-- Написать SQL запрос, который бы выбирал по каждому активу минимальную и 
--максимальную цену за каждый месяц 2021 года.
select min(ClosePrice), max(ClosePrice) from dbo.tblClosePrice
where YEAR(PriceDate)=2021
group by MONTH(PriceDate), PriceAssetId