import { FormControl } from "@angular/forms";

export interface Signup {
    firstName:string | null,
    lastName:string | null,
    dateOfBirth: string | null,
    gender: string | null,
    email: string | null,
    password: string | null,
}

export interface SignupForm {
    firstName:FormControl<string | null>,
    lastName:FormControl<string | null>,
    dateOfBirth: FormControl<string | null>,
    gender: FormControl<string | null>,
    email: FormControl<string | null>,
    password: FormControl<string | null>,
}