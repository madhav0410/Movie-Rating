import { Component } from '@angular/core';
import { AuthService } from '../../../modules/account/service/auth.service';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AddmovieComponent } from '../../../modules/admin/pages/addmovie/addmovie.component';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MatButtonModule,CommonModule,RouterLink,],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  constructor(
    private authService: AuthService,
    public dialog: MatDialog
  ) {}

  public get isAuthenticated(): boolean {
    return this.authService.isLoggedIn();
  }

  public get role(): string | null {
    return this.authService.getRoleFromToken();
  }

  public openAddMovieDialog(): void {
    this.dialog.open(AddmovieComponent)
  }

  public logout(): void {
    this.authService.signOut();
  }
}
