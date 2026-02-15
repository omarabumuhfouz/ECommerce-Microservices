namespace AuthService.Domain.Users;



[Flags] 
public enum UserRole
{
    None = 0,        
    Customer = 1,   
    Admin = 2        
}
