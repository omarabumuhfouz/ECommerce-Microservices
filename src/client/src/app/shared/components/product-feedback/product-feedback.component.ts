import { Component, ChangeDetectionStrategy, Input } from '@angular/core';
import { DatePipe } from '@angular/common';
import { StarRatingComponent } from '../star-rating/star-rating.component';
import { ProductReview } from '../../../core/models/product.model';

@Component({
  selector: 'app-product-feedback',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [DatePipe, StarRatingComponent],
  templateUrl: './product-feedback.component.html',
  styleUrl: './product-feedback.component.css'
})
export class ProductFeedbackComponent {
  @Input({ required: true }) reviews: ProductReview[] = [];
  @Input({ required: true }) averageRating: number = 0;
}
