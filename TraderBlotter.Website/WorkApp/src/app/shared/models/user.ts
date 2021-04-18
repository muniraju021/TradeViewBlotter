export class User {
    LoginName: string;
    Password: string;
    EmailId: string;
    RoleName: string;
    UserCode: string;
    IsActive: boolean;
    authdata?: string;
}

export class UserRole {
    RoleId: string;
    RoleName: string;
}