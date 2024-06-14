import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule } from '@angular/material/radio';
import { Router, RouterLink } from '@angular/router';
import { Login, LoginForm } from '../../models/login';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../service/auth.service';
import { Role } from '../../../../common/enums/role.';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, FormsModule, CommonModule, ReactiveFormsModule, MatCardModule, MatRadioModule, MatButtonModule, MatIconModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  hide: boolean = true;
  loginForm = new FormGroup<LoginForm>({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    password: new FormControl<string>('', Validators.required)
  });

  constructor(
    private router: Router,
    private authService: AuthService,
    private toastr: ToastrService
  ) { }

  public clickEvent(event: MouseEvent): void {
    this.hide = !this.hide;
    event.stopPropagation();
  }

  public onLogin(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }
    const info: Login = {
      email: this.loginCtrl.email.value,
      password: this.loginCtrl.password.value,
    }

    this.authService.login(info).subscribe({
      next: (res) => {
        this.toastr.success(res.message)
        this.authService.storeToken(res.data)
        const role = this.authService.getRoleFromToken()
        if (role == Role.User) {
          this.router.navigate(['/user/dashboard'])
        }
        else {
          this.router.navigate(['/admin/dashboard'])
        }
      },
      error: (err) => {
        this.toastr.error(err?.error.message)
      }
    })
  }
  
  public get loginCtrl(): LoginForm { return this.loginForm.controls; }
}
