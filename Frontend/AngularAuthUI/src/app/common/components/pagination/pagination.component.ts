import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { MovieService } from '../../service/movie.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent{
  @Input() pageSize!: number;
  @Input() totalPages!: number;
  @Output() changedPage = new EventEmitter<number>();
  selectedGenres: string[] = []

  currentPage: number = this.movieService.currentPage.getValue();
 

  constructor(private movieService: MovieService) { }

 
  private goToPage(page: number): void {
    this.currentPage = page;
    this.changedPage.emit(this.currentPage);
  }

  public prevPage(): void {
    if (this.currentPage > 1) {
      this.goToPage(this.currentPage - 1);
    }
  }

  public nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.goToPage(this.currentPage + 1);
    }
  }
}


