import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../service/auth.service';

type ForgotPasswordForm = {
  email: FormControl<string | null>
}

@Component({
  selector: 'app-forgotpassword',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, FormsModule, CommonModule, ReactiveFormsModule, MatCardModule, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './forgotpassword.component.html',
  styleUrl: './forgotpassword.component.css',
})

export class ForgotpasswordComponent {

  forgotPasswordForm = new FormGroup<ForgotPasswordForm>({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
  })

  constructor(
    private router: Router, 
    private authService: AuthService, 
    private toastr: ToastrService
  ) { }

  public sendLink(): void {
    if (this.forgotPasswordForm.invalid) {
      this.forgotPasswordForm.markAllAsTouched();
      return;
    }

    const email = this.forgotPasswordCtrl.email.value;
    if (email) {
      this.authService.forgotPassword(email).subscribe({
        next: (res) => {
          this.toastr.success("Reset password link is sent");
          this.router.navigate(['/account/login']);
        },
        error: (err) => {
          this.toastr.error(err?.error.message);
        }
      });
    }
  }

  public get forgotPasswordCtrl(): ForgotPasswordForm { return this.forgotPasswordForm.controls; }

  
}
