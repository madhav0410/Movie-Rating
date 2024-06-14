import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../modules/account/service/auth.service';
import { ToastrService } from 'ngx-toastr';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService)
  const router = inject(Router)
 
  if(!authService.isLoggedIn()){
    router.navigate(['/'])
    return false;
  }
  return true;
};
