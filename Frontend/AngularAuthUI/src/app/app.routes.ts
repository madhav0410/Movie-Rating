import { Routes } from '@angular/router';
import { authGuard } from './common/guards/auth.guard';
import { roleGuard } from './common/guards/role.guard';
import { accountGuard } from './common/guards/account.guard';
import { SecureLayoutComponent } from './common/layout/secure-layout/secure-layout.component';
import { PublicLayoutComponent } from './common/layout/public-layout/public-layout.component';


export const routes: Routes = [
  {
    path: '', redirectTo: 'user', pathMatch: 'full'
  },
  {
    path: '',
    component: PublicLayoutComponent,
    children: [
      {
        path: 'account',
        loadChildren: () => import('./modules/account/account.routes').then((m) => m.accountRoute),
        canActivate: [accountGuard]
      },
    ]
  },
  {
    path: '',
    component: SecureLayoutComponent,
    children: [
      {
        path: 'user',
        loadChildren: () => import('./modules/user/user.routes').then((m) => m.userRoute),
        // canActivate: [authGuard, roleGuard],
        // data: { expectedRole: '2' }
      },
      {
        path: 'admin',
        loadChildren: () => import('./modules/admin/admin.routes').then((m) => m.adminRoute),
        canActivate: [authGuard, roleGuard],
        data: { expectedRole: '1' }
      },
    ]
  },
  {
    path: '**', redirectTo: 'user', pathMatch: 'full'
  }
];
