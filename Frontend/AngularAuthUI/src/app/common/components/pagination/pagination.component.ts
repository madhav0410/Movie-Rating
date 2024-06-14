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
export class PaginationComponent implements OnChanges{
  @Input() pageSize!: number;
  @Input() totalPages!: number;
  @Output() changedPage = new EventEmitter<number>();
  selectedGenres: string[] = []

  currentPage: number = 1;
 

  constructor(private movieService: MovieService) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes?.['totalPages']) {
      this.currentPage = 1; // Reset to first page when page size changes
    }
  }

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


