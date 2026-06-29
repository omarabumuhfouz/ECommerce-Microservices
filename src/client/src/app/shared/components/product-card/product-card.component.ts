import { Component, input, output, ChangeDetectionStrategy, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CurrencyPipe } from '@angular/common';
import { Product } from '../../../core/models/product.model';
import { CartService } from '../../../core/services/cart.service';
import { StarRatingComponent } from '../star-rating/star-rating.component';

@Component({
  selector: 'app-product-card',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [RouterLink, CurrencyPipe, StarRatingComponent],
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.css',
})
export class ProductCardComponent {
  product = input.required<Product>();
  viewMode = input<'grid' | 'list'>('grid');
  addedToCart = output<Product>();

  private cartService = inject(CartService);

  get inCart() { return this.cartService.isInCart(this.product().id); }
  get discountPercent() { return this.product().discount ?? 0; }

  onAddToCart(e: Event) {
    e.preventDefault();
    e.stopPropagation();
    this.cartService.addItem(this.product());
    this.addedToCart.emit(this.product());
  }
}
