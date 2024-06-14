import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { Movie } from '../../models/movie';
import { RouterLink } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { AdminService } from '../../../modules/admin/service/admin.service';
import { MovieService } from '../../service/movie.service';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { AddmovieComponent } from '../../../modules/admin/pages/addmovie/addmovie.component';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from '../../../modules/account/service/auth.service';

@Component({
  selector: 'app-moviecard',
  standalone: true,
  imports: [MatCardModule,RouterLink,MatIcon,MatButtonModule,MatButton,],
  templateUrl: './moviecard.component.html',
  styleUrl: './moviecard.component.css'
})
export class MoviecardComponent {

  page: number = 1;
  limit: number = 12;
  selectedGenres: string[] = []

  @Input() movie!: Movie 
  hover!:boolean
  routerLink = this.role === "1" ? "/admin/movie" : "/user/movie"
  
  constructor(
    private authService: AuthService, 
    private adminService: AdminService,
    private movieService: MovieService,
    public dialog: MatDialog
  ){} 

  public get isAuthenticated(): boolean {
    return this.authService.isLoggedIn();
  }

  public get role(): string | null {
    return this.authService.getRoleFromToken();
  }
  
  public editMovie(record: Movie): void{
    this.dialog.open(AddmovieComponent,{
      data: record
    })
  }

  public deleteMovie(movie: Movie): void{
    if(movie){
      this.adminService.deleteMovie(movie.id).subscribe({
        next: ()=>{
          this.movieService.getMovies(this.page,this.limit,this.selectedGenres).subscribe(res => {
            if(res && res.data){
              this.movieService.movieList.next(res.data.paginatedList)
            }
          });
        },
        error: (err)=>{
          console.log(err?.error.message); 
        }
      })
    }
  }

  
}
