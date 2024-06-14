import { Routes } from "@angular/router";

export const userRoute: Routes = [
    {
        path: '', redirectTo: 'dashboard', pathMatch: 'full'
    },
    {
        path: 'dashboard', loadComponent: () => import('./pages/dashboard/dashboard.component').then(x => x.DashboardComponent)
    },
    {
        path: 'movie', loadComponent: () => import('../../common/components/movie/movie.component').then(x => x.MovieComponent)
    },
    {
        path: '**', redirectTo:'dashboard', pathMatch: 'full'
    }
];