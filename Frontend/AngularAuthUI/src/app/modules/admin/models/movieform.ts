import { FormControl } from "@angular/forms"

export interface MovieForm {
    id: FormControl<number | null>,
    title: FormControl<string | null>,
    cast: FormControl<string[] | null>
    director: FormControl<string | null>,
    plot: FormControl<string | null>,
    genre: FormControl<string[] | null>,
    poster: FormControl<string | null>,
    releaseDate: FormControl<string | null>
    trailer: FormControl<string | null>
  }