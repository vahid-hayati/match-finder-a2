export interface AppUser {
    email: string;
    userName: string;
    dateOfBirth: string | undefined;
    password: string;
    confirmPassword: string;
    gender: string;
    city: string;
    country: string;
}