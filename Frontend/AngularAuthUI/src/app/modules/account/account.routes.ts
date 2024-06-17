import { Routes } from "@angular/router";

export const accountRoute: Routes = [
    {
        path: '', redirectTo: 'login', pathMatch: 'full'
    },
    {
        path: 'login', loadComponent: () => import('./pages/login/login.component').then(x => x.LoginComponent)
    },
    {
        path: 'sign-up', loadComponent: () => import('./pages/signup/signup.component').then(x => x.SignupComponent)
    },
    {
        path: 'forgot-password', loadComponent: () => import('./pages/forgotpassword/forgotpassword.component').then(x => x.ForgotpasswordComponent)
    },
    {
        path: 'reset-password', loadComponent: () => import('./pages/resetpassword/resetpassword.component').then(x => x.ResetpasswordComponent)
    },
    {
        path: '**', redirectTo: 'login', pathMatch: 'full'
    }
];