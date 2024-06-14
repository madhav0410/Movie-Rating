export interface Movie {
    id: number | null
    title: string | null,
    cast: string[] | null,
    director: string | null,
    plot: string | null,
    genre: string[] | null,
    poster: string | null,
    releaseDate: string | null,
    trailer: string | null
}

export interface PaginatedMovies {
    totalPages: number
    paginatedList: Movie []
}