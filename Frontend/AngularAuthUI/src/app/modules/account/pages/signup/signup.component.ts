import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule, DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule } from '@angular/material/radio';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { provideNativeDateAdapter } from '@angular/material/core';
import { Router, RouterLink } from '@angular/router';
import { Signup, SignupForm } from '../../models/signup';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../service/auth.service';

@Component({
  selector: 'app-signup',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [MatFormFieldModule, MatInputModule, FormsModule, CommonModule, ReactiveFormsModule, MatCardModule, MatRadioModule, MatButtonModule, MatIconModule, MatDatepickerModule, RouterLink],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  
  hide: boolean = true;

  signupForm = new FormGroup<SignupForm>({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    gender: new FormControl('', Validators.required),
    dateOfBirth: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.pattern(/^(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\D*\d).{8,}$/)])
  })

  constructor(
    private router: Router, 
    private authService: AuthService,
    private toastr: ToastrService
  ) { }

  public clickEvent(event: MouseEvent): void {
    this.hide = !this.hide;
    event.stopPropagation();
  }

  public submit(): void {
    if (this.signupForm.invalid) {
      this.signupForm.markAllAsTouched();
      return;
    }
    const user: Signup = {
      firstName: this.signupCtrl.firstName.value,
      lastName: this.signupCtrl.lastName.value,
      dateOfBirth: this.signupCtrl.dateOfBirth.value,
      gender: this.signupCtrl.gender.value,
      email: this.signupCtrl.email.value,
      password: this.signupCtrl.password.value
    };

    this.authService.signUp(user).subscribe({
      next: (res) => {
        this.toastr.success(res.message)
        this.router.navigate(['/account/login'])
      },
      error: (err) => {
        this.toastr.error(err?.error.message)
      }
    });
  }
  
  public get signupCtrl(): SignupForm { return this.signupForm.controls; }
}
