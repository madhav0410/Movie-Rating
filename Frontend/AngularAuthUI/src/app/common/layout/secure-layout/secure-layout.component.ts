import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-secure-layout',
  standalone: true,
  imports: [RouterModule, NavbarComponent],
  templateUrl: './secure-layout.component.html',
  styleUrl: './secure-layout.component.css'
})
export class SecureLayoutComponent {

}
