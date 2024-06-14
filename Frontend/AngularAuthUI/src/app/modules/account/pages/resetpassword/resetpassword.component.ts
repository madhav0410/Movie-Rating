import { Component} from '@angular/core';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule } from '@angular/material/radio';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../service/auth.service';
import { ResetPassword, ResetPasswordForm } from '../../models/resetPassword';


@Component({
  selector: 'app-resetpassword',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, FormsModule, CommonModule, ReactiveFormsModule, MatCardModule, MatRadioModule, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './resetpassword.component.html',
  styleUrl: './resetpassword.component.css'
})

export class ResetpasswordComponent {

  hide: boolean = true;
  time: string = this.route.snapshot.queryParamMap.get('time') ?? '';
  email: string = this.route.snapshot.queryParamMap.get('email') ?? '';

  resetPasswordForm = new FormGroup<ResetPasswordForm>({
    password: new FormControl('', [Validators.required, Validators.pattern(/^(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\D*\d).{8,}$/)]),
    confirmPassword: new FormControl('', [Validators.required, this.passwordMatchValidator])
  })

  constructor(
    private router: Router, 
    private authService: AuthService, 
    private toastr: ToastrService, 
    private route: ActivatedRoute
  ) { }

  public clickEvent(event: MouseEvent): void {
    this.hide = !this.hide;
    event.stopPropagation();
  }

  public onReset(): void {
    if (this.resetPasswordForm.invalid) {
      this.resetPasswordForm.markAllAsTouched();
      return;
    }
    const info: ResetPassword = {
      password: this.resetPasswordCtrl.password.value,
      confirmPassword: this.resetPasswordCtrl.confirmPassword.value,
    }

    if (this.email && this.time) {
      this.authService.resetPassword(this.email, this.time, info).subscribe({
        next: (res) => {
          this.toastr.success(res.message);
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.toastr.error(err?.error.message)
        }
      });
    }
  }

  private passwordMatchValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    return password && confirmPassword && password.value === confirmPassword.value ? null : { 'mismatch': true };
  }

  public get resetPasswordCtrl(): ResetPasswordForm { return this.resetPasswordForm.controls; }
}

