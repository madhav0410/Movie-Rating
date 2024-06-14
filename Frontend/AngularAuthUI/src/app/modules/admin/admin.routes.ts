import { Routes } from "@angular/router";

export const adminRoute: Routes = [
    {
        path: '', redirectTo: 'dashboard', pathMatch: 'full'
    },
    {
        path: 'dashboard', loadComponent: () => import('./pages/dashboard/dashboard.component').then(x => x.DashboardComponent)
    },
    {
        path: 'add-movie', loadComponent: () => import('./pages/addmovie/addmovie.component').then(x => x.AddmovieComponent)
    },
    {
        path: 'movie', loadComponent: () => import('../../common/components/movie/movie.component').then(x => x.MovieComponent)
    },
    {
        path: '**', redirectTo:'dashboard', pathMatch: 'full'
    }
];