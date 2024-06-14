import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MovieService } from '../../../../common/service/movie.service';
import { NavbarComponent } from '../../../../common/layout/navbar/navbar.component';
import { MoviecardComponent } from '../../../../common/components/moviecard/moviecard.component';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from '../../../../common/components/pagination/pagination.component';
import { PaginatedMovies } from '../../../../common/models/movie';
import { MatFormField } from '@angular/material/form-field';
import {MatSelectModule} from '@angular/material/select';
import { FormsModule } from '@angular/forms';



@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatButtonModule, NavbarComponent, MoviecardComponent, CommonModule,PaginationComponent,MatFormField,MatSelectModule,FormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  page: number = 1;
  pageSize: number = 12;
  totalPages!: number;
  selectedGenres: string[] = []
  allGenre: string[] = ['Action', 'Thriller','Crime', 'Comedy', 'Suspense', 'Biography', 'Horror', 'Drama', 'Musical', 'Mystery', 'Sci-fi', 'Fantasy', 'Romance', 'Adventure'];

  constructor(public movieService: MovieService) { }

  ngOnInit(): void {
    this.getMovies()
  }
  
  public onPageChanged(currentPage: number): void{
    this.page = currentPage;
    this.getMovies()
  }
   
  public filterMovies(): void{
    this.page = 1
    this.getMovies()
  }

  private getMovies(): void{ 
    this.movieService.getMovies(this.page, this.pageSize, this.selectedGenres).subscribe({
      next: (res) => {
        this.movieService.movieList.next(res.data.paginatedList);
        this.totalPages = res.data.totalPages;
      },
      error: (err) => {
        console.log(err?.error.message);
      }
    })
  }
}
