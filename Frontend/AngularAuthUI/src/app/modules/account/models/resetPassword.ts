import { FormControl } from "@angular/forms"

export interface ResetPassword{
    password: string | null,
    confirmPassword: string | null,
}

export interface ResetPasswordForm {
    password: FormControl<string | null>,
    confirmPassword: FormControl<string | null>
}
  