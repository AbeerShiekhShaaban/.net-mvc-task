Create procedure [dbo].[UpdateUserImage]
(  
   @StdId int,  
   @Email nvarchar (50),  
   @Password nvarchar (50),  
   @Image nvarchar (100)  
)   
as  
begin  
   Update users   
   set Image=@Image 
   where Email=@Email 
End