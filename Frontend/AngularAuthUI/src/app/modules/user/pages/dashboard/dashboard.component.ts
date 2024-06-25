import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MovieService } from '../../../../common/service/movie.service';
import { NavbarComponent } from '../../../../common/layout/navbar/navbar.component';
import { MoviecardComponent } from '../../../../common/components/moviecard/moviecard.component';
import { CommonModule } from '@angular/common';
import { PaginationComponent } from '../../../../common/components/pagination/pagination.component';
import { MatFormField, MatFormFieldControl } from '@angular/material/form-field';
import {MatSelectModule} from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';



@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatButtonModule, NavbarComponent, MoviecardComponent, CommonModule,PaginationComponent,MatFormField,MatSelectModule,FormsModule,MatIconModule,MatInputModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  page: number = 1;
  pageSize: number = 12;
  totalPages!: number;
  selectedGenres: string[] = []
  searchQuery: string = '';
  allGenre: string[] = ['Action', 'Thriller','Crime', 'Comedy', 'Suspense', 'Biography', 'Horror', 'Drama', 'Musical', 'Mystery', 'Sci-fi', 'Fantasy', 'Romance', 'Adventure'];
  private searchSubject: Subject<string> = new Subject();
  
  constructor(public movieService: MovieService) { 
    this.searchSubject.pipe(
      debounceTime(300), // Waits for 300ms pause in events
      distinctUntilChanged() // Only emit if the current value is different than the last
    ).subscribe(searchQuery => {
      this.searchQuery = searchQuery;
      this.searchMovie();
    });
  }

  ngOnInit(): void {
    this.getMovies(this.movieService.currentPage.getValue())
  }
  
  public onPageChanged(currentPage: number): void{
    this.movieService.currentPage.next(currentPage);
    this.getMovies(this.movieService.currentPage.getValue())
  }
  
  public filterMovies(): void{
    if(this.selectedGenres.length > 0){
      this.page = 1
      this.getMovies(this.page)
    }else{
      this.getMovies(this.movieService.currentPage.getValue())
    }
  }
  
  public onKeyup(event: KeyboardEvent): void {
    const target = event.target as HTMLInputElement;
    this.searchSubject.next(target.value);
  }

  public searchMovie(): void{
    if(this.searchQuery !== ''){
      this.page = 1
      this.getMovies(this.page)
    }else{
      this.getMovies(this.movieService.currentPage.getValue())
    }
  }
  
  private getMovies(currentPage: number): void{ 
    this.movieService.getMovies(currentPage, this.pageSize, this.selectedGenres,this.searchQuery).subscribe({
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
