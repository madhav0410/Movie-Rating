import { FormControl } from "@angular/forms";

export interface Login {
    email: string | null,
    password: string | null,
}

export interface LoginForm {
    email: FormControl<string | null>,
    password: FormControl<string | null>,
}