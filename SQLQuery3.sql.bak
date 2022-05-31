--1) Выбрать всех клиентов (ClientId, Name) и посчитать по каждому общую сумму 
--платежей (TotalSum).
SELECT tblClient.[ClientId], Name, ISNULL(SUM(PaymentSum),0) as TotalSum
FROM tblClient Left JOIN tblClientPayments
ON tblClient.ClientId = tblClientPayments.ClientId
group by tblClient.[ClientId], Name
;
--2) Выбрать всех клиентов (ClientId, Name, TotalSum), у которых либо есть платежи 
--после 05.03.2022, либо сумма всех платежей превышает 7000.
SELECT tblClient.[ClientId], Name, SUM(PaymentSum) as TotalSum
FROM tblClient Left JOIN tblClientPayments
ON tblClient.ClientId = tblClientPayments.ClientId
WHERE exists(
select PaymentSum
from tblClientPayments
where DATEDIFF(day, '2022-03-05', PaymentDate)>0 
and tblClient.[ClientId]=ClientId
) 
group by tblClient.[ClientId], Name
having SUM(PaymentSum)>7000
;


--3) Выбрать постоянных клиентов (ClientId, Name), то есть клиентов, у которых есть 
--платежи по крайней мере в двух разных календарных месяцах одного года.
SELECT tblClient.[ClientId], Name
FROM tblClient LEFT JOIN tblClientPayments
ON tblClient.ClientId = tblClientPayments.ClientId

group by tblClient.[ClientId], Name

--4) Измените платёж Кузнецовой Анны Андреевны от 02.03.2022, применив к нему 
--скидку 10% (Уменьшите на 10%). Напишите SQL запрос.


--5) Добавьте платёж Петрова Петра Петровича за текущую дату на сумму 18000.
