import { Component, input, ChangeDetectionStrategy } from '@angular/core';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-star-rating',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="stars" [attr.aria-label]="rating() + ' out of 5 stars'">
      @for (star of starsArray; track star) {
        <span class="star" [class.full]="star <= fullStars" [class.half]="star === halfStar">★</span>
      }
      @if (showCount()) {
        <span class="count">({{ count() | number }})</span>
      }
    </div>
  `,
  styles: [`
    .stars { display: inline-flex; align-items: center; gap: 1px; }
    .star { font-size: var(--star-size, 14px); color: #374151; line-height: 1; }
    .star.full, .star.half { color: #F59E0B; }
    .count { font-size: 0.78em; color: var(--text-secondary); margin-left: 4px; }
  `],
  imports: [DecimalPipe],
})
export class StarRatingComponent {
  rating = input<number>(0);
  count = input<number>(0);
  showCount = input<boolean>(false);

  starsArray = [1, 2, 3, 4, 5];
  get fullStars(): number { return Math.floor(this.rating()); }
  get halfStar(): number | null {
    return this.rating() % 1 >= 0.5 ? Math.ceil(this.rating()) : null;
  }
}
