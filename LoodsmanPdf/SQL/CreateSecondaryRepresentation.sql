if object_id ('[dbo].[CreateSecondaryRepresentation]') is not null drop procedure [dbo].[CreateSecondaryRepresentation]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  PROCEDURE [dbo].[CreateSecondaryRepresentation]
  (
    @Objects varchar(8000),
    @Params  text = null
  )
 AS

declare @prm varchar(max)
declare @hexstring varchar(max)
declare @id int
declare @cs bigint

set @prm = Convert(varchar(max),@params, 1)


set @cs = cast(right(left(@prm,charindex(';', @prm)-1), len(left(@prm,charindex(';', @prm)-1))-4) as bigint)
set @prm = right(@prm, len(@prm)- charindex(';', @prm))
set @hexstring =right(@prm,len(@prm)- charindex('=', @prm) )

Select top 1 @id = id from dbo.ExtractObjectsIds(@objects)


 DECLARE @hex CHAR(2), @i INT, @count INT, @b varbinary(max), @odd BIT, @start bit
   SET @count = LEN(@hexstring)
   SET @start = 1
   SET @b = CAST('' AS varbinary(1))
   IF SUBSTRING(@hexstring, 1, 2) = '0x'
       SET @i = 3
   ELSE
       SET @i = 1
   SET @odd = CAST(LEN(SUBSTRING(@hexstring, @i, LEN(@hexstring))) % 2 AS BIT)
   WHILE (@i <= @count)
    BEGIN
       IF @start = 1 AND @odd = 1
       BEGIN
           SET @hex = '0' + SUBSTRING(@hexstring, @i, 1)
       END
       ELSE
       BEGIN
           SET @hex = SUBSTRING(@hexstring, @i, 2)
       END
       SET @b = @b +
               CAST(CASE WHEN SUBSTRING(@hex, 1, 1) LIKE '[0-9]'
                   THEN CAST(SUBSTRING(@hex, 1, 1) AS INT)
                   ELSE CAST(ASCII(UPPER(SUBSTRING(@hex, 1, 1)))-55 AS INT)
               END * 16 +
               CASE WHEN SUBSTRING(@hex, 2, 1) LIKE '[0-9]'
                   THEN CAST(SUBSTRING(@hex, 2, 1) AS INT)
                   ELSE CAST(ASCII(UPPER(SUBSTRING(@hex, 2, 1)))-55 AS INT)
               END AS binary(1))
       SET @i = @i + (2 - (CAST(@start AS INT) * CAST(@odd AS INT)))
       IF @start = 1
       BEGIN
           SET @start = 0
       END
    END


BEGIN  TRY
	 exec dbo.prSaveAltView @obj_id = @id, @crc = @cs,@content=@b , @stExt = 'pdf', @access = 2,@lock_id = null 
	 select 'no error'	
END TRY
BEGIN CATCH
	select ERROR_NUMBER() AS ErrorNumber
          ,ERROR_MESSAGE() AS ErrorMessage
END CATCH	

go
 
grant exec on dbo.[CreateSecondaryRepresentation] to LoodsmanUsers
grant exec on dbo.[CreateSecondaryRepresentation] to LoodsmanAdministrators

GO


