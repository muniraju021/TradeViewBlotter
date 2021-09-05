export class User {
    LoginName: string;
    Password: string;
    EmailId: string;
    RoleName: string;
    UserCode: string;
    IsActive: boolean;
    authdata?: string;
    token: string;
    lastLogin: string;
}

export class UserRole {
    RoleId: string;
    RoleName: string;
}