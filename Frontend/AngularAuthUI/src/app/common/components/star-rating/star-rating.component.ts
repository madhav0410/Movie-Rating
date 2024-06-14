import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faStar } from '@fortawesome/free-solid-svg-icons';
import { UserService } from '../../../modules/user/service/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-star-rating',
  standalone: true,
  imports: [CommonModule,FontAwesomeModule,MatIconModule],
  templateUrl: './star-rating.component.html',
  styleUrl: './star-rating.component.css'
})
export class StarRatingComponent implements OnInit, OnChanges {
  
  @Input() starCount: number = 10;
  @Input() selectedStar: number = 0;
  @Input() isLoggedIn!:boolean;
  @Output() ratingUpdated = new EventEmitter<number>();

  previousSelection: number = 0;
  stars: any[] = [];
  
  constructor(private toastr: ToastrService) { }

  ngOnChanges(changes: SimpleChanges): void {
    this.previousSelection = this.selectedStar
  }

  ngOnInit(): void {
    this.stars = Array(this.starCount).fill(0);
  }

  public handleMouseEnter(index: number): void{
      this.selectedStar = index + 1;
  }

  public handleMouseLeave(): void{
    this.selectedStar = this.previousSelection 
  }
  
  public rating(index: number): void{
    if(!this.isLoggedIn){
      this.selectedStar = 0;
      this.toastr.error("Please Login First")
    }else{
      this.selectedStar = index + 1;
      this.ratingUpdated.emit(this.selectedStar);
      this.previousSelection = this.selectedStar;
    }
   
  }
}
