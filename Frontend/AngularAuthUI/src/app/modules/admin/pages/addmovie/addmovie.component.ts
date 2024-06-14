import { Component, Inject, OnInit, inject } from '@angular/core';
import { NavbarComponent } from '../../../../common/layout/navbar/navbar.component';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatChipEditedEvent, MatChipInputEvent, MatChipsModule } from '@angular/material/chips';
import { MatAutocompleteSelectedEvent, MatAutocompleteModule } from '@angular/material/autocomplete';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { Movie } from '../../../../common/models/movie';
import { AdminService } from '../../service/admin.service';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MovieService } from '../../../../common/service/movie.service';
import { MovieForm } from '../../models/movieform';




@Component({
  selector: 'app-addmovie',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [NavbarComponent, MatFormFieldModule, MatInputModule, FormsModule, CommonModule, ReactiveFormsModule, MatCardModule, MatDatepickerModule, MatButtonModule, MatIconModule, MatChipsModule, MatAutocompleteModule, RouterLink],
  templateUrl: './addmovie.component.html',
  styleUrl: './addmovie.component.css'
})

export class AddmovieComponent implements OnInit {
  
  page: number = 1;
  limit: number = 12;

  announcer = inject(LiveAnnouncer);
  readonly separatorKeysCodes = [188] as const;
  editMode: boolean = false;
  addOnBlur: boolean = true;
  castList: string[]  = [];
  genreList: string[] = []
  allGenre: string[] = ['Action', 'Thriller','Crime', 'Comedy', 'Suspense', 'Biography', 'Horror', 'Drama', 'Musical', 'Mystery', 'Sci-fi', 'Fantasy', 'Romance', 'Adventure'];
  selectedGenres: string[] = []

  MovieForm!: FormGroup<MovieForm>

  constructor(
    private adminService: AdminService,
    private dialogRef: MatDialogRef<AddmovieComponent>,
    private movieService: MovieService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit():void {
    this.castList = this.data ? this.data.cast : []
    this.genreList = this.data ? this.data.genre : []
    this.editMode = this.data ? true : false

    this.MovieForm = new FormGroup<MovieForm>({
      id: new FormControl(this.data ? this.data.id : 0),
      title: new FormControl(this.data ? this.data.title : '', Validators.required),
      cast: new FormControl(this.data ? this.data.cast : [], Validators.required),
      director: new FormControl(this.data ? this.data.director : '', Validators.required),
      plot: new FormControl(this.data ? this.data.plot : '', Validators.required),
      genre: new FormControl(this.data ? this.data.genre : [], Validators.required),
      poster: new FormControl(this.data ? this.data.poster : '', Validators.required),
      releaseDate: new FormControl(this.data ? this.data.releaseDate : '', Validators.required),
      trailer: new FormControl(this.data ? this.data.trailer : '', Validators.required)
    });
  }


  public addCast(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    // Add cast
    if (value) {
      this.castList.push(value);
    }
    // Clear the input value
    event.chipInput!.clear();
  }

  public removeCast(actor: string): void {
    const index = this.castList.indexOf(actor);
    if (index >= 0) {
      this.castList.splice(index, 1);
      this.announcer.announce(`Removed ${actor}`);
    }
  }

  public editCast(actor: string, event: MatChipEditedEvent) {
    const value = event.value.trim();
    // Remove cast if it no longer has a name
    if (!value) {
      this.removeCast(actor);
      return;
    }
    // Edit existing cast
    const index = this.castList.indexOf(actor);
    if (index >= 0) {
      this.castList[index] = value;
    }
  }

  public addGenre(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();

    // Add genre
    if (value) {
      this.genreList.push(value);
    }

    // Clear the input value
    event.chipInput!.clear();
  }

  public removeGenre(g: string): void {
    const index = this.genreList.indexOf(g);

    if (index >= 0) {
      this.genreList.splice(index, 1);

      this.announcer.announce(`Removed ${g}`);
    }
  }

  public selected(event: MatAutocompleteSelectedEvent): void {
    this.genreList.push(event.option.viewValue);

  }

  public submit(): void {
    if (this.MovieForm.invalid) {
      this.MovieForm.markAllAsTouched();
      return;
    }

    const movie: Movie = {
      id: this.MovieCtrl.id.value,
      title: this.MovieCtrl.title.value,
      cast: this.MovieCtrl.cast.value,
      director: this.MovieCtrl.director.value,
      plot: this.MovieCtrl.plot.value,
      genre: this.MovieCtrl.genre.value,
      poster: this.MovieCtrl.poster.value,
      releaseDate: this.MovieCtrl.releaseDate.value,
      trailer: this.MovieCtrl.trailer.value
    }

    if (this.editMode) {
      this.adminService.editMovie(movie).subscribe({
        next: () => {
          this.dialogRef.close();
          this.getMovieList();
        },
        error: (err) => {
          console.log(err?.error.message)
        }
      });
    }
    else {
      this.adminService.addMovie(movie).subscribe({
        next: () => {
          this.dialogRef.close();
          this.getMovieList();
        },
        error: (err) => {
          console.log(err?.error.message)
        }
      });
    }
  }
  
  public closeDialog(): void{
    this.dialogRef.close();
  }

  public get MovieCtrl() { return this.MovieForm.controls }
  
  private getMovieList(): void{
    this.movieService.getMovies(this.page,this.limit,this.selectedGenres).subscribe(res => {
      if (res && res.data) {
        this.movieService.movieList.next(res.data.paginatedList)
      }
    });
  }
  
}
