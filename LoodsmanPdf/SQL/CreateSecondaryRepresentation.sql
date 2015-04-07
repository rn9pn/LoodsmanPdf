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

declare @id int
declare @pdffileonchar varchar(8000)
declare @pdffileonbinary varbinary(8000)

select top 1 @id = id from dbo.ExtractObjectsIds(@objects)

set @pdffileonchar = dbo.fnValueOf(@params, 'pdffile', 59)

set @pdffileonbinary = CONVERT (VARBINARY(8000), @pdffileonchar, 1)

BEGIN  TRY
	exec dbo.prSaveAltView @id, @pdffileonbinary, 'pdf', 2, null 
END TRY
BEGIN CATCH
	select ERROR_NUMBER() AS ErrorNumber
          ,ERROR_MESSAGE() AS ErrorMessage
END CATCH	
GO 
 
grant exec on dbo.[CreateSecondaryRepresentation] to LoodsmanUsers
grant exec on dbo.[CreateSecondaryRepresentation] to LoodsmanAdministrators
GO


