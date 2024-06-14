import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../modules/account/service/auth.service';

export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService)
  const router = inject(Router)
  
  const expectedRole = route.data?.['expectedRole'];
  if (!authService.isLoggedIn || authService.getRoleFromToken() !== expectedRole) {
    const redirectUrl = authService.getDashboardUrl();
    router.navigate([redirectUrl]);
    return false;
  }
  return true;
};
