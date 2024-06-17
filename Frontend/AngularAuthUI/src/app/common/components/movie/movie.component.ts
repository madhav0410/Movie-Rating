import { Component, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Movie } from '../../models/movie';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { MatCardModule } from '@angular/material/card';
import { MovieService } from '../../service/movie.service';
import { StarRatingComponent } from '../star-rating/star-rating.component';
import { UserService } from '../../../modules/user/service/user.service';
import { AuthService } from '../../../modules/account/service/auth.service';
import { MatIconModule } from '@angular/material/icon';
import { SafeUrlPipe } from '../../safe-url.pipe';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SignalrService } from '../../service/signalr.service';
import {MatInput} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';



@Component({
  selector: 'app-movie',
  standalone: true,
  imports: [NavbarComponent,MatCardModule,StarRatingComponent,MatIconModule,SafeUrlPipe,CommonModule,FormsModule,MatFormFieldModule,MatInput,MatButtonModule],
  templateUrl: './movie.component.html',
  styleUrl: './movie.component.css'
})

export class MovieComponent implements OnInit{
  movie!: Movie;
  userRating: number = 0;
  avgRating: number = 0;
  title: string = '';
  email: string = '';
  // comments: string[] = [];
  // newCommentText: string = '';
  isLoggedIn: boolean = this.authService.isLoggedIn();

  constructor(
    private authService: AuthService,
    private movieService: MovieService,
    private userService: UserService, 
    private route: ActivatedRoute,
    private signalRService: SignalrService
  ){}
   
  ngOnInit(): void {
    this.title = this.route.snapshot.queryParamMap.get('title') ?? '';
    this.email = this.authService.getEmailFromToken() ?? '';

    if(this.title){
      this.movieService.getMovieByTitle(this.title).subscribe({
        next: (res) => {
          this.movie = res.data;
        },
        error: (err) => {
          console.log(err?.error.message);        
        }
      })
      if(this.isLoggedIn){
        this.UserRating();
      }
      this.AvgRating();
      // this.signalRService.startConnection();
    //   this.signalRService.addReceiveCommentListener((title, comment) => {
    //   this.comments.push(comment); // Update comments array with received comment
    // });
    }

  }
  
  public onRatingChange(rating: number): void {
    if(this.title){
      this.userService.updateRating(this.email,this.title,rating).subscribe({
        next: ()=>{
          if(this.isLoggedIn){
            this.UserRating();
          }
          this.AvgRating();
        },
        error: (err)=>{
          console.log(err?.error.message); 
        }
      })
    }
  }

  // public  onSubmitComment() {
  //   if (this.newCommentText.trim()) {
  //     this.signalRService.invokeAddComment(this.title, this.email, this.newCommentText);
  //     this.newCommentText = ''; // Clear input after submission
  //   }
  // }

  private UserRating(): void{
    this.userService.getUserRating(this.email,this.title).subscribe({
      next: (res) => {
        this.userRating = res.data;
      },
      error: (err) => {
        console.log(err?.error.message);        
      }
    })
  }

  private AvgRating(): void{
    this.userService.getAvgRating(this.title).subscribe({
      next: (res) => {
        this.userService.avgRating.next(res.data);
        this.avgRating = res.data;
      },
      error: (err) => {
        console.log(err?.error.message);        
      }
    })
  }
   
}

