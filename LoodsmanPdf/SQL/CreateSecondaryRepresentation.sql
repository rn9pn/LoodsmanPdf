if object_id ('[dbo].[CreateSecondaryRepresentation]') is not null drop procedure [dbo]. [CreateSecondaryRepresentation]

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
declare @pdffile varbinary(8000)
Select top 1 @id = id from dbo.ExtractObjectsIds(@objects)
set @pdffile =dbo.fnValueOf(@params, 'pdffile', 59)

BEGIN  TRY
	exec dbo.prSaveAltView @id, @pdffile, 'pdf',2,null 
END TRY
BEGIN CATCH
	select ERROR_NUMBER() AS ErrorNumber
          ,ERROR_MESSAGE() AS ErrorMessage
END CATCH	

GO 
 
grant exec on dbo.[CreateSecondaryRepresentation] to LoodsmanUsers
grant exec on dbo.[CreateSecondaryRepresentation] to LoodsmanAdministrators

GO


