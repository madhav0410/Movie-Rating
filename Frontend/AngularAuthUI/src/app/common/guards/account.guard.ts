import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../modules/account/service/auth.service';
import { inject } from '@angular/core';

export const accountGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService)
  const router = inject(Router)

  if (authService.isLoggedIn()) {
    const redirectUrl = authService.getDashboardUrl();
    router.navigate([redirectUrl]); 
  }
  return true;
};
